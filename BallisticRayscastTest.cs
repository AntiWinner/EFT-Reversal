using UnityEngine;

[ExecuteInEditMode]
public class BallisticRayscastTest : MonoBehaviour
{
	[SerializeField]
	private Transform _point1;

	[SerializeField]
	private Transform _point2;

	[SerializeField]
	private float _forwardRadius = 0.005f;

	[SerializeField]
	private float _backwardRadius = 0.005f;

	[SerializeField]
	private int _forwardCount;

	private RaycastHit[] m__E000 = new RaycastHit[32];

	[SerializeField]
	private int _backwardCount;

	private RaycastHit[] m__E001 = new RaycastHit[32];

	[SerializeField]
	private bool _hitFound;

	private RaycastHit m__E002;

	private void Update()
	{
		if (!(_point1 == null) && !(_point2 == null))
		{
			_E002();
		}
	}

	private void _E000()
	{
		_forwardCount = _E003(_point1.position, _point2.position, this.m__E000);
		_backwardCount = _E003(_point2.position, _point1.position, this.m__E001);
	}

	private void _E001()
	{
		Physics.queriesHitBackfaces = true;
		_forwardCount = _E003(_point1.position, _point2.position, this.m__E000);
		Physics.queriesHitBackfaces = false;
	}

	private void _E002()
	{
		_hitFound = _E320.LinecastPrecise(_point2.position, _point1.position, out this.m__E002, _EC20.HitMask, reverseCheck: true, this.m__E001, (RaycastHit hit) => false);
	}

	private int _E003(Vector3 point1, Vector3 point2, RaycastHit[] hits)
	{
		Vector3 vector = point2 - point1;
		return Physics.RaycastNonAlloc(new Ray(point1, vector.normalized), hits, vector.magnitude, _EC20.HitMask);
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(_point1.position, 0.05f);
		_E004(this.m__E000, _forwardCount, _forwardRadius, Color.blue);
		_E004(this.m__E001, _backwardCount, _backwardRadius, Color.red);
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(_point2.position, 0.05f);
		Gizmos.color = Color.white;
		Gizmos.DrawLine(_point1.position, _point2.position);
		if (_hitFound)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(this.m__E002.point, _backwardRadius * 1.25f);
		}
	}

	private void _E004(RaycastHit[] hits, int count, float radius, Color color)
	{
		for (int i = 0; i < count; i++)
		{
			RaycastHit raycastHit = hits[i];
			Gizmos.color = color;
			Gizmos.DrawSphere(raycastHit.point, radius);
		}
	}
}
