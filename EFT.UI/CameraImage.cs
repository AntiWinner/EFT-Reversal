using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace EFT.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RawImage))]
public sealed class CameraImage : MonoBehaviour
{
	private struct _E000
	{
		public Color AmbientEquatorColor;

		public Color AmbientGroundColor;

		public float AmbientIntensity;

		public Color AmbientLight;

		public AmbientMode AmbientMode;

		public Color AmbientSkyColor;

		public bool Fog;

		public static _E000 Store()
		{
			_E000 result = default(_E000);
			result.AmbientEquatorColor = RenderSettings.ambientEquatorColor;
			result.AmbientGroundColor = RenderSettings.ambientGroundColor;
			result.AmbientIntensity = RenderSettings.ambientIntensity;
			result.AmbientLight = RenderSettings.ambientLight;
			result.AmbientMode = RenderSettings.ambientMode;
			result.AmbientSkyColor = RenderSettings.ambientSkyColor;
			result.Fog = RenderSettings.fog;
			return result;
		}

		public static void Reset()
		{
			RenderSettings.ambientEquatorColor = Color.black;
			RenderSettings.ambientGroundColor = Color.black;
			RenderSettings.ambientIntensity = 0f;
			RenderSettings.ambientLight = Color.black;
			RenderSettings.ambientMode = AmbientMode.Flat;
			RenderSettings.ambientSkyColor = Color.black;
			RenderSettings.fog = false;
		}

		public void Restore()
		{
			RenderSettings.ambientEquatorColor = AmbientEquatorColor;
			RenderSettings.ambientGroundColor = AmbientGroundColor;
			RenderSettings.ambientIntensity = AmbientIntensity;
			RenderSettings.ambientLight = AmbientLight;
			RenderSettings.ambientMode = AmbientMode;
			RenderSettings.ambientSkyColor = AmbientSkyColor;
			RenderSettings.fog = Fog;
		}
	}

	public Camera targetCamera;

	private RenderTexture m__E000;

	public int TextureDepth = 24;

	public RenderTextureFormat TextureFormat;

	public RenderTextureReadWrite RenderTextureReadWrite;

	public int Antialiasing = 1;

	private int m__E001;

	private int m__E002;

	private RawImage m__E003;

	private _E000 _E004;

	private void Awake()
	{
		this.m__E003 = GetComponent<RawImage>();
	}

	private void OnEnable()
	{
		_E000();
	}

	public void InitCamera([CanBeNull] Camera cam)
	{
		CleanUp();
		targetCamera = cam;
		_E000();
	}

	private void _E000()
	{
		if (!base.gameObject.activeInHierarchy || targetCamera == null)
		{
			return;
		}
		CleanUp();
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(_E001));
		Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(_E003));
		Rect rect = GetComponent<RectTransform>().rect;
		this.m__E001 = (int)rect.width;
		this.m__E002 = (int)rect.height;
		if (this.m__E001 != 0 && this.m__E002 != 0)
		{
			targetCamera.enabled = true;
			targetCamera.renderingPath = RenderingPath.DeferredShading;
			if (this.m__E000 != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m__E000);
			}
			this.m__E000 = new RenderTexture(this.m__E001 * 2, this.m__E002 * 2, TextureDepth, TextureFormat, RenderTextureReadWrite)
			{
				name = _ED3E._E000(255761),
				antiAliasing = Antialiasing,
				anisoLevel = 4
			};
			targetCamera.targetTexture = this.m__E000;
			this.m__E003.texture = this.m__E000;
			this.m__E003.enabled = true;
		}
	}

	public void SetRawImageColor(Color32 color)
	{
		GetComponent<RawImage>().color = color;
	}

	public void CleanUp()
	{
		if (targetCamera != null)
		{
			targetCamera.enabled = false;
			targetCamera.targetTexture = null;
		}
		if (this.m__E003 != null)
		{
			this.m__E003.enabled = false;
			this.m__E003.texture = null;
		}
		if (this.m__E000 != null)
		{
			this.m__E000.Release();
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(this.m__E000);
			}
		}
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPreRender, new Camera.CameraCallback(_E001));
		Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(_E003));
		this.m__E000 = null;
	}

	private void Update()
	{
		Rect rect = GetComponent<RectTransform>().rect;
		int num = (int)rect.width;
		int num2 = (int)rect.height;
		if (num != this.m__E001 || num2 != this.m__E002)
		{
			_E000();
		}
	}

	private void OnDisable()
	{
		CleanUp();
	}

	private void OnDestroy()
	{
		CleanUp();
	}

	private void _E001(Camera cam)
	{
		if (!(cam != targetCamera))
		{
			_E004 = CameraImage._E000.Store();
			_E002();
		}
	}

	private void _E002()
	{
		RenderSettings.ambientIntensity = 1f;
		RenderSettings.ambientMode = AmbientMode.Flat;
		RenderSettings.ambientLight = Color.black;
		RenderSettings.ambientSkyColor = Color.black;
	}

	private void _E003(Camera cam)
	{
		if (!(cam != targetCamera))
		{
			_E004.Restore();
		}
	}
}
