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

        public bool ready = false;

        public string? path;
        public string? name;
        public abstract Animation Normalise(Animation animation, out AnimEvent reset);
        public abstract Rectangle Getcoord(Animation anim, Direction Dir, int Time, out int Width, out int Height, out AnimEvent reset);
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
        public AnimEvent EndBehaviour(Animation animation)
        {
            switch (animation)
            {
                case Animation.Walk:
                case Animation.Idle:
                case Animation.Sleep:
                case Animation.Eventsleep:
                case Animation.Eat:
                case Animation.Float:
                case Animation.Laying:
                    return AnimEvent.Reset;
                default:
                    return AnimEvent.End;
            }
        }
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
