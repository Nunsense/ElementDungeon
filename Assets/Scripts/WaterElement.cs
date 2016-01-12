using UnityEngine;
using System.Collections;

public class WaterElement : Element {
	public GameObject particles;

	void Start() {
	
	}

	public override void Action(int i, int j) {
		Element[][] near = world.GetElementsNearBy(i, j, 2);
		for (int ii = 0; ii < near.Length; ii++) {
			if (near[ii] == null)
				continue;

			for (int jj = 0; jj < near[ii].Length; jj++) {
				if (ii > 0 && ii < 4 && jj > 0 && jj < 4)
					continue;

				Element elem = near[ii][jj];

				if (elem != null && elem.GetElementType() == ElementType.Water) {
					int iii = gridX + (gridX - elem.GetGridX() > 0 ? -1 : 1);
					int jjj = gridY + (gridY - elem.GetGridY() > 0 ? -1 : 1);
					world.CreateElementAtGridPos(ElementType.Water, iii, jjj);
				}
			}	
		}
	}

	public override ElementType GetElementType() {
		return ElementType.Water;
	}

	public override bool CanPickUp() {
		return true;
	}

	public override void SetVisible(bool visible) {
		particles.SetActive(visible);
	}
}
