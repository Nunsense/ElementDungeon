using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	private Transform trans;
	private float shakeTime = 0f;

	void Awake() {
		if (trans == null) {
			trans = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void Update() {
		if (shakeTime > 0) {
			trans.localPosition = trans.localPosition + Random.insideUnitSphere * shakeAmount;
			
			shakeTime -= Time.deltaTime * decreaseFactor;
		} else {
			shakeTime = 0f;
			enabled = false;
		}
	}

	public void Action(float amount) {
		if (amount < 0.1f)
			return;

		shakeTime = 0.5f;
		shakeAmount = Mathf.Min(amount, 0.7f);
		enabled = true;
	}
}
