using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[_E2E2(19000)]
[ExecuteAlways]
public class WaterRendererv2 : MonoBehaviour
{
	private readonly _E42E m__E000 = new _E42E(_ED3E._E000(86011), CameraEvent.BeforeReflections);

	private List<WaterForSSRv2> m__E001 = new List<WaterForSSRv2>();

	private void OnEnable()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(_E002));
		WaterForSSRv2.OnAdd += _E001;
		WaterForSSRv2.OnRemove += _E003;
		_E000();
	}

	private void OnDisable()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(_E002));
		WaterForSSRv2.OnAdd -= _E001;
		WaterForSSRv2.OnRemove -= _E003;
	}

	private void _E000()
	{
		WaterForSSRv2[] array = _E3AA.FindUnityObjectsOfType<WaterForSSRv2>();
		foreach (WaterForSSRv2 waterForSSRv in array)
		{
			if (waterForSSRv.enabled && waterForSSRv.gameObject.activeSelf)
			{
				_E001(waterForSSRv);
			}
		}
	}

	private void _E001(WaterForSSRv2 water)
	{
		if (!this.m__E001.Contains(water))
		{
			this.m__E001.Add(water);
		}
		foreach (KeyValuePair<Camera, CommandBuffer> camera in this.m__E000.Cameras)
		{
			if (camera.Key != null && water.IsCorrectLayer(camera.Key.cullingMask))
			{
				water.InitBuffer(camera.Value, camera.Key);
			}
		}
	}

	private void _E002(Camera currentCamera)
	{
		if ((!currentCamera.CompareTag(_ED3E._E000(42407)) && !currentCamera.CompareTag(_ED3E._E000(42129)) && Application.isPlaying) || !this.m__E000.UpdateOnPreCullRender(out var buffer))
		{
			return;
		}
		buffer.Clear();
		for (int i = 0; i < this.m__E001.Count; i++)
		{
			if (this.m__E001[i].IsCorrectLayer(currentCamera.cullingMask))
			{
				this.m__E001[i].InitBuffer(buffer, currentCamera);
			}
		}
	}

	private void _E003(WaterForSSRv2 water)
	{
		if (this.m__E001.Contains(water))
		{
			this.m__E001.Remove(water);
		}
	}
}
