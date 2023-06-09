using System.Threading.Tasks;
using UnityEngine;

namespace EFT;

internal sealed class ObservedQuickGrenadeThrowController : Player.QuickGrenadeThrowController, ObservedPlayer._E004
{
	private float _E055;

	internal new static ObservedQuickGrenadeThrowController _E000(ObservedPlayer player, _EADF item)
	{
		return Player.QuickGrenadeThrowController._E000<ObservedQuickGrenadeThrowController>(player, item);
	}

	internal static Task<ObservedQuickGrenadeThrowController> _E001(ObservedPlayer player, _EADF item)
	{
		return Player.QuickGrenadeThrowController._E001<ObservedQuickGrenadeThrowController>(player, item);
	}

	void ObservedPlayer._E004.ProcessPlayerPacket(_E733 framePlayerInfo)
	{
		_E954 grenadeThrowData = framePlayerInfo.GrenadePacket.GrenadeThrowData;
		if (grenadeThrowData.HasThrowData)
		{
			base._E00F(_E055, grenadeThrowData.ThrowGreanadePosition, grenadeThrowData.ThrowGrenadeRotation, grenadeThrowData.ThrowForce, grenadeThrowData.LowThrow);
		}
	}

	bool ObservedPlayer._E004.IsInIdleState()
	{
		return false;
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

	protected override void _E00F(float timeSinceSafetyLevelRemoved, Vector3 position, Quaternion rotation, Vector3 force, bool lowThrow)
	{
		_E055 = timeSinceSafetyLevelRemoved;
	}
}
