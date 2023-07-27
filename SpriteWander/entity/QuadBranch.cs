namespace SpriteWander.entity
{
    internal class QuadBranch : QuadTree
    {
        protected List<QuadTree> _children;
        protected QuadTree TopLeft { get => _children[0]; set => _children[0] = value; }
        protected QuadTree TopRight { get => _children[1]; set => _children[1] = value; }
        protected QuadTree BotLeft { get => _children[2]; set => _children[2] = value; }
        protected QuadTree BotRight { get => _children[3]; set => _children[3] = value; }

        protected float MidX { get => (MinX + MaxX) / 2; }
        protected float MidY { get => (MinY + MaxY) / 2; }

        public QuadBranch(int max, float maxX, float minX, float maxY, float minY, QuadTree? root = null) : base(max, maxX, minX, maxY, minY, root)
        {
            _children = new List<QuadTree>
            {
                new QuadLeaf(max, MidX, minX, maxY, MidY, this.root),
                new QuadLeaf(max, maxX, MidX, maxY, MidY, this.root),
                new QuadLeaf(max, MidX, minX, MidY, minY, this.root),
                new QuadLeaf(max, maxX, MidX, MidY, minY, this.root)
            };
        }

        public override QuadTree Add(Entity.Entity e)
        {
            switch (e.X <= MidX, e.Y <= MidY)
            {
                case (true,true):
                    BotLeft = BotLeft.Add(e);
                    break;
                case (true,false):
                    TopLeft = TopLeft.Add(e);
                    break;
                case (false,true):
                    BotRight = BotRight.Add(e);
                    break;
                case (false,false):
                    TopRight = TopRight.Add(e);
                    break;
            }
            Ocupation++;
            return this;
        }

        public override Entity.Entity? Get(Predicate<Entity.Entity> predicate)
        {
            var res = TopLeft.Get(predicate);
            res ??= TopRight.Get(predicate);
            res ??= BotLeft.Get(predicate);
            res ??= BotRight.Get(predicate);
            return res;
        }

        public override SortedSet<Entity.Entity> Gets(Predicate<Entity.Entity> predicate)
        {
            var res = new SortedSet<Entity.Entity>();
            foreach (var e in _children)
            {
                res.UnionWith(e.Gets(predicate));
            }
            return res;
        }

        protected QuadTree fuse()
        {
            var Leaf = new QuadLeaf(Max, MaxX, MinX, MaxY, MinY, root);
            return Leaf.AddList(GetAll().ToList());
        }

        public override QuadTree Remove(Entity.Entity e)
        {
            switch (e.X <= MidX, e.Y <= MidY)
            {
                case (true, true):
                    BotLeft = BotLeft.Remove(e);
                    break;
                case (true, false):
                    TopLeft = TopLeft.Remove(e);
                    break;
                case (false, true):
                    BotRight = BotRight.Remove(e);
                    break;
                case (false, false):
                    TopRight = TopRight.Remove(e);
                    break;
            }
            Ocupation--;
            return (Ocupation <= Max) ? fuse() : this;
        }

        public override QuadTree RemoveAll(Predicate<Entity.Entity> predicate)
        {
            var (res, _) = RemoveAllRec(predicate);
            return res;
        }

        public override void Run(Action<Entity.Entity> f)
        {
            foreach (var e in _children)
            {
                e.Run(f);
            }
        }

        public override (QuadTree, int) RemoveAllRec(Predicate<Entity.Entity> predicate)
        {
            int res = 0;
            for (int i = 0; i < 4; i++)
            {
                int resi = 0;
                (_children[i], resi) = _children[i].RemoveAllRec(predicate);
                res += resi;
            }
            Ocupation -= res;
            var ResTree = (Ocupation <= Max) ? fuse() : this;
            return (ResTree, res);
        }

        public override QuadTree update()
        {
            for (var i  = 0; i < 4; ++i)
            {
                _children[i] = _children[i].update();
            }
            Ocupation = 0;
            for (int i = 0; i < 4; i++)
            {
                Ocupation += _children[i].Ocupation;
            }
            return (Ocupation <= Max) ? fuse() : this;
        }
    }
}
