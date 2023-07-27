namespace SpriteWander.entity
{
    internal class QuadLeaf : QuadTree
    {
        protected SortedSet<Entity.Entity> Elementes;

        public QuadLeaf(int max, float maxX, float minX, float maxY, float minY, QuadTree? root = null) : base(max, maxX, minX, maxY, minY, root)
        {
            Elementes = new SortedSet<Entity.Entity>();
        }

        public override QuadTree Add(Entity.Entity e)
        {
            Elementes.Add(e);
            Ocupation++;
            if (Ocupation > Max)
            {
                var NewRoot = root == this ? null : root;
                var res = new QuadBranch(Max, MaxX, MinX, MaxY, MinY, NewRoot);
                return res.AddList(Elementes.ToList());
            }
            else return this;
        }

        public override SortedSet<Entity.Entity> Gets(Predicate<Entity.Entity> predicate)
        {
            return new SortedSet<Entity.Entity>(Elementes.ToList().FindAll(predicate));
        }

        public override Entity.Entity? Get(Predicate<Entity.Entity> predicate)
        {
            return Elementes.ToList().Find(predicate);
        }

        public override QuadTree Remove(Entity.Entity e)
        {
            bool res = Elementes.Remove(e);
            if (!res) { throw new InvalidOperationException(); }
            Ocupation--;
            return this;
        }

        public override (QuadTree, int) RemoveAllRec(Predicate<Entity.Entity> predicate)
        {
            int res = Elementes.RemoveWhere(predicate);
            Ocupation -= res;
            return (this, res);
        }

        public override QuadTree RemoveAll(Predicate<Entity.Entity> predicate)
        {
            _ = RemoveAllRec(predicate);
            return this;
        }

        public override void Run(Action<Entity.Entity> f)
        {
            foreach (Entity.Entity ent in Elementes) f(ent);
        }

        public override QuadTree update()
        {
            List<Entity.Entity> ToRemove = new List<Entity.Entity>();
            foreach (Entity.Entity e in Elementes)
            {
                float X = Math.Clamp(e.X, MinX, MaxX);
                float Y = Math.Clamp(e.Y, MinY, MaxY);
                if ((X != e.X) && (Y != e.Y))
                {
                    ToRemove.Add(e);
                    root.Add(e);
                }
            }
            foreach (Entity.Entity e in ToRemove)
            {
                Ocupation--;
                Elementes.Remove(e);
            }
            return this;
        }
    }
}
