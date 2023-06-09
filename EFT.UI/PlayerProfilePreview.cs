using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using EFT.Utilities;
using UnityEngine;

namespace EFT.UI;

[Serializable]
public class PlayerProfilePreview : UIElement
{
	public enum ECameraViewType
	{
		FullBody,
		Head
	}

	[SerializeField]
	private PlayerModelView _playerModelView;

	[SerializeField]
	private RectTransform _transform;

	[SerializeField]
	private XCoordRotation _rotator;

	[SerializeField]
	private DragTrigger _dragTrigger;

	[SerializeField]
	private SideSelectionToggle _hoverTrigger;

	[SerializeField]
	private EPlayerSide _side;

	[SerializeField]
	private Transform _cameraContainer;

	[SerializeField]
	private Camera _camera;

	[SerializeField]
	private CanvasGroup _godLight;

	[SerializeField]
	private List<GameObject> _lights;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private Dictionary<ECameraViewType, Transform> _cameraPositions = new Dictionary<ECameraViewType, Transform>();

	[SerializeField]
	private Dictionary<ECameraViewType, RectTransform> _viewportPositions = new Dictionary<ECameraViewType, RectTransform>();

	private bool _selected;

	public PlayerModelView PlayerModelView => _playerModelView;

	public XCoordRotation Rotator => _rotator;

	public DragTrigger DragTrigger => _dragTrigger;

	public SideSelectionToggle HoverTrigger => _hoverTrigger;

	public EPlayerSide Side => _side;

	public Profile Profile { get; private set; }

	public bool RotationEnabled
	{
		get
		{
			return _dragTrigger.isActiveAndEnabled;
		}
		set
		{
			if (_dragTrigger.isActiveAndEnabled != value)
			{
				_dragTrigger.enabled = value;
				_dragTrigger.gameObject.SetActive(value);
				if (value && Selectable)
				{
					_hoverTrigger.isOn = false;
					_hoverTrigger.gameObject.SetActive(value: false);
				}
			}
		}
	}

	public bool Selectable
	{
		get
		{
			return _hoverTrigger.isActiveAndEnabled;
		}
		set
		{
			if (_hoverTrigger.isOn != value || _hoverTrigger.isActiveAndEnabled != value)
			{
				_hoverTrigger.gameObject.SetActive(value);
				if (!value)
				{
					SetGodLightStatus(active: false);
				}
				if (value && RotationEnabled)
				{
					_dragTrigger.enabled = false;
					_dragTrigger.gameObject.SetActive(value: false);
				}
			}
		}
	}

	public Task Show(Profile profile, float update = 0f)
	{
		UI.Dispose();
		ShowGameObject();
		Profile = profile;
		Rotator.Init(_cameraContainer);
		UI.AddDisposable(HoverTrigger.OnHover.Subscribe(_E000));
		return PlayerModelView.Show(profile.GetVisualEquipmentState(clone: false), null, delegate
		{
		}, update, Vector3.zero, animateWeapon: false);
	}

	public async Task ChangeCameraPosition(ECameraViewType viewType, float duration)
	{
		_cameraContainer.DOKill();
		_camera.transform.DOKill();
		_transform.DOKill();
		Transform transform = _cameraPositions[viewType];
		RectTransform rectTransform = _viewportPositions[viewType];
		if (duration > 0f)
		{
			_transform.DOSizeDelta(rectTransform.sizeDelta, duration);
			_transform.DOAnchorPos(rectTransform.anchoredPosition, duration);
			_transform.DOAnchorMin(rectTransform.anchorMin, duration);
			_transform.DOAnchorMax(rectTransform.anchorMax, duration);
			_camera.transform.DOLocalMove(transform.localPosition, duration);
			_camera.transform.DOLocalRotate(transform.localRotation.eulerAngles, duration);
			await _cameraContainer.DOLocalRotate(Vector3.zero, duration);
		}
		else
		{
			_transform.sizeDelta = rectTransform.sizeDelta;
			_transform.anchoredPosition = rectTransform.anchoredPosition;
			_transform.anchorMin = rectTransform.anchorMin;
			_transform.anchorMax = rectTransform.anchorMax;
			_camera.transform.localPosition = transform.localPosition;
			_camera.transform.localRotation = transform.localRotation;
			_cameraContainer.localRotation = Quaternion.identity;
		}
		Rotator.SetRotation(0f);
	}

	public void SetVisibility(bool visible, float duration)
	{
		_canvasGroup.DOKill();
		if (duration.Positive())
		{
			_canvasGroup.DOFade(visible ? 1 : 0, duration);
		}
		else
		{
			_canvasGroup.alpha = (visible ? 1 : 0);
		}
	}

	public void SetLightStatus(bool isOn)
	{
		_selected = isOn;
		SetGodLightStatus(isOn);
		foreach (GameObject light in _lights)
		{
			light.SetActive(isOn);
		}
	}

	public void SetGodLightStatus(bool active, float duration = 0.5f)
	{
		_godLight.DOKill();
		if (duration.Positive())
		{
			_godLight.DOFade(active ? 1 : 0, duration);
		}
		else
		{
			_godLight.alpha = (active ? 1 : 0);
		}
	}

	private void _E000(bool isUnderCursor)
	{
		if (Selectable && !_selected)
		{
			SetGodLightStatus(isUnderCursor);
		}
	}

	public override void Close()
	{
		base.Close();
		_playerModelView.Close();
	}
}
