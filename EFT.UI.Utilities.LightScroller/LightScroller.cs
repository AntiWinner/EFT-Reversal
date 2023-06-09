using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Utilities.LightScroller;

public sealed class LightScroller : UIElement, IScrollHandler, IEventSystemHandler
{
	public enum EScrollDirection
	{
		Vertical,
		Horizontal
	}

	public enum EScrollOrder
	{
		Straight,
		Reversed
	}

	public enum EScrollbarVisibilityEnum
	{
		Always,
		IfNecessary,
		Never
	}

	private interface _E000 : IDisposable
	{
		void MarkAsDeprecated();

		void CleanDeprecated();
	}

	private sealed class _E001<_E0BF, _E0C0> : _E000, IDisposable where _E0C0 : UIElement
	{
		private sealed class _E000 : IDisposable
		{
			public readonly _E0BF Item;

			public readonly _E0C0 ItemView;

			public bool Deprecated;

			public _E000(_E0BF item, _E0C0 itemView)
			{
				Item = item;
				ItemView = itemView;
			}

			public void Recycle()
			{
				ItemView.Close();
			}

			public void Dispose()
			{
				ItemView.Close();
				UnityEngine.Object.DestroyImmediate(ItemView.gameObject);
			}
		}

		[CompilerGenerated]
		private sealed class _E001
		{
			public _E0BF item;

			internal bool _E000(_E000 cache)
			{
				_E0BF val = cache.Item;
				if (val.Equals(item))
				{
					return !cache.Deprecated;
				}
				return false;
			}
		}

		[CompilerGenerated]
		private sealed class _E002
		{
			public _E0BF item;

			internal bool _E000(_E000 cache)
			{
				_E0BF val = cache.Item;
				if (val.Equals(item))
				{
					return cache.Deprecated;
				}
				return false;
			}
		}

		private readonly _E002<_E0BF, _E0C0> m__E000;

		private readonly _E003<_E0BF, Enum> m__E001;

		private readonly RectTransform m__E002;

		private readonly RectTransform _E003;

		private readonly RectTransform _E004;

		private readonly List<_E000> _E005 = new List<_E000>();

		private readonly Dictionary<Enum, List<_E0C0>> _E006 = new Dictionary<Enum, List<_E0C0>>();

		private readonly Action<_E0BF, _E0C0> _E007;

		private readonly bool _E008;

		public _E001(_E002<_E0BF, _E0C0> viewTemplateFactory, _E003<_E0BF, Enum> dataTypeGetter, RectTransform container, RectTransform rebuildContainer, RectTransform storage, Action<_E0BF, _E0C0> showAction)
		{
			_E008 = true;
			this._E000 = viewTemplateFactory;
			this._E001 = dataTypeGetter;
			this._E002 = container;
			_E003 = rebuildContainer;
			_E004 = storage;
			_E007 = showAction;
		}

		public _E001(_E002<_E0BF, _E0C0> viewTemplateFactory, RectTransform container, RectTransform rebuildContainer, Action<_E0BF, _E0C0> showAction)
		{
			this._E000 = viewTemplateFactory;
			this._E002 = container;
			_E003 = rebuildContainer;
			_E007 = showAction;
		}

		public Vector2? GetViewSize(_E0BF item)
		{
			return ((RectTransform)(_E005.FirstOrDefault(delegate(_E000 cache)
			{
				_E0BF item2 = cache.Item;
				return item2.Equals(item) && !cache.Deprecated;
			})?.ItemView.Transform))?.rect.size;
		}

