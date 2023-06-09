using EFT;
using UnityEngine;

public class DataSenderControl : MonoBehaviour
{
	private const KeyCode _E000 = KeyCode.F5;

	private ClientPlayer _E001;

	private ClientPlayer _E002
	{
		get
		{
			if (_E001 == null)
			{
				_E001 = Object.FindObjectOfType<ClientPlayer>();
			}
			return _E001;
		}
	}

	public static void Add()
	{
		_E39A.Add<DataSenderControl>();
		Debug.Log(_ED3E._E000(48237) + KeyCode.F5);
	}

	public static void Remove()
	{
		_E39A.Remove<DataSenderControl>();
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.F5))
		{
			ClientPlayer._E001 obj = (ClientPlayer._E001)_E002.GetDataSender();
			obj.PreventDispatch = !obj.PreventDispatch;
			Debug.LogFormat(_ED3E._E000(48268), obj.PreventDispatch);
		}
		_ = _E001 != null;
	}
}
