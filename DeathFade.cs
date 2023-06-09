using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class DeathFade : MonoBehaviour
{
	[SerializeField]
	private Material _deathFade;

	[SerializeField]
	private Material _deathColorFade;

	[SerializeField]
	private Color _color = Color.black;

	[SerializeField]
	private float _closeEyesTime = 0.1f;

	[SerializeField]
	private float _closeEyesDelay = 1.8f;

	[SerializeField]
	private float _disableTime = 0.001f;

	[SerializeField]
	private AnimationCurve _enableCurve;

	[SerializeField]
	private AnimationCurve _disableCurve;

	[SerializeField]
	private bool _debug;

	private float _E000;

	[SerializeField]
	[Range(0f, 1f)]
	private float _closeEyesValue;

	[Range(0f, 1f)]
	[SerializeField]
	private float _fadeValue;

	private bool _E001;

	private float _E002;

	private float _E003;

	private AnimationCurve _E004;

	private Material _E005;

	private Material _E006;

	private float _E007;

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(44277));

	private static readonly int _E009 = Shader.PropertyToID(_ED3E._E000(36528));

	public void EnableEffect()
	{
		_E000 = _closeEyesTime;
		_E001 = true;
		_E004 = _enableCurve;
	}

	public void DisableEffect()
	{
		_E000 = _disableTime;
		_closeEyesValue = 0f;
		_fadeValue = 0f;
		_E003 = 0f;
		_E007 = 0f;
		_E001 = false;
		_E004 = _disableCurve;
	}

	private void Awake()
	{
		_E005 = _deathFade.CopyToPreventMaterialChangeInEditor();
		_E006 = _deathColorFade.CopyToPreventMaterialChangeInEditor();
	}

	private void Update()
	{
		if (_debug || _E004 == null)
		{
			return;
		}
		if (_E001)
		{
			_E007 += Time.deltaTime;
			_E003 = Mathf.Clamp01(_E003 + Time.deltaTime / 2f);
			if (_E007 >= _closeEyesDelay)
			{
				_E002 = Mathf.Clamp01(_E002 + Time.deltaTime / _E000);
			}
		}
		else
		{
			_E003 = Mathf.Clamp01(_E003 - Time.deltaTime / 2f);
			_E002 = Mathf.Clamp01(_E002 - Time.deltaTime / _E000);
		}
		_closeEyesValue = _E004.Evaluate(_E002);
		_fadeValue = _E004.Evaluate(_E003);
	}

	protected void OnRenderImage(RenderTexture source, RenderTexture destanation)
	{
		if (Math.Abs(_closeEyesValue) < Mathf.Epsilon && Math.Abs(_fadeValue) < Mathf.Epsilon)
		{
			_E3A1.BlitOrCopy(source, destanation);
			return;
		}
		_E006.SetFloat(_E008, _fadeValue / 1.5f);
		Graphics.Blit(source, destanation, _E006);
		if (!(Math.Abs(_closeEyesValue) < Mathf.Epsilon))
		{
			_E005.SetFloat(_E008, _closeEyesValue / 2f);
			_E005.SetColor(_E009, _color);
			Graphics.Blit(source, destanation, _E005);
		}
	}
}
