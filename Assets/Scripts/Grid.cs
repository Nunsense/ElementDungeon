using UnityEngine;
using System.Collections;

[System.Serializable]
public class Grid
{
	public int w;
	public int h;

	public Element[][] elements;

	public void set(int i, int j, Element elem) {
		elements[i][j] = elem;
	}

	public Element get(int i, int j) {
		return elements[i][j];
	}
}