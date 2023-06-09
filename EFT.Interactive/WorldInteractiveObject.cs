using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.AssetsManager;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.AI;

namespace EFT.Interactive;

public class WorldInteractiveObject : InteractableObject, _EC07
{
	protected class InteractionState
	{
		private float _E000;

		private float _E001;

		private AnimationCurve _E002;

		[CompilerGenerated]
		private EDoorState _E003;

		[CompilerGenerated]
		private bool _E004;

		[CompilerGenerated]
		private float _E005;

		[CompilerGenerated]
		private float _E006;

		[CompilerGenerated]
		private float _E007;

		[CompilerGenerated]
		private float _E008;

		[CompilerGenerated]
		private float _E009;

		[CompilerGenerated]
		private float _E00A;

		[CompilerGenerated]
		private bool _E00B;

		public EDoorState ResultState
		{
			[CompilerGenerated]
			get
			{
				return _E003;
			}
			[CompilerGenerated]
			set
			{
				_E003 = value;
			}
		}

		public bool Break
		{
			[CompilerGenerated]
			get
			{
				return _E004;
			}
			[CompilerGenerated]
			set
			{
				_E004 = value;
			}
		}

		public float ResultAngle
		{
			[CompilerGenerated]
			get
			{
				return _E005;
			}
			[CompilerGenerated]
			private set
			{
				_E005 = value;
			}
		}

		public float ProgressTime
		{
			[CompilerGenerated]
			get
			{
				return _E006;
			}
			[CompilerGenerated]
			private set
			{
				_E006 = value;
			}
		}

		public float Duration
		{
			[CompilerGenerated]
			get
			{
				return _E007;
			}
			[CompilerGenerated]
			private set
			{
				_E007 = value;
			}
		}

		public float Speed
		{
			[CompilerGenerated]
			get
			{
				return _E008;
			}
			[CompilerGenerated]
			set
			{
				_E008 = value;
			}
		}

		public float StartTime
		{
			[CompilerGenerated]
			get
			{
				return _E009;
			}
			[CompilerGenerated]
			set
			{
				_E009 = value;
			}
		}

		public float EndTime
		{
			[CompilerGenerated]
			get
			{
				return _E00A;
			}
			[CompilerGenerated]
			set
			{
				_E00A = value;
			}
		}

		public bool IsConfirmed
		{
			[CompilerGenerated]
			get
			{
				return _E00B;
			}
			[CompilerGenerated]
			set
			{
				_E00B = value;
			}
		}

		public bool TimeIsOver => ProgressTime >= Duration;

		public bool IsInProgress
		{
			get
			{
				if (ProgressTime < Duration)
				{
					return !Break;
				}
				return false;
			}
		}

		public float CurrentAngle => _E000 + ProgressCurve.Evaluate(ProgressTime) * _E001;

		public bool NearToResult => Mathf.Abs(ResultAngle - _E000) / 5f > Mathf.Abs(ResultAngle - CurrentAngle);

		public AnimationCurve ProgressCurve
		{
			get
			{
				return _E002 ?? EFTHardSettings.Instance.DoorCurve;
			}
			set
			{
				_E002 = value;
			}
		}

		public void UpdateTime(float deltaTime)
		{
			ProgressTime += deltaTime * Speed;
		}

		public void SyncAngle(float angle)
		{
			float value = 1f - Mathf.DeltaAngle(angle, ResultAngle) / _E001;
			ProgressTime = ProgressCurve.GetTime(value, 0.2f);
		}

		public void InitSmoothOpen(EDoorState state, float initAngle, float resultAngle, AnimationCurve progressCurve = null, bool confirmed = false)
		{
			ProgressCurve = progressCurve ?? EFTHardSettings.Instance.DoorCurve;
			Duration = ProgressCurve.GetDuration();
			Break = false;
			ResultState = state;
			ResultAngle = resultAngle * ProgressCurve[ProgressCurve.length - 1].value;
			_E000 = initAngle;
			_E001 = Mathf.DeltaAngle(initAngle, resultAngle);
			ProgressTime = 0f;
			IsConfirmed = confirmed;
		}

		public void SetEndData(EDoorState state, AnimationCurve curve)
		{
			ResultState = state;
			ProgressCurve = curve;
			Duration = curve.GetDuration();
			EndTime = Time.time;
		}

		public void SetEndData(InteractionState otherState)
		{
			SetEndData(otherState.ResultState, otherState.ProgressCurve);
		}

