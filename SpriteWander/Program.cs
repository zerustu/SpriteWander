using CommandLine;
using System.IO.Compression;
using System.Xml.Serialization;
using SpriteWander.textures;
using System.Diagnostics;
using SpriteWander.Entity;
using System.Runtime.InteropServices;
using System;
using System.Web;

namespace SpriteWander
{
    static class Program
    {

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        public static Dictionary<string, Texture> entries;

        public static DrawPark Park = null!;

        public static Options _options = null!;

        static void configuration(ParserSettings with) 
        {
            with.EnableDashDash = true;
            with.HelpWriter = null;
            with.AutoHelp = true;
            with.IgnoreUnknownArguments = false;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                // Double-clicked to start the application
                // Create a console window and hide it
                AllocConsole();
                ShowWindow(GetConsoleWindow(), SW_HIDE);
            }
            var parser = new Parser(configuration);
            var result = parser.ParseArguments<Options>(args);
            switch (result.Tag)
            {
                case ParserResultType.Parsed:
                    StartWithParsedArgs(result.Value);
                    break;
                case ParserResultType.NotParsed:
                    var HT = CommandLine.Text.HelpText.AutoBuild(result);
                    MessageBox.Show(HT.ToString());
                    break;
            }

        }
        static void StartWithParsedArgs(Options o)
        {
            _options = o;
            if (_options.NoConsole)
            {
                // Create a console window and hide it
                AllocConsole();
                ShowWindow(GetConsoleWindow(), SW_HIDE);
            }

            LoadData(_options.Folder);

            if (entries.Count == 0)
            {
                MessageBox.Show("You need to import a sprite first!", "Warning", MessageBoxButtons.OK);
                Application.Exit();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Park = new DrawPark(_options.Scale, _options.Scale, !_options.NotTopmost);
                Task.Run(ReadConsole);
                Application.Run(Park);
            }
        }

        public static async void ReadConsole()
        {
            string v;
            while (true)
            {
                v = Console.ReadLine();
                switch (v)
                {
                    case "exit":
                        Park.Exit(); 
                        break;
                    default:
                        break;
                }
            }
        }

        public static void LoadData(string path)
        {

            entries = new();

            XmlSerializer serializer = new(typeof(MultiTexture));
            string[] fichiersZip = Directory.GetFiles(path, "*.zip");
            foreach (string fichier in fichiersZip)
            {
                ZipArchive archive = ZipFile.OpenRead(fichier);
                ZipArchiveEntry? animDataEntry = archive.GetEntry("AnimData.xml");
                if (animDataEntry != null)
                {
                    Stream stream = animDataEntry.Open();
                    MultiTexture? AnimsData = (MultiTexture?)serializer.Deserialize(stream);
                    if (AnimsData != null)
                    {
                        AnimsData.path = fichier;
                        AnimsData.name = Path.GetFileNameWithoutExtension(fichier);
                        entries.Add(AnimsData.name, AnimsData);
                    }
                }
                archive.Dispose();
            }
        }
    }
}
