using UnityEngine;

[ExecuteInEditMode]
public class FogEllipsoid : MonoBehaviour
{
	public enum Blend
	{
		Additive,
		Multiplicative
	}

	public Blend m_Blend;

	public float m_Density = 1f;

	[_E3FB(0f)]
	public float m_Radius = 1f;

	[_E3FB(0f)]
	public float m_Stretch = 2f;

	[Range(0f, 1f)]
	public float m_Feather = 0.7f;

	[Range(0f, 1f)]
	public float m_NoiseAmount;

	public float m_NoiseSpeed = 1f;

	[_E3FB(0f)]
	public float m_NoiseScale = 1f;

	private bool m__E000;

	private void _E000()
	{
		if (!this.m__E000)
		{
			this.m__E000 = LightManager<FogEllipsoid>.Add(this);
		}
	}

	private void OnEnable()
	{
		_E000();
	}

	private void Update()
	{
		_E000();
	}

	private void OnDisable()
	{
		LightManager<FogEllipsoid>.Remove(this);
		this.m__E000 = false;
	}

	private void OnDrawGizmosSelected()
	{
		Matrix4x4 identity = Matrix4x4.identity;
		Transform transform = base.transform;
		identity.SetTRS(transform.position, transform.rotation, new Vector3(1f, m_Stretch, 1f));
		Gizmos.matrix = identity;
		Gizmos.DrawWireSphere(Vector3.zero, m_Radius);
	}
}
