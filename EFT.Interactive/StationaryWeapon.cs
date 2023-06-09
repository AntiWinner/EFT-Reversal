using System.Collections;
using System.Runtime.CompilerServices;
using EFT.Animations;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.Serialization;

namespace EFT.Interactive;

public class StationaryWeapon : InteractableObject, _E354, _EC07
{
	public enum EStationaryAnimationType
	{
		UtesSit,
		UtesStand,
		AGS_17
	}

	[SerializeField]
	private string _id;

	[SerializeField]
	private GameObject[] _gameObjectsWithRenderers;

	[SerializeField]
	[FormerlySerializedAs("BeltPivotChamber")]
	private Transform _beltPivotChamber;

	[FormerlySerializedAs("BetlPivotMagazine")]
	[SerializeField]
	private Transform _beltPivotMagazine;

	[SerializeField]
	private Transform _beltEmptyPivotChamber;

	[SerializeField]
	private Vector2 _initialOrientation;

	[SerializeField]
	private Vector2 _pitchLimit;

	[SerializeField]
	private Vector2 _yawLimit;

	[SerializeField]
	private MagVisualController _magController;

	[SerializeField]
	private Transform _debugViews;

	[SerializeField]
	private Transform _collidersToCut;

	public string Template;

	public EStationaryAnimationType Animation;

	public Transform OperatorTransform;

	public Transform Hinge;

	public Transform TripodView;

	public Transform TripodAnchor;

	public Transform WeaponTransform;

	public float PitchToleranceUp;

	public float PitchToleranceDown;

	public float YawTolerance;

	public _EB1E ItemController;

	public WeaponMachinery Machinery;

	public MgBelt Belt;

	public MgBelt BeltEmpty;

	private Coroutine _E03A;

	private Transform _E03B;

	[SerializeField]
	private FollowerCullingObject _cullingObject;

	private string _E03C;

	private Vector3 _E03D;

	private Quaternion _E03E;

	[CompilerGenerated]
	private bool _E03F;

	public string OperatorId => _E03C;

	public string IdEditable
	{
		get
		{
			return Id;
		}
		set
		{
			_id = value;
		}
	}

	public GameObject GameObject => base.gameObject;

	public string TypeKey => _ED3E._E000(212990);

	public string Id => _id;

	public Item Item => ItemController.RootItem;

	public float Pitch
	{
		get
		{
			float num = Hinge.rotation.eulerAngles.x + 90f;
			if (!(num > 180f))
			{
				if (!(num < -180f))
				{
					return num;
				}
				return num + 360f;
			}
			return num - 360f;
		}
	}

	public float Yaw => Hinge.rotation.eulerAngles.y;

	public Vector2 Orientation => new Vector2(Yaw, Pitch);

	public Vector2 PitchLimit => _pitchLimit;

	public Vector2 YawLimit => _yawLimit;

	public Vector3 OperatorPosition => OperatorTransform.position;

	public bool Locked
	{
		[CompilerGenerated]
		get
		{
			return _E03F;
		}
		[CompilerGenerated]
		set
		{
			_E03F = value;
		}
	}

	public EDoorState State
	{
		get
		{
			if (!Locked)
			{
				return EDoorState.Open;
			}
			return EDoorState.Locked;
		}
	}

	public LootPointParameters AsLootPointParameters()
	{
		LootPointParameters lootPointParameters = new LootPointParameters();
		lootPointParameters.Enabled = true;
		lootPointParameters.IsContainer = true;
		lootPointParameters.IsStatic = true;
		lootPointParameters.Id = Id;
		lootPointParameters.FilterInclusive = new string[1] { Template };
		return lootPointParameters;
	}

	public void Init(_EB1E itemController)
	{
		ItemController = itemController;
		_EA6A currentMagazine = Item.GetCurrentMagazine();
		if (_magController != null)
		{
			_E54C resourceType = default(_E54C);
			resourceType.ResourceType = ResourceType.Magazine;
			_E760.SetupGameObjectWithoutPool(resourceType, _magController.gameObject);
			_magController.InitMagazine(Item.GetCurrentMagazine());
			_magController.gameObject.SetActive(value: true);
		}
		_E03E = WeaponTransform.localRotation;
		_E03D = WeaponTransform.localPosition;
		if ((bool)Belt)
		{
			Belt.Init();
			Belt.KeyElemetns[3].transform.SetPositionAndRotation(_beltPivotMagazine.position, _beltPivotMagazine.rotation);
			Belt.SetPivotPoint(_beltPivotChamber);
		}
		if (BeltEmpty != null)
		{
			BeltEmpty.Init();
			BeltEmpty.SetPivotPoint(_beltEmptyPivotChamber);
			BeltEmpty.DisplayAll(currentMagazine.MaxCount - currentMagazine.Count);
		}
		if (_cullingObject != null)
		{
			_cullingObject.OnVisibilityChanged += _E000;
			_E000(_cullingObject.IsVisible);
		}
	}