		public override string ToString()
		{
			return string.Format(_ED3E._E000(211009), _ED3E._E000(64014), ResultState, Break, ResultAngle, ProgressTime, Duration, Speed, StartTime, EndTime);
		}
	}

	public enum EDoorAxis
	{
		X,
		Y,
		Z,
		XNegative,
		YNegative,
		ZNegative
	}

	public enum EInteractionAction
	{
		Pull,
		Push
	}

	public enum ERotationInterpolationMode
	{
		ViewTarget,
		ViewTargetWithZeroPitch,
		ViewTargetAsOrientation
	}

	public struct _E000
	{
		public string Id;

		public byte State;

		public int Angle;

		public bool Updated;

		public bool IsBroken;

		[CompilerGenerated]
		private int m__E000;

		public int NetId
		{
			[CompilerGenerated]
			get
			{
				return m__E000;
			}
			[CompilerGenerated]
			set
			{
				m__E000 = value;
			}
		}

		public _E000(string id, EDoorState state, float angle)
		{
			this = default(_E000);
			Id = id;
			State = (byte)state;
			Angle = Mathf.FloorToInt(angle);
		}
	}

	public struct _E001
	{
		public EDoorState InitialState;

		public EInteractionAction Action;

		public Vector3 ViewTarget;

		public Vector3 InteractionPosition;

		public GripPose Grip;

		public int AnimationId;

		public bool Snap;

		public bool Sit;

		public ERotationInterpolationMode RotationMode;

		public bool BlockChangePosLevel;
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public _EBFE interactionResult;

		public WorldInteractiveObject _003C_003E4__this;

		internal void _E000()
		{
			switch (interactionResult.InteractionType)
			{
			case EInteractionType.Open:
				_003C_003E4__this.Open();
				break;
			case EInteractionType.Close:
				_003C_003E4__this.Close();
				break;
			case EInteractionType.Unlock:
				_003C_003E4__this.Unlock();
				break;
			case EInteractionType.Lock:
				_003C_003E4__this.Lock();
				break;
			case EInteractionType.Breach:
				break;
			}
		}
	}

	[CompilerGenerated]
	private _EC08 _E040;

	public static bool InteractionShouldBeConfirmed;

	private static int _E041;

	private const float _E042 = 1f;

	private const float _E043 = 20f;

	private const float _E044 = 30f;

	private const int _E045 = 1000;

	private const float _E046 = 0.8f;

	private const float _E047 = 0.1f;

	[Header("States that take control of a player")]
	[_E376(typeof(EDoorState))]
	public EDoorState Snap = EDoorState.Locked | EDoorState.Shut | EDoorState.Open | EDoorState.Interacting;

	public string KeyId;

	public string Id;

	[SerializeField]
	protected float _currentAngle;

	public Vector3 interactPosition1;

	public Vector3 interactPosition2;

	public Vector3 viewTarget1;

	[_E368]
	public DoorHandle LockHandle;

	public float OpenAngle = 60f;

	public float CloseAngle;

	public EDoorAxis DoorAxis = EDoorAxis.Z;

	public EDoorAxis DoorForward = EDoorAxis.Z;

	[Header("Animations")]
	public bool interactWithoutAnimation;

	public int PushID;

	public int CloseID;

	[_E368]
	public AudioClip[] ShutSound;

	[_E368]
	public AudioClip[] SqueakSound;

	[_E368]
	public AudioClip[] OpenSound;

	public float ShutShift;

	public NavMeshObstacle Obstacle;

	[SerializeField]
	protected EDoorState _doorState;

	[_E368]
	protected InteractionState _interaction = new InteractionState();

	[_E368]
	protected InteractionState _previousInteraction = new InteractionState();

	[SerializeField]
	protected DoorHandle _handle;

	[_E368]
	protected GripPose[] Grips;

	[SerializeField]
	private bool _forceLocalInteraction;

	[Header("Actions")]
	[Space(10f)]
	public bool Operatable = true;

	private int _E048;

	protected bool _forceSync;

	private bool _E049;

	private EDoorState _E04A;

	private float _E04B;

	protected bool _shutPlayed;

	[CompilerGenerated]
	private Player _E04C;

	[CompilerGenerated]
	private Collider _E04D;

	[CompilerGenerated]
	private EDoorState _E04E;

	[CompilerGenerated]
	private EDoorState _E04F;

