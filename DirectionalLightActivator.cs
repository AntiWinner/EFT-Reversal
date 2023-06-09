using UnityEngine;

public class DirectionalLightActivator : MonoBehaviour
{
	private Light[] m__E000;

	private void Awake()
	{
		this.m__E000 = GetComponentsInChildren<Light>();
		_E001(value: false);
		_E8A8.Instance.OnCameraChanged += _E000;
	}

	private void OnDestroy()
	{
		_E8A8.Instance.OnCameraChanged -= _E000;
	}

	private void _E000()
	{
		_E001(value: true);
	}

	private void _E001(bool value)
	{
		for (int i = 0; i < this.m__E000.Length; i++)
		{
			this.m__E000[i].enabled = value;
		}
	}
}
