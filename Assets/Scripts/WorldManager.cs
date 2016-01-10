﻿using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {
	[SerializeField] private int worldW;
	[SerializeField] private int worldH;
	[SerializeField] private float blockW;

	[SerializeField] private float tickTime = 1f;
	private float nextTick;

	public Transform placementCube;
	public Transform player;

	public GameObject wallPrefav;
	public GameObject firePrefav;
	public GameObject waterPrefav;
	public GameObject stonePrefav;
	public GameObject plantPrefav;

	private Element[][] world;

	void Start() {
		world = new Element[worldW][];
		for (int i = 0; i < worldW; i++) {
			world[i] = new Element[worldH];
			for (int j = 0; j < worldH; j++) {
				Element elem = null;
				if (i == 0 || i == worldW - 1 || j == 0 || j == worldH - 1) {
					elem = (GameObject.Instantiate(wallPrefav) as GameObject).GetComponent<Element>();
				} else {
					if (Random.value < 0.01) {
						float rand = Random.value;
						if (rand < 0.1) {
							elem = (GameObject.Instantiate(firePrefav) as GameObject).GetComponent<Element>();
						} else if (rand < 0.4) {
							elem = (GameObject.Instantiate(stonePrefav) as GameObject).GetComponent<Element>();
						} else if (rand < 0.5) {
							elem = (GameObject.Instantiate(waterPrefav) as GameObject).GetComponent<Element>();
						} else if (rand < 0.7) {
							elem = (GameObject.Instantiate(plantPrefav) as GameObject).GetComponent<Element>();
						}
					}
				}

				if (elem != null) {
					elem.transform.parent = transform;
					SetElementAtGridPos(elem, i, j);
				}
			}
		}
	}

	void Update() {
		if (nextTick <= Time.time) {
			Tick();
			nextTick = Time.time + tickTime;
		}	

		placementCube.position = SnapToGrid(PositionInFrontPlayer());
	}

	void Tick() {
		Element elem;

		for (int i = 0; i < worldW; i++) {
			for (int j = 0; j < worldH; j++) {
				elem = world[i][j];
				if (elem != null) {
					elem.Action(i, j);
				}
			}
		}
	}

	public Element PickUpObjectInFront(Transform trans) {
		Element elem = PickUpElementAtWorldPos(PositionInFront(trans));

		return elem != null && elem.CanPickUp() ? elem : null;
	}

	public Vector3 PositionInFront(Transform trans) {
		return trans.position + (trans.forward * blockW);
	}

	public Vector3 PositionInFrontPlayer() {
		return PositionInFront(player);
	}

	public Element PickUpElementAtWorldPos(Vector3 pos) {
		int x = Mathf.RoundToInt(pos.x / blockW);
		int y = Mathf.RoundToInt(pos.z / blockW);

		Element elem = world[x][y];
		world[x][y] = null;

		return elem;
	}

	public Element GetElementAtWorldPos(Vector3 pos) {
		int x = Mathf.RoundToInt(pos.x / blockW);
		int y = Mathf.RoundToInt(pos.z / blockW);

		return world[x][y];
	}

	public bool MoveElementAtWorldPos(Element elem, Vector3 pos) {
		int x = Mathf.RoundToInt(pos.x / blockW);
		int y = Mathf.RoundToInt(pos.z / blockW);
		return MoveElementAtGridPos(elem, x, y);
	}

	public bool MoveElementAtGridPos(Element elem, int i, int j) {
		if (world[i][j] != null) {
			Element other = world[i][j];
			switch (other.GetElement()) {
			case ElementType.Fire:
				switch (elem.GetElement()) {
				case ElementType.Water:
					Destroy(other.gameObject);
					Destroy(elem.gameObject);
					world[i][j] = null;
					break;
				case ElementType.Stone:
					SetElementAtGridPos(elem, i, j);
					break;
				default : 
					Destroy(elem.gameObject);
					break;
				}
				return true;
			case ElementType.Water:
				switch (elem.GetElement()) {
				case ElementType.Fire:
					Destroy(other.gameObject);
					Destroy(elem.gameObject);
					world[i][j] = null;
					break;
				case ElementType.Water:
					Destroy(elem.gameObject);
					break;
				case ElementType.Stone:
					SetElementAtGridPos(elem, i, j);
					break;
				case ElementType.Plant:
					SetElementAtGridPos(elem, i, j);
					break;
				}
				return true;
			case ElementType.Plant:
				switch (elem.GetElement()) {
				case ElementType.Water:
					Destroy(elem.gameObject);
					return true;
				case ElementType.Fire:
					SetElementAtGridPos(elem, i, j);
					return true;
				case ElementType.Stone:
					SetElementAtGridPos(elem, i, j);
					return true;
				case ElementType.Plant:
					return false;
				}
				break;
			case ElementType.Stone:
				switch (elem.GetElement()) {
				case ElementType.Fire:
					Destroy(elem.gameObject);
					return true;
				case ElementType.Water:
					Destroy(elem.gameObject);
					return true;
				case ElementType.Stone:
					return false;
				case ElementType.Plant:
					return false;
				}
				break;
			}
		}

		SetElementAtGridPos(elem, i, j);
		return true;
	}

	public void SetElementAtWorldPos(Element elem, Vector3 pos) {
		int x = Mathf.RoundToInt(pos.x / blockW);
		int y = Mathf.RoundToInt(pos.z / blockW);
		SetElementAtGridPos(elem, x, y);
	}

	public void SetElementAtGridPos(Element elem, int i, int j) {
		if (world[i][j] != null)
			Destroy(world[i][j].gameObject);
		world[i][j] = elem;
		elem.PutDown(i, j);
		elem.transform.position = new Vector3(i * blockW, 0, j * blockW);
	}


	public Vector3 SnapToGrid(Vector3 pos) {
		int x = Mathf.RoundToInt(pos.x / blockW);
		int y = Mathf.RoundToInt(pos.z / blockW);

		return new Vector3(x * blockW, 0, y * blockW);
	}

	public bool CreateElementArround(ElementType type, int ii, int jj) {
		for (int i = ii - 1; i < ii + 1; i++) {
			if (!iInMap(i))
				continue;

			for (int j = jj - 1; j < jj + 1; j++) {
				if (!jInMap(j))
					continue;

				Element elem = null;
				if (world[i][j] == null) {
					switch (type) {
					case ElementType.Fire:
						elem = (GameObject.Instantiate(firePrefav) as GameObject).GetComponent<Element>();
						break;
					case ElementType.Plant:
						elem = (GameObject.Instantiate(plantPrefav) as GameObject).GetComponent<Element>();
						break;
					case ElementType.Water:
						elem = (GameObject.Instantiate(waterPrefav) as GameObject).GetComponent<Element>();
						break;
					case ElementType.Stone:
						elem = (GameObject.Instantiate(stonePrefav) as GameObject).GetComponent<Element>();
						break;
					}
					return true;
				}

				if (elem != null) {
					elem.transform.parent = transform;
					SetElementAtGridPos(elem, i, j);
				}
			}
		}
		return false;
	}

	public Element CreateElementAtGridPos(ElementType type, int i, int j) {
		if (world[i][j] != null) {
			Destroy(world[i][j].gameObject);
			world[i][j] = null;
		}

		Element elem = null;
		switch (type) {
		case ElementType.Fire:
			elem = (GameObject.Instantiate(firePrefav) as GameObject).GetComponent<Element>();
			break;
		case ElementType.Plant:
			elem = (GameObject.Instantiate(plantPrefav) as GameObject).GetComponent<Element>();
			break;
		case ElementType.Water:
			elem = (GameObject.Instantiate(waterPrefav) as GameObject).GetComponent<Element>();
			break;
		case ElementType.Stone:
			elem = (GameObject.Instantiate(stonePrefav) as GameObject).GetComponent<Element>();
			break;
		}
					

		if (elem != null) {
			elem.transform.parent = transform;
			SetElementAtGridPos(elem, i, j);
		}

		return world[i][j];
	}

	public bool IsElementClose(ElementType type, int ii, int jj, int r) {
		for (int i = ii - r; i < ii + r; i++) {
			if (!iInMap(i))
				continue;

			for (int j = jj - r; j < jj + r; j++) {
				if (!jInMap(j))
					continue;

				if (world[i][j] != null && world[i][j].GetElement() == type)
					return true;
			}
		}
		return false;
	}

	public Element [][] GetElementsNearBy(int ii, int jj, int r) {
		int iii = 0, jjj = 0;
		Element[][] nearBys = new Element[(r * 2) + 1] [];
		for (int i = ii - r; i < ii + r; i++) {
			nearBys[iii] = new Element[(r * 2) + 1];
			if (iInMap(i)) {
				for (int j = jj - r; j < jj + r; j++) {
					if (jInMap(j)) {
						nearBys[iii][jjj] = world[i][j];
					}
					jjj++;
				}
				jjj = 0;
			}
			iii++;
		}
		return nearBys;
	}

	public bool InMap(int i, int j) {
		return jInMap(i) && jInMap(j);
	}

	public bool iInMap(int i) {
		return i >= 0 && i < worldW;
	}

	public bool jInMap(int j) {
		return j >= 0 && j < worldH;
	}
}
