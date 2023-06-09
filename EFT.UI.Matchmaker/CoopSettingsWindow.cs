using UnityEngine;

namespace EFT.UI.Matchmaker;

public sealed class CoopSettingsWindow : BaseUiWindow
{
	[SerializeField]
	private MatchmakerRaidSettingsSummaryView _summaryView;

	public void Show(RaidSettings raidSettings)
	{
		ShowGameObject();
		_summaryView.Show(raidSettings);
		UI.AddDisposable(_summaryView.Close);
	}
}