		public _E0C0 GetItemView(_E0BF item)
		{
			_E000 obj = _E005.FirstOrDefault(delegate(_E000 cache)
			{
				_E0BF item2 = cache.Item;
				return item2.Equals(item) && cache.Deprecated;
			});
			if (obj == null)
			{
				_E0C0 val;
				if (_E008 && _E006.TryGetValue(this._E001(item), out var value) && value.Any())
				{
					val = value.First();
					value.Remove(val);
					val.transform.SetParent(_E003);
				}
				else
				{
					val = UnityEngine.Object.Instantiate(this._E000(item).gameObject, _E003, worldPositionStays: false).GetComponent<_E0C0>();
				}
				_E007?.Invoke(item, val);
				_E003.gameObject.SetActive(value: true);
				LayoutRebuilder.ForceRebuildLayoutImmediate(_E003);
				val.transform.SetParent(this._E002);
				_E003.gameObject.SetActive(value: false);
				obj = new _E000(item, val);
				_E005.Add(obj);
			}
			obj.Deprecated = false;
			return obj.ItemView;
		}

		public void MarkAsDeprecated()
		{
			_E005.ForEach(delegate(_E000 cachedItem)
			{
				cachedItem.Deprecated = true;
			});
		}

		public void CleanDeprecated()
		{
			for (int num = _E005.Count - 1; num >= 0; num--)
			{
				_E000 obj = _E005[num];
				if (obj.Deprecated)
				{
					if (_E008)
					{
						Enum key = this._E001(obj.Item);
						if (!_E006.TryGetValue(key, out var value))
						{
							value = new List<_E0C0>();
							_E006.Add(key, value);
						}
						obj.Recycle();
						_E0C0 itemView = obj.ItemView;
						itemView.transform.SetParent(_E004);
						value.Add(itemView);
					}
					else
					{
						obj.Dispose();
					}
					_E005.Remove(obj);
				}
			}
		}

		public void Dispose()
		{
			_E005.ForEach(delegate(_E000 cachedItem)
			{
				cachedItem.Dispose();
			});
			_E005.Clear();
			_E006.Clear();
		}
	}

	public delegate _E0C0 _E002<in _E0C1, out _E0C0>(_E0C1 data);

	public delegate _E0C2 _E003<in _E0C1, out _E0C2>(_E0C1 data);

	[CompilerGenerated]
	private sealed class _E004<_E0BF, _E0C0> where _E0C0 : UIElement
	{
		public _E002<_E0BF, _E0C0> viewTemplateFactory;

		public _E003<_E0BF, Enum> dataTypeGetter;

		public LightScroller _003C_003E4__this;

		public RectTransform cacheStorage;

		public Action<_E0BF, _E0C0> showAction;

		public _ECEF<_E0BF> data;

		internal _E000 _E000()
		{
			_E005<_E0BF, _E0C0> CS_0024_003C_003E8__locals0 = new _E005<_E0BF, _E0C0>
			{
				CS_0024_003C_003E8__locals1 = this,
				cache = new _E001<_E0BF, _E0C0>(viewTemplateFactory, dataTypeGetter, _003C_003E4__this._E228, _003C_003E4__this._E229, cacheStorage, showAction)
			};
			_003C_003E4__this._E237 = (int index) => CS_0024_003C_003E8__locals0.cache.GetItemView(CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.data[index]);
			_003C_003E4__this._E238 = (int index) => CS_0024_003C_003E8__locals0.cache.GetViewSize(CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.data[index]);
			return CS_0024_003C_003E8__locals0.cache;
		}

		internal void _E001()
		{
			UnityEngine.Object.DestroyImmediate(cacheStorage.gameObject);
		}
	}

	[CompilerGenerated]
	private sealed class _E005<_E0BF, _E0C0> where _E0C0 : UIElement
	{
		public _E001<_E0BF, _E0C0> cache;

		public _E004<_E0BF, _E0C0> CS_0024_003C_003E8__locals1;

		internal UIElement _E000(int index)
		{
			return cache.GetItemView(CS_0024_003C_003E8__locals1.data[index]);
		}

		internal Vector2? _E001(int index)
		{
			return cache.GetViewSize(CS_0024_003C_003E8__locals1.data[index]);
		}
	}

