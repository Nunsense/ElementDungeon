using UnityEngine;
using System.Collections;

public class SoulElement : Element {
	public GameObject particles;
	public GameObject lights;

	void Start () {
	
	}

	public override ElementType GetElement () {
		return ElementType.Soul;
	}

	public override bool CanPickUp () {
		return true;
	}

	public override void SetVisible(bool visible) {
		lights.SetActive(visible);
		particles.SetActive(visible);
	}
}
