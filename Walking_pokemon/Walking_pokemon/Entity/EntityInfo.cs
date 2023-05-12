namespace Walking_pokemon.Entity
{
    public class EntityInfo
    {
        public string animPath;
        public string imagePath;
        public float scale;

        public EntityInfo(string animPath, string imagePath, float scale)
        {
            this.animPath = animPath;
            this.imagePath = imagePath;
            this.scale = scale;
        }
    }
}
