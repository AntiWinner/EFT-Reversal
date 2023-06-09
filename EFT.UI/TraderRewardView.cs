using Comfort.Common;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class TraderRewardView : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[SerializeField]
	private TextMeshProUGUI _traderName;

	[SerializeField]
	private Image _avatar;

	private SimpleTooltip _E02A;

	public void Show(string traderId)
	{
		_E02A = ItemUiContext.Instance.Tooltip;
		ShowGameObject();
		_E5CB.TraderSettings traderSettings = Singleton<_E5CB>.Instance.TradersSettings[traderId];
		traderSettings.GetAndAssignAvatar(_avatar, base.CancellationToken).HandleExceptions();
		_traderName.text = traderSettings.Nickname.Localized();
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		if (_E02A != null)
		{
			_E02A.Show(_ED3E._E000(262067).Localized());
		}
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		if (_E02A != null && _E02A.gameObject.activeSelf)
		{
			_E02A.Close();
		}
	}
}
