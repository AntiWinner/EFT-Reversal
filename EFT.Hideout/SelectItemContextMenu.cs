using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EFT.InventoryLogic;
using EFT.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.Hideout;

public sealed class SelectItemContextMenu : UIElement, IPointerClickHandler, IEventSystemHandler
{
	private struct _E000
	{
		public int CornerIndex;

		public GridLayoutGroup.Corner StartCorner;

		public Vector2 DirectionAnchor => StartCorner switch
		{
			GridLayoutGroup.Corner.LowerLeft => Vector2.zero, 
			GridLayoutGroup.Corner.UpperRight => Vector2.one, 
			GridLayoutGroup.Corner.LowerRight => new Vector2(1f, 0f), 
			GridLayoutGroup.Corner.UpperLeft => new Vector2(0f, 1f), 
			_ => throw new InvalidEnumArgumentException(), 
		};

		public _E000(int cornerIndex, GridLayoutGroup.Corner startCorner)
		{
			CornerIndex = cornerIndex;
			StartCorner = startCorner;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public SelectItemContextMenu _003C_003E4__this;

		public Func<Item, string> getDetails;

		internal void _E000(Item item, SelectingItemView view)
		{
			view.SetItem(item, _003C_003E4__this._E000);
			view.SetDetails(getDetails(item));
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public SelectingItemView emptyItem;

		internal void _E000()
		{
			UnityEngine.Object.Destroy(emptyItem.gameObject);
		}
	}

	[SerializeField]
	private RectTransform _container;

	private TaskCompletionSource<(Item, bool)> _E066;

	private GridLayoutGroup _E067;

	private readonly Vector3[] _E068 = new Vector3[4];

	private static readonly Dictionary<EContextPriorDirection, _E000> _E069 = new Dictionary<EContextPriorDirection, _E000>
	{
		{
			EContextPriorDirection.BottomLeftToRight,
			new _E000(0, GridLayoutGroup.Corner.UpperLeft)
		},
		{
			EContextPriorDirection.BottomRightToLeft,
			new _E000(3, GridLayoutGroup.Corner.UpperRight)
		},
		{
			EContextPriorDirection.LeftDownUp,
			new _E000(0, GridLayoutGroup.Corner.LowerRight)
		},
		{
			EContextPriorDirection.LeftUpDown,
			new _E000(1, GridLayoutGroup.Corner.UpperRight)
		},
		{
			EContextPriorDirection.RightDownUp,
			new _E000(3, GridLayoutGroup.Corner.LowerLeft)
		},
		{
			EContextPriorDirection.RightUpDown,
			new _E000(2, GridLayoutGroup.Corner.UpperLeft)
		},
		{
			EContextPriorDirection.UpperLeftToRight,
			new _E000(1, GridLayoutGroup.Corner.LowerLeft)
		},
		{
			EContextPriorDirection.UpperRightToLeft,
			new _E000(2, GridLayoutGroup.Corner.LowerRight)
		}
	};

	private void Awake()
	{
		_E067 = _container.GetComponent<GridLayoutGroup>();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Close();
	}

	public Task<(Item, bool)> GetItem(IEnumerable<Item> availableItems, Func<Item, string> getDetails, bool showEmptyCell, SelectingItemView itemTemplate, RectTransform parentPosition, Vector2 offset, EContextPriorDirection direction)
	{
		ShowGameObject();
		_E067.startCorner = _E069[direction].StartCorner;
		_E001(parentPosition, offset, direction);
		_E066 = new TaskCompletionSource<(Item, bool)>();
		UI.AddViewList(availableItems, itemTemplate, _container, delegate(Item item, SelectingItemView view)
		{
			view.SetItem(item, _E000);
			view.SetDetails(getDetails(item));
		});
		if (showEmptyCell)
		{
			SelectingItemView emptyItem = UI.AddDisposable(UnityEngine.Object.Instantiate(itemTemplate, _container));
			emptyItem.SetItem(null, _E000);
			UI.AddDisposable(delegate
			{
				UnityEngine.Object.Destroy(emptyItem.gameObject);
			});
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(_container);
		_container.CorrectPositionResolution();
		return _E066.Task;
	}

	private void _E000(Item selectedItem)
	{
		_E066.SetResult((selectedItem, true));
		Close();
	}

	private void _E001(RectTransform parentPosition, Vector2 offset, EContextPriorDirection direction)
	{
		_E000 obj = _E069[direction];
		parentPosition.GetWorldCorners(_E068);
		Vector3 position = _E068[obj.CornerIndex];
		Vector2 directionAnchor = obj.DirectionAnchor;
		position.x += offset.x;
		position.y += offset.y;
		Transform parent = base.RectTransform.parent;
		if (parent != null)
		{
			parent.SetAsLastSibling();
		}
		_container.anchorMin = directionAnchor;
		_container.anchorMax = directionAnchor;
		_container.pivot = directionAnchor;
		_container.position = position;
	}

	public override void Close()
	{
		base.Close();
		if (_E066 != null)
		{
			Task<(Item, bool)> task = _E066.Task;
			if (task == null || !task.IsCompleted)
			{
				_E066.SetResult((null, false));
			}
		}
	}
}
