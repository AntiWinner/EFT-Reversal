using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace EFT.Hideout;

[Serializable]
public sealed class BodyPartBuffRequirement : Requirement
{
	[JsonProperty("EffectName")]
	private string _effectName;

	[JsonProperty("BodyPart")]
	private string _bodyPart;

	[JsonProperty("Excluded")]
	private bool _excluded;

	private _E981 _healthController;

	public override ERequirementType Type => ERequirementType.BodyPartBuff;

	[JsonIgnore]
	private EBodyPart _E000 => (EBodyPart)Enum.Parse(typeof(EBodyPart), _bodyPart);

	public override bool Fulfilled => _E000() == !_excluded;

	private bool _E000()
	{
		return (from effect in _healthController.GetAllActiveEffects(this._E000)
			select effect.Type.ToString() == _effectName).FirstOrDefault();
	}

	public void Test(_E981 healthController)
	{
		_healthController = healthController;
	}

	[CompilerGenerated]
	private bool _E001(_E992 effect)
	{
		return effect.Type.ToString() == _effectName;
	}
}
