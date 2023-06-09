using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public sealed class HandoverRagfairMoneyWindow : MessageWindow
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public int newCount;

		internal int _E000(KeyValuePair<_E7D3, int> offer)
		{
			return newCount;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Dictionary<ECurrencyType, int> neededAmount;

		public Dictionary<ECurrencyType, int> existingAmount;

		public HandoverRagfairMoneyWindow _003C_003E4__this;

		internal void _E000()
		{
			bool flag = true;
			foreach (ECurrencyType value in Enum.GetValues(typeof(ECurrencyType)))
			{
				int num = neededAmount[value];
				if (num <= 0 || existingAmount[value] >= num)
				{
					continue;
				}
				flag = false;
				break;
			}
			if (!flag)
			{
				_003C_003E4__this._notEnoughMoneyWindow.Show(_ED3E._E000(232963).Localized(), _ED3E._E000(232282).Localized(), _003C_003E4__this._E023, null);
			}
			else
			{
				_003C_003E4__this._E022(new _E7D8(_003C_003E4__this._E021));
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public ECurrencyType moneyType;

		internal bool _E000(_E7D4 x)
		{
			return _EA10.GetCurrencyTypeById(x.TemplateId) == moneyType;
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public Item item;

		internal bool _E000(_E7D4 x)
		{
			return x.TemplateId == item.TemplateId;
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public _EA10._E000 currencyData;

		internal bool _E000(_EA76 x)
		{
			return x.TemplateId == currencyData.Id;
		}
	}

	private const string _E01D = "ragfair/You don't have enough money for the transaction";

	private const string _E01E = "ragfair/Are you sure you want to buy {0} ({1}) for {2}?";

	private const string _E01F = "Are you sure you want to buy selected items for {0}?";

	private const string _E020 = "ragfair/Item purchase";

	[SerializeField]
	private GameObject _countPanel;

	[SerializeField]
	private MessageWindow _notEnoughMoneyWindow;

	[SerializeField]
	private CustomTextMeshProInputField _inputField;

	[SerializeField]
	private CustomTextMeshProUGUI _maxCountLabel;

	[SerializeField]
	private Button _allItemsButton;

	[NonSerialized]
	public Dictionary<_E7D3, int> Goods;

	private _E7D8 _E021 = new _E7D8();

	private Item[] _E015;

	private Action<_E7D8> _E022;

	private Action _E023;

	private DateTime _E018;

	private EMemberCategory _E024;

	private IEnumerator _E019;

	private TimeSpan _E000 => _E018 - _E5AD.UtcNow;

	protected override void Awake()
	{
		base.Awake();
		_inputField.onValueChanged.AddListener(delegate(string arg)
		{
			if (base.gameObject.activeSelf)
			{
				_E3EF.RagfairValidation(arg, _inputField, ref Goods, SetAcceptActive, delegate
				{
					_E000(_E015);
				});
			}
		});
		_allItemsButton.onClick.AddListener(delegate
		{
			int newCount = Goods.Min((KeyValuePair<_E7D3, int> x) => x.Key.CurrentItemCount);
			Dictionary<_E7D3, int> goods = Goods.ToDictionary((KeyValuePair<_E7D3, int> offer) => offer.Key, (KeyValuePair<_E7D3, int> offer) => newCount);
			Goods = goods;
			_inputField.text = newCount.ToString(_ED3E._E000(27314));
		});
	}

	public void Show(_EAE7 inventory, bool displayInputField, Dictionary<_E7D3, int> goods, Action<_E7D8> acceptAction, Action cancelAction)
	{
		Goods = goods;
		_E024 = ((!Goods.Any((KeyValuePair<_E7D3, int> x) => x.Key.MemberType != EMemberCategory.Trader)) ? EMemberCategory.Trader : EMemberCategory.Default);
		_E018 = Goods.Min((KeyValuePair<_E7D3, int> x) => x.Key.EndTime);
		_E021 = new _E7D8();
		_E022 = acceptAction;
		_E023 = cancelAction;
		_E015 = inventory.Stash.GetAllItemsFromCollection().ToArray();
		_E000(_E015);
		if (displayInputField)
		{
			_inputField.text = _ED3E._E000(27343);
		}
		string text = Goods.Min((KeyValuePair<_E7D3, int> x) => x.Key.CurrentItemCount).ToString();
		_maxCountLabel.text = _ED3E._E000(124720) + text;
		if (_E024 != EMemberCategory.Trader)
		{
			_E002();
			_E019 = this.StartBehaviourTimer(1f, repeatable: true, _E002);
		}
		_countPanel.gameObject.SetActive(!Goods.Any((KeyValuePair<_E7D3, int> x) => x.Key.SellInOnePiece) && displayInputField);
	}

	private void _E000(Item[] playerItems)
	{
		_E021.Clear();
		Dictionary<ECurrencyType, int> existingAmount = new Dictionary<ECurrencyType, int>
		{
			{
				ECurrencyType.RUB,
				0
			},
			{
				ECurrencyType.EUR,
				0
			},
			{
				ECurrencyType.USD,
				0
			}
		};
		Dictionary<ECurrencyType, int> neededAmount = new Dictionary<ECurrencyType, int>
		{
			{
				ECurrencyType.RUB,
				0
			},
			{
				ECurrencyType.EUR,
				0
			},
			{
				ECurrencyType.USD,
				0
			}
		};
		Dictionary<_E7D3, _E7D9> dictionary = _EC47.RecalculateReferences(playerItems, Goods);
		int value;
		ECurrencyType key2;
		foreach (KeyValuePair<_E7D3, int> good in Goods)
		{
			_E39D.Deconstruct(good, out var key, out value);
			_E7D3 obj = key;
			int num = value;
			int count = (obj.SellInOnePiece ? obj.Item.StackObjectsCount : num);
			List<_E7D4> itemReferences = dictionary[obj].ItemReferences;
			Dictionary<Item, int> itemsToHandover = dictionary[obj].ItemsToHandover;
			foreach (_E7D4 item3 in itemReferences)
			{
				ECurrencyType moneyType = _EA10.GetCurrencyTypeById(item3.TemplateId);
				int num2 = itemReferences.Where((_E7D4 x) => _EA10.GetCurrencyTypeById(x.TemplateId) == moneyType).Sum((_E7D4 x) => x.RequiredCount);
				Dictionary<ECurrencyType, int> dictionary2 = neededAmount;
				key2 = moneyType;
				dictionary2[key2] += num2;
			}
			List<_E7D4> list = new List<_E7D4>();
			foreach (KeyValuePair<Item, int> item4 in itemsToHandover)
			{
				_E39D.Deconstruct(item4, out var key3, out value);
				Item item = key3;
				int b = value;
				int num3 = list.Sum((_E7D4 y) => y.ExistingCount);
				int num4 = Mathf.RoundToInt(Mathf.Min(itemReferences.Where((_E7D4 x) => x.TemplateId == item.TemplateId).Sum((_E7D4 y) => y.RequiredCount) - num3, b));
				if (num4 <= 0)
				{
					break;
				}
				_E7D4 item2 = new _E7D4
				{
					Id = item.Id,
					TemplateId = item.TemplateId,
					ExistingCount = num4
				};
				list.Add(item2);
			}
			_E021.Add(new _E7D7(obj, count, list.ToList()));
		}
		foreach (KeyValuePair<ECurrencyType, _EA10._E000> item5 in _EA10.CurrencyIndex)
		{
			_E39D.Deconstruct(item5, out key2, out var value2);
			ECurrencyType num5 = key2;
			_EA10._E000 currencyData = value2;
			Dictionary<ECurrencyType, int> dictionary2 = existingAmount;
			key2 = num5;
			dictionary2[key2] += (from x in playerItems.OfType<_EA76>()
				where x.TemplateId == currencyData.Id
				select x).Sum((_EA76 x) => x.StackObjectsCount);
		}
		string message;
		if (Goods.Count == 1)
		{
			_E39D.Deconstruct(neededAmount.FirstOrDefault((KeyValuePair<ECurrencyType, int> x) => x.Value > 0), out key2, out value);
			ECurrencyType moneyType2 = key2;
			int number = value;
			string arg = _ED3E._E000(2540) + Goods.FirstOrDefault().Key.Item.ShortName.Localized();
			string arg2 = _ED3E._E000(260834) + _E021.Sum((_E7D7 x) => x.Count) + _ED3E._E000(47210);
			string arg3 = _ED3E._E000(2540) + number.FormatSeparate(_ED3E._E000(18502)) + _E001(moneyType2);
			message = string.Format(_ED3E._E000(232065).Localized(), arg, arg2, arg3);
		}
		else
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<ECurrencyType, int> item6 in neededAmount)
			{
				_E39D.Deconstruct(item6, out key2, out value);
				ECurrencyType moneyType3 = key2;
				int num6 = value;
				if (num6 > 0)
				{
					string value3 = num6.FormatSeparate(_ED3E._E000(18502)) + _ED3E._E000(18502) + _E001(moneyType3) + _ED3E._E000(18502);
					stringBuilder.Append(value3);
				}
			}
			message = string.Format(_ED3E._E000(232137).Localized(), string.Format(_ED3E._E000(232214), stringBuilder));
		}
		Show(_ED3E._E000(232211).Localized(), message, delegate
		{
			bool flag = true;
			foreach (ECurrencyType value4 in Enum.GetValues(typeof(ECurrencyType)))
			{
				int num7 = neededAmount[value4];
				if (num7 > 0 && existingAmount[value4] < num7)
				{
					flag = false;
					break;
				}
			}
			if (!flag)
			{
				_notEnoughMoneyWindow.Show(_ED3E._E000(232963).Localized(), _ED3E._E000(232282).Localized(), _E023, null);
			}
			else
			{
				_E022(new _E7D8(_E021));
			}
		}, _E023);
	}

	private string _E001(ECurrencyType moneyType)
	{
		return moneyType switch
		{
			ECurrencyType.RUB => _ED3E._E000(232249), 
			ECurrencyType.EUR => _ED3E._E000(232236), 
			ECurrencyType.USD => _ED3E._E000(232231), 
			_ => string.Empty, 
		};
	}

	private void _E002()
	{
		if (this._E000.TotalSeconds <= 0.0)
		{
			Close();
		}
	}

	public override void Close()
	{
		if (_E024 != EMemberCategory.Trader)
		{
			this.StopBehaviourTimer(ref _E019);
		}
		base.Close();
	}

	[CompilerGenerated]
	private void _E003(string arg)
	{
		if (base.gameObject.activeSelf)
		{
			_E3EF.RagfairValidation(arg, _inputField, ref Goods, SetAcceptActive, delegate
			{
				_E000(_E015);
			});
		}
	}

	[CompilerGenerated]
	private void _E004()
	{
		_E000(_E015);
	}

	[CompilerGenerated]
	private void _E005()
	{
		int newCount = Goods.Min((KeyValuePair<_E7D3, int> x) => x.Key.CurrentItemCount);
		Dictionary<_E7D3, int> goods = Goods.ToDictionary((KeyValuePair<_E7D3, int> offer) => offer.Key, (KeyValuePair<_E7D3, int> offer) => newCount);
		Goods = goods;
		_inputField.text = newCount.ToString(_ED3E._E000(27314));
	}
}
