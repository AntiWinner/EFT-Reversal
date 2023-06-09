using System;
using UnityEngine;

public class SimpleSparksAdapter : Emitter
{
	[Serializable]
	public class CurveValue
	{
		public AnimationCurve Curve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		public float Value = 1f;

		public float[] Values = new float[128];

		public void RecalcVals()
		{
			for (int i = 0; i < 128; i++)
			{
				Values[i] = Curve.Evaluate((float)i * (1f / 128f)) * Value;
			}
		}

		public void RecalcVals(Func<float, float> func)
		{
			for (int i = 0; i < 128; i++)
			{
				Values[i] = func(Curve.Evaluate((float)i * (1f / 128f)) * Value);
			}
		}
	}

	[Serializable]
	public class ByteCurveValue
	{
		public AnimationCurve Curve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		public byte Value = byte.MaxValue;

		public byte[] Values = new byte[128];

		public void RecalcVals()
		{
			for (int i = 0; i < 128; i++)
			{
				Values[i] = (byte)(Curve.Evaluate((float)i * (1f / 128f)) * (float)(int)Value);
			}
		}

		public void RecalcVals(Func<float, float> func)
		{
			for (int i = 0; i < 128; i++)
			{
				Values[i] = (byte)func(Curve.Evaluate((float)i * (1f / 128f)) * (float)(int)Value);
			}
		}
	}

	public SimpleSparksRenderer SimpleSparksObject;

	[_E2BE("Curve 240;Value 60", true)]
	public CurveValue LifetimeCurve;

	[_E2BE("Curve 240;Value 60", true)]
	public CurveValue SpeedCurve;

	[_E2BE("Curve 240;Value 60", true)]
	public CurveValue GravityCurve;

	[_E2BE("Curve 240;Value 60", true)]
	public CurveValue DragCurve;

	[_E2BE("Curve 240;Value 120 S 0 255", true)]
	public ByteCurveValue EmissionCurve;

	[_E2BE("Curve 240;Value 120 S 0 255", true)]
	public ByteCurveValue SizeCurve;

	[_E2BE("Curve 240;Value 120 S 0 255", true)]
	public ByteCurveValue TurbulenceAmplitudeCurve;

	[_E2BE("Curve 240;Value 120 S 0 255", true)]
	public ByteCurveValue TurbulenceFrequencyCurve;

	public float EmissionTimeShift;

	private const int _E007 = 128;

	private const float _E008 = 1f / 128f;

	private void Awake()
	{
		if (SimpleSparksObject == null)
		{
			SimpleSparksObject = GetComponent<SimpleSparksRenderer>();
		}
	}

	private void OnValidate()
	{
		LifetimeCurve.RecalcVals();
		SpeedCurve.RecalcVals();
		GravityCurve.RecalcVals();
		DragCurve.RecalcVals(_E000);
		EmissionCurve.RecalcVals();
		SizeCurve.RecalcVals();
		TurbulenceAmplitudeCurve.RecalcVals();
		TurbulenceFrequencyCurve.RecalcVals();
	}

	private static float _E000(float val)
	{
		val = 1f - val;
		val *= val;
		val *= val;
		return Math.Max(Math.Min(val, 0.999f), 0f);
	}

	public override void Emit(Vector3 pos, Vector3 vel)
	{
		int num = UnityEngine.Random.Range(0, 128);
		float lifeTime = LifetimeCurve.Values[num];
		float num2 = SpeedCurve.Values[num];
		float gravity = GravityCurve.Values[num];
		float drag = DragCurve.Values[num];
		byte emission = EmissionCurve.Values[num];
		byte size = SizeCurve.Values[num];
		byte turbulence = TurbulenceAmplitudeCurve.Values[num];
		byte frequency = TurbulenceFrequencyCurve.Values[num];
		SimpleSparksObject.EmitSeg(pos, vel * num2, Time.time + EmissionTimeShift, gravity, drag, lifeTime, emission, size, turbulence, frequency);
	}
}
