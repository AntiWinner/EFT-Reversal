using System;
using System.IO;

namespace AnimationEventSystem;

[Serializable]
public sealed class EventCondition : ICloneable
{
	private delegate bool _E000(EventCondition condition, IAnimator animator);

	private enum EConditionType
	{
		None = -1,
		IntEqual,
		IntNotEqual,
		IntGreaterThan,
		IntLessThan,
		IntGreaterEqualThan,
		IntLessEqualThan,
		FloatGreaterThan,
		FloatLessThan,
		BoolEqual,
		EConditionTypeEnumsCount
	}

	private static readonly _E000[] _CONDITION_DELEGATES;

	public bool BoolValue;

	public float FloatValue;

	public int IntValue;

	public string ParameterName;

	private int _cachedNameHash;

	private EConditionType _conditionMode = EConditionType.None;

	public EEventConditionParamTypes ConditionParamType;

	public EEventConditionModes ConditionMode;

	static EventCondition()
	{
		_CONDITION_DELEGATES = new _E000[9];
		_CONDITION_DELEGATES[0] = _E008;
		_CONDITION_DELEGATES[1] = _E007;
		_CONDITION_DELEGATES[2] = _E006;
		_CONDITION_DELEGATES[3] = _E005;
		_CONDITION_DELEGATES[4] = _E004;
		_CONDITION_DELEGATES[5] = _E003;
		_CONDITION_DELEGATES[6] = _E002;
		_CONDITION_DELEGATES[7] = _E001;
		_CONDITION_DELEGATES[8] = _E000;
	}

	private static bool _E000(EventCondition condition, IAnimator animator)
	{
		return animator.GetBool(condition._cachedNameHash) == condition.BoolValue;
	}

	private static bool _E001(EventCondition condition, IAnimator animator)
	{
		return animator.GetFloat(condition._cachedNameHash) < condition.FloatValue;
	}

	private static bool _E002(EventCondition condition, IAnimator animator)
	{
		return animator.GetFloat(condition._cachedNameHash) > condition.FloatValue;
	}

	private static bool _E003(EventCondition condition, IAnimator animator)
	{
		return animator.GetInteger(condition._cachedNameHash) <= condition.IntValue;
	}

	private static bool _E004(EventCondition condition, IAnimator animator)
	{
		return animator.GetInteger(condition._cachedNameHash) >= condition.IntValue;
	}

	private static bool _E005(EventCondition condition, IAnimator animator)
	{
		return animator.GetInteger(condition._cachedNameHash) < condition.IntValue;
	}

	private static bool _E006(EventCondition condition, IAnimator animator)
	{
		return animator.GetInteger(condition._cachedNameHash) > condition.IntValue;
	}

	private static bool _E007(EventCondition condition, IAnimator animator)
	{
		return animator.GetInteger(condition._cachedNameHash) != condition.IntValue;
	}

	private static bool _E008(EventCondition condition, IAnimator animator)
	{
		return animator.GetInteger(condition._cachedNameHash) == condition.IntValue;
	}

	public bool IsSucceed(IAnimator animator)
	{
		if (_conditionMode == EConditionType.None)
		{
			_cachedNameHash = animator.StringToHash(ParameterName);
			_E009();
		}
		return _CONDITION_DELEGATES[(int)_conditionMode](this, animator);
	}

	public string ToString(IAnimator animator)
	{
		return string.Format(_ED3E._E000(126360), ParameterName, ConditionMode, IsSucceed(animator));
	}

	private void _E009()
	{
		switch (ConditionParamType)
		{
		case EEventConditionParamTypes.Int:
			switch (ConditionMode)
			{
			case EEventConditionModes.Equal:
				_conditionMode = EConditionType.IntEqual;
				break;
			case EEventConditionModes.NotEqual:
				_conditionMode = EConditionType.IntNotEqual;
				break;
			case EEventConditionModes.GreaterThan:
				_conditionMode = EConditionType.IntGreaterThan;
				break;
			case EEventConditionModes.LessThan:
				_conditionMode = EConditionType.IntLessThan;
				break;
			case EEventConditionModes.GreaterEqualThan:
				_conditionMode = EConditionType.IntGreaterEqualThan;
				break;
			case EEventConditionModes.LessEqualThan:
				_conditionMode = EConditionType.IntLessEqualThan;
				break;
			}
			break;
		case EEventConditionParamTypes.Float:
			switch (ConditionMode)
			{
			case EEventConditionModes.GreaterThan:
				_conditionMode = EConditionType.FloatGreaterThan;
				break;
			case EEventConditionModes.LessThan:
				_conditionMode = EConditionType.FloatLessThan;
				break;
			}
			break;
		case EEventConditionParamTypes.Boolean:
			_conditionMode = EConditionType.BoolEqual;
			break;
		}
	}

	public object Clone()
	{
		return new EventCondition
		{
			BoolValue = BoolValue,
			FloatValue = FloatValue,
			IntValue = IntValue,
			_conditionMode = _conditionMode,
			_cachedNameHash = _cachedNameHash,
			ConditionMode = ConditionMode,
			ConditionParamType = ConditionParamType,
			ParameterName = ParameterName
		};
	}

	public void Serialize(BinaryWriter writer)
	{
		writer.Write(BoolValue);
		writer.Write(FloatValue);
		writer.Write(IntValue);
		writer.Write((short)_conditionMode);
		writer.Write(_cachedNameHash);
		writer.Write((short)ConditionMode);
		writer.Write((short)ConditionParamType);
		writer.Write(ParameterName);
	}

	public void Deserialize(BinaryReader reader)
	{
		BoolValue = reader.ReadBoolean();
		FloatValue = reader.ReadSingle();
		IntValue = reader.ReadInt32();
		_conditionMode = (EConditionType)reader.ReadInt16();
		_cachedNameHash = reader.ReadInt32();
		ConditionMode = (EEventConditionModes)reader.ReadInt16();
		ConditionParamType = (EEventConditionParamTypes)reader.ReadInt16();
		ParameterName = reader.ReadString();
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
		return Equals((EventCondition)obj);
	}

	public override int GetHashCode()
	{
		return (int)(((uint)(((((((((BoolValue.GetHashCode() * 397) ^ FloatValue.GetHashCode()) * 397) ^ IntValue) * 397) ^ ((ParameterName != null) ? ParameterName.GetHashCode() : 0)) * 397) ^ _cachedNameHash) * 397) ^ (uint)_conditionMode) * 397) ^ (int)ConditionParamType;
	}

	protected bool Equals(EventCondition other)
	{
		if (BoolValue == other.BoolValue && FloatValue.Equals(other.FloatValue) && IntValue == other.IntValue && ParameterName == other.ParameterName && _cachedNameHash == other._cachedNameHash && _conditionMode == other._conditionMode && ConditionParamType == other.ConditionParamType)
		{
			return ConditionMode == other.ConditionMode;
		}
		return false;
	}
}
