using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class Posteffect : MonoBehaviour
{
	[SerializeField]
	protected Shader EffectShader;

	[SerializeField]
	protected string EffectValueName = _ED3E._E000(88800);

	[Range(0f, 1f)]
	[SerializeField]
	private float _effectAmount;

	protected Material EffectMaterial;

	public float EffectAmount
	{
		get
		{
			return _effectAmount;
		}
		set
		{
			_effectAmount = Mathf.Clamp01(value);
		}
	}

	protected virtual void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
		if (EffectShader == null)
		{
			Debug.Log(_ED3E._E000(88788));
			base.enabled = false;
		}
		else if (!EffectShader.isSupported)
		{
			Debug.Log(EffectShader.name + _ED3E._E000(88822));
			base.enabled = false;
		}
		EffectMaterial = new Material(EffectShader)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
	}

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destanation)
	{
		if (Math.Abs(EffectAmount) < Mathf.Epsilon)
		{
			_E3A1.BlitOrCopy(source, destanation);
			return;
		}
		EffectMaterial.SetFloat(EffectValueName, EffectAmount);
		Graphics.Blit(source, destanation, EffectMaterial);
	}
}
