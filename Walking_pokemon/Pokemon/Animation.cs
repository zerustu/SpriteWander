using System;
using System.Collections.Generic;
using System.Drawing;

namespace Walking_pokemon.Pokemon
{
    public class Animation
    {
        public Dictionary<string, List<frame>> animation;

        public Animation(Dictionary<string, List<frame>> animation)
        {
            this.animation = animation;
        }

        public List<frame> getAnimList(int state, int substate)
        {
            switch (state)
            {
                case 0:
                default:
                    return get("stand");
                case 1:
                    switch (substate)
                    {
                        case 0:
                        default:
                            return get("walkright");
                        case 1:
                            return get("walkdown");
                        case 2:
                            return get("walkleft");
                        case 3:
                            return get("walkup");
                    }
                case 2:
                    return get("sleep");
            }
        }

        private List<frame> get(string key)
        {
            List<frame> result;
            if (animation.TryGetValue(key, out result)) return result;
            else return animation["stand"];
        }
        
        public int getMax()
        {
            int res = 0;
            foreach (var item in animation)
            {
                foreach (frame frame in item.Value)
                {
                    res = Math.Max(res, frame.height);
                    res = Math.Max(res, frame.width);
                }
            }
            return res;
        }
    }

    public class frame
    {
        public int length;
        public int x;
        public int y;
        public int width;
        public int height;

        public frame(int length, int x, int y, int width, int height)
        {
            this.length = length;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public Rectangle Rect { get => new Rectangle(x, y, width, height); }
    }
}
