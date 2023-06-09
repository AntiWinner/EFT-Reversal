using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using TMPro;
using UnityEngine;

namespace EFT.Hideout;

public sealed class GroupBonusPanel : AbstractPanel<List<_E5EA>>
{
	[CompilerGenerated]
	private sealed class _E001
	{
		public BonusPanel view;

		internal void _E000(Result<Texture2D> result)
		{
			view.UpdateIcon(result.Value);
		}
	}

	[SerializeField]
	private BonusesPanelSettings _settings;

	[SerializeField]
	private BonusPanel _bonusPanel;

	[SerializeField]
	private TextMeshProUGUI _bonusesLabel;

	[SerializeField]
	private RectTransform _container;

	private _EC6D<_E5EA, BonusPanel> m__E000;

	private string[] m__E001;

	public async Task Show(RelatedData relatedData, Stage stage, ELevelType levelType, AreaData areaData, Player player, _E796 session, string[] localisationTexts)
	{
		this.m__E001 = localisationTexts;
		this.m__E000?.Dispose();
		await base.Show(relatedData, stage, levelType, areaData, player, session);
		UI.AddDisposable(areaData.LightStatusChanged.Subscribe(delegate
		{
			SetEnvironmentDetails();
		}));
		UI.AddDisposable(areaData.ProductionStateChanged.Subscribe(delegate
		{
			SetEnvironmentDetails();
		}));
	}

	public override async Task ShowContents()
	{
		base.gameObject.SetActive(base.Info.Any());
		_bonusesLabel.text = ((base.LevelType == ELevelType.Current) ? this.m__E001[0].Localized() : this.m__E001[1].Localized());
		this.m__E000 = UI.AddDisposable(new _EC6D<_E5EA, BonusPanel>());
		await this.m__E000.InitAsync(base.Info, (_E5EA arg) => _bonusPanel, (_E5EA arg) => _container, delegate(_E5EA item, BonusPanel view)
		{
			view.Show(item, _settings[item.TargetType]);
			view.UpdateView(base.AreaData.LightStatus, base.AreaData.HasActiveProduction, base.LevelType);
			if (!string.IsNullOrEmpty(item.Icon))
			{
				base.Session.LoadTextureMain(item.Icon, delegate(Result<Texture2D> result)
				{
					view.UpdateIcon(result.Value);
				});
			}
		});
	}

	public override void SetInfo()
	{
	}

	public void SetEnvironmentDetails()
	{
		if (this.m__E000 == null || this.m__E000.Count == 0)
		{
			return;
		}
		ELightStatus lightStatus = base.AreaData.LightStatus;
		bool hasActiveProduction = base.AreaData.HasActiveProduction;
		foreach (KeyValuePair<_E5EA, BonusPanel> item in this.m__E000)
		{
			_E39D.Deconstruct(item, out var _, out var value);
			value.UpdateView(lightStatus, hasActiveProduction, base.LevelType);
		}
	}

	[CompilerGenerated]
	private void _E000(ELightStatus status)
	{
		SetEnvironmentDetails();
	}

	[CompilerGenerated]
	private void _E001(bool state)
	{
		SetEnvironmentDetails();
	}

	[DebuggerHidden]
	[CompilerGenerated]
	private Task _E002(RelatedData relatedData, Stage stage, ELevelType levelType, AreaData areaData, Player player, _E796 session)
	{
		return base.Show(relatedData, stage, levelType, areaData, player, session);
	}

	[CompilerGenerated]
	private BonusPanel _E003(_E5EA arg)
	{
		return _bonusPanel;
	}

	[CompilerGenerated]
	private Transform _E004(_E5EA arg)
	{
		return _container;
	}

	[CompilerGenerated]
	private void _E005(_E5EA item, BonusPanel view)
	{
		view.Show(item, _settings[item.TargetType]);
		view.UpdateView(base.AreaData.LightStatus, base.AreaData.HasActiveProduction, base.LevelType);
		if (!string.IsNullOrEmpty(item.Icon))
		{
			base.Session.LoadTextureMain(item.Icon, delegate(Result<Texture2D> result)
			{
				view.UpdateIcon(result.Value);
			});
		}
	}
}
