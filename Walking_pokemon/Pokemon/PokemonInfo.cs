namespace Walking_pokemon.Pokemon
{
    public class PokemonInfo
    {
        public string animPath;
        public string imagePath;
        public float scale;

        public PokemonInfo(string animPath, string imagePath, float scale)
        {
            this.animPath = animPath;
            this.imagePath = imagePath;
            this.scale = scale;
        }
    }
}
