using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.UI;
using UnityEngine;

namespace EFT.Hideout;

public abstract class ProduceViewBase<TItemsProducer, TProductionScheme> : UIElement, _E825 where TItemsProducer : _E823 where TProductionScheme : _E828
{
	protected const string REQUIREMENTS_NOT_FULFILLED = "hideout/Requirements are not fulfilled";

	protected const string OUT_OF_FUEL = "hideout/Cannot start production without the fuel in Generator";

	protected const string OTHER_ITEMS_READY = "hideout/Take crafted items";

	[SerializeField]
	protected Dictionary<EProductionState, List<GameObject>> _stateActiveObjects;

	[SerializeField]
	protected DefaultUIButton _restartButton;

	[SerializeField]
	protected DefaultUIButton _cancelButton;

	[SerializeField]
	private InteractableElement _electricityNeededIcon;

	[CompilerGenerated]
	private bool _E01A;

	[CompilerGenerated]
	private _EAED _E00E;

	[CompilerGenerated]
	private ItemUiContext _E00F;

	[CompilerGenerated]
	private TItemsProducer _E01B;

	[CompilerGenerated]
	private TProductionScheme _E01C;

	[CompilerGenerated]
	private Action<string> _E01D;

	[CompilerGenerated]
	private Action<string> _E01E;

	protected bool GettingItems
	{
		[CompilerGenerated]
		get
		{
			return _E01A;
		}
		[CompilerGenerated]
		private set
		{
			_E01A = value;
		}
	}

	protected _EAED InventoryController
	{
		[CompilerGenerated]
		get
		{
			return _E00E;
		}
		[CompilerGenerated]
		private set
		{
			_E00E = value;
		}
	}

	protected ItemUiContext ItemUiContext
	{
		[CompilerGenerated]
		get
		{
			return _E00F;
		}
		[CompilerGenerated]
		private set
		{
			_E00F = value;
		}
	}

	protected TItemsProducer Producer
	{
		[CompilerGenerated]
		get
		{
			return _E01B;
		}
		[CompilerGenerated]
		private set
		{
			_E01B = value;
		}
	}

	protected TProductionScheme Scheme
	{
		[CompilerGenerated]
		get
		{
			return _E01C;
		}
		[CompilerGenerated]
		private set
		{
			_E01C = value;
		}
	}

	protected Action<string> OnStartProducing
	{
		[CompilerGenerated]
		get
		{
			return _E01D;
		}
		[CompilerGenerated]
		private set
		{
			_E01D = value;
		}
	}

	protected Action<string> OnGetProducedItems
	{
		[CompilerGenerated]
		get
		{
			return _E01E;
		}
		[CompilerGenerated]
		private set
		{
			_E01E = value;
		}
	}

	protected abstract bool ShowItemsListWindow { get; }

	public virtual void UpdateView()
	{
		EProductionState productionState = Producer.GetProductionState(Scheme._id);
		bool flag = productionState == EProductionState.Error;
		if (_electricityNeededIcon != null)
		{
			_electricityNeededIcon.gameObject.SetActive(Scheme.needFuelForAllProductionTime && !flag);
		}
		ApplyState(productionState, _stateActiveObjects);
	}

	protected void Show(ItemUiContext itemUiContext, _EAED inventoryController, TProductionScheme scheme, TItemsProducer producer, Action<string> onStartProducing, Action<string> getProducedItems)
	{
		ItemUiContext = itemUiContext;
		InventoryController = inventoryController;
		Scheme = scheme;
		Producer = producer;
		OnStartProducing = onStartProducing;
		OnGetProducedItems = getProducedItems;
		ShowGameObject();
		UpdateView();
	}

	protected void GetProducedItems()
	{
		if (!GettingItems)
		{
			GettingItems = true;
			UpdateView();
			_E000().HandleExceptions();
		}
	}

	protected void ApplyState(EProductionState state, Dictionary<EProductionState, List<GameObject>> _stateActiveObjects)
	{
		if (_stateActiveObjects == null)
		{
			return;
		}
		foreach (var (eProductionState2, list2) in _stateActiveObjects)
		{
			if (eProductionState2 == state)
			{
				continue;
			}
			foreach (GameObject item in list2)
			{
				item.SetActive(value: false);
			}
		}
		if (!_stateActiveObjects.TryGetValue(state, out var value))
		{
			return;
		}
		foreach (GameObject item2 in value)
		{
			item2.SetActive(value: true);
		}
	}

	protected async Task RestartProducing()
	{
		if (_restartButton != null)
		{
			_restartButton.Interactable = false;
		}
		if (_cancelButton != null)
		{
			_cancelButton.Interactable = false;
		}
		_E815._E000.Instance.LogInfo(string.Format(_ED3E._E000(170151), Scheme._id, Scheme.areaType));
		IResult result = await Singleton<_E815>.Instance.RestartProduction(Scheme);
		if (!(this == null))
		{
			UpdateView();
			if (result.Succeed)
			{
				OnStartProducing?.Invoke(Scheme._id);
			}
		}
	}

	protected async Task CancelProducing()
	{
		if (_restartButton != null)
		{
			_restartButton.Interactable = false;
		}
		if (_cancelButton != null)
		{
			_cancelButton.Interactable = false;
		}
		_E815._E000.Instance.LogInfo(string.Format(_ED3E._E000(170223), Scheme._id, Scheme.areaType));
		IResult result = await Singleton<_E815>.Instance.CancelProduction(Scheme);
		if (!(this == null))
		{
			if (result.Succeed)
			{
				OnStartProducing(string.Empty);
			}
			UpdateView();
		}
	}

	private async Task _E000()
	{
		if (await Singleton<_E815>.Instance.GetProducedItems(Producer, Scheme._id, ShowItemsListWindow))
		{
			OnGetProducedItems?.Invoke(Scheme._id);
		}
		GettingItems = false;
		if (!(this == null))
		{
			UpdateView();
		}
	}
}
