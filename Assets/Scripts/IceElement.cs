using UnityEngine;
using System.Collections;

public class IceElement : Element {

	void Start() {
	}

	public override void Action(int i, int j) {
	}

	public override ElementType GetElement() {
		return ElementType.Ice;
	}

	public override bool CanPickUp() {
		return false;
	}
}