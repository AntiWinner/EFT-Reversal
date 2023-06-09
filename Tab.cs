using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tab : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	[CompilerGenerated]
	private Action<Tab, bool> m__E001;

	[SerializeField]
	protected GameObject _normalVersion;

	[SerializeField]
	protected GameObject _selectedVersion;

	[SerializeField]
	protected Image _targetImage;

	[SerializeField]
	protected Sprite _hoverSprite;

	[SerializeField]
	protected GameObject _hoverGraphic;

	[SerializeField]
	protected GameObject _idleGraphic;

	public bool Interactable = true;

	protected Sprite _normalSprite;

	private bool _E002;

	private bool _E003;

	protected _EC62 Controller;

	protected virtual bool CanHandlePointerClick
	{
		get
		{
			if (!_E002)
			{
				return Interactable;
			}
			return false;
		}
	}

	public event Action<Tab, bool> OnSelectionChanged
	{
		[CompilerGenerated]
		add
		{
			Action<Tab, bool> action = this.m__E001;
			Action<Tab, bool> action2;
			do
			{
				action2 = action;
				Action<Tab, bool> value2 = (Action<Tab, bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Tab, bool> action = this.m__E001;
			Action<Tab, bool> action2;
			do
			{
				action2 = action;
				Action<Tab, bool> value2 = (Action<Tab, bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		OnAwake();
	}

	protected virtual void OnAwake()
	{
		if (_targetImage != null)
		{
			_normalSprite = _targetImage.sprite;
		}
	}

	public void Init(_EC62 controller)
	{
		Controller = controller;
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		if (CanHandlePointerClick)
		{
			HandlePointerClick(_E002);
		}
	}

	public virtual void HandlePointerClick(bool isSelectedNow)
	{
		this.m__E001?.Invoke(this, !isSelectedNow);
	}

	public virtual void Select(bool sendCallback = true)
	{
		UpdateVisual(selected: true);
		if (sendCallback)
		{
			Controller?.Show();
		}
	}

	public virtual async Task<bool> Deselect()
	{
		bool flag = Controller != null;
		if (flag)
		{
			flag = !(await Controller.TryHide());
		}
		if (flag)
		{
			return false;
		}
		UpdateVisual(selected: false);
		return true;
	}

	protected virtual void UpdateVisual(bool selected, bool uiOnly = false)
	{
		if (!uiOnly)
		{
			_E002 = selected;
		}
		_E003 = selected;
		_E000();
	}

	internal virtual void _E001(bool active)
	{
		if (_targetImage != null)
		{
			_targetImage.ChangeImageAlpha(active ? 1f : 0.15f);
		}
		Interactable = active;
	}

	private void _E000()
	{
		_normalVersion.gameObject.SetActive(!_E002 && !_E003);
		_selectedVersion.gameObject.SetActive(_E002 || _E003);
	}

	public virtual void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		Hover(isHovered: true);
	}

	public virtual void OnPointerExit([NotNull] PointerEventData eventData)
	{
		Hover(isHovered: false);
	}

	public virtual void Hover(bool isHovered)
	{
		if (Interactable)
		{
			if (_targetImage != null)
			{
				_targetImage.sprite = (isHovered ? _hoverSprite : _normalSprite);
			}
			if (_hoverGraphic != null)
			{
				_hoverGraphic.SetActive(isHovered);
			}
			if (_idleGraphic != null)
			{
				_idleGraphic.SetActive(!isHovered);
			}
		}
	}
}
