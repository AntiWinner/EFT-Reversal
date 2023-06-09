using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Graphic))]
public sealed class UIDragComponent : UIBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler
{
	[SerializeField]
	private RectTransform _target;

	[SerializeField]
	private bool _putOnTop;

	private Vector2 m__E000;

	public void Init(RectTransform target, bool putOnTop)
	{
		_target = target;
		_putOnTop = putOnTop;
		_E000();
	}

	protected override void Awake()
	{
		base.Awake();
		_E000();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		_EC74.OnResolutionChanged += _E000;
		_E000();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		_EC74.OnResolutionChanged -= _E000;
	}

	private void _E000()
	{
		if (_E001())
		{
			_target.CorrectPositionResolution();
		}
	}

	private bool _E001(bool showError = false)
	{
		if (_target != null)
		{
			return true;
		}
		if (showError)
		{
			Debug.LogError(_ED3E._E000(255404) + base.gameObject.name);
		}
		return false;
	}

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		if (_E001())
		{
			_target.position = eventData.position - this.m__E000;
			_target.CorrectPositionResolution();
		}
	}

	void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
	{
		if (_E001(showError: true))
		{
			this.m__E000 = eventData.position - (Vector2)_target.position;
			if (_putOnTop)
			{
				_target.SetAsLastSibling();
			}
		}
	}
}