	[CompilerGenerated]
	private sealed class _E006<_E0BF, _E0C0> where _E0C0 : UIElement
	{
		public _E002<_E0BF, _E0C0> viewTemplateFactory;

		public LightScroller _003C_003E4__this;

		public Action<_E0BF, _E0C0> showAction;

		public _ECEF<_E0BF> data;

		internal _E000 _E000()
		{
			_E007<_E0BF, _E0C0> CS_0024_003C_003E8__locals0 = new _E007<_E0BF, _E0C0>
			{
				CS_0024_003C_003E8__locals1 = this,
				cache = new _E001<_E0BF, _E0C0>(viewTemplateFactory, _003C_003E4__this._E228, _003C_003E4__this._E229, showAction)
			};
			_003C_003E4__this._E237 = (int index) => CS_0024_003C_003E8__locals0.cache.GetItemView(CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.data[index]);
			_003C_003E4__this._E238 = (int index) => CS_0024_003C_003E8__locals0.cache.GetViewSize(CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.data[index]);
			return CS_0024_003C_003E8__locals0.cache;
		}
	}

	[CompilerGenerated]
	private sealed class _E007<_E0BF, _E0C0> where _E0C0 : UIElement
	{
		public _E001<_E0BF, _E0C0> cache;

		public _E006<_E0BF, _E0C0> CS_0024_003C_003E8__locals1;

		internal UIElement _E000(int index)
		{
			return cache.GetItemView(CS_0024_003C_003E8__locals1.data[index]);
		}

		internal Vector2? _E001(int index)
		{
			return cache.GetViewSize(CS_0024_003C_003E8__locals1.data[index]);
		}
	}

	[CompilerGenerated]
	private sealed class _E008<_E0BF>
	{
		public LightScroller _003C_003E4__this;

		public _ECEF<_E0BF> data;

		internal void _E000(_E0BF _)
		{
			_E002();
		}

		internal void _E001(IEnumerable<_E0BF> _)
		{
			_E002();
		}

		internal void _E002()
		{
			_003C_003E4__this._E22F = data.Count;
			_003C_003E4__this._E000 = _003C_003E4__this._E000;
			_003C_003E4__this._E230 = new List<float>(_003C_003E4__this._E22F);
			_003C_003E4__this._E231 = 0f;
			for (int i = 0; i < _003C_003E4__this._E22F; i++)
			{
				float num = _003C_003E4__this._E004(i, _003C_003E4__this._defaultViewSize);
				_003C_003E4__this._E230.Add(num);
				_003C_003E4__this._E231 += num;
			}
			_003C_003E4__this._E000();
			_003C_003E4__this._E0B3 = true;
		}

		internal void _E003()
		{
			data.ItemAdded -= delegate
			{
				_E002();
			};
			data.ItemRemoved -= delegate
			{
				_E002();
			};
			data.ItemsAdded -= delegate
			{
				_E002();
			};
			data.AllItemsRemoved -= delegate
			{
				_003C_003E4__this._E22F = data.Count;
				_003C_003E4__this._E000 = _003C_003E4__this._E000;
				_003C_003E4__this._E230 = new List<float>(_003C_003E4__this._E22F);
				_003C_003E4__this._E231 = 0f;
				for (int i = 0; i < _003C_003E4__this._E22F; i++)
				{
					float num = _003C_003E4__this._E004(i, _003C_003E4__this._defaultViewSize);
					_003C_003E4__this._E230.Add(num);
					_003C_003E4__this._E231 += num;
				}
				_003C_003E4__this._E000();
				_003C_003E4__this._E0B3 = true;
			};
			data.ItemUpdated -= delegate
			{
				_E002();
			};
		}
	}

	[SerializeField]
	private EScrollDirection _direction;

	[SerializeField]
	private EScrollOrder _order;

	[SerializeField]
	private EScrollbarVisibilityEnum _scrollBarVisibility = EScrollbarVisibilityEnum.IfNecessary;

	[SerializeField]
	private RectTransform _targetArea;

