﻿using CommandLine;
using System.IO.Compression;
using System.Xml.Serialization;
using SpriteWander.textures;
using System.Diagnostics;

namespace SpriteWander
{
    static class Program
    {

        public static Dictionary<string, Texture> entries = new();

        public static DrawPark Park = null!;

        public static Options _options = null!;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var parser = new Parser(with => with.EnableDashDash = true); 
            parser.ParseArguments<Options>(args).WithParsed(StartWithParsedArgs);

            if (entries.Count == 0)
            {
                MessageBox.Show("You need to import a sprite first!", "Warning", MessageBoxButtons.OK);
                Application.Exit();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Park = new DrawPark(_options.Scale, _options.Scale);
                Application.Run(Park);
            }
        }
        static void StartWithParsedArgs(Options o)
        {
            _options = o;
            XmlSerializer serializer = new(typeof(MultiTexture));

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
                        AnimsData.name = Path.GetFileNameWithoutExtension(fichier);
                        entries.Add(AnimsData.name, AnimsData);
                    }
                }
                archive.Dispose();
            }
        }
    }
}
