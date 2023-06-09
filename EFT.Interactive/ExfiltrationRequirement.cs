using System;
using EFT.InventoryLogic;
using JetBrains.Annotations;

namespace EFT.Interactive;

[Serializable]
public class ExfiltrationRequirement : IExfiltrationRequirement
{
	public ERequirementState Requirement;

	public string Id;

	public int Count;

	public EquipmentSlot RequiredSlot;

	public string RequirementTip;

	[CanBeNull]
	public static IExfiltrationRequirement CreateRequirement(ERequirementState requirementState)
	{
		return requirementState switch
		{
			ERequirementState.None => null, 
			ERequirementState.Empty => new _EBF2(), 
			ERequirementState.TransferItem => new _EBF1(), 
			ERequirementState.WorldEvent => new _EBF9(), 
			ERequirementState.ScavCooperation => new _EBF8(), 
			ERequirementState.Reference => null, 
			ERequirementState.SkillLevel => new _EBF7(), 
			ERequirementState.HasItem => new _EBF5(), 
			ERequirementState.EmptyOrSize => new _EBF4(), 
			ERequirementState.Train => new _EBF6(), 
			ERequirementState.Timer => new _EBF3(), 
			_ => throw new ArgumentOutOfRangeException(_ED3E._E000(212478), requirementState, null), 
		};
	}

	public virtual string GetLocalizedTip(string profileId)
	{
		return string.Format(RequirementTip.Localized(), (Id + _ED3E._E000(182596)).Localized()) + string.Format(_ED3E._E000(47265), Count);
	}

	public virtual bool Met(Player player, ExfiltrationPoint point)
	{
		return true;
	}

	public virtual void Enter(Player player, ExfiltrationPoint point)
	{
	}

	public virtual void Exit(Player player, ExfiltrationPoint point)
	{
	}

	public virtual void Start(ExfiltrationPoint point)
	{
	}

	public virtual void OnDestroy()
	{
	}
}
