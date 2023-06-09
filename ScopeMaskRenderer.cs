using System;
using System.Collections.Generic;
using EFT.CameraControl;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class ScopeMaskRenderer : MonoBehaviour
{
	private readonly _E42E m__E000 = new _E42E(_ED3E._E000(88913), CameraEvent.BeforeGBuffer);

	private Shader m__E001;

	private Material m__E002;

	private Material m__E003;

	private Shader m__E004;

	private Material m__E005;

	private RenderTexture m__E006;

	private CollimatorSight m__E007;

	private HashSet<CollimatorSight> m__E008 = new HashSet<CollimatorSight>();

	private static readonly Color m__E009 = Color.red;

	private static readonly Color m__E00A = Color.blue;

	private static readonly Color m__E00B = Color.green;

	private static readonly float _E00C = 9f;

	private static readonly int _E00D = Shader.PropertyToID(_ED3E._E000(88900));

	private static readonly int _E00E = Shader.PropertyToID(_ED3E._E000(36528));

	private OpticSight _E00F => _E8A8.Instance.OpticCameraManager.CurrentOpticSight;

	private void Awake()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(_E004));
		CollimatorSight.OnCollimatorEnabled += _E000;
		CollimatorSight.OnCollimatorDisabled += _E001;
		CollimatorSight.OnCollimatorUpdated += _E002;
		_E003();
	}

	private void OnDestroy()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(_E004));
		CollimatorSight.OnCollimatorEnabled -= _E000;
		CollimatorSight.OnCollimatorDisabled -= _E001;
		CollimatorSight.OnCollimatorUpdated -= _E002;
		if (this.m__E006 != null)
		{
			UnityEngine.Object.DestroyImmediate(this.m__E006);
		}
	}

	private void _E000(CollimatorSight collimatorSight)
	{
		this.m__E008.Add(collimatorSight);
	}

	private void _E001(CollimatorSight collimatorSight)
	{
		this.m__E008.Remove(collimatorSight);
	}

	private void _E002(CollimatorSight collimatorSight)
	{
		if (collimatorSight.isActiveAndEnabled)
		{
			this.m__E007 = collimatorSight;
		}
	}

	private void _E003()
	{
		this.m__E008 = new HashSet<CollimatorSight>();
		if (this.m__E001 == null)
		{
			this.m__E001 = _E3AC.Find(_ED3E._E000(38202));
		}
		if (this.m__E002 == null)
		{
			this.m__E002 = new Material(this.m__E001);
			this.m__E002.SetColor(_E00E, ScopeMaskRenderer.m__E009);
		}
		if (this.m__E003 == null)
		{
			this.m__E003 = new Material(this.m__E001);
			this.m__E003.SetColor(_E00E, ScopeMaskRenderer.m__E00A);
		}
		if (this.m__E004 == null)
		{
			this.m__E004 = _E3AC.Find(_ED3E._E000(60315));
		}
		if (this.m__E005 == null)
		{
			this.m__E005 = new Material(this.m__E004);
		}
		Shader.SetGlobalTexture(_E00D, Texture2D.blackTexture);
	}

	private void _E004(Camera currentCamera)
	{
		if (!(currentCamera == null) && !(_E8A8.Instance.Camera != currentCamera) && this.m__E000.UpdateOnPreCullRender(out var buffer))
		{
			buffer.Clear();
			_E005(buffer, currentCamera, this.m__E000.GetSSAAComponent());
		}
	}

	private void _E005(CommandBuffer buffer, Camera currentCamera, SSAA ssaa)
	{
		_E006(currentCamera, ssaa);
		_E007(buffer);
		_E009(buffer, currentCamera);
		_E00A(buffer);
		_E008(buffer);
		Shader.SetGlobalTexture(_E00D, this.m__E006);
	}

	private void _E006(Camera currentCamera, SSAA ssaa)
	{
		int num = (ssaa ? ssaa.GetInputWidth() : currentCamera.pixelWidth);
		if (num == 0)
		{
			num = currentCamera.pixelWidth;
		}
		int num2 = (ssaa ? ssaa.GetInputHeight() : currentCamera.pixelHeight);
		if (num2 == 0)
		{
			num2 = currentCamera.pixelHeight;
		}
		if (this.m__E006 == null)
		{
			_E00B(currentCamera, num, num2);
		}
		if (this.m__E006.width != num || this.m__E006.height != num2)
		{
			UnityEngine.Object.DestroyImmediate(this.m__E006);
			_E00B(currentCamera, num, num2);
		}
	}

	private void _E007(CommandBuffer buffer)
	{
		buffer.SetRenderTarget(this.m__E006, BuiltinRenderTextureType.CameraTarget);
		buffer.ClearRenderTarget(clearDepth: false, clearColor: true, Color.black);
	}

	private void _E008(CommandBuffer buffer)
	{
		this.m__E002.SetColor(_E00E, ScopeMaskRenderer.m__E009);
		Renderer renderer = _E00F?.LensRenderer;
		if (renderer != null)
		{
			buffer.DrawRenderer(renderer, this.m__E002, 0, 0);
		}
	}

	private void _E009(CommandBuffer buffer, Camera currentCamera)
	{
		this.m__E003.SetColor(_E00E, ScopeMaskRenderer.m__E00A);
		Vector3 position = currentCamera.transform.position;
		foreach (CollimatorSight item in this.m__E008)
		{
			Vector3 position2 = item.CollimatorMeshRenderer.transform.position;
			if (!(Vector3.SqrMagnitude(position - position2) > _E00C))
			{
				buffer.DrawRenderer(item.CollimatorMeshRenderer, this.m__E003);
			}
		}
	}

	private void _E00A(CommandBuffer buffer)
	{
		if (this.m__E007 != null)
		{
			this.m__E005.CopyPropertiesFromMaterial(this.m__E007.CollimatorMaterial);
			this.m__E005.SetColor(_E00E, ScopeMaskRenderer.m__E00B);
			buffer.DrawRenderer(this.m__E007.CollimatorMeshRenderer, this.m__E005);
		}
	}

	private void _E00B(Camera currentCamera, int width, int height)
	{
		this.m__E006 = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32)
		{
			name = _ED3E._E000(88873) + currentCamera.name
		};
	}
}
