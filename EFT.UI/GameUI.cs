using EFT.UI.Gestures;
using EFT.UI.Screens;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI;

[UsedImplicitly]
public sealed class GameUI : MonoBehaviourSingleton<GameUI>
{
	[SerializeField]
	public BattleUIScreen BattleUiScreen;

	[SerializeField]
	public ExtractionTimersPanel TimerPanel;

	[SerializeField]
	public BattleUIPanelDeath BattleUiPanelDeath;

	[SerializeField]
	public GameObject BattleUiInvalidEPPanel;

	[SerializeField]
	public BattleUIPanelExitTrigger BattleUiPanelExitTrigger;

	[SerializeField]
	public BattleUIPanelExtraction BattleUiPanelExtraction;

	[SerializeField]
	public GesturesMenu BattleUIGesturesMenu;

	[SerializeField]
	public BattleUIPmcCount BattleUiPmcCount;

	[SerializeField]
	public GesturesQuickPanel GesturesQuickPanel;

	[SerializeField]
	public PostFXPreviewScreen PostFXPreview;

	public override void Awake()
	{
		_EC92.Instance.RegisterScreen(EEftScreenType.BattleUI, BattleUiScreen);
		base.Awake();
	}

	public override void OnDestroy()
	{
		_EC92.Instance.ReleaseScreen(EEftScreenType.BattleUI, BattleUiScreen);
		base.OnDestroy();
	}
}
