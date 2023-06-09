using System.Collections.Generic;
using UnityEngine;

public class TestRandomizer : MonoBehaviour
{
	private void Awake()
	{
		List<int> list = new List<int>(2) { 1, 2 };
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < 10000; i++)
		{
			if (list.RandomElement() == 1)
			{
				num++;
			}
			else
			{
				num2++;
			}
		}
		Debug.Log(_ED3E._E000(25413) + num + _ED3E._E000(25470) + num2);
	}
}
