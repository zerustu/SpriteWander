# SpriteWander

SpriteWander is an application developed in C# that allows users to create an overlay on their screen where they can draw and animate sprites. It provides a customizable and interactive experience, allowing users to add various entities like Pokémon, pets, and more to their screen.

## Getting Started

### Installation

1. Clone the repository: `git clone https://github.com/zerustu/SpriteWander.git`
2. Open the project in your preferred C# development environment (e.g., Visual Studio, Visual Studio Code).
3. Build the project to generate the executable file (e.g., `SpriteWander.exe`).

Alternatively, an already built Windows executable can be found in the "executable" folder.

### Usage

- Launch the application by double-clicking the executable file.

You can also run the application with command-line options:

```SpriteWander.exe [--tickFreq <ticks-per-second>] [--alpha <opacity>] [--folder <entity-list-folder>] [--scale <int>]```

- `--tickFreq` or `-t` (optional): Specifies the number of ticks per second for calculating entity positions and states (default: 50).
- `--alpha` or `-a` (optional): Sets the opacity of the overlay (default: 0.6).
- `--folder` or `-f` (optional): Specifies the path to the folder with all entities to load (default: `./`).
- `--scale` or `-s` (optional): Specifies the approximate pixel size of an entity (default: 40).

In the entity folder (`./` by default), place all the entities you want to be able to load.

The expected format is the one used by the PMD Sprite Collab project ([Home page](https://sprites.pmdcollab.org), [GitHub project](https://github.com/PMDCollab/SpriteCollab), [Discord](https://discord.com/invite/skytemple)). Drop a Pokémon sprite zip archive in the entity folder, rename it (as the name of the archive is used as the name of the entity), and they should be loaded by the program (restart required).

The tool isn't restricted to Pokémon sprites, but this format is used for reading the files.

## Known issues

When starting the app, the main screen will become white. Switching window focus (by pressing Alt + Tab or clicking anywhere on your screen) will fix this issue.

If, at some point, the sprites are no longer on the topmost layer (go behind other windows), you can click on the taskbar and then back to the app, and it should fix the issue. This applies to Windows; I'm not sure about other operating systems.

## Contributing

Contributions are welcome! If you have any ideas, suggestions, or bug reports, please open an issue or submit a pull request.

You can reuse this project, modify it as you want.

## License

This project is licensed under the [MIT License](LICENSE).
