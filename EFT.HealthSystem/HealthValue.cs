using UnityEngine;

namespace EFT.HealthSystem;

public class HealthValue
{
	protected ValueStruct Value;

	public virtual float Current
	{
		get
		{
			return Value.Current;
		}
		set
		{
			float num = value;
			float current = Value.Current;
			float num2 = num - current;
			if (num2 < 0f)
			{
				num = current + num2 * DownMult;
			}
			Value.Current = Mathf.Clamp(num, Value.Minimum, Value.Maximum);
			LastDiff = Value.Current - current;
		}
	}

	public float Minimum => Value.Minimum;

	public float Maximum => Value.Maximum;

	public ValueStruct CurrentAndMaximum => Value;

	public float Normalized => Value.Normalized;

	public float LastDiff { get; protected set; }

	public float DownMult { get; set; }

	public bool AtMinimum => Value.AtMinimum;

	public bool AtMaximum => Value.AtMaximum;

	public HealthValue(Profile._E000.ValueInfo valueInfo)
	{
		_E000(valueInfo.Current, valueInfo.Minimum, valueInfo.Maximum);
	}

	public HealthValue(float cur, float max, float min = 0f)
	{
		_E000(cur, min, max);
	}

	private void _E000(float cur, float min, float max)
	{
		Value.Minimum = min;
		Value.Maximum = max;
		Value.Current = cur;
		DownMult = 1f;
	}
}
