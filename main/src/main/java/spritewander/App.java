package spritewander;

import com.jogamp.opengl.GLCapabilities;
import com.jogamp.opengl.GLProfile;
import com.jogamp.opengl.awt.GLCanvas;
import com.jogamp.opengl.util.FPSAnimator;

import spritewander.entity.Entity;

import java.awt.*;
/**
 * Hello world!
 *
 */
public class App 
{
    public static void main(String[] args) throws InterruptedException {
        final GLProfile gp = GLProfile.get(GLProfile.GL2);
        GLCapabilities cap = new GLCapabilities(gp);

        final GLCanvas gc = new GLCanvas(cap);
        park Park = new park();
        gc.addGLEventListener(Park);
        gc.setSize(350,350);


        final Frame frame = new Frame("This is the frame");
        frame.add(gc);
        frame.setSize(500,400);
        frame.setVisible(true);

        final FPSAnimator animator = new FPSAnimator(gc, 400,true);   
        animator.start();   
        System.out.println("waitning");
        Thread.sleep(1000);
        System.out.println("adding a pokemon");
        
        Entity Et0 = new Entity(Park);
        Park.addEntity(Et0);
    }
}
