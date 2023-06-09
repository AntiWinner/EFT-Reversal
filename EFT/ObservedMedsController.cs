using System.Threading.Tasks;
using EFT.InventoryLogic;

namespace EFT;

internal sealed class ObservedMedsController : Player.MedsController, ObservedPlayer._E004
{
	internal static ObservedMedsController _E000(ObservedPlayer player, Item item, EBodyPart bodyPart, float amount, int animationVariant)
	{
		return Player.MedsController._E000<ObservedMedsController>(player, item, bodyPart, amount, animationVariant);
	}

	internal static Task<ObservedMedsController> _E001(ObservedPlayer player, Item item, EBodyPart bodyPart, float amount, int animationVariant)
	{
		return Player.MedsController._E001<ObservedMedsController>(player, item, bodyPart, amount, animationVariant);
	}

	protected override bool CanChangeCompassState(bool newState)
	{
		return false;
	}

	protected override void OnCanUsePropChanged(bool canUse)
	{
	}

	public override void SetCompassState(bool active)
	{
	}

	void ObservedPlayer._E004.ProcessPlayerPacket(_E733 framePlayerInfo)
	{
	}

	bool ObservedPlayer._E004.IsInIdleState()
	{
		return true;
	}
}
