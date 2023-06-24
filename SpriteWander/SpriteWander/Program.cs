using Newtonsoft.Json;
using SpriteWander.Entity;
using CommandLine;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Serialization;
using SpriteWander.textures;
using System.Diagnostics;

namespace SpriteWander
{
    static class Program
    {

        public static Dictionary<string, Texture> entries = new();

        public static DrawPark? Park;

        public static Options? _options;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var parser = new Parser(with => with.EnableDashDash = true); 
            XmlSerializer serializer = new(typeof(MultiTexture));
            parser.ParseArguments<Options>(args).WithParsed(o =>
            {
                _options = o;
                string[] fichiersZip = Directory.GetFiles(_options.Folder, "*.zip");
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
                            AnimsData.name = fichier.Substring(fichier.LastIndexOf('/') + 1, fichier.Length - 5 - fichier.LastIndexOf('/'));
                            entries.Add(AnimsData.name, AnimsData);
                        }
                    }
                    archive.Dispose();
                }
                if (entries.Count == 0)
                {
                    MessageBox.Show("You need to import a sprite first!", "Warning", MessageBoxButtons.OK);
                }
                else
                { 
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Park = new DrawPark(30 , 30);
                    Application.Run(Park);
                }
            });
        }
    }
}
