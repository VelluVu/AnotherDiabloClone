using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColliderLineRenderer2D : MonoBehaviour {
	public bool customColor = false;
	public Color color = Color.white;
	public float lineWidth = 1;

	private bool edgeCollider = false; // For Edge Collider

	private Polygon2D polygon = null;
	private Mesh mesh = null;
	private float lineWidthSet = 1;

	private Material material;
	static private Material staticMaterial;

	const float lineOffset = -0.01f;

	public void Initialize() {
		Max2D.Check();

		polygon = null;
		mesh = null;

		if (GetComponent<EdgeCollider2D>() != null) {
			edgeCollider = true;
		} else {
			edgeCollider = false;
		}
		
		if (material == null) {
			material = new Material(Max2D.lineMaterial);
		}
		
		if (staticMaterial == null) {
			staticMaterial = new Material(Max2D.lineMaterial);
		}

		GenerateMesh();
		Draw();
	}

	void Start () {
		Initialize();
	}
	
	public void LateUpdate() {
		if (lineWidth != lineWidthSet) {
			if (lineWidth < 0.01f) {
				lineWidth = 0.01f;
			}
			GenerateMesh();
		}

		Draw();
	}

	public Polygon2D GetPolygon() {
		if (polygon == null) {
			polygon = Polygon2DList.CreateFromGameObject (gameObject)[0];
		}
		return(polygon);
	}

	public void GenerateMesh() {
		lineWidthSet = lineWidth;

		mesh = Max2DMesh.GeneratePolygon2DMeshNew(transform, GetPolygon(), lineOffset, lineWidth, edgeCollider == false);
	}

	public void Draw() {
		if (customColor) {
			if (material != null) {
				material.SetColor ("_Emission", color);
				Max2DMesh.Draw(mesh, transform, material);
			}
		} else {
			Max2D.Check();
			if (staticMaterial != null) {
				staticMaterial.SetColor ("_Emission", Color.black);
				Max2DMesh.Draw(mesh, transform, staticMaterial);
			}
		}
	}
}