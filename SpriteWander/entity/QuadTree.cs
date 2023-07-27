namespace SpriteWander.entity
{
    public abstract class QuadTree
    {
        public int Ocupation;

        protected int Max;

        protected float MaxX;

        protected float MinX;

        protected float MaxY;

        protected float MinY;

        protected QuadTree root;

        protected QuadTree(int max, float maxX, float minX, float maxY, float minY, QuadTree? root = null)
        {
            Max = max;
            MaxX = maxX;
            MinX = minX;
            MaxY = maxY;
            MinY = minY;
            this.root = root ?? this;
        }

        public abstract QuadTree Add(Entity.Entity e);

        public QuadTree AddList(List<Entity.Entity> list)
        {
            QuadTree result = this;
            foreach (Entity.Entity e in list)
            {
                result = result.Add(e);
            }
            return result;
        }

        public abstract QuadTree Remove(Entity.Entity e);

        public abstract (QuadTree, int) RemoveAllRec(Predicate<Entity.Entity> predicate);

        public abstract QuadTree RemoveAll(Predicate<Entity.Entity> predicate);

        public abstract SortedSet<Entity.Entity> Gets(Predicate<Entity.Entity> predicate);

        public SortedSet<Entity.Entity> GetAll()
        {
            return Gets(_ => true);
        }

        public abstract Entity.Entity? Get(Predicate<Entity.Entity> predicate);

        public abstract void Run(Action<Entity.Entity> f);

        public abstract QuadTree update();
    }
}
