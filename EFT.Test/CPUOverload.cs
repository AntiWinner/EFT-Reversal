using System.Threading;
using UnityEngine;

namespace EFT.Test;

public class CPUOverload : MonoBehaviour
{
	private static GameObject _E000;

	private static int _E001;

	public static void SetOverload(int ms)
	{
		if (_E000 == null)
		{
			_E000 = new GameObject();
			_E000.AddComponent<CPUOverload>();
		}
		else if (_E000.GetComponent<CPUOverload>() == null)
		{
			_E000.AddComponent<CPUOverload>();
		}
		_E001 = ((ms > 0) ? ms : 0);
		_E000.SetActive(ms > 0);
	}

	private void Update()
	{
		if (_E001 > 0)
		{
			Thread.Sleep(_E001);
		}
	}
}
