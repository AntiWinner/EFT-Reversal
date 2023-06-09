using UnityEngine;

namespace EFT;

public class AudioListenerConsistencyManager : MonoBehaviour
{
	private Transform _E000;

	private Transform _E001;

	private readonly Vector3 _E002 = new Vector3(0f, -1000f, 0f);

	private void Awake()
	{
		_E001 = base.transform;
		Reset();
	}

	public void Follow(Transform target)
	{
		_E000 = target;
		if (_E000 == null)
		{
			Reset();
		}
	}

	private void Update()
	{
		if (_E000 != null)
		{
			_E001.SetPositionAndRotation(_E000.position, _E000.rotation);
		}
	}

	public void Reset()
	{
		_E000 = null;
		if (_E001 != null)
		{
			_E001.SetPositionAndRotation(_E002, Quaternion.identity);
		}
	}
}
