using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI;
using TMPro;
using UnityEngine;

namespace EFT.Hideout;

public sealed class ProductionPanel : AbstractPanel<bool>
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ProductionPanel _003C_003E4__this;

		public ItemUiContext itemUiContext;

		public bool availableSearch;

		internal void _E000()
		{
			_003C_003E4__this._farmingViewTemplate.Close();
		}

		internal ScavCaseView _E001(_E82A arg)
		{
			return _003C_003E4__this._scavCaseTemplate;
		}

		internal Transform _E002(_E82A arg)
		{
			return _003C_003E4__this._container;
		}

		internal void _E003()
		{
			_003C_003E4__this._permanentViewTemplate.Close();
		}

		internal ProduceView _E004(_E829 arg)
		{
			return _003C_003E4__this._produceViewTemplate;
		}

		internal Transform _E005(_E829 arg)
		{
			return _003C_003E4__this._container;
		}

		internal void _E006(_E829 scheme, ProduceView view)
		{
			view.Show(itemUiContext, _003C_003E4__this.Player._E0DE, scheme, _003C_003E4__this.m__E000, _003C_003E4__this._E007, _003C_003E4__this._E003, availableSearch);
			view.OnUpdateFavoriteSchemesList += _003C_003E4__this._E008;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E82D scavProducer;

		public _E000 CS_0024_003C_003E8__locals1;

		internal void _E000(_E82A scheme, ScavCaseView view)
		{
			view.Show(CS_0024_003C_003E8__locals1.itemUiContext, CS_0024_003C_003E8__locals1._003C_003E4__this.Player._E0DE, scheme, scavProducer, CS_0024_003C_003E8__locals1._003C_003E4__this._E007, CS_0024_003C_003E8__locals1._003C_003E4__this._E003);
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public ProductionPanel _003C_003E4__this;

		public Item item;

		public Func<ItemRequirement, bool> _003C_003E9__2;

		internal async Task _E000()
		{
			await TasksExtensions.WaitUntil(() => _003C_003E4__this.m__E002 != null || _003C_003E4__this == null);
			if (_003C_003E4__this == null || _003C_003E4__this.m__E002 == null)
			{
				return;
			}
			foreach (var (obj3, obj4) in _003C_003E4__this.m__E002)
			{
				if (!obj3.RequiredItems.All((ItemRequirement x) => x.TemplateId != item.TemplateId))
				{
					obj4.UpdateView();
				}
			}
		}

		internal bool _E001()
		{
			if (_003C_003E4__this.m__E002 == null)
			{
				return _003C_003E4__this == null;
			}
			return true;
		}

		internal bool _E002(ItemRequirement x)
		{
			return x.TemplateId != item.TemplateId;
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public string schemeId;

		internal bool _E000(_E828 x)
		{
			return x._id == schemeId;
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public string value;

		internal bool _E000(_E829 scheme)
		{
			return scheme.endProduct.LocalizedName().IndexOf(value, StringComparison.InvariantCultureIgnoreCase) >= 0;
		}
	}

	[SerializeField]
	private ProduceView _produceViewTemplate;

	[SerializeField]
	private FarmingView _farmingViewTemplate;

	[SerializeField]
	private PermanentProductionView _permanentViewTemplate;

	[SerializeField]
	private ScavCaseView _scavCaseTemplate;

	[SerializeField]
	private RectTransform _container;

	[SerializeField]
	private TextMeshProUGUI _header;

	[SerializeField]
	private ValidationInputField _searchInputField;

	private _E823 m__E000;

	private _E828[] m__E001;

	private Dictionary<_E828, _E825> m__E002;

	private _EC6D<_E829, ProduceView> m__E003;

	public override async Task ShowContents()
	{
		UI.SubscribeEvent(_searchInputField.onValueChanged, _E009);
		_E826 productionController = Singleton<_E815>.Instance.ProductionController;
		this.m__E000 = productionController.GetProducer(base.AreaData);
		if (this.m__E000 == null)
		{
			throw new NullReferenceException(_ED3E._E000(170294) + base.AreaData.GetType().Name);
		}
		this.m__E001 = this.m__E000?.ProductionSchemes;
		if (this.m__E001 == null)
		{
			return;
		}
		ItemUiContext itemUiContext = ItemUiContext.Instance;
		bool availableSearch = this.m__E001.Length > 1;
		switch (base.AreaData.Template.Type)
		{
		case EAreaType.BitcoinFarm:
			if (!(this.m__E001.FirstOrDefault() is _E829 obj2))
			{
				throw new NullReferenceException(_ED3E._E000(170323));
			}
			_header.gameObject.SetActive(value: false);
			this.m__E002 = new Dictionary<_E828, _E825> { { obj2, _farmingViewTemplate } };
			_farmingViewTemplate.Show(itemUiContext, base.Player._E0DE, obj2, this.m__E000 as _E81E, _E003);
			UI.AddDisposable(delegate
			{
				_farmingViewTemplate.Close();
			});
			break;
		case EAreaType.ScavCase:
		{
			_E82A[] array = this.m__E001.OfType<_E82A>().ToArray();
			if (!array.Any())
			{
				throw new NullReferenceException(_ED3E._E000(170362));
			}
			_E82D scavProducer;
			if ((scavProducer = this.m__E000 as _E82D) == null)
			{
				throw new ArgumentException(_ED3E._E000(170372) + this.m__E000.GetType());
			}
			_header.gameObject.SetActive(value: false);
			_EC6D<_E82A, ScavCaseView> obj3 = new _EC6D<_E82A, ScavCaseView>();
			UI.AddDisposable(obj3);
			await obj3.InitAsync(array, (_E82A arg) => _scavCaseTemplate, (_E82A arg) => _container, delegate(_E82A scheme, ScavCaseView view)
			{
				view.Show(itemUiContext, base.Player._E0DE, scheme, scavProducer, _E007, _E003);
			});
			if (this == null)
			{
				return;
			}
			this.m__E002 = ((IEnumerable<KeyValuePair<_E82A, ScavCaseView>>)obj3).ToDictionary((Func<KeyValuePair<_E82A, ScavCaseView>, _E828>)((KeyValuePair<_E82A, ScavCaseView> view) => view.Key), (Func<KeyValuePair<_E82A, ScavCaseView>, _E825>)((KeyValuePair<_E82A, ScavCaseView> view) => view.Value));
			break;
		}
		default:
		{
			_searchInputField.gameObject.SetActive(availableSearch);
			if (this.m__E000 is _E824 producer)
			{
				if (!(this.m__E001.FirstOrDefault() is _E829 obj))
				{
					throw new NullReferenceException(_ED3E._E000(170452));
				}
				_header.gameObject.SetActive(value: true);
				this.m__E002 = new Dictionary<_E828, _E825> { { obj, _permanentViewTemplate } };
				_permanentViewTemplate.Show(itemUiContext, base.Player._E0DE, obj, producer, _E003);
				UI.AddDisposable(delegate
				{
					_permanentViewTemplate.Close();
				});
				break;
			}
			IEnumerable<_E829> enumerable = _E004();
			if (enumerable == null)
			{
				throw new NullReferenceException(_ED3E._E000(170494));
			}
			_header.gameObject.SetActive(value: true);
			this.m__E003 = new _EC6D<_E829, ProduceView>();
			UI.AddDisposable(this.m__E003);
			Task task = this.m__E003.InitAsync(enumerable, (_E829 arg) => _produceViewTemplate, (_E829 arg) => _container, delegate(_E829 scheme, ProduceView view)
			{
				view.Show(itemUiContext, base.Player._E0DE, scheme, this.m__E000, _E007, _E003, availableSearch);
				view.OnUpdateFavoriteSchemesList += _E008;
			});
			task.HandleExceptions();
			await task;
			if (this == null)
			{
				return;
			}
			this.m__E002 = ((IEnumerable<KeyValuePair<_E829, ProduceView>>)this.m__E003).ToDictionary((Func<KeyValuePair<_E829, ProduceView>, _E828>)((KeyValuePair<_E829, ProduceView> view) => view.Key), (Func<KeyValuePair<_E829, ProduceView>, _E825>)((KeyValuePair<_E829, ProduceView> view) => view.Value));
			break;
		}
		}
		Singleton<_E815>.Instance.EnergyController.OnEnergyGenerationChanged += _E000;
		this.m__E000.OnProductionComplete += _E005;
		this.m__E000.OnDataChanged += _E001;
		this.m__E000.OnItemChanged += _E002;
		this.m__E000.OnProduceStatusChanged += _E006;
	}

	private void _E000(bool obj)
	{
		_E006();
	}

	private void _E001()
	{
		_E006();
	}

	private void _E002(Item item)
	{
		_E003 obj = new _E003();
		obj._003C_003E4__this = this;
		obj.item = item;
		obj._E000().HandleExceptions();
	}

	private void _E003(string schemeId)
	{
		this.m__E000.GetItems(schemeId);
		_E006();
	}

	private IEnumerable<_E829> _E004()
	{
		return from scheme in this.m__E001.OfType<_E829>()
			where !scheme.locked
			orderby scheme.FavoriteIndex, scheme.Level
			select scheme;
	}

	private void _E005(Item[] items, _E827 _)
	{
		_E006();
	}

	private void _E006()
	{
		_E00A().HandleExceptions();
	}

	private void _E007(string schemeId)
	{
		_E828 obj = this.m__E001.FirstOrDefault((_E828 x) => x._id == schemeId);
		if (obj != null)
		{
			this.m__E000.StartProducing(obj);
		}
		_E006();
	}

	public override void SetInfo()
	{
	}

	public override void Close()
	{
		Singleton<_E815>.Instance.EnergyController.OnEnergyGenerationChanged -= _E000;
		if (this.m__E000 != null)
		{
			this.m__E000.OnProductionComplete -= _E005;
			this.m__E000.OnDataChanged -= _E001;
			this.m__E000.OnItemChanged -= _E002;
			this.m__E000.OnProduceStatusChanged -= _E006;
		}
		base.Close();
	}

	private void _E008()
	{
		IEnumerable<_E829> orderedItems = _E004();
		this.m__E003.UpdateOrder(orderedItems);
	}

	private void _E009(string value)
	{
		if (!value.IsNullOrEmpty())
		{
			this.m__E003.FilterBy((_E829 scheme) => scheme.endProduct.LocalizedName().IndexOf(value, StringComparison.InvariantCultureIgnoreCase) >= 0);
		}
		else
		{
			this.m__E003.FilterBy();
		}
	}

	[CompilerGenerated]
	private async Task _E00A()
	{
		await TasksExtensions.WaitUntil(() => this.m__E002 != null || this == null);
		if (this == null || this.m__E002 == null)
		{
			return;
		}
		foreach (KeyValuePair<_E828, _E825> item in this.m__E002)
		{
			item.Value?.UpdateView();
		}
	}

	[CompilerGenerated]
	private bool _E00B()
	{
		if (this.m__E002 == null)
		{
			return this == null;
		}
		return true;
	}
}
