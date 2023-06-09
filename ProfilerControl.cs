using UnityEngine;

public class ProfilerControl : MonoBehaviour
{
	private const KeyCode _E000 = KeyCode.F4;

	private int _E001;

	private const int _E002 = 300;

	public static void Add()
	{
		_E39A.Add<ProfilerControl>();
		Debug.Log(_ED3E._E000(53607) + KeyCode.F4);
		Debug.Log(_ED3E._E000(53636));
	}

	public static void Remove()
	{
		_E39A.Remove<ProfilerControl>();
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
		if (Input.GetKeyUp(KeyCode.F4))
		{
			_ECC9.enabled = !_ECC9.enabled;
		}
		if (Input.GetMouseButtonDown(2))
		{
			_E001 = Time.frameCount;
		}
		if (_E001 + 300 == Time.frameCount)
		{
			_ECC9.enabled = false;
		}
		_E372.DebugLogsEnabled(!_ECC9.enabled);
	}
}
