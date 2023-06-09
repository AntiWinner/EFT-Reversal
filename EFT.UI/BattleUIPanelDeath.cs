using System;
using UnityEngine;

namespace EFT.UI;

public class BattleUIPanelDeath : BattleUIPanel
{
	[SerializeField]
	private CustomTextMeshProUGUI _nickname;

	[SerializeField]
	private CustomTextMeshProUGUI _time;

	[SerializeField]
	private GameObject _survivedPanel;

	[SerializeField]
	private GameObject _leftPanel;

	[SerializeField]
	private GameObject _missingPanel;

	[SerializeField]
	private GameObject _killedPanel;

	[SerializeField]
	private GameObject _runnerPanel;

	[SerializeField]
	private BattleUIPanelExitTrigger _battleUiPanelExitTrigger;

	private Coroutine _E09E;

	public void Show(Profile activeProfile, ExitStatus exitStatus, TimeSpan playTime)
	{
		using (_E069.StartWithToken(_ED3E._E000(250410)))
		{
			ShowGameObject();
			_battleUiPanelExitTrigger.Close();
			if (base.gameObject.activeInHierarchy)
			{
				_E09E = StartCoroutine(Co_ShowInfoPanel(1f, 0.8f));
			}
			_nickname.text = activeProfile.GetCorrectedNickname();
			_time.text = string.Format(_ED3E._E000(250450), playTime.Minutes, playTime.Seconds);
			_survivedPanel.SetActive(exitStatus == ExitStatus.Survived);
			_leftPanel.SetActive(exitStatus == ExitStatus.Left);
			_missingPanel.SetActive(exitStatus == ExitStatus.MissingInAction);
			_killedPanel.SetActive(exitStatus == ExitStatus.Killed);
			_runnerPanel.SetActive(exitStatus == ExitStatus.Runner);
		}
	}

	protected void StopShowingPanel()
	{
		if (_E09E != null)
		{
			StopCoroutine(_E09E);
		}
	}

	public override void Close()
	{
		base.Close();
		StopShowingPanel();
	}
}