	public Player InteractingPlayer
	{
		[CompilerGenerated]
		get
		{
			return _E04C;
		}
		[CompilerGenerated]
		private set
		{
			_E04C = value;
		}
	}

	public bool ForceLocalInteraction => _forceLocalInteraction;

	string _EC07.IdEditable
	{
		get
		{
			return Id;
		}
		set
		{
			Id = value;
		}
	}

	public int NetId
	{
		get
		{
			if (_E048 == 0)
			{
				_E048 = Interlocked.Increment(ref _E041);
			}
			return _E048;
		}
		set
		{
			_E048 = value;
		}
	}

	public virtual float CurrentAngle
	{
		get
		{
			return _currentAngle;
		}
		protected set
		{
			_E049 = true;
			_currentAngle = value;
			if (base.transform.parent != null)
			{
				base.transform.rotation = GetDoorRotation(_currentAngle) * base.transform.parent.rotation;
			}
		}
	}

	public virtual EDoorState DoorState
	{
		get
		{
			return _doorState;
		}
		set
		{
			EDoorState doorState = _doorState;
			_doorState = value;
			if (_doorState == EDoorState.Open && Singleton<_E307>.Instance != null)
			{
				Singleton<_E307>.Instance.InteractObject(Id, base.transform.position);
			}
			if (_doorState != doorState)
			{
				_E040?.Invoke(this, doorState, DoorState);
				DoorStateChanged(_doorState);
			}
		}
	}

	GameObject _EC07.GameObject => base.gameObject;

	public virtual float AngleSyncForSync => 30f;

	public virtual float MaxAllowedAngleDesync => 20f;

	public virtual string TypeKey => _ED3E._E000(210948);

	public virtual AnimationCurve ProgressCurve => EFTHardSettings.Instance.DoorCurve;

	public virtual float LockDistance => 0.8f;

	public Collider Collider
	{
		[CompilerGenerated]
		get
		{
			return _E04D;
		}
		[CompilerGenerated]
		private set
		{
			_E04D = value;
		}
	}

	public EDoorState FallbackState
	{
		[CompilerGenerated]
		get
		{
			return _E04E;
		}
		[CompilerGenerated]
		protected set
		{
			_E04E = value;
		}
	}

	public EDoorState InitialDoorState
	{
		[CompilerGenerated]
		get
		{
			return _E04F;
		}
		[CompilerGenerated]
		private set
		{
			_E04F = value;
		}
	}

	public Transform Handle => _handle.transform;

	public event _EC08 OnDoorStateChanged
	{
		[CompilerGenerated]
		add
		{
			_EC08 obj = _E040;
			_EC08 obj2;
			do
			{
				obj2 = obj;
				_EC08 value2 = (_EC08)Delegate.Combine(obj2, value);
				obj = Interlocked.CompareExchange(ref _E040, value2, obj2);
			}
			while ((object)obj != obj2);
		}
		[CompilerGenerated]
		remove
		{
			_EC08 obj = _E040;
			_EC08 obj2;
			do
			{
				obj2 = obj;
				_EC08 value2 = (_EC08)Delegate.Remove(obj2, value);
				obj = Interlocked.CompareExchange(ref _E040, value2, obj2);
			}
			while ((object)obj != obj2);
		}
	}

	public virtual void OnEnable()
	{
		if (Collider == null)
		{
			Collider = GetComponentsInChildren<Collider>().FirstOrDefault((Collider c) => c.gameObject.layer == _E37B.DoorLayer);
		}
		Grips = GetComponentsInChildren<GripPose>();
		CurrentAngle = GetAngle(DoorState);
		InitialDoorState = DoorState;
		_E04A = DoorState;
		GripPose[] grips = Grips;
		foreach (GripPose obj in grips)
		{
			Transform parent = obj.transform.parent;
			obj.transform.parent = null;
			obj.transform.localScale = Vector3.one;
			obj.transform.parent = parent;
		}
	}

	protected virtual void DoorStateChanged(EDoorState newState)
	{
	}

	public override void OnReturnToPool(AssetPoolObject assetPoolObject)
	{
		base.OnReturnToPool(assetPoolObject);
		_interaction = new InteractionState();
		_previousInteraction = new InteractionState();
	}

