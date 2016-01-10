using UnityEngine;
using System.Collections;

public class SoulElement : Element {

	void Start () {
	
	}

	public override ElementType GetElement () {
		return ElementType.Soul;
	}

	public override bool CanPickUp () {
		return true;
	}
}
