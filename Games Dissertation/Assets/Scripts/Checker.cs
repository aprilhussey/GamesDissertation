using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
	public enum CheckerColor
	{
		black,
		white
	}

	[SerializeField]
	private CheckerColor checkerColor;

	[SerializeField]
	private bool king;

    public CheckerColor GetCheckerColor
	{
		get { return checkerColor; }
	}

	public bool King
	{
		get { return king; }
		set { king = value; }
	}
}