	public virtual GripPose GetClosestGrip(Vector3 yourPosition)
	{
		if (DoorState == EDoorState.Locked && LockHandle != null)
		{
			LockHandle.DefPos();
		}
		float num = float.MaxValue;
		float num2 = 0f;
		GripPose result = null;
		for (int i = 0; i < Grips.Length; i++)
		{
			if ((Grips[i].DoorState & DoorState) != 0)
			{
				num2 = yourPosition.SqrDistance(Grips[i].transform.position);
				if (!(num2 > num))
				{
					num = num2;
					result = Grips[i];
				}
			}
		}
		return result;
	}

	public virtual Vector3 GetInteractionPosition(Vector3 yourPosition)
	{
		Vector3 normalized = Vector3.Cross(GetRotationAxis(DoorForward, base.transform), GetRotationAxis(DoorAxis, base.transform)).normalized;
		Vector3 vector = ((LockHandle != null) ? Vector3.Lerp(LockHandle.transform.position, base.transform.position, 0.1f) : base.transform.position);
		Vector3 vector2 = ((DoorState == EDoorState.Locked) ? (vector + normalized * LockDistance) : (base.transform.parent.rotation * interactPosition1 + base.transform.position));
		vector2 = new Vector3(vector2.x, yourPosition.y, vector2.z);
		Vector3 lhs = vector - vector2;
		if ((Vector3.Dot(vector - yourPosition, normalized) > 0f) ^ (Vector3.Dot(lhs, normalized) > 0f))
		{
			vector2 = ((DoorState == EDoorState.Locked) ? (vector - normalized * LockDistance) : (base.transform.parent.rotation * interactPosition2 + base.transform.position));
			return new Vector3(vector2.x, yourPosition.y, vector2.z);
		}
		return vector2;
	}

	public void LockForInteraction()
	{
		if (DoorState != EDoorState.Interacting)
		{
			FallbackState = DoorState;
		}
		if (DoorState != EDoorState.Interacting)
		{
			DoorState = EDoorState.Interacting;
		}
	}

	public virtual void SetInitialSyncState(_E000 info)
	{
		if ((info.State & 4u) != 0 || (info.State & 2u) != 0 || ((uint)info.State & (true ? 1u : 0u)) != 0)
		{
			DoorState = (EDoorState)(info.State & 0xE7);
			CurrentAngle = GetAngle(DoorState);
		}
	}

	public virtual void SyncInteractState(_E000 info)
	{
		EDoorState eDoorState = (EDoorState)(info.State & 0xE7);
		bool flag = (info.State & 8) != 0;
		if (DoorState == eDoorState || (info.State & 0x10u) != 0)
		{
			return;
		}
		if (flag)
		{
			if (_interaction.ResultState == eDoorState)
			{
				_interaction.IsConfirmed = true;
			}
			if (!_E000(info.Angle, eDoorState))
			{
				LockForInteraction();
				bool flag2 = false;
				if ((_interaction.TimeIsOver && !_interaction.Break) || _interaction.Break)
				{
					InitializeSmoothOpenInteraction(eDoorState, confirmed: true);
					flag2 = true;
				}
				else if (eDoorState != _interaction.ResultState)
				{
					_previousInteraction.SetEndData(_interaction);
					InitializeSmoothOpenInteraction(eDoorState, confirmed: true);
				}
				if (flag2)
				{
					StartCoroutine(SmoothDoorOpenCoroutine(eDoorState, isLocalInteraction: false, 1.5f));
				}
				_interaction.SyncAngle(info.Angle);
			}
			else if (CanStartInteraction(eDoorState))
			{
				LockForInteraction();
				InitializeSmoothOpenInteraction(eDoorState);
				_interaction.IsConfirmed = true;
				StartCoroutine(SmoothDoorOpenCoroutine(eDoorState, isLocalInteraction: false, 1.5f));
			}
		}
		else if (CanStartInteraction(eDoorState) || _interaction.Break || !_E000(info.Angle, eDoorState))
		{
			_interaction.Break = true;
			_interaction.ResultState = eDoorState;
			_previousInteraction.SetEndData(eDoorState, ProgressCurve);
			CurrentAngle = GetAngle(eDoorState);
			DoorState = eDoorState;
		}
		else if (_interaction.IsInProgress && _interaction.ResultState == eDoorState)
		{
			_interaction.IsConfirmed = true;
		}
	}

