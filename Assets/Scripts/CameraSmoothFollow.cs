using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour {
	public Transform target;
	public float factor = 1;
	private Vector3 posOffset;

	void Awake () {
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void Start () {
		posOffset = target.position - transform.position;
	}

	void Update () {
		transform.position = Vector3.Lerp (transform.position, target.position - posOffset, Time.deltaTime * factor);
		Quaternion rotTo = Quaternion.LookRotation (target.position - transform.position);
		rotTo.y = 0;
		rotTo.z = 0;
		transform.rotation = Quaternion.Lerp (transform.rotation, rotTo, Time.deltaTime * factor);
	}
}
