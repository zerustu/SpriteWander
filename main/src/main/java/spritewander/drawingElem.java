package spritewander;

import com.jogamp.opengl.GL2;

public class drawingElem {

    public float sx;
    public float sy;
    public float ex;
    public float ey;

    public drawingElem(float sx, float sy, float ex, float ey) {
        this.sx = sx;
        this.sy = sy;
        this.ex = ex;
        this.ey = ey;
    }

    public void display(GL2 gl)
    {
        gl.glBegin(GL2.GL_LINES);
        gl.glVertex2d(sx, sy);
        gl.glVertex2d(ex, ey);
        gl.glEnd();
    }
    
}