	private bool _E000(float syncAngle, EDoorState state)
	{
		float angle = GetAngle(state);
		float num = Mathf.DeltaAngle(CurrentAngle, angle);
		if (num == 0f)
		{
			return true;
		}
		float num2 = num - Mathf.DeltaAngle(syncAngle, angle);
		if (Mathf.Sign(num2) != Mathf.Sign(num))
		{
			return true;
		}
		return Math.Abs(num2) <= MaxAllowedAngleDesync;
	}

	protected virtual bool CanStartInteraction(EDoorState state, bool logFalse = false)
	{
		return DoorState != state && (_interaction.ResultState != state || _interaction.Break) && (_previousInteraction.ResultState != state || _previousInteraction.EndTime + _interaction.Duration < Time.time);
	}

	private void _E001(EDoorState state, bool force = false)
	{
		if (CanStartInteraction(state, logFalse: true))
		{
			_interaction.ResultState = state;
			if (force)
			{
				CurrentAngle = GetAngle(state);
				DoorState = state;
			}
			else
			{
				InitializeSmoothOpenInteraction(state);
				StartCoroutine(SmoothDoorOpenCoroutine(state, isLocalInteraction: true, 1.5f));
			}
		}
		else if (!_interaction.IsInProgress && DoorState == EDoorState.Interacting)
		{
			DoorState = FallbackState;
		}
	}

	protected virtual void InitializeSmoothOpenInteraction(EDoorState state, bool confirmed = false)
	{
		_interaction.InitSmoothOpen(state, CurrentAngle, GetAngle(state), ProgressCurve, confirmed);
	}

	public virtual void OnValidate()
	{
		CheckUniqueIdOnDuplicateEvent();
		CurrentAngle = GetAngle((DoorState == EDoorState.Open) ? EDoorState.Open : EDoorState.Shut);
	}

	protected void CheckUniqueIdOnDuplicateEvent()
	{
	}

	public Quaternion GetDoorRotation(float currentAngle)
	{
		return Quaternion.AngleAxis(currentAngle, GetRotationAxis(DoorAxis, base.transform.parent));
	}

	public static Vector3 GetRotationAxis(EDoorAxis doorAxis, Transform objectTransform)
	{
		if (objectTransform == null)
		{
			return Vector3.zero;
		}
		return doorAxis switch
		{
			EDoorAxis.X => objectTransform.right, 
			EDoorAxis.Y => objectTransform.up, 
			EDoorAxis.Z => objectTransform.forward, 
			EDoorAxis.XNegative => -objectTransform.right, 
			EDoorAxis.YNegative => -objectTransform.up, 
			EDoorAxis.ZNegative => -objectTransform.forward, 
			_ => Vector3.zero, 
		};
	}

	protected void PlaySound(EDoorState state)
	{
		if (OpenSound.Length != 0 && state == EDoorState.Open)
		{
			AudioClip audioClip = OpenSound[UnityEngine.Random.Range(0, OpenSound.Length)];
			if ((bool)audioClip)
			{
				Singleton<BetterAudio>.Instance.PlayAtPoint(base.transform.position, audioClip, _E8A8.Instance.Distance(base.transform.position), BetterAudio.AudioSourceGroupType.Collisions, 35, UnityEngine.Random.Range(0.8f, 1f), EOcclusionTest.Fast);
			}
			else
			{
				Debug.LogError(string.Format(_ED3E._E000(211000), base.transform.parent, base.gameObject.name));
			}
		}
		if (SqueakSound.Length != 0)
		{
			AudioClip audioClip2 = SqueakSound[UnityEngine.Random.Range(0, SqueakSound.Length)];
			if ((bool)audioClip2)
			{
				Singleton<BetterAudio>.Instance.PlayAtPoint(base.transform.position, audioClip2, _E8A8.Instance.Distance(base.transform.position), BetterAudio.AudioSourceGroupType.Collisions, 35, UnityEngine.Random.Range(0.8f, 1f), EOcclusionTest.Fast);
			}
			else
			{
				Debug.LogError(string.Format(_ED3E._E000(211000), base.transform.parent, base.gameObject.name));
			}
		}
	}

