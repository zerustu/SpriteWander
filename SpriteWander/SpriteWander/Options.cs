using CommandLine;

namespace SpriteWander
{
    class Options
    {
        [Option('t', "tickFreq", Default = 60, HelpText = "The tick frequency in updates per second.")]
        public int TickFrequency { get; set; }

        [Option('f', "folder", Default = "./", HelpText = "path to the folder with the entities.")]
        public string Folder { get; set; }

        [Option('a', "alpha", Default = 0.6, HelpText = "Opacity of the entites on the screen.")]
        public double Alpha { get; set; }
    }
}
