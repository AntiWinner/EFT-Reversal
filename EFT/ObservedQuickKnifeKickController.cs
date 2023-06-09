using System.Threading.Tasks;
using EFT.InventoryLogic;

namespace EFT;

internal sealed class ObservedQuickKnifeKickController : Player.QuickKnifeKickController
{
	internal new static ObservedQuickKnifeKickController _E000(ObservedPlayer player, KnifeComponent knife)
	{
		return Player.QuickKnifeKickController._E000<ObservedQuickKnifeKickController>(player, knife);
	}

	internal new static Task<ObservedQuickKnifeKickController> _E001(ObservedPlayer player, KnifeComponent knife)
	{
		return Player.QuickKnifeKickController._E001<ObservedQuickKnifeKickController>(player, knife);
	}
}
