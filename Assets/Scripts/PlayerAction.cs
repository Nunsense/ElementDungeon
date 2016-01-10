using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour {

	public WorldManager world;

	private Element pickedUpObject;

	void Start() {
	
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (pickedUpObject == null) {
				pickedUpObject = world.PickUpObjectInFront(transform);
				if (pickedUpObject != null) {
					pickedUpObject.PickUp(gameObject);
				}
			} else {
				if (world.MoveElementAtWorldPos(pickedUpObject, pickedUpObject.transform.position))
					pickedUpObject = null;
			}
		}
	}
}
