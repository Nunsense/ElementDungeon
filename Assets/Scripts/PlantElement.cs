using UnityEngine;
using System.Collections;

public class PlantElement : Element {
	void Start () {
	
	}

	protected override void ActionElement (int i, int j) {
		if (world.IsElementClose (ElementType.Water, i, j, 2)) {
			world.CreateElementArround (ElementType.Plant, i, j);
		}
	}

	public override ElementType GetElementType () {
		return ElementType.Plant;
	}

	public override bool CanPickUp () {
		return true;
	}
}
