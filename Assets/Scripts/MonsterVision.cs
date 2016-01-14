using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterVision : MonoBehaviour {
	public List<Element> nearByLetal;

	void Start() {
		nearByLetal = new List<Element>();
	}

	void OnTriggerEnter(Collider coll) {
		Element tempElem = coll.GetComponent<Element>();
		if (tempElem && tempElem.tag == "Letal") {
			nearByLetal.Add(tempElem);
		}
	}

	void OnTriggerExit(Collider coll) {
		Element tempElem = coll.GetComponent<Element>();
		if (tempElem && tempElem.tag == "Letal") {
			nearByLetal.Remove(tempElem);
		}
	}
}
