using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public class SimpleContextMenu : UIElement, IPointerExitHandler, IEventSystemHandler
{
	[CompilerGenerated]
	private sealed class _E000<_E077> where _E077 : Enum
	{
		public Item item;

		public SimpleContextMenu _003C_003E4__this;

		internal void _E000(IEnumerable<string> itemIds)
		{
			if (itemIds == null || itemIds.Contains(item.Id))
			{
				_003C_003E4__this.Close();
			}
		}
	}

	[SerializeField]
	private InteractionButtonsContainer _interactionButtonsContainer;

	[CompilerGenerated]
	private Action<PointerEventData> _E181;

	[NonSerialized]
	public readonly _ECEC OnMenuClosed = new _ECEC();

	public SimpleContextMenu SubMenuTemplate
	{
		set
		{
			_interactionButtonsContainer.SubMenuTemplate = value;
		}
	}

	public event Action<PointerEventData> OnMouseExit
	{
		[CompilerGenerated]
		add
		{
			Action<PointerEventData> action = _E181;
			Action<PointerEventData> action2;
			do
			{
				action2 = action;
				Action<PointerEventData> value2 = (Action<PointerEventData>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E181, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<PointerEventData> action = _E181;
			Action<PointerEventData> action2;
			do
			{
				action2 = action;
				Action<PointerEventData> value2 = (Action<PointerEventData>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E181, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public virtual void Show<T>(Vector2 position, _EC4E<T> contextInteractions, IReadOnlyDictionary<T, string> names = null, Item item = null) where T : Enum
	{
		_E000(position, contextInteractions, names, null, item);
	}

	public virtual void Show<T>(Vector2 position, _EC4E<T> contextInteractions, IReadOnlyDictionary<T, string> names, IReadOnlyDictionary<T, Sprite> icons) where T : Enum
	{
		_E000(position, contextInteractions, names, icons, null);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_E181?.Invoke(eventData);
	}

	private void _E000<_E077>(Vector2 position, _EC4E<_E077> contextInteractions, IReadOnlyDictionary<_E077, string> names, IReadOnlyDictionary<_E077, Sprite> icons, Item item) where _E077 : Enum
	{
		if (contextInteractions.AvailableInteractions == null)
		{
			return;
		}
		UI.Dispose();
		UI.AddDisposable(_interactionButtonsContainer);
		ShowGameObject();
		_interactionButtonsContainer.Show(contextInteractions, names, null, item, icons);
		if (contextInteractions != null)
		{
			if (!(contextInteractions is _EC4A obj))
			{
				if (contextInteractions is _EC98 obj2)
				{
					_EC98 obj3 = obj2;
					UI.AddDisposable(obj3.SubscribeOnClose(delegate(IEnumerable<string> itemIds)
					{
						if (itemIds == null || itemIds.Contains(item.Id))
						{
							Close();
						}
					}));
				}
			}
			else
			{
				_EC4A obj4 = obj;
				UI.AddDisposable(obj4.SubscribeOnClose(delegate(IEnumerable<string> itemIds)
				{
					if (itemIds == null || itemIds.Contains(item.Id))
					{
						Close();
					}
				}));
			}
		}
		base.transform.position = position;
		CorrectPosition();
	}

	private void Update()
	{
		if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
		{
			Close();
		}
	}

	public override void Close()
	{
		base.Close();
		OnMenuClosed.Invoke();
	}

	protected override void CorrectPosition(_E3F3 margins = default(_E3F3))
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)base.transform);
		base.CorrectPosition(margins);
	}
}