	public void ManualUpdate(Vector3 position, Quaternion rotation)
	{
		WeaponTransform.SetPositionAndRotation(position, rotation);
		Vector3 position2 = TripodAnchor.position;
		TripodView.position = new Vector3(position2.x, TripodView.position.y, position2.z);
		Machinery.UpdateJoints();
	}

	public void UpdateJoints()
	{
	}

	public void OnValidate()
	{
		_E001();
		if (!(Hinge == null))
		{
			_initialOrientation = Orientation;
			_pitchLimit = new Vector2(_initialOrientation.y - PitchToleranceUp, _initialOrientation.y + PitchToleranceDown);
			_pitchLimit.x = ((_pitchLimit.x > 180f) ? (_pitchLimit.x - 360f) : ((_pitchLimit.x < -180f) ? (_pitchLimit.x + 360f) : _pitchLimit.x));
			_pitchLimit.y = ((_pitchLimit.y > 180f) ? (_pitchLimit.y - 360f) : ((_pitchLimit.y < -180f) ? (_pitchLimit.y + 360f) : _pitchLimit.y));
			_yawLimit = new Vector2(_initialOrientation.x - YawTolerance, _initialOrientation.x + YawTolerance);
			if (_yawLimit.x > 360f || _yawLimit.y > 360f)
			{
				_yawLimit -= 360f * Vector2.one;
			}
		}
	}

	private void Awake()
	{
		_E03B = base.transform;
		if (_cullingObject != null)
		{
			_cullingObject.Init(() => _E03B);
		}
	}

	private void OnDestroy()
	{
		_magController?.ReturnAmmo();
		if (_cullingObject != null)
		{
			_cullingObject.OnVisibilityChanged -= _E000;
		}
	}

	private void _E000(bool isVisible)
	{
		if (Belt != null)
		{
			Belt.SetVisible(isVisible);
		}
		if (BeltEmpty != null)
		{
			BeltEmpty.SetVisible(isVisible);
		}
	}

	private void _E001()
	{
	}

	public WorldInteractiveObject._E001 GetInteractionParameters()
	{
		WorldInteractiveObject._E001 result = default(WorldInteractiveObject._E001);
		result.InteractionPosition = OperatorPosition;
		result.Snap = true;
		result.RotationMode = WorldInteractiveObject.ERotationInterpolationMode.ViewTargetAsOrientation;
		result.ViewTarget = new Vector3(Yaw, Pitch);
		result.Sit = Animation != EStationaryAnimationType.UtesStand;
		return result;
	}

	public void Hide(bool isAI)
	{
		GameObject[] gameObjectsWithRenderers = _gameObjectsWithRenderers;
		for (int i = 0; i < gameObjectsWithRenderers.Length; i++)
		{
			gameObjectsWithRenderers[i].SetActive(value: false);
		}
		if (BeltEmpty != null)
		{
			BeltEmpty.On = true;
		}
		if (Belt != null)
		{
			Belt.On = true;
		}
		if (_E03A != null)
		{
			StopCoroutine(_E03A);
		}
		if (isAI && _collidersToCut != null)
		{
			_collidersToCut.gameObject.SetActive(value: false);
		}
	}

	public void Show()
	{
		GameObject[] gameObjectsWithRenderers = _gameObjectsWithRenderers;
		for (int i = 0; i < gameObjectsWithRenderers.Length; i++)
		{
			gameObjectsWithRenderers[i].SetActive(value: true);
		}
		if (BeltEmpty != null)
		{
			BeltEmpty.SetPivotPoint(_beltEmptyPivotChamber);
			BeltEmpty.On = false;
		}
		if (Belt != null)
		{
			Belt.On = false;
			Belt.SetPivotPoint(_beltPivotChamber);
		}
		if (_E03A != null)
		{
			StopCoroutine(_E03A);
		}
		_E03A = StartCoroutine(ResetCoroutine());
		if (_collidersToCut != null && !_collidersToCut.gameObject.activeInHierarchy)
		{
			_collidersToCut.gameObject.SetActive(value: true);
		}
	}

