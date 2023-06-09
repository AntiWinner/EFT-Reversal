using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class VolumetricLightRenderer : MonoBehaviour
{
	public enum VolumtericResolution
	{
		Full,
		Half,
		Quarter
	}

	[CompilerGenerated]
	private static Action<VolumetricLightRenderer, Matrix4x4> m__E000;

	private static Mesh m__E001;

	private static Mesh m__E002;

	private static Material m__E003;

	public bool IsOn = true;

	public bool IsOptic;

	private Camera m__E004;

	private SSAA m__E005;

	private SSAAOptic m__E006;

	private CommandBuffer m__E007;

	private CommandBuffer m__E008;

	private Matrix4x4 _E009;

	private Material _E00A;

	private Material _E00B;

	private RenderTexture _E00C;

	private RenderTexture _E00D;

	private RenderTexture _E00E;

	private static Texture _E00F;

	private RenderTexture _E010;

	private RenderTexture _E011;

	private VolumtericResolution _E012 = VolumtericResolution.Half;

	private static Texture2D _E013;

	private static Texture3D _E014;

	private static bool _E015;

	private static int _E016 = -1;

	private static RenderTexture _E017;

	private static RenderTexture _E018;

	private static RenderTexture _E019;

	private static RenderTexture _E01A;

	private static RenderTexture _E01B;

	public VolumtericResolution Resolution = VolumtericResolution.Half;

	public Texture DefaultSpotCookie;

	private Matrix4x4 _E01C;

	private static readonly int _E01D = Shader.PropertyToID(_ED3E._E000(85208));

	private static readonly int _E01E = Shader.PropertyToID(_ED3E._E000(85188));

	private static readonly int _E01F = Shader.PropertyToID(_ED3E._E000(85242));

	private static readonly int _E020 = Shader.PropertyToID(_ED3E._E000(85217));

	private static readonly int _E021 = Shader.PropertyToID(_ED3E._E000(85266));

	private static readonly int _E022 = Shader.PropertyToID(_ED3E._E000(85249));

	private static readonly int _E023 = Shader.PropertyToID(_ED3E._E000(85303));

	public CommandBuffer GlobalCommandBuffer => this.m__E007;

	public static event Action<VolumetricLightRenderer, Matrix4x4> PreRenderEvent
	{
		[CompilerGenerated]
		add
		{
			Action<VolumetricLightRenderer, Matrix4x4> action = VolumetricLightRenderer.m__E000;
			Action<VolumetricLightRenderer, Matrix4x4> action2;
			do
			{
				action2 = action;
				Action<VolumetricLightRenderer, Matrix4x4> value2 = (Action<VolumetricLightRenderer, Matrix4x4>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref VolumetricLightRenderer.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<VolumetricLightRenderer, Matrix4x4> action = VolumetricLightRenderer.m__E000;
			Action<VolumetricLightRenderer, Matrix4x4> action2;
			do
			{
				action2 = action;
				Action<VolumetricLightRenderer, Matrix4x4> value2 = (Action<VolumetricLightRenderer, Matrix4x4>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref VolumetricLightRenderer.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static Material GetLightMaterial()
	{
		return VolumetricLightRenderer.m__E003;
	}

	public static Mesh GetPointLightMesh()
	{
		return VolumetricLightRenderer.m__E001;
	}

	public static Mesh GetSpotLightMesh()
	{
		return VolumetricLightRenderer.m__E002;
	}

	public RenderTexture GetVolumeLightBuffer()
	{
		return Resolution switch
		{
			VolumtericResolution.Quarter => _E00E, 
			VolumtericResolution.Half => _E00D, 
			_ => _E00C, 
		};
	}

	public RenderTexture GetVolumeLightDepthBuffer()
	{
		return Resolution switch
		{
			VolumtericResolution.Quarter => _E011, 
			VolumtericResolution.Half => _E010, 
			_ => null, 
		};
	}

	public static Texture GetDefaultSpotCookie()
	{
		return _E00F;
	}

	private void Awake()
	{
		if (this.m__E004 == null)
		{
			this.m__E004 = GetComponent<Camera>();
		}
		this.m__E005 = GetComponent<SSAA>();
		this.m__E006 = GetComponent<SSAAOptic>();
		if (this.m__E004.actualRenderingPath == RenderingPath.Forward)
		{
			this.m__E004.depthTextureMode = DepthTextureMode.Depth;
		}
		_E012 = Resolution;
		Shader shader = _E3AC.Find(_ED3E._E000(37950));
		_E00A = new Material(shader);
		shader = _E3AC.Find(_ED3E._E000(37916));
		_E00B = new Material(shader);
		this.m__E007 = new CommandBuffer
		{
			name = _ED3E._E000(82700)
		};
		this.m__E008 = new CommandBuffer
		{
			name = _ED3E._E000(82693)
		};
		if (VolumetricLightRenderer.m__E001 == null)
		{
			GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			VolumetricLightRenderer.m__E001 = obj.GetComponent<MeshFilter>().sharedMesh;
			UnityEngine.Object.DestroyImmediate(obj);
		}
		if (VolumetricLightRenderer.m__E002 == null)
		{
			VolumetricLightRenderer.m__E002 = _E008();
		}
		if (VolumetricLightRenderer.m__E003 == null)
		{
			shader = _E3AC.Find(_ED3E._E000(37424));
			VolumetricLightRenderer.m__E003 = new Material(shader);
		}
		if (_E00F == null)
		{
			_E00F = DefaultSpotCookie;
		}
		int opticRenderResolution = _E8A8.Instance.OpticCameraManager.OpticRenderResolution;
		_E000(opticRenderResolution);
		_E006();
		_E007();
		if (IsOptic)
		{
			_E001();
		}
		else
		{
			_E002();
		}
		this.m__E004.RemoveCommandBuffer(CameraEvent.BeforeLighting, this.m__E007);
		this.m__E004.AddCommandBuffer(CameraEvent.BeforeLighting, this.m__E007);
		this.m__E004.RemoveCommandBuffer(CameraEvent.BeforeImageEffects, this.m__E008);
		this.m__E004.AddCommandBuffer(CameraEvent.BeforeImageEffects, this.m__E008);
	}

	private void _E000(int opticResolution)
	{
		if (_E015)
		{
			return;
		}
		_E016 = opticResolution;
		RenderTextureFormat format = (SystemInfo.SupportsRenderTextureFormat(RuntimeUtilities.defaultHDRRenderTextureFormat) ? RuntimeUtilities.defaultHDRRenderTextureFormat : RenderTextureFormat.ARGB32);
		if (_E017 != null)
		{
			_E017.Release();
			UnityEngine.Object.DestroyImmediate(_E017);
		}
		_E017 = new RenderTexture(opticResolution, opticResolution, 0, format)
		{
			name = _ED3E._E000(82730),
			filterMode = FilterMode.Bilinear
		};
		if (Resolution == VolumtericResolution.Half || Resolution == VolumtericResolution.Quarter)
		{
			RenderTextureFormat format2 = (SystemInfo.SupportsRenderTextureFormat(RuntimeUtilities.defaultHDRRenderTextureFormat) ? RuntimeUtilities.defaultHDRRenderTextureFormat : RenderTextureFormat.ARGB32);
			if (_E018 != null)
			{
				_E018.Release();
				UnityEngine.Object.DestroyImmediate(_E018);
			}
			_E018 = new RenderTexture(opticResolution / 2, opticResolution / 2, 0, format2)
			{
				name = _ED3E._E000(82769),
				filterMode = FilterMode.Bilinear
			};
			RenderTextureFormat format3 = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RHalf) ? RenderTextureFormat.RHalf : RenderTextureFormat.ARGB32);
			if (_E019 != null)
			{
				_E019.Release();
				UnityEngine.Object.DestroyImmediate(_E019);
			}
			_E019 = new RenderTexture(opticResolution / 2, opticResolution / 2, 0, format3)
			{
				name = _ED3E._E000(82804),
				filterMode = FilterMode.Point
			};
			_E019.Create();
			RenderTextureFormat format4 = (SystemInfo.SupportsRenderTextureFormat(RuntimeUtilities.defaultHDRRenderTextureFormat) ? RuntimeUtilities.defaultHDRRenderTextureFormat : RenderTextureFormat.ARGB32);
			if (_E01A != null)
			{
				_E01A.Release();
				UnityEngine.Object.DestroyImmediate(_E01A);
			}
			_E01A = new RenderTexture(opticResolution / 4, opticResolution / 4, 0, format4)
			{
				name = _ED3E._E000(82846),
				filterMode = FilterMode.Bilinear
			};
			RenderTextureFormat format5 = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RHalf) ? RenderTextureFormat.RHalf : RenderTextureFormat.ARGB32);
			if (_E01B != null)
			{
				_E01B.Release();
				UnityEngine.Object.DestroyImmediate(_E01B);
			}
			_E01B = new RenderTexture(opticResolution / 4, opticResolution / 4, 0, format5)
			{
				name = _ED3E._E000(82876),
				filterMode = FilterMode.Point
			};
			_E01B.Create();
		}
		_E015 = true;
	}

	private void _E001()
	{
		_E00C = _E017;
		_E00D = _E018;
		_E010 = _E019;
		_E00E = _E01A;
		_E011 = _E01B;
	}

	private void _E002()
	{
		int num = (this.m__E005 ? this.m__E005.GetInputWidth() : this.m__E004.pixelWidth);
		int num2 = (this.m__E005 ? this.m__E005.GetInputHeight() : this.m__E004.pixelHeight);
		num = ((num > 0) ? num : this.m__E004.pixelWidth);
		num2 = ((num2 > 0) ? num2 : this.m__E004.pixelHeight);
		if (_E00C != null)
		{
			_E00C.Release();
			UnityEngine.Object.DestroyImmediate(_E00C);
		}
		RenderTextureFormat format = (SystemInfo.SupportsRenderTextureFormat(RuntimeUtilities.defaultHDRRenderTextureFormat) ? RuntimeUtilities.defaultHDRRenderTextureFormat : RenderTextureFormat.ARGB32);
		_E00C = new RenderTexture(num, num2, 0, format)
		{
			name = _ED3E._E000(82849),
			filterMode = FilterMode.Bilinear
		};
		if (_E010 != null)
		{
			_E010.Release();
			UnityEngine.Object.DestroyImmediate(_E010);
		}
		if (_E00D != null)
		{
			_E00D.Release();
			UnityEngine.Object.DestroyImmediate(_E00D);
		}
		if (Resolution == VolumtericResolution.Half || Resolution == VolumtericResolution.Quarter)
		{
			RenderTextureFormat format2 = (SystemInfo.SupportsRenderTextureFormat(RuntimeUtilities.defaultHDRRenderTextureFormat) ? RuntimeUtilities.defaultHDRRenderTextureFormat : RenderTextureFormat.ARGB32);
			_E00D = new RenderTexture(num / 2, num2 / 2, 0, format2)
			{
				name = _ED3E._E000(82899),
				filterMode = FilterMode.Bilinear
			};
			RenderTextureFormat format3 = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RHalf) ? RenderTextureFormat.RHalf : RenderTextureFormat.ARGB32);
			_E010 = new RenderTexture(num / 2, num2 / 2, 0, format3)
			{
				name = _ED3E._E000(82937),
				filterMode = FilterMode.Point
			};
			_E010.Create();
		}
		if (_E00E != null)
		{
			_E00E.Release();
			UnityEngine.Object.DestroyImmediate(_E00E);
		}
		if (_E011 != null)
		{
			_E011.Release();
			UnityEngine.Object.DestroyImmediate(_E011);
		}
		if (Resolution == VolumtericResolution.Quarter)
		{
			RenderTextureFormat format4 = (SystemInfo.SupportsRenderTextureFormat(RuntimeUtilities.defaultHDRRenderTextureFormat) ? RuntimeUtilities.defaultHDRRenderTextureFormat : RenderTextureFormat.ARGB32);
			_E00E = new RenderTexture(num / 4, num2 / 4, 0, format4)
			{
				name = _ED3E._E000(82918),
				filterMode = FilterMode.Bilinear
			};
			RenderTextureFormat format5 = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RHalf) ? RenderTextureFormat.RHalf : RenderTextureFormat.ARGB32);
			_E011 = new RenderTexture(num / 4, num2 / 4, 0, format5)
			{
				name = _ED3E._E000(85007),
				filterMode = FilterMode.Point
			};
			_E011.Create();
		}
	}

	public void OnPreRender()
	{
		if (!(this.m__E004 == null))
		{
			_E01C = Matrix4x4.Perspective(this.m__E004.fieldOfView, this.m__E004.aspect, 0.01f, this.m__E004.farClipPlane);
			_E01C = GL.GetGPUProjectionMatrix(_E01C, renderIntoTexture: true);
			_E009 = _E01C * this.m__E004.worldToCameraMatrix;
			_E003();
			_E005();
			VolumetricLightRenderer.m__E000?.Invoke(this, _E009);
			this.m__E007.SetRenderTarget(BuiltinRenderTextureType.CameraTarget, BuiltinRenderTextureType.CameraTarget);
			_E004();
		}
	}

	private void _E003()
	{
		bool flag = SystemInfo.graphicsShaderLevel > 40;
		if (this.m__E007 == null)
		{
			Awake();
		}
		this.m__E007.Clear();
		switch (Resolution)
		{
		case VolumtericResolution.Quarter:
		{
			Texture source2 = null;
			this.m__E007.Blit(source2, _E010, _E00B, flag ? 4 : 10);
			this.m__E007.Blit(source2, _E011, _E00B, flag ? 6 : 11);
			this.m__E007.SetRenderTarget(_E00E);
			break;
		}
		case VolumtericResolution.Half:
		{
			Texture source = null;
			this.m__E007.Blit(source, _E010, _E00B, flag ? 4 : 10);
			this.m__E007.SetRenderTarget(_E00D);
			break;
		}
		default:
			this.m__E007.SetRenderTarget(_E00C);
			break;
		}
		this.m__E007.ClearRenderTarget(clearDepth: false, clearColor: true, new Color(0f, 0f, 0f, 1f));
	}

	private void _E004()
	{
		this.m__E008.Clear();
		if (IsOn)
		{
			string text = _ED3E._E000(85047);
			this.m__E008.BeginSample(text);
			switch (Resolution)
			{
			case VolumtericResolution.Quarter:
			{
				string text4 = _ED3E._E000(85076);
				this.m__E008.BeginSample(text4);
				this.m__E008.GetTemporaryRT(_E023, _E011.width, _E011.height, 0, FilterMode.Bilinear, RuntimeUtilities.defaultHDRRenderTextureFormat);
				this.m__E008.Blit(_E00E, _E023, _E00B, 8);
				this.m__E008.Blit(_E023, _E00E, _E00B, 9);
				this.m__E008.Blit(_E00E, _E00C, _E00B, 7);
				this.m__E008.EndSample(text4);
				break;
			}
			case VolumtericResolution.Half:
			{
				string text3 = _ED3E._E000(85117);
				this.m__E008.BeginSample(text3);
				this.m__E008.GetTemporaryRT(_E023, _E00D.width, _E00D.height, 0, FilterMode.Bilinear, RuntimeUtilities.defaultHDRRenderTextureFormat);
				this.m__E008.Blit(_E00D, _E023, _E00B, 2);
				this.m__E008.Blit(_E023, _E00D, _E00B, 3);
				this.m__E008.Blit(_E00D, _E00C, _E00B, 5);
				this.m__E008.EndSample(text3);
				break;
			}
			default:
			{
				string text2 = _ED3E._E000(85099);
				this.m__E008.BeginSample(text2);
				this.m__E008.GetTemporaryRT(_E023, _E00C.width, _E00C.height, 0, FilterMode.Bilinear, RuntimeUtilities.defaultHDRRenderTextureFormat);
				this.m__E008.Blit(_E00C, _E023, _E00B, 0);
				this.m__E008.Blit(_E023, _E00C, _E00B, 1);
				this.m__E008.EndSample(text2);
				break;
			}
			}
			this.m__E008.BeginSample(_ED3E._E000(85132));
			if ((bool)this.m__E005)
			{
				this.m__E008.Blit(_E00C, this.m__E005.GetRTIdentifier(), _E00A);
			}
			else
			{
				this.m__E008.Blit(_E00C, BuiltinRenderTextureType.CameraTarget, _E00A);
			}
			this.m__E008.EndSample(_ED3E._E000(85132));
			this.m__E008.ReleaseTemporaryRT(_E023);
			this.m__E008.SetRenderTarget(BuiltinRenderTextureType.CameraTarget, BuiltinRenderTextureType.CameraTarget);
			this.m__E008.EndSample(text);
		}
	}

	private void _E005()
	{
		_E00B.SetTexture(_E01D, _E010);
		_E00B.SetTexture(_E01E, _E00D);
		_E00B.SetTexture(_E01F, _E011);
		_E00B.SetTexture(_E020, _E00E);
		Shader.SetGlobalTexture(_E021, _E013);
		Shader.SetGlobalTexture(_E022, _E014);
	}

	private void Update()
	{
		if (!IsOptic)
		{
			if (_E012 != Resolution)
			{
				_E012 = Resolution;
				_E002();
			}
			int num = (this.m__E005 ? this.m__E005.GetInputWidth() : this.m__E004.pixelWidth);
			int num2 = (this.m__E005 ? this.m__E005.GetInputHeight() : this.m__E004.pixelHeight);
			if (_E00C != null && (_E00C.width != num || _E00C.height != num2))
			{
				_E002();
			}
		}
		else
		{
			int opticRenderResolution = _E8A8.Instance.OpticCameraManager.OpticRenderResolution;
			if (_E016 != opticRenderResolution)
			{
				_E015 = false;
				_E000(opticRenderResolution);
				_E001();
			}
		}
	}

	private void _E006()
	{
		if (_E014 != null)
		{
			return;
		}
		TextAsset textAsset = _E3A2.Load(_ED3E._E000(85123)) as TextAsset;
		byte[] bytes = textAsset.bytes;
		uint num = BitConverter.ToUInt32(textAsset.bytes, 12);
		uint num2 = BitConverter.ToUInt32(textAsset.bytes, 16);
		uint num3 = BitConverter.ToUInt32(textAsset.bytes, 20);
		uint num4 = BitConverter.ToUInt32(textAsset.bytes, 24);
		uint num5 = BitConverter.ToUInt32(textAsset.bytes, 80);
		uint num6 = BitConverter.ToUInt32(textAsset.bytes, 88);
		if (num6 == 0)
		{
			num6 = num3 / num2 * 8;
		}
		_E014 = new Texture3D((int)num2, (int)num, (int)num4, TextureFormat.RGBA32, mipChain: false);
		_E014.name = _ED3E._E000(85175);
		Color[] array = new Color[num2 * num * num4];
		uint num7 = 128u;
		if (textAsset.bytes[84] == 68 && textAsset.bytes[85] == 88 && textAsset.bytes[86] == 49 && textAsset.bytes[87] == 48 && (num5 & 4u) != 0)
		{
			uint num8 = BitConverter.ToUInt32(textAsset.bytes, (int)num7);
			if (num8 >= 60 && num8 <= 65)
			{
				num6 = 8u;
			}
			else if (num8 >= 48 && num8 <= 52)
			{
				num6 = 16u;
			}
			else if (num8 >= 27 && num8 <= 32)
			{
				num6 = 32u;
			}
			num7 += 20;
		}
		uint num9 = num6 / 8u;
		num3 = (num2 * num6 + 7) / 8u;
		for (int i = 0; i < num4; i++)
		{
			for (int j = 0; j < num; j++)
			{
				for (int k = 0; k < num2; k++)
				{
					float num10 = (float)(int)bytes[num7 + k * num9] / 255f;
					array[k + j * num2 + i * num2 * num] = new Color(num10, num10, num10, num10);
				}
				num7 += num3;
			}
		}
		_E014.SetPixels(array);
		_E014.Apply();
	}

	private void _E007()
	{
		if (!(_E013 != null))
		{
			_E013 = new Texture2D(8, 8, TextureFormat.Alpha8, mipChain: false, linear: true);
			_E013.name = _ED3E._E000(85160);
			_E013.filterMode = FilterMode.Point;
			Color32[] array = new Color32[8 * 8];
			int num = 0;
			byte b = 3;
			array[num++] = new Color32(b, b, b, b);
			b = 192;
			array[num++] = new Color32(b, b, b, b);
			b = 51;
			array[num++] = new Color32(b, b, b, b);
			b = 239;
			array[num++] = new Color32(b, b, b, b);
			b = 15;
			array[num++] = new Color32(b, b, b, b);
			b = 204;
			array[num++] = new Color32(b, b, b, b);
			b = 62;
			array[num++] = new Color32(b, b, b, b);
			b = 251;
			array[num++] = new Color32(b, b, b, b);
			b = 129;
			array[num++] = new Color32(b, b, b, b);
			b = 66;
			array[num++] = new Color32(b, b, b, b);
			b = 176;
			array[num++] = new Color32(b, b, b, b);
			b = 113;
			array[num++] = new Color32(b, b, b, b);
			b = 141;
			array[num++] = new Color32(b, b, b, b);
			b = 78;
			array[num++] = new Color32(b, b, b, b);
			b = 188;
			array[num++] = new Color32(b, b, b, b);
			b = 125;
			array[num++] = new Color32(b, b, b, b);
			b = 35;
			array[num++] = new Color32(b, b, b, b);
			b = 223;
			array[num++] = new Color32(b, b, b, b);
			b = 19;
			array[num++] = new Color32(b, b, b, b);
			b = 207;
			array[num++] = new Color32(b, b, b, b);
			b = 47;
			array[num++] = new Color32(b, b, b, b);
			b = 235;
			array[num++] = new Color32(b, b, b, b);
			b = 31;
			array[num++] = new Color32(b, b, b, b);
			b = 219;
			array[num++] = new Color32(b, b, b, b);
			b = 160;
			array[num++] = new Color32(b, b, b, b);
			b = 98;
			array[num++] = new Color32(b, b, b, b);
			b = 145;
			array[num++] = new Color32(b, b, b, b);
			b = 82;
			array[num++] = new Color32(b, b, b, b);
			b = 172;
			array[num++] = new Color32(b, b, b, b);
			b = 109;
			array[num++] = new Color32(b, b, b, b);
			b = 156;
			array[num++] = new Color32(b, b, b, b);
			b = 94;
			array[num++] = new Color32(b, b, b, b);
			b = 11;
			array[num++] = new Color32(b, b, b, b);
			b = 200;
			array[num++] = new Color32(b, b, b, b);
			b = 58;
			array[num++] = new Color32(b, b, b, b);
			b = 247;
			array[num++] = new Color32(b, b, b, b);
			b = 7;
			array[num++] = new Color32(b, b, b, b);
			b = 196;
			array[num++] = new Color32(b, b, b, b);
			b = 54;
			array[num++] = new Color32(b, b, b, b);
			b = 243;
			array[num++] = new Color32(b, b, b, b);
			b = 137;
			array[num++] = new Color32(b, b, b, b);
			b = 74;
			array[num++] = new Color32(b, b, b, b);
			b = 184;
			array[num++] = new Color32(b, b, b, b);
			b = 121;
			array[num++] = new Color32(b, b, b, b);
			b = 133;
			array[num++] = new Color32(b, b, b, b);
			b = 70;
			array[num++] = new Color32(b, b, b, b);
			b = 180;
			array[num++] = new Color32(b, b, b, b);
			b = 117;
			array[num++] = new Color32(b, b, b, b);
			b = 43;
			array[num++] = new Color32(b, b, b, b);
			b = 231;
			array[num++] = new Color32(b, b, b, b);
			b = 27;
			array[num++] = new Color32(b, b, b, b);
			b = 215;
			array[num++] = new Color32(b, b, b, b);
			b = 39;
			array[num++] = new Color32(b, b, b, b);
			b = 227;
			array[num++] = new Color32(b, b, b, b);
			b = 23;
			array[num++] = new Color32(b, b, b, b);
			b = 211;
			array[num++] = new Color32(b, b, b, b);
			b = 168;
			array[num++] = new Color32(b, b, b, b);
			b = 105;
			array[num++] = new Color32(b, b, b, b);
			b = 153;
			array[num++] = new Color32(b, b, b, b);
			b = 90;
			array[num++] = new Color32(b, b, b, b);
			b = 164;
			array[num++] = new Color32(b, b, b, b);
			b = 102;
			array[num++] = new Color32(b, b, b, b);
			b = 149;
			array[num++] = new Color32(b, b, b, b);
			b = 86;
			array[num++] = new Color32(b, b, b, b);
			_E013.SetPixels32(array);
			_E013.Apply();
		}
	}

	private Mesh _E008()
	{
		Mesh mesh = new Mesh();
		Vector3[] array = new Vector3[50];
		Color32[] array2 = new Color32[50];
		array[0] = new Vector3(0f, 0f, 0f);
		array[1] = new Vector3(0f, 0f, 1f);
		float num = 0f;
		float num2 = (float)Math.PI / 8f;
		float num3 = 0.9f;
		for (int i = 0; i < 16; i++)
		{
			array[i + 2] = new Vector3((0f - Mathf.Cos(num)) * num3, Mathf.Sin(num) * num3, num3);
			array2[i + 2] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			array[i + 2 + 16] = new Vector3(0f - Mathf.Cos(num), Mathf.Sin(num), 1f);
			array2[i + 2 + 16] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
			array[i + 2 + 32] = new Vector3((0f - Mathf.Cos(num)) * num3, Mathf.Sin(num) * num3, 1f);
			array2[i + 2 + 32] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			num += num2;
		}
		mesh.vertices = array;
		mesh.colors32 = array2;
		int[] array3 = new int[288];
		int num4 = 0;
		for (int j = 2; j < 17; j++)
		{
			array3[num4++] = 0;
			array3[num4++] = j;
			array3[num4++] = j + 1;
		}
		array3[num4++] = 0;
		array3[num4++] = 17;
		array3[num4++] = 2;
		for (int k = 2; k < 17; k++)
		{
			array3[num4++] = k;
			array3[num4++] = k + 16;
			array3[num4++] = k + 1;
			array3[num4++] = k + 1;
			array3[num4++] = k + 16;
			array3[num4++] = k + 16 + 1;
		}
		array3[num4++] = 2;
		array3[num4++] = 17;
		array3[num4++] = 18;
		array3[num4++] = 18;
		array3[num4++] = 17;
		array3[num4++] = 33;
		for (int l = 18; l < 33; l++)
		{
			array3[num4++] = l;
			array3[num4++] = l + 16;
			array3[num4++] = l + 1;
			array3[num4++] = l + 1;
			array3[num4++] = l + 16;
			array3[num4++] = l + 16 + 1;
		}
		array3[num4++] = 18;
		array3[num4++] = 33;
		array3[num4++] = 34;
		array3[num4++] = 34;
		array3[num4++] = 33;
		array3[num4++] = 49;
		for (int m = 34; m < 49; m++)
		{
			array3[num4++] = 1;
			array3[num4++] = m + 1;
			array3[num4++] = m;
		}
		array3[num4++] = 1;
		array3[num4++] = 34;
		array3[num4++] = 49;
		mesh.triangles = array3;
		mesh.RecalculateBounds();
		return mesh;
	}
}
