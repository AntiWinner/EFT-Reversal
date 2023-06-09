using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI.Map;
using EFT.UI.Screens;
using JsonType;
using UnityEngine;

namespace EFT.UI.Matchmaker;

public sealed class MatchMakerSelectionLocationScreen : MatchmakerEftScreen<MatchMakerSelectionLocationScreen._E000, MatchMakerSelectionLocationScreen>
{
	public new sealed class _E000 : _EC8F<_E000, MatchMakerSelectionLocationScreen>
	{
		public readonly _E796 Session;

		public readonly RaidSettings RaidSettings;

		[CompilerGenerated]
		private new Action m__E000;

		public override EEftScreenType ScreenType => EEftScreenType.SelectLocation;

		public override bool KeyScreen => true;

		public event Action OnShowPocketMapScreen
		{
			[CompilerGenerated]
			add
			{
				Action action = this.m__E000;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = this.m__E000;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public _E000(_E796 session, ref RaidSettings raidSettings, _EC99 matchmakerPlayersController)
			: base(matchmakerPlayersController)
		{
			Session = session;
			RaidSettings = raidSettings;
			UpdateSelectedDateTime();
		}

		internal new void _E000()
		{
			this.m__E000?.Invoke();
		}

		public void UpdateSelectedDateTime()
		{
			RaidSettings.SelectedDateTime = (_E554.Location.AvailableMaps.Contains(RaidSettings.SelectedLocation?.Id) ? RaidSettings.SelectedDateTime : EDateTime.CURR);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E554 locations;

		public MatchMakerSelectionLocationScreen _003C_003E4__this;

		public _E554.Location selectedLocation;

		internal _E051<_E554.Location, _E554.Location> _E000(_E554._E001 x)
		{
			return new _E051<_E554.Location, _E554.Location>(locations.locations[x.Source], locations.locations[x.Destination]);
		}

		internal void _E001(_E554.Location location, LocationButton locationButton)
		{
			locationButton.Show(location, _003C_003E4__this.m__E003, location == selectedLocation || (selectedLocation != null && location.Id == _ED3E._E000(124512) && selectedLocation.Id == _ED3E._E000(124565)), _003C_003E4__this.m__E002.Info.Level, isOverloaded: false, isNew: false, _003C_003E4__this._E000);
			if (location == _003C_003E4__this._E006)
			{
				locationButton.Select(value: true);
			}
		}

		internal void _E002()
		{
			_003C_003E4__this._conditions.OnTimeChanged -= _003C_003E4__this._E001;
		}
	}

	private new const string m__E000 = "MapPointConfigs/";

	[SerializeField]
	private DefaultUIButton _acceptButton;

	[SerializeField]
	private DefaultUIButton _backButton;

	[SerializeField]
	private DefaultUIButton _readyButton;

	[SerializeField]
	private DefaultUIButton _pocketMapButton;

	[SerializeField]
	private LocationButton _locationButtonTemplate;

	[SerializeField]
	private LocationPath _locationPathTemplate;

	[SerializeField]
	private Transform _locationButtonsContainer;

	[SerializeField]
	private Transform _locationPathsContainer;

	[SerializeField]
	private LocationInfoPanel _infoPanel;

	[SerializeField]
	private LocationConditionsPanel _conditions;

	private _E554.Location m__E001;

	private Profile m__E002;

	private ESideType m__E003;

	private EDateTime m__E004;

	private RaidSettings m__E005;

	private _E554.Location _E006
	{
		get
		{
			return this.m__E001;
		}
		set
		{
			this.m__E001 = value;
			string text = this.m__E001?.Id;
			if (!(text == _ED3E._E000(124565)))
			{
				if (text == _ED3E._E000(124512) || text == _ED3E._E000(124636))
				{
					this.m__E004 = EDateTime.CURR;
				}
			}
			else
			{
				this.m__E004 = EDateTime.PAST;
			}
			this.m__E005.SelectedLocation = this.m__E001;
			this.m__E005.SelectedDateTime = this.m__E004;
			ScreenController.UpdateSelectedDateTime();
			bool flag = this.m__E001 != null && !this.m__E001.Locked && this.m__E002.Info.Level >= this.m__E001.RequiredPlayerLevel && (this.m__E003 != ESideType.Savage || !this.m__E001.DisabledForScav);
			_acceptButton.gameObject.SetActive(flag);
			_readyButton.gameObject.SetActive(flag);
			_infoPanel.gameObject.SetActive(this.m__E001 != null);
			if (!flag)
			{
				_conditions.Close();
			}
			MapPoints mapPoints = ((this.m__E001 != null) ? _E905.Pop<MapPoints>(_ED3E._E000(140841) + this.m__E001.Id + ((this.m__E003 == ESideType.Savage) ? _ED3E._E000(140890) : string.Empty)) : null);
			_pocketMapButton.gameObject.SetActive(mapPoints != null);
		}
	}

	private void Awake()
	{
		_acceptButton.OnClick.AddListener(delegate
		{
			((_EC90<_E000, MatchMakerSelectionLocationScreen>)ScreenController)._E000();
		});
		_backButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
		_readyButton.OnClick.AddListener(delegate
		{
			((_EC8F<_E000, MatchMakerSelectionLocationScreen>)ScreenController)._E000();
		});
		_pocketMapButton.OnClick.AddListener(delegate
		{
			ScreenController._E000();
		});
	}

	public override void Show(_E000 controller)
	{
		base.Show(controller);
		Show(controller.Session, controller.RaidSettings);
	}

	private void Show(_E796 session, RaidSettings raidSettings)
	{
		this.m__E002 = session.Profile;
		this.m__E005 = raidSettings;
		this.m__E003 = raidSettings.Side;
		this.m__E004 = raidSettings.SelectedDateTime;
		_E554 locations = session.LocationSettings;
		_E554.Location selectedLocation = this.m__E005.SelectedLocation;
		_E000(selectedLocation);
		ShowGameObject();
		IEnumerable<_E051<_E554.Location, _E554.Location>> source = locations.paths.Select((_E554._E001 x) => new _E051<_E554.Location, _E554.Location>(locations.locations[x.Source], locations.locations[x.Destination]));
		UI.AddViewList(source.Where((_E051<_E554.Location, _E554.Location> x) => x.Source.Enabled && x.Destination.Enabled), _locationPathTemplate, _locationPathsContainer, delegate(_E051<_E554.Location, _E554.Location> path, LocationPath pathView)
		{
			pathView.Show(path.Source, path.Destination);
		});
		UI.AddViewList(locations.locations.Values.Where(_E003), _locationButtonTemplate, _locationButtonsContainer, delegate(_E554.Location location, LocationButton locationButton)
		{
			locationButton.Show(location, this.m__E003, location == selectedLocation || (selectedLocation != null && location.Id == _ED3E._E000(124512) && selectedLocation.Id == _ED3E._E000(124565)), this.m__E002.Info.Level, isOverloaded: false, isNew: false, _E000);
			if (location == this._E006)
			{
				locationButton.Select(value: true);
			}
		});
		_conditions.OnTimeChanged += _E001;
		UI.AddDisposable(delegate
		{
			_conditions.OnTimeChanged -= _E001;
		});
	}

	private void _E000(_E554.Location location)
	{
		if (location == null)
		{
			this._E006 = null;
			return;
		}
		_E796 session = ScreenController.Session;
		_E002(location, this.m__E005.SelectedDateTime);
		this._E006 = location;
		if (_E554.Location.NightTimeAllowedLocations.Contains(location.Id))
		{
			_conditions.Set(session, this.m__E005, takeFromCurrent: true);
		}
		else
		{
			_conditions.Close();
		}
		_infoPanel.Set(location, this.m__E003, this.m__E002.Info.Level);
	}

	private void _E001(EDateTime time)
	{
		this.m__E004 = time;
		this.m__E005.SelectedDateTime = this.m__E004;
		ScreenController.UpdateSelectedDateTime();
		_E002(this.m__E005.SelectedLocation, this.m__E004);
	}

	private void _E002(_E554.Location location, EDateTime time)
	{
		if (!(location.Id != _ED3E._E000(124512)) || !(location.Id != _ED3E._E000(124565)))
		{
			_E554 locationSettings = ScreenController.Session.LocationSettings;
			_E554.Location location2 = locationSettings.locations.Values.FirstOrDefault((_E554.Location x) => x.Id == _ED3E._E000(124512));
			_E554.Location location3 = locationSettings.locations.Values.FirstOrDefault((_E554.Location x) => x.Id == _ED3E._E000(124565));
			this._E006 = ((time == EDateTime.CURR) ? location2 : location3);
		}
	}

	private static bool _E003(_E554.Location location)
	{
		if (location.Enabled)
		{
			return location.Id != _ED3E._E000(124565);
		}
		return false;
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape))
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
			ScreenController.CloseScreen();
			return ETranslateResult.BlockAll;
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	[CompilerGenerated]
	private void _E004()
	{
		((_EC90<_E000, MatchMakerSelectionLocationScreen>)ScreenController)._E000();
	}

	[CompilerGenerated]
	private void _E005()
	{
		ScreenController.CloseScreen();
	}

	[CompilerGenerated]
	private void _E006()
	{
		((_EC8F<_E000, MatchMakerSelectionLocationScreen>)ScreenController)._E000();
	}

	[CompilerGenerated]
	private void _E007()
	{
		ScreenController._E000();
	}
}
