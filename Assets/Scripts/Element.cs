using UnityEngine;
using System.Collections;

public enum ElementType {
	Fire,
	Water,
	Stone,
	Plant,
	None

}

public class Element : MonoBehaviour {
	private int gridX;
	private int gridY;

	private ElementType type;
	private Transform pickUpOrigin;

	protected WorldManager world;

	static int[] rotations = new int[] { 0, 90, 180, 270 };

	void Awake() {
		world = GameObject.Find("World").GetComponent<WorldManager>();
		transform.eulerAngles = new Vector3(0, rotations[Random.Range(0, rotations.Length)], 0);
	}

	void Update() {
		if (CanPickUp() && pickUpOrigin != null) {
			transform.position = world.PositionInFrontPlayer();
		}

		UpdateElement();
	}

	public void PickUp(GameObject obj) {
		pickUpOrigin = obj.transform;
		Debug.Log(pickUpOrigin);
	}

	public void PutDown(int i, int j) {
		gridX = i;
		gridY = j;
		pickUpOrigin = null;
	}

	public int GetGridX() {
		return gridX;
	}

	public int GetGridY() {
		return gridY;
	}

	protected virtual void UpdateElement() {
	}

	public virtual void Action(int i, int j) {
	}

	public virtual ElementType GetElement() {
		return ElementType.None;
	}

	public virtual bool CanPickUp() {
		return false;
	}
}
