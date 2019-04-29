using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightingRoom2D : MonoBehaviour {
	public enum RoomType {Collider};
	public Color color = Color.black;
	public RoomType roomType = RoomType.Collider;

	private Mesh mesh;

	public static List<LightingRoom2D> list = new List<LightingRoom2D>();

	public void OnEnable() {
		list.Add(this);
	}

	public void OnDisable() {
		list.Remove(this);
	}

	static public List<LightingRoom2D> GetList() {
		return(list);
	}

	public Mesh GetMesh() {
		if (mesh == null) {
			List<Polygon2D> polygons = Polygon2DList.CreateFromGameObject (gameObject);
			if (polygons.Count > 0) {
				mesh = PolygonTriangulator2D.Triangulate (polygons[0], Vector2.zero, Vector2.zero, PolygonTriangulator2D.Triangulation.Advanced);
			} else {
				Debug.Log("SmartLighting2D: LightingRoom2D object is missing Collider Component");
			}
		}	
		return(mesh);
	}
}
