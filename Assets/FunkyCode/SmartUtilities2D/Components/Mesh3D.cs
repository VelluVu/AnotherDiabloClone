using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesh3D : MonoBehaviour {
	public float size = 1f;
	public PolygonTriangulator2D.Triangulation triangulation = PolygonTriangulator2D.Triangulation.Advanced;

	// Optionable material
	public Material material;

	public string sortingLayerName; 
	public int sortingLayerID;
	public int sortingOrder;

	void Start () {
		// Generate Mesh from collider
		Polygon2D polygon = Polygon2DList.CreateFromGameObject (gameObject)[0];
		if (polygon != null) {
			polygon.CreateMesh3D(gameObject, size, Vector2.zero, Vector2.zero, triangulation);

			// Setting Mesh material
			if (material != null) {
				MeshRenderer meshRenderer = GetComponent<MeshRenderer> ();
				meshRenderer.material = material;
			
				meshRenderer.sortingLayerName = sortingLayerName;
				meshRenderer.sortingLayerID = sortingLayerID;
				meshRenderer.sortingOrder = sortingOrder;
			}
		}
	}
}
