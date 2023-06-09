using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.Interactive;

public class LootableContainer : WorldInteractiveObject, _E354
{
	[CompilerGenerated]
	private new sealed class _E000
	{
		public _EBFE interactionResult;

		public LootableContainer _003C_003E4__this;

		internal void _E000()
		{
			switch (interactionResult.InteractionType)
			{
			case EInteractionType.Open:
				if (_003C_003E4__this.DoorState != EDoorState.Locked)
				{
					_003C_003E4__this.Open();
				}
				break;
			case EInteractionType.Close:
				_003C_003E4__this.Close();
				break;
			case EInteractionType.Unlock:
				if (((_EBFF)interactionResult).Succeed)
				{
					_003C_003E4__this.Unlock();
				}
				break;
			case EInteractionType.Lock:
				_003C_003E4__this.Lock();
				break;
			case EInteractionType.Breach:
				break;
			}
		}
	}

	[SerializeField]
	public _EB1E ItemOwner;

	public string Template;

	public Vector3 ClosedPosition;

	public Vector3 OpenPosition;

	private _EC02[] _E039;

	public float ChanceModifier;

	[_E368]
	public List<LootableContainerParameters> FilterExtended;

	public _EC02[] Trees
	{
		get
		{
			if (_E039 == null || _E039.Length == 0)
			{
				_E039 = _E2E4<_EC0C>.Instance.ItemTemplatesTree.Select(_EC02.CreateReferenceTree).ToArray();
			}
			return _E039;
		}
	}

	public override string TypeKey => _ED3E._E000(212891);

	public new string Id
	{
		get
		{
			return base.Id;
		}
		set
		{
			base.Id = value;
		}
	}

	public override void Interact(_EBFE interactionResult)
	{
		this.StartBehaviourTimer(EFTHardSettings.Instance.DelayToOpenContainer, delegate
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
			case EInteractionType.Lock:
				Lock();
				break;
			case EInteractionType.Breach:
				break;
			}
		});
	}

	[ContextMenu("Save OPEN position")]
	private void _E000()
	{
		OpenPosition = base.transform.localPosition;
	}

	[ContextMenu("Save CLOSED position")]
	private void _E001()
	{
		ClosedPosition = base.transform.localPosition;
	}

	public override void OnValidate()
	{
		CheckUniqueIdOnDuplicateEvent();
		CurrentAngle = GetAngle((DoorState == EDoorState.Open) ? EDoorState.Open : EDoorState.Shut);
		if ((ClosedPosition - OpenPosition).sqrMagnitude != 0f)
		{
			base.transform.localPosition = Vector3.Lerp(OpenPosition, ClosedPosition, (DoorState != EDoorState.Open) ? 1 : 0);
		}
	}

	public LootPointParameters AsLootPointParameters()
	{
		LootPointParameters lootPointParameters = new LootPointParameters();
		lootPointParameters.Enabled = true;
		lootPointParameters.FilterInclusive = new string[1] { Template };
		lootPointParameters.FilterExclusive = new string[0];
		lootPointParameters.FilterExtended = FilterExtended.ToArray();
		lootPointParameters.ChanceModifier = ChanceModifier;
		lootPointParameters.Position = Vector3.zero;
		lootPointParameters.Rotation = Vector3.zero;
		lootPointParameters.IsContainer = true;
		lootPointParameters.IsStatic = true;
		lootPointParameters.useGravity = true;
		lootPointParameters.randomRotation = false;
		lootPointParameters.Id = Id;
		return lootPointParameters;
	}

	public void Init(_EB1E itemController)
	{
		ItemOwner = itemController;
	}

	protected override IEnumerator SmoothDoorOpenCoroutine(EDoorState state, bool isLocalInteraction, float speed = 1f)
	{
		if (_handle != null && state == EDoorState.Open)
		{
			StartCoroutine(_handle.OpenCoroutine());
			yield return new WaitForSeconds(_handle.OpenAnimation.keys.FirstOrDefault((Keyframe k) => k.value >= 1f).time);
		}
		PlaySound(state);
		float angle = GetAngle(state);
		float currentAngle = CurrentAngle;
		float num = 0f;
		float num2 = Mathf.DeltaAngle(CurrentAngle, angle);
		_shutPlayed = false;
		while (num < ProgressCurve.GetDuration())
		{
			float num3 = ProgressCurve.Evaluate(num);
			CurrentAngle = currentAngle + num3 * num2;
			base.transform.localPosition = ((state == EDoorState.Open) ? Vector3.Lerp(ClosedPosition, OpenPosition, num3) : Vector3.Lerp(ClosedPosition, OpenPosition, 1f - num3));
			num += Time.deltaTime * speed;
			if (state == EDoorState.Shut && !_shutPlayed && num > ProgressCurve.GetDuration() - ShutShift)
			{
				PlayShut();
			}
			yield return null;
		}
		if (!_shutPlayed && state == EDoorState.Shut)
		{
			PlayShut();
		}
		base.transform.localPosition = ((state == EDoorState.Open) ? OpenPosition : ClosedPosition);
		DoorState = state;
		CurrentAngle = angle;
	}
}
