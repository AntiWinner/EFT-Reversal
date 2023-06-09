using System;
using System.Globalization;
using System.IO;

namespace AnimationEventSystem;

[Serializable]
public sealed class AnimationEventParameter : ICloneable
{
	public bool BoolParam;

	public float FloatParam;

	public int IntParam;

	public string StringParam;

	public EAnimationEventParamType ParamType;

	public object Clone()
	{
		return new AnimationEventParameter
		{
			BoolParam = BoolParam,
			FloatParam = FloatParam,
			IntParam = IntParam,
			StringParam = StringParam,
			ParamType = ParamType
		};
	}

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((short)ParamType);
		if (ParamType != 0)
		{
			writer.Write(BoolParam);
			writer.Write(FloatParam);
			writer.Write(IntParam);
			writer.Write(StringParam);
		}
	}

	public void Deserialize(BinaryReader reader)
	{
		ParamType = (EAnimationEventParamType)reader.ReadInt16();
		if (ParamType != 0)
		{
			BoolParam = reader.ReadBoolean();
			FloatParam = reader.ReadSingle();
			IntParam = reader.ReadInt32();
			StringParam = reader.ReadString();
		}
	}

	public override string ToString()
	{
		return ParamType switch
		{
			EAnimationEventParamType.Int32 => IntParam.ToString(), 
			EAnimationEventParamType.Float => FloatParam.ToString(CultureInfo.InvariantCulture), 
			EAnimationEventParamType.String => StringParam, 
			EAnimationEventParamType.Boolean => BoolParam.ToString(), 
			EAnimationEventParamType.None => "", 
			_ => throw new ArgumentOutOfRangeException(), 
		};
	}

	public object GetParameter()
	{
		return ParamType switch
		{
			EAnimationEventParamType.Int32 => IntParam, 
			EAnimationEventParamType.Float => FloatParam, 
			EAnimationEventParamType.String => StringParam, 
			EAnimationEventParamType.Boolean => BoolParam, 
			_ => null, 
		};
	}

	public string GetSelectedParameterFieldName()
	{
		return ParamType switch
		{
			EAnimationEventParamType.Int32 => _ED3E._E000(124882), 
			EAnimationEventParamType.Float => _ED3E._E000(124875), 
			EAnimationEventParamType.String => _ED3E._E000(124926), 
			EAnimationEventParamType.Boolean => _ED3E._E000(124914), 
			_ => "", 
		};
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
		return Equals((AnimationEventParameter)obj);
	}

	public override int GetHashCode()
	{
		return (((((((BoolParam.GetHashCode() * 397) ^ FloatParam.GetHashCode()) * 397) ^ IntParam) * 397) ^ ((StringParam != null) ? StringParam.GetHashCode() : 0)) * 397) ^ (int)ParamType;
	}

	protected bool Equals(AnimationEventParameter other)
	{
		if (BoolParam == other.BoolParam && FloatParam.Equals(other.FloatParam) && IntParam == other.IntParam && StringParam == other.StringParam)
		{
			return ParamType == other.ParamType;
		}
		return false;
	}
}
