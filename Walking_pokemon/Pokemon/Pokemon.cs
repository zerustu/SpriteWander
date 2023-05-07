using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Newtonsoft.Json;

namespace Walking_pokemon.Pokemon
{
    public class Pokemon
    {
        // Position limits
        private Pokepark Park;
        protected static float MIN_X = 0;
        protected float MAX_X = 1;
        protected float MIN_Y = 0;
        protected float MAX_Y = 1;
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

        public float[] Pos
        { 
            get 
            {
                Rectangle draw = SpriteRect;
                float[] vertices = {
                    (X - 0.1f + MIN_X )/(MIN_X + MAX_X) * 2f - 1f,
                    (Y - 0.1f + MIN_Y )/(MIN_Y + MAX_Y) * 2f - 1f,
                    (X + 0.1f + MIN_X )/(MIN_X + MAX_X) * 2f - 1f,
                    (Y - 0.1f + MIN_Y )/(MIN_Y + MAX_Y) * 2f - 1f,
                    (X + 0.1f + MIN_X )/(MIN_X + MAX_X) * 2f - 1f,
                    (Y + 0.1f + MIN_Y )/(MIN_Y + MAX_Y) * 2f - 1f,
                    (X - 0.1f + MIN_X )/(MIN_X + MAX_X) * 2f - 1f,
                    (Y + 0.1f + MIN_Y )/(MIN_Y + MAX_Y) * 2f - 1f
                };
                return vertices;
            } 
        }
        public Point oldPos;

        //animations variable
        protected Animation animations;
        protected double animTimer = 0;
        protected string JsonAnimationPath;
        public Image Spritesheet;
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



        public Pokemon(PokemonInfo info, Pokepark Park, float scale = -1)
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
            Spritesheet = Image.FromFile(info.imagePath);
            //maxSize = animations.getMax();
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
                                X += speed*(float)time;
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
    }
}
