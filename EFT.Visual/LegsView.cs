using UnityEngine;

namespace EFT.Visual;

public class LegsView : MonoBehaviour
{
	[SerializeField]
	private bool _isRightLeg = true;

	[SerializeField]
	private Transform _holster;

	public bool IsRightLegHolster => _isRightLeg;

	public void SetHolster(PlayerBody playerBody)
	{
		playerBody.PlayerBones.AdjustPistolHolster(_holster, _isRightLeg);
	}
}
