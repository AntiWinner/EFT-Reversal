using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using UnityEngine;

namespace EFT.InputSystem;

[Serializable]
public sealed class KeyGroup
{
	public EGameKey keyName;

	public List<InputSource> variants;

	public EPressType pressType;

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	[DefaultValue(true)]
	public bool pressAvailable = true;

	[NonSerialized]
	public bool IgnorePool;

	[JsonIgnore]
	public bool PressTypeAvailable => variants.Any((InputSource keyVariant) => !keyVariant.IsEmpty);

	public KeyGroup Clone()
	{
		return new KeyGroup
		{
			keyName = keyName,
			variants = variants.ConvertAll((InputSource x) => x.Clone()),
			pressType = pressType
		};
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (obj is KeyGroup other)
		{
			return Equals(other);
		}
		return false;
	}

	public bool Equals(KeyGroup other)
	{
		if (keyName != other.keyName || pressType != other.pressType || variants.Count != other.variants.Count)
		{
			return false;
		}
		for (int i = 0; i < variants.Count; i++)
		{
			if (!variants[i].Equals(other.variants[i]))
			{
				return false;
			}
		}
		return true;
	}

	public static bool EqualityCheck(KeyGroup x, KeyGroup y)
	{
		return x.Equals(y);
	}

	public static KeyGroup CopyItem(KeyGroup item)
	{
		return item.Clone();
	}

	public static KeyGroup Create(EGameKey gameKey, EPressType press, KeyCode main, KeyCode? alt = null)
	{
		List<InputSource> list = new List<InputSource>(2) { _E000(main) };
		if (alt.HasValue)
		{
			list.Add(_E000(alt.Value));
		}
		return new KeyGroup
		{
			keyName = gameKey,
			pressType = press,
			variants = list
		};
	}

	public static KeyGroup CreateFrom(KeyGroup source, EGameKey gameKey, EPressType pressType, bool ignorePool)
	{
		KeyGroup keyGroup = source.Clone();
		keyGroup.keyName = gameKey;
		keyGroup.pressType = pressType;
		keyGroup.IgnorePool = ignorePool;
		return keyGroup;
	}

	public override string ToString()
	{
		return keyName.ToStringNoBox() + _ED3E._E000(168938) + string.Join(_ED3E._E000(10270), variants) + _ED3E._E000(11164);
	}

	[CompilerGenerated]
	internal static InputSource _E000(KeyCode keyCode)
	{
		return new InputSource
		{
			isAxis = false,
			keyCode = new List<KeyCode> { keyCode },
			axisName = null,
			positiveAxis = false,
			deadZone = 0f,
			sensitivity = 1f
		};
	}
}
