using System.Collections;
using UnityEngine;

public class WwwTest : MonoBehaviour
{
	[SerializeField]
	private int[] yolos = new int[10];

	[SerializeField]
	private int perframe = 3;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			StartCoroutine(_E000());
		}
	}

	private IEnumerator _E000()
	{
		Debug.Log(Time.frameCount);
		int num = 0;
		for (int i = 0; i < yolos.Length; i++)
		{
			int num2 = num + 1;
			num = num2;
			if (num2 == perframe)
			{
				num = 0;
				Debug.Log(Time.frameCount);
				yield return 0;
			}
		}
	}
}
