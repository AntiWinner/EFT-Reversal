using UnityEngine;

public class Trigger : MonoBehaviour
{
	[SerializeField]
	private float _impactForceThreshold;

	private float _E000 => _impactForceThreshold * _impactForceThreshold;

	public void TriggerDestruction(Vector3 triggerPosition, float magnitude)
	{
		BoxFracture[] components = base.gameObject.GetComponents<BoxFracture>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].Destroy(triggerPosition, magnitude);
		}
	}

	private void OnCollisionEnter(Collision c)
	{
		float sqrMagnitude = c.relativeVelocity.sqrMagnitude;
		if (!(sqrMagnitude < _E000))
		{
			TriggerDestruction(c.contacts[0].point, sqrMagnitude * 0.1f / _E000);
		}
	}
}
