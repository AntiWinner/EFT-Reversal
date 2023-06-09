using UnityEngine;

namespace EFT.UI;

public sealed class GamePauseControl : MonoBehaviour
{
	private const KeyCode _E000 = KeyCode.F2;

	public static void Add()
	{
		_E39A.Add<GamePauseControl>();
		Debug.Log(_ED3E._E000(250025) + KeyCode.F2);
	}

	public static void Remove()
	{
		_E39A.Remove<GamePauseControl>();
	}

	public static void Remove<T>() where T : MonoBehaviour
	{
		T val = _E3AA.FindUnityObjectOfType<T>();
		if ((Object)val != (Object)null)
		{
			Object.Destroy(val.gameObject);
		}
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.F2))
		{
			Debug.Break();
		}
	}
}
