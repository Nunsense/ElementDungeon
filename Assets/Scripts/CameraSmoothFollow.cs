using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour {
	public Transform target;
	public float followFactor = 1;
	public float rotationFactor = 0.2f;
	private Vector3 posOffset;

	void Awake () {
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void Start () {
		posOffset = target.position - transform.position;
	}

	void Update () {
		transform.position = Vector3.Lerp (transform.position, target.position - posOffset, Time.deltaTime * followFactor);
//		Quaternion rotTo = Quaternion.LookRotation (target.position);
//		rotTo.y = 0;
//		rotTo.z = 0;
//		transform.rotation = Quaternion.Lerp (transform.rotation, rotTo, Time.deltaTime * rotationFactor);
	}
}
