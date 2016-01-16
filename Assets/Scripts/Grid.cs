using UnityEngine;
using System.Collections;

[System.Serializable]
public class Grid
{
	public int w;
	public int h;

	public Element[][] elements;

	public void Set(int i, int j, Element elem) {
		elements[i][j] = elem;
	}

	public Element Get(int i, int j) {
		return elements[i][j];
	}

	public bool InGrid(int i, int j) {
		return jInGrid(i) && jInGrid(j);
	}

	public bool iInGrid(int i) {
		return i >= 0 && i < w;
	}

	public bool jInGrid(int j) {
		return j >= 0 && j < h;
	}
}