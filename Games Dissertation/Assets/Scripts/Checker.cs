using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Checker : MonoBehaviour
{
	public enum CheckerColor
	{
		Black,
		White
	}

	[SerializeField]
	private CheckerColor checkerColor;

	[SerializeField]
	private bool king;

	private bool highlighted = false;

	public CheckerColor GetCheckerColor
	{
		get { return checkerColor; }
	}

	public bool King
	{
		get { return king; }
		set { king = value; }
	}

	public bool Highlighted
	{
		get { return highlighted; }
		set { highlighted = value; }
	}
}
