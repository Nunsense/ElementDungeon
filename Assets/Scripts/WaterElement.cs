using UnityEngine;
using System.Collections;

public class WaterElement : Element {

	void Start () {
	
	}

	protected override void UpdateElement () {
	}

	public override ElementType GetElement () {
		return ElementType.Water;
	}

	public override bool CanPickUp () {
		return true;
	}
}
