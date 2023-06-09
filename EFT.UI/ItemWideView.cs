using Comfort.Common;
using EFT.InventoryLogic;
using EFT.Quests;
using EFT.UI.DragAndDrop;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class ItemWideView : ItemIconView, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[SerializeField]
	private CustomTextMeshProUGUI _name;

	[SerializeField]
	private CustomTextMeshProUGUI _count;

	[SerializeField]
	private Image _unlockRewardIcon;

	[SerializeField]
	private GameObject _foundInRaid;

	[SerializeField]
	private Sprite _schemeUnlockSprite;

	private SimpleTooltip _E02A;

	private ERewardType _E1B3;

	private string _E1B4;

	private bool _E1B5;

	public void Show(_E936 reward)
	{
		ShowGameObject();
		Item item = reward.GetItem();
		if (item is _EA76)
		{
			item.StackObjectsCount = (int)Singleton<BonusController>.Instance.Calculate(EBonusType.QuestMoneyReward, item.StackObjectsCount);
		}
		_E02A = ItemUiContext.Instance.Tooltip;
		_E1B3 = reward.type;
		switch (_E1B3)
		{
		case ERewardType.AssortmentUnlock:
		{
			_E5CB.TraderSettings traderSettings = Singleton<_E5CB>.Instance.TradersSettings[reward.traderId];
			_E1B4 = string.Format(string.Concat(_E1B3, _ED3E._E000(261578)).Localized(), traderSettings.Nickname.Localized() + _ED3E._E000(54246) + reward.loyaltyLevel + _ED3E._E000(261621).Localized() + _ED3E._E000(27308));
			break;
		}
		case ERewardType.ProductionScheme:
		{
			EAreaType type = (EAreaType)int.Parse(reward.traderId);
			_E1B4 = string.Format(string.Concat(_E1B3, _ED3E._E000(261578)).Localized(), type.LocalizeAreaName() + _ED3E._E000(54246) + reward.loyaltyLevel + _ED3E._E000(18502) + _ED3E._E000(223604).Localized() + _ED3E._E000(27308));
			_unlockRewardIcon.sprite = _schemeUnlockSprite;
			break;
		}
		default:
			_E1B4 = string.Concat(_E1B3, _ED3E._E000(261578)).Localized();
			break;
		}
		if (_E1B3 != ERewardType.Item && _E1B3 != ERewardType.AssortmentUnlock && _E1B3 != ERewardType.ProductionScheme)
		{
			Debug.LogError(_ED3E._E000(261616));
			return;
		}
		if (item == null)
		{
			Debug.LogError(_ED3E._E000(261603) + _E1B3);
			return;
		}
		_unlockRewardIcon.gameObject.SetActive(_E1B3 == ERewardType.AssortmentUnlock || _E1B3 == ERewardType.ProductionScheme);
		string text = Singleton<_E63B>.Instance.BriefItemName(item, item.Name.Localized());
		_name.text = text;
		_count.text = ((item.StackObjectsCount > 1) ? item.StackObjectsCount.ToString() : "");
		if (item.StackObjectsCount > 1)
		{
			CustomTextMeshProUGUI customTextMeshProUGUI = _name;
			customTextMeshProUGUI.text = customTextMeshProUGUI.text + _ED3E._E000(54246) + item.StackObjectsCount + _ED3E._E000(27308);
		}
		ItemIcon = ItemViewFactory.LoadItemIcon(item);
		if (ItemIcon == null)
		{
			Debug.LogError(_ED3E._E000(261640) + item.Name.Localized() + _ED3E._E000(261694));
			return;
		}
		_E000(reward.findInRaid);
		if (ItemIcon.Sprite != null)
		{
			if (base.IconLoader != null)
			{
				base.IconLoader.SetActive(ItemIcon.Sprite == null);
			}
			base.MainImage.sprite = ItemIcon.Sprite;
		}
		else
		{
			Show(item, delegate
			{
			});
		}
	}

	private void _E000(bool state)
	{
		if (!(_foundInRaid == null))
		{
			_foundInRaid.SetActive(state);
		}
	}

	protected override void UpdateScale()
	{
		if (base.MainImage.gameObject.activeSelf)
		{
			RectTransform rectTransform = base.MainImage.rectTransform;
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.offsetMin = new Vector2(0f, 0f);
			rectTransform.offsetMax = new Vector2(0f, 0f);
			base.MainImage.transform.localScale = Vector3.one;
		}
	}

	private void _E001()
	{
		if (!(_E02A == null) && _E02A.gameObject.activeSelf && _E1B5)
		{
			_E02A.Close();
			_E1B5 = false;
		}
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		if (!(_E02A == null))
		{
			_E02A.Show(_E1B4);
			_E1B5 = true;
		}
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		_E001();
	}

	public void OnDisable()
	{
		_E001();
	}
}
