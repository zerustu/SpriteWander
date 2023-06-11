using OpenTK.Graphics.OpenGL4;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;

namespace SpriteWander.Entity
{
    public class Texture
    {
        public int Handle;
        public int Width;
        public int Height;

        public Texture(string Path)
        {
            Bitmap bitmap = new(Path);
            Width = bitmap.Width;
            Height = bitmap.Height;
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            var pixels = new byte[4 * Width * Height];
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
            Handle = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
            this.Use();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public void Dispose()
        {
            GL.DeleteTexture(Handle);
            Handle = 0;
        }
    }
}
