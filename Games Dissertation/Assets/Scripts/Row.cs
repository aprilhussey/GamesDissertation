using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    private List<Tile> row = new List<Tile>();

	void Awake()
	{
		 foreach (Transform child in this.transform)
		{
			Tile tile = child.GetComponent<Tile>();

            if (tile != null)
			{ 
				row.Add(tile);
            }
        }
	}

	public List<Tile> GetRow
	{
		get { return row; }
	}
}
