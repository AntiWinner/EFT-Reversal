using System;
using System.Threading.Tasks;
using EFT.Ballistics;
using EFT.InventoryLogic;

namespace EFT;

internal sealed class ClientQuickKnifeKickController : Player.QuickKnifeKickController
{
	public _E959 KnifePacket;

	internal new static ClientQuickKnifeKickController _E000(ClientPlayer player, KnifeComponent knife)
	{
		return Player.QuickKnifeKickController._E000<ClientQuickKnifeKickController>(player, knife);
	}

	internal new static Task<ClientQuickKnifeKickController> _E001(ClientPlayer player, string itemId)
	{
		KnifeComponent knifeComponent = (string.IsNullOrEmpty(itemId) ? null : player._E0DE.FindItem(itemId).GetItemComponent<KnifeComponent>());
		if (knifeComponent == null)
		{
			throw new Exception(_ED3E._E000(192615));
		}
		return Player.QuickKnifeKickController._E001<ClientQuickKnifeKickController>(player, knifeComponent);
	}

	protected override _E6FF _E00B(Player._E00D hit, BallisticCollider ballisticCollider)
	{
		_E6FF obj = base._E00B(hit, ballisticCollider);
		KnifePacket.AddHit(hit, ballisticCollider, obj, LastKickType);
		return obj;
	}
}
