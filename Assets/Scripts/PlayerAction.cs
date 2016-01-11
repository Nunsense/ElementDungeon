using UnityEngine;
using System.Collections.Generic;

public class PlayerAction : MonoBehaviour {

	public WorldManager world;

	public List<ThrowableObject> snowBalls;
	public List<ThrowableObject> fireBalls;
	private Element pickedUpObject;

	void Awake() {
		snowBalls = new List<ThrowableObject>();
		fireBalls = new List<ThrowableObject>();
	}

	void Start() {
	
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.D)) {
			if (pickedUpObject == null) {
				pickedUpObject = world.PickUpObjectInFront(transform);
				if (pickedUpObject != null) {
					pickedUpObject.PickUp(gameObject);
				}
			} else {
				if (world.MoveElementAtWorldPos(pickedUpObject, pickedUpObject.transform.position))
					pickedUpObject = null;
			}
		} else if (Input.GetKeyDown(KeyCode.C)) {
			if (snowBalls.Count > 0) {
				world.Throw(snowBalls[0], transform);
				snowBalls.RemoveAt(0);
			}
		} else if (Input.GetKeyDown(KeyCode.X)) {
			if (fireBalls.Count > 0) {
				world.Throw(fireBalls[0], transform);
				fireBalls.RemoveAt(0);
			}
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "SnowBall") {
			snowBalls.Add(col.gameObject.GetComponent<ThrowableObject>());
			col.gameObject.SetActive(false);
		} else if (col.tag == "FireBall") {
			fireBalls.Add(col.gameObject.GetComponent<ThrowableObject>());
			col.gameObject.SetActive(false);
		}
	}
}
