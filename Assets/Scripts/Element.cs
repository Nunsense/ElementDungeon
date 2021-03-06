﻿using UnityEngine;
using System.Collections;

public enum ElementType {
	Fire,
	Water,
	Stone,
	Plant,
	None,
	Enemy,
	Soul,
	Ice,
	Lighting,
	Life
}

public class Element : MonoBehaviour {
	protected int gridX;
	protected int gridY;

	private ElementType type;
	private Transform pickUpOrigin;

	protected WorldManager world;

	protected Renderer ren;

	public bool canReact;

	static int[] rotations = new int[] { 0, 90, 180, 270 };

	void Awake() {
		world = GameObject.Find("World").GetComponent<WorldManager>();
		ren = GetComponentInChildren<Renderer>();
		RotateRandom();
		canReact = true;
	}

	void Update() {
		if (CanPickUp() && pickUpOrigin != null) {
			transform.position = world.PositionInFrontWorldPos(pickUpOrigin.transform);
		}

		UpdateElement();
	}

	public void PickUp(GameObject obj) {
		pickUpOrigin = obj.transform;
	}

	public void PutDown(int i, int j) {
		gridX = i;
		gridY = j;
		pickUpOrigin = null;
		canReact = false;
	}

	public int GetGridX() {
		return gridX;
	}

	public int GetGridY() {
		return gridY;
	}

	public void Action(int i, int j) {
		if (canReact)
			ActionElement(i, j);
		else
			canReact = true;
	}

	protected void RotateRandom() {
		transform.eulerAngles = new Vector3(0, rotations[Random.Range(0, rotations.Length)], 0);
	}

	protected virtual void UpdateElement() {
	}

	protected virtual void ActionElement(int i, int j) {
	}

	public virtual ElementType GetElementType() {
		return ElementType.None;
	}

	public virtual bool CanPickUp() {
		return false;
	}

	public virtual void SetVisible(bool visible) {
	}

	public virtual bool InGrid() {
		return true;
	}
}
