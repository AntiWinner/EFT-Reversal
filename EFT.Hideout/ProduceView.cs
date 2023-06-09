using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.Hideout;

public sealed class ProduceView : ProduceViewBase<_E823, _E829>
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ProduceView _003C_003E4__this;

		public IEnumerable<Item> allItems;

		internal void _E000()
		{
			_003C_003E4__this.RestartProducing().HandleExceptions();
		}

		internal void _E001()
		{
			_003C_003E4__this.CancelProducing().HandleExceptions();
		}

		internal void _E002()
		{
			_003C_003E4__this.Producer.OnProduceStatusChanged -= _003C_003E4__this.UpdateView;
		}

		internal void _E003(ItemRequirement requirement, HideoutProductionRequirementView view)
		{
			view.Show(_003C_003E4__this.ItemUiContext, _003C_003E4__this.InventoryController, requirement, _003C_003E4__this.Scheme, allItems, !_003C_003E4__this.Producer.CanStartByProductionCount);
			_003C_003E4__this._E008 &= view.IsFulfilled;
		}
	}

	private const int m__E000 = 1;

	private static readonly string m__E001 = _ED3E._E000(171762) + EDetailsType.Producing.ToStringNoBox();

	[SerializeField]
	private CanvasGroup _viewCanvas;

	[SerializeField]
	private HideoutProductionRequirementView _requiredItemTemplate;

	[SerializeField]
	private Transform _requiredItemsContainer;

	[SerializeField]
	private GameObject _expectedTimePanel;

	[SerializeField]
	private TextMeshProUGUI _expectedTime;

	[SerializeField]
	private GameObject _productionPercentagesPanel;

	[SerializeField]
	private GameObject _productionBackground;

	[SerializeField]
	private TextMeshProUGUI _productionPercentages;

	[SerializeField]
	private HideoutItemViewFactory _resultItemIconViewFactory;

	[SerializeField]
	private DefaultUIButton _startButton;

	[SerializeField]
	private DefaultUIButton _getItemsButton;

	[SerializeField]
	private ComplementaryButton _favoriteButton;

	[SerializeField]
	private TextMeshProUGUI _productionStatus;

	[SerializeField]
	private GameObject _loaderPanel;

	private _EC79<ItemRequirement, HideoutProductionRequirementView> m__E002;

	private SimpleTooltip m__E003;

	private bool m__E004;

	private bool m__E005;

	private DateTime m__E006;

	[CompilerGenerated]
	private Action m__E007;

	protected override bool ShowItemsListWindow => false;

	private bool _E008
	{
		get
		{
			return this.m__E005;
		}
		set
		{
			if (this.m__E005 != value)
			{
				this.m__E005 = value;
				_E003();
			}
		}
	}

	public bool AnyItemsReady
	{
		set
		{
			if (this.m__E004 != value)
			{
				this.m__E004 = value;
				_E003();
			}
		}
	}

	public event Action OnUpdateFavoriteSchemesList
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E007;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E007, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E007;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E007, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		this.m__E003 = ItemUiContext.Instance.Tooltip;
		_expectedTimePanel.GetOrAddComponent<HoverTrigger>().Init(delegate
		{
			if (!base.Producer.TimeReduction.ZeroOrNegative())
			{
				this.m__E003.Show(string.Format(_ED3E._E000(170122).Localized(), base.Producer.TimeReduction));
			}
		}, delegate
		{
			this.m__E003.Close();
		});
	}

	private void _E000(bool isClick = false)
	{
		base.Scheme.FavoriteScheme = !base.Scheme.FavoriteScheme;
		_favoriteButton.SetSelectedStatus(base.Scheme.FavoriteScheme);
		this.m__E007?.Invoke();
	}

	public void Show(ItemUiContext itemUiContext, _EAED inventoryController, _E829 scheme, _E823 producer, Action<string> onStartProducing, Action<string> getProducedItems, bool availableFavorites)
	{
		Show(itemUiContext, inventoryController, scheme, producer, onStartProducing, getProducedItems);
		_startButton.OnClick.AddListener(_E002);
		_restartButton.OnClick.AddListener(delegate
		{
			RestartProducing().HandleExceptions();
		});
		_cancelButton.OnClick.AddListener(delegate
		{
			CancelProducing().HandleExceptions();
		});
		_getItemsButton.OnClick.AddListener(base.GetProducedItems);
		base.Producer.OnProduceStatusChanged += UpdateView;
		UI.AddDisposable(delegate
		{
			base.Producer.OnProduceStatusChanged -= UpdateView;
		});
		Item hideoutSchemeItem = Singleton<_E63B>.Instance.GetHideoutSchemeItem(base.Scheme.endProduct, base.Scheme._id, EOwnerType.HideoutProduction);
		if (hideoutSchemeItem is _EA85)
		{
			hideoutSchemeItem.GetItemComponentsInChildren<RecodableComponent>().First().SetEncoded(base.Scheme.isEncoded);
		}
		_favoriteButton.gameObject.SetActive(availableFavorites);
		if (availableFavorites)
		{
			_favoriteButton.Show(_E000);
			_favoriteButton.SetSelectedStatus(scheme.FavoriteScheme);
			UI.AddDisposable(_favoriteButton);
		}
		_resultItemIconViewFactory.Show(hideoutSchemeItem, base.InventoryController, base.ItemUiContext);
		UI.AddDisposable(_resultItemIconViewFactory);
		_E001();
		if (base.Scheme.HasRequirements)
		{
			IEnumerable<Item> allItems = Singleton<_E815>.Instance.AllStashItems;
			_E008 = true;
			this.m__E002 = UI.AddViewList(base.Scheme.RequiredItems, _requiredItemTemplate, _requiredItemsContainer, delegate(ItemRequirement requirement, HideoutProductionRequirementView view)
			{
				view.Show(base.ItemUiContext, base.InventoryController, requirement, base.Scheme, allItems, !base.Producer.CanStartByProductionCount);
				_E008 &= view.IsFulfilled;
			});
			_E003();
		}
	}

	public override void UpdateView()
	{
		if (this == null)
		{
			return;
		}
		base.UpdateView();
		_E001();
		if (this.m__E002 == null)
		{
			return;
		}
		IEnumerable<Item> allStashItems = Singleton<_E815>.Instance.AllStashItems;
		bool flag = true;
		foreach (var (requirement, hideoutProductionRequirementView2) in this.m__E002)
		{
			hideoutProductionRequirementView2.Show(base.ItemUiContext, base.InventoryController, requirement, base.Scheme, allStashItems, !base.Producer.CanStartByProductionCount);
			flag &= hideoutProductionRequirementView2.IsFulfilled;
		}
		_E008 = flag;
		_E003();
	}

	private void _E001()
	{
		AnyItemsReady = base.Producer.CompleteItemsStorage.AnyItemsReady;
		bool flag = base.Producer.CompleteItemsStorage.GetItemsCount(base.Scheme._id) > 0;
		_E827 value;
		bool flag2 = base.Producer.ProducingItems.TryGetValue(base.Scheme._id, out value);
		bool flag3 = base.Producer.GetProductionState(base.Scheme._id) == EProductionState.Error;
		_productionBackground.SetActive(flag2);
		if (flag || flag2 || (!base.Producer.CanStartByProductionCount && base.Producer.IsWorking))
		{
			_expectedTimePanel.SetActive(value: false);
			_productionPercentagesPanel.SetActive(value: true);
			if (flag2)
			{
				_E004(value);
			}
			else
			{
				_productionPercentages.text = string.Empty;
			}
		}
		else
		{
			string text = TimeSpan.FromSeconds(base.Producer.CalculateProductionTime(base.Scheme)).TimeLeftShortFormat('\n');
			_expectedTime.SetMonospaceText(text);
			_productionPercentagesPanel.SetActive(value: false);
		}
		bool value2 = base.Producer.CanStartByProductionCount || flag2 || flag;
		_viewCanvas.SetUnlockStatus(value2, setRaycast: false);
		_getItemsButton.gameObject.SetActive(flag && !base.GettingItems);
		_productionStatus.gameObject.SetActive(!flag && flag2);
		_startButton.gameObject.SetActive(!flag && !flag2 && !flag3);
		_loaderPanel.SetActive(base.GettingItems);
		_resultItemIconViewFactory.ShowInfo(base.Scheme.count > 1, showFulfilledStatus: false);
		_resultItemIconViewFactory.SetCounterText(base.Scheme.count.ToString());
	}

	private void _E002()
	{
		if (_E008 && base.Producer.CanStartByProductionCount && base.Producer.CanStartScheme(base.Scheme))
		{
			_E815._E000.Instance.LogInfo(_ED3E._E000(171922), base.Scheme._id, base.Scheme.areaType, base.Producer.CanStart);
			_E815._E000.Instance.LogInfo(_ED3E._E000(171977), base.Producer.CompleteItemsStorage.AnyItemsReady, base.Producer.ProducingItems.Any());
			Singleton<_E815>.Instance.StartSingleProduction(base.Scheme, delegate
			{
				base.OnStartProducing?.Invoke(base.Scheme._id);
			}).HandleExceptions();
		}
	}

	private void _E003()
	{
		bool flag = this.m__E005 && base.Producer.CanStart && base.Producer.CanStartByProductionCount && base.Producer.CanStartScheme(base.Scheme);
		string tooltip = string.Empty;
		if (!flag)
		{
			if (this.m__E004)
			{
				tooltip = _ED3E._E000(170017);
			}
			else if (!this.m__E005)
			{
				tooltip = _ED3E._E000(171571);
			}
			else if (!base.Producer.CanStart || !base.Producer.CanStartScheme(base.Scheme))
			{
				tooltip = _ED3E._E000(170052);
			}
		}
		_startButton.SetDisabledTooltip(tooltip);
		_startButton.Interactable = flag && base.Producer.GetErrorsCountRaw() == 0;
		_restartButton.Interactable = flag;
		_restartButton.SetDisabledTooltip(tooltip);
		_cancelButton.Interactable = true;
	}

	private void Update()
	{
		DateTime utcNow = _E5AD.UtcNow;
		if (!((utcNow - this.m__E006).TotalSeconds < 1.0))
		{
			this.m__E006 = utcNow;
			if (base.Producer.ProducingItems.TryGetValue(base.Scheme._id, out var value))
			{
				_E004(value);
			}
		}
	}

	private void _E004(_E827 producingItem)
	{
		_productionStatus.SetMonospaceText(ProduceView.m__E001.Localized() + _ED3E._E000(54246) + producingItem.EstimatedTimeInt.ToTimeString() + _ED3E._E000(27308));
		_productionPercentages.SetMonospaceText(string.Format(_ED3E._E000(171594), Mathf.FloorToInt((float)producingItem.Progress * 100f)));
	}

	public override void Close()
	{
		this.m__E007 = null;
		this.m__E003.Close();
		_startButton.OnClick.RemoveAllListeners();
		_restartButton.OnClick.RemoveAllListeners();
		_cancelButton.OnClick.RemoveAllListeners();
		_getItemsButton.OnClick.RemoveAllListeners();
		this.m__E002 = null;
		base.Close();
	}

	[CompilerGenerated]
	private void _E005(PointerEventData arg)
	{
		if (!base.Producer.TimeReduction.ZeroOrNegative())
		{
			this.m__E003.Show(string.Format(_ED3E._E000(170122).Localized(), base.Producer.TimeReduction));
		}
	}

	[CompilerGenerated]
	private void _E006(PointerEventData arg)
	{
		this.m__E003.Close();
	}

	[CompilerGenerated]
	private void _E007()
	{
		base.OnStartProducing?.Invoke(base.Scheme._id);
	}
}
