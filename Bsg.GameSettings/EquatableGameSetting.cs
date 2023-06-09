using System;
using System.Threading.Tasks;

namespace Bsg.GameSettings;

[Serializable]
public class EquatableGameSetting<T> : StateGameSetting<T> where T : IEquatable<T>
{
	internal EquatableGameSetting(string key, T defaultValue, Func<T, Task<T>> asyncPreProcessor)
		: base(key, defaultValue, asyncPreProcessor)
	{
	}

	internal EquatableGameSetting(string key, T defaultValue, Func<T, T> preProcessor)
		: base(key, defaultValue, preProcessor)
	{
	}

	public override bool HasSameValue(GameSetting<T> other)
	{
		if (GameSetting<T>.IsNullable)
		{
			bool flag = base.Value == null;
			bool flag2 = other.Value == null;
			if (flag || flag2)
			{
				return flag == flag2;
			}
		}
		return other.Value.Equals(base.Value);
	}
}
