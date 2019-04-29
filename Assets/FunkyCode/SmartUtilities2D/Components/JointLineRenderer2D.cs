using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class JointLineRenderer2D : MonoBehaviour {
	public Color color = Color.white;
	public float lineWidth = 1;

	private List<Joint2D> joints = new List<Joint2D>();
	private Mesh mesh = null;
	private Material material;

	const float lineOffset = -0.001f;

	public void Start() {
		joints = Joint2D.GetJoints(gameObject);

		Max2D.Check();
		material = new Material(Max2D.lineMaterial);
	}

	public void Update() {
		foreach(Joint2D joint in joints) {
			if (joint.gameObject == null) {
				continue;
			}
			if (joint.anchoredJoint2D == null) {
				continue;
			}
			if (joint.anchoredJoint2D.isActiveAndEnabled == false) {
				continue;
			}
			if (joint.anchoredJoint2D.connectedBody == null) {
				continue;
			}

			switch (joint.jointType) {
				case Joint2D.Type.HingeJoint2D:
					Pair2D pairA = new Pair2D(new Vector2D (transform.TransformPoint (joint.anchoredJoint2D.anchor)), new Vector2D (joint.anchoredJoint2D.connectedBody.transform.TransformPoint (Vector2.zero)));
					GenerateMesh(pairA);
					Draw();
					break;

				default:
					Pair2D pairB = new Pair2D(new Vector2D (transform.TransformPoint (joint.anchoredJoint2D.anchor)), new Vector2D (joint.anchoredJoint2D.connectedBody.transform.TransformPoint (joint.anchoredJoint2D.connectedAnchor)));
					GenerateMesh(pairB);
					Draw();
					break;
			}
		}
	}

	public void GenerateMesh(Pair2D pair) {
		List<Mesh2DTriangle> trianglesList = new List<Mesh2DTriangle>();
		trianglesList.Add(Max2DMesh.CreateLineNew(pair, lineWidth, transform.position.z + lineOffset));
		mesh = Max2DMesh.ExportMesh2(trianglesList);
	}

	public void Draw() {
		//material.color = color;
		material.SetColor ("_Emission", color);
		
		Max2DMesh.Draw(mesh, material);
	}
}