using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.EnvironmentEffect;

[Obsolete]
[RequireComponent(typeof(Collider))]
public class EnvironmentSwitcherTrigger : MonoBehaviour, _E31B, IPhysicsTrigger
{
	[Header("Синяя стрелка (ось Z) должна указывать наружу помещения")]
	public float MaxDaylight = 1f;

	public bool ChangeHarmonics = true;

	public float FadeTime = 1f;

	public Color IndorColor = new Color(0.013f, 0.013f, 0.013f, 0f);

	[Space(15f)]
	public float SSAOIntensity = 1.5f;

	public int IndorShadowDistance = -1;

	[CompilerGenerated]
	private readonly string _E000 = _ED3E._E000(209132);

	public string Description
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public void OnTriggerEnter(Collider col)
	{
	}

	public void OnTriggerStay(Collider col)
	{
	}

	public void OnTriggerExit(Collider col)
	{
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		float num = 0.3f;
		Gizmos.color = Color.red;
		Gizmos.DrawLine(Vector3.zero, Vector3.zero + Vector3.forward * 2f * num);
		Gizmos.DrawLine(Vector3.zero + Vector3.forward * 2f * num, Vector3.zero + (Vector3.forward * 1.5f * num + Vector3.left * 0.5f * num));
		Gizmos.DrawLine(Vector3.zero + Vector3.forward * 2f * num, Vector3.zero + (Vector3.forward * 1.5f * num + Vector3.right * 0.5f * num));
		Gizmos.color = new Color(0f, 0.2f, 0.8f, 0.5f);
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
	}

	public void OnDrawGizmos()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.color = new Color(0.8f, 0.1f, 0f, 0.5f);
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
	}

	[SpecialName]
	bool IPhysicsTrigger.get_enabled()
	{
		return base.enabled;
	}

	[SpecialName]
	void IPhysicsTrigger.set_enabled(bool value)
	{
		base.enabled = value;
	}
}