	[SerializeField]
	private Scrollbar _scrollbar;

	[SerializeField]
	private RectOffset _padding;

	[SerializeField]
	private float _spacing;

	[SerializeField]
	private TextAnchor _childAlignment;

	[Range(1f, 500f)]
	[SerializeField]
	private float _scrollSpeedPixels = 50f;

	[SerializeField]
	[Range(1f, 500f)]
	private float _defaultViewSize = 100f;

	private bool _E227;

	private bool _E03F;

	private RectTransform _E228;

	private RectTransform _E229;

	private _E000 _E22A;

	private bool _E22B;

	private int _E22C;

	private Vector2 _E22D;

	private float _E22E;

	private int _E22F;

	private List<float> _E230 = new List<float>();

	private float _E231;

	private float _E232;

	private float _E233;

	private bool _E234;

	private (float FirstPadding, float SecondPadding) _E235;

	private bool _E0B3;

	private bool _E236;

	private Func<int, UIElement> _E237;

	private Func<int, Vector2?> _E238;

	[CompilerGenerated]
	private float _E239;

	public float NormalizedScrollPosition
	{
		[CompilerGenerated]
		get
		{
			return _E239;
		}
		[CompilerGenerated]
		private set
		{
			_E239 = value;
		}
	}

	private float _E000
	{
		get
		{
			return _E22E;
		}
		set
		{
			_E22E = Mathf.Clamp(value, 0f, _E22F);
			int max = ((_E22F > 0) ? (_E22F - 1) : 0);
			_E22C = Mathf.Clamp((int)Math.Floor(_E22E), 0, max);
		}
	}

	private void _E000()
	{
		if (_E22B && !_E231.IsZero())
		{
			float value = _E007(_E22D) / _E231;
			_E236 = false;
			_scrollbar.size = Mathf.Clamp(value, 0.1f, 1f);
			_scrollbar.value = NormalizedScrollPosition;
			_E236 = true;
		}
	}

	private void OnEnable()
	{
		if (_E03F)
		{
			_scrollbar.numberOfSteps = 0;
			_E0B3 = true;
		}
	}

	public void OnScroll(PointerEventData eventData)
	{
		if (_E22F != 0 && _E03F)
		{
			_E0B3 = true;
			float deltaPixels = (0f - eventData.scrollDelta.y) * _scrollSpeedPixels;
			_E001(deltaPixels);
		}
	}

	private void _E001(float deltaPixels)
	{
		int num = ((_order == EScrollOrder.Straight) ? 1 : (-1));
		deltaPixels *= (float)num;
		float num2 = Math.Abs(deltaPixels);
		bool flag = deltaPixels > 0f;
		int num3 = (flag ? 1 : (-1));
		int num4 = _E22C;
		num2 += _E230[num4] * (flag ? (this._E000 - (float)num4) : ((float)num4 - this._E000 + 1f));
		while (num4 >= 0 && num4 < _E22F)
		{
			float num5 = _E230[num4] - num2;
			if (num5.Positive())
			{
				float num6 = num5 / _E230[num4];
				num6 = (flag ? (1f - num6) : num6);
				this._E000 = (float)num4 + num6;
				return;
			}
			num4 += num3;
			num2 = 0f - num5;
		}
		this._E000 = Mathf.Clamp(num4, 0, _E22F);
	}

	public void Show<TItemType, TViewType>(_ECEF<TItemType> data, _E002<TItemType, TViewType> viewTemplateFactory, _E003<TItemType, Enum> dataTypeGetter, Action<TItemType, TViewType> showAction) where TViewType : UIElement
	{
		RectTransform cacheStorage = new GameObject(_ED3E._E000(254197), typeof(RectTransform)).GetComponent<RectTransform>();
		cacheStorage.gameObject.SetActive(value: false);
		cacheStorage.SetParent(_targetArea);
		_E002(data, delegate
		{
			_E001<TItemType, TViewType> cache = new _E001<TItemType, TViewType>(viewTemplateFactory, dataTypeGetter, _E228, _E229, cacheStorage, showAction);
			_E237 = (int index) => cache.GetItemView(data[index]);
			_E238 = (int index) => cache.GetViewSize(data[index]);
			return cache;
		});
		UI.AddDisposable(delegate
		{
			UnityEngine.Object.DestroyImmediate(cacheStorage.gameObject);
		});
	}

