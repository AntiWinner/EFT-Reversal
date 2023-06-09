using System.Threading.Tasks;
using EFT.Hideout;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class TraderRequirementIcon : UIElement
{
	[SerializeField]
	private Image _avatar;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private RankPanel _rankPanel;

	public void Show(_E83C requirement)
	{
		if (requirement is TraderLoyaltyRequirement traderLoyaltyRequirement)
		{
			_rankPanel.Show(traderLoyaltyRequirement.LoyaltyLevel, requirement.Trader.MaxLoyaltyLevel);
		}
		else
		{
			_rankPanel.Close();
		}
		ShowGameObject();
		_E000(requirement.Trader.Settings).HandleExceptions();
	}

	private async Task _E000(_E5CB.TraderSettings traderSettings)
	{
		if (await traderSettings.GetAndAssignAvatar(_avatar, base.CancellationToken))
		{
			if (_loader != null)
			{
				_loader.SetActive(value: false);
			}
			if (_avatar != null)
			{
				_avatar.gameObject.SetActive(value: true);
			}
		}
	}
}
