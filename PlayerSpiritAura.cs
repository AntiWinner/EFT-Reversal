using UnityEngine;

public class PlayerSpiritAura : MonoBehaviour
{
	[SerializeField]
	private BoxCollider _collider;

	[SerializeField]
	private float _boundsExpand = 0.3f;

	public void UpdateBounds(Bounds localBounds)
	{
		localBounds.Expand(_boundsExpand);
		_collider.center = localBounds.center;
		_collider.size = localBounds.size;
	}

	public Collider GetCollider()
	{
		return _collider;
	}
}
