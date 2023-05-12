using Newtonsoft.Json;
using OpenTK.Graphics.OpenGL4;

namespace Walking_pokemon.Entity
{
    public class Entity
    {
        // Position limits
        private readonly DrawPark Park;
        protected  float MIN_X { get => Park.Left; }
        protected float MAX_X { get => Park.Right; }
        protected float MIN_Y { get => Park.Top; }
        protected float MAX_Y { get => Park.Bottom; }


        protected readonly int maxSize = 30;
        float HalfSize { get => maxSize / 2 * Scale; }

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
                if (value < MIN_X + HalfSize)
                {
                    x = MIN_X + HalfSize;
                    timer = 0;
                }
                else if (value > MAX_X - HalfSize)
                {
                    x = MAX_X - HalfSize;
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
                if (value < MIN_Y + HalfSize)
                {
                    y = MIN_Y + HalfSize;
                    timer = 0;
                }
                else if (value > MAX_Y - HalfSize)
                {
                    y = MAX_Y - HalfSize;
                    timer = 0;
                }
                else
                {
                    y = value;
                }
            }
        }

        private float[] GetCenter()
        {
            return new float[] { (X - MIN_X) / (MAX_X - MIN_X), (Y - MIN_Y) / (MAX_Y - MIN_Y) };
        }

        public float[] GetPos()
        {
            Rectangle draw = GetSpriteRect();
            float[] center = GetCenter();
            float cx = center[0];
            float cy = center[1];
            float[] vertices = {
                    cx * 2f - 1f - draw.Width * Scale / Park.Width, //bas gauche
                    cy * 2f - 1f - draw.Height * Scale / Park.Height,
                    ((float)draw.X) / (float)Texture.Width,
                    1-((float)draw.Y + draw.Height) / (float)Texture.Height,
                    cx * 2f - 1f + draw.Width * Scale / Park.Width, // bas droite
                    cy * 2f - 1f - draw.Height * Scale / Park.Height,
                    ((float)draw.X + draw.Width) / (float)Texture.Width,
                    1-((float)draw.Y + draw.Height) / (float)Texture.Height,
                    cx * 2f - 1f - draw.Width * Scale / Park.Width, // haut gauche
                    cy * 2f - 1f + draw.Height * Scale / Park.Height,
                    ((float)draw.X) / (float)Texture.Width,
                    1-((float)draw.Y) / (float)Texture.Height,
                    cx * 2f - 1f + draw.Width * Scale / Park.Width, //hautdroite
                    cy * 2f - 1f + draw.Height * Scale / Park.Height,
                    ((float)draw.X + (float)draw.Width) / (float)Texture.Width,
                    1-((float)draw.Y) / (float)Texture.Height
                };
            return vertices;
        }

        private static readonly uint[] INDICES = {  // note that we start from 0!
            0, 1, 2,   // first triangle
            1, 2, 3    // second triangle
        };
        public readonly Texture Texture;
        readonly int VertexBufferObject;
        readonly int VertexArrayObject;
        readonly int ElementBufferObject;


        //animations variable
        protected readonly Animation animations;
        protected double animTimer = 0;

        public Rectangle GetSpriteRect()
        {
            List<Frame> animFrames = animations.GetAnimList(state, subState);
            while (true)
            {
                int tFrame = 0;
                foreach (Frame item in animFrames)
                {
                    tFrame += item.length;
                    if (tFrame > animTimer)
                    {
                        return item.Rect;
                    }
                }
                animTimer -= tFrame;
            }
        }


        //Pokemon state
        public int state = 0;
        public double timer = 0;
        protected int subState = 0;
        protected float speed = 0;

        protected readonly float scale = 0;

        public float Scale => scale;

        protected static readonly Random rng = new();



        public Entity(EntityInfo info, DrawPark Park, Texture texture, float scale = -1)
        {
            this.Park = Park;
            this.scale = scale < 0 ? (float)(rng.NextDouble() + 2.5) : scale;
            X = MAX_X / 2;
            Y = MAX_Y / 2;
            this.scale *= info.scale;

            animations = JsonConvert.DeserializeObject<Animation>(System.IO.File.ReadAllText(info.animPath));
            Texture = texture;
            maxSize = animations.GetMax();
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, GetPos().Length * sizeof(float), GetPos(), BufferUsageHint.StreamDraw);

            Park.shader.Use();
            int CoordLocation = GL.GetAttribLocation(Park.shader.Handle, "aPosition");
            GL.EnableVertexAttribArray(CoordLocation);
            GL.VertexAttribPointer(CoordLocation, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            int textLocation = GL.GetAttribLocation(Park.shader.Handle, "aTexCoord");
            GL.EnableVertexAttribArray(textLocation);
            GL.VertexAttribPointer(textLocation, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

            GL.BufferData(BufferTarget.ElementArrayBuffer, INDICES.Length * sizeof(uint), INDICES, BufferUsageHint.StaticDraw);
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
                            case 0: //left
                                X += speed * (float)time;
                                break;
                            case 1: //down
                                Y -= speed * (float)time;
                                break;
                            case 2: //right
                                X -= speed * (float)time;
                                break;
                            case 3: //up
                                Y += speed * (float)time;
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
            GL.BufferData(BufferTarget.ArrayBuffer, GetPos().Length * sizeof(float), GetPos(), BufferUsageHint.StreamDraw);
            GL.DrawElements(PrimitiveType.Triangles, INDICES.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteBuffer(ElementBufferObject);
            GL.DeleteBuffer(VertexArrayObject);
        }
    }
}