	protected virtual IEnumerator SmoothDoorOpenCoroutine(EDoorState state, bool isLocalInteraction, float speed = 1f)
	{
		if (_interaction.Break)
		{
			yield break;
		}
		_interaction.StartTime = Time.time;
		if (_handle != null && _interaction.ResultState == EDoorState.Open)
		{
			StartCoroutine(_handle.OpenCoroutine());
			yield return new WaitForSeconds(_handle.OpenAnimation.keys.FirstOrDefault((Keyframe k) => k.value >= 1f).time);
		}
		PlaySound(_interaction.ResultState);
		_shutPlayed = false;
		_interaction.Speed = speed;
		int num = (int)(_interaction.Duration * 1000f);
		while (_interaction.IsInProgress && num-- > 0)
		{
			if (!ForceLocalInteraction && InteractionShouldBeConfirmed && !_interaction.IsConfirmed && _interaction.StartTime + 1f < Time.time)
			{
				Debug.LogErrorFormat(string.Format(_ED3E._E000(211095), Id, Time.time - _interaction.StartTime, NetworkGameSession.RTT));
				DoorState = FallbackState;
				CurrentAngle = GetAngle(DoorState);
				_interaction.Break = true;
				break;
			}
			CurrentAngle = _interaction.CurrentAngle;
			_interaction.UpdateTime(Time.deltaTime);
			if (_interaction.ResultState == EDoorState.Shut && !_shutPlayed && _interaction.ProgressTime > _interaction.Duration - ShutShift)
			{
				PlayShut();
			}
			yield return null;
		}
		if (num <= 0)
		{
			Debug.LogErrorFormat(_ED3E._E000(211150), Id, base.gameObject.scene.name, GetType().Name);
		}
		else if (!_interaction.Break)
		{
			if (!_shutPlayed && _interaction.ResultState == EDoorState.Shut)
			{
				PlayShut();
			}
			DoorState = ((_interaction.ResultState == EDoorState.Breaching) ? EDoorState.Open : _interaction.ResultState);
			CurrentAngle = _interaction.ResultAngle;
			_previousInteraction.SetEndData(_interaction);
		}
	}

	protected void PlayShut()
	{
		_shutPlayed = true;
		if (ShutSound.Length != 0)
		{
			AudioClip audioClip = ShutSound[UnityEngine.Random.Range(0, ShutSound.Length)];
			if ((bool)audioClip && Singleton<BetterAudio>.Instantiated)
			{
				Singleton<BetterAudio>.Instance.PlayAtPoint(base.transform.position, audioClip, _E8A8.Instance.Distance(base.transform.position), BetterAudio.AudioSourceGroupType.Collisions, 50, UnityEngine.Random.Range(0.2f, 0.3f), EOcclusionTest.Fast);
			}
			else
			{
				Debug.LogError(string.Format(_ED3E._E000(211000), base.transform.parent, base.gameObject.name));
			}
		}
	}

	public virtual float GetAngle(EDoorState state)
	{
		switch (state)
		{
		case EDoorState.Locked:
		case EDoorState.Shut:
			return CloseAngle;
		case EDoorState.Open:
			return OpenAngle;
		default:
			return _currentAngle;
		}
	}

