using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

[Serializable]
public class HeadSelectionState : _EC44
{
	[CompilerGenerated]
	private sealed class _E001
	{
		public _E373<EquipmentSlot, PlayerBody._E000> slotViews;

		internal bool _E000(EquipmentSlot slotType)
		{
			return slotViews.ContainsKey(slotType);
		}

		internal GameObject _E001(EquipmentSlot slotType)
		{
			return slotViews.GetByKey(slotType).ParentedModel.Value;
		}
	}

	[SerializeField]
	private DropDownBox _headSelector;

	[SerializeField]
	private DropDownBox _voiceSelector;

	private readonly List<EquipmentSlot> _hiddenSlots = new List<EquipmentSlot>
	{
		EquipmentSlot.Earpiece,
		EquipmentSlot.Eyewear,
		EquipmentSlot.FaceCover,
		EquipmentSlot.Headwear
	};

	private string[] _availableCustomizations;

	private Profile _previewProfile;

	private List<KeyValuePair<string, _EBDF>> _headTemplates;

	private List<KeyValuePair<string, _EBE9>> _voiceTemplates;

	private Dictionary<int, TagBank> _voices = new Dictionary<int, TagBank>();

	private int _selectedHeadIndex;

	private int _selectedVoiceIndex;

	private PlayerProfilePreview _preview;

	private _E77E._E000 _profileData;

	private Dictionary<EPlayerSide, PlayerProfilePreview> _previews;

	protected override float TransitionDuration => 0.8f;

	public void Init(_E77E._E000 profileData, string[] availableCustomizations, Dictionary<EPlayerSide, PlayerProfilePreview> previews)
	{
		_profileData = profileData;
		_availableCustomizations = availableCustomizations;
		_previews = previews;
	}

	public override async Task ShowState()
	{
		_preview = _previews[_profileData.Side];
		_previewProfile = _preview.Profile.Clone();
		_E002();
		if (_preview.PlayerModelView.LoadingComplete)
		{
			_E000();
		}
		else
		{
			_preview.PlayerModelView.LoadingCompletedEvent += _E000;
		}
		foreach (var (ePlayerSide2, playerProfilePreview2) in _previews)
		{
			playerProfilePreview2.SetVisibility(ePlayerSide2 == _profileData.Side, PreviewAnimationDuration);
		}
		await Task.WhenAll(base.ShowState(), _preview.ChangeCameraPosition(PlayerProfilePreview.ECameraViewType.Head, PreviewAnimationDuration));
		_headSelector.enabled = true;
		_voiceSelector.enabled = true;
		_preview.RotationEnabled = true;
		_preview.DragTrigger.onDrag += _E006;
	}

	private void _E000()
	{
		_E001(active: false);
	}

	private void _E001(bool active)
	{
		_E373<EquipmentSlot, PlayerBody._E000> slotViews = _preview.PlayerModelView.PlayerBody.SlotViews;
		foreach (GameObject item in from slotType in _hiddenSlots
			where slotViews.ContainsKey(slotType)
			select slotViews.GetByKey(slotType).ParentedModel.Value into model
			where model != null
			select model)
		{
			item.SetActive(active);
		}
	}

	private void _E002()
	{
		_headSelector.Bind(_E007);
		_voiceSelector.Bind(_E008);
		_E60E instance = Singleton<_E60E>.Instance;
		_headTemplates = new List<KeyValuePair<string, _EBDF>>();
		_voiceTemplates = new List<KeyValuePair<string, _EBE9>>();
		string[] availableCustomizations = _availableCustomizations;
		foreach (string text in availableCustomizations)
		{
			_EBDE anyCustomizationItem = instance.GetAnyCustomizationItem(text);
			if (anyCustomizationItem == null || !anyCustomizationItem.Side.Contains(_preview.Side))
			{
				continue;
			}
			if (anyCustomizationItem is _EBDF obj)
			{
				if (obj.BodyPart == EBodyModelPart.Head)
				{
					_headTemplates.Add(new KeyValuePair<string, _EBDF>(text, obj));
				}
			}
			else if (anyCustomizationItem is _EBE9 value)
			{
				_voiceTemplates.Add(new KeyValuePair<string, _EBE9>(text, value));
			}
		}
		_voices.Clear();
		_E004();
		_E005();
	}

