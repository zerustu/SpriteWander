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
            XmlSerializer serializer = new XmlSerializer(typeof(textures.MultiTexture));
            parser.ParseArguments<Options>(args).WithParsed<Options>(o =>
            {
                _options = o;
                List<textures.MultiTexture> entries = new List<textures.MultiTexture>();
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
                            entries.Add(AnimsData);
                        }
                    }
                }
                Console.WriteLine($"{entries.Count} entité lu");
                foreach (textures.MultiTexture texture in entries)
                {
                    Console.WriteLine($"ShadowSize: {texture.ShadowSize}");

                    foreach (Anim anim in texture.Anims)
                    {
                        Console.WriteLine($"Name: {anim.Name}");
                        Console.WriteLine($"Index: {anim.Index}");
                        Console.WriteLine($"FrameWidth: {anim.Width}");
                        Console.WriteLine($"FrameHeight: {anim.Height}");
                        Console.WriteLine($"RushFrame: {anim.RushFrame}");
                        Console.WriteLine($"HitFrame: {anim.HitFrame}");
                        Console.WriteLine($"ReturnFrame: {anim.ReturnFrame}");

                        Console.WriteLine("Durations:");
                        foreach (int duration in anim.Durations)
                        {
                            Console.WriteLine(duration);
                        }

                        Console.WriteLine();
                    }

                }
                /*
                    _options = o;
                    AllEntities = JsonConvert.DeserializeObject<Dictionary<string, EntityInfo>>(File.ReadAllText(o.EList));
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Park = new DrawPark();
                    Application.Run(Park); */
            });
        }
    }
}
