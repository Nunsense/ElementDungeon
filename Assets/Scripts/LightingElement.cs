using UnityEngine;
using System.Collections;

public class LightingElement : Element {

	private float initialY;
	private bool falling;
	private Vector3 upPosition;
	private Vector3 downPosition;

	[SerializeField] private float cooldownMax = 2f;
	[SerializeField] private float cooldownMin = 5f;
	private float timeTillNext;
	[SerializeField] private float fallingTime = 0.1f;
	private float timeSpent;

	private CameraShake cameraShake;

	public ParticleSystem particles;
	public Light lights;
	public Transform ray;

	void Start() {
		cameraShake = Camera.main.GetComponent<CameraShake>();
		lights.enabled = false;
		falling = false;
		downPosition = ray.position;
		upPosition = ray.position;
		upPosition.y = 10;
	}

	protected override void UpdateElement() {
		if (falling) {
			timeSpent += Time.deltaTime;

			ray.position = Vector3.Lerp(ray.position, downPosition, timeSpent / fallingTime);

			lights.intensity = Random.Range(0, 8);
			ray.eulerAngles = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5));

			if (timeSpent >= fallingTime) {
//				particles.Play();
				lights.enabled = false;
				ray.position = upPosition;
				ray.gameObject.SetActive(false);
				falling = false;
				timeTillNext = Random.Range(cooldownMin, cooldownMax);
			}

			return;
		}

		timeTillNext -= Time.deltaTime;
		if (timeTillNext <= 0) {
			timeSpent = 0;
			falling = true;
			cameraShake.Action(1 - (world.DistanceToPlayer(transform.position) * 0.06f));
			lights.enabled = true;
			ray.gameObject.SetActive(true);
		}
	}

	public override void SetVisible(bool visible) {
		lights.enabled = visible;
		particles.gameObject.SetActive(visible);
	}

	public override ElementType GetElementType() {
		return ElementType.Lighting;
	}

	public override bool InGrid() {
		return false;
	}
}
