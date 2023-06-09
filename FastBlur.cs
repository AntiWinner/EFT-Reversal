using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FastBlur : MonoBehaviour
{
	[Serializable]
	public enum Dimensions
	{
		_128 = 0x80,
		_256 = 0x100,
		_512 = 0x200,
		_1024 = 0x400,
		_2048 = 0x800
	}

	[Range(0f, 1f)]
	[SerializeField]
	[Header("Blur Setup")]
	private float _value;

	[SerializeField]
	private Material _material;

	[SerializeField]
	private Dimensions _upsampleTexDimension = Dimensions._512;

	[SerializeField]
	private Dimensions _downsampleTexDimension = Dimensions._256;

	[SerializeField]
	[Range(1f, 8f)]
	private int _blurCount = 4;

	[SerializeField]
	[Header("Hit")]
	private float _hitNoise = 0.2f;

	[SerializeField]
	private float _hitTime = 0.5f;

	[SerializeField]
	private AnimationCurve _hitCurve;

	[SerializeField]
	[Header("Death")]
	private float _deathTime = 1.5f;

	[SerializeField]
	private AnimationCurve _deathCurve;

	private float _E000 = 1f;

	private float _E001 = 1f;

	private AnimationCurve _E002;

	private bool _E003;

	private float _E004;

	private Material _E005;

	private static readonly int _E006 = Shader.PropertyToID(_ED3E._E000(43641));

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(44277));

	private void Awake()
	{
		if (_material != null)
		{
			_E005 = _material.CopyToPreventMaterialChangeInEditor();
		}
	}

	private void Start()
	{
		_E002 = _hitCurve;
	}

	public void Hit(float power)
	{
		if (!_E003)
		{
			_E001 = 0f;
			_E000 = _hitTime;
			_E002 = _hitCurve;
			_E004 = power;
		}
	}

	public void Die()
	{
		if (!_E003)
		{
			_E001 = 0f;
			_E000 = _deathTime;
			_E002 = _deathCurve;
			_E003 = true;
		}
	}

	public void Reset()
	{
		_E001 = 1f;
		_value = 0f;
		_E003 = false;
		_E002 = _hitCurve;
	}

	public void Update()
	{
		_E001 = Mathf.Clamp01(_E001 + Time.deltaTime / _E000);
		float num = 0f;
		if (_E001 > 0f && _E001 < 1f && !_E003)
		{
			num = UnityEngine.Random.Range(0f - _hitNoise, _hitNoise);
		}
		_value = (_E002.Evaluate(_E001) + num) * _E004;
		_value = Mathf.Clamp01(_value);
		if (_E001 >= 1f)
		{
			base.enabled = false;
		}
	}

	protected void OnRenderImage(RenderTexture source, RenderTexture destanation)
	{
		if (Mathf.Abs(_value) < Mathf.Epsilon)
		{
			_E3A1.BlitOrCopy(source, destanation);
			return;
		}
		RenderTexture temporary = RenderTexture.GetTemporary((int)_upsampleTexDimension, (int)_upsampleTexDimension, 0);
		RenderTexture temporary2 = RenderTexture.GetTemporary((int)_downsampleTexDimension, (int)_downsampleTexDimension, 0);
		Graphics.Blit(source, temporary);
		for (int i = 0; i < _blurCount; i++)
		{
			Graphics.Blit(temporary, temporary2);
			Graphics.Blit(temporary2, temporary);
		}
		_E005.SetTexture(_E006, temporary);
		_E005.SetFloat(_E007, _value);
		Graphics.Blit(source, destanation, _E005);
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(temporary2);
	}
}
