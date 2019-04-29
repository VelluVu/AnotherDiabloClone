using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingSourceTexture {
    static Vector2D p0 = Vector2D.Zero(); 
    static Vector2D p1 = Vector2D.Zero(); 
    static Vector2D p2 = Vector2D.Zero();
    static Vector2D p3 = Vector2D.Zero();

    static Vector2D z0 = Vector2D.Zero(); 
    static Vector2D z1 = Vector2D.Zero(); 
    static Vector2D z2 = Vector2D.Zero();
    static Vector2D z3 = Vector2D.Zero();
    static Vector2D z4 = Vector2D.Zero();
    static Vector2D z5 = Vector2D.Zero();

    static Vector2D up0 = Vector2D.Zero(); 
    static Vector2D up1 = Vector2D.Zero(); 
    static Vector2D up2 = Vector2D.Zero();
    static Vector2D up3 = Vector2D.Zero();

    static Vector2D down0 = Vector2D.Zero(); 
    static Vector2D down1 = Vector2D.Zero(); 
    static Vector2D down2 = Vector2D.Zero();
    static Vector2D down3 = Vector2D.Zero();

    static Vector2D left0 = Vector2D.Zero(); 
    static Vector2D left1 = Vector2D.Zero(); 
    static Vector2D left2 = Vector2D.Zero();
    static Vector2D left3 = Vector2D.Zero();

    static Vector2D right0 = Vector2D.Zero(); 
    static Vector2D right1 = Vector2D.Zero(); 
    static Vector2D right2 = Vector2D.Zero();
    static Vector2D right3 = Vector2D.Zero();

    static Vector2D size = Vector2D.Zero(); 

    static Vector2D zero = Vector2D.Zero();

    public static void CalculatePoints(LightingBuffer2D buffer) {
        float rotation = buffer.lightSource.transform.rotation.eulerAngles.z * Mathf.Deg2Rad + Mathf.PI / 4;
        float squaredSize = Mathf.Sqrt((float)((size.x * size.x) + (size.y * size.y)));

        p0.RotToVecItself(rotation);
        p0.x *= squaredSize;
        p0.y *= squaredSize;

        p1.RotToVecItself(rotation + Mathf.PI / 2);
        p1.x *= squaredSize;
        p1.y *= squaredSize;

        p2.RotToVecItself(rotation + Mathf.PI);
        p2.x *= squaredSize;
        p2.y *= squaredSize;

        p3.RotToVecItself(rotation - Mathf.PI / 2);
        p3.x *= squaredSize;
        p3.y *= squaredSize;

        z0.RotToVecItself(rotation + Mathf.PI / 4 + Mathf.PI / 2);
        z0.x *= squaredSize;
        z0.y *= squaredSize;

        z1.RotToVecItself(rotation + Mathf.PI / 4);
        z1.x *= squaredSize;
        z1.y *= squaredSize;

        z2.RotToVecItself(rotation + Mathf.PI / 4 - Mathf.PI / 2);
        z2.x *= squaredSize;
        z2.y *= squaredSize;

        z3.RotToVecItself(rotation + Mathf.PI / 4 - Mathf.PI);
        z3.x *= squaredSize;
        z3.y *= squaredSize;

        z4.RotToVecItself(rotation + Mathf.PI / 4 - Mathf.PI / 2 - Mathf.PI);
        z4.x *= squaredSize;
        z4.y *= squaredSize;

        z5.RotToVecItself(rotation + Mathf.PI / 4 + Mathf.PI);
        z5.x *= squaredSize;
        z5.y *= squaredSize;
    }

    public static void CalculateOffsets() {
        // 1
        up0.x = p1.x + z0.x;
        up0.y = p1.y + z0.y;

        up1.x = p1.x + z1.x + z0.x;
        up1.y = p1.y + z1.y + z0.y;

        up2.x = p0.x + z1.x + z2.x;
        up2.y = p0.y + z1.y + z2.y;

        up3.x = p0.x + z2.x;
        up3.y = p0.y + z2.y;

        // 2
        down0.x = p3.x + z2.x;
        down0.y = p3.y + z2.y;

        down1.x = p3.x + z2.x + z3.x;
        down1.y = p3.y + z2.y + z3.y;

        down2.x = p2.x + z3.x + z4.x;
        down2.y = p2.y + z3.y + z4.y;

        down3.x = p2.x + z4.x;
        down3.y = p2.y + z4.y;
    
        // 3
        left0.x = p0.x + z1.x;
        left0.y = p0.y + z1.y;

        left1.x = p0.x + z1.x + z2.x;
        left1.y = p0.y + z1.y + z2.y;

        left2.x = p3.x + z2.x + z3.x;
        left2.y = p3.y + z2.y + z3.y;

        left3.x = p3.x + z3.x;
        left3.y = p3.y + z3.y;
            
        // 4
        right0.x = p2.x + z5.x;
        right0.y = p2.y + z5.y;

        right1.x = p2.x + z0.x + z5.x;
        right1.y = p2.y + z0.y + z5.y;

        right2.x = p1.x + z0.x + z1.x;
        right2.y = p1.y + z0.y + z1.y;

        right3.x = p1.x + z1.x;
        right3.y = p1.y + z1.y;
    }

	///// Light Texture
	public static void Draw(LightingBuffer2D buffer) {
		float z = buffer.transform.position.z;

		size.x = buffer.bufferCamera.orthographicSize;
        size.y = buffer.bufferCamera.orthographicSize;

		if (buffer.lightSource.rotationEnabled) {
			// Light Rotation!!!
            CalculatePoints(buffer);
            CalculateOffsets();

			GL.PushMatrix();
			buffer.lightSource.GetMaterial ().SetPass(0);
			GL.Begin (GL.QUADS);

			Max2D.DrawImage_Batched(Vector2.zero, size, buffer.lightSource.transform.rotation.eulerAngles.z, z);

			buffer.lightSource.GetMaterial ().color = Color.black;

			Max2DMatrix.DrawQuad(right0, right1, right2, right3, z);
			Max2DMatrix.DrawQuad(left0, left1, left2, left3, z);
			Max2DMatrix.DrawQuad(down0, down1, down2, down3, z);
			Max2DMatrix.DrawQuad(up0, up1, up2, up3, z);

			GL.End ();
			GL.PopMatrix();
		} else {
			Max2D.DrawImage(buffer.lightSource.GetMaterial (), Vector2.zero, size, 0, z);
		}
	}
}