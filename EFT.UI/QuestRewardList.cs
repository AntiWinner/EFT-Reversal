using System;
using System.Collections.Generic;
using EFT.Quests;
using UnityEngine;

namespace EFT.UI;

public sealed class QuestRewardList : UIElement
{
	[SerializeField]
	private CustomTextMeshProUGUI _title;

	[SerializeField]
	private CustomTextMeshProUGUI _info;

	[SerializeField]
	private ItemWideView _itemPrefab;

	[SerializeField]
	private StatView _statPrefab;

	[SerializeField]
	private TraderRewardView _traderRewardView;

	[SerializeField]
	private GameObject _unknownPrefab;

	[SerializeField]
	private RectTransform _container;

	public void Init(string title, IEnumerable<_E936> rewards, bool showUnknown, string info = null)
	{
		_title.text = title;
		_info.gameObject.SetActive(info != null);
		_info.text = info;
		foreach (_E936 reward in rewards)
		{
			if (reward.unknown && !showUnknown)
			{
				_container.InstantiatePrefab(_unknownPrefab);
				continue;
			}
			switch (reward.type)
			{
			case ERewardType.Experience:
			case ERewardType.Skill:
			case ERewardType.TraderStanding:
			case ERewardType.TraderStandingRestore:
				UnityEngine.Object.Instantiate(_statPrefab, _container).Show(reward);
				break;
			case ERewardType.Item:
			case ERewardType.AssortmentUnlock:
			case ERewardType.ProductionScheme:
				UnityEngine.Object.Instantiate(_itemPrefab, _container).Show(reward);
				break;
			case ERewardType.TraderUnlock:
				UnityEngine.Object.Instantiate(_traderRewardView, _container).Show(reward.target);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case ERewardType.Location:
			case ERewardType.Counter:
			case ERewardType.TraderStandingReset:
				break;
			}
		}
	}
}
