using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Matchmaker;

public sealed class MatchMakerPlayerPreview : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _EC99 matchmaker;

		public MatchMakerPlayerPreview _003C_003E4__this;

		internal void _E000()
		{
			matchmaker.OnMatchingTypeUpdate -= _003C_003E4__this._E001;
		}
	}

	private const int _E2A0 = 23;

	private const int _E2A1 = 19;

	[SerializeField]
	private PlayerModelView _playerModelView;

	[SerializeField]
	private Image _noVisualImage;

	[SerializeField]
	private RawImage _playerVisualImage;

	[SerializeField]
	private PlayerNamePanel _namePanel;

	[SerializeField]
	private ChatSpecialIcon _scavName;

	[SerializeField]
	private TextMeshProUGUI _groupStatusField;

	[SerializeField]
	private InteractionButtonsContainer _interactionButtons;

	[SerializeField]
	private PlayerLevelPanel _levelPanel;

	[SerializeField]
	private bool _isMainCharacter;

	private _E54F _E2A2;

	private _EC99 _E006;

	private _EC9B _E094;

	private _EC98 _E2A3;

	[CompilerGenerated]
	private string _E2A4;

	public string PlayerAid
	{
		[CompilerGenerated]
		get
		{
			return _E2A4;
		}
		[CompilerGenerated]
		private set
		{
			_E2A4 = value;
		}
	}

	public async Task Show(_EC99 matchmaker, RaidSettings raidSettings, _EC9B player, _EC98 contextInteractions)
	{
		UI.Dispose();
		ShowGameObject();
		_E006 = matchmaker;
		PlayerAid = player.AccountId;
		_E2A2 = player.Info;
		_E094 = player;
		bool flag = raidSettings.Side == ESideType.Savage;
		int textSize = (flag ? 19 : 23);
		_namePanel.Set(showDetails: true, _E2A2.MemberCategory, player.GetCorrectedNickname(), textSize);
		_levelPanel.Set(player.Info.Level, raidSettings.Side);
		_scavName.Show(EMemberCategory.Default, _E2A2.SavageNickname.Transliterate());
		_scavName.gameObject.SetActive(flag);
		_E2A3 = contextInteractions;
		if (_isMainCharacter && _E2A3 != null)
		{
			_interactionButtons.Show(_E2A3, null, null, null, null, autoClose: false);
		}
		else
		{
			_interactionButtons.Close();
		}
		if (!raidSettings.Local)
		{
			matchmaker.OnMatchingTypeUpdate += _E001;
			UI.AddDisposable(delegate
			{
				matchmaker.OnMatchingTypeUpdate -= _E001;
			});
		}
		UI.AddDisposable(_playerModelView);
		UI.AddDisposable(_interactionButtons);
		UI.AddDisposable(player.VisualChanged.Bind(_E000));
		UI.AddDisposable(player.StatusChanged.Bind(_E002));
	}

	private void _E000()
	{
		PlayerVisualRepresentation playerVisualRepresentation = _E094.PlayerVisualRepresentation;
		bool flag = playerVisualRepresentation != null && !playerVisualRepresentation.IsEmpty();
		_noVisualImage.gameObject.SetActive(!flag);
		_playerVisualImage.gameObject.SetActive(flag);
		if (flag)
		{
			_playerModelView.Show(_E094.PlayerVisualRepresentation).HandleExceptions();
		}
	}

	private void _E001(EMatchingType _)
	{
		_E002();
	}

	private void _E002()
	{
		EMatchingType matchingType = _E006.GetMatchingType(PlayerAid);
		bool readyStatus = _E006.GetReadyStatus(PlayerAid);
		switch (matchingType)
		{
		case EMatchingType.GroupLeader:
			_groupStatusField.text = _ED3E._E000(235105) + _ED3E._E000(235153).Localized() + _ED3E._E000(59467);
			break;
		case EMatchingType.GroupPlayer:
			_groupStatusField.text = (readyStatus ? (_ED3E._E000(235105) + _ED3E._E000(235171).Localized(EStringCase.Upper) + _ED3E._E000(59467)) : (_ED3E._E000(235142) + _ED3E._E000(235190).Localized(EStringCase.Upper) + _ED3E._E000(59467)));
			break;
		case EMatchingType.Single:
			_groupStatusField.text = string.Empty;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private void _E003(bool active)
	{
		if (!_isMainCharacter && _E2A3 != null)
		{
			if (active)
			{
				_interactionButtons.Show(_E2A3, null, null, null, null, autoClose: false);
			}
			else
			{
				_interactionButtons.Close();
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_E003(active: true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_E003(active: false);
	}
}
