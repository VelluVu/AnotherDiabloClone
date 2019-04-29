using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DayLightingShadowCollider {
	public float sunDirection;

	public List<Polygon2D> polygons = new List<Polygon2D>();
	public List<Mesh> meshes = new List<Mesh>();
}