	public void Show<TItemType, TViewType>(_ECEF<TItemType> data, _E002<TItemType, TViewType> viewTemplateFactory, Action<TItemType, TViewType> showAction) where TViewType : UIElement
	{
		_E002(data, delegate
		{
			_E001<TItemType, TViewType> cache = new _E001<TItemType, TViewType>(viewTemplateFactory, _E228, _E229, showAction);
			_E237 = (int index) => cache.GetItemView(data[index]);
			_E238 = (int index) => cache.GetViewSize(data[index]);
			return cache;
		});
	}

	private void _E002<_E0BF>(_ECEF<_E0BF> data, Func<_E000> cacheFactory)
	{
		_E008<_E0BF> CS_0024_003C_003E8__locals0 = new _E008<_E0BF>();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		CS_0024_003C_003E8__locals0.data = data;
		UI.Dispose();
		ShowGameObject();
		_E003();
		_E22A = cacheFactory();
		_E03F = true;
		_E232 = 0f;
		CS_0024_003C_003E8__locals0._E002();
		CS_0024_003C_003E8__locals0.data.ItemAdded += delegate
		{
			CS_0024_003C_003E8__locals0._E002();
		};
		CS_0024_003C_003E8__locals0.data.ItemRemoved += delegate
		{
			CS_0024_003C_003E8__locals0._E002();
		};
		CS_0024_003C_003E8__locals0.data.ItemsAdded += delegate
		{
			CS_0024_003C_003E8__locals0._E002();
		};
		CS_0024_003C_003E8__locals0.data.AllItemsRemoved += delegate
		{
			CS_0024_003C_003E8__locals0._003C_003E4__this._E22F = CS_0024_003C_003E8__locals0.data.Count;
			CS_0024_003C_003E8__locals0._003C_003E4__this._E000 = CS_0024_003C_003E8__locals0._003C_003E4__this._E000;
			CS_0024_003C_003E8__locals0._003C_003E4__this._E230 = new List<float>(CS_0024_003C_003E8__locals0._003C_003E4__this._E22F);
			CS_0024_003C_003E8__locals0._003C_003E4__this._E231 = 0f;
			for (int j = 0; j < CS_0024_003C_003E8__locals0._003C_003E4__this._E22F; j++)
			{
				float num2 = CS_0024_003C_003E8__locals0._003C_003E4__this._E004(j, CS_0024_003C_003E8__locals0._003C_003E4__this._defaultViewSize);
				CS_0024_003C_003E8__locals0._003C_003E4__this._E230.Add(num2);
				CS_0024_003C_003E8__locals0._003C_003E4__this._E231 = CS_0024_003C_003E8__locals0._003C_003E4__this._E231 + num2;
			}
			CS_0024_003C_003E8__locals0._003C_003E4__this._E000();
			CS_0024_003C_003E8__locals0._003C_003E4__this._E0B3 = true;
		};
		CS_0024_003C_003E8__locals0.data.ItemUpdated += delegate
		{
			CS_0024_003C_003E8__locals0._E002();
		};
		_E22D = new Vector2(_targetArea.rect.width, _targetArea.rect.height);
		SetScrollPosition(0f);
		UI.AddDisposable(_E22A);
		UI.AddDisposable(delegate
		{
			CS_0024_003C_003E8__locals0.data.ItemAdded -= delegate
			{
				CS_0024_003C_003E8__locals0._E002();
			};
			CS_0024_003C_003E8__locals0.data.ItemRemoved -= delegate
			{
				CS_0024_003C_003E8__locals0._E002();
			};
			CS_0024_003C_003E8__locals0.data.ItemsAdded -= delegate
			{
				CS_0024_003C_003E8__locals0._E002();
			};
			CS_0024_003C_003E8__locals0.data.AllItemsRemoved -= delegate
			{
				CS_0024_003C_003E8__locals0._003C_003E4__this._E22F = CS_0024_003C_003E8__locals0.data.Count;
				CS_0024_003C_003E8__locals0._003C_003E4__this._E000 = CS_0024_003C_003E8__locals0._003C_003E4__this._E000;
				CS_0024_003C_003E8__locals0._003C_003E4__this._E230 = new List<float>(CS_0024_003C_003E8__locals0._003C_003E4__this._E22F);
				CS_0024_003C_003E8__locals0._003C_003E4__this._E231 = 0f;
				for (int i = 0; i < CS_0024_003C_003E8__locals0._003C_003E4__this._E22F; i++)
				{
					float num = CS_0024_003C_003E8__locals0._003C_003E4__this._E004(i, CS_0024_003C_003E8__locals0._003C_003E4__this._defaultViewSize);
					CS_0024_003C_003E8__locals0._003C_003E4__this._E230.Add(num);
					CS_0024_003C_003E8__locals0._003C_003E4__this._E231 = CS_0024_003C_003E8__locals0._003C_003E4__this._E231 + num;
				}
				CS_0024_003C_003E8__locals0._003C_003E4__this._E000();
				CS_0024_003C_003E8__locals0._003C_003E4__this._E0B3 = true;
			};
			CS_0024_003C_003E8__locals0.data.ItemUpdated -= delegate
			{
				CS_0024_003C_003E8__locals0._E002();
			};
		});
		if (_E22B)
		{
			UI.SubscribeEvent(_scrollbar.onValueChanged, SetScrollPosition);
		}
	}

