using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public class ButtonWithHint : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[SerializeField]
	private Button _button;

	[SerializeField]
	private TextMeshProUGUI _label;

	private Action m__E000;

	private void Awake()
	{
		_button.onClick.AddListener(delegate
		{
			this.m__E000?.Invoke();
		});
	}

	public void Show(Action onClick)
	{
		this.m__E000 = onClick;
		_label.text = _label.text.Localized();
		_label.gameObject.SetActive(value: false);
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		_label.gameObject.SetActive(value: true);
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		_label.gameObject.SetActive(value: false);
	}

	[CompilerGenerated]
	private void _E000()
	{
		this.m__E000?.Invoke();
	}
}
