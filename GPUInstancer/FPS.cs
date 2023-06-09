using System.Collections;
using UnityEngine;

namespace GPUInstancer;

public class FPS : MonoBehaviour
{
	public float FPSCount;

	private IEnumerator Start()
	{
		while (true)
		{
			if (Time.timeScale == 1f)
			{
				yield return new WaitForSeconds(0.1f);
				FPSCount = Mathf.Round(1f / Time.deltaTime);
			}
			else
			{
				FPSCount = 0f;
			}
			yield return new WaitForSeconds(0.5f);
		}
	}
}
