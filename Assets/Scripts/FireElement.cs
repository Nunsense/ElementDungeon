using UnityEngine;
using System.Collections;

public class FireElement : Element {
	public GameObject particles;
	public GameObject lights;

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
				if (elem != null && elem.GetElementType() == ElementType.Plant) {
					world.CreateElementAtGridPos(ElementType.Fire, elem.GetGridX(), elem.GetGridY());
				}
			}	
		}
	}

	public override ElementType GetElementType() {
		return ElementType.Fire;
	}

	public override bool CanPickUp() {
		return true;
	}

	public override void SetVisible(bool visible) {
		lights.SetActive(visible);
		particles.SetActive(visible);
	}
}
