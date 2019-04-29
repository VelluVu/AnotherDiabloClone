using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint2D {
	public enum Type {DistanceJoint2D, FixedJoint2D, FrictionJoint2D, HingeJoint2D, RelativeJoint2D, SliderJoint2D, SpringJoint2D, TargetJoint2D, WheelJoint2D};
	public Type jointType;
	public GameObject gameObject;
	
	public DistanceJoint2D distanceJoint2D;
	public FixedJoint2D fixedJoint2D;
	public FrictionJoint2D frictionJoint2D;
	public HingeJoint2D hingeJoint2D;

	public RelativeJoint2D relativeJoint2D;

	public SliderJoint2D sliderJoint2D;
	public SpringJoint2D springJoint2D;
	public TargetJoint2D targetJoint2D;
	public WheelJoint2D wheelJoint2D;

	public AnchoredJoint2D anchoredJoint2D;

	public Joint2D(Type type) {
		jointType = type;
	}
	
	public static List<Joint2D> GetJoints(GameObject gameObject) {
		List<Joint2D> result = new List<Joint2D>();
		
		foreach(DistanceJoint2D joint in gameObject.GetComponents<DistanceJoint2D>()) {
			Joint2D joint2D = new Joint2D(Type.DistanceJoint2D);
			joint2D.distanceJoint2D = joint;
			joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
			joint2D.gameObject = joint.gameObject;
			result.Add(joint2D);
		}

		foreach(FixedJoint2D joint in gameObject.GetComponents<FixedJoint2D>()) {
			Joint2D joint2D = new Joint2D(Type.FixedJoint2D);
			joint2D.fixedJoint2D = joint;
			joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
			joint2D.gameObject = joint.gameObject;
			result.Add(joint2D);
		}

		foreach(FrictionJoint2D joint in gameObject.GetComponents<FrictionJoint2D>()) {
			Joint2D joint2D = new Joint2D(Type.FrictionJoint2D);
			joint2D.frictionJoint2D = joint;
			joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
			joint2D.gameObject = joint.gameObject;
			result.Add(joint2D);
		}
		
		foreach(HingeJoint2D joint in gameObject.GetComponents<HingeJoint2D>()) {
			Joint2D joint2D = new Joint2D(Type.HingeJoint2D);
			joint2D.hingeJoint2D = joint;
			joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
			joint2D.gameObject = joint.gameObject;
			result.Add(joint2D);
		}

		foreach(SliderJoint2D joint in gameObject.GetComponents<SliderJoint2D>()) {
			Joint2D joint2D = new Joint2D(Type.SliderJoint2D);
			joint2D.sliderJoint2D = joint;
			joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
			joint2D.gameObject = joint.gameObject;
			result.Add(joint2D);
		}

		foreach(SpringJoint2D joint in gameObject.GetComponents<SpringJoint2D>()) {
			Joint2D joint2D = new Joint2D(Type.SpringJoint2D);
			joint2D.springJoint2D = joint;
			joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
			joint2D.gameObject = joint.gameObject;
			result.Add(joint2D);
		}

		foreach(WheelJoint2D joint in gameObject.GetComponents<WheelJoint2D>()) {
			Joint2D joint2D = new Joint2D(Type.WheelJoint2D);
			joint2D.wheelJoint2D = joint;
			joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
			result.Add(joint2D);
			joint2D.gameObject = joint.gameObject;
		}

		foreach(TargetJoint2D joint in gameObject.GetComponents<TargetJoint2D>()) {
			Joint2D joint2D = new Joint2D(Type.SpringJoint2D);
			joint2D.targetJoint2D = joint;
			joint2D.gameObject = joint.gameObject;
			result.Add(joint2D);
		}

		//foreach(RelativeJoint2D joint in gameObject.GetComponents<RelativeJoint2D>()) {
		//	Joint2D joint2D = new Joint2D(Type.RelativeJoint2D);
		//	joint2D.relativeJoint2D = joint;
		//	joint2D.gameObject = joint.gameObject;
		//	result.Add(joint2D);
		//}


		return(result);
	}

	public static List<Joint2D> GetJointsConnected(Rigidbody2D connected) {
		List<Joint2D> result = new List<Joint2D>();

		foreach(AnchoredJoint2D anchoredJoint in Object.FindObjectsOfType<AnchoredJoint2D>()) {
			if (anchoredJoint.connectedBody == connected) {
				
				foreach(DistanceJoint2D joint in anchoredJoint.gameObject.GetComponents<DistanceJoint2D>()) {
					if (joint.connectedBody == connected) {
						Joint2D joint2D = new Joint2D(Type.DistanceJoint2D);
						joint2D.distanceJoint2D = joint;
						joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
						joint2D.gameObject = joint.gameObject;
						result.Add(joint2D);
					}
				}

				foreach(FixedJoint2D joint in anchoredJoint.gameObject.GetComponents<FixedJoint2D>()) {
					if (joint.connectedBody == connected) {
						Joint2D joint2D = new Joint2D(Type.FixedJoint2D);
						joint2D.fixedJoint2D = joint;
						joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
						joint2D.gameObject = joint.gameObject;
						result.Add(joint2D);
					}
				}

				foreach(FrictionJoint2D joint in anchoredJoint.gameObject.GetComponents<FrictionJoint2D>()) {
					if (joint.connectedBody == connected) {
						Joint2D joint2D = new Joint2D(Type.FrictionJoint2D);
						joint2D.frictionJoint2D = joint;
						joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
						joint2D.gameObject = joint.gameObject;
						result.Add(joint2D);
					}
				}

				foreach(HingeJoint2D joint in anchoredJoint.gameObject.GetComponents<HingeJoint2D>()) {
					if (joint.connectedBody == connected) {
						Joint2D joint2D = new Joint2D(Type.HingeJoint2D);
						joint2D.hingeJoint2D = joint;
						joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
						joint2D.gameObject = joint.gameObject;
						result.Add(joint2D);
					}
				}

				foreach(SliderJoint2D joint in anchoredJoint.gameObject.GetComponents<SliderJoint2D>()) {
					if (joint.connectedBody == connected) {
						Joint2D joint2D = new Joint2D(Type.SliderJoint2D);
						joint2D.sliderJoint2D = joint;
						joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
						joint2D.gameObject = joint.gameObject;
						result.Add(joint2D);
					}
				}

				foreach(SpringJoint2D joint in anchoredJoint.gameObject.GetComponents<SpringJoint2D>()) {
					if (joint.connectedBody == connected) {
						Joint2D joint2D = new Joint2D(Type.SpringJoint2D);
						joint2D.springJoint2D = joint;
						joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
						joint2D.gameObject = joint.gameObject;
						result.Add(joint2D);
					}
				}

				foreach(WheelJoint2D joint in anchoredJoint.gameObject.GetComponents<WheelJoint2D>()) {
					if (anchoredJoint.connectedBody == connected) {
						Joint2D joint2D = new Joint2D(Type.WheelJoint2D);
						joint2D.wheelJoint2D = joint;
						joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
						joint2D.gameObject = joint.gameObject;
						result.Add(joint2D);
					}
				}

			}
		}

		//foreach(RelativeJoint2D joint in Object.FindObjectsOfType<RelativeJoint2D>()) {
		//	if (joint.connectedBody == connected) {
		//		Joint2D joint2D = new Joint2D(Type.RelativeJoint2D);
		//		joint2D.relativeJoint2D = joint;
		//		result.Add(joint2D);
		//	}
		//}

		//foreach(TargetJoint2D joint in Object.FindObjectsOfType<TargetJoint2D>()) {
		//	if (joint.connectedBody == connected) {
		//		Joint2D joint2D = new Joint2D(Type.SpringJoint2D);
		//		joint2D.targetJoint2D = joint;
		//		result.Add(joint2D);
		//	}
		//}

		return(result);
	}

	public static List<Joint2D> GetJointsConnected() {
		List<Joint2D> result = new List<Joint2D>();

		foreach(AnchoredJoint2D anchoredJoint in Object.FindObjectsOfType<AnchoredJoint2D>()) {
			if (anchoredJoint.connectedBody != null) {
				
				foreach(DistanceJoint2D joint in anchoredJoint.gameObject.GetComponents<DistanceJoint2D>()) {
					if (joint.connectedBody != null) {
						Joint2D joint2D = new Joint2D(Type.DistanceJoint2D);
						joint2D.distanceJoint2D = joint;
						joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
						joint2D.gameObject = joint.gameObject;
						result.Add(joint2D);
					}
				}

				foreach(FixedJoint2D joint in anchoredJoint.gameObject.GetComponents<FixedJoint2D>()) {
					if (joint.connectedBody != null) {
						Joint2D joint2D = new Joint2D(Type.FixedJoint2D);
						joint2D.fixedJoint2D = joint;
						joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
						joint2D.gameObject = joint.gameObject;
						result.Add(joint2D);
					}
				}

				foreach(FrictionJoint2D joint in anchoredJoint.gameObject.GetComponents<FrictionJoint2D>()) {
					if (joint.connectedBody != null) {
						Joint2D joint2D = new Joint2D(Type.FrictionJoint2D);
						joint2D.frictionJoint2D = joint;
						joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
						joint2D.gameObject = joint.gameObject;
						result.Add(joint2D);
					}
				}

				foreach(HingeJoint2D joint in anchoredJoint.gameObject.GetComponents<HingeJoint2D>()) {
					if (joint.connectedBody != null) {
						Joint2D joint2D = new Joint2D(Type.HingeJoint2D);
						joint2D.hingeJoint2D = joint;
						joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
						joint2D.gameObject = joint.gameObject;
						result.Add(joint2D);
					}
				}

				foreach(SliderJoint2D joint in anchoredJoint.gameObject.GetComponents<SliderJoint2D>()) {
					if (joint.connectedBody != null) {
						Joint2D joint2D = new Joint2D(Type.SliderJoint2D);
						joint2D.sliderJoint2D = joint;
						joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
						joint2D.gameObject = joint.gameObject;
						result.Add(joint2D);
					}
				}

				foreach(SpringJoint2D joint in anchoredJoint.gameObject.GetComponents<SpringJoint2D>()) {
					if (joint.connectedBody != null) {
						Joint2D joint2D = new Joint2D(Type.SpringJoint2D);
						joint2D.springJoint2D = joint;
						joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
						joint2D.gameObject = joint.gameObject;
						result.Add(joint2D);
					}
				}

				foreach(WheelJoint2D joint in anchoredJoint.gameObject.GetComponents<WheelJoint2D>()) {
					if (joint.connectedBody != null) {
						Joint2D joint2D = new Joint2D(Type.WheelJoint2D);
						joint2D.wheelJoint2D = joint;
						joint2D.anchoredJoint2D = (AnchoredJoint2D)joint;
						joint2D.gameObject = joint.gameObject;
						result.Add(joint2D);
					}
				}

			}
		}

		//foreach(RelativeJoint2D joint in Object.FindObjectsOfType<RelativeJoint2D>()) {
		//	if (joint.connectedBody == connected) {
		//		Joint2D joint2D = new Joint2D(Type.RelativeJoint2D);
		//		joint2D.relativeJoint2D = joint;
		//		result.Add(joint2D);
		//	}
		//}

		//foreach(TargetJoint2D joint in Object.FindObjectsOfType<TargetJoint2D>()) {
		//	if (joint.connectedBody == connected) {
		//		Joint2D joint2D = new Joint2D(Type.SpringJoint2D);
		//		joint2D.targetJoint2D = joint;
		//		result.Add(joint2D);
		//	}
		//}

		return(result);
	}
}
