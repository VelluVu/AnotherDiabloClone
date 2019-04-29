using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomPhysicsShape {
    private List<Polygon2D> polygons; 
	private Mesh shapeMesh = null;
    public Sprite sprite;
    
    public List<Polygon2D> GetShape() {
        if (polygons == null) {
            GeneratePhysicsShape();
        }
        return(polygons);
    }

    private void GeneratePhysicsShape() {
        polygons = new List<Polygon2D>();

		#if UNITY_2018_1_OR_NEWER

			int count = sprite.GetPhysicsShapeCount();

			List<Vector2> points;
			Polygon2D newPolygon;

			for(int i = 0; i < count; i++) {
				points = new List<Vector2>();
				sprite.GetPhysicsShape(i, points);
				
				newPolygon = new Polygon2D();

				for(int id = 0; id < points.Count; id++) {
					newPolygon.AddPoint(points[id]);
				}

				polygons.Add(newPolygon);
			}

			LightingDebug.totalObjectMaskShapeGenerations ++;
		#endif
	}

    public Mesh GetShapeMesh() {
		if (shapeMesh == null) {
			if (polygons.Count > 0) {
				shapeMesh = polygons[0].CreateMesh(Vector2.zero, Vector2.zero);	
				LightingDebug.totalObjectMaskMeshGenerations ++;
			}
		}
		return(shapeMesh);
	}
}
