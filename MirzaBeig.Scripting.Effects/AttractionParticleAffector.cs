using UnityEngine;

namespace MirzaBeig.Scripting.Effects;

public class AttractionParticleAffector : ParticleAffector
{
	[Header("Affector Controls")]
	public float arrivalRadius = 1f;

	public float arrivedRadius = 0.5f;

	private float _E000;

	private float _E001;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void LateUpdate()
	{
		float x = base.transform.lossyScale.x;
		_E000 = arrivalRadius * arrivalRadius * x;
		_E001 = arrivedRadius * arrivedRadius * x;
		base.LateUpdate();
	}

	protected override Vector3 GetForce()
	{
		if (!(parameters.distanceToAffectorCenterSqr < _E001))
		{
			if (parameters.distanceToAffectorCenterSqr < _E000)
			{
				float num = 1f - parameters.distanceToAffectorCenterSqr / _E000;
				return Vector3.Normalize(parameters.scaledDirectionToAffectorCenter) * num;
			}
			return Vector3.Normalize(parameters.scaledDirectionToAffectorCenter);
		}
		Vector3 result = default(Vector3);
		result.x = 0f;
		result.y = 0f;
		result.z = 0f;
		return result;
	}

	protected override void OnDrawGizmosSelected()
	{
		if (base.enabled)
		{
			base.OnDrawGizmosSelected();
			float x = base.transform.lossyScale.x;
			float num = arrivalRadius * x;
			float num2 = arrivedRadius * x;
			Vector3 center = base.transform.position + offset;
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(center, num);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(center, num2);
		}
	}
}