	public void SetScrollPosition(float position)
	{
		if (!_E236)
		{
			return;
		}
		position = Mathf.Clamp(position, 0f, 1f);
		NormalizedScrollPosition = position;
		_E0B3 = true;
		float num = 0f;
		float num2 = _E231 - _E007(_E22D);
		float num3 = num2 * position;
		for (int i = 0; i < _E22F; i++)
		{
			float num4 = _E230[i];
			num += num4;
			if (num / num2 >= position)
			{
				float num5 = num - num3;
				this._E000 = (float)i + (num4 - num5) / num4;
				return;
			}
		}
		NormalizedScrollPosition = 0f;
		this._E000 = 0f;
	}

	private void _E003()
	{
		if (!_E227)
		{
			_E227 = true;
			_defaultViewSize = Mathf.Clamp(_defaultViewSize, 1f, 500f);
			bool flag = _direction == EScrollDirection.Vertical;
			bool flag2 = _order == EScrollOrder.Straight;
			Type type = (flag ? typeof(VerticalLayoutGroup) : typeof(HorizontalLayoutGroup));
			GameObject gameObject = new GameObject(_ED3E._E000(254186), typeof(RectTransform), type);
			float item = ((!flag) ? (flag2 ? _padding.left : _padding.right) : (flag2 ? _padding.top : _padding.bottom));
			float item2 = ((!flag) ? (flag2 ? _padding.right : _padding.left) : (flag2 ? _padding.bottom : _padding.top));
			_E235 = (item, item2);
			HorizontalOrVerticalLayoutGroup component = gameObject.GetComponent<HorizontalOrVerticalLayoutGroup>();
			component.childControlWidth = true;
			component.childControlHeight = true;
			component.childForceExpandWidth = false;
			component.childForceExpandHeight = false;
			component.padding.top = ((!flag) ? _padding.top : 0);
			component.padding.bottom = ((!flag) ? _padding.bottom : 0);
			component.padding.left = (flag ? _padding.left : 0);
			component.padding.right = (flag ? _padding.right : 0);
			component.spacing = _spacing;
			component.childAlignment = _childAlignment;
			ContentSizeFitter contentSizeFitter = gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = ((!flag) ? ContentSizeFitter.FitMode.PreferredSize : ContentSizeFitter.FitMode.Unconstrained);
			contentSizeFitter.verticalFit = (flag ? ContentSizeFitter.FitMode.PreferredSize : ContentSizeFitter.FitMode.Unconstrained);
			float num = (flag2 ? 0f : 1f);
			float num2 = (flag2 ? 1f : 0f);
			Vector2 pivot = new Vector2(flag ? 0.5f : num, flag ? num2 : 0.5f);
			float num3 = (flag2 ? 1f : 0f);
			float num4 = (flag2 ? 0f : 1f);
			_E228 = gameObject.GetComponent<RectTransform>();
			_E228.SetParent(_targetArea, worldPositionStays: false);
			_E228.pivot = pivot;
			_E228.anchorMin = new Vector2(flag ? 0f : num4, flag ? num3 : 0f);
			_E228.anchorMax = new Vector2(flag ? 1f : num4, flag ? num3 : 1f);
			_E228.offsetMin = Vector2.zero;
			_E228.offsetMax = Vector2.zero;
			_E229 = (RectTransform)UnityEngine.Object.Instantiate(gameObject, _targetArea).transform;
			_E229.gameObject.name = _ED3E._E000(254231);
			_E229.gameObject.SetActive(value: false);
			CanvasGroup canvasGroup = _E229.gameObject.AddComponent<CanvasGroup>();
			canvasGroup.alpha = 0f;
			canvasGroup.blocksRaycasts = false;
			canvasGroup.interactable = false;
			gameObject.AddComponent<NonDrawingGraphic>().color = new Color(0f, 0f, 0f, 0f);
			if (!(_scrollbar == null))
			{
				_E22B = _scrollBarVisibility != EScrollbarVisibilityEnum.Never;
				_scrollbar.gameObject.SetActive(value: false);
			}
		}
	}

	private void Update()
	{
		if (!_E227 || !_E03F || _targetArea == null || _targetArea.gameObject == null)
		{
			return;
		}
		Rect rect = _targetArea.rect;
		Vector2 vector = new Vector2(rect.width, rect.height);
		if (Math.Abs(vector.x - _E22D.x) >= 1f || Math.Abs(vector.y - _E22D.y) >= 1f)
		{
			_E22D = vector;
			_E0B3 = true;
		}
		if (!_E0B3 && !_E234 && !(_E006(_E228) - _E232).IsZero())
		{
			Vector2? vector2 = _E238(_E22C);
			float num = _E230[_E22C];
			float num2 = _E004(_E22C, _E007(vector2.Value));
			if (!(num - num2).IsZero())
			{
				float value = num * (this._E000 - (float)_E22C) / num2;
				this._E000 = (float)_E22C + Mathf.Clamp(value, 0f, 1f);
			}
			_E0B3 = true;
		}
		_E234 = false;
		if (_E0B3)
		{
			_E005();
		}
	}

	private float _E004(int itemIndex, float transformSize)
	{
		float num = transformSize + _spacing;
		if (itemIndex == 0)
		{
			num += _E235.FirstPadding - _spacing / 2f;
		}
		else if (itemIndex == _E22F - 1)
		{
			num += _E235.SecondPadding - _spacing / 2f;
		}
		if (!num.Positive())
		{
			num = 0.01f;
		}
		return num;
	}

