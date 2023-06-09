using System;
using Newtonsoft.Json;

namespace Bsg.GameSettings;

[JsonConverter(typeof(_E472))]
public interface IGameSetting
{
	string Key { get; }

	bool IsAvailableToEdit { get; set; }

	object ObjectValue { get; set; }

	Type ObjectType { get; }

	bool HasSameValue(IGameSetting other);

	void TakeValueFrom(IGameSetting other);

	void ForceApply();
}
