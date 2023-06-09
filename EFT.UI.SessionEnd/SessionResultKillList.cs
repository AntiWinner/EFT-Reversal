using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InputSystem;
using EFT.InventoryLogic;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.UI.SessionEnd;

public sealed class SessionResultKillList : EftScreen<SessionResultKillList._E000, SessionResultKillList>
{
	public new sealed class _E000 : _EC90<_E000, SessionResultKillList>
	{
		public readonly _ED07<VictimStats> Victims;

		public readonly DogtagComponent[] Tags;

		public override EEftScreenType ScreenType => EEftScreenType.KillList;

		protected override bool MainEnvironment => false;

		public override bool KeyScreen => true;

		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.Disabled;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		public _E000(_ED07<VictimStats> victims, DogtagComponent[] tags)
		{
			Victims = victims;
			Tags = tags;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public DogtagComponent[] tags;

		public int index;

		internal void _E000(VictimStats victim, KillListVictim view)
		{
			bool knownName = tags.Select((DogtagComponent x) => x.ProfileId).Contains(victim.ProfileId);
			view.Show(victim, knownName, index);
			index++;
		}
	}

	[SerializeField]
	private KillListVictim _victimTemplate;

	[SerializeField]
	private RectTransform _container;

	[SerializeField]
	private GameObject _noKillsObject;

	[SerializeField]
	private DefaultUIButton _nextButton;

	[SerializeField]
	private DefaultUIButton _backButton;

	private void Awake()
	{
		_nextButton.OnClick.AddListener(delegate
		{
			ScreenController._E000();
		});
		_backButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
	}

	public override void Show(_E000 controller)
	{
		Show(controller.Victims, controller.Tags);
	}

	private void Show(_ED07<VictimStats> victims, DogtagComponent[] tags)
	{
		ShowGameObject();
		_noKillsObject.SetActive(victims.Count <= 0);
		int index = 1;
		UI.AddDisposable(new _EC71<VictimStats, KillListVictim>(victims, _victimTemplate, _container, delegate(VictimStats victim, KillListVictim view)
		{
			bool knownName = tags.Select((DogtagComponent x) => x.ProfileId).Contains(victim.ProfileId);
			view.Show(victim, knownName, index);
			index++;
		}));
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		return InputNode.GetDefaultBlockResult(command);
	}

	[CompilerGenerated]
	private void _E000()
	{
		ScreenController._E000();
	}

	[CompilerGenerated]
	private void _E001()
	{
		ScreenController.CloseScreen();
	}
}
