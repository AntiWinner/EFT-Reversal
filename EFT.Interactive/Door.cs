using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;
using UnityEngine.AI;

namespace EFT.Interactive;

public class Door : WorldInteractiveObject
{
	private BrokenDoor _E028;

	public bool IsBroken;

	public bool CanBeBreached = true;

	public bool CanInteractWithBreach = true;

	[Space(10f)]
	[_E368]
	public AnimationCurve KickCurve = new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(0.5f, 1f, 1f, 1f));

	public ParticleSystem HitEffect;

	public AudioClip HitClip;

	public AudioClip BreachSound;

	[SerializeField]
	private OcclusionPortal _occlusionPortal;

	private List<Collider> _E029;

	public override EDoorState DoorState
	{
		get
		{
			return base.DoorState;
		}
		set
		{
			base.DoorState = value;
			_E000();
			_EBAF.Instance.CreateCommonEvent<_EBB7>().Invoke(this);
		}
	}

	public override float CurrentAngle
	{
		get
		{
			return base.CurrentAngle;
		}
		protected set
		{
			base.CurrentAngle = value;
		}
	}

	public override string TypeKey => _ED3E._E000(212701);

	public List<Collider> CollisionColliders
	{
		get
		{
			if (_E029 == null)
			{
				_E029 = new List<Collider>();
				Collider[] componentsInChildren = base.gameObject.GetComponentsInChildren<Collider>();
				foreach (Collider collider in componentsInChildren)
				{
					if (((int)_E37B.PlayerStaticCollisionsMask & (1 << collider.gameObject.layer)) != 0)
					{
						_E029.Add(collider);
					}
				}
			}
			return _E029;
		}
	}

	public override void OnValidate()
	{
		base.OnValidate();
		_E000();
	}

	private void _E000()
	{
		if (_occlusionPortal != null)
		{
			_occlusionPortal.open = DoorState != EDoorState.Shut && DoorState != EDoorState.Locked;
		}
	}

	public bool IsBreachAngle(Vector3 yourPosition)
	{
		Vector3 vector = base.transform.TransformPoint(viewTarget1) - yourPosition;
		Vector3 vector2 = GetDoorRotation(GetAngle(EDoorState.Shut)) * WorldInteractiveObject.GetRotationAxis(DoorForward, base.transform);
		return Mathf.Abs(Vector2.Dot(new Vector2(vector.x, vector.z).normalized, new Vector2(vector2.x, vector2.z).normalized)) < EFTHardSettings.Instance.DOOR_BREACH_THRESHOLD;
	}

	public bool BreachSuccessRoll(Vector3 yourPosition)
	{
		if (!CanBeBreached || !Operatable)
		{
			return false;
		}
		Vector3 vector = base.transform.TransformPoint(viewTarget1) - yourPosition;
		Vector3 vector2 = GetDoorRotation(GetAngle(EDoorState.Shut)) * WorldInteractiveObject.GetRotationAxis(DoorForward, base.transform);
		Vector3 vector3 = GetDoorRotation(GetAngle(EDoorState.Open)) * WorldInteractiveObject.GetRotationAxis(DoorForward, base.transform);
		Vector3 vector4 = vector2 + vector3;
		return Vector2.Dot(new Vector2(vector.x, vector.z).normalized, new Vector2(vector4.x, vector4.z).normalized) > 0f;
	}

	public static _ECD9<_EBFE> Interact(Player player, EInteractionType interactionType)
	{
		_ECD1 canInteract = player.MovementContext.CanInteract;
		if (canInteract != null)
		{
			return canInteract;
		}
		return new _EBFE(interactionType);
	}

	public override void Interact(_EBFE interactionResult)
	{
		switch (interactionResult.InteractionType)
		{
		case EInteractionType.Open:
			if (DoorState != EDoorState.Locked)
			{
				Open();
			}
			break;
		case EInteractionType.Close:
			Close();
			break;
		case EInteractionType.Unlock:
			if (((_EBFF)interactionResult).Succeed)
			{
				Unlock();
			}
			break;
		case EInteractionType.Breach:
			KickOpen(confirmed: false);
			break;
		}
	}

	public override WorldInteractiveObject._E001 GetInteractionParameters(Vector3 yourPosition)
	{
		Vector3 interactionPosition = GetInteractionPosition(yourPosition);
		int animationId = CloseID;
		if (DoorState == EDoorState.Locked)
		{
			animationId = 50;
		}
		else if (DoorState == EDoorState.Shut)
		{
			Vector3 vector = base.transform.TransformPoint(viewTarget1) - interactionPosition;
			Vector3 vector2 = GetDoorRotation(GetAngle(EDoorState.Shut)) * WorldInteractiveObject.GetRotationAxis(DoorForward, base.transform);
			Vector3 vector3 = GetDoorRotation(GetAngle(EDoorState.Open)) * WorldInteractiveObject.GetRotationAxis(DoorForward, base.transform);
			Vector3 vector4 = vector2 + vector3;
			animationId = ((Vector2.Dot(new Vector2(vector.x, vector.z).normalized, new Vector2(vector4.x, vector4.z).normalized) > 0f) ? PushID : (PushID + 1));
		}
		WorldInteractiveObject._E001 result = default(WorldInteractiveObject._E001);
		result.InteractionPosition = interactionPosition;
		result.Grip = GetClosestGrip(yourPosition);
		result.AnimationId = animationId;
		result.ViewTarget = GetViewDirection(yourPosition);
		result.Snap = (DoorState & Snap) != 0;
		result.InitialState = DoorState;
		result.RotationMode = ERotationInterpolationMode.ViewTarget;
		return result;
	}

	public void FailBreach(Vector3 yourPosition)
	{
		if (HitEffect != null)
		{
			HitEffect.Play();
		}
		if (HitClip != null)
		{
			Vector3 vector = base.transform.TransformPoint(viewTarget1);
			Vector3 vector2 = yourPosition - vector;
			Singleton<BetterAudio>.Instance.PlayAtPoint(vector + vector2.normalized / 3f, HitClip, _E8A8.Instance.Distance(base.transform.position), BetterAudio.AudioSourceGroupType.Collisions, 40, 1f, EOcclusionTest.Fast);
		}
		StartCoroutine(_E001());
	}

	private IEnumerator _E001()
	{
		yield return new WaitForSeconds(0.3f);
		if (!_interaction.IsInProgress)
		{
			DoorState = base.FallbackState;
		}
	}

	public override void SetInitialSyncState(WorldInteractiveObject._E000 info)
	{
		if (!IsBroken && info.IsBroken)
		{
			IsBroken = true;
			if (_E028 != null)
			{
				_E028.BreachQuick();
			}
		}
		base.SetInitialSyncState(info);
	}

	public override void SyncInteractState(WorldInteractiveObject._E000 info)
	{
		if ((info.State & 0x10u) != 0)
		{
			if (_interaction.ResultState != EDoorState.Breaching)
			{
				if (CanStartInteraction(EDoorState.Breaching))
				{
					if ((info.State & 8u) != 0)
					{
						LockForInteraction();
						KickOpen(confirmed: true);
					}
					else
					{
						DoorState = EDoorState.Open;
						CurrentAngle = GetAngle(EDoorState.Breaching);
					}
				}
			}
			else
			{
				_interaction.IsConfirmed = true;
			}
		}
		else
		{
			base.SyncInteractState(info);
		}
	}

	public void KickOpen(bool confirmed)
	{
		KickOpen(base.transform.position, confirmed);
	}

	public void KickOpen(Vector3 yourPosition, bool confirmed = false)
	{
		if (_interaction.ResultState != EDoorState.Breaching || _interaction.Break)
		{
			_interaction.InitSmoothOpen(EDoorState.Breaching, CloseAngle, OpenAngle, KickCurve);
			_interaction.IsConfirmed = confirmed;
			StartCoroutine(SmoothDoorOpenCoroutine(EDoorState.Open, isLocalInteraction: true));
			IsBroken = true;
			if ((object)_E028 != null)
			{
				_E028.Breach(yourPosition);
			}
			if (BreachSound != null)
			{
				Vector3 vector = base.transform.TransformPoint(viewTarget1);
				Vector3 vector2 = yourPosition - vector;
				Singleton<BetterAudio>.Instance.PlayAtPoint(vector + vector2.normalized / 3f, BreachSound, _E8A8.Instance.Distance(base.transform.position), BetterAudio.AudioSourceGroupType.Collisions, 60, 1f, EOcclusionTest.Fast);
			}
		}
	}

	public override float GetAngle(EDoorState state)
	{
		if (state == EDoorState.Breaching)
		{
			return KickCurve[KickCurve.length - 1].value * OpenAngle;
		}
		return base.GetAngle(state);
	}

	protected void Start()
	{
		bool flag = AIEditorPosibleState();
		if (Operatable && flag)
		{
			if (Obstacle == null)
			{
				Obstacle = FindSelfObstalce();
			}
			if (Obstacle != null)
			{
				Obstacle.enabled = true;
				Obstacle.carving = false;
			}
			_E028 = base.transform.parent.GetComponentInChildren<BrokenDoor>(includeInactive: true);
			if ((object)_E028 != null)
			{
				_E028.Init();
			}
		}
	}

	public bool AIEditorPosibleState()
	{
		if (DoorState != EDoorState.Open)
		{
			return DoorState == EDoorState.Shut;
		}
		return true;
	}

	public WorldInteractiveObject._E001 GetBreakInParameters(Vector3 yourPosition)
	{
		Vector3 interactionPosition = _E002(yourPosition);
		Vector3 viewDirection = GetViewDirection(yourPosition);
		Vector3 vector = WorldInteractiveObject.GetRotationAxis(DoorForward, base.transform) * 0.5f + base.transform.position;
		viewDirection.x = vector.x;
		viewDirection.z = vector.z;
		WorldInteractiveObject._E001 result = default(WorldInteractiveObject._E001);
		result.InteractionPosition = interactionPosition;
		result.Grip = null;
		result.AnimationId = 0;
		result.ViewTarget = viewDirection;
		result.Snap = (DoorState & Snap) != 0;
		result.InitialState = DoorState;
		result.RotationMode = ERotationInterpolationMode.ViewTargetWithZeroPitch;
		return result;
	}

	protected override IEnumerator SmoothDoorOpenCoroutine(EDoorState state, bool isLocalInteraction, float speed = 1f)
	{
		if (_interaction.ResultState != EDoorState.Breaching)
		{
			yield return new WaitForSeconds(0.5f);
		}
		yield return base.SmoothDoorOpenCoroutine(state, isLocalInteraction);
	}

	private Vector3 _E002(Vector3 yourPosition)
	{
		Vector3 rotationAxis = WorldInteractiveObject.GetRotationAxis(DoorForward, base.transform);
		Vector3 vector = Vector3.Cross(rotationAxis, Vector3.up);
		Vector3 vector2 = base.transform.position + vector * 1.3f + rotationAxis * 0.5f;
		vector2.y = yourPosition.y;
		Vector3 vector3 = base.transform.position - vector * 1.3f + rotationAxis * 0.5f;
		vector3.y = yourPosition.y;
		if (!(Vector3.Distance(vector2, yourPosition) < Vector3.Distance(vector3, yourPosition)))
		{
			return vector3;
		}
		return vector2;
	}

	public NavMeshObstacle FindSelfObstalce()
	{
		List<NavMeshObstacle> list = new List<NavMeshObstacle>();
		_E39D.SearchTransform(base.transform, list);
		if (list.Count == 1)
		{
			NavMeshObstacle navMeshObstacle = list[0];
			if (navMeshObstacle.GetComponent<Door>() != null)
			{
				return navMeshObstacle;
			}
		}
		return null;
	}

	public override WorldInteractiveObject._E000 GetStatusInfo(bool solidState = false)
	{
		WorldInteractiveObject._E000 statusInfo = base.GetStatusInfo();
		statusInfo.IsBroken = IsBroken;
		return statusInfo;
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private IEnumerator _E003(EDoorState state, bool isLocalInteraction, float speed)
	{
		return base.SmoothDoorOpenCoroutine(state, isLocalInteraction, speed);
	}
}
