using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI;

public class SkillsAndMasteringScreen : UIElement
{
	public sealed class _E000 : _EC64<SkillsAndMasteringScreen>
	{
		private new readonly Profile m__E000;

		private readonly _EAE6 _E001;

		private readonly _E9C4 _E002;

		public _E000(SkillsAndMasteringScreen skillsTab, Profile profile, _EAE6 inventoryController, _E9C4 healthController)
			: base(skillsTab)
		{
			m__E000 = profile;
			_E001 = inventoryController;
			_E002 = healthController;
		}

		public override void Show()
		{
			base._E000.Show(m__E000, _E001, _E002);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Profile profile;

		public _E9C4 healthController;

		public _EAE6 inventoryController;

		public SkillsAndMasteringScreen _003C_003E4__this;

		internal void _E000(SkillsScreen x)
		{
			x.Show(profile, healthController);
		}

		internal void _E001(MasteringScreen x)
		{
			x.Show(profile, inventoryController);
		}

		internal void _E002()
		{
			_003C_003E4__this._E1AA.TryHide();
		}
	}

	[SerializeField]
	private PlayerExperiencePanel _playerExperiencePanel;

	[SerializeField]
	private SkillsScreen _skillsScreen;

	[SerializeField]
	private MasteringScreen _masteringScreen;

	[SerializeField]
	private Tab _skillsTab;

	[SerializeField]
	private Tab _masteringTab;

	private _EC67 _E1AA;

	private void Awake()
	{
		_E1AA = new _EC67(new Tab[2] { _skillsTab, _masteringTab }, _skillsTab);
	}

	private void Show(Profile profile, _EAE6 inventoryController, _E9C4 healthController)
	{
		inventoryController.StopProcesses();
		ItemUiContext.Instance.CloseAllWindows();
		ShowGameObject();
		_playerExperiencePanel.Set(profile.Info.Experience, profile.Info.Experience);
		_skillsTab.Init(new _EC66<SkillsScreen>(_skillsScreen, delegate(SkillsScreen x)
		{
			x.Show(profile, healthController);
		}));
		_masteringTab.Init(new _EC66<MasteringScreen>(_masteringScreen, delegate(MasteringScreen x)
		{
			x.Show(profile, inventoryController);
		}));
		UI.AddDisposable(delegate
		{
			_E1AA.TryHide();
		});
		_E1AA.Show(null);
	}
}
