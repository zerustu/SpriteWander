namespace SpriteWander.Entity
{
    public class Animation
    {
        public Dictionary<string, List<Frame>> animation;

        public Animation(Dictionary<string, List<Frame>> animation)
        {
            this.animation = animation;
        }

        public List<Frame> GetAnimList(int state, int substate)
        {
            return state switch
            {
                1 => substate switch
                {
                    1 => Get("walkdown"),
                    2 => Get("walkleft"),
                    3 => Get("walkup"),
                    _ => Get("walkright"),
                },
                2 => Get("sleep"),
                _ => Get("stand"),
            };
        }

        private List<Frame> Get(string key)
        {
            if (animation.TryGetValue(key, out List<Frame> result)) return result;
            else return animation["stand"];
        }

        public int GetMax()
        {
            int res = 0;
            foreach (var item in animation)
            {
                foreach (Frame frame in item.Value)
                {
                    res = Math.Max(res, frame.height);
                    res = Math.Max(res, frame.width);
                }
            }
            return res;
        }
    }

    public class Frame
    {
        public int length;
        public int x;
        public int y;
        public int width;
        public int height;

        public Frame(int length, int x, int y, int width, int height)
        {
            this.length = length;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public Rectangle Rect { get => new(x, y, width, height); }
    }
}
