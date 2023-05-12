using Newtonsoft.Json;
using SpriteWander.Entity;
using CommandLine;

namespace SpriteWander
{
    static class Program
    {

        public static Dictionary<string, EntityInfo>? AllEntities;

        public static DrawPark? Park;

        public static Options? _options;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var parser = new Parser(with => with.EnableDashDash = true);
            parser.ParseArguments<Options>(args).WithParsed<Options>(o =>
            {
                _options = o;
                AllEntities = JsonConvert.DeserializeObject<Dictionary<string, EntityInfo>>(File.ReadAllText(o.EList));
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Park = new DrawPark();
                Application.Run(Park);
            });
        }
    }
}
