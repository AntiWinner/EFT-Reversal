using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class InteractionButtonsContainer : UIElement, _EC4C, IPointerExitHandler, IEventSystemHandler
{
	[CompilerGenerated]
	private sealed class _E000<_E077> where _E077 : Enum
	{
		public InteractionButtonsContainer _003C_003E4__this;

		public _EC4E<_E077> contextInteractions;

		public List<_E077> prohibitedInteractions;

		public IReadOnlyDictionary<_E077, string> names;

		public IReadOnlyDictionary<_E077, Sprite> icons;

		public bool autoClose;

		public Item item;

		internal void _E000()
		{
			_003C_003E4__this._E160.Dispose();
			bool active = false;
			foreach (_E077 availableInteraction in contextInteractions.AvailableInteractions)
			{
				if ((prohibitedInteractions == null || !prohibitedInteractions.Contains(availableInteraction)) && contextInteractions.IsActive(availableInteraction))
				{
					active = true;
					if (names == null || !names.TryGetValue(availableInteraction, out var value))
					{
						value = availableInteraction.ToString();
					}
					if (icons == null || !icons.TryGetValue(availableInteraction, out var value2))
					{
						value2 = _E905.Pop<Sprite>(_ED3E._E000(246119) + availableInteraction);
					}
					_003C_003E4__this._E002(availableInteraction, contextInteractions, value, value2, autoClose);
				}
			}
			foreach (_EC4D dynamicInteraction in contextInteractions.DynamicInteractions)
			{
				_003C_003E4__this._E003(dynamicInteraction);
				active = true;
			}
			if (_003C_003E4__this._E162)
			{
				_003C_003E4__this._background.SetActive(active);
			}
		}

		internal void _E001(IEnumerable<string> templateIds)
		{
			if (templateIds == null || templateIds.Contains(item.TemplateId))
			{
				_E000();
			}
		}

		internal void _E002(bool locked)
		{
			_E000();
		}

		internal void _E003(_E992 effect)
		{
			if (effect is _E9BF)
			{
				_E000();
			}
		}

		internal void _E004()
		{
			contextInteractions.OnRedrawRequired -= delegate
			{
				_003C_003E4__this._E160.Dispose();
				bool active = false;
				foreach (_E077 availableInteraction in contextInteractions.AvailableInteractions)
				{
					if ((prohibitedInteractions == null || !prohibitedInteractions.Contains(availableInteraction)) && contextInteractions.IsActive(availableInteraction))
					{
						active = true;
						if (names == null || !names.TryGetValue(availableInteraction, out var value))
						{
							value = availableInteraction.ToString();
						}
						if (icons == null || !icons.TryGetValue(availableInteraction, out var value2))
						{
							value2 = _E905.Pop<Sprite>(_ED3E._E000(246119) + availableInteraction);
						}
						_003C_003E4__this._E002(availableInteraction, contextInteractions, value, value2, autoClose);
					}
				}
				foreach (_EC4D dynamicInteraction in contextInteractions.DynamicInteractions)
				{
					_003C_003E4__this._E003(dynamicInteraction);
					active = true;
				}
				if (_003C_003E4__this._E162)
				{
					_003C_003E4__this._background.SetActive(active);
				}
			};
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public bool autoClose;

		public InteractionButtonsContainer _003C_003E4__this;

		public Action onButtonClicked;

		public Action onMouseHover;

		internal void _E000()
		{
			if (autoClose)
			{
				_003C_003E4__this.Close();
			}
			onButtonClicked?.Invoke();
		}

		internal void _E001()
		{
			onMouseHover?.Invoke();
		}
	}

	[CompilerGenerated]
	private sealed class _E002<_E077> where _E077 : Enum
	{
		public InteractionButtonsContainer _003C_003E4__this;

		public SimpleContextMenuButton button;

		public _EC4E<_E077> contextInteractions;

		public _E077 interaction;

		internal void _E000()
		{
			_003C_003E4__this._E164 = button;
			_003C_003E4__this._E004();
			if (contextInteractions.HasSubInteractions(interaction) && contextInteractions.IsInteractionAvailable(interaction))
			{
				contextInteractions.CreateSubInteractions(interaction, _003C_003E4__this);
			}
		}

		internal void _E001()
		{
			contextInteractions.ExecuteInteraction(interaction);
		}
	}

	[CompilerGenerated]
	private sealed class _E003<_E077> where _E077 : Enum
	{
		public SimpleContextMenuButton subMenuButton;

		public InteractionButtonsContainer _003C_003E4__this;

		internal void _E000()
		{
			if (subMenuButton != null)
			{
				subMenuButton.Blocked = false;
			}
			if (!(_003C_003E4__this._E163 == null))
			{
				_003C_003E4__this._E163.OnMouseExit -= _003C_003E4__this.OnPointerExit;
				_003C_003E4__this._E163.Close();
				UnityEngine.Object.Destroy(_003C_003E4__this._E163.gameObject);
				_003C_003E4__this._E163 = null;
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public SimpleContextMenuButton button;

		internal void _E000()
		{
			if (!(button == null) && !(button.gameObject == null))
			{
				button.Close();
				UnityEngine.Object.DestroyImmediate(button.GameObject);
			}
		}
	}

	private const string _E15F = "Characteristics/Icons/";

	[SerializeField]
	public SimpleContextMenu SubMenuTemplate;

	[SerializeField]
	public Transform SubMenuContainer;

	[SerializeField]
	private SimpleContextMenuButton _buttonTemplate;

	[SerializeField]
	private RectTransform _buttonsContainer;

	[SerializeField]
	private GameObject _background;

	[SerializeField]
	private SpecifiedContextButtons _specifiedButtons;

	private _EC76 _E160 = new _EC76();

	private _EC76 _E161 = new _EC76();

	private bool _E162;

	private SimpleContextMenu _E163;

	private SimpleContextMenuButton _E164;

	private bool _E165;

	public void OnDisable()
	{
		UI.Dispose();
	}

	private void Awake()
	{
		_E162 = _background != null && _background.activeSelf;
	}

	public void Show<T>(_EC4E<T> contextInteractions, IReadOnlyDictionary<T, string> names = null, List<T> prohibitedInteractions = null, Item item = null, IReadOnlyDictionary<T, Sprite> icons = null, bool autoClose = true) where T : Enum
	{
		_E000<T> CS_0024_003C_003E8__locals0 = new _E000<T>();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		CS_0024_003C_003E8__locals0.contextInteractions = contextInteractions;
		CS_0024_003C_003E8__locals0.prohibitedInteractions = prohibitedInteractions;
		CS_0024_003C_003E8__locals0.names = names;
		CS_0024_003C_003E8__locals0.icons = icons;
		CS_0024_003C_003E8__locals0.autoClose = autoClose;
		CS_0024_003C_003E8__locals0.item = item;
		UI.Dispose();
		if (SubMenuContainer == null)
		{
			SubMenuContainer = base.transform;
		}
		_E165 = CS_0024_003C_003E8__locals0.contextInteractions.HasIcons;
		CS_0024_003C_003E8__locals0.names = CS_0024_003C_003E8__locals0.names ?? CS_0024_003C_003E8__locals0.contextInteractions.CustomNames;
		ShowGameObject();
		CS_0024_003C_003E8__locals0._E000();
		_EC4E<T> contextInteractions2 = CS_0024_003C_003E8__locals0.contextInteractions;
		if (contextInteractions2 != null)
		{
			if (!(contextInteractions2 is _EC4A obj))
			{
				if (contextInteractions2 is _EC98 obj2)
				{
					_EC98 obj3 = obj2;
					UI.AddDisposable(obj3.SubscribeOnRedraw(delegate(IEnumerable<string> templateIds)
					{
						if (templateIds == null || templateIds.Contains(CS_0024_003C_003E8__locals0.item.TemplateId))
						{
							CS_0024_003C_003E8__locals0._E000();
						}
					}));
				}
			}
			else
			{
				_EC4A obj4 = obj;
				UI.AddDisposable(obj4.SubscribeOnRedraw(delegate(IEnumerable<string> templateIds)
				{
					if (templateIds == null || templateIds.Contains(CS_0024_003C_003E8__locals0.item.TemplateId))
					{
						CS_0024_003C_003E8__locals0._E000();
					}
				}));
				UI.AddDisposable(obj4.SubscribeOnInventoryLocked(delegate
				{
					CS_0024_003C_003E8__locals0._E000();
				}));
				if (CS_0024_003C_003E8__locals0.item is _EA72)
				{
					UI.AddDisposable(obj4.SubscribeOnEffectsChange(delegate(_E992 effect)
					{
						if (effect is _E9BF)
						{
							CS_0024_003C_003E8__locals0._E000();
						}
					}));
				}
			}
		}
		CS_0024_003C_003E8__locals0.contextInteractions.OnRedrawRequired += delegate
		{
			CS_0024_003C_003E8__locals0._003C_003E4__this._E160.Dispose();
			bool active2 = false;
			foreach (T availableInteraction in CS_0024_003C_003E8__locals0.contextInteractions.AvailableInteractions)
			{
				if ((CS_0024_003C_003E8__locals0.prohibitedInteractions == null || !CS_0024_003C_003E8__locals0.prohibitedInteractions.Contains(availableInteraction)) && CS_0024_003C_003E8__locals0.contextInteractions.IsActive(availableInteraction))
				{
					active2 = true;
					if (CS_0024_003C_003E8__locals0.names == null || !CS_0024_003C_003E8__locals0.names.TryGetValue(availableInteraction, out var value3))
					{
						value3 = availableInteraction.ToString();
					}
					if (CS_0024_003C_003E8__locals0.icons == null || !CS_0024_003C_003E8__locals0.icons.TryGetValue(availableInteraction, out var value4))
					{
						value4 = _E905.Pop<Sprite>(_ED3E._E000(246119) + availableInteraction);
					}
					CS_0024_003C_003E8__locals0._003C_003E4__this._E002(availableInteraction, CS_0024_003C_003E8__locals0.contextInteractions, value3, value4, CS_0024_003C_003E8__locals0.autoClose);
				}
			}
			foreach (_EC4D dynamicInteraction in CS_0024_003C_003E8__locals0.contextInteractions.DynamicInteractions)
			{
				CS_0024_003C_003E8__locals0._003C_003E4__this._E003(dynamicInteraction);
				active2 = true;
			}
			if (CS_0024_003C_003E8__locals0._003C_003E4__this._E162)
			{
				CS_0024_003C_003E8__locals0._003C_003E4__this._background.SetActive(active2);
			}
		};
		UI.AddDisposable(_E160);
		UI.AddDisposable(_E161);
		UI.AddDisposable(delegate
		{
			CS_0024_003C_003E8__locals0.contextInteractions.OnRedrawRequired -= delegate
			{
				CS_0024_003C_003E8__locals0._003C_003E4__this._E160.Dispose();
				bool active = false;
				foreach (T availableInteraction2 in CS_0024_003C_003E8__locals0.contextInteractions.AvailableInteractions)
				{
					if ((CS_0024_003C_003E8__locals0.prohibitedInteractions == null || !CS_0024_003C_003E8__locals0.prohibitedInteractions.Contains(availableInteraction2)) && CS_0024_003C_003E8__locals0.contextInteractions.IsActive(availableInteraction2))
					{
						active = true;
						if (CS_0024_003C_003E8__locals0.names == null || !CS_0024_003C_003E8__locals0.names.TryGetValue(availableInteraction2, out var value))
						{
							value = availableInteraction2.ToString();
						}
						if (CS_0024_003C_003E8__locals0.icons == null || !CS_0024_003C_003E8__locals0.icons.TryGetValue(availableInteraction2, out var value2))
						{
							value2 = _E905.Pop<Sprite>(_ED3E._E000(246119) + availableInteraction2);
						}
						CS_0024_003C_003E8__locals0._003C_003E4__this._E002(availableInteraction2, CS_0024_003C_003E8__locals0.contextInteractions, value, value2, CS_0024_003C_003E8__locals0.autoClose);
					}
				}
				foreach (_EC4D dynamicInteraction2 in CS_0024_003C_003E8__locals0.contextInteractions.DynamicInteractions)
				{
					CS_0024_003C_003E8__locals0._003C_003E4__this._E003(dynamicInteraction2);
					active = true;
				}
				if (CS_0024_003C_003E8__locals0._003C_003E4__this._E162)
				{
					CS_0024_003C_003E8__locals0._003C_003E4__this._background.SetActive(active);
				}
			};
		});
	}

	private SimpleContextMenuButton _E000<_E077>(_E077 key, string caption, [CanBeNull] Sprite sprite, [CanBeNull] Action onButtonClicked, [CanBeNull] Action onMouseHover = null, bool subMenu = false, bool autoClose = true) where _E077 : Enum
	{
		string key2 = key.ToString();
		(SimpleContextMenuButton, RectTransform)? tuple = _specifiedButtons?.GetButton(key);
		SimpleContextMenuButton template = tuple?.Item1 ?? _buttonTemplate;
		RectTransform container = tuple?.Item2 ?? _buttonsContainer;
		return _E001(key2, caption, template, container, sprite, onButtonClicked, onMouseHover, subMenu, autoClose);
	}

	private SimpleContextMenuButton _E001(string key, string caption, SimpleContextMenuButton template, RectTransform container, [CanBeNull] Sprite sprite, [CanBeNull] Action onButtonClicked, [CanBeNull] Action onMouseHover, bool subMenu = false, bool autoClose = true)
	{
		string imageText = key.ToUpper();
		SimpleContextMenuButton simpleContextMenuButton = UnityEngine.Object.Instantiate(template, container, worldPositionStays: false);
		simpleContextMenuButton.Show(caption, imageText, sprite, delegate
		{
			if (autoClose)
			{
				Close();
			}
			onButtonClicked?.Invoke();
		}, delegate
		{
			onMouseHover?.Invoke();
		}, subMenu, !_E165);
		return simpleContextMenuButton;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (_E163 == null)
		{
			return;
		}
		if (eventData.pointerCurrentRaycast.gameObject != null)
		{
			Transform transform = eventData.pointerCurrentRaycast.gameObject.transform;
			Transform transform2 = _E163.transform;
			if (transform.IsChildOf(transform2) || transform == transform2 || transform.IsChildOf(base.transform) || transform == base.transform)
			{
				return;
			}
		}
		_E004();
	}

	private void _E002<_E077>(_E077 interaction, _EC4E<_E077> contextInteractions, string caption, [CanBeNull] Sprite sprite, bool autoClose) where _E077 : Enum
	{
		SimpleContextMenuButton button = null;
		button = _E000(interaction, caption, sprite, delegate
		{
			contextInteractions.ExecuteInteraction(interaction);
		}, delegate
		{
			_E164 = button;
			_E004();
			if (contextInteractions.HasSubInteractions(interaction) && contextInteractions.IsInteractionAvailable(interaction))
			{
				contextInteractions.CreateSubInteractions(interaction, this);
			}
		}, contextInteractions.HasSubInteractions(interaction), autoClose);
		button.SetButtonInteraction(contextInteractions.IsInteractive(interaction));
		_E005(button);
	}

	private void _E003(_EC4D interaction)
	{
		SimpleContextMenuButton simpleContextMenuButton = _E001(interaction.Key, interaction.Key, _buttonTemplate, _buttonsContainer, interaction.Icon, interaction.Execute, _E004);
		simpleContextMenuButton.SetButtonInteraction((true, string.Empty));
		_E005(simpleContextMenuButton);
	}

	public void SetSubInteractions<T>(_EC4E<T> contextInteractions) where T : Enum
	{
		if (_E163 == null)
		{
			SimpleContextMenuButton subMenuButton = _E164;
			_E163 = UnityEngine.Object.Instantiate(SubMenuTemplate, SubMenuContainer, worldPositionStays: false);
			_E163.SubMenuTemplate = SubMenuTemplate;
			_E163.OnMouseExit += OnPointerExit;
			_E161.AddDisposable(delegate
			{
				if (subMenuButton != null)
				{
					subMenuButton.Blocked = false;
				}
				if (!(_E163 == null))
				{
					_E163.OnMouseExit -= OnPointerExit;
					_E163.Close();
					UnityEngine.Object.Destroy(_E163.gameObject);
					_E163 = null;
				}
			});
		}
		else
		{
			_E163.transform.SetAsLastSibling();
		}
		_E164.Blocked = true;
		RectTransform rectTransform = (RectTransform)_E164.transform;
		Vector2 size = rectTransform.rect.size;
		Vector2 vector = size - size * rectTransform.pivot;
		vector = rectTransform.TransformPoint(vector);
		_E163.Show(vector, contextInteractions);
		RectTransform rectTransform2 = (RectTransform)_E163.transform;
		if (!(rectTransform2.position.x - vector.x).IsZero())
		{
			float num = (size.x + rectTransform2.rect.size.x) * rectTransform2.lossyScale.x;
			vector.x -= num;
			vector.y = rectTransform2.position.y;
			rectTransform2.position = vector;
		}
	}

	private void _E004()
	{
		_E161.Dispose();
	}

	private void _E005(SimpleContextMenuButton button)
	{
		_E160.AddDisposable(delegate
		{
			if (!(button == null) && !(button.gameObject == null))
			{
				button.Close();
				UnityEngine.Object.DestroyImmediate(button.GameObject);
			}
		});
	}
}
