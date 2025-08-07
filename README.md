# VHF-Controller

A lightweight utility that lets you dial and set your COM1 Active frequency in-sim—without touching the cockpit knobs. It runs alongside MSFS via SimConnect and enforces real-world aviation rules (118–136 MHz, 8.33 kHz spacing) to prevent invalid entries.

Note: This application won’t win any design awards—functionality is more important.

---

## Key Features

- Numeric keypad UI (0–9, Reset, Advisory/Set)  
- Prefix validation: only 118–136 MHz COM1 channels allowed  
- Automatic decimal placement: just type the digits (e.g. `125350` → `125.350`)  
- Unicom/Advisory preset: one-click ADV button to prefill 122.800 MHz  
- Button-state feedback: disables invalid digits as you type  
- Transient confirmation: green “CHECK” message after each Set  
- SimConnect integration for rock-solid in-sim tuning  
- Framework-dependent .NET 8 (7 files total) — no installer required  

---

## Requirements

- .NET 8.0 Desktop Runtime  
- Microsoft Flight Simulator 2020 or 2024 (in-flight, running)  

---

## Installation

1. Download and unzip the package.  
2. Launch MSFS and load a flight.  
3. Double-click `VHF-Controller.exe`.  
4. If the application fails to start, install the .NET 8.0 Desktop Runtime:
   - The app will prompt and download automatically.  
   - Installation takes less than a minute.  
5. If Windows flags the app as unsafe, click More Info → Run anyway.  

---

## Usage

1. Dial your desired COM1 frequency via the on-screen buttons (or keyboard).  
2. Press Set to send it to the sim as Active COM1.  
3. Press RST (Reset) to clear back to `___.___`.  
4. Press ADV (Advisory) to pre-fill 122.800 MHz on a fresh panel—then press Set to apply.  
   Prevents accidental Unicom tuning by requiring an explicit Set.

---

## Compatibility

Tested with (but not limited to):  
- iniBuilds default aircraft (MSFS 2024)  
- Fenix-Family aircraft  
- PMDG aircraft
- GA aircraft

It basically works with every aircraft that has a VHF radio.

---

## Support

If you have questions or run into issues, I’m happy to help!
