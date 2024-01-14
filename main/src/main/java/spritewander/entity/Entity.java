package spritewander.entity;

import java.awt.Rectangle;
import java.util.Random;
import spritewander.utils.*;
import spritewander.park;
import spritewander.textures.*;

import com.jogamp.opengl.GL2;
import com.jogamp.opengl.GLAutoDrawable;

public class Entity {

    protected static Random rng = new Random();

    protected final park P;

    protected float MIN_X() { return P.borderMargin; }
    protected float MAX_X() { return P.max_X - P.borderMargin; }
    protected float MIN_Y() { return P.borderMargin; }
    protected float MAX_Y() { return P.max_X - P.borderMargin; }

    //position
    protected float x;
    public float getX() {
        return x;
    }
    public void setX(float x) {
        try {
            this.x = utils.Clamp(x, MIN_X(), MAX_X());
        } catch (Exception e) {
            this.x = MIN_X();
            System.err.println("setX got an error : " + e.getMessage());
            e.printStackTrace();
        }
        if (x != this.x) state = Animation.Bump;
    }

    protected float y;

    public float getY() {
        return y;
    }
    public void setY(float y) {
        try {
            this.y = utils.Clamp(y, MIN_Y(), MAX_Y());
        } catch (Exception e) {
            this.y = MIN_Y();
            System.err.println("setX got an error : " + e.getMessage());
            e.printStackTrace();
        }
        if (y != this.y) state = Animation.Bump;
    }
    protected float[] getCorner()
    {
        float cx = x/ P.max_X;
        float cy = y/P.max_Y;
        Rectangle draw = new Rectangle(1, 1);
        return new float[] {
            cx * 2f - 1f - Scale * draw.width / (P.max_X * 20), // bas gauche
            cy * 2f - 1f - Scale * draw.height / (P.max_Y * 20),

            cx * 2f - 1f + Scale * draw.width / (P.max_X * 20), // bas droite
            cy * 2f - 1f - Scale * draw.height / (P.max_Y * 20),

            cx * 2f - 1f - Scale * draw.width / (P.max_X * 20), // haut gauche
            cy * 2f - 1f + Scale * draw.height / (P.max_Y * 20),

            cx * 2f - 1f + Scale * draw.width / (P.max_X * 20), // haut droite
            cy * 2f - 1f + Scale * draw.height / (P.max_Y * 20),
        };
    }

    //animations variable
    protected int animTimer = 0;
    protected AnimEvent Event = AnimEvent.Nothing;


    //Pokemon state
    public Animation state = Animation.Default;
    public int cycle = 0;
    protected Direction subState = Direction.Up;

    protected float scale = 0.2f;

    public float Scale = scale;

    
    protected float RANDOM_X() { return (float) rng.nextDouble() * (MAX_X() - MIN_X()) + MIN_X(); }
    protected float RANDOM_Y() { return (float) rng.nextDouble() * (MAX_Y() - MIN_Y()) + MIN_Y(); }

    protected float WalkSTD() {return (MAX_X() - MIN_X()) / 4f;};
    protected float[] RANDOM_NORMAL_VECTOR()
        {
    
            float X, X1, Y, Y1;
            do
            {
                double R = Math.sqrt(-2 * Math.log(rng.nextDouble())) * WalkSTD();
                double T = 2 * Math.PI * rng.nextDouble();
                X = (float)(R * Math.cos(T)) + x;
                Y = (float)(R * Math.sin(T)) + y;
                try {
                    X1 = utils.Clamp(X, MIN_X(), MAX_X());
                    Y1 = utils.Clamp(Y, MIN_Y(), MAX_Y());
                } catch (Exception e) {
                    X1 = MIN_X();
                    Y1 = MIN_Y();
                    System.err.println("RANDOM_NORMAL_VECTOR clamp failled : " + e.getMessage());
                    e.printStackTrace();
                }
            } while (X != X1 || Y != Y1);
            return new float[]{X, Y};
    
        }
    
    protected float targetX, targetY;
    
    public Entity(park Park) {
        this.P = Park;
        this.x = this.RANDOM_X();
        this.y = this.RANDOM_Y();
    }

    protected void Move(float speed)
    {
        int[] vec = subState.ToVect();
        this.setX(getX() + vec[0] * speed);
        this.setY(getY() + vec[1] * speed);
    }

