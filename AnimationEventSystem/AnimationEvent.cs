using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AnimationEventSystem;

[Serializable]
public sealed class AnimationEvent : ICloneable
{
	public const float MAX_EVENT_TIME = 1f;

	public AnimationEventParameter Parameter;

	[SerializeField]
	private string _functionName;

	[SerializeField]
	private int _functionNameHash;

	public bool Enabled = true;

	[SerializeField]
	private float _time;

	public List<EventCondition> EventConditions;

	public string FunctionName
	{
		get
		{
			return _functionName;
		}
		set
		{
			_functionName = value;
			_functionNameHash = ((!string.IsNullOrEmpty(_functionName)) ? _functionName.GetHashCode() : 0);
		}
	}

	public int FunctionNameHash => _functionNameHash;

	public float Time
	{
		get
		{
			return _time;
		}
		set
		{
			_time = Mathf.Clamp(value, 0f, 1f);
		}
	}

	public AnimationEvent()
	{
		Parameter = new AnimationEventParameter
		{
			ParamType = EAnimationEventParamType.None
		};
	}

	public bool IsTimeToFire(float previousNormalizedTime, float normalizedTime)
	{
		if (_time >= previousNormalizedTime)
		{
			return _time < normalizedTime;
		}
		return false;
	}

	public bool IsConditionsSucceed(IAnimator animator)
	{
		if (!Enabled)
		{
			return false;
		}
		if (EventConditions == null)
		{
			return true;
		}
		for (int i = 0; i < EventConditions.Count; i++)
		{
			if (!EventConditions[i].IsSucceed(animator))
			{
				return false;
			}
		}
		return true;
	}

	public object Clone()
	{
		AnimationEvent animationEvent = new AnimationEvent
		{
			Parameter = (AnimationEventParameter)Parameter.Clone(),
			Enabled = Enabled,
			_functionName = _functionName,
			_functionNameHash = _functionNameHash,
			_time = _time
		};
		if (EventConditions != null)
		{
			animationEvent.EventConditions = new List<EventCondition>();
			for (int i = 0; i < EventConditions.Count; i++)
			{
				animationEvent.EventConditions.Add((EventCondition)EventConditions[i].Clone());
			}
		}
		return animationEvent;
	}

	public void Serialize(BinaryWriter writer)
	{
		Parameter.Serialize(writer);
		writer.Write(Enabled);
		writer.Write(_functionName);
		writer.Write(_functionNameHash);
		writer.Write(_time);
		if (EventConditions == null)
		{
			writer.Write((short)0);
			return;
		}
		writer.Write((short)EventConditions.Count);
		for (int i = 0; i < EventConditions.Count; i++)
		{
			EventConditions[i].Serialize(writer);
		}
	}

	public void Deserialize(BinaryReader reader)
	{
		Parameter.Deserialize(reader);
		Enabled = reader.ReadBoolean();
		_functionName = reader.ReadString();
		_functionNameHash = reader.ReadInt32();
		_time = reader.ReadSingle();
		short num = reader.ReadInt16();
		if (num == 0)
		{
			EventConditions = null;
			return;
		}
		EventConditions = new List<EventCondition>(num);
		for (int i = 0; i < num; i++)
		{
			EventCondition eventCondition = new EventCondition();
			eventCondition.Deserialize(reader);
			EventConditions.Add(eventCondition);
		}
	}

	public override string ToString()
	{
		return string.Format(_ED3E._E000(124863), FunctionName, Parameter, Time, FunctionNameHash);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((AnimationEvent)obj);
	}

	protected bool Equals(AnimationEvent other)
	{
		if (EventConditions == null != (other.EventConditions == null))
		{
			return false;
		}
		if (EventConditions != null && !EventConditions.SequenceEqual(other.EventConditions))
		{
			return false;
		}
		if (object.Equals(Parameter, other.Parameter) && _functionName == other._functionName && _functionNameHash == other._functionNameHash && Enabled == other.Enabled)
		{
			return _time.Equals(other._time);
		}
		return false;
	}
}
