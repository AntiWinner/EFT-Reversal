using System;
using EFT.InputSystem;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI.Gestures;

public abstract class GestureBaseItem : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public struct _E000
	{
		public int ItemIndex;

		public Vector2 Position;

		public PointerEventData.InputButton Button;
	}

	[NonSerialized]
	public readonly _ECED<_E000> OnPointerClicked = new _ECED<_E000>();

	private bool _E2F9;

	public bool IsUnderPointer
	{
		get
		{
			return _E2F9;
		}
		set
		{
			if (_E2F9 != value)
			{
				_E2F9 = value;
				UnderPointerChanged(_E2F9);
			}
		}
	}

	public abstract int ItemIndex { get; }

	private void OnDisable()
	{
		IsUnderPointer = false;
	}

	protected void ShowInternal(ECommand command, int index, string caption)
	{
		UI.Dispose();
		UI.AddDisposable(GesturesMenu.OnBindUpdated.Bind(BindUpdatedHandler, new GesturesMenu._E000(command, index, caption)));
		ShowGameObject();
	}

	protected abstract void BindUpdatedHandler(GesturesMenu._E000 bind);

	protected abstract void UnderPointerChanged(bool isUnderPointer);

	void IPointerEnterHandler.OnPointerEnter([NotNull] PointerEventData eventData)
	{
		IsUnderPointer = true;
	}

	void IPointerExitHandler.OnPointerExit([NotNull] PointerEventData eventData)
	{
		IsUnderPointer = false;
	}

	void IPointerClickHandler.OnPointerClick([NotNull] PointerEventData eventData)
	{
		OnPointerClicked.Invoke(new _E000
		{
			ItemIndex = ItemIndex,
			Position = eventData.position,
			Button = eventData.button
		});
	}
}
