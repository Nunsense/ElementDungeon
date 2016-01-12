using UnityEngine;
using System.Collections;

public class StoneElement : Element {

	// Use this for initialization
	void Start () {
	
	}

	public override bool CanPickUp () {
		return true;	
	}

	public override ElementType GetElementType () {
		return ElementType.Stone;
	}
}
