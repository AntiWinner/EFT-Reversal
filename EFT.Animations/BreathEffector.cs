using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using CW2.Animations;
using UnityEngine;

namespace EFT.Animations;

[Serializable]
public class BreathEffector : IEffector
{
	public _E338 Physical;

	public bool IsAiming;

	private Spring _handsRotationSpring;

	private Spring _recoilRotationSpring;

	public float Intensity = 1f;

	[Header("Smooth Random")]
	private float _shakeIntensity = 1f;

	public SmoothRandom XRandom;

	public SmoothRandom YRandom;

	private float _breathIntensity = 1f;

	private float _breathFrequency = 1f;

	[Header("Tremor")]
	public SmoothRandom TremorXRandom;

	public SmoothRandom TremorYRandom;

	public SmoothRandom TremorZRandom;

	[Header("Hip Wiggle")]
	public SmoothRandom HipXRandom;

	public SmoothRandom HipZRandom;

	public float HipPenalty = 1f;

	public AnimVal[] Curves;

	private _E948[] _processors;

	public BreathParameters BreathParams;

	public bool TremorOn = true;

	public bool Fracture;

	private float _cameraSensetivity = 1f;

	public float StiffUntill;

	public float EnergyLowerLimit = 0.05f;

	public float EnergyFractureLimit = 0.3f;

	public float AmplitudeGainPerShot = 0.2f;

	public float Delay = 1f;

	public Player.BetterValueBlender AmplitudeGain = new Player.BetterValueBlender
	{
		Target = 0f,
		Speed = 0.1f
	};

	public Player.BetterValueBlender Hardness = new Player.BetterValueBlender();

	[SerializeField]
	private Vector2 _baseHipRandomAmplitudes = new Vector2(1.1f, 1.2f);

	[SerializeField]
	private Vector2 _randomBetween;

	[SerializeField]
	private float _shotHardness = 0.15f;

	[SerializeField]
	private AnimationCurve _lackOfOxygenStrength;

	public _E394<float> OxygenLevel;

	public _E394<float> StaminaLevel;

	public float Overweight { get; set; }

	public void Initialize(PlayerSpring playerSpring)
	{
		_processors = Curves.Select((AnimVal val) => new _E948(val)).ToArray();
		_handsRotationSpring = playerSpring.HandsRotation;
		_recoilRotationSpring = playerSpring.Recoil;
		Hardness.Target = 0.1f;
		_E948[] processors = _processors;
		for (int i = 0; i < processors.Length; i++)
		{
			processors[i].Initialize(playerSpring.CameraRotation, playerSpring.HandsPosition, playerSpring.HandsRotation, _breathIntensity, _breathFrequency, isHeadbobbing: true);
		}
		OxygenLevel = (Singleton<_E5CB>.Instance.Stamina.StaminaExhaustionRocksCamera ? new _E394<float>(() => Mathf.Min(Physical.Oxygen.NormalValue, Physical.Stamina.NormalValue)) : new _E394<float>(() => Physical.Oxygen.NormalValue));
		StaminaLevel = (Singleton<_E5CB>.Instance.Stamina.StaminaExhaustionCausesJiggle ? new _E394<float>(() => Mathf.Clamp(Mathf.Min(Physical.HandsStamina.NormalValue, Physical.Stamina.NormalValue), EnergyLowerLimit, Fracture ? EnergyFractureLimit : 1f)) : new _E394<float>(() => Mathf.Clamp(Physical.HandsStamina.NormalValue, EnergyLowerLimit, Fracture ? EnergyFractureLimit : 1f)));
	}

	public void OnShot()
	{
		if (!IsAiming)
		{
			float value = AmplitudeGain.Value;
			value += Mathf.Max(0f, AmplitudeGainPerShot - 0.2f * (Mathf.Sqrt(value) - 1f));
			AmplitudeGain.ChangeValue(value, Delay);
			Hardness.ChangeValue(_shotHardness, 0f);
			HipXRandom.Amplitude = Mathf.Clamp(_baseHipRandomAmplitudes.x + value, 0f, 3f);
			HipZRandom.Amplitude = Mathf.Clamp(_baseHipRandomAmplitudes.y + value, 0f, 3f);
			HipXRandom.Hardness = (HipZRandom.Hardness = _shotHardness);
			float value2 = HipXRandom.GetValue(UnityEngine.Random.Range(_randomBetween.x, _randomBetween.y));
			float value3 = HipZRandom.GetValue(UnityEngine.Random.Range(_randomBetween.x, _randomBetween.y));
			_recoilRotationSpring.Zero = new Vector3(Mathf.Min(_recoilRotationSpring.Zero.x, value2), 0f, value3 / 2f) * (Intensity * HipPenalty);
		}
	}

