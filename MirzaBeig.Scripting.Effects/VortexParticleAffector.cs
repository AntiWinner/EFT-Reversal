using UnityEngine;

namespace MirzaBeig.Scripting.Effects;

public class VortexParticleAffector : ParticleAffector
{
	private Vector3 _E014;

	[Header("Affector Controls")]
	public Vector3 axisOfRotationOffset = Vector3.zero;

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
		base.LateUpdate();
	}

	private void _E000()
	{
		_E014 = Quaternion.Euler(axisOfRotationOffset) * base.transform.up;
	}

	protected override void PerParticleSystemSetup()
	{
		_E000();
	}

	protected override Vector3 GetForce()
	{
		return Vector3.Normalize(Vector3.Cross(_E014, parameters.scaledDirectionToAffectorCenter));
	}

	protected override void OnDrawGizmosSelected()
	{
		if (base.enabled)
		{
			base.OnDrawGizmosSelected();
			Gizmos.color = Color.red;
			Vector3 vector;
			if (Application.isPlaying && base.enabled)
			{
				_E000();
				vector = _E014;
			}
			else
			{
				vector = Quaternion.Euler(axisOfRotationOffset) * base.transform.up;
			}
			Gizmos.DrawLine(base.transform.position + offset, base.transform.position + offset + vector * base.scaledRadius);
		}
	}
}
