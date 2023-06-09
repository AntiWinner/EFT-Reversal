using System;
using System.Linq;
using UnityEngine;

namespace EFT.Weather;

[Serializable]
public class WeatherCurve : IWeatherCurve
{
	private AnimationCurve windXCurve = new AnimationCurve();

	private AnimationCurve windYCurve = new AnimationCurve();

	private AnimationCurve topWindXCurve = new AnimationCurve();

	private AnimationCurve topWindYCurve = new AnimationCurve();

	private AnimationCurve rainCurve = new AnimationCurve();

	private AnimationCurve cloudinessCurve = new AnimationCurve();

	private AnimationCurve fogCurve = new AnimationCurve();

	private AnimationCurve temperatureCurve = new AnimationCurve();

	private const float MAX_WIND_DERIVATIVE = 1.0416667E-10f;

	private const float MAX_ADDITIONAL_FOG = 0.007f;

	public long StartTime { get; }

	public long FinishTime { get; }

	public Vector2 Wind => new Vector2(windXCurve.Evaluate(this._E000), windYCurve.Evaluate(this._E000));

	public Vector2 TopWind => new Vector2(topWindXCurve.Evaluate(this._E000), topWindYCurve.Evaluate(this._E000));

	public float Rain => Mathf.Clamp01(rainCurve.Evaluate(this._E000));

	public float Cloudiness => cloudinessCurve.Evaluate(this._E000);

	public float Fog => Mathf.Clamp01(fogCurve.Evaluate(this._E000));

	public float Temperature => temperatureCurve.Evaluate(this._E000);

	public float LightningThunderProbability => Mathf.InverseLerp(0.5f, 1f, Cloudiness);

	private float _E000 => (float)(this._E001 - StartTime) / this._E002;

	private long _E001 => _E5AD.MoscowNow.Ticks;

	private float _E002 => Mathf.Max(FinishTime - StartTime, 1f);

	public WeatherCurve()
	{
	}

	public WeatherCurve(_E8EB[] _nodes)
	{
		StartTime = _nodes.First().Time;
		FinishTime = _nodes.Last().Time;
		_E001(WrapMode.PingPong);
		_E002(_nodes);
	}

	public WeatherCurve(IWeatherCurve start, _E8EB end)
	{
		StartTime = this._E001;
		FinishTime = end.Time;
		Vector2 vector = 0.2f * _E8EB.WindDirections[end.WindDirection];
		windXCurve.AddKey(0f, vector.x);
		windYCurve.AddKey(0f, vector.y);
		topWindXCurve.AddKey(0f, start.TopWind.x);
		topWindYCurve.AddKey(0f, start.TopWind.y);
		rainCurve.AddKey(0f, start.Rain);
		fogCurve.AddKey(0f, start.Fog);
		cloudinessCurve.AddKey(0f, start.Cloudiness);
		temperatureCurve.AddKey(0f, start.Temperature);
		_E002(new _E8EB[1] { end });
		SetHalloweenWind(45, end.WindDirection);
		SetCurveConstant(windXCurve);
		SetCurveConstant(windYCurve);
		_E001(WrapMode.ClampForever);
	}

	public static void SetCurveConstant(AnimationCurve curve)
	{
		Vector2 vector = default(Vector2);
		Vector2 vector2 = default(Vector2);
		for (int i = 0; i < curve.keys.Length; i++)
		{
			float inTangent = 0f;
			float outTangent = 0f;
			bool flag = false;
			bool flag2 = false;
			Keyframe key = curve[i];
			if (i == 0)
			{
				inTangent = 0f;
				flag = true;
			}
			if (i == curve.keys.Length - 1)
			{
				outTangent = 0f;
				flag2 = true;
			}
			if (!flag)
			{
				vector.x = curve.keys[i - 1].time;
				vector.y = curve.keys[i - 1].value;
				vector2.x = curve.keys[i].time;
				vector2.y = curve.keys[i].value;
				_ = vector2 - vector;
				inTangent = float.PositiveInfinity;
			}
			if (!flag2)
			{
				vector.x = curve.keys[i].time;
				vector.y = curve.keys[i].value;
				vector2.x = curve.keys[i + 1].time;
				vector2.y = curve.keys[i + 1].value;
				_ = vector2 - vector;
				outTangent = float.PositiveInfinity;
			}
			key.inTangent = inTangent;
			key.outTangent = outTangent;
			curve.MoveKey(i, key);
		}
	}

