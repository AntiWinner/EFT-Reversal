using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

[Serializable]
public class SideSelectionState : _EC44
{
	private const string BEAR_DESCRIPTION = "Select BEAR Character";

	private const string USEC_DESCRIPTION = "Select USEC Character";

	[SerializeField]
	private CustomTextMeshProUGUI _descriptionLabel;

	[SerializeField]
	private ToggleGroup _toggleGroup;

	private Dictionary<EPlayerSide, PlayerProfilePreview> _previews;

	private Dictionary<EPlayerSide, string> _sideDescriptions = new Dictionary<EPlayerSide, string>
	{
		{
			EPlayerSide.Bear,
			_ED3E._E000(253445)
		},
		{
			EPlayerSide.Usec,
			_ED3E._E000(253491)
		}
	};

	private _E77E._E000 _profileData;

	public void Init(_E77E._E000 profileData, Profile bearProfile, Profile usecProfile, Dictionary<EPlayerSide, PlayerProfilePreview> previews)
	{
		_profileData = profileData;
		_previews = previews;
		_E002(string.Empty);
		foreach (PlayerProfilePreview value in _previews.Values)
		{
			value.ChangeCameraPosition(PlayerProfilePreview.ECameraViewType.FullBody, 0f).HandleExceptions();
			value.SetVisibility(visible: true, 0f);
			value.SetGodLightStatus(active: false, 0f);
			value.HoverTrigger.Init(_E001);
		}
		_E000(active: true);
	}

	public override async Task ShowState()
	{
		_E000(active: true);
		Task task = base.ShowState();
		foreach (var (ePlayerSide2, playerProfilePreview2) in _previews)
		{
			playerProfilePreview2.ChangeCameraPosition(PlayerProfilePreview.ECameraViewType.FullBody, PreviewAnimationDuration).HandleExceptions();
			playerProfilePreview2.SetVisibility(visible: true, PreviewAnimationDuration);
			playerProfilePreview2.SetGodLightStatus(_profileData.HasSide && _profileData.Side == ePlayerSide2, PreviewAnimationDuration);
		}
		await task;
		if (_profileData.HasSide)
		{
			_E001(_profileData.Side);
			StateReady = true;
		}
	}

	private void _E000(bool active)
	{
		foreach (PlayerProfilePreview value in _previews.Values)
		{
			value.Selectable = active;
		}
	}

	public override async Task HideState()
	{
		_E000(active: false);
		await base.HideState();
	}

	private void _E001(EPlayerSide side)
	{
		if (side == EPlayerSide.Savage)
		{
			return;
		}
		_profileData.Side = side;
		_E002(_sideDescriptions[side].Localized());
		foreach (var (ePlayerSide2, playerProfilePreview2) in _previews)
		{
			playerProfilePreview2.SetLightStatus(ePlayerSide2 == side);
		}
		StateReady = true;
	}

	private void _E002(string text)
	{
		_descriptionLabel.text = text;
	}

	[DebuggerHidden]
	[CompilerGenerated]
	private Task _E003()
	{
		return base.ShowState();
	}

	[DebuggerHidden]
	[CompilerGenerated]
	private Task _E004()
	{
		return base.HideState();
	}
}
