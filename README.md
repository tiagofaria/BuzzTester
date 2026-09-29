# BuzzTester

BuzzTester is a standalone Unity application for testing PS3 wireless Buzz! controllers on Windows. Connect the receiver and see every button press in real time for up to four players.

## Quick start

1. Download the ZIP file from the [latest release](https://github.com/tiagofaria/BuzzTester/releases/latest).
2. Extract the entire ZIP file to a folder.
3. Connect the Wbuzz USB receiver to the computer.
4. Turn on the controllers that have batteries. You do not need to connect all four controllers.
5. Run `BuzzTester.exe`.
6. Check that the connection status is green and press the controller buttons.
7. Use **TEST LIGHT** to turn on that player's red light for 1.5 seconds.

## Features

- Detects the Sony Wbuzz receiver (`VID 054C / PID 1000`)
- Tests all five buttons for up to four players
- Displays the latest received HID report
- Tests each controller's red light individually
- Automatically reconnects when the receiver becomes available
- Portuguese and English interface
- Safely closes the HID connection when the application exits

## Requirements

- Windows 10 or Windows 11
- Sony wireless Buzz! receiver for PS3
- Unity is not required to run a release build

## Build from source

Open the repository in Unity `6000.3.22f1` and select **Buzz Tester > Build Windows**. The Windows build will be created locally in `Builds/Windows`.

## License

This project is distributed under the [GNU General Public License v3.0](LICENSE).
