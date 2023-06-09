using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using EFT.UI.SessionEnd;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class StatisticsSpawn : UIElement
{
	public enum EStatisticsType
	{
		Overall,
		Session
	}

	public enum EStatGroupLayoutType
	{
		Default,
		SameColumn
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public StatisticsSpawn _003C_003E4__this;

		public StatGroupView prevGroupView;

		public List<(StatGroupView, StatGroupView)> updateParents;

		public int currentViewCount;

		public GameObject currentGroup;

		public _EC6D<SessionResultStatistics._E001, StatGroupView> viewList;

		public CancellationToken cancellationToken;

		internal void _E000(SessionResultStatistics._E001 group, StatGroupView groupView)
		{
			groupView.Show(group);
			if (_003C_003E4__this._horizontalGroup == null)
			{
				return;
			}
			if (group.LayoutType == EStatGroupLayoutType.SameColumn && prevGroupView != null)
			{
				groupView.gameObject.SetActive(value: false);
				updateParents.Add((groupView, prevGroupView));
				return;
			}
			if (currentViewCount % 2 == 0)
			{
				currentGroup = _003C_003E4__this._E001();
			}
			currentViewCount++;
			groupView.transform.SetParent(currentGroup.transform);
			prevGroupView = groupView;
		}

		internal async Task _E001()
		{
			await viewList.InitTask;
			if (cancellationToken.IsCancellationRequested || updateParents.Count == 0)
			{
				return;
			}
			foreach (var (statGroupView, statGroupView2) in updateParents)
			{
				statGroupView.transform.SetParent(statGroupView2.transform);
				statGroupView.gameObject.SetActive(value: true);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public HorizontalLayoutGroup group;

		internal void _E000()
		{
			Object.Destroy(group.gameObject);
		}
	}

	[SerializeField]
	private StatGroupView _statGroupViewTemplate;

	[SerializeField]
	private Transform _statsContainer;

	[SerializeField]
	private HorizontalLayoutGroup _horizontalGroup;

	private EStatisticsType _E180;

	internal void _E000(Profile profile, EStatisticsType type)
	{
		_E000 CS_0024_003C_003E8__locals0 = new _E000();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		CS_0024_003C_003E8__locals0.cancellationToken = base.CancellationToken;
		_E180 = type;
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
		List<SessionResultStatistics._E001> list = new List<SessionResultStatistics._E001>();
		_EC5E obj = new _EC5E(_E180, profile, list);
		obj.AddOverallStats();
		obj.AddStandingStats();
		obj.AddHealthStats();
		obj.AddLootingStats();
		obj.AddDailyStats();
		obj.AddBattleStats();
		CS_0024_003C_003E8__locals0.currentViewCount = 0;
		CS_0024_003C_003E8__locals0.currentGroup = _E001();
		CS_0024_003C_003E8__locals0.prevGroupView = null;
		CS_0024_003C_003E8__locals0.updateParents = new List<(StatGroupView, StatGroupView)>();
		CS_0024_003C_003E8__locals0.viewList = UI.AddViewListAsync(list, _statGroupViewTemplate, _statsContainer, delegate(SessionResultStatistics._E001 group, StatGroupView groupView)
		{
			groupView.Show(group);
			if (!(CS_0024_003C_003E8__locals0._003C_003E4__this._horizontalGroup == null))
			{
				if (group.LayoutType == EStatGroupLayoutType.SameColumn && CS_0024_003C_003E8__locals0.prevGroupView != null)
				{
					groupView.gameObject.SetActive(value: false);
					CS_0024_003C_003E8__locals0.updateParents.Add((groupView, CS_0024_003C_003E8__locals0.prevGroupView));
				}
				else
				{
					if (CS_0024_003C_003E8__locals0.currentViewCount % 2 == 0)
					{
						CS_0024_003C_003E8__locals0.currentGroup = CS_0024_003C_003E8__locals0._003C_003E4__this._E001();
					}
					CS_0024_003C_003E8__locals0.currentViewCount++;
					groupView.transform.SetParent(CS_0024_003C_003E8__locals0.currentGroup.transform);
					CS_0024_003C_003E8__locals0.prevGroupView = groupView;
				}
			}
		});
		CS_0024_003C_003E8__locals0._E001().HandleExceptions();
	}

	private GameObject _E001()
	{
		if (_horizontalGroup == null)
		{
			return _statsContainer.gameObject;
		}
		HorizontalLayoutGroup group = Object.Instantiate(_horizontalGroup, _statsContainer);
		group.gameObject.SetActive(value: true);
		UI.AddDisposable(delegate
		{
			Object.Destroy(group.gameObject);
		});
		return group.gameObject;
	}
}
