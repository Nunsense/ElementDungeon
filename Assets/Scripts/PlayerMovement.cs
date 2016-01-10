using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float speed = 6.0F;
	public float turnSpeed = 25.0F;
	private Vector3 moveDirection = Vector3.zero;

	void Update () {
		moveDirection = transform.forward * Input.GetAxis ("Vertical");
		moveDirection *= speed;

		transform.Rotate (new Vector3 (0, Input.GetAxis ("Horizontal") * turnSpeed * Time.deltaTime, 0));
		transform.position = transform.position + moveDirection * Time.deltaTime;
	}

	bool isGraunded () {
		return transform.position.y < 0.6f;
	}
}
