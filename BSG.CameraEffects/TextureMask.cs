using System.Collections.Generic;
using UnityEngine;

namespace BSG.CameraEffects;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/TextureMask")]
public class TextureMask : MonoBehaviour
{
	private const string m__E000 = "_STRETCH";

	public Shader Shader;

	public Color Color;

	public Texture Mask;

	public bool Stretch;

	public float Size = 1f;

	private readonly HashSet<_E442> _E001 = new HashSet<_E442>();

	private Material _E002;

	private Camera _E003;

	private SSAA _E004;

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(36528));

	private static readonly int _E006 = Shader.PropertyToID(_ED3E._E000(86305));

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(92796));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(92785));

	private static readonly int _E009 = Shader.PropertyToID(_ED3E._E000(92772));

	private void Awake()
	{
		TryGetComponent<Camera>(out _E003);
		TryGetComponent<SSAA>(out _E004);
	}

	private void OnEnable()
	{
		ApplySettings();
	}

	private void OnValidate()
	{
		if (_E003 == null)
		{
			TryGetComponent<Camera>(out _E003);
		}
		if (_E004 == null)
		{
			TryGetComponent<SSAA>(out _E004);
		}
		ApplySettings();
	}

	private Material _E000()
	{
		if (!(_E002 == null))
		{
			return _E002;
		}
		return _E002 = new Material(Shader);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, _E000());
	}

	public void ApplySettings()
	{
		Material material = _E000();
		material.SetColor(_E005, Color);
		material.SetTexture(_E006, Mask);
		float num = 1f / Size;
		material.SetFloat(_E007, num);
		float num2 = ((Mask != null) ? ((float)Mask.height / (float)Mask.width) : 1f);
		material.SetFloat(_E008, num2);
		if (_E003 == null)
		{
			TryGetComponent<Camera>(out _E003);
		}
		float num3 = ((_E003 != null) ? _E003.aspect : ((float)(Screen.width / Screen.height)));
		material.SetFloat(_E009, num3);
		if (Stretch)
		{
			material.EnableKeyword(_ED3E._E000(92747));
		}
		else
		{
			material.DisableKeyword(_ED3E._E000(92747));
		}
		if (_E004 != null)
		{
			SSAA.MaskParams maskParams = default(SSAA.MaskParams);
			maskParams.mask = Mask;
			maskParams.invMaskSize = num;
			maskParams.invAspect = num2;
			maskParams.stretch = Stretch;
			maskParams.cameraAspect = num3;
			SSAA.MaskParams maskParams2 = maskParams;
			_E004.SetMaskParams(in maskParams2);
		}
	}

	public void TryToEnable(_E442 textureMaskHolder, bool on)
	{
		if (on)
		{
			_E001.Add(textureMaskHolder);
		}
		else
		{
			_E001.Remove(textureMaskHolder);
		}
		base.enabled = _E001.Count > 0;
	}
}
