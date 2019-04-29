using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ColliderBevelRenderer2D : MonoBehaviour {
	public float lineWidth = 1.0f;
	public bool smooth = true;

	public GameObject lineObject;

	//private float lineOffset = - 0.125f;

	private List<Polygon2D> polygons;
	Material materialAdditive;
	Material materialMultiply;

	const float uv0 = 1f / 32;
	const float uv1 = 1f - uv0;

	public void Start()
	{
		polygons = Polygon2DList.CreateFromGameObject (gameObject); 
		
		Shader shaderAdditive = Shader.Find("Particles/Additive");
		Shader shaderMultiply = Shader.Find("Particles/Multiply");

		materialAdditive = new Material(shaderAdditive);
		materialAdditive.color = Color.white;
		materialAdditive.mainTexture = Resources.Load ("Textures/Bevel") as Texture;

		materialMultiply = new Material(shaderMultiply);
		materialMultiply.color = Color.white;
		materialMultiply.mainTexture = Resources.Load ("Textures/Bevel") as Texture;
	}

	public void OnRenderObject() {
		Max2D.SetLineWidth (lineWidth);
		Max2D.SetColor (Color.white);
		Max2D.SetLineMode (Max2D.LineMode.Smooth);
		Max2D.SetBorder (false);

		if (polygons.Count < 1) {
			return;
		}
		/*

		Polygon2D poly = polygon.ToWorldSpace(transform);
		
		float z = transform.position.z + lineOffset;

		
		List<DoublePair2D> list = DoublePair2D.GetList(poly.pointsList);
		DoublePair2D lastPair = list.Last();

		GL.PushMatrix ();
		materialAdditive.SetPass (0); 
		GL.Begin(GL.QUADS);

		foreach (DoublePair2D p in list) {
			Vector2D vA = GetPoint(lastPair);
			Vector2D vB = GetPoint(p);

			if (vA == null || vB == null) {
				continue;
			}

			float c = (Mathf.Cos((float)Vector2D.Atan2(vA, vB) + Mathf.PI / 4) + 1f) / 8.75f ;
			GL.Color(new Color(c, c, c));

			GL.TexCoord2(uv0, uv0);
			GL.Vertex3 ((float)p.A.x, (float)p.A.y, z);
			GL.TexCoord2(uv1, uv0);
			GL.Vertex3 ((float)p.B.x, (float)p.B.y, z);
			GL.TexCoord2(uv1, uv1);
			GL.Vertex3 ((float)vB.x, (float)vB.y, z);
			GL.TexCoord2(uv0, uv1);
			GL.Vertex3 ((float)vA.x, (float)vA.y, z);

			lastPair = p;
		}

		GL.End ();
		
		materialMultiply.SetPass (0); 
		GL.Begin(GL.QUADS);

		foreach (DoublePair2D p in list) {
			Vector2D vA = GetPoint(lastPair);
			Vector2D vB = GetPoint(p);

			if (vA == null || vB == null) {
				continue;
			}

			float c = (Mathf.Cos((float)Vector2D.Atan2(vA, vB) + Mathf.PI / 4) + 1f) / 1.75f + 0.5f;
			GL.Color(new Color(c, c, c));

			GL.TexCoord2(uv0, uv0);
			GL.Vertex3 ((float)p.A.x, (float)p.A.y, z);
			GL.TexCoord2(uv1, uv0);
			GL.Vertex3 ((float)p.B.x, (float)p.B.y, z);
			GL.TexCoord2(uv1, uv1);
			GL.Vertex3 ((float)vB.x, (float)vB.y, z);
			GL.TexCoord2(uv0, uv1);
			GL.Vertex3 ((float)vA.x, (float)vA.y, z);

			lastPair = p;
		}

		GL.End ();
		GL.PopMatrix ();
	 */
	}

	public Vector2D GetPoint(DoublePair2D pair) {
		float rotA = (float)Vector2D.Atan2(pair.B, pair.A);
		float rotC = (float)Vector2D.Atan2(pair.B, pair.C);

		Vector2D pairA = new Vector2D(pair.A);
		pairA.Push(rotA - Mathf.PI / 2,  lineWidth);
		pairA.Push(rotA, -100f);

		Vector2D pairC = new Vector2D(pair.C);
		pairC.Push(rotC + Mathf.PI / 2,  lineWidth);
		pairC.Push(rotC, -100f);
		
		Vector2D vecA = new Vector2D(pair.B);
		vecA.Push(rotA - Mathf.PI / 2, lineWidth);
		vecA.Push(rotA, 100f);

		Vector2D vecC = new Vector2D(pair.B);
		vecC.Push(rotC + Mathf.PI / 2, lineWidth);
		vecC.Push(rotC, 100f);
	
		return(Math2D.GetPointLineIntersectLine(new Pair2D(pairA, vecA), new Pair2D(pairC, vecC)));
	}
}

class lineQuad {
	public Vector2 A1;
	public Vector2 B1;
	public Vector2 A2;
	public Vector2 B2;
	public int type = 0;

	public lineQuad(Vector2 _A1, Vector2 _A2, Vector2 _B1, Vector2 _B2, int t =0) {
		A1 = _A1;
		A2 = _A2;
		B1 = _B1;
		B2 = _B2;
		type = t;
	}
}


		/*
		GL.PushMatrix ();
		materialShade.SetPass (0); 
		GL.Begin(GL.QUADS);

		foreach (Pair2D p in Pair2D.GetList(poly.pointsList)) {
			float c = (Mathf.Cos(Vector2D.Atan2(p.A, p.B) + Mathf.PI / 4) + 1f) / 2  + 0.5f;
			GL.Color(new Color(c, c, c));
			Vector2D vA = list[poly.pointsList.IndexOf(p.A)];
			Vector2D vB = list[poly.pointsList.IndexOf(p.B)];
			GL.Vertex3 (p.A.GetX(), p.A.GetY(), z);
			GL.Vertex3 (p.B.GetX(), p.B.GetY(), z);
			GL.Vertex3 (vB.GetX(), vB.GetY(), z);
			GL.Vertex3 (vA.GetX(), vA.GetY(), z);
		}
		GL.End ();
		GL.PopMatrix ();

 		*/
		//Max2D.DrawPolygon (poly.ToWorldSpace (transform), transform.position.z + lineOffset);
/*
Vector2 ComputeOutsetPoint(Vector2 A, Vector2 B, Vector2 C)
{
    Vector2 ba = new Vector2(A.x - B.x, A.y - B.y); 
	Vector2 bc = new Vector2(C.x - B.x, C.y - B.y);

	ba.Normalize();
	float d =  1f;

    Vector2 tmp = new Vector2(bc.x * -ba.x + bc.y * -ba.y, bc.x * ba.y + bc.y * -ba.x);
	
    float norm = Mathf.Sqrt(tmp.x * tmp.x + tmp.y * tmp.y);
    float dist = d * Mathf.Sqrt((norm - tmp.x) / (norm + tmp.x));
    tmp.x = tmp.y < 0.0 ? dist : -dist;
    tmp.y = d;

    return new Vector2(tmp.x * -ba.x + tmp.y * ba.y, tmp.x * -ba.y + tmp.y * -ba.x);
}
 */