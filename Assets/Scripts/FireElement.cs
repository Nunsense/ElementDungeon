using UnityEngine;
using System.Collections;

public class FireElement : Element {
	void Start() {
	
	}

	public override void Action(int i, int j) {
		Element[][] near = world.GetElementsNearBy(i, j, 1);
		for (int ii = 0; ii < near.Length; ii++) {
			if (near[ii] == null)
				continue;

			for (int jj = 0; jj < near[ii].Length; jj++) {
				if (ii == i && jj == j)
					continue;

				Element elem = near[ii][jj];

				if (elem != null && elem.GetElement() == ElementType.Plant) {
					world.CreateElementAtGridPos(ElementType.Fire, elem.GetGridX(), elem.GetGridY());
				}
			}	
		}
	}

	public override ElementType GetElement() {
		return ElementType.Fire;
	}

	public override bool CanPickUp() {
		return true;
	}
}
