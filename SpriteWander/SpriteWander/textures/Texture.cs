using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteWander.textures
{
    public abstract class Texture : IDisposable
    {
        public abstract Animation Normalise(Animation animation, out AnimEvent reset);
        public abstract Rectangle Getcoord(Animation Anim, Direction Dir, int Time, out AnimEvent reset);
        public abstract void Use(Animation n);
        public abstract void Dispose();
    }

    public enum Animation
    {
        Walk = 0,
        Attack = 1,
        Attack1 = 2,
        Attack2 = 3,
        Attack3 = 4,
        Sleep = 5,
        Hurt = 6,
        Idle = 7,
        Swing = 8,
        Double = 9,
        Hop = 10,
        Charge = 11,
        Rotate = 12,
        Eventsleep = 13,
        Wake = 14,
        Eat = 15,
        Tumble = 16,
        Pose = 17,
        Pull = 18,
        Pain = 19,
        Float = 20,
        DeepBreath = 21,
        Nod = 22,
        Sit = 23,
        LookUp = 24,
        Sink = 25,
        Trip = 26,
        Laying = 27,
        LeapForth = 28,
        Head = 29,
        Cringe = 30,
        LostBalance = 31,
        TumbleBack = 32,
        Faint = 33,
        HitGround = 34,
        Default = 35,
        Bump = 36
    }

    public enum Direction
    {
        Down = 0,
        DownRight = 1,
        Right = 2,
        UpRight = 3,
        Up = 4,
        UpLeft = 5,
        Left = 6,
        DownLeft = 7
    }

    public enum AnimEvent
    {
        Nothing,
        Reset,
        End
    }
}
