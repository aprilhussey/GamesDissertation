using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
	private enum CheckerColor
	{
		black,
		white
	}

	[SerializeField]
	private CheckerColor checkerColor;

	[SerializeField]
	private bool king;
}
