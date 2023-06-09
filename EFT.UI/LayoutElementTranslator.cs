using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

[ExecuteAlways]
[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public sealed class LayoutElementTranslator : UIBehaviour, ILayoutElement
{
	public Component Source;

	private Object m__E000;

	private ILayoutElement _E001;

	private ILayoutElement _E002
	{
		get
		{
			if (_E001 != null && (object)Source == this.m__E000)
			{
				return _E001;
			}
			this.m__E000 = Source;
			_E001 = this.m__E000 as ILayoutElement;
			return _E001;
		}
	}

	private bool _E003 => _E002 != null;

	private bool _E004 => _E002 == null;

	public float minWidth
	{
		get
		{
			if (!_E003)
			{
				return 0f;
			}
			return _E002.minWidth;
		}
	}

	public float preferredWidth
	{
		get
		{
			if (!_E003)
			{
				return 0f;
			}
			return _E002.preferredWidth;
		}
	}

	public float flexibleWidth
	{
		get
		{
			if (!_E003)
			{
				return 0f;
			}
			return _E002.flexibleWidth;
		}
	}

	public float minHeight
	{
		get
		{
			if (!_E003)
			{
				return 0f;
			}
			return _E002.minHeight;
		}
	}

	public float preferredHeight
	{
		get
		{
			if (!_E003)
			{
				return 0f;
			}
			return _E002.preferredHeight;
		}
	}

	public float flexibleHeight
	{
		get
		{
			if (!_E003)
			{
				return 0f;
			}
			return _E002.flexibleHeight;
		}
	}

	public int layoutPriority
	{
		get
		{
			if (!_E003)
			{
				return 0;
			}
			return _E002.layoutPriority;
		}
	}

	public void CalculateLayoutInputHorizontal()
	{
		if (_E003)
		{
			_E002.CalculateLayoutInputHorizontal();
		}
	}

	public void CalculateLayoutInputVertical()
	{
		if (_E003)
		{
			_E002.CalculateLayoutInputVertical();
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		_E000();
	}

	protected override void OnTransformParentChanged()
	{
		_E000();
	}

	protected override void OnDisable()
	{
		_E000();
		base.OnDisable();
	}

	protected override void OnDidApplyAnimationProperties()
	{
		_E000();
	}

	protected override void OnBeforeTransformParentChanged()
	{
		_E000();
	}

	private void _E000()
	{
		if (IsActive())
		{
			LayoutRebuilder.MarkLayoutForRebuild(base.transform as RectTransform);
		}
	}
}
