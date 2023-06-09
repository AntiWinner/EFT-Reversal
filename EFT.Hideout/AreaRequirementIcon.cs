using DG.Tweening;

namespace EFT.Hideout;

public sealed class AreaRequirementIcon : AreaIcon
{
	private AreaRequirement _E033;

	public void Show(AreaData data, AreaRequirement requirement)
	{
		UI.Dispose();
		_E033 = requirement;
		SetData(data);
	}

	protected override void SetLevel()
	{
		SetLevel(_E033.RequiredLevel);
	}

	protected override void SetStatus()
	{
		if (!(this == null))
		{
			BackImageFade?.Pause();
			base.BackgroundImage.sprite = (_E033.Fulfilled ? base.DefaultSprite : base.ErrorSprite);
			base.LockedIcon.SetActive(value: false);
			base.UnlockedIcon.SetActive(value: false);
			base.ReadyToUpgradeIcon.SetActive(value: false);
			base.ConstructingIcon.SetActive(value: false);
			base.UpgradingIcon.SetActive(value: false);
		}
	}
}
