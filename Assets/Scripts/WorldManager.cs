using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {
	public bool debug;

	public float blockW;

	[SerializeField] private float tickTime = 1f;
	private float nextTick;

	public Transform placementCube;
	public Transform player;

	public GameObject wallPrefav;
	public GameObject firePrefav;
	public GameObject waterPrefav;
	public GameObject stonePrefav;
	public GameObject plantPrefav;
	public GameObject soulPrefav;
	public GameObject icePrefav;

	public GameObject snowBallPrefav;

	public GameObject lightingPrefav;
	public GameObject enemyPrefav;

	public ParticleSystem mosnterExplosion;

	public Grid grid;

	void Start() {
		grid.elements = new Element[grid.w][];
		for (int i = 0; i < grid.w; i++) {
			grid.elements[i] = new Element[grid.h];
			for (int j = 0; j < grid.h; j++) {
				Element elem = null;
				if (i == 0 || i == grid.w - 1 || j == 0 || j == grid.h - 1) {
					elem = (GameObject.Instantiate(wallPrefav) as GameObject).GetComponent<Element>();
				} else {
					if (debug) {
						if (Random.value < 0.05) {
							float rand = Random.value;
							if (rand < 0.1) {
								elem = (GameObject.Instantiate(firePrefav) as GameObject).GetComponent<Element>();
							} else if (rand < 0.4) {
								elem = (GameObject.Instantiate(stonePrefav) as GameObject).GetComponent<Element>();
							} else if (rand < 0.5) {
								elem = (GameObject.Instantiate(waterPrefav) as GameObject).GetComponent<Element>();
							} else if (rand < 0.7) {
								elem = (GameObject.Instantiate(plantPrefav) as GameObject).GetComponent<Element>();
							} else if (rand < 0.8) {
								elem = (GameObject.Instantiate(enemyPrefav) as GameObject).GetComponent<Element>();
							} else if (rand < 0.9) {
								elem = (GameObject.Instantiate(lightingPrefav) as GameObject).GetComponent<Element>();
							} else {
								elem = (GameObject.Instantiate(soulPrefav) as GameObject).GetComponent<Element>();
							}
						}
					} else {
						if (Random.value < 0.01) {
							float rand = Random.value;
							if (rand < 0.2) {
								elem = (GameObject.Instantiate(stonePrefav) as GameObject).GetComponent<Element>();
							} else if (rand < 0.5) {
								elem = (GameObject.Instantiate(plantPrefav) as GameObject).GetComponent<Element>();
							} else if (rand < 0.8) {
								elem = (GameObject.Instantiate(waterPrefav) as GameObject).GetComponent<Element>();
							} else if (rand < 0.9) {
								elem = (GameObject.Instantiate(enemyPrefav) as GameObject).GetComponent<Element>();
							}
						}
					}
				}

					
				if (elem != null) {
					elem.transform.parent = transform;
					SetElementAtGridPos(elem, i, j);
				}
			}
		}

		if (!debug) {
			Element lighting = (GameObject.Instantiate(lightingPrefav) as GameObject).GetComponent<Element>();
			SetElementAtGridPos(lighting, 50, 60);
		}
	}

	void Update() {
		if (nextTick <= Time.time) {
			Tick();
			nextTick = Time.time + tickTime;
		}	

		placementCube.position = SnapToGrid(PositionInFrontPlayer());

		Vector2 pos = WorldToGridPos(player.position);
		int side = 15;
		float minX = pos.x - side, maxX = pos.x + side;
		float minY = pos.y - side, maxY = pos.y + side;
		Element elem;
		for (int i = 0; i < grid.w; i++) {
			for (int j = 0; j < grid.h; j++) {
				elem = grid.get(i, j);
				if (elem != null) {
					elem.SetVisible(i >= minX && i <= maxX && j >= minY && j <= maxY);
				}
			}
		}
	}

	void Tick() {
		Element elem;

		for (int i = 0; i < grid.w; i++) {
			for (int j = 0; j < grid.h; j++) {
				elem = grid.get(i, j);
				if (elem != null) {
					elem.Action(i, j);
				}
			}
		}
	}

	public Element PickUpObjectInFront(Transform trans) {
		Element elem = PickUpElementAtWorldPos(PositionInFrontWorldPos(trans));

		return elem != null && elem.CanPickUp() ? elem : null;
	}

	public Vector3 PositionInFrontWorldPos(Transform trans) {
		return trans.position + (trans.forward * blockW);
	}

	public Vector2 PositionInFrontGridPos(Transform trans) {
		Vector3 pos = PositionInFrontWorldPos(trans);
		int x = Mathf.RoundToInt(pos.x / blockW);
		int y = Mathf.RoundToInt(pos.z / blockW);
		return new Vector2(x, y);
	}

	public Vector3 PositionInFrontPlayer() {
		return PositionInFrontWorldPos(player);
	}

	public Element PickUpElementAtGridPos(int i, int j) {
		Element elem = grid.get(i, j);
		grid.set(i, j, null);

		return elem;
	}

	public Element PickUpElementAtWorldPos(Vector3 pos) {
		int x = Mathf.RoundToInt(pos.x / blockW);
		int y = Mathf.RoundToInt(pos.z / blockW);

		return PickUpElementAtGridPos(x, y);
	}

	public Element GetElementAtGridPos(int i, int j) {
		return grid.get(i, j);
	}

	public Element GetElementAtWorldPos(Vector3 pos) {
		int x = Mathf.RoundToInt(pos.x / blockW);
		int y = Mathf.RoundToInt(pos.z / blockW);

		return GetElementAtGridPos(x, y);
	}

	public bool MoveElementAtWorldPos(Element elem, Vector3 pos) {
		int x = Mathf.RoundToInt(pos.x / blockW);
		int y = Mathf.RoundToInt(pos.z / blockW);
		return MoveElementAtGridPos(elem, x, y);
	}

	public bool MoveElementAtGridPos(Element newElem, int i, int j) {
		if (grid.get(i, j) != null) {
			Element currentElem = grid.get(i, j);
			switch (currentElem.GetElementType()) {
			case ElementType.Fire:
				switch (newElem.GetElementType()) {
				case ElementType.Water:
					Destroy(currentElem.gameObject);
					Destroy(newElem.gameObject);
					grid.set(i, j, null);
					break;
				case ElementType.Stone:
					SetElementAtGridPos(newElem, i, j);
					break;
				case ElementType.Ice:
					Destroy(currentElem.gameObject);
					Destroy(newElem.gameObject);
					CreateElementAtGridPos(ElementType.Water, i, j);
					break;
				default : 
					Destroy(newElem.gameObject);
					break;
				}
				return true;
			case ElementType.Water:
				switch (newElem.GetElementType()) {
				case ElementType.Fire:
					Destroy(currentElem.gameObject);
					Destroy(newElem.gameObject);
					grid.set(i, j, null);
					break;
				case ElementType.Water:
					Destroy(newElem.gameObject);
					break;
				case ElementType.Stone:
					SetElementAtGridPos(newElem, i, j);
					break;
				case ElementType.Plant:
					SetElementAtGridPos(newElem, i, j);
					break;
				}
				return true;
			case ElementType.Ice:
				switch (newElem.GetElementType()) {
				case ElementType.Fire:
					Destroy(currentElem.gameObject);
					Destroy(newElem.gameObject);
					CreateElementAtGridPos(ElementType.Water, i, j);
					break;
				case ElementType.Water:
					Destroy(newElem.gameObject);
					break;
				case ElementType.Stone:
					Destroy(currentElem.gameObject);
					break;
				case ElementType.Plant:
					Destroy(newElem.gameObject);
					break;
				case ElementType.Ice:
					Destroy(newElem.gameObject);
					break;
				}
				return true;
			case ElementType.Plant:
				switch (newElem.GetElementType()) {
				case ElementType.Water:
					Destroy(newElem.gameObject);
					return true;
				case ElementType.Fire:
					SetElementAtGridPos(newElem, i, j);
					return true;
				case ElementType.Stone:
					SetElementAtGridPos(newElem, i, j);
					return true;
				case ElementType.Plant:
					return false;
				}
				break;
			case ElementType.Stone:
				switch (newElem.GetElementType()) {
				case ElementType.Fire:
					Destroy(newElem.gameObject);
					return true;
				case ElementType.Water:
					Destroy(newElem.gameObject);
					return true;
				case ElementType.Stone:
					return false;
				case ElementType.Plant:
					return false;
				}
				break;
			case ElementType.Soul:
				switch (newElem.GetElementType()) {
				case ElementType.Stone:
					CreateSquareOfElement(ElementType.Stone, i, j, 5, false);
					Destroy(newElem.gameObject);
					break;
				case ElementType.Water:
					Destroy(newElem.gameObject);
					CreatePoolOfElement(ElementType.Ice, i, j, 3, true);
					break;
				}
				Destroy(currentElem.gameObject);
				return true;
			}
		}

		SetElementAtGridPos(newElem, i, j);
		return true;
	}

	private void CreateSquareOfElement(ElementType type, int i, int j, int r, bool filled) {
		for (int ii = -r; ii <= r; ii++) {
			for (int jj = -r; jj <= r; jj++) {
				if (!filled && ii != -r && ii != r && jj != -r && jj != r)
					continue;

				int jjj = j + jj;
				int iii = i + ii;

				if (InMap(iii, jjj)) {
					CreateElementAtGridPos(type, iii, jjj);
				}
			}	
		}
	}

	private void CreatePoolOfElement(ElementType type, int oi, int oj, int dist, bool noise) {
		if (dist <= 0)
			return;

		CreateElementAtGridPos(type, oi, oj);

		for (int ii = oi - 1; ii <= oi + 1; ii++) {
			if (!iInMap(ii))
				continue;

			for (int jj = oj - 1; jj <= oj + 1; jj++) {
				if (!jInMap(jj))
					continue;

				CreatePoolOfElement(type, ii, jj, dist - (noise ? Random.Range(1, 4) : 1), noise);
			}	
		}
	}

	private void CreateSnowBalls(int i, int j) {
		for (int m = 0; m < Random.Range(2, 5); m++) {
			GameObject soul = GameObject.Instantiate(snowBallPrefav);
			soul.transform.parent = transform;

			Vector3 diff = Vector3.forward * Random.Range(-1f, 1f) + Vector3.right * Random.Range(-1f, 1f) + Vector3.up * Random.Range(0.2f, 2);
			soul.transform.position = GridToWorldPos(i, j) + diff;
		}
	}

	public void SetElementAtWorldPos(Element elem, Vector3 pos) {
		int x = Mathf.RoundToInt(pos.x / blockW);
		int y = Mathf.RoundToInt(pos.z / blockW);
		SetElementAtGridPos(elem, x, y);
	}

	public void SetElementAtGridPos(Element elem, int i, int j) {
		if (grid.get(i, j) != null)
			Destroy(grid.get(i, j).gameObject);
		if (elem.InGrid())
			grid.set(i, j, elem);

		elem.PutDown(i, j);
		elem.transform.position = GridToWorldPos(i, j);
	}

	public Vector3 GridToWorldPos(int i, int j) {
		return new Vector3(i * blockW, 0, j * blockW);
	}

	public Vector2 WorldToGridPos(Vector3 pos) {
		int x = Mathf.RoundToInt(pos.x / blockW);
		int y = Mathf.RoundToInt(pos.z / blockW);

		return new Vector2(x, y);
	}

	public Vector3 SnapToGrid(Vector3 pos) {
		int x = Mathf.RoundToInt(pos.x / blockW);
		int y = Mathf.RoundToInt(pos.z / blockW);

		return new Vector3(x * blockW, 0, y * blockW);
	}

	public bool CreateElementArround(ElementType type, int ii, int jj) {
		for (int i = ii - 1; i <= ii + 1; i++) {
			if (!iInMap(i))
				continue;

			for (int j = jj - 1; j <= jj + 1; j++) {
				if (!jInMap(j))
					continue;

				Element elem = null;
				if (grid.get(i, j) == null) {
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
					return true;
				}
			}
		}
		return false;
	}

	public Element CreateElementAtGridPos(ElementType type, int i, int j) {
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
		case ElementType.Soul:
			elem = (GameObject.Instantiate(soulPrefav) as GameObject).GetComponent<Element>();
			break;
		case ElementType.Ice:
			elem = (GameObject.Instantiate(icePrefav) as GameObject).GetComponent<Element>();
			break;
		}
					

		if (elem != null) {
			MoveElementAtGridPos(elem, i, j);
		}

		return grid.get(i, j);
	}

	public bool IsElementClose(ElementType type, int ii, int jj, int r) {
		for (int i = ii - r; i < ii + r; i++) {
			if (!iInMap(i))
				continue;

			for (int j = jj - r; j < jj + r; j++) {
				if (!jInMap(j))
					continue;

				if (grid.get(i, j) != null && grid.get(i, j).GetElementType() == type)
					return true;
			}
		}
		return false;
	}

	public Element [][] GetElementsNearBy(int ii, int jj, int r) {
		int iii = 0, jjj = 0;
		Element[][] nearBys = new Element[(r * 2) + 1] [];
		for (int i = ii - r; i <= ii + r; i++) {
			nearBys[iii] = new Element[(r * 2) + 1];
			if (iInMap(i)) {
				for (int j = jj - r; j <= jj + r; j++) {
					if (jInMap(j)) {
						nearBys[iii][jjj] = grid.get(i, j);
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
		return i >= 0 && i < grid.w;
	}

	public bool jInMap(int j) {
		return j >= 0 && j < grid.h;
	}

	public void Throw(ThrowableObject obj, Transform origin) {
		obj.Throw(this);
		obj.transform.position = origin.position + origin.forward * 2 + origin.up;
		obj.gameObject.SetActive(true);
		obj.GetComponent<Rigidbody>().AddForce(origin.forward * 500f);
	}

	public void HitMonster(GameObject go) {
		mosnterExplosion.transform.position = go.transform.position;
		mosnterExplosion.Play();
		go.transform.rotation = Random.rotation;
		go.GetComponent<Monster>().Kill();
	}

	public void KillMonster(GameObject go) {
		Vector2 pos = WorldToGridPos(go.transform.position);
		Destroy(go);
		CreateElementAtGridPos(ElementType.Soul, (int)pos.x, (int)pos.y);
	}

	public float DistanceToPlayer(Vector3 position) {
		return Vector3.Distance(player.position, position);
	}

	public Transform PlayerTransform() {
		return player.transform;
	}
//
//	void OnDrawGizmos ()
//	{
//		Vector3 pos = transform.position;
//
//		float initY = pos.y;
//		float totalH = -(((grid.h - 1) * blockW) - initY);
//
//		float initX = pos.x;
//		float totalW = initX + ((grid.w - 1) * blockW);
//
//		for (float x = initX; x <= totalW + 1; x += blockW) {
//			Gizmos.DrawLine (new Vector3 (x, 0.0f, initY), new Vector3 (x, 0.0f, totalH));
//		}
//		for (float y = initY; y > totalH; y -= blockW) {
//			Gizmos.DrawLine (new Vector3 (initX, 0.0f, y), new Vector3 (totalW, 0.0f, y));	
//		}
//	    	
//	}
}
