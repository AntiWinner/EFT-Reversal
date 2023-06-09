using System.Runtime.CompilerServices;
using Cinemachine;
using Comfort.Common;
using EFT.UI;
using UnityEngine;

namespace EFT.Hideout;

public sealed class HideoutCameraController : UIElement
{
	public const float TRANSITION_TIME = 0.8f;

	private const int _E057 = 68;

	private const int _E058 = 5;

	private const int _E059 = 15;

	[SerializeField]
	private HideoutCamera[] _camerasWithPath;

	[SerializeField]
	private CinemachineVirtualCamera _playerCamera;

	private int _E05A;

	private bool _E044;

	private bool _E05B;

	private bool _E05C;

	private bool _E05D = true;

	private TriggerColliderSearcher _E05E;

	private HideoutCamera _E000 => _camerasWithPath[_E05A];

	public bool NightVisionState
	{
		get
		{
			return _E05B;
		}
		set
		{
			_E05B = value;
			_E000();
		}
	}

	public bool AreaSelected
	{
		get
		{
			return _E05C;
		}
		set
		{
			_E05C = value;
			_E000();
		}
	}

	public bool SurroundingIllumination
	{
		get
		{
			return _E05D;
		}
		set
		{
			_E05D = value;
			_E000();
		}
	}

	public bool CameraIsInThirdPerson => (double)(this._E000.Transform.position - _E8A8.Instance.Camera.transform.position).magnitude < 0.02;

	private void Awake()
	{
		_E8A8.Instance.OnFovChanged += delegate(float fov)
		{
			if (!(_playerCamera == null))
			{
				LensSettings lens2 = _playerCamera.m_Lens;
				lens2.FieldOfView = fov;
				_playerCamera.m_Lens = lens2;
			}
		};
		UI.AddDisposable(delegate
		{
			_E8A8.Instance.OnFovChanged -= delegate(float fov)
			{
				if (!(_playerCamera == null))
				{
					LensSettings lens = _playerCamera.m_Lens;
					lens.FieldOfView = fov;
					_playerCamera.m_Lens = lens;
				}
			};
		});
	}

	public void ResetPlayerPosition(Player player)
	{
		Vector3 position = _E8A8.Instance.Camera.transform.position;
		Quaternion rotation = player.CameraContainer.transform.rotation;
		_playerCamera.transform.SetPositionAndRotation(position, rotation);
		Show();
	}

	public void MoveCamera(EMovementDirection direction)
	{
		HideoutCamera[] camerasWithPath = _camerasWithPath;
		for (int i = 0; i < camerasWithPath.Length; i++)
		{
			camerasWithPath[i].MoveAside(direction);
		}
	}

	public void SwitchCamera()
	{
		int num = _E05A + 1;
		if (num >= _camerasWithPath.Length)
		{
			num = 0;
		}
		_E001(num);
	}

	private void Update()
	{
		if (_E044)
		{
			HideoutCamera[] camerasWithPath = _camerasWithPath;
			for (int i = 0; i < camerasWithPath.Length; i++)
			{
				camerasWithPath[i].CameraUpdate();
			}
		}
		UpdateFakePlayerCollider(Time.deltaTime);
	}

	private void _E000()
	{
		HideoutCameraFlashlight flashlight = _E8A8.Instance.Flashlight;
		if (!(flashlight == null))
		{
			flashlight.SetState(!_E05D && !NightVisionState && _E05C);
		}
	}

	public void Show()
	{
		HideoutCamera[] camerasWithPath = _camerasWithPath;
		for (int i = 0; i < camerasWithPath.Length; i++)
		{
			camerasWithPath[i].Show();
		}
		_E001(0);
		SetActiveState(state: true);
		SetAnimationTime(68);
	}

	public void SetActiveState(bool state)
	{
		_playerCamera.Priority = (state ? 5 : 15);
		_E044 = state;
	}

	public void SetAnimationTime(int percent)
	{
		HideoutCamera[] camerasWithPath = _camerasWithPath;
		for (int i = 0; i < camerasWithPath.Length; i++)
		{
			camerasWithPath[i].SetAnimationTimePercent(percent);
		}
	}

	public void Zoom(float? value)
	{
		HideoutCamera[] camerasWithPath = _camerasWithPath;
		for (int i = 0; i < camerasWithPath.Length; i++)
		{
			camerasWithPath[i].Zoom(value);
		}
	}

	private void _E001(int index)
	{
		this._E000.SetCameraActive(value: false);
		HideoutCamera.NegativeDirection = index > 0;
		_E05A = index;
		this._E000.SetCameraActive(value: true);
	}

	public void CreateFakePlayerCollider(HideoutPlayer player)
	{
		GameObject gameObject = new GameObject(_ED3E._E000(164236));
		gameObject.transform.SetParent(_E8A8.Instance.Camera.gameObject.transform, worldPositionStays: false);
		gameObject.layer = _E37B.PlayerLayer;
		BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
		boxCollider.size = Vector3.one;
		boxCollider.isTrigger = true;
		boxCollider.enabled = false;
		_E05E = gameObject.AddComponent<TriggerColliderSearcher>();
		_E05E.Init(boxCollider, EFTHardSettings.Instance.TriggersCastLayerMask);
		_E05E.OverrideTransformPosition = gameObject.transform;
		_E05E.ConnectToCharacterController(player._characterController);
		_E05E.IsEnabled = true;
		Singleton<GameWorld>.Instance.RegisterPlayerCollider(player, boxCollider);
	}

	public void UpdateFakePlayerCollider(float deltaTime)
	{
		_E05E?.ManualUpdate(deltaTime);
	}

	public void DestroyFakePlayerCollider()
	{
		Object.Destroy(_E05E.gameObject);
		_E05E = null;
	}

	[CompilerGenerated]
	private void _E002(float fov)
	{
		if (!(_playerCamera == null))
		{
			LensSettings lens = _playerCamera.m_Lens;
			lens.FieldOfView = fov;
			_playerCamera.m_Lens = lens;
		}
	}

	[CompilerGenerated]
	private void _E003()
	{
		_E8A8.Instance.OnFovChanged -= delegate(float fov)
		{
			if (!(_playerCamera == null))
			{
				LensSettings lens = _playerCamera.m_Lens;
				lens.FieldOfView = fov;
				_playerCamera.m_Lens = lens;
			}
		};
	}
}
