using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DG.Tweening;
using EFT.UI.DragAndDrop;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class ItemViewAnimation : SerializedMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private const float m__E000 = 1f;

	[SerializeField]
	private Transform _itemView;

	[SerializeField]
	private Image _mainImage;

	[SerializeField]
	private Image _colorPanel;

	[SerializeField]
	private GameObject _notExaminedPanel;

	[SerializeField]
	private ItemViewExamineComponent _examineComponentTemplate;

	[SerializeField]
	private ItemViewLoadAmmoComponent _loadAmmoComponentTemplate;

	private bool m__E001;

	private bool m__E002;

	private bool m__E003;

	private bool m__E004;

	private bool m__E005;

	private ItemViewExamineComponent m__E006;

	private ItemViewLoadAmmoComponent m__E007;

	private Color m__E008;

	public void Init(Color backgroundColor)
	{
		this.m__E008 = backgroundColor;
	}

	public void SetSearchedState(bool searched)
	{
		this.m__E001 = searched;
		_E005();
	}

	public void SetBlinkingState(bool blinking)
	{
		if (blinking)
		{
			_E002();
		}
		else
		{
			_E003();
		}
	}

	public void SetDragState(bool beingDragged)
	{
		this.m__E003 = beingDragged;
		_E006();
		_E005();
	}

	public void SetExaminedState(bool examined)
	{
		this.m__E002 = examined;
		_E006();
		_E005();
	}

	public void StartExamination(float duration)
	{
		_E008(examining: true);
		if (this.m__E006 != null)
		{
			this.m__E006.Show(duration);
		}
	}

	public void StopExamination()
	{
		_E008(examining: false);
	}

	private void _E000()
	{
		this.m__E005 = true;
		_E009().HandleExceptions();
	}

	private void _E001()
	{
		this.m__E005 = false;
		_colorPanel.DOKill();
	}

	private void _E002()
	{
		this.m__E004 = true;
		_E00A().HandleExceptions();
	}

	private void _E003()
	{
		this.m__E004 = false;
		_mainImage.DOKill();
		_E005();
	}

	public void StartSearch()
	{
		_mainImage.color = _mainImage.color.SetAlpha(0.85f);
	}

	private void _E004()
	{
		_E005();
	}

	public void StartLoading(float oneAmmoDuration, int ammoTotal, int startCount = 0)
	{
		_E007(loading: true);
		_E002();
		_E000();
		if (this.m__E007 != null)
		{
			this.m__E007.Show(oneAmmoDuration, ammoTotal, startCount);
		}
	}

	public void StopLoading()
	{
		_E007(loading: false);
		_E003();
		_E001();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.m__E007 != null)
		{
			this.m__E007.SetStopButtonStatus(active: true);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.m__E007 != null)
		{
			this.m__E007.SetStopButtonStatus(active: false);
		}
	}

	public void Stop()
	{
		StopLoading();
		StopExamination();
		_E003();
		_E001();
		_E004();
		this.m__E001 = false;
		this.m__E002 = false;
		this.m__E003 = false;
		this.m__E004 = false;
		this.m__E005 = false;
	}

	private void _E005()
	{
		if (!this.m__E004)
		{
			if (this.m__E003 || this.m__E001)
			{
				_mainImage.color = _mainImage.color.SetAlpha(0.85f);
			}
			else if (!this.m__E002)
			{
				_mainImage.color = _mainImage.color.SetAlpha(0.7f);
			}
			else
			{
				_mainImage.color = _mainImage.color.SetAlpha(1f);
			}
		}
	}

	private void _E006()
	{
		if (_notExaminedPanel != null)
		{
			_notExaminedPanel.gameObject.SetActive(!this.m__E003 && !this.m__E002);
		}
	}

	private void _E007(bool loading)
	{
		if (_loadAmmoComponentTemplate == null)
		{
			return;
		}
		if (!loading)
		{
			if (this.m__E007 != null)
			{
				this.m__E007.Destroy();
				this.m__E007 = null;
			}
		}
		else if (this.m__E007 == null)
		{
			this.m__E007 = Object.Instantiate(_loadAmmoComponentTemplate, _itemView);
		}
	}

	private void _E008(bool examining)
	{
		if (_examineComponentTemplate == null)
		{
			return;
		}
		if (!examining)
		{
			if (this.m__E006 != null)
			{
				this.m__E006.Destroy();
				this.m__E006 = null;
			}
		}
		else if (this.m__E006 == null)
		{
			this.m__E006 = Object.Instantiate(_examineComponentTemplate, _itemView);
		}
	}

	[CompilerGenerated]
	private async Task _E009()
	{
		while (this.m__E005)
		{
			await _colorPanel.DOColor(this.m__E008.SetAlpha(0.6f), 0.5f);
			if (!this.m__E005)
			{
				break;
			}
			await _colorPanel.DOColor(this.m__E008, 0.5f);
		}
		_colorPanel.color = this.m__E008;
	}

	[CompilerGenerated]
	private async Task _E00A()
	{
		while (this.m__E004)
		{
			await _mainImage.DOColor(_mainImage.color.SetAlpha(0f), 0.5f);
			if (!this.m__E004)
			{
				break;
			}
			await _mainImage.DOColor(_mainImage.color.SetAlpha(1f), 0.5f);
		}
		_E005();
	}
}
