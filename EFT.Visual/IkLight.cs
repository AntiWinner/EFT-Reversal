using System;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.Visual;

public class IkLight : MonoBehaviour
{
	[SerializeField]
	[CanBeNull]
	public Light Light;

	[CanBeNull]
	[SerializeField]
	public MultiFlareLight FlareLight;

	private float m__E000;

	private _E8A8 _E001 => _E8A8.Instance;

	private _E8A9 _E002 => _E001.OpticCameraManager;

	private void OnValidate()
	{
		if (!Application.isPlaying && Light != null)
		{
			Light.color = Color.red;
		}
	}

	private void Awake()
	{
		if (Light != null)
		{
			Light.color = Color.red;
			this.m__E000 = Light.intensity;
		}
	}

	private void OnEnable()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(_E000));
	}

	private void OnDisable()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(_E000));
	}

	private void _E000(Camera cam)
	{
		if (cam == null)
		{
			return;
		}
		bool flag = cam == _E001.Camera;
		bool flag2 = cam == _E002.Camera;
		if (flag || flag2)
		{
			bool flag3 = (flag ? _E001.NightVision.On : _E002.NightVision.On);
			if (Light != null)
			{
				Light.intensity = (flag3 ? this.m__E000 : 0f);
			}
			if (FlareLight != null)
			{
				FlareLight.CurrentAlpha *= (flag3 ? 1 : 0);
			}
		}
	}
}
