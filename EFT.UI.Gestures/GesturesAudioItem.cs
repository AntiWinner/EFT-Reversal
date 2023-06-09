using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI.Gestures;

public sealed class GesturesAudioItem : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public bool GroupIsAlwaysOpen;

	[SerializeField]
	private LocalizedText _nameText;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private GameObject _selectionGlow;

	[SerializeField]
	private GameObject _subItemsContainer;

	[SerializeField]
	private GesturesAudioSubItem _subItemTemplate;

	[SerializeField]
	private List<GesturesAudioSubItem> _audioSubItems;

	private bool _E17E;

	public IReadOnlyList<GesturesAudioSubItem> AudioSubItems => _audioSubItems;

	public bool HasOnlySituational => _audioSubItems.All((GesturesAudioSubItem x) => x.IsSituational);

	public string LocalizationKey
	{
		set
		{
			if (_nameText != null)
			{
				_nameText.LocalizationKey = value;
			}
		}
	}

	public bool IsOpen
	{
		get
		{
			return _E17E;
		}
		set
		{
			value |= GroupIsAlwaysOpen;
			_E17E = value;
			if (_selectionGlow != null)
			{
				_selectionGlow.SetActive(_E17E);
			}
			if (_subItemsContainer != null)
			{
				_subItemsContainer.SetActive(_E17E);
			}
		}
	}

	private void Awake()
	{
		if (_subItemTemplate != null)
		{
			_subItemTemplate.gameObject.SetActive(value: false);
		}
	}

	public GestureBaseItem CreateNewPhrase(EPhraseTrigger phrase, bool isSituational)
	{
		GesturesAudioSubItem gesturesAudioSubItem = Object.Instantiate(_subItemTemplate, _subItemsContainer.transform);
		gesturesAudioSubItem.gameObject.name = phrase.ToString();
		gesturesAudioSubItem.PhraseTrigger = phrase;
		gesturesAudioSubItem.IsSituational = isSituational;
		_audioSubItems.Add(gesturesAudioSubItem);
		return gesturesAudioSubItem;
	}

	public void Show(Sprite defaultBackground, Sprite selectedBackground, Color defaultAudioColor, Color selectedAudioColor, _ECAB binds, HashSet<EPhraseTrigger> availablePhrases)
	{
		ShowGameObject();
		IsOpen = false;
		bool flag = false;
		foreach (GesturesAudioSubItem audioSubItem in _audioSubItems)
		{
			EPhraseTrigger phraseTrigger = audioSubItem.PhraseTrigger;
			if (phraseTrigger != EPhraseTrigger.PhraseNone && (availablePhrases.Contains(phraseTrigger) || phraseTrigger == EPhraseTrigger.MumblePhrase))
			{
				audioSubItem.Show(defaultBackground, selectedBackground, defaultAudioColor, selectedAudioColor, binds);
				flag = true;
			}
			else
			{
				audioSubItem.Close();
			}
		}
		_canvasGroup.SetUnlockStatus(GroupIsAlwaysOpen || flag);
	}

	void IPointerEnterHandler.OnPointerEnter([NotNull] PointerEventData eventData)
	{
		IsOpen = true;
	}

	void IPointerExitHandler.OnPointerExit([NotNull] PointerEventData eventData)
	{
		IsOpen = false;
	}

	public override void Close()
	{
		if (_audioSubItems != null)
		{
			for (int num = _audioSubItems.Count - 1; num >= 0; num--)
			{
				_audioSubItems[num].Close();
			}
		}
		IsOpen = false;
		if (_selectionGlow != null)
		{
			_selectionGlow.SetActive(value: false);
		}
		if (_subItemsContainer != null)
		{
			_subItemsContainer.SetActive(value: false);
		}
		base.Close();
	}
}
