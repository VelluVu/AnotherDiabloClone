using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Max2DMatrix {
	const float pi = Mathf.PI;
	const float pi2 = pi / 2;
	const float uv0 = 1f / 32;
	const float uv1 = 1f - uv0;

	public static float c_x = 0;
	public static float c_y = 0;

	public static void DrawSlice(List<Vector2D> list, float z, bool connect = false) {
		foreach (Pair2D p in Pair2D.GetList(list, connect)) {
			GL.Vertex3((float)p.A.x, (float)p.A.y, z);
			GL.Vertex3((float)p.B.x, (float)p.B.y, z);
		}
	}

	public static void DrawTriangle(float x0, float y0, float x1, float y1, float x2, float y2, Vector2D offset, float z = 0f) {
		GL.Vertex3(x0 + (float)offset.x, y0 + (float)offset.y, z);
		GL.Vertex3(x1 + (float)offset.x, y1 + (float)offset.y, z);
		GL.Vertex3(x2 + (float)offset.x, y2 + (float)offset.y, z);
	}

	/*
	public static void DrawTriangle(Vector2 vA, Vector2 vB, Vector2 vC, Vector2D offset, float z, Vector2D scale) {
		GL.Vertex3(vA.x * (float)(scale.x + offset.x), vA.y * (float)(scale.y + offset.y), z);
		GL.Vertex3(vB.x * (float)(scale.x + offset.x), vB.y * (float)(scale.y + offset.y), z);
		GL.Vertex3(vC.x * (float)(scale.x + offset.x), vC.y * (float)(scale.y + offset.y), z);
	}
 	*/
	 
	public static void DrawTriangle(Vector2D vA, Vector2D vB, Vector2D vC, Vector2D offset, float z = 0f) {
		GL.TexCoord2(c_x, c_y);
		GL.Vertex3((float)vA.x + (float)offset.x, (float)vA.y + (float)offset.y, z);
		GL.TexCoord2(c_x, c_y);
		GL.Vertex3((float)vB.x + (float)offset.x, (float)vB.y + (float)offset.y, z);
		GL.TexCoord2(c_x, c_y);
		GL.Vertex3((float)vC.x + (float)offset.x, (float)vC.y + (float)offset.y, z);
	}


	public static void DrawTriangle(Vector2 vA, Vector2 vB, Vector2 vC, Vector2 offset, float z = 0f) {
		GL.Vertex3(vA.x * (float) offset.x, vA.y * (float) offset.y, z);
		GL.Vertex3(vB.x * (float) offset.x, vB.y * (float) offset.y, z);
		GL.Vertex3(vC.x * (float) offset.x, vC.y * (float) offset.y, z);
	}

	
	public static void DrawTriangle(Vector2 vA, Vector2 vB, Vector2 vC, Vector2 offset, float z, Vector2D scale) {
		GL.Vertex3(vA.x * (float)scale.x + offset.x, vA.y * (float)scale.y + offset.y, z);
		GL.Vertex3(vB.x * (float)scale.x + offset.x, vB.y * (float)scale.y + offset.y, z);
		GL.Vertex3(vC.x * (float)scale.x + offset.x, vC.y * (float)scale.y + offset.y, z);
	}

	public static void DrawQuad(Vector2 vA, Vector2 vB, Vector2 vC, Vector2 vD, Vector2D offset, float z, Vector2D scale) {
		GL.Vertex3(vA.x * (float)scale.x + (float)offset.x, vA.y * (float)(scale.y) + (float)offset.y, z);
		GL.Vertex3(vB.x * (float)scale.x + (float)offset.x, vB.y * (float)(scale.y) + (float)offset.y, z);
		GL.Vertex3(vC.x * (float)scale.x + (float)offset.x, vC.y * (float)(scale.y) + (float)offset.y, z);
		GL.Vertex3(vD.x * (float)scale.x + (float)offset.x, vD.y * (float)(scale.y) + (float)offset.y, z);
	}

	public static void DrawQuad(Vector2D vA, Vector2D vB, Vector2D vC, Vector2D vD, float z) {
		GL.Vertex3((float)vA.x, (float)vA.y, z);
		GL.Vertex3((float)vB.x, (float)vB.y, z);
		GL.Vertex3((float)vC.x, (float)vC.y, z);
		GL.Vertex3((float)vD.x, (float)vD.y, z);
	}





	public static void DrawLine(float x0, float y0, float x1, float y1, float z = 0f) {
		GL.Vertex3(x0, y0, z);
		GL.Vertex3(x1, y1, z);
	}

	public static void DrawSliceImage(List<Vector2D> list, float z, bool connect = false) {
		foreach (Pair2D p in Pair2D.GetList(list, connect)) {
			DrawLineImage (p, z);
		}
	}

	public static void DrawLineImage(Pair2D pair, float z = 0f) {
		float size = Max2D.lineWidth * Max2D.setScale;

		float rot = (float)Vector2D.Atan2 (pair.A, pair.B);

		Vector2D A1 = new Vector2D (pair.A);
		Vector2D A2 = new Vector2D (pair.A);
		Vector2D B1 = new Vector2D (pair.B);
		Vector2D B2 = new Vector2D (pair.B);

		A1.Push (rot + pi2, size);
		A2.Push (rot - pi2, size);
		B1.Push (rot + pi2, size);
		B2.Push (rot - pi2, size);

		GL.TexCoord2(0.5f + uv0, 0);
		GL.Vertex3((float)B1.x, (float)B1.y, z);
		GL.TexCoord2(uv1, 0);
		GL.Vertex3((float)A1.x, (float)A1.y, z);
		GL.TexCoord2(uv1, 1);
		GL.Vertex3((float)A2.x, (float)A2.y, z);
		GL.TexCoord2(0.5f + uv0, 1);
		GL.Vertex3((float)B2.x, (float)B2.y, z);

		A1 = new Vector2D (pair.A);
		A2 = new Vector2D (pair.A);
		Vector2D A3 = new Vector2D (pair.A);
		Vector2D A4 = new Vector2D (pair.A);

		A1.Push (rot + pi2, size);
		A2.Push (rot - pi2, size);

		A3.Push (rot + pi2, size);
		A4.Push (rot - pi2, size);
		A3.Push (rot + pi, -size);
		A4.Push (rot + pi, -size);

		GL.TexCoord2(uv0, 0);
		GL.Vertex3((float)A3.x, (float)A3.y, z);
		GL.TexCoord2(uv0, 1);
		GL.Vertex3((float)A4.x, (float)A4.y, z);
		GL.TexCoord2(0.5f - uv0, 1);
		GL.Vertex3((float)A2.x, (float)A2.y, z);
		GL.TexCoord2(0.5f - uv0, 0);
		GL.Vertex3((float)A1.x, (float)A1.y, z);

		B1 = new Vector2D (pair.B);
		B2 = new Vector2D (pair.B);
		Vector2D B3 = new Vector2D (pair.B);
		Vector2D B4 = new Vector2D (pair.B);

		B1.Push (rot + pi2, size);
		B2.Push (rot - pi2, size);

		B3.Push (rot + pi2, size);
		B4.Push (rot - pi2, size);
		B3.Push (rot + pi, size);
		B4.Push (rot + pi , size);

		GL.TexCoord2(uv0, 0);
		GL.Vertex3((float)B4.x, (float)B4.y, z);
		GL.TexCoord2(uv0, 1);
		GL.Vertex3((float)B3.x, (float)B3.y, z);
		GL.TexCoord2(0.5f - uv0, 1);
		GL.Vertex3((float)B1.x, (float)B1.y, z);
		GL.TexCoord2(0.5f - uv0, 0);
		GL.Vertex3((float)B2.x, (float)B2.y, z);
	}

	
	public static void DrawLineImage(Transform transform, Pair2D pair, float z = 0f) {
		float size = Max2D.lineWidth * Max2D.setScale;

		float rot = (float)Vector2D.Atan2 (pair.A, pair.B);

		Vector2D A1 = new Vector2D (pair.A);
		Vector2D A2 = new Vector2D (pair.A);
		Vector2D B1 = new Vector2D (pair.B);
		Vector2D B2 = new Vector2D (pair.B);

		Vector2 scale = new Vector2(1f / transform.localScale.x, 1f / transform.localScale.y);

		A1.Push (rot + pi2, size, scale);
		A2.Push (rot - pi2, size, scale);
		B1.Push (rot + pi2, size, scale);
		B2.Push (rot - pi2, size, scale);

		GL.TexCoord2(0.5f + uv0, 0);
		GL.Vertex3((float)B1.x, (float)B1.y, z);
		GL.TexCoord2(uv1, 0);
		GL.Vertex3((float)A1.x, (float)A1.y, z);
		GL.TexCoord2(uv1, 1);
		GL.Vertex3((float)A2.x, (float)A2.y, z);
		GL.TexCoord2(0.5f + uv0, 1);
		GL.Vertex3((float)B2.x, (float)B2.y, z);

		A1 = new Vector2D (pair.A);
		A2 = new Vector2D (pair.A);
		Vector2D A3 = new Vector2D (pair.A);
		Vector2D A4 = new Vector2D (pair.A);

		A1.Push (rot + pi2, size, scale);
		A2.Push (rot - pi2, size, scale);

		A3.Push (rot + pi2, size, scale);
		A4.Push (rot - pi2, size, scale);
		A3.Push (rot + pi, -size, scale);
		A4.Push (rot + pi, -size, scale);

		GL.TexCoord2(uv0, 0);
		GL.Vertex3((float)A3.x, (float)A3.y, z);
		GL.TexCoord2(uv0, 1);
		GL.Vertex3((float)A4.x, (float)A4.y, z);
		GL.TexCoord2(0.5f - uv0, 1);
		GL.Vertex3((float)A2.x, (float)A2.y, z);
		GL.TexCoord2(0.5f - uv0, 0);
		GL.Vertex3((float)A1.x, (float)A1.y, z);

		B1 = new Vector2D (pair.B);
		B2 = new Vector2D (pair.B);
		Vector2D B3 = new Vector2D (pair.B);
		Vector2D B4 = new Vector2D (pair.B);

		B1.Push (rot + pi2, size, scale);
		B2.Push (rot - pi2, size, scale);

		B3.Push (rot + pi2, size, scale);
		B4.Push (rot - pi2, size, scale);
		B3.Push (rot + pi, size, scale);
		B4.Push (rot + pi , size, scale);

		GL.TexCoord2(uv0, 0);
		GL.Vertex3((float)B4.x, (float)B4.y, z);
		GL.TexCoord2(uv0, 1);
		GL.Vertex3((float)B3.x, (float)B3.y, z);
		GL.TexCoord2(0.5f - uv0, 1);
		GL.Vertex3((float)B1.x, (float)B1.y, z);
		GL.TexCoord2(0.5f - uv0, 0);
		GL.Vertex3((float)B2.x, (float)B2.y, z);
	}

	public static void DrawSliceImage(Transform transform, List<Vector2D> list, float z, bool connect = false) {
		foreach (Pair2D p in Pair2D.GetList(list, connect)) {
			DrawLineImage (transform, p, z);
		}
	}
}
