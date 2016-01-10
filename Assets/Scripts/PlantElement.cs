using UnityEngine;
using System.Collections;

public class PlantElement : Element {
	void Start () {
	
	}

	public override void Action (int i, int j) {
		if (world.IsElementClose (ElementType.Water, i, j, 2)) {
			world.CreateElementArround (ElementType.Plant, i, j);
		}
	}

	public override ElementType GetElement () {
		return ElementType.Plant;
	}

	public override bool CanPickUp () {
		return true;
	}
}
