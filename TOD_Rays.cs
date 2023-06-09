using EFT.BlitDebug;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("Time of Day/Camera God Rays")]
[ExecuteInEditMode]
public class TOD_Rays : TOD_ImageEffect
{
	public enum ResolutionType
	{
		Low,
		Normal,
		High
	}

	public enum BlendModeType
	{
		Screen,
		Add
	}

	public Shader GodRayShader;

	public Shader ScreenClearShader;

	public ResolutionType Resolution = ResolutionType.Normal;

	public BlendModeType BlendMode;

	[_E05A(0f, 4f)]
	public int BlurIterations = 2;

	[_E058(0f)]
	public float BlurRadius = 2f;

	[_E058(0f)]
	public float Intensity = 1f;

	[_E058(0f)]
	public float MaxRadius = 0.5f;

	public bool UseDepthTexture = true;

	private Material _E003;

	private Material _E004;

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(19789));

	private static readonly int _E006 = Shader.PropertyToID(_ED3E._E000(19778));

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(19825));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(19813));

	private const int _E009 = 2;

	private const int _E00A = 3;

	private const int _E00B = 1;

	private const int _E00C = 0;

	private const int _E00D = 4;

	protected void OnEnable()
	{
		if (!GodRayShader)
		{
			GodRayShader = _E3AC.Find(_ED3E._E000(19721));
		}
		if (!ScreenClearShader)
		{
			ScreenClearShader = _E3AC.Find(_ED3E._E000(19757));
		}
		_E003 = CreateMaterial(GodRayShader);
		_E004 = CreateMaterial(ScreenClearShader);
	}

	protected void OnDisable()
	{
		if ((bool)_E003)
		{
			Object.DestroyImmediate(_E003);
		}
		if ((bool)_E004)
		{
			Object.DestroyImmediate(_E004);
		}
	}

	protected void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!CheckSupport(UseDepthTexture))
		{
			Graphics.Blit(source, destination);
			return;
		}
		sky.Components.Rays = this;
		int width;
		int height;
		int depthBuffer;
		if (Resolution == ResolutionType.High)
		{
			width = source.width;
			height = source.height;
			depthBuffer = 0;
		}
		else if (Resolution == ResolutionType.Normal)
		{
			width = source.width / 2;
			height = source.height / 2;
			depthBuffer = 0;
		}
		else
		{
			width = source.width / 4;
			height = source.height / 4;
			depthBuffer = 0;
		}
		Vector3 vector = cam.WorldToViewportPoint(sky.Components.LightTransform.position);
		_E003.SetVector(_E005, new Vector4(1f, 1f, 0f, 0f) * BlurRadius);
		_E003.SetVector(_E006, new Vector4(vector.x, vector.y, vector.z, MaxRadius));
		RenderTexture temporary = RenderTexture.GetTemporary(width, height, depthBuffer);
		RenderTexture renderTexture = null;
		if (UseDepthTexture)
		{
			DebugGraphics.Blit(source, temporary, _E003, 2);
		}
		else
		{
			DebugGraphics.Blit(source, temporary, _E003, 3);
		}
		DrawBorder(temporary, _E004);
		float num = BlurRadius * 0.0013020834f;
		_E003.SetVector(_E005, new Vector4(num, num, 0f, 0f));
		_E003.SetVector(_E006, new Vector4(vector.x, vector.y, vector.z, MaxRadius));
		for (int i = 0; i < BlurIterations; i++)
		{
			renderTexture = RenderTexture.GetTemporary(width, height, depthBuffer);
			DebugGraphics.Blit(temporary, renderTexture, _E003, 1);
			RenderTexture.ReleaseTemporary(temporary);
			num = BlurRadius * (((float)i * 2f + 1f) * 6f) / 768f;
			_E003.SetVector(_E005, new Vector4(num, num, 0f, 0f));
			temporary = RenderTexture.GetTemporary(width, height, depthBuffer);
			DebugGraphics.Blit(renderTexture, temporary, _E003, 1);
			RenderTexture.ReleaseTemporary(renderTexture);
			num = BlurRadius * (((float)i * 2f + 2f) * 6f) / 768f;
			_E003.SetVector(_E005, new Vector4(num, num, 0f, 0f));
		}
		Vector4 value = (((double)vector.z >= 0.0) ? (Intensity * (Vector4)sky.RayColor) : Vector4.zero);
		_E003.SetVector(_E007, value);
		_E003.SetTexture(_E008, temporary);
		if (BlendMode == BlendModeType.Screen)
		{
			DebugGraphics.Blit(source, destination, _E003, 0);
		}
		else
		{
			DebugGraphics.Blit(source, destination, _E003, 4);
		}
		RenderTexture.ReleaseTemporary(temporary);
	}
}
