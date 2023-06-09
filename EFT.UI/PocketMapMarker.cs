using System;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public class PocketMapMarker : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private Image _image;

	[SerializeField]
	private CustomTextMeshProUGUI _note;

	[SerializeField]
	private RectTransform _noteBack;

	[SerializeField]
	private Image _shadow;

	[SerializeField]
	private SpriteMap _spriteMap;

	[NonSerialized]
	public MapMarker Marker;

	private PocketMapMarkerManager _E000;

	private void Start()
	{
		_shadow.gameObject.SetActive(value: false);
		ShowNote(value: false);
	}

	public void Init(MapMarker marker, PocketMapMarkerManager manager)
	{
		Marker = marker;
		_E000 = manager;
		_image.sprite = _spriteMap[Marker.Type];
		_image.SetNativeSize();
		_note.text = Marker.Note;
	}

	public void ShowNote(bool value)
	{
		_noteBack.gameObject.SetActive(value);
		if (value)
		{
			base.transform.SetAsLastSibling();
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		PointerEventData.InputButton button = eventData.button;
		if (button != 0 && button == PointerEventData.InputButton.Right)
		{
			ShowNote(value: false);
			_E000.ShowEditContextMenu(eventData.position, Marker);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_shadow.gameObject.SetActive(value: true);
		ShowNote(value: true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_shadow.gameObject.SetActive(value: false);
		ShowNote(value: false);
	}
}
