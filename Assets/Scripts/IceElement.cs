using UnityEngine;
using System.Collections;

public class IceElement : Element {

	void Start() {
	}

	public override ElementType GetElementType() {
		return ElementType.Ice;
	}

	public override bool CanPickUp() {
		return false;
	}
}