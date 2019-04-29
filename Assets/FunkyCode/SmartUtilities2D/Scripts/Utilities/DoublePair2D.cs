using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoublePair2D {

	public Vector2D A;
	public Vector2D B;
	public Vector2D C;

	public DoublePair2D(Vector2D pointA, Vector2D pointB, Vector2D pointC)
	{
		A = pointA;
		B = pointB;
		C = pointC;
	}

	static public List<DoublePair2D> GetList(List<Vector2D> list, bool connect = true)
	{
		List<DoublePair2D> pairsList = new List<DoublePair2D>();
		if (list.Count > 0) {
			foreach (Vector2D pB in list) {
				int indexB = list.IndexOf (pB);

				int indexA = (indexB - 1);
				if (indexA < 0) {
					indexA += list.Count;
				}

				int indexC = (indexB + 1);
				if (indexC >= list.Count) {
					indexC -= list.Count;
				}

				pairsList.Add (new DoublePair2D (list[indexA], pB, list[indexC]));
			}
		}
		return(pairsList);
	}
}
