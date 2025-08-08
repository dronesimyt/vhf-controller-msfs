using Microsoft.FlightSimulator.SimConnect;
using System.Runtime.InteropServices;      // for COMException
using System.Globalization;

namespace VHF_Controller
{

    public partial class MainForm : Form
    {
        private string frequencyBuffer = "";
        private SimConnect? simConnect = null;
        private const int WM_USER_SIMCONNECT = 0x0402;
        private bool advPrefilled = false;
        private bool isConnected = false;
        private int activeVhf = 1;
        private readonly Color ActiveColor = Color.FromArgb(70, 130, 180);  // steel blue
        private readonly Color InactiveColor = SystemColors.Control;



        private enum DEFINITIONS
        {
            INI_COM1_STBY_FREQUENCY
        }

        private enum EVENT_ID
        {
            COM1_SET_HZ,    // our identifier for the “tune COM1” event
            COM2_SET_HZ     // our identifier for the “tune COM2” event
        }

        // alongside your existing DEFINITIONS and EVENT_ID enums
        private enum GROUP_ID
        {
            GROUP0   // corresponds to “notification group 0”
        }



        public MainForm()
        {
            InitializeComponent();
            InitTimer();

            this.AcceptButton = btnEnter;

            btnVhf1.Click += (_, __) => SetActiveVhf(1);
            btnVhf2.Click += (_, __) => SetActiveVhf(2);

            // set the default active radio
            SetActiveVhf(activeVhf);

            // Prevent manual resizing
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;   // disable the Maximize button
            this.MinimizeBox = true;    // leave Minimize enabled if you want

            // Optional: absolutely lock the client size
            this.MaximumSize = this.MinimumSize = this.Size;

            this.KeyPreview = true;
            this.KeyDown += MainForm_KeyDown;


            // Start in a disconnected state until ConnectToSim() runs:
            isConnected = false;
            btnReconnect.Visible = false;      // hide until we know we’re disconnected
            UpdateConnectionStatusUI();

            ConnectToSim();                    // attempt initial connection
            btnReset_Click(null, null);        // set up your panel UI
        }

        private void InitTimer()
        {
            feedbackTimer.Tick += (s, e) =>
            {
                feedbackTimer.Stop();
                lblFeedback.Visible = false;
                btnEnter.Text = "ADV";
                btnReset_Click(null, null);  // reset UI to initial state
            };
        }

        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == WM_USER_SIMCONNECT)
            {
                try
                {
                    simConnect?.ReceiveMessage();
                }
                catch (COMException)
                {
                    // Log or inspect ex.ErrorCode if you want,
                    // then cleanly disconnect and update UI:
                    HandleSimDisconnect();
                }
            }
            else
            {
                base.DefWndProc(ref m);
            }
        }

        private void HandleSimDisconnect()
        {
            try { simConnect?.Dispose(); } catch { }
            simConnect = null;
            isConnected = false;

            btnEnter.Enabled = false;
            btnReset.Enabled = false;
            btnReconnect.Visible = true;

            UpdateConnectionStatusUI();   // ← update the label here
        }

        private void ConnectToSim()
        {
            // Attempt to tear down any existing connection first
            try
            {
                simConnect?.Dispose();
            }
            catch { /* ignore */ }

            try
            {
                // 1) Create the SimConnect session
                simConnect = new SimConnect(
                    "Radio Frequency Controller",
                    this.Handle,
                    WM_USER_SIMCONNECT,
                    null,
                    0
                );

                // 2) Wire up SimConnect shutdown/exception callbacks
                simConnect.OnRecvException += (s, a) =>
                {
                    BeginInvoke(() =>
                    {
                        MessageBox.Show($"SimConnect error {a.dwException}", "SimConnect Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        HandleSimDisconnect();
                    });
                };
                simConnect.OnRecvQuit += (s, a) =>
                {
                    BeginInvoke(() =>
                    {
                        MessageBox.Show("MSFS has quit.", "SimConnect Disconnected",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        HandleSimDisconnect();
                    });
                };

                // 3) Map and register your event & data definitions
                // COM1
                simConnect.MapClientEventToSimEvent(EVENT_ID.COM1_SET_HZ, "COM_RADIO_SET_HZ");
                simConnect.AddClientEventToNotificationGroup(GROUP_ID.GROUP0, EVENT_ID.COM1_SET_HZ, false);
                // COM2
                simConnect.MapClientEventToSimEvent(EVENT_ID.COM2_SET_HZ, "COM2_RADIO_SET_HZ");
                simConnect.AddClientEventToNotificationGroup(GROUP_ID.GROUP0, EVENT_ID.COM2_SET_HZ, false);

                simConnect.AddToDataDefinition(
                    DEFINITIONS.INI_COM1_STBY_FREQUENCY,
                    "COM ACTIVE FREQUENCY:1",
                    "MHz",
                    SIMCONNECT_DATATYPE.FLOAT64,
                    0, 0
                );
                simConnect.RegisterDataDefineStruct<double>(DEFINITIONS.INI_COM1_STBY_FREQUENCY);


                simConnect.SetNotificationGroupPriority(
                    GROUP_ID.GROUP0,
                    SimConnect.SIMCONNECT_GROUP_PRIORITY_HIGHEST
                );

                isConnected = true;
                UpdateConnectionStatusUI();
                btnReconnect.Visible = false;
            }
            catch (COMException)
            {
                // 5) On failure, clean up and inform the user
                simConnect = null;
                isConnected = false;
                UpdateConnectionStatusUI();
                btnReconnect.Visible = true;

                MessageBox.Show(
                    "Failed to connect to MSFS. Make sure the simulator is running and in a flight.",
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        private void UpdateConnectionStatusUI()
        {
            if (isConnected)
            {
                lblStatus.Text = "Connected";
                lblStatus.ForeColor = Color.Green;
            }
            else
            {
                lblStatus.Text = "Disconnected";
                lblStatus.ForeColor = Color.Red;
            }
        }


        private void SetActiveVhf(int vhf)
        {
            activeVhf = vhf;

            UpdateVhfToggleUI();
        }

        private void UpdateVhfToggleUI()
        {
            // VHF1 active?
            btnVhf1.BackColor = (activeVhf == 1) ? ActiveColor : InactiveColor;
            btnVhf1.ForeColor = (activeVhf == 1) ? Color.White : Color.Black;

            // VHF2 active?
            btnVhf2.BackColor = (activeVhf == 2) ? ActiveColor : InactiveColor;
            btnVhf2.ForeColor = (activeVhf == 2) ? Color.White : Color.Black;

        }

        // 2) Handle numpad digits, Backspace → Reset, Enter → Set/ADV
        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            // NUMPAD 0–9 → btn0…btn9
            if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
            {
                int num = e.KeyCode - Keys.NumPad0;               // 0…9
                var btn = Controls.Find("btn" + num, true)       // find your button
                                .FirstOrDefault() as Button;
                if (btn != null && btn.Enabled)
                    btn.PerformClick();                          // simulate click
                e.Handled = true;
                return;
            }

            // BACKSPACE → Reset
            if (e.KeyCode == Keys.Back)
            {
                btnReset.PerformClick();
                e.Handled = true;
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                btnEnter.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;   // prevent the focused button from also seeing Enter
            }

            if (e.KeyCode == Keys.V)
            {
                // flip activeVhf (1↔2) and update UI
                SetActiveVhf(activeVhf == 1 ? 2 : 1);
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            if (e.KeyCode == Keys.U)
            {
                // Trigger exactly the same code path as clicking ADV
                // only prefill if we haven't already and nothing typed
                if (frequencyBuffer.Length == 0 && !advPrefilled)
                {
                    btnEnter.PerformClick();
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }
        }

        private void btnReconnect_Click(object sender, EventArgs e)
        {
            // Try again
            ConnectToSim();

            if (isConnected)
            {
                // only if we succeeded do we reset the panel
                btnReset_Click(null, null);
            }
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {

            if (frequencyBuffer.Length == 0 && !advPrefilled)
            {
                // Prefill 122800 → "122.800" in UI
                frequencyBuffer = "122800";
                advPrefilled = true;

                UpdateDisplay();       // shows "122.800"
                UpdateButtonStates();  // re-enable buttons as if user typed it

                btnEnter.Text = "SET";
                return;  // exit before the normal “send to sim” logic
            }

            // 2) Build & validate the 3-digit prefix
            string prefix3 = frequencyBuffer
                                .Substring(0, Math.Min(3, frequencyBuffer.Length))
                                .PadRight(3, '0');
            int prefixVal = int.Parse(prefix3);
            if (prefixVal < 118 || prefixVal > 136)
            {
                MessageBox.Show("Invalid prefix. It must be between 118 and 136 (e.g. 12 → 120).");
                return;
            }

            // 3) Build & pad the fraction
            string fraction = frequencyBuffer.Length > 3
                                ? frequencyBuffer.Substring(3)
                                : "";
            fraction = fraction.PadRight(3, '0');

            // 4) Combine and parse
            string fullBuffer = prefix3 + fraction;       // e.g. "122"+"800" => "122800"
            string formatted = fullBuffer.Insert(3, "."); // "122.800"
            if (!double.TryParse(formatted, NumberStyles.AllowDecimalPoint,
                                 CultureInfo.InvariantCulture,
                                 out double freqMhz))
            {
                MessageBox.Show("Error parsing frequency.");
                return;
            }

            // 5) Send to MSFS via SimConnect
            decimal freqDec = decimal.Parse(formatted, CultureInfo.InvariantCulture);
            uint freqHz = (uint)(freqDec * 1_000_000m);
            if (simConnect != null)
            {

                var evt = (activeVhf == 1)
                    ? EVENT_ID.COM1_SET_HZ
                    : EVENT_ID.COM2_SET_HZ;

                try
                {
                    simConnect.TransmitClientEvent(
                        SimConnect.SIMCONNECT_OBJECT_ID_USER,
                        evt,
                        freqHz,
                        GROUP_ID.GROUP0,
                        SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY
                    );

                }
                catch (COMException)
                {
                    //MessageBox.Show("Error: could not send frequency to MSFS. Please reconnect and try again.",
                    //                "SimConnect Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblFeedback.Text = "UNABLE";
                    lblFeedback.ForeColor = Color.Red;
                    HandleSimDisconnect();
                    return;
                }

                // Immediately reset the panel to its initial state:
                // Show the green feedback (“CHECK”) for 1 second:
                lblFeedback.Text = "CHECK";
                lblFeedback.Visible = true;
                feedbackTimer.Start();

                return;
            }
            else
            {
                MessageBox.Show("Not connected to MSFS.");
                return;
            }

        }

        private void btnReset_Click(object? sender, EventArgs? e)
        {
            frequencyBuffer = "";  // Clear the input completely
            advPrefilled = false;
            lblFrequencyDisplay.Text = "___.___";  // Show placeholder underscores
            btnEnter.Text = "ADV";
            UpdateButtonStates(); // Reset buttons
            btnEnter.Focus();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            btnEnter.Text = "SET";
            Button? btn = sender as Button;
            if (btn == null || frequencyBuffer.Length >= 6) return; // Ensure max 6 digits

            string newBuffer = frequencyBuffer + btn.Text;

            if (frequencyBuffer.Length == 0 && btn.Text != "1")
            {
                MessageBox.Show("Frequency must start with '1' (e.g., 118-136).");
                return;
            }

            frequencyBuffer = newBuffer;
            UpdateDisplay();
            UpdateButtonStates();
            btnEnter.Focus();
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            Button? btn = sender as Button;
            if (btn == null || frequencyBuffer.Length >= 6) return; // Ensure max 6 digits

            string newBuffer = frequencyBuffer + btn.Text;

            if (frequencyBuffer.Length == 0 && btn.Text != "1")
            {
                MessageBox.Show("Frequency must start with '1' (e.g., 118-136).");
                return;
            }

            frequencyBuffer = newBuffer;
            UpdateDisplay();
            UpdateButtonStates();
            btnEnter.Focus();
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            Button? btn = sender as Button;
            if (btn == null || frequencyBuffer.Length >= 6) return; // Ensure max 6 digits

            string newBuffer = frequencyBuffer + btn.Text;

            if (frequencyBuffer.Length == 0 && btn.Text != "1")
            {
                MessageBox.Show("Frequency must start with '1' (e.g., 118-136).");
                return;
            }

            frequencyBuffer = newBuffer;
            UpdateDisplay();
            UpdateButtonStates();
            btnEnter.Focus();
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            Button? btn = sender as Button;
            if (btn == null || frequencyBuffer.Length >= 6) return; // Ensure max 6 digits

            string newBuffer = frequencyBuffer + btn.Text;

            if (frequencyBuffer.Length == 0 && btn.Text != "1")
            {
                MessageBox.Show("Frequency must start with '1' (e.g., 118-136).");
                return;
            }

            frequencyBuffer = newBuffer;
            UpdateDisplay();
            UpdateButtonStates();
            btnEnter.Focus();
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            Button? btn = sender as Button;
            if (btn == null || frequencyBuffer.Length >= 6) return; // Ensure max 6 digits

            string newBuffer = frequencyBuffer + btn.Text;

            if (frequencyBuffer.Length == 0 && btn.Text != "1")
            {
                MessageBox.Show("Frequency must start with '1' (e.g., 118-136).");
                return;
            }

            frequencyBuffer = newBuffer;
            UpdateDisplay();
            UpdateButtonStates();
            btnEnter.Focus();
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            Button? btn = sender as Button;
            if (btn == null || frequencyBuffer.Length >= 6) return; // Ensure max 6 digits

            string newBuffer = frequencyBuffer + btn.Text;

            if (frequencyBuffer.Length == 0 && btn.Text != "1")
            {
                MessageBox.Show("Frequency must start with '1' (e.g., 118-136).");
                return;
            }

            frequencyBuffer = newBuffer;
            UpdateDisplay();
            UpdateButtonStates();
            btnEnter.Focus();
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            Button? btn = sender as Button;
            if (btn == null || frequencyBuffer.Length >= 6) return; // Ensure max 6 digits

            string newBuffer = frequencyBuffer + btn.Text;

            if (frequencyBuffer.Length == 0 && btn.Text != "1")
            {
                MessageBox.Show("Frequency must start with '1' (e.g., 118-136).");
                return;
            }
            frequencyBuffer = newBuffer;
            UpdateDisplay();
            UpdateButtonStates();
            btnEnter.Focus();

        }

        private void btn8_Click(object sender, EventArgs e)
        {
            Button? btn = sender as Button;
            if (btn == null || frequencyBuffer.Length >= 6) return; // Ensure max 6 digits

            string newBuffer = frequencyBuffer + btn.Text;

            if (frequencyBuffer.Length == 0 && btn.Text != "1")
            {
                MessageBox.Show("Frequency must start with '1' (e.g., 118-136).");
                return;
            }

            frequencyBuffer = newBuffer;
            UpdateDisplay();
            UpdateButtonStates();
            btnEnter.Focus();
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            Button? btn = sender as Button;
            if (btn == null || frequencyBuffer.Length >= 6) return; // Ensure max 6 digits

            string newBuffer = frequencyBuffer + btn.Text;

            if (frequencyBuffer.Length == 0 && btn.Text != "1")
            {
                MessageBox.Show("Frequency must start with '1' (e.g., 118-136).");
                return;
            }

            frequencyBuffer = newBuffer;
            UpdateDisplay();
            UpdateButtonStates();
            btnEnter.Focus();

        }

        private void btn0_Click(object sender, EventArgs e)
        {
            Button? btn = sender as Button;
            if (btn == null || frequencyBuffer.Length >= 6) return; // Ensure max 6 digits

            string newBuffer = frequencyBuffer + btn.Text;

            if (frequencyBuffer.Length == 0 && btn.Text != "1")
            {
                MessageBox.Show("Frequency must start with '1' (e.g., 118-136).");
                return;
            }

            frequencyBuffer = newBuffer;
            UpdateDisplay();
            UpdateButtonStates();
            btnEnter.Focus();
        }

        private void UpdateDisplay()
        {
            if (string.IsNullOrEmpty(frequencyBuffer))
            {
                lblFrequencyDisplay.Text = "___.___";  // Show underscores if empty
                return;
            }

            // Ensure at least 3 digits before the decimal
            string formattedFrequency = frequencyBuffer.PadRight(6, '_');

            // Insert the decimal point automatically after the first 3 digits
            string displayFrequency = formattedFrequency.Insert(3, ".");

            lblFrequencyDisplay.Text = displayFrequency;
        }


        private void UpdateButtonStates()
        {
            // Disable all buttons first
            btn0.Enabled = false;
            btn1.Enabled = false;
            btn2.Enabled = false;
            btn3.Enabled = false;
            btn4.Enabled = false;
            btn5.Enabled = false;
            btn6.Enabled = false;
            btn7.Enabled = false;
            btn8.Enabled = false;
            btn9.Enabled = false;

            if (frequencyBuffer.Length == 0)
            {
                // First digit must be '1'
                btn1.Enabled = true;
            }
            else if (frequencyBuffer.Length == 1)
            {
                // Second digit can only be '2' or '3'
                if (frequencyBuffer == "1")
                {
                    btn1.Enabled = true;
                    btn2.Enabled = true;
                    btn3.Enabled = true;
                }
            }
            else if (frequencyBuffer.Length == 2)
            {
                // Third digit depends on the second digit
                char secondDigit = frequencyBuffer[1];

                if (secondDigit == '1')
                {
                    // Allow 8-9 for "12x"
                    btn8.Enabled = true;
                    btn9.Enabled = true;
                }
                else if (secondDigit == '2')
                {
                    // Allow 0-9 for "12x"
                    btn0.Enabled = true;
                    btn1.Enabled = true;
                    btn2.Enabled = true;
                    btn3.Enabled = true;
                    btn4.Enabled = true;
                    btn5.Enabled = true;
                    btn6.Enabled = true;
                    btn7.Enabled = true;
                    btn8.Enabled = true;
                    btn9.Enabled = true;

                }
                else if (secondDigit == '3')
                {
                    // Allow only 0-6 for "13x"
                    btn0.Enabled = true;
                    btn1.Enabled = true;
                    btn2.Enabled = true;
                    btn3.Enabled = true;
                    btn4.Enabled = true;
                    btn5.Enabled = true;
                    btn6.Enabled = true;
                }
            }
            // can be deleted i guess or length >=3
            else if (frequencyBuffer.Length == 3)
            {
                char thirdDigit = frequencyBuffer[2];
                if (!string.IsNullOrEmpty(thirdDigit.ToString()))
                {
                    btn0.Enabled = true;
                    btn1.Enabled = true;
                    btn2.Enabled = true;
                    btn3.Enabled = true;
                    btn4.Enabled = true;
                    btn5.Enabled = true;
                    btn6.Enabled = true;
                    btn7.Enabled = true;
                    btn8.Enabled = true;
                    btn9.Enabled = true;
                }

            }
            else if (frequencyBuffer.Length == 4)
            {
                char fourthDigit = frequencyBuffer[3];
                if (!string.IsNullOrEmpty(fourthDigit.ToString()))
                {
                    btn0.Enabled = true;
                    btn1.Enabled = true;
                    btn2.Enabled = true;
                    btn3.Enabled = true;
                    btn4.Enabled = true;
                    btn5.Enabled = true;
                    btn6.Enabled = true;
                    btn7.Enabled = true;
                    btn8.Enabled = true;
                    btn9.Enabled = true;
                }
            }
            else if (frequencyBuffer.Length == 5)
            {
                char fifthDigit = frequencyBuffer[4];
                if (!string.IsNullOrEmpty(fifthDigit.ToString()))
                {
                    btn0.Enabled = true;
                    btn1.Enabled = false;
                    btn2.Enabled = false;
                    btn3.Enabled = false;
                    btn4.Enabled = false;
                    btn5.Enabled = true;
                    btn6.Enabled = false;
                    btn7.Enabled = false;
                    btn8.Enabled = false;
                    btn9.Enabled = false;
                }
            }
            else
            {
                // do nothing
            }
        }

        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkAlwaysOnTop.Checked;
        }

        private void btnVhf1_Click(object sender, EventArgs e)
        {
            btnEnter.Focus();
        }

        private void btnVhf2_Click(object sender, EventArgs e)
        {
            btnEnter.Focus();
        }
    }
}
