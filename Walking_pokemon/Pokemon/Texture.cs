using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;

namespace Walking_pokemon.Pokemon
{
    public class Texture
    {
        public int Handle;
        public int Width;
        public int Height;

        public Texture(string Path)
        {
            Bitmap bitmap = new Bitmap(Path);
            Width = bitmap.Width;
            Height = bitmap.Height;
            var pixels = new float[4 * Width * Height];
            int index = 0;
            BitmapData data = null;
            try
            {
                data = bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                unsafe
                {
                    var ptr = (byte*)data.Scan0;
                    int remain = data.Stride - data.Width * 4;
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; j++)
                        {
                            pixels[index++] = ptr[2] / 255f;
                            pixels[index++] = ptr[1] / 255f;
                            pixels[index++] = ptr[0] / 255f;
                            pixels[index++] = ptr[3] / 255f;
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
            GL.CreateTextures(TextureTarget.Texture2D, 1, out Handle);
            GL.TextureStorage2D(
                Handle,
                1,                           // levels of mipmapping
                SizedInternalFormat.Rgba32f, // format of texture
                Width,
                Height);
            this.Use();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba4, Width, Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Rgba, PixelType.Float, pixels);
        }

        public void Use()
        {
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}
