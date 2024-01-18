package spritewander;

import com.jogamp.opengl.*;
import com.jogamp.opengl.awt.GLCanvas;

import spritewander.entity.Entity;

import java.awt.*;
import java.util.HashSet;

public class park implements GLEventListener  {

    protected HashSet<Entity> my_entities;
    public int max_X, max_Y;
    public float borderMargin = 0.5f;

    

    public park() {
        my_entities = new HashSet<Entity>();
        max_X = 1;
        max_Y = 1;
    }

    @Override
    public void display(GLAutoDrawable drawable) {
        for (Entity entity : my_entities) {
            entity.Tick(0.1f);
        }
        
        final GL2 gl = drawable.getGL().getGL2(); 
        gl.glClear(GL2.GL_COLOR_BUFFER_BIT | GL2.GL_DEPTH_BUFFER_BIT);
        for (Entity entity : my_entities) {
            if (!entity.isTexLoaded()) {
                entity.LinkTex(gl);
            }
            else {
                entity.display(gl);
            }
        }
        gl.glFlush(); 
    }

    @Override
    public void dispose(GLAutoDrawable arg0) {
    }

    @Override
    public void init(GLAutoDrawable arg0) {
    }

    @Override
    public void reshape(GLAutoDrawable arg0, int arg1, int arg2, int arg3, int arg4) {
        max_X = arg3;
        max_Y = arg4;
    }

    public static void main(String[] args)
    {
        final GLProfile gp = GLProfile.get(GLProfile.GL4);
        GLCapabilities cap = new GLCapabilities(gp);

        final GLCanvas gc = new GLCanvas(cap);
        park  af = new park();
        gc.addGLEventListener(af);
        gc.setSize(350,350);

        final Frame frame = new Frame("This is the frame");
        frame.add(gc);
        frame.setSize(500,400);
        frame.setVisible(true);
    }
    
    public void addEntity(Entity en)
    {
        my_entities.add(en);
    }
}
