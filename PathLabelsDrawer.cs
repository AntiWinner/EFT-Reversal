using System.Collections.Generic;
using UnityEngine;

public class PathLabelsDrawer : MonoBehaviour
{
	private static List<Corner> _E000 = new List<Corner>();

	public static void SetPoints(List<Corner> corners)
	{
		_E000 = corners;
	}
}
