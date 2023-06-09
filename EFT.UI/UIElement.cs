using System;
using System.Threading;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EFT.UI;

public class UIElement : SerializedMonoBehaviour, IShowable, IUIView, IDisposable
{
	protected readonly _EC76 UI = new _EC76();

	private RectTransform _E216;

	[CanBeNull]
	public GameObject GameObject
	{
		get
		{
			if (!(this != null) || !(base.gameObject != null))
			{
				return null;
			}
			return base.gameObject;
		}
	}

	protected RectTransform RectTransform => _E216 ?? (_E216 = (RectTransform)base.transform);

	protected CancellationToken CancellationToken => UI.CancellationToken;

	public Transform Transform => base.transform;

	protected virtual void CorrectPosition(_E3F3 margins = default(_E3F3))
	{
		if (this != null && base.transform != null)
		{
			RectTransform.CorrectPositionResolution(margins);
		}
	}

	public void Dispose()
	{
		Close();
	}

	public virtual void Display()
	{
		ShowGameObject();
	}

	public virtual void Close()
	{
		UI.Dispose();
		HideGameObject();
	}

	public void ShowGameObject()
	{
		base.gameObject.SetActive(value: true);
	}

	public void HideGameObject()
	{
		base.gameObject.SetActive(value: false);
	}
}
