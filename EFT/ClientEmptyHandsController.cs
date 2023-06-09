using System.Threading.Tasks;

namespace EFT;

internal sealed class ClientEmptyHandsController : Player.EmptyHandsController
{
	public _E94E EmptyHandPacket;

	internal static ClientEmptyHandsController _E000(ClientPlayer player)
	{
		return Player.EmptyHandsController._E000<ClientEmptyHandsController>(player);
	}

	internal static Task<ClientEmptyHandsController> _E001(ClientPlayer player)
	{
		return Player.EmptyHandsController._E001<ClientEmptyHandsController>(player);
	}

	protected override void CompassStateHandler(bool isActive)
	{
		EmptyHandPacket.CompassPacket = new _E94A(isActive);
		base.CompassStateHandler(isActive);
	}

	public override void ShowGesture(EGesture gesture)
	{
		EmptyHandPacket.Gesture = gesture;
		base.ShowGesture(gesture);
	}

	public override void SetInventoryOpened(bool opened)
	{
		EmptyHandPacket.EnableInventoryPacket = new _E94F
		{
			EnableInventory = true,
			InventoryStatus = opened
		};
		base.SetInventoryOpened(opened);
	}
}
