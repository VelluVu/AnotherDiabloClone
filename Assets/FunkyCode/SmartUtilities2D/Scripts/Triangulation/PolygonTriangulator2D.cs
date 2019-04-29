using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PolygonTriangulator2D : MonoBehaviour {
	public enum Triangulation {Advanced, Legacy};
	static float precision = 0.001f;

	
	public static Mesh Triangulate3D(Polygon2D polygon, float z, Vector2 UVScale, Vector2 UVOffset, Triangulation triangulation)
	{
		Mesh result = null;
		switch (triangulation) {
			case Triangulation.Advanced:
				Polygon2D newPolygon = new Polygon2D(PreparePolygon(polygon));
				foreach (Polygon2D hole in polygon.holesList) {
					newPolygon.AddHole(new Polygon2D(PreparePolygon(hole)));
				}

				if (newPolygon.pointsList.Count < 3) {
					if ((int)polygon.GetArea() == 0) {
						List<Vector2> l = new List<Vector2>();
						foreach(Vector2D p in polygon.pointsList) {
							l.Add(p.ToVector2());
						}

						result = UnityDefaultTriangulator.Create(l.ToArray());;
						
						return(result);
					}
				}

				List<Vector3> sideVertices = new List<Vector3>();
				List<int> sideTriangles = new List<int>();
				int vCount = 0;
				foreach(Pair2D pair in Pair2D.GetList(polygon.pointsList)) {
					Vector3 pointA = new Vector3((float)pair.A.x, (float)pair.A.y, 0);
					Vector3 pointB = new Vector3((float)pair.B.x, (float)pair.B.y, 0);
					Vector3 pointC = new Vector3((float)pair.B.x, (float)pair.B.y, 1);
					Vector3 pointD = new Vector3((float)pair.A.x, (float)pair.A.y, 1);

					sideVertices.Add(pointA);
					sideVertices.Add(pointB);
					sideVertices.Add(pointC);
					sideVertices.Add(pointD);
					
					sideTriangles.Add(vCount + 2);
					sideTriangles.Add(vCount + 1);
					sideTriangles.Add(vCount + 0);
					
					sideTriangles.Add(vCount + 0);
					sideTriangles.Add(vCount + 3);
					sideTriangles.Add(vCount + 2);

					vCount += 4;
				}

				Mesh meshA = TriangulateAdvanced(newPolygon, UVScale, UVOffset);

				Mesh meshB = new Mesh();
				List<Vector3> verticesB = new List<Vector3>();
				foreach(Vector3 v in meshA.vertices) {
					verticesB.Add(new Vector3(v.x, v.y, v.z + z));
				}
				meshB.vertices = verticesB.ToArray();
				meshB.triangles = meshA.triangles.Reverse().ToArray();
		

				Mesh mesh = new Mesh();
				mesh.vertices = sideVertices.ToArray();
				mesh.triangles= sideTriangles.ToArray();
			
				List<Vector3> vertices = new List<Vector3>();
				foreach(Vector3 v in meshA.vertices) {
					vertices.Add(v);
				}
				foreach(Vector3 v in meshB.vertices) {
					vertices.Add(v);
				}
				foreach(Vector3 v in sideVertices) {
					vertices.Add(v);
				}
				mesh.vertices = vertices.ToArray();

				List<int> triangles = new List<int>();
				foreach(int p in meshA.triangles) {
					triangles.Add(p);
				}
				int count = meshA.vertices.Count();
				foreach(int p in meshB.triangles) {
					triangles.Add(p + count);
				}
				count = meshA.vertices.Count() + meshB.vertices.Count();
				foreach(int p in sideTriangles) {
					triangles.Add(p + count);
				}
				mesh.triangles = triangles.ToArray();

				mesh.RecalculateNormals();
				mesh.RecalculateBounds();
			
				result = mesh;

			break;
		}

		return(result);
	}

	public static Mesh Triangulate(Polygon2D polygon, Vector2 UVScale, Vector2 UVOffset, Triangulation triangulation)
	{
		Mesh result = null;
		switch (triangulation) {
			case Triangulation.Advanced:
				Polygon2D newPolygon = new Polygon2D(PreparePolygon(polygon));
				if (newPolygon.pointsList.Count < 3) {
					Debug.LogWarning("mesh is too small for advanced triangulation, using simplified triangulations instead (size: " + polygon.GetArea() +")");
					
					result = TriangulateAdvanced(polygon, UVScale, UVOffset);

					return(result);
				}

				foreach (Polygon2D hole in polygon.holesList) {
					newPolygon.AddHole(new Polygon2D(PreparePolygon(hole)));
				}

				result = TriangulateAdvanced(newPolygon, UVScale, UVOffset);

			break;

			case Triangulation.Legacy:

				List<Vector2> list = new List<Vector2>();
				foreach(Vector2D p in polygon.pointsList) {
					list.Add(p.ToVector2());
				}
				result = UnityDefaultTriangulator.Create(list.ToArray());
				return(result);
		}

		return(result);
	}

	// Not finished - still has some artifacts
	static public List<Vector2D> PreparePolygon(Polygon2D polygon)
	{
		Polygon2D newPolygon = new Polygon2D();

		polygon.Normalize();

		DoublePair2D pair;
		foreach (Vector2D pB in polygon.pointsList) {
				int indexB = polygon.pointsList.IndexOf (pB);

				int indexA = (indexB - 1);
				if (indexA < 0) {
					indexA += polygon.pointsList.Count;
				}

				int indexC = (indexB + 1);
				if (indexC >= polygon.pointsList.Count) {
					indexC -= polygon.pointsList.Count;
				}

				pair = new DoublePair2D (polygon.pointsList[indexA], pB, polygon.pointsList[indexC]);

				float rotA = (float)Vector2D.Atan2(pair.B, pair.A);
				float rotC = (float)Vector2D.Atan2(pair.B, pair.C);

				Vector2D pairA = new Vector2D(pair.A);
				pairA.Push(rotA - Mathf.PI / 2, precision);

				Vector2D pairC = new Vector2D(pair.C);
				pairC.Push(rotC + Mathf.PI / 2, precision);
				
				Vector2D vecA = new Vector2D(pair.B);
				vecA.Push(rotA - Mathf.PI / 2, precision);
				vecA.Push(rotA, 10f);

				Vector2D vecC = new Vector2D(pair.B);
				vecC.Push(rotC + Mathf.PI / 2, precision);
				vecC.Push(rotC, 10f);

				Vector2D result = Math2D.GetPointLineIntersectLine(new Pair2D(pairA, vecA), new Pair2D(pairC, vecC));

				if (result != null) {
					newPolygon.AddPoint(result);
				}
			}

		return(newPolygon.pointsList);
	} 

	public static Mesh TriangulateAdvanced(Polygon2D polygon, Vector2 UVScale, Vector2 UVOffset)
	{
		foreach(Pair2D p in Pair2D.GetList(new List<Vector2D>(polygon.pointsList))) {
			if (polygon.pointsList.Count < 4) {
				break;
			}
			if (Vector2D.Distance(p.A, p.B) < 0.005f) {
				Debug.LogWarning("Slicer2D: Polygon points are too close");
				polygon.pointsList.Remove(p.A);
			}
		}

		TriangulationWrapper.Polygon poly = new TriangulationWrapper.Polygon();

		List<Vector2> pointsList = null;
		List<Vector2> UVpointsList = null;

		Vector3 v = Vector3.zero;

		foreach (Vector2D p in polygon.pointsList) {
			v = p.ToVector2();
			poly.outside.Add (v);
			poly.outsideUVs.Add (new Vector2(v.x / UVScale.x + .5f + UVOffset.x, v.y / UVScale.y + .5f + UVOffset.y));
		}

		foreach (Polygon2D hole in polygon.holesList) {
			pointsList = new List<Vector2> ();
			UVpointsList = new List<Vector2> ();
			
			foreach (Vector2D p in hole.pointsList) {
				v = p.ToVector2();
				pointsList.Add (v);
				UVpointsList.Add (new Vector2(v.x / UVScale.x + .5f, v.y / UVScale.y + .5f));
			}

			poly.holes.Add (pointsList);
			poly.holesUVs.Add (UVpointsList);
		}

		return(TriangulationWrapper.CreateMesh (poly));
	}
}