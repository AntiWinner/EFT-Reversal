using System;
using Comfort.Common;
using EFT;
using UnityEngine;

public class CharacterControllerSpawner : MonoBehaviour
{
	[Serializable]
	public class Mode
	{
		public ControllerType Type;

		public bool WithRigidbody;

		public bool WithMovementValidation;

		public bool WithTriggerCollider;

		public bool WithKinematicOption;
	}

	public enum ControllerType
	{
		Unity,
		Impostor,
		Simple,
		SteeringImpostor,
		BotAISteeringImpostorWithDoors
	}

	[SerializeField]
	private float _slopeLimit;

	[SerializeField]
	private float _stepOffset;

	[SerializeField]
	private float _skinWidth;

	[SerializeField]
	private float _minMoveDistance;

	[SerializeField]
	private Vector3 _center;

	[SerializeField]
	private float _radius;

	[SerializeField]
	private float _height;

	[SerializeField]
	[Range(-5f, 5f)]
	private float _tilt;

	[SerializeField]
	[Range(-1f, 1f)]
	private float _step;

	[SerializeField]
	private PlayerSpiritBones.PoseType _pose;

	private CharacterController m__E000;

	private CapsuleCollider m__E001;

	private Rigidbody _E002;

	private TriggerColliderSearcher _E003;

	private Mode _E004;

	public float slopeLimit => _slopeLimit;

	public float stepOffset => _stepOffset;

	public float skinWidth => _skinWidth;

	public Vector3 center
	{
		get
		{
			return _center;
		}
		set
		{
			_center = value;
		}
	}

	public float radius => _radius;

	public float height
	{
		get
		{
			return _height;
		}
		set
		{
			_height = value;
		}
	}

	public float tilt
	{
		get
		{
			return _tilt;
		}
		set
		{
			_tilt = value;
		}
	}

	public float step
	{
		get
		{
			return _step;
		}
		set
		{
			_step = value;
		}
	}

	public PlayerSpiritBones.PoseType pose
	{
		get
		{
			return _pose;
		}
		set
		{
			_pose = value;
		}
	}

	public CharacterController CharacterController => this.m__E000;

	public CapsuleCollider CapsuleCollider => this.m__E001;

	public Rigidbody Rigidbody => _E002;

	public TriggerColliderSearcher TriggerColliderSearcher => _E003;

	public _E33A Spawn(Mode settings, Player player, GameObject gameObject, bool isSpirit, bool isThirdPerson)
	{
		_E33A obj = null;
		_E004 = settings;
		Collider collider = null;
		if (_E004.Type == ControllerType.Unity)
		{
			if (this.m__E000 == null)
			{
				this.m__E000 = gameObject.GetOrAddComponent<CharacterController>();
			}
			this.m__E000.slopeLimit = _slopeLimit;
			this.m__E000.stepOffset = _stepOffset;
			this.m__E000.skinWidth = _skinWidth;
			this.m__E000.minMoveDistance = _minMoveDistance;
			this.m__E000.center = _center;
			this.m__E000.radius = _radius;
			this.m__E000.height = _height;
			collider = this.m__E000;
			obj = ((!isSpirit) ? ((_E33A)new _E339(CharacterController)) : ((_E33A)new _E34A(CharacterController)));
		}
		else if (_E004.Type == ControllerType.Impostor)
		{
			_E001();
			collider = this.m__E001;
			obj = new _E348(_E004.WithMovementValidation, CapsuleCollider, Rigidbody, player, skinWidth, _E37B.PlayerStaticCollisionsMask);
		}
		else if (_E004.Type == ControllerType.SteeringImpostor)
		{
			_E001();
			collider = this.m__E001;
			obj = new _E347(_E004.WithMovementValidation, CapsuleCollider, Rigidbody, player, skinWidth, _E37B.PlayerCollisionsMask);
		}
		else if (_E004.Type == ControllerType.BotAISteeringImpostorWithDoors)
		{
			_E001();
			collider = this.m__E001;
			obj = new _E347(_E004.WithMovementValidation, CapsuleCollider, Rigidbody, player, skinWidth, _E37B.PlayerStaticDoorMask);
		}
		else if (_E004.Type == ControllerType.Simple)
		{
			_E001();
			collider = this.m__E001;
			SimpleCharacterController orAddComponent = gameObject.GetOrAddComponent<SimpleCharacterController>();
			orAddComponent.Init(CapsuleCollider.transform, new Collider[1] { CapsuleCollider }, CapsuleCollider, EFTHardSettings.Instance.SIMPLE_CHARACTER_CONTROLLER_FIXED_DELTA_DISTANCE, _E37B.PlayerCollisionsMask, stepOffset, slopeLimit);
			if (Singleton<GameWorld>.Instantiated)
			{
				orAddComponent.RegisterCanSeepThroughDelegate((Collider x) => Singleton<GameWorld>.Instance.CanPlayerSeepThrough(x));
			}
			orAddComponent.AddNoSpeedLimitCollisionColliders(LocationScene.DoorsCollisionColliders);
			obj = orAddComponent;
		}
		_E000(collider, isThirdPerson);
		if (this.m__E001 != null)
		{
			this.m__E001.isTrigger = _E004.WithTriggerCollider;
		}
		if (_E004.WithRigidbody)
		{
			CreateRigidbody();
		}
		if (obj == null)
		{
			throw new Exception(_ED3E._E000(58610));
		}
		return obj;
	}

	public _E33A ReplaceSpawn(Mode settings, Player player, GameObject gameObject, bool isSpirit, bool isThirdPerson)
	{
		_E33A component = gameObject.GetComponent<_E33A>();
		if (component != null)
		{
			UnityEngine.Object.DestroyImmediate(component as Component);
			component = null;
		}
		return Spawn(settings, player, gameObject, isSpirit, isThirdPerson);
	}

	private void _E000(Collider collider, bool thirdPerson)
	{
		_E003 = base.gameObject.GetOrAddComponent<TriggerColliderSearcher>();
		if (!_E003.IsInited)
		{
			if (thirdPerson)
			{
				_E003.Init(collider, EFTHardSettings.Instance.TriggersCastLayerMaskForObservedPlayers);
			}
			else
			{
				_E003.Init(collider, EFTHardSettings.Instance.TriggersCastLayerMask);
			}
		}
	}

	private void _E001()
	{
		if (this.m__E001 == null)
		{
			this.m__E001 = base.gameObject.GetOrAddComponent<CapsuleCollider>();
			this.m__E001.center = _center;
			this.m__E001.radius = _radius;
			this.m__E001.height = _height;
		}
		this.m__E001.enabled = true;
	}

	public void CreateRigidbody()
	{
		if (_E002 == null)
		{
			_E002 = base.gameObject.GetOrAddComponent<Rigidbody>();
		}
		_E002.isKinematic = _E004.WithKinematicOption;
		_E002.useGravity = false;
		_E002.constraints = (RigidbodyConstraints)80;
	}
}
