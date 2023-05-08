using Newtonsoft.Json;
using OpenTK.Graphics.OpenGL4;

namespace Walking_pokemon.Pokemon
{
    public class Pokemon
    {
        // Position limits
        private DrawPark Park;
        protected float MIN_X { get => Park.Left; }
        protected float MAX_X { get => Park.Right; }
        protected float MIN_Y { get => Park.Top; }
        protected float MAX_Y { get => Park.Bottom; }

        //protected float MIN_X = 0;
        //protected float MAX_X = 1;
        //protected float MIN_Y = 0;
        //protected float MAX_Y = 1;


        protected int maxSize = 30;

        //position
        protected float x = 20;
        protected float y = 20;

        public float X
        {
            get
            {
                return x;
            }
            set
            {
                if (value < MIN_X)
                {
                    x = MIN_X;
                    timer = 0;
                }
                else if (value > MAX_X)
                {
                    x = MAX_X;
                    timer = 0;
                }
                else
                {
                    x = value;
                }
            }
        }

        public float Y
        {
            get
            {
                return y;
            }
            set
            {
                if (value < MIN_Y)
                {
                    y = MIN_Y;
                    timer = 0;
                }
                else if (value > MAX_Y)
                {
                    y = MAX_Y;
                    timer = 0;
                }
                else
                {
                    y = value;
                }
            }
        }

        private float[] Center
        {
            get => new float[]{ (X - MIN_X) / (MAX_X - MIN_X), (Y - MIN_Y) / (MAX_Y - MIN_Y)};
        }

        public float[] Pos
        {
            get
            {
                Rectangle draw = SpriteRect;
                float[] center = Center;
                x = Center[0];
                y = Center[1];
                float[] vertices = {
                    x * 2f - 1f - 0.1f,
                    y * 2f - 1f - 0.1f,
                    (float)draw.X / (float)Texture.Width,
                    (float)draw.Y / (float)Texture.Height,
                    x * 2f - 1f + 0.1f,
                    y * 2f - 1f - 0.1f  ,
                    ((float)draw.X + (float)draw.Width) / (float)Texture.Width,
                    (float)draw.Y / (float)Texture.Height,
                    x * 2f - 1f - 0.1f,
                    y * 2f - 1f + 0.1f,
                    (float)draw.X / (float)Texture.Width,
                    ((float)draw.Y + (float)draw.Height) / (float)Texture.Height,
                    x * 2f - 1f + 0.1f,
                    y * 2f - 1f + 0.1f,
                    ((float)draw.X + (float)draw.Width) / (float)Texture.Width,
                    ((float)draw.Y + (float)draw.Height) / (float)Texture.Height
                };
                return vertices;
            }
        }
        uint[] indices = {  // note that we start from 0!
            0, 1, 2,   // first triangle
            1, 2, 3    // second triangle
        };
        public Texture Texture;
        int VertexBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;


        //animations variable
        protected Animation animations;
        protected double animTimer = 0;
        protected string JsonAnimationPath;
        protected int textureWidth;
        protected int textureHeight;
        public Rectangle SpriteRect
        {
            get
            {
                List<frame> animFrames = animations.getAnimList(state, subState);
                while (true)
                {
                    int tFrame = 0;
                    foreach (frame item in animFrames)
                    {
                        tFrame += item.length;
                        if (tFrame > animTimer)
                        {
                            oldRect = item.Rect;
                            return item.Rect;
                        }
                    }
                    animTimer -= tFrame;
                }
            }
        }

        public Rectangle oldRect;

        //Pokemon state
        public int state = 0;
        public double timer = 0;
        protected int subState = 0;
        protected float speed = 0;

        protected float scale = 0;

        public float Scale { get => scale; }

        protected Random rng;



        public Pokemon(PokemonInfo info, DrawPark Park, Texture texture, int textureWidth, int textureHeight, int program, float scale = -1)
        {
            this.Park = Park;
            rng = new Random();
            if (scale < 0) this.scale = (float)(rng.NextDouble() + 2.5);
            else this.scale = scale;
            X = 0.5f;
            Y = 0.5f;
            this.scale *= info.scale;

            JsonAnimationPath = info.animPath;
            string jsonAnim = System.IO.File.ReadAllText(JsonAnimationPath);
            animations = JsonConvert.DeserializeObject<Animation>(jsonAnim);
            Texture = texture;
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;
            //maxSize = animations.getMax();
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, Pos.Length * sizeof(float), Pos, BufferUsageHint.StreamDraw);

            Park.shader.Use();
            int CoordLocation = GL.GetAttribLocation(Park.shader.Handle, "aPosition");
            GL.EnableVertexAttribArray(CoordLocation);
            GL.VertexAttribPointer(CoordLocation, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            int textLocation = GL.GetAttribLocation(Park.shader.Handle, "aTexCoord");
            GL.EnableVertexAttribArray(textLocation);
            GL.VertexAttribPointer(textLocation, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        }


        public void Tick(double time)
        {
            timer -= time;
            animTimer += time;
            if (timer < 0) timer = 0;
            switch (state)
            {
                case 0: //idle
                    if (timer == 0) //start runing
                    {
                        if (rng.Next(10) < 2)
                        {
                            state = 2;
                            timer = rng.Next(50, 300);
                        }
                        else
                        {
                            subState = rng.Next(4);
                            timer = rng.Next(10, 300);
                            state = 1;
                            speed = (float)rng.NextDouble() + 3;
                            animTimer = 0;
                        }
                    }
                    break;
                case 1:// walk
                    if (timer == 0) //why are you running
                    {
                        state = 0;
                        timer = rng.Next(10, 100);
                        animTimer = 0;
                    }
                    else
                    {
                        switch (subState) // move
                        {
                            case 0:
                                X += speed * (float)time;
                                break;
                            case 1:
                                Y += speed * (float)time;
                                break;
                            case 2:
                                X -= speed * (float)time;
                                break;
                            case 3:
                                Y -= speed * (float)time;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case 2: // sleep
                    if (timer == 0) //stop sleeping
                    {
                        state = 0;
                        timer = rng.Next(10, 100);
                        animTimer = 0;
                    }
                    break;
                default:
                    state = 0;
                    timer = 100;
                    break;
            }
        }

        public void Bind()
        {
            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            Texture.Use();
        }

        public void Render()
        {
            GL.BufferData(BufferTarget.ArrayBuffer, Pos.Length * sizeof(float), Pos, BufferUsageHint.StreamDraw);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteBuffer(ElementBufferObject);
            GL.DeleteBuffer(VertexArrayObject);
        }
    }
}
