using System;
using System.Collections.Generic;
using UnityEngine;

namespace EFT.UI;

public class UISpawner<T> : UIElement where T : MonoBehaviour
{
	[SerializeField]
	protected T _object;

	[SerializeField]
	protected string _headerCaption;

	[SerializeField]
	protected int _headerFontSize;

	[SerializeField]
	protected List<Transform> _preservedChildren;

	[SerializeField]
	protected float _minWidth = -1f;

	[SerializeField]
	protected bool _useEllipsis;

	private Action _E217;

	private T _E218;

	public T SpawnedObject
	{
		get
		{
			if ((UnityEngine.Object)_E218 != (UnityEngine.Object)null)
			{
				return _E218;
			}
			SpawnObject();
			_E02E(_headerCaption, _headerFontSize);
			if (_minWidth >= 0f)
			{
				SetMinWidth(_minWidth);
			}
			SetEllipsis(_useEllipsis);
			return _E218;
		}
	}

	protected virtual T SpawnObject()
	{
		_E001();
		if ((UnityEngine.Object)_E218 == (UnityEngine.Object)null)
		{
			UI.AddDisposable(_E7AD._E010.AddLocaleUpdateListener(_E000));
		}
		if (Application.isPlaying)
		{
			_E218 = UnityEngine.Object.Instantiate(_object);
		}
		_E218.name = base.gameObject.name.Replace(_ED3E._E000(255507), string.Empty);
		((RectTransform)SpawnedObject.transform).SetRectTransformParent((RectTransform)base.transform);
		return SpawnedObject;
	}

	internal virtual void _E02E(string caption, int size)
	{
	}

	protected virtual void SetMinWidth(float minWidth)
	{
	}

	protected virtual void SetEllipsis(bool useEllipsis)
	{
	}

	private void _E000()
	{
		if ((UnityEngine.Object)_E218 != (UnityEngine.Object)null && base.gameObject != null)
		{
			_E02E(_headerCaption, _headerFontSize);
		}
	}

	private void _E001()
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		foreach (Transform item in base.transform)
		{
			if (!(item == null) && (_preservedChildren == null || _preservedChildren.IndexOf(item) < 0))
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(item.gameObject);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(item.gameObject, allowDestroyingAssets: true);
				}
			}
		}
	}

	private void OnDestroy()
	{
		Dispose();
	}
}