	public IEnumerator ResetCoroutine()
	{
		float num = 0f;
		Vector3 localPosition = WeaponTransform.localPosition;
		Quaternion localRotation = WeaponTransform.localRotation;
		while (num < 1f)
		{
			num += Time.deltaTime;
			WeaponTransform.localPosition = Vector3.Lerp(localPosition, _E03D, num);
			WeaponTransform.localRotation = Quaternion.Lerp(localRotation, _E03E, num);
			yield return null;
		}
	}

	[ContextMenu("Align to ground")]
	public void AlignToGround()
	{
		if (Physics.Raycast(OperatorPosition + Vector3.up, Vector3.down, out var hitInfo, 4f, LayerMask.GetMask(_ED3E._E000(30808), _ED3E._E000(60734), _ED3E._E000(25436))))
		{
			Vector3 vector = OperatorPosition - hitInfo.point;
			base.transform.parent.position -= vector;
		}
	}

	public void Shot()
	{
		_EA6A currentMagazine = Item.GetCurrentMagazine();
		if ((bool)_magController)
		{
			_magController.Update();
		}
		if ((bool)Belt)
		{
			if (currentMagazine.Count < 20)
			{
				Belt.KeyElemetns[3].isKinematic = false;
			}
			Belt.AddWave();
		}
		if ((bool)BeltEmpty)
		{
			BeltEmpty.DisplayDelta(currentMagazine.MaxCount - currentMagazine.Count);
			BeltEmpty.AddWave();
		}
	}

	public void Unlock(string profileId)
	{
		if (IsOperator(profileId) || string.IsNullOrEmpty(profileId))
		{
			Locked = false;
		}
	}

	public void SetOperator(string visitorId, bool isAI = false)
	{
		_E03C = visitorId;
		Locked = true;
	}

	public bool IsOperator(string profileId)
	{
		return profileId == _E03C;
	}

	public bool IsAvailable(string profileId)
	{
		if (Locked)
		{
			return IsOperator(profileId);
		}
		return true;
	}

	public override void OnDrawGizmosSelected()
	{
		Vector3 position = Hinge.position;
		Quaternion[] array = new Quaternion[6]
		{
			Quaternion.Euler(Pitch, Yaw - YawTolerance, 0f),
			Quaternion.Euler(Pitch, Yaw + YawTolerance, 0f),
			Quaternion.Euler(Pitch + PitchToleranceDown, Yaw - YawTolerance, 0f),
			Quaternion.Euler(Pitch - PitchToleranceUp, Yaw - YawTolerance, 0f),
			Quaternion.Euler(Pitch + PitchToleranceDown, Yaw + YawTolerance, 0f),
			Quaternion.Euler(Pitch - PitchToleranceUp, Yaw + YawTolerance, 0f)
		};
		foreach (Quaternion quaternion in array)
		{
			Debug.DrawRay(position, quaternion * Vector3.forward * 10f, Color.cyan, Time.deltaTime);
		}
		if ((bool)_debugViews)
		{
			for (int j = 0; j < _debugViews.childCount; j++)
			{
				Transform child = _debugViews.GetChild(j);
				Debug.DrawLine(child.position, child.GetChild(0).position, Color.cyan, Time.deltaTime);
			}
		}
	}

	public void SetPivots(TransformLinks hierarchy)
	{
		Transform root = hierarchy.GetTransform(ECharacterWeaponBones.weapon);
		if (Machinery != null)
		{
			Machinery.SetTransforms(hierarchy);
		}
		if ((bool)Belt)
		{
			Belt.SetPivotPoint(root.FindTransform(_beltPivotChamber.name));
		}
		if ((bool)BeltEmpty)
		{
			BeltEmpty.SetPivotPoint(root.FindTransform(_beltEmptyPivotChamber.name));
		}
	}

	public void OnRotation()
	{
		Machinery.OnRotation();
	}

	[CompilerGenerated]
	private Transform _E002()
	{
		return _E03B;
	}
}
