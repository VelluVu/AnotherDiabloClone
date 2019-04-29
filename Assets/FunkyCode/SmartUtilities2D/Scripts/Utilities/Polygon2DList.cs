using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon2DList : Polygon2D {

	// Get List Of Polygons from Collider (Usually Used Before Creating Slicer2D Object)
	static public List<Polygon2D> CreateFromPolygonColliderToWorldSpace(PolygonCollider2D collider) {
		List<Polygon2D> result = new List<Polygon2D> ();

		if (collider != null && collider.pathCount > 0) {
			Polygon2D newPolygon = new Polygon2D ();

			foreach (Vector2 p in collider.GetPath (0)) {
				newPolygon.AddPoint (p + collider.offset);
			}
			
			newPolygon = newPolygon.ToWorldSpace(collider.transform);

			result.Add (newPolygon);

			for (int i = 1; i < collider.pathCount; i++) {
				Polygon2D hole = new Polygon2D ();
				foreach (Vector2 p in collider.GetPath (i)) {
					hole.AddPoint (p + collider.offset);
				}

				hole = hole.ToWorldSpace(collider.transform);

				if (newPolygon.PolyInPoly (hole) == true) {
					newPolygon.AddHole(hole);
				} else {
					result.Add(hole);
				}
			}
		}
		return(result);
	}

	static public List<Polygon2D> CreateFromPolygonColliderToLocalSpace(PolygonCollider2D collider) {
		List<Polygon2D> result = new List<Polygon2D>();

		if (collider != null && collider.pathCount > 0) {
			Polygon2D newPolygon = new Polygon2D ();

			foreach (Vector2 p in collider.GetPath (0)) {
				newPolygon.AddPoint (p + collider.offset);
			}

			result.Add(newPolygon);

			for (int i = 1; i < collider.pathCount; i++) {
				Polygon2D hole = new Polygon2D ();
				foreach (Vector2 p in collider.GetPath (i)) {
					hole.AddPoint (p + collider.offset);
				}

				if (newPolygon.PolyInPoly (hole) == true) {
					newPolygon.AddHole (hole);
				} else {
					result.Add(hole);
				}
			}
		}
		return(result);
	}

	// Slower CreateFromCollider
	public static List<Polygon2D> CreateFromGameObject(GameObject gameObject) {
		return(CreateFromGameObject(gameObject, GetColliderType(gameObject)));
	}

	// Faster CreateFromCollider
	public static List<Polygon2D> CreateFromGameObject(GameObject gameObject, ColliderType colliderType) {
		List<Polygon2D> result = new List<Polygon2D>();
		
		switch (colliderType) {
			case ColliderType.Edge:
				result.Add(CreateFromEdgeCollider (gameObject.GetComponent<EdgeCollider2D> ()));
				break;
			case ColliderType.Polygon:
				result = CreateFromPolygonColliderToLocalSpace(gameObject.GetComponent<PolygonCollider2D> ());
				break;
			case ColliderType.Box:
				result.Add(CreateFromBoxCollider (gameObject.GetComponent<BoxCollider2D> ()));
				break;
			case ColliderType.Circle:
				result.Add(CreateFromCircleCollider (gameObject.GetComponent<CircleCollider2D> ()));
				break;
			case ColliderType.Capsule:
				result.Add(CreateFromCapsuleCollider (gameObject.GetComponent<CapsuleCollider2D> ()));
				break;
			default:
				break;
		}
		return(result);
	}
}
