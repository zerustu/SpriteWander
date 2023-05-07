using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Walking_pokemon.Pokemon;

namespace Walking_pokemon
{
    static class Program
    {

        public static Dictionary<string, PokemonInfo> pokedex = JsonConvert.DeserializeObject<Dictionary<string, PokemonInfo>>(System.IO.File.ReadAllText(@".\pokemons.json"));

        public static Pokepark Park;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Park = new Pokepark(800, 600, false);
            Park.Run();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new controls());
        }
    }
}
