using UnityEngine;
using System.Collections;
using UnityEditor;

//[CustomEditor(typeof(WorldManager))]
public class LevelMapEditor : Editor {
	int tempGridW, tempGridH;

	public WorldManager map;

	private GameObject selectedElement;

	private float cooldown;

	public void Awake() {
		map = (WorldManager)target;
	}

	public void OnEnable() {
		if (map.grid.elements == null || map.grid.elements.Length == 0) { 
			map.grid.w = 5;
			map.grid.h = 5;
			map.grid.elements = new Element[map.grid.w][];
			for (int i = 0; i < map.grid.elements.Length; i++) {
				map.grid.elements[i] = new Element[map.grid.h];
			}
		}
		tempGridW = map.grid.w;
		tempGridH = map.grid.h;
		SceneView.onSceneGUIDelegate = GridUpdate;
		
	}

	public void OnDisable() {
		SceneView.onSceneGUIDelegate -= GridUpdate;
		changeSelected(null);
	}

	public override void OnInspectorGUI() {
		GUILayout.BeginHorizontal();
		GUILayout.Label("Grid W, H");
		tempGridW = EditorGUILayout.IntField(tempGridW, GUILayout.Width(50));
		tempGridH = EditorGUILayout.IntField(tempGridH, GUILayout.Width(50));

		if (GUILayout.Button("Apply")) {
			Element[][] tempGrid = new Element[tempGridW][];

			for (int i = 0; i < tempGridW && i < map.grid.w; i++) {
				tempGrid[i] = new Element[tempGridH];
				for (int j = 0; j < tempGridH && j < map.grid.h; j++) {
					tempGrid[i][j] = map.grid.get(i, j);
				}
			}

			for (int i = 0; i < map.grid.w; i++) {
				for (int j = 0; j < map.grid.h; j++) {
					map.grid.set(i, j, null);
				}
			}
			map.grid.elements = tempGrid;
			map.grid.w = tempGridW;
			map.grid.h = tempGridH;
			EditorUtility.SetDirty(map);
		}
		GUILayout.EndHorizontal();

		if (GUILayout.Button("Clear")) {
			for (int i = 0; i < map.grid.w; i++) {
				for (int j = 0; j < map.grid.h; j++) {
					if (map.grid.get(i, j)) {
						GameObject.DestroyImmediate(map.grid.get(i, j).gameObject);
					}
					map.grid.set(i, j, null);
				}
			}
			map.grid.elements = new Element[map.grid.w][];
			for (int i = 0; i < map.grid.elements.Length; i++) {
				map.grid.elements[i] = new Element[map.grid.h];
			}
			EditorUtility.SetDirty(map);
		}
		DrawDefaultInspector();

		if (GUILayout.Button("Fire")) {
			changeSelected(map.firePrefav);
		}
		if (GUILayout.Button("Water")) {
			changeSelected(map.waterPrefav);
		}
		if (GUILayout.Button("Plant")) {
			changeSelected(map.plantPrefav);
		}
		if (GUILayout.Button("Stone")) {
			changeSelected(map.stonePrefav);
		}
		if (GUILayout.Button("Lighting")) {
			changeSelected(map.lightingPrefav);
		}
		if (GUILayout.Button("None")) {
			changeSelected(null);
		}
	}

	void GridUpdate(SceneView sceneview) {
		Event e = Event.current;
		Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
		Vector3 mousePos = r.origin;


		if (e.command) {
			cooldown = 0.2f;
			if (selectedElement != null) {
				map.SetElementAtWorldPos(selectedElement.GetComponent<Element>(), selectedElement.transform.position);									
				EditorUtility.SetDirty(map);
			}
		}

		if (selectedElement != null)
			selectedElement.transform.position = map.SnapToGrid(mousePos);
	}

	private void changeSelected(GameObject prefav) {
		if (selectedElement != null && selectedElement.transform.parent == null)
			DestroyImmediate(selectedElement);

		if (prefav == null)
			return;

		selectedElement = (GameObject.Instantiate(prefav) as GameObject);
	}
}