	private async Task _E003()
	{
		await _preview.Show(_previewProfile);
		_E001(active: false);
	}

	private void _E004()
	{
		string text = _previewProfile.Customization[EBodyModelPart.Head];
		int i;
		for (i = 0; i < _headTemplates.Count && !(_headTemplates[i].Key == text); i++)
		{
		}
		_selectedHeadIndex = i;
		_headSelector.Show(() => _headTemplates.Select((KeyValuePair<string, _EBDF> x) => x.Value.NameLocalizationKey.Localized()).ToArray());
		_headSelector.UpdateValue(_selectedHeadIndex, sendCallback: false);
		_profileData.HeadId = _headTemplates[_selectedHeadIndex].Key;
	}

	private void _E005()
	{
		_selectedVoiceIndex = 0;
		_voiceSelector.Show(() => _voiceTemplates.Select((KeyValuePair<string, _EBE9> x) => x.Value.NameLocalizationKey.Localized()).ToArray());
		_profileData.VoiceId = _voiceTemplates[_selectedVoiceIndex].Key;
	}

	public override async Task HideState()
	{
		_preview.DragTrigger.onDrag -= _E006;
		_preview.RotationEnabled = false;
		_headSelector.Hide();
		_voiceSelector.Hide();
		_headSelector.enabled = false;
		_voiceSelector.enabled = false;
		await base.HideState();
		if (_preview.PlayerModelView.LoadingComplete)
		{
			_E001(active: true);
		}
	}

	private void _E006(PointerEventData pointerData)
	{
		_preview.Rotator.Rotate(0f - pointerData.delta.x);
	}

	private void _E007(int selectedIndex)
	{
		if (selectedIndex != _selectedHeadIndex)
		{
			_selectedHeadIndex = selectedIndex;
			string key = _headTemplates[_selectedHeadIndex].Key;
			_previewProfile.Customization[EBodyModelPart.Head] = key;
			_profileData.HeadId = key;
			_E003().HandleExceptions();
		}
	}

	private void _E008(int selectedIndex)
	{
		_selectedVoiceIndex = selectedIndex;
		_E009(selectedIndex).HandleExceptions();
	}

	private async Task _E009(int selectedIndex)
	{
		if (!_voices.TryGetValue(selectedIndex, out var _))
		{
			TagBank tagBank = await Singleton<_E3C1>.Instance.TakeVoice(_voiceTemplates[_selectedVoiceIndex].Value.Name);
			_voices.Add(selectedIndex, tagBank);
			if (tagBank == null)
			{
				return;
			}
		}
		_profileData.VoiceId = _voiceTemplates[_selectedVoiceIndex].Key;
		int num = UnityEngine.Random.Range(0, _voices[selectedIndex].Clips.Length);
		TaggedClip taggedClip = _voices[selectedIndex].Clips[num];
		await Singleton<GUISounds>.Instance.ForcePlaySound(taggedClip.Clip);
	}

	public override void Close()
	{
		base.Close();
		if (_preview != null)
		{
			_preview.DragTrigger.onDrag -= _E006;
			_preview.PlayerModelView.LoadingCompletedEvent -= _E000;
		}
		_headSelector.Hide();
		_voiceSelector.Hide();
	}

	[DebuggerHidden]
	[CompilerGenerated]
	private Task _E00A()
	{
		return base.ShowState();
	}

	[CompilerGenerated]
	private IEnumerable<string> _E00B()
	{
		return _headTemplates.Select((KeyValuePair<string, _EBDF> x) => x.Value.NameLocalizationKey.Localized()).ToArray();
	}

	[CompilerGenerated]
	private IEnumerable<string> _E00C()
	{
		return _voiceTemplates.Select((KeyValuePair<string, _EBE9> x) => x.Value.NameLocalizationKey.Localized()).ToArray();
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _E00D()
	{
		return base.HideState();
	}
}
