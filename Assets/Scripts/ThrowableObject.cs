using UnityEngine;
using System.Collections;

public class ThrowableObject : MonoBehaviour {

	public bool isLetal;
	private WorldManager world;
	
	void Start () {
		isLetal = false;
	}

	public void Throw(WorldManager w) {
		isLetal = true;
		world = w;
	}

	void OnCollisionEnter(Collision col) {
		if (isLetal && col.collider.tag == "Monster") {
			world.KillMonster(col.gameObject);
			col.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			Destroy(gameObject);
		}
	}
}