	private void _E005()
	{
		_E233 = 0f;
		_E0B3 = false;
		_E234 = true;
		_E22A.MarkAsDeprecated();
		if (_E22F == 0)
		{
			_E22A.CleanDeprecated();
			_E232 = 0f;
			if (_E22B && _scrollBarVisibility == EScrollbarVisibilityEnum.IfNecessary)
			{
				_scrollbar.gameObject.SetActive(value: false);
			}
			return;
		}
		bool flag = _order == EScrollOrder.Straight;
		bool flag2 = _direction == EScrollDirection.Vertical;
		_E232 = 0f - _spacing;
		float num = _E007(_E22D);
		float num2 = _E009(_E22C, flag);
		float num3 = num2 * (this._E000 - (float)_E22C);
		int num4 = _E22C + 1;
		while (_E233 - num3 < num && _E008(num4))
		{
			_E009(num4, !flag);
			num4++;
		}
		bool flag3 = false;
		if (_E233 - num3 < num)
		{
			flag3 = true;
			num3 = _E233 - num;
			num3 = Mathf.Clamp(num3, 0f, num2);
			this._E000 = (float)_E22C + num3 / num2;
		}
		num4 = _E22C - 1;
		while (_E233 < num && _E008(num4))
		{
			float num5 = _E009(num4, flag);
			if (_E233 >= num)
			{
				num3 = _E233 - num;
				this._E000 = (float)num4 + num3 / num5;
			}
			num4--;
		}
		_E22A.CleanDeprecated();
		float value = _E232 + _E235.FirstPadding + _E235.SecondPadding - num;
		if (value.Negative())
		{
			flag3 = false;
			num3 = 0f;
			this._E000 = 0f;
		}
		float num6 = (flag3 ? (_E232 - num + _E235.SecondPadding) : (num3 - _spacing / 2f));
		if (_E22C == 0)
		{
			num6 -= _E235.FirstPadding - _spacing / 2f;
		}
		num6 *= (flag ? 1f : (-1f));
		Vector2 anchoredPosition = _E228.anchoredPosition;
		if (flag2)
		{
			anchoredPosition.y = num6;
		}
		else
		{
			anchoredPosition.x = num6;
		}
		_E228.anchoredPosition = anchoredPosition;
		NormalizedScrollPosition = 0f;
		float num7 = _E231 - num;
		float num8 = _E231;
		for (int num9 = _E22F - 1; num9 >= 0; num9--)
		{
			float num10 = _E230[num9];
			num8 -= num10;
			if (!(num7 < num8))
			{
				num3 = num7 - num8;
				float num11 = num3 / num10;
				float num12 = (float)num9 + num11;
				NormalizedScrollPosition = this._E000 / num12;
				break;
			}
		}
		if (_E22B)
		{
			if (_scrollBarVisibility == EScrollbarVisibilityEnum.IfNecessary)
			{
				_scrollbar.gameObject.SetActive(!value.Negative());
			}
			_E000();
		}
	}

	public override void Close()
	{
		UI.Dispose();
		_E03F = false;
		_E22F = 0;
		_E22C = 0;
		_E22E = 0f;
		_E231 = 0f;
		_E0B3 = false;
		_E230.Clear();
		base.Close();
	}

	private float _E006(RectTransform rectTransform)
	{
		if (_direction != 0)
		{
			return rectTransform.rect.width;
		}
		return rectTransform.rect.height;
	}

	private float _E007(Vector2 vector)
	{
		if (_direction != 0)
		{
			return vector.x;
		}
		return vector.y;
	}

	[CompilerGenerated]
	private bool _E008(int index)
	{
		if (index < _E22F)
		{
			return index >= 0;
		}
		return false;
	}

	[CompilerGenerated]
	private float _E009(int itemIndex, bool putFirst)
	{
		RectTransform rectTransform = (RectTransform)_E237(itemIndex).transform;
		float num = _E006(rectTransform);
		_E232 += num + _spacing;
		num = _E004(itemIndex, num);
		_E233 += num;
		_E231 += num - _E230[itemIndex];
		_E230[itemIndex] = num;
		if (putFirst)
		{
			rectTransform.SetAsFirstSibling();
		}
		else
		{
			rectTransform.SetAsLastSibling();
		}
		return num;
	}
}
