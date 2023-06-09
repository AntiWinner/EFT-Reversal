using System;
using System.Threading.Tasks;
using EFT.InventoryLogic;

namespace EFT;

internal sealed class ClientMedsController : Player.MedsController
{
	internal static ClientMedsController _E000(ClientPlayer player, Item item, EBodyPart bodyPart, float amount, int animationVariant)
	{
		return Player.MedsController._E000<ClientMedsController>(player, item, bodyPart, amount, animationVariant);
	}

	internal static Task<ClientMedsController> _E001(ClientPlayer player, string itemId, EBodyPart bodyPart, float amount, int animationVariant)
	{
		Item item = (string.IsNullOrEmpty(itemId) ? null : player._E0DE.FindItem(itemId));
		if (item == null)
		{
			throw new Exception(_ED3E._E000(192615));
		}
		return Player.MedsController._E001<ClientMedsController>(player, item, bodyPart, amount, animationVariant);
	}
}
