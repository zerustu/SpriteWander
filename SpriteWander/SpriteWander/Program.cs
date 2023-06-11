using Newtonsoft.Json;
using SpriteWander.Entity;
using CommandLine;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Serialization;
using SpriteWander.textures;

namespace SpriteWander
{
    static class Program
    {

        public static Dictionary<string, textures.Texture> entries = new Dictionary<string, textures.Texture>();

        public static DrawPark? Park;

        public static Options? _options;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var parser = new Parser(with => with.EnableDashDash = true); 
            XmlSerializer serializer = new XmlSerializer(typeof(textures.MultiTexture));
            parser.ParseArguments<Options>(args).WithParsed<Options>(o =>
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
                        textures.MultiTexture? AnimsData = (textures.MultiTexture?)serializer.Deserialize(stream);
                        if (AnimsData != null)
                        {
                            AnimsData.path = fichier;
                            AnimsData.name = fichier.Substring(fichier.LastIndexOf('/') + 1, fichier.Length - 5 - fichier.LastIndexOf('/'));
                            entries.Add(AnimsData.name, AnimsData);
                        }
                    }
                    archive.Dispose();
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Park = new DrawPark(30 , 30);
                Application.Run(Park); 
            });
        }
    }
}
