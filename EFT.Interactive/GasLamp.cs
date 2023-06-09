using System;
using System.Linq;
using UnityEngine;

namespace EFT.Interactive;

public class GasLamp : Turnable
{
	public enum FlickerSounds
	{
		Flicker1,
		Flicker2,
		Flicker3,
		Flicker4
	}

	private struct _E000
	{
		public Light Light;

		public float OriginalIntensity;
	}

	private struct _E001
	{
		public MultiFlareLight Light;

		public float OriginalAlpha;
	}

	private static readonly int _E002 = Animator.StringToHash(_ED3E._E000(205981));

	[SerializeField]
	private AudioSource _audioSource;

	[SerializeField]
	private float _intensity;

	private float _E01F;

	private _E000[] _E020;

	private _E001[] _E021;

	private void OnValidate()
	{
		CheckUniqueIdOnDuplicateEvent();
	}

	private void Awake()
	{
		_E020 = GetComponentsInChildren<Light>().Select(delegate(Light x)
		{
			_E000 result2 = default(_E000);
			result2.Light = x;
			result2.OriginalIntensity = x.intensity;
			return result2;
		}).ToArray();
		_E021 = GetComponentsInChildren<MultiFlareLight>().Select(delegate(MultiFlareLight x)
		{
			_E001 result = default(_E001);
			result.Light = x;
			result.OriginalAlpha = x.CurrentAlpha;
			return result;
		}).ToArray();
		_intensity = 1f;
		_E01F = 1f;
	}

	private void Update()
	{
		if (Math.Abs(_intensity - _E01F) > 0.01f)
		{
			_E000[] array = _E020;
			for (int i = 0; i < array.Length; i++)
			{
				_E000 obj = array[i];
				obj.Light.intensity = obj.OriginalIntensity * _intensity;
			}
			_E001[] array2 = _E021;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].Light.CurrentAlpha = 0.7f * _intensity;
			}
			_E01F = _intensity;
		}
	}

	public void PlaySound(AudioClip clip)
	{
		if (!(_audioSource == null))
		{
			_audioSource.clip = clip;
			_audioSource.Play();
		}
	}

	public override void Switch(EState switchTo)
	{
		GetComponent<Animator>().SetBool(_E002, switchTo == EState.On);
	}
}
