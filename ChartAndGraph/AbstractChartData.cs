using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ChartAndGraph;

[Serializable]
public abstract class AbstractChartData
{
	protected class Slider
	{
		public string category;

		public string group;

		public double from;

		public double to;

		public float startTime;

		public float totalTime;

		public float timeScale = 1f;

		public AnimationCurve curve;

		public bool UpdateSlider(AbstractChartData data)
		{
			float num = Time.time - startTime;
			num *= timeScale;
			if (num > totalTime)
			{
				data.SetValueInternal(category, group, to);
				return true;
			}
			float num2 = num / totalTime;
			if (curve != null)
			{
				num2 = curve.Evaluate(num2);
			}
			double value = from * (1.0 - (double)num2) + to * (double)num2;
			data.SetValueInternal(category, group, value);
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public string group;

		internal bool _E000(Slider x)
		{
			return x.group == group;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public string category;

		internal bool _E000(Slider x)
		{
			return x.category == category;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public string category;

		public string group;

		internal bool _E000(Slider x)
		{
			if (x.category == category)
			{
				return x.group == group;
			}
			return false;
		}
	}

	protected List<Slider> mSliders = new List<Slider>();

	protected void RemoveSliderForGroup(string group)
	{
		mSliders.RemoveAll((Slider x) => x.group == group);
	}

	protected void RemoveSliderForCategory(string category)
	{
		mSliders.RemoveAll((Slider x) => x.category == category);
	}

	protected void RemoveSlider(string category, string group)
	{
		mSliders.RemoveAll((Slider x) => x.category == category && x.group == group);
	}

	private bool _E000(Slider s)
	{
		return s.UpdateSlider(this);
	}

	protected void UpdateSliders()
	{
		mSliders.RemoveAll(_E000);
	}

	protected abstract void SetValueInternal(string column, string row, double value);
}
