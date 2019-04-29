using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ComplexCut {
	public List<Vector2D> pointsList;
	float size = 1f;

	static public ComplexCut Create(List<Vector2D> pointsList, float size)
	{
		ComplexCut cut = new ComplexCut();
		cut.size = size;
		cut.pointsList = pointsList;
		return(cut);
	}

	public List<Vector2D> GetPointsList(float multiplier = 1f){
		if (pointsList == null) {
			Debug.LogError("Complex Cut generation issue");
			return(new List<Vector2D>());
		}
		float sizeM = size * multiplier;
		float sizeM2 = 2 * sizeM;

		List<Vector2D> list = new List<Vector2D>(pointsList);

		if (list.Count < 2) {
			return(new List<Vector2D>());
		}
		
		List<Vector2D> newPointsListA = new List<Vector2D>();
		List<Vector2D> newPointsListB = new List<Vector2D>();
		
		if (list.Count > 2) {

			foreach(DoublePair2D pair in DoublePair2D.GetList(list)) {
				float rotA = (float)Vector2D.Atan2(pair.B, pair.A);
				float rotC = (float)Vector2D.Atan2(pair.B, pair.C);

				Vector2D pairA = new Vector2D(pair.A);
				pairA.Push(rotA - Mathf.PI / 2, sizeM);

				Vector2D pairC = new Vector2D(pair.C);
				pairC.Push(rotC + Mathf.PI / 2, sizeM);
				
				Vector2D vecA = new Vector2D(pair.B);
				vecA.Push(rotA - Mathf.PI / 2, sizeM);
				vecA.Push(rotA, 10f);

				Vector2D vecC = new Vector2D(pair.B);
				vecC.Push(rotC + Mathf.PI / 2, sizeM);
				vecC.Push(rotC, 10f);

				Vector2D result = Math2D.GetPointLineIntersectLine(new Pair2D(pairA, vecA), new Pair2D(pairC, vecC));

				if (result != null) {
					newPointsListA.Add(result);
				}
			}

			if (newPointsListA.Count > 2) {
				newPointsListA.Remove(newPointsListA.First());
				newPointsListA.Remove(newPointsListA.Last());
			}

			
			foreach(DoublePair2D pair in DoublePair2D.GetList(list, false)) {
				float rotA = (float)Vector2D.Atan2(pair.B, pair.A);
				float rotC = (float)Vector2D.Atan2(pair.B, pair.C);

				Vector2D pairA = new Vector2D(pair.A);
				pairA.Push(rotA - Mathf.PI / 2, -sizeM);

				Vector2D pairC = new Vector2D(pair.C);
				pairC.Push(rotC + Mathf.PI / 2, -sizeM);
				
				Vector2D vecA = new Vector2D(pair.B);
				vecA.Push(rotA - Mathf.PI / 2, -sizeM);
				vecA.Push(rotA, 10f);

				Vector2D vecC = new Vector2D(pair.B);
				vecC.Push(rotC + Mathf.PI / 2, -sizeM);
				vecC.Push(rotC, 10f);

				Vector2D result = Math2D.GetPointLineIntersectLine(new Pair2D(pairA, vecA), new Pair2D(pairC, vecC));

				if (result != null) {
					newPointsListB.Add(result);
				}
			}

			if (newPointsListB.Count > 2) {
				newPointsListB.Remove(newPointsListB.First());
				newPointsListB.Remove(newPointsListB.Last());
			}
		}

		List<Vector2D> newPointsList = new List<Vector2D>();
		foreach(Vector2D p in newPointsListA) {
			newPointsList.Add(p);
		}
		
		Vector2D prevA = new Vector2D(list.ElementAt(list.Count - 2));
		Vector2D pA = new Vector2D(list.Last());
		pA.Push(Vector2D.Atan2(pA, prevA) - Mathf.PI / 6, sizeM2);
		newPointsList.Add(pA);

		pA = new Vector2D(list.Last());
		pA.Push(Vector2D.Atan2(pA, prevA) + Mathf.PI / 6, sizeM2);
		newPointsList.Add(pA);

		newPointsListB.Reverse();
		foreach(Vector2D p in newPointsListB) {
			newPointsList.Add(p);
		}

		Vector2D prevB = new Vector2D(list.ElementAt(1));
		Vector2D pB = new Vector2D(list.First());
		pB.Push(Vector2D.Atan2(pB, prevB) - Mathf.PI / 6, sizeM2);
		newPointsList.Add(pB);

		pB = new Vector2D(list.First());
		pB.Push(Vector2D.Atan2(pB, prevB) + Mathf.PI / 6, sizeM2);
		newPointsList.Add(pB);

		return(newPointsList);
	}
}
