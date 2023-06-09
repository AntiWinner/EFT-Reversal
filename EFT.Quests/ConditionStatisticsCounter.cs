using System;
using EFT.Counters;
using Newtonsoft.Json;
using UnityEngine;

namespace EFT.Quests;

public sealed class ConditionStatisticsCounter : ConditionOneTarget
{
	private _E945._E000 _counter;

	private CounterTag? _counterTag;

	public bool byTag;

	[JsonIgnore]
	public _E945._E000 Counter
	{
		get
		{
			if (byTag)
			{
				throw new Exception("wrong call Counter");
			}
			return _counter ?? (_counter = _E944.GetCounter(target));
		}
	}

	[JsonIgnore]
	public CounterTag CounterTag
	{
		get
		{
			if (!byTag)
			{
				throw new Exception("wrong call CounterTag");
			}
			if (!_counterTag.HasValue)
			{
				if (Enum.TryParse<CounterTag>(target, out var result))
				{
					_counterTag = result;
				}
				else
				{
					Debug.LogErrorFormat("cant parse {0}", target);
					_counterTag = CounterTag.Exp;
				}
			}
			return _counterTag.Value;
		}
	}
}
