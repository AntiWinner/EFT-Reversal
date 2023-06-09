using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public class TradeItemType : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[SerializeField]
	private Image _icon;

	private readonly Color _E1F9 = new Color(0.51f, 0.04f, 0.031f);

	private SimpleTooltip _E02A;

	private EItemType _E1FA;

	public void Show(EItemType itemType)
	{
		_E02A = ItemUiContext.Instance.Tooltip;
		ShowGameObject();
		_icon.gameObject.SetActive(itemType != EItemType.None);
		if (itemType != 0)
		{
			_E1FA = itemType;
			_icon.sprite = EFTHardSettings.Instance.StaticIcons.GetItemTypeIcon(itemType);
			_icon.SetNativeSize();
			_icon.color = (_E38D.True ? Color.white : _E1F9);
		}
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		_E02A.Show(_E1FA.ToString().Localized());
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		_E02A.Close();
	}

	public override void Close()
	{
		base.Close();
		if (_E02A.isActiveAndEnabled)
		{
			_E02A.Close();
		}
	}
}