	public void Process(float deltaTime)
	{
		float num = Mathf.Sqrt(AmplitudeGain.Value);
		HipXRandom.Amplitude = Mathf.Clamp(_baseHipRandomAmplitudes.x + num, 0f, 3f);
		HipZRandom.Amplitude = Mathf.Clamp(_baseHipRandomAmplitudes.y + num, 0f, 3f);
		HipXRandom.Hardness = (HipZRandom.Hardness = Hardness.Value);
		_shakeIntensity = 1f;
		bool flag = TremorOn || Fracture;
		float num2 = 1f;
		if (Time.time < StiffUntill)
		{
			float num3 = Mathf.Clamp(0f - StiffUntill + Time.time + 1f, flag ? 0.5f : 0.3f, 1f);
			_breathIntensity = num3 * Intensity;
			_shakeIntensity = num3;
			num2 = num3;
		}
		else if (Physical.HoldingBreath)
		{
			_breathIntensity = 0.15f;
			_shakeIntensity = 0.15f;
		}
		else
		{
			float t = _lackOfOxygenStrength.Evaluate(OxygenLevel);
			float b = (IsAiming ? 0.75f : 1f);
			_breathIntensity = Mathf.Clamp(Mathf.Lerp(4f, b, t), 1f, 1.5f) * Intensity;
			_breathFrequency = Mathf.Clamp(Mathf.Lerp(4f, 1f, t), 1f, 2.5f) * deltaTime;
			_cameraSensetivity = Mathf.Lerp(2f, 0f, t) * Intensity;
		}
		_E394<float> staminaLevel = StaminaLevel;
		YRandom.Amplitude = BreathParams.AmplitudeCurve.Evaluate(staminaLevel);
		float num4 = BreathParams.Delay.Evaluate(staminaLevel);
		XRandom.MinMaxDelay = (YRandom.MinMaxDelay = new Vector2(num4 / 2f, num4));
		YRandom.Hardness = BreathParams.Hardness.Evaluate(staminaLevel);
		float value = YRandom.GetValue(deltaTime);
		float value2 = XRandom.GetValue(deltaTime);
		_handsRotationSpring.AddAcceleration(new Vector3(Mathf.Max(0f, 0f - value) * (1f - (float)staminaLevel) * 2f, value, value2) * (_shakeIntensity * Intensity));
		Vector3 vector;
		if (!flag)
		{
			vector = (IsAiming ? Vector3.zero : (new Vector3(HipXRandom.GetValue(deltaTime), 0f, HipZRandom.GetValue(deltaTime)) * (Intensity * HipPenalty)));
		}
		else
		{
			float num5 = (TremorOn ? deltaTime : (deltaTime / 2f));
			num5 *= num2;
			float num6 = TremorXRandom.GetValue(num5);
			float value3 = TremorYRandom.GetValue(num5);
			float value4 = TremorZRandom.GetValue(num5);
			if (Fracture && !IsAiming)
			{
				num6 += Mathf.Max(0f, value) * Mathf.Lerp(1f, 100f / EnergyFractureLimit, staminaLevel);
			}
			vector = new Vector3(num6, value3, value4) * Intensity;
		}
		if (Vector3.SqrMagnitude(vector - _recoilRotationSpring.Zero) > 0.01f)
		{
			_recoilRotationSpring.Zero = Vector3.Lerp(_recoilRotationSpring.Zero, vector, 0.1f);
		}
		else
		{
			_recoilRotationSpring.Zero = vector;
		}
		_processors[0].ProcessRaw(_breathFrequency, _breathIntensity);
		_processors[1].ProcessRaw(_breathFrequency, _breathIntensity * _cameraSensetivity);
	}

	public string DebugOutput()
	{
		throw new NotImplementedException();
	}

	[CompilerGenerated]
	private float _E000()
	{
		return Mathf.Min(Physical.Oxygen.NormalValue, Physical.Stamina.NormalValue);
	}

	[CompilerGenerated]
	private float _E001()
	{
		return Physical.Oxygen.NormalValue;
	}

	[CompilerGenerated]
	private float _E002()
	{
		return Mathf.Clamp(Mathf.Min(Physical.HandsStamina.NormalValue, Physical.Stamina.NormalValue), EnergyLowerLimit, Fracture ? EnergyFractureLimit : 1f);
	}

	[CompilerGenerated]
	private float _E003()
	{
		return Mathf.Clamp(Physical.HandsStamina.NormalValue, EnergyLowerLimit, Fracture ? EnergyFractureLimit : 1f);
	}
}