	public float GetInitAngle(EDoorState state)
	{
		switch (state)
		{
		case EDoorState.Shut:
			return OpenAngle;
		case EDoorState.Open:
		case EDoorState.Breaching:
			return CloseAngle;
		default:
			return _currentAngle;
		}
	}

	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.red;
		Gizmos.DrawRay(base.transform.position, GetDoorRotation(GetAngle(EDoorState.Shut)) * GetRotationAxis(DoorForward, base.transform));
		Gizmos.color = Color.green;
		Gizmos.DrawRay(base.transform.position, GetDoorRotation(GetAngle(EDoorState.Open)) * GetRotationAxis(DoorForward, base.transform));
		Gizmos.color = Color.blue;
		Gizmos.DrawCube(base.transform.parent.rotation * interactPosition1 + base.transform.position, Vector3.one * 0.1f);
		Gizmos.DrawCube(base.transform.parent.rotation * interactPosition2 + base.transform.position, Vector3.one * 0.1f);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.TransformPoint(viewTarget1), 0.05f);
	}

	public virtual _E001 GetInteractionParameters(Vector3 yourPosition)
	{
		Vector3 interactionPosition = GetInteractionPosition(yourPosition);
		int animationId = CloseID;
		if (DoorState == EDoorState.Locked)
		{
			animationId = 50;
		}
		else if (DoorState == EDoorState.Shut)
		{
			animationId = PushID;
		}
		_E001 result = default(_E001);
		result.InteractionPosition = interactionPosition;
		result.Grip = GetClosestGrip(yourPosition);
		result.AnimationId = animationId;
		result.ViewTarget = GetViewDirection(yourPosition);
		result.Snap = (DoorState & Snap) != 0;
		result.InitialState = DoorState;
		result.RotationMode = ERotationInterpolationMode.ViewTarget;
		return result;
	}

	public virtual void SetUser(Player player)
	{
		InteractingPlayer = player;
	}

	public virtual void Interact(_EBFE interactionResult)
	{
		this.StartBehaviourTimer(EFTHardSettings.Instance.DelayToOpenContainer, delegate
		{
			switch (interactionResult.InteractionType)
			{
			case EInteractionType.Open:
				Open();
				break;
			case EInteractionType.Close:
				Close();
				break;
			case EInteractionType.Unlock:
				Unlock();
				break;
			case EInteractionType.Lock:
				Lock();
				break;
			case EInteractionType.Breach:
				break;
			}
		});
	}

	protected void Lock()
	{
		EDoorState doorState = (_interaction.ResultState = EDoorState.Locked);
		DoorState = doorState;
		CurrentAngle = GetAngle(DoorState);
	}

	protected void Unlock()
	{
		StartCoroutine(UnlockCoroutine());
	}

	protected virtual IEnumerator UnlockCoroutine()
	{
		if (LockHandle != null)
		{
			yield return LockHandle.OpenCoroutine();
		}
		DoorState = EDoorState.Shut;
		CurrentAngle = GetAngle(DoorState);
	}

	protected void Open()
	{
		_E001(EDoorState.Open);
	}

	protected void Close()
	{
		_E001(EDoorState.Shut);
	}

	public void ForceCloseLocalDoor(bool playSound)
	{
		if (ForceLocalInteraction)
		{
			_E001(EDoorState.Shut, force: true);
			if (playSound)
			{
				PlayShut();
			}
		}
	}

	public virtual Vector3 GetViewDirection(Vector3 position)
	{
		if (DoorState == EDoorState.Locked && LockHandle != null)
		{
			return Vector3.Lerp(LockHandle.transform.position, new Vector3(base.transform.position.x, LockHandle.transform.position.y, base.transform.position.z), 0.1f);
		}
		return base.transform.TransformPoint(viewTarget1);
	}

	public void SetFromStatusInfo(_E000 info)
	{
		_E001((EDoorState)info.State, force: true);
	}

	public bool IsInteractionSuitsCurrentState(EInteractionType interaction)
	{
		EDoorState eDoorState = DoorState;
		if (eDoorState == EDoorState.Interacting)
		{
			eDoorState = FallbackState;
		}
		switch (interaction)
		{
		case EInteractionType.Open:
		case EInteractionType.Breach:
			if (eDoorState != EDoorState.Open)
			{
				break;
			}
			return false;
		case EInteractionType.Close:
			if (eDoorState != EDoorState.Shut)
			{
				break;
			}
			return false;
		}
		return true;
	}

	public virtual _E000 GetStatusInfo(bool solidState = false)
	{
		_E04B = CurrentAngle;
		_E049 = false;
		EDoorState eDoorState = DoorState;
		if (DoorState == EDoorState.Interacting)
		{
			eDoorState |= _interaction.ResultState;
		}
		if (solidState)
		{
			eDoorState &= ~EDoorState.Interacting;
			if (eDoorState == EDoorState.Breaching)
			{
				eDoorState = EDoorState.Open;
			}
		}
		_E04A = eDoorState;
		_forceSync = false;
		_E000 result = new _E000(Id, eDoorState, CurrentAngle);
		result.NetId = NetId;
		return result;
	}

	public virtual _ECD9<_EBFF> UnlockOperation(KeyComponent key, Player player)
	{
		_ECD1 canInteract = player.MovementContext.CanInteract;
		if (canInteract != null)
		{
			return canInteract;
		}
		if (!(key.Template.KeyId == KeyId))
		{
			return new _ECD2(_ED3E._E000(211031));
		}
		_ECD8<_EB38> obj = default(_ECD8<_EB38>);
		key.NumberOfUsages++;
		if (key.NumberOfUsages >= key.Template.MaximumNumberOfUsage && key.Template.MaximumNumberOfUsage > 0)
		{
			obj = _EB29.Discard(key.Item, (_EB1E)key.Item.Parent.GetOwner());
			if (obj.Failed)
			{
				return obj.Error;
			}
		}
		return new _EBFF(key, obj.Value, succeed: true);
	}
}
