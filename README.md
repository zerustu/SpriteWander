# SpriteWander

SpriteWander is an application developed in C# that allows users to create an overlay on their screen where they can draw and animate sprites. It provides a customizable and interactive experience, allowing users to add various entities like Pokémon, pets, and more to their screen.

## Getting Started

### Installation

1. Clone the repository: `git clone https://github.com/zerustu/SpriteWander.git`
2. Open the project in your preferred C# development environment (e.g., Visual Studio, Visual Studio Code).
3. Build the project to generate the executable file (e.g., `SpriteWander.exe`).

### Usage

- Launch the application by double-clicking the executable file.

You can also run the application with command-line options:

```SpriteWander.exe [--tickFreq <ticks-per-second>] [--alpha <opacity>] [--list <entity-list-path>]```

- `--tickFreq` or `-t` (optional): Specifies the number of ticks per second for calculating entity positions and states (default: 50).
- `--alpha` or `-a` (optional): Sets the opacity of the overlay (default: 0.6).
- `--list` or `-l` (optional): Specifies the path to the JSON file containing the entity list to load (default: "./EntityList.json").

For the application to run, you need to have a valide Entity list json file, either at the same place as the execution file, or use the -l argument to point to one.
you can check the branch 'PokemonPack' for a exemple of Entity List and entities files. copy the content of this branch in the same folder as your execution file.

### Entity List JSON File

The entity list JSON file contains a dictionary where the keys are the names of the entities, and the values contain three fields:

- `animPath`: Path to a JSON file defining the entity's animations.
- `imagePath`: Path to a PNG file containing all the sprites for the entity.
- `scale`: A double value representing the scaling factor for the entity's size.

The animation JSON file consists of a dictionary with animation names as keys and a list of frames as values. Each frame is defined by five properties:

- `length`: The duration of the frame in seconds.
- `x` and `y`: The coordinates of the top-left corner of the area to be drawn from the image for this frame.
- `width` and `height`: The width and height of the area to be drawn for this frame.

For more details and examples, refer to the `PokemonPack` branch in this repository.

## Contributing

Contributions are welcome! If you have any ideas, suggestions, or bug reports, please open an issue or submit a pull request.


## License

This project is licensed under the [MIT License](LICENSE).
