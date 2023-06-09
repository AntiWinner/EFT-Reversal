using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public sealed class ClickableUIText : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private TextMeshProUGUI _textMessage;

	private ECursorType m__E000;

	private Action<string> _E001;

	private Action<PointerEventData> _E002;

	public void SetText(string message, Action<PointerEventData> onMouseClicked, [CanBeNull] Action<string> onOfferClicked)
	{
		_E001 = onOfferClicked;
		_E002 = onMouseClicked;
		_textMessage.text = message;
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		string text = _E000(eventData);
		if (string.IsNullOrEmpty(text))
		{
			_E002?.Invoke(eventData);
			return;
		}
		_EC45.SetCursor(this.m__E000);
		_E001?.Invoke(text);
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		if (!string.IsNullOrEmpty(_E000(eventData)))
		{
			this.m__E000 = _EC45.PreviousType;
			_EC45.SetCursor(ECursorType.Hand);
		}
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		_EC45.SetCursor(this.m__E000);
	}

	private string _E000(PointerEventData eventData)
	{
		int num = TMP_TextUtilities.FindIntersectingLink(_textMessage, eventData.position, eventData.pressEventCamera);
		if (num == -1)
		{
			return string.Empty;
		}
		TMP_LinkInfo tMP_LinkInfo = _textMessage.textInfo.linkInfo[num];
		return tMP_LinkInfo.GetLinkID();
	}
}