    protected void NextAnim()
        {
            animTimer = 0;
            double rngvalue = rng.nextDouble();
            switch (state)
            {
                case Sleep:
                case Eventsleep:
                case Laying:
                    state = Animation.Wake;
                    break;
                default:
                case Default:
                case Idle:
                    if (rngvalue < 0.5) // Set new target position to locate
                    {
                        state = Animation.Walk;
                        float[] RNV = RANDOM_NORMAL_VECTOR();
                        targetX = RNV[0];
                        targetY = RNV[1];
                    }
                    else if (rngvalue < 0/*.55*/) state = Animation.LeapForth;
                    else if (rngvalue < 0.90) state = Animation.Sleep;
                    else if (rngvalue < 0.95) state = Animation.Pose;
                    else state = Animation.Eat;
                    break;
                case Trip:
                case LostBalance:
                case Faint:
                case HitGround:
                    state = Animation.Laying;
                    break;
                case Walk:
                case Attack:
                case Attack1:
                case Attack2:
                case Attack3:
                case Double:
                case Hurt:
                case Bump:
                case Swing:
                case Hop:
                case Charge:
                case Rotate:
                case Eat:
                case Tumble:
                case TumbleBack:
                case Wake:
                case Pose:
                case Pull:
                case Pain:
                case Float:
                case DeepBreath:
                case Nod:
                case Sit:
                case LookUp:
                case Sink:
                case LeapForth:
                case Head:
                case Cringe:
                    state = Animation.Idle;
                    break;
            }
            //state = Texture.Normalise(state, out _);
            if (true)//Texture.EndBehaviour(state) == AnimEvent.Reset)
            {
                int maxCycle = 20;
                cycle = state == Animation.Walk ? 100 : rng.nextInt(maxCycle-4) + 4;
            }
        }
    
    

    protected void TurnRandom(double p)
    {
        p /= 0.7;
        double rngvalue = rng.nextDouble();
        int dir = subState.getValue();
        if (rngvalue < 0.2 * p) dir++;
        else if (rngvalue < 0.4 * p) dir--;
        else if (rngvalue < 0.5 * p) dir += 2;
        else if (rngvalue < 0.6 * p) dir -= 2;
        else if (rngvalue < 0.7 * p) dir += 4;
        dir = (dir + 8) % 8;
        subState = Direction.fromInt(dir);
    }

    public void Tick(double time)
    {
        animTimer++;
        if (cycle < 0) cycle = 0;
        switch (Event)
        {
            case End:
                NextAnim();
                TurnRandom(0.7);
                break;
            case Reset:
                TurnRandom(0.1);
                if (cycle == 0) NextAnim();
                else
                {
                    Event = AnimEvent.Nothing;
                    animTimer = 0;
                    cycle--;
                }
                if (state == Animation.Walk) // Calculate direction to position before moving
                {
                    double angle = Math.atan2(targetY - getY(), targetX - getX()) * (4.0 / Math.PI) + 2.25;
                    subState = Direction.fromInt((int)(Math.round(angle + 8) % 8));
                }
                break;
            case Nothing:
            default: 
                break;
        }
        int length = 5;//Texture.Length(state);
        switch (state)
        {
            case Walk:
            case LeapForth:
                if (Math.sqrt(Math.pow(targetX - getX(), 2) + Math.pow(targetY - getY(), 2)) < 1) // Made it to position (or nearly)?
                {
                    cycle = 0;
                }
                Move(1f / length);
                break;
            case TumbleBack:
                Move(-1f / length);
                break;
            default:
                break;
        }
    }

    public void display(GL2 gl) {

        float[] coords = getCorner();
        gl.glBegin(GL2.GL_QUADS);
        gl.glColor4f(1f, 0f, 0f, 1f);
        gl.glVertex3d(coords[0], coords[1], 0);
        gl.glColor4f(0f, 0f, 1f, 1f);
        gl.glVertex3d(coords[2], coords[3], 0);
        gl.glColor4f(0f, 1f, 0f, 1f);
        gl.glVertex3d(coords[4], coords[5], 0);
        gl.glColor4f(1f, 1f, 1f, 0f);
        gl.glVertex3d(coords[6], coords[7], 0);
        gl.glEnd();
    }

    public void dispose(GLAutoDrawable arg0) {
    }
    
}
