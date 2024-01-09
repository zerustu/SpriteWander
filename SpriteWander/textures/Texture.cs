using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Reflection.Metadata;

namespace SpriteWander.textures
{
    public abstract class Texture : IDisposable
    {
        /** <summary>disposable class, is the texture readyy to be use and loaded in GPU</summary> **/
        public bool ready = false;

        public string? path;
        public string? name;

        /** <summary>return the correct animation to run if some animation is not available or have other name</summary>*/
        public abstract Animation Normalise(Animation animation, out AnimEvent reset);
        /** <summary> return a rectangle of the animation on the sprite to display (unit is pixel)</summary>
         * <param name="reset">Anim event use to signalise that the animation reach the end. \n
         * if Time > duration of the animation, should render the last valid frame and return a repeat or end anim event</param>
         * <param name="Height">full height of the sprite sheet of this animation</param>
           <param name="Width">full wigth of the sprite sheet of this animation</param>*/
        public abstract Rectangle Getcoord(Animation anim, Direction Dir, int Time, out int Width, out int Height, out AnimEvent reset);

        /** <summary>select the correct animation sheet to be display if animation are on different sprite sheet</summary>
         */
        public abstract void Use(Animation n);

        protected static int GenerateHandle()
        {
            return GL.GenTexture();
        }

        protected static void Use(int Handle, TextureUnit textureUnit)
        {
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        protected static void LoadFile(Stream steam, int Handle, TextureUnit textureUnit, out int fullWidth, out int fullHeight)
        {
            Bitmap bitmap = new(steam);
            fullWidth = bitmap.Width;
            fullHeight = bitmap.Height;
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            var pixels = new byte[4 * fullWidth * fullHeight];
            int index = 0;
            BitmapData? data = null;
            try
            {
                data = bitmap.LockBits(new Rectangle(0, 0, fullWidth, fullHeight), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                unsafe
                {
                    var ptr = (byte*)data.Scan0;
                    int remain = data.Stride - data.Width * 4;
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; j++)
                        {
                            pixels[index++] = ptr[2];
                            pixels[index++] = ptr[1];
                            pixels[index++] = ptr[0];
                            pixels[index++] = ptr[3];
                            ptr += 4;
                        }
                        ptr += remain;
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(data);
            }

            Use(Handle, textureUnit);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, fullWidth, fullHeight, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Dispose(int Handle)
        {
            GL.DeleteTexture(Handle);
            ready = false;
        }
        public abstract void Load();
        public abstract void Print();
        public abstract void Dispose();
        public abstract int Length(Animation animation);
        public AnimEvent EndBehaviour(Animation animation) => animation switch
        {
            Animation.Walk or
            Animation.Idle or
            Animation.Sleep or
            Animation.Eventsleep or
            Animation.Eat or
            Animation.Float or
            Animation.Laying => AnimEvent.Reset,
            _ => AnimEvent.End,
        };

        public static Animation ToId(string name) => name.ToLower() switch
        {
            "walk" => Animation.Walk,
            "attack" => Animation.Attack,
            "attack1" => Animation.Attack1,
            "attack2" => Animation.Attack2,
            "attack3" => Animation.Attack3,
            "sleep" => Animation.Sleep,
            "hurt" => Animation.Hurt,
            "idle" => Animation.Idle,
            "swing" => Animation.Swing,
            "double" => Animation.Double,
            "hop" => Animation.Hop,
            "charge" => Animation.Charge,
            "rotate" => Animation.Rotate,
            "eventsleep" => Animation.Eventsleep,
            "wake" => Animation.Wake,
            "eat" => Animation.Eat,
            "tumble" => Animation.Tumble,
            "pose" => Animation.Pose,
            "pull" => Animation.Pull,
            "pain" => Animation.Pain,
            "float" => Animation.Float,
            "deepbreath" => Animation.DeepBreath,
            "nod" => Animation.Nod,
            "sit" => Animation.Sit,
            "lookup" => Animation.LookUp,
            "sink" => Animation.Sink,
            "trip" => Animation.Trip,
            "laying" => Animation.Laying,
            "leapforth" => Animation.LeapForth,
            "head" => Animation.Head,
            "cringe" => Animation.Cringe,
            "lostbalance" => Animation.LostBalance,
            "tumbleback" => Animation.TumbleBack,
            "faint" => Animation.Faint,
            "hitground" => Animation.HitGround,
            "standingup" => Animation.StandingUp,
            "bump" => Animation.Bump,
            _ => Animation.Default,
        };

        public (int, int) DirToVect(Direction dir) => dir switch
        {
            Direction.DownRight => ( 1, -1 ),
            Direction.Right => ( 1, 0 ),
            Direction.UpRight => ( 1, 1 ),
            Direction.Up => ( 0, 1 ),
            Direction.UpLeft => ( -1, 1 ),
            Direction.Left => ( -1, 0 ),
            Direction.DownLeft => ( -1, -1 ),
            _ => ( 0, -1 ),
        };

        protected static bool isActivated(Animation anim)
        {
            switch (anim)
            {
                case Animation.Walk:
                case Animation.Sleep:
                case Animation.Hurt:
                case Animation.Idle:
                case Animation.Eventsleep:
                case Animation.Wake:
                case Animation.Laying:
                case Animation.TumbleBack:
                case Animation.Default:
                case Animation.Eat:
                    return true;
                default:
                    return false;
            }
            return false;
        }
    }

    public enum Animation
    {
        Walk,
        Attack,
        Attack1,
        Attack2,
        Attack3,
        Sleep,
        Hurt,
        Idle,
        Swing,
        Double,
        Hop,
        Charge,
        Rotate,
        Eventsleep,
        Wake,
        Eat,
        Tumble,
        Pose,
        Pull,
        Pain,
        Float,
        DeepBreath,
        Nod,
        Sit,
        LookUp,
        Sink,
        Trip,
        Laying,
        LeapForth,
        Head,
        Cringe,
        LostBalance,
        TumbleBack,
        Faint,
        HitGround,
        StandingUp,
        Default,
        Bump
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

    /** <summary>discribe what action to take regarding the currently playing animatin</summary> */
    public enum AnimEvent
    {
        /// <summary>
        /// keep playing the current animation
        /// </summary>
        Nothing,
        /// <summary>
        /// keep playing the current animation but reset the animation timer to loop again (change the animation if played to many time)
        /// </summary>
        Reset,
        /// <summary>
        /// stop playing that animation and change to the next one
        /// </summary>
        End
    }
}
