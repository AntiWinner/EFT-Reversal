using System;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public sealed class BloodOnScreenOld : Posteffect
{
	[Serializable]
	public struct BloodData
	{
		public Texture DuDv;

		public Texture Blood;
	}

	[SerializeField]
	[Header("Blood")]
	private BloodData[] _bloodData;

	[SerializeField]
	private string _dudvPropertyName = _ED3E._E000(41665);

	[SerializeField]
	private string _bloodPropertyName = _ED3E._E000(41880);

	[SerializeField]
	private string _bloodAmountPropertyName = _ED3E._E000(41722);

	[SerializeField]
	private string _cutoutPropertyName = _ED3E._E000(41874);

	[SerializeField]
	[Range(0f, 1f)]
	private float _cutout;

	[Range(0f, 1f)]
	[SerializeField]
	private float _bloodTextureAmount = 0.5f;

	[Range(0f, 1f)]
	[Header("Refraction")]
	[SerializeField]
	private float _refraction = 0.022f;

	[SerializeField]
	private string _refractionPropertyName = _ED3E._E000(41702);

	[Range(0f, 4f)]
	[Header("Downsampling")]
	[SerializeField]
	private int _downsampling = 4;

	[HideInInspector]
	[SerializeField]
	[Header("Color corection")]
	public int Mode;

	[SerializeField]
	[HideInInspector]
	public float InputMinL;

	[SerializeField]
	[HideInInspector]
	public float InputMaxL = 255f;

	[SerializeField]
	[HideInInspector]
	public float InputGammaL = 1f;

	[SerializeField]
	[HideInInspector]
	public float InputMinR;

	[HideInInspector]
	[SerializeField]
	public float InputMaxR = 255f;

	[HideInInspector]
	[SerializeField]
	public float InputGammaR = 1f;

	[SerializeField]
	[HideInInspector]
	public float InputMinG;

	[SerializeField]
	[HideInInspector]
	public float InputMaxG = 255f;

	[HideInInspector]
	[SerializeField]
	public float InputGammaG = 1f;

	[SerializeField]
	[HideInInspector]
	public float InputMinB;

	[HideInInspector]
	[SerializeField]
	public float InputMaxB = 255f;

	[SerializeField]
	[HideInInspector]
	public float InputGammaB = 1f;

	[SerializeField]
	[HideInInspector]
	public float OutputMinL;

	[SerializeField]
	[HideInInspector]
	public float OutputMaxL = 255f;

	[HideInInspector]
	[SerializeField]
	public float OutputMinR;

	[HideInInspector]
	[SerializeField]
	public float OutputMaxR = 255f;

	[SerializeField]
	[HideInInspector]
	public float OutputMinG;

	[HideInInspector]
	[SerializeField]
	public float OutputMaxG = 255f;

	[SerializeField]
	[HideInInspector]
	public float OutputMinB;

	[HideInInspector]
	[SerializeField]
	public float OutputMaxB = 255f;

	private Dictionary<int, BloodData> _E000;

	private static readonly int _E001 = Shader.PropertyToID(_ED3E._E000(41728));

	private static readonly int _E002 = Shader.PropertyToID(_ED3E._E000(41786));

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(41772));

	private static readonly int _E004 = Shader.PropertyToID(_ED3E._E000(41760));

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(41819));

	private static readonly int _E006 = Shader.PropertyToID(_ED3E._E000(41806));

	public float Cutout
	{
		get
		{
			return _cutout;
		}
		set
		{
			_cutout = Mathf.Clamp01(value);
		}
	}

	public int TextureSetsAmount => _bloodData.Length;

	protected override void Start()
	{
		base.Start();
		_E000 = new Dictionary<int, BloodData>(_bloodData.Length);
		for (int i = 0; i < _bloodData.Length; i++)
		{
			_E000.Add(i, _bloodData[i]);
		}
		SetTexture(0);
	}

	public void SetTexture(int setNumber)
	{
		if (_E000 == null)
		{
			_E000 = new Dictionary<int, BloodData>(_bloodData.Length);
			for (int i = 0; i < _bloodData.Length; i++)
			{
				_E000.Add(i, _bloodData[i]);
			}
		}
		setNumber = Mathf.Clamp(setNumber, 0, _E000.Count - 1);
		EffectMaterial.SetTexture(_dudvPropertyName, _E000[setNumber].DuDv);
		EffectMaterial.SetTexture(_bloodPropertyName, _E000[setNumber].Blood);
	}

	protected override void OnRenderImage(RenderTexture source, RenderTexture destanation)
	{
		if (Math.Abs(base.EffectAmount) < Mathf.Epsilon)
		{
			_E3A1.BlitOrCopy(source, destanation);
			return;
		}
		EffectMaterial.SetFloat(_refractionPropertyName, _refraction);
		EffectMaterial.SetFloat(EffectValueName, base.EffectAmount);
		EffectMaterial.SetFloat(_bloodAmountPropertyName, _bloodTextureAmount);
		EffectMaterial.SetFloat(_cutoutPropertyName, Cutout);
		if (Mode == 0)
		{
			EffectMaterial.SetVector(_E001, new Vector4(InputMinL / 255f, InputMinL / 255f, InputMinL / 255f, 1f));
			EffectMaterial.SetVector(_E002, new Vector4(InputMaxL / 255f, InputMaxL / 255f, InputMaxL / 255f, 1f));
			EffectMaterial.SetVector(_E003, new Vector4(InputGammaL, InputGammaL, InputGammaL, 1f));
			EffectMaterial.SetVector(_E004, new Vector4(OutputMinL / 255f, OutputMinL / 255f, OutputMinL / 255f, 1f));
			EffectMaterial.SetVector(_E005, new Vector4(OutputMaxL / 255f, OutputMaxL / 255f, OutputMaxL / 255f, 1f));
		}
		else
		{
			EffectMaterial.SetVector(_E001, new Vector4(InputMinR / 255f, InputMinG / 255f, InputMinB / 255f, 1f));
			EffectMaterial.SetVector(_E002, new Vector4(InputMaxR / 255f, InputMaxG / 255f, InputMaxB / 255f, 1f));
			EffectMaterial.SetVector(_E003, new Vector4(InputGammaR, InputGammaG, InputGammaB, 1f));
			EffectMaterial.SetVector(_E004, new Vector4(OutputMinR / 255f, OutputMinG / 255f, OutputMinB / 255f, 1f));
			EffectMaterial.SetVector(_E005, new Vector4(OutputMaxR / 255f, OutputMaxG / 255f, OutputMaxB / 255f, 1f));
		}
		RenderTexture temporary = RenderTexture.GetTemporary(512, 512, 0);
		temporary.name = _ED3E._E000(41854);
		RenderTexture temporary2 = RenderTexture.GetTemporary(256, 256, 0);
		temporary2.name = _ED3E._E000(41839);
		Graphics.Blit(source, temporary);
		for (int i = 0; i < _downsampling; i++)
		{
			Graphics.Blit(temporary, temporary2);
			Graphics.Blit(temporary2, temporary);
		}
		EffectMaterial.SetTexture(_E006, temporary);
		Graphics.Blit(source, destanation, EffectMaterial);
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(temporary2);
	}
}
