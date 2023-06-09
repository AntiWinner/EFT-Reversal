using UnityEngine;

namespace GPUInstancer;

[RequireComponent(typeof(Camera))]
public class GPUInstancerCameraHandler : MonoBehaviour
{
	private Camera _E000;

	private bool _E001;

	private void OnEnable()
	{
		if (!_E000)
		{
			_E000 = GetComponent<Camera>();
		}
		_E001 = false;
	}

	private void OnDisable()
	{
		_E001 = false;
	}

	private void Update()
	{
		if (_E000.isActiveAndEnabled && !_E001)
		{
			_E4BD.SetCamera(_E000);
			_E001 = true;
		}
		else if (!_E000.isActiveAndEnabled && _E001)
		{
			_E001 = false;
		}
	}
}
