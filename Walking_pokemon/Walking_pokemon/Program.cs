using Newtonsoft.Json;
using Walking_pokemon.Pokemon;

namespace Walking_pokemon
{
    static class Program
    {

        public static Dictionary<string, PokemonInfo> pokedex = JsonConvert.DeserializeObject<Dictionary<string, PokemonInfo>>(System.IO.File.ReadAllText(@".\pokemons.json"));

        public static DrawPark? Park;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Park = new DrawPark();
            Application.Run(Park);
        }
    }
}
