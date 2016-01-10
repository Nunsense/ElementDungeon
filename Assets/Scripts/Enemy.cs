using UnityEngine;
using System.Collections;

public class Enemy : Element {
	private bool isWandering;

	private Vector3 walkingTargetPos;
	private int walkingTargetGridX;
	private int walkingTargetGridY;
	private float walkingTime;

	private float maxRestTime = 5f;
	private float restingTime;

	private Element pickedUpObject;

	public float blockDistanceWalkTime = 0.2f;

	void Start() {
		isWandering = true;
		walkingTime = 0;
	}

	protected override void UpdateElement() {
		if (restingTime > 0) {
			restingTime -= Time.deltaTime;
			return;
		}

		if (isWandering) {
			if (walkingTargetPos != Vector3.zero) {
				if (world.GetElementAtGridPos(walkingTargetGridX, walkingTargetGridY) == null) {
					walkingTime += Time.deltaTime;
					if (walkingTime <= blockDistanceWalkTime) {
						transform.position = Vector3.Lerp(transform.position, walkingTargetPos, walkingTime / blockDistanceWalkTime);
					} else {
						world.SetElementAtGridPos(this, walkingTargetGridX, walkingTargetGridY);
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
					} else if (elem.GetElement() == ElementType.Plant || elem.GetElement() == ElementType.Stone) {
						world.PickUpElementAtGridPos((int)inFront.x, (int)inFront.y);
						elem.PickUp(gameObject);
					} else {
						RotateRandom();
						restingTime = Random.Range(0, maxRestTime);
					}
				}
			}
		}
	}

	public override bool CanPickUp() {
		return false;
	}

	public override ElementType GetElement() {
		return ElementType.Enemy;
	}
}
