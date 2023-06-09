using UnityEngine;

namespace MirzaBeig.Scripting.Effects;

[AddComponentMenu("Effects/Particle Force Fields/Attraction Particle Force Field")]
public class AttractionParticleForceField : ParticleForceField
{
	[Tooltip("Tether force based on linear inverse particle distance to force field center.")]
	[Header("ForceField Controls")]
	public float arrivalRadius = 1f;

	[Tooltip("Dead zone from force field center in which no additional force is applied.")]
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
		if (!(parameters.distanceToForceFieldCenterSqr < _E001))
		{
			if (parameters.distanceToForceFieldCenterSqr < _E000)
			{
				float num = 1f - parameters.distanceToForceFieldCenterSqr / _E000;
				return Vector3.Normalize(parameters.scaledDirectionToForceFieldCenter) * num;
			}
			return Vector3.Normalize(parameters.scaledDirectionToForceFieldCenter);
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
			Vector3 vector = base.transform.position + center;
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(vector, num);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(vector, num2);
		}
	}
}
