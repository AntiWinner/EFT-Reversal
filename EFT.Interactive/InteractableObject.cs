using EFT.AssetsManager;
using UnityEngine;

namespace EFT.Interactive;

public abstract class InteractableObject : PoolSafeMonoBehaviour, _E2B8, _E633
{
	public ESpecificInteractionContext specificInteractionContext;

	private float _E02A;

	public float InteractionDot;

	public Vector3 InteractionDirection;

	private BetterSource _E02B;

	public virtual Transform TrackableTransform => base.transform;

	public float StateUpdateTime => _E02A;

	public Vector3 WorldInteractionDirection => -base.transform.TransformDirection(InteractionDirection);

	protected void SetStateUpdateTime()
	{
		_E02A = Time.time;
	}

	public bool InteractsFromAppropriateDirection(Vector3 playerForward)
	{
		if (InteractionDirection.sqrMagnitude < 0.001f)
		{
			return true;
		}
		return Vector3.Dot(playerForward, WorldInteractionDirection) > InteractionDot;
	}

	public virtual void Kill()
	{
	}

	public virtual void OnDrawGizmosSelected()
	{
		if (InteractionDirection.sqrMagnitude > 0f)
		{
			Vector3 worldInteractionDirection = WorldInteractionDirection;
			Debug.DrawLine(base.transform.position - worldInteractionDirection, base.transform.position, Color.magenta, Time.deltaTime);
		}
	}
}