	public static void SetCurveLinear(AnimationCurve curve)
	{
		Vector2 vector = default(Vector2);
		Vector2 vector2 = default(Vector2);
		for (int i = 0; i < curve.keys.Length; i++)
		{
			float inTangent = 0f;
			float outTangent = 0f;
			bool flag = false;
			bool flag2 = false;
			Keyframe key = curve[i];
			if (i == 0)
			{
				inTangent = 0f;
				flag = true;
			}
			if (i == curve.keys.Length - 1)
			{
				outTangent = 0f;
				flag2 = true;
			}
			if (!flag)
			{
				vector.x = curve.keys[i - 1].time;
				vector.y = curve.keys[i - 1].value;
				vector2.x = curve.keys[i].time;
				vector2.y = curve.keys[i].value;
				Vector2 vector3 = vector2 - vector;
				inTangent = vector3.y / vector3.x;
			}
			if (!flag2)
			{
				vector.x = curve.keys[i].time;
				vector.y = curve.keys[i].value;
				vector2.x = curve.keys[i + 1].time;
				vector2.y = curve.keys[i + 1].value;
				Vector2 vector3 = vector2 - vector;
				outTangent = vector3.y / vector3.x;
			}
			key.inTangent = inTangent;
			key.outTangent = outTangent;
			curve.MoveKey(i, key);
		}
	}

	public void SetHalloweenWind(int secDuration, int windDirection)
	{
		long ticks = _E5AD.MoscowNow.AddSeconds(3.0).Ticks;
		long ticks2 = _E5AD.MoscowNow.AddSeconds(secDuration - 5).Ticks;
		long ticks3 = _E5AD.MoscowNow.AddSeconds(secDuration - 3).Ticks;
		_E000(ticks, 1f, windDirection);
		_E000(ticks2, 1f, windDirection);
		float time = (float)(ticks3 - StartTime) / this._E002;
		Vector2 vector = new Vector2(windXCurve.Evaluate(1f), windYCurve.Evaluate(1f));
		windXCurve.AddKey(time, vector.x);
		windYCurve.AddKey(time, vector.y);
	}

	private void _E000(long time, float windVal, int windDirection)
	{
		float time2 = (float)(time - StartTime) / this._E002;
		Vector2 vector = windVal * _E8EB.WindDirections[windDirection];
		windXCurve.AddKey(time2, vector.x);
		windYCurve.AddKey(time2, vector.y);
	}

	private void _E001(WrapMode mode)
	{
		windXCurve.postWrapMode = mode;
		windYCurve.postWrapMode = mode;
		topWindXCurve.postWrapMode = mode;
		topWindYCurve.postWrapMode = mode;
		rainCurve.postWrapMode = mode;
		cloudinessCurve.postWrapMode = mode;
		fogCurve.postWrapMode = mode;
		temperatureCurve.postWrapMode = mode;
	}

	private void _E002(_E8EB[] _nodes)
	{
		long num = StartTime;
		float lyingWater = 0f;
		float rainA = 0f;
		float temperatureA = 0f;
		float num2 = 0.2f;
		foreach (_E8EB obj in _nodes)
		{
			long time = obj.Time;
			float num3 = Mathf.Max(time - num, 1f);
			float time2 = (float)(time - StartTime) / this._E002;
			float num4 = Mathf.InverseLerp(1f, 5f, obj.Rain);
			float num5 = Mathf.InverseLerp(1f, 5f, obj.Wind);
			float num6 = Mathf.Sign(num5 - num2);
			float num7 = Mathf.Min(Mathf.Abs(num5 - num2) / num3, 1.0416667E-10f);
			float num8 = num6 * num7 * num3 + num2;
			Vector2 vector = num8 * _E8EB.WindDirections[obj.WindDirection];
			num2 = num8;
			num = time;
			lyingWater = _E003(lyingWater, rainA, num4, temperatureA, obj.Temperature, num3 / 8.64E+11f);
			temperatureA = obj.Temperature;
			rainA = num4;
			windXCurve.AddKey(time2, vector.x);
			windYCurve.AddKey(time2, vector.y);
			topWindXCurve.AddKey(time2, obj.TopWindDirection.x);
			topWindYCurve.AddKey(time2, obj.TopWindDirection.y);
			rainCurve.AddKey(time2, num4);
			fogCurve.AddKey(time2, obj.ScaterringFogDensity + num4 * 0.007f);
			cloudinessCurve.AddKey(time2, Mathf.Min(obj.Cloudness + num4 * num4 * num4, 1f));
			temperatureCurve.AddKey(time2, obj.Temperature);
		}
	}

	private float _E003(float lyingWater, float rainA, float rainB, float temperatureA, float temperatureB, float transitionLength, float t = 1f)
	{
		float a = (0.5f + Math.Max(temperatureA * 0.8f, 0f)) * Mathf.Lerp(rainA * 10f, 1f, 0.01f);
		float b = (0.5f + Math.Max(temperatureB * 0.8f, 0f)) * Mathf.Lerp(rainB * 10f, 1f, 0.01f);
		t *= transitionLength;
		lyingWater += _E004(rainA, rainB, transitionLength, t) * 96f;
		lyingWater -= _E004(a, b, transitionLength, t);
		lyingWater = Mathf.Clamp01(lyingWater);
		return lyingWater;
	}

	private float _E004(float a, float b, float tl, float t)
	{
		return t * t * (-2f * a * t + 3f * a * tl + 2f * b * t) / (6f * tl);
	}
}
