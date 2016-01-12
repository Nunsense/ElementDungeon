using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	public Transform camTransform;
	
	// How long the object should shake for.
	private float shakeTime = 0f;
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;
	
	Vector3 originalPos;

	void Awake() {
		if (camTransform == null) {
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable() {
		originalPos = camTransform.localPosition;
	}

	void Update() {
		if (shakeTime > 0) {
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			
			shakeTime -= Time.deltaTime * decreaseFactor;
		} else {
			shakeTime = 0f;
			camTransform.localPosition = originalPos;
			enabled = false;
		}
	}

	public void Action(float amount) {
	Debug.Log(amount);
		if (amount < 0.1f)
			return;

		shakeTime = 0.5f;
		shakeAmount = Mathf.Min(amount, 0.7f);
		enabled = true;
	}
}
