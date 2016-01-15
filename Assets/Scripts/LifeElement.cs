using UnityEngine;
using System.Collections;

public class LifeElement : Element {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override bool CanPickUp() {
		return true;
	}

	public override ElementType GetElementType() {
		return ElementType.Life;
	}
}
