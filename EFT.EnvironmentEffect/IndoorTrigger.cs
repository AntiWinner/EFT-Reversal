using JetBrains.Annotations;
using UnityEngine;

namespace EFT.EnvironmentEffect;

public class IndoorTrigger : MonoBehaviour, EnvironmentManagerBase._E000
{
	[SerializeField]
	public bool IsBunker;

	[SerializeField]
	public float FadeTime = 1f;

	[SerializeField]
	[Space(15f)]
	public float ExposureSpeed = 4f;

	[SerializeField]
	public float ExposureOffset = 0.14f;

	[SerializeField]
	public float RainVolume = 0.7f;

	[SerializeField]
	public Bounds Bounds;

	private static int _E000;

	public int AreaAutoId;

	private void OnValidate()
	{
		if (!Application.isPlaying)
		{
			Awake();
		}
	}

	private void Awake()
	{
		if (Bounds.size.magnitude < Mathf.Epsilon)
		{
			Reinit();
		}
	}

	public void Reinit()
	{
		float num = 0.5f;
		Vector3 vector = base.transform.TransformPoint(new Vector3(num, num, num));
		Vector3 vector2 = base.transform.TransformPoint(new Vector3(0f - num, 0f - num, 0f - num));
		Vector3 vector3 = base.transform.TransformPoint(new Vector3(0f - num, num, num));
		Vector3 vector4 = base.transform.TransformPoint(new Vector3(num, 0f - num, num));
		Vector3 vector5 = base.transform.TransformPoint(new Vector3(num, num, 0f - num));
		Vector3 vector6 = base.transform.TransformPoint(new Vector3(num, 0f - num, 0f - num));
		Vector3 vector7 = base.transform.TransformPoint(new Vector3(0f - num, 0f - num, num));
		Vector3 vector8 = base.transform.TransformPoint(new Vector3(0f - num, num, 0f - num));
		Vector3 vector9 = new Vector3(Mathf.Min(vector.x, vector2.x, vector3.x, vector4.x, vector5.x, vector6.x, vector7.x, vector8.x), Mathf.Min(vector.y, vector2.y, vector3.y, vector4.y, vector5.y, vector6.y, vector7.y, vector8.y), Mathf.Min(vector.z, vector2.z, vector3.z, vector4.z, vector5.z, vector6.z, vector7.z, vector8.z));
		Vector3 size = new Vector3(Mathf.Max(vector.x, vector2.x, vector3.x, vector4.x, vector5.x, vector6.x, vector7.x, vector8.x), Mathf.Max(vector.y, vector2.y, vector3.y, vector4.y, vector5.y, vector6.y, vector7.y, vector8.y), Mathf.Max(vector.z, vector2.z, vector3.z, vector4.z, vector5.z, vector6.z, vector7.z, vector8.z)) - vector9;
		Bounds = new Bounds(base.transform.position, size);
	}

	[CanBeNull]
	public IndoorTrigger Check(Vector3 pos)
	{
		if (!Bounds.Contains(pos))
		{
			return null;
		}
		Vector3 vector = base.transform.InverseTransformPoint(pos);
		if (!(Mathf.Abs(vector.x) < 0.5f) || !(Mathf.Abs(vector.y) < 0.5f) || !(Mathf.Abs(vector.z) < 0.5f))
		{
			return null;
		}
		return this;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.color = new Color(0f, 0.2f, 0.4f, 0.4f);
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
		Gizmos.color = new Color(0f, 0.5f, 0f, 0.9f);
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}

	private void OnDrawGizmos()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.color = new Color(0f, 0.2f, 0.4f, 0.3f);
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
	}
}
