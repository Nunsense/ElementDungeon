using UnityEngine;
using System.Collections;

public class Monster : Element {
	public GameObject lights;

	private bool isWandering;
	private bool isDead;

	private Vector3 walkingTargetPos;
	private int walkingTargetGridX;
	private int walkingTargetGridY;
	private float walkingTime;

	private float maxRestTime = 5f;
	private float restingTime;

	private float maxDeadTime = 2f;
	private float dieingTime;

	private Element pickedUpObject;

	public float blockDistanceWalkTime = 0.2f;
	[SerializeField][Range(0, 1)] private float agroChance;
	[SerializeField] private float agroDistance;

	public MonsterVision vision;

	void Start() {
		isDead = false;
		isWandering = true;
		walkingTime = 0;
	}

	public void Kill() {
		dieingTime = maxDeadTime;
		isDead = true;
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Letal") {
			world.HitMonster(gameObject);
			GetComponent<Rigidbody>().AddForce((transform.position - col.transform.position) * 5000f);
		}
	}

	public void ElementIsNear(Element elem) {
		
	}

	protected override void UpdateElement() {
		if (isDead) {
			dieingTime -= Time.deltaTime;
			if (dieingTime <= 0) {
				world.KillMonster(gameObject);
			}

			return;
		}

		if (isWandering) {
			if (restingTime > 0) {
				restingTime -= Time.deltaTime;
				return;
			}

			if (walkingTargetPos != Vector3.zero) {
//				if (vision) {
//					for (int i = 0; i < vision.nearByLetal.Count; i++) {
//						Debug.Log((transform.position - vision.nearByLetal[i].gameObject.transform.position) * Time.deltaTime * 0.001f);
//						walkingTargetPos += (transform.position - vision.nearByLetal[i].gameObject.transform.position) * Time.deltaTime * 0.001f;
//					}
//				}
				if (world.GetElementAtGridPos(walkingTargetGridX, walkingTargetGridY) == null) {
					walkingTime += Time.deltaTime;
					if (walkingTime <= blockDistanceWalkTime) {
						transform.position = Vector3.Lerp(transform.position, walkingTargetPos, walkingTime / blockDistanceWalkTime);
					} else {
//						world.SetElementAtGridPos(this, walkingTargetGridX, walkingTargetGridY);
						walkingTime = 0;
						walkingTargetPos = Vector3.zero;
						restingTime = Random.Range(0, maxRestTime);
					}
				}
			} else {
				float rand = Random.value;
				if (rand < 0.3f) {
					RotateRandom();
					restingTime = Random.Range(0, maxRestTime);
				} else if (pickedUpObject != null && rand < 0.6f) {
					if (world.MoveElementAtWorldPos(pickedUpObject, pickedUpObject.transform.position))
						pickedUpObject = null;

					RotateRandom();
					restingTime = Random.Range(0, maxRestTime);
				} else {
					Vector2 inFront = world.PositionInFrontGridPos(transform);

					Element elem = world.GetElementAtGridPos((int)inFront.x, (int)inFront.y);
					if (world.GetElementAtGridPos((int)inFront.x, (int)inFront.y) == null) {
						walkingTargetGridX = (int)inFront.x;
						walkingTargetGridY = (int)inFront.y;
						walkingTargetPos = world.GridToWorldPos(walkingTargetGridX, walkingTargetGridY);
					} else if (elem.GetElementType() == ElementType.Plant || elem.GetElementType() == ElementType.Stone) {
						world.PickUpElementAtGridPos((int)inFront.x, (int)inFront.y);
						elem.PickUp(gameObject);
					} else {
						RotateRandom();
						restingTime = Random.Range(0, maxRestTime);
					}
				}


				rand = Random.value;
				if (rand < agroChance) {
					if (world.DistanceToPlayer(transform.position) < agroDistance) {
						isWandering = false;
						restingTime = 0;
					}
				}
			}
		} else {
			restingTime += Time.deltaTime;
			if (restingTime > 0.5f) {
				if (world.DistanceToPlayer(transform.position) > 5.8f) {
					isWandering = true;
					walkingTargetPos = Vector3.zero;
					RotateRandom();
					restingTime = Random.Range(0, maxRestTime);
					return;
				}
				restingTime = 0;
			}
			 
			transform.LookAt(world.PlayerTransform().position);
			transform.position = Vector3.Lerp(transform.position, world.PlayerTransform().position, Time.deltaTime);
		}
	}

	public override ElementType GetElementType() {
		return ElementType.Enemy;
	}

	public override bool InGrid() {
		return false;
	}

	public override void SetVisible(bool visible) {
		lights.SetActive(visible);
	}
}
