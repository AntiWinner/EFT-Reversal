using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderExtrusion : MonoBehaviour
{
	[SerializeField]
	private Collider _collider;

	[SerializeField]
	private float _castHalo;

	[SerializeField]
	private LayerMask _castMask;

	private const int m__E000 = 16;

	private Collider[] m__E001 = new Collider[16];

	private int _E002;

	private float _E003;

	private Vector3 _E004;

	private WaitForEndOfFrame _E005 = new WaitForEndOfFrame();

	private HashSet<Collider> _E006 = new HashSet<Collider>();

	public Collider Collider => _collider;

	public void Init(LayerMask castMask)
	{
		_castMask = castMask;
	}

	public void SetCollider(Collider newCollider)
	{
		if (newCollider != null)
		{
			_collider = newCollider;
		}
	}

	private void _E000()
	{
		Bounds bounds = _collider.bounds;
		_E003 = Mathf.Max(Mathf.Max(bounds.extents.x, bounds.extents.y), bounds.extents.z) + _castHalo;
	}

	public void AddIgnoredCollider(Collider collider)
	{
		_E006.Add(collider);
	}

	public void RemoveIgnoredCollider(Collider collider)
	{
		_E006.Remove(collider);
	}

	private bool _E001(Collider collider)
	{
		if (!(collider == _collider))
		{
			return _E006.Contains(collider);
		}
		return true;
	}

	public void Calculate()
	{
		Transform transform = _collider.transform;
		Calculate(transform.position, transform.rotation);
	}

	public void CalculateThroughMotion(Vector3 motion)
	{
		Transform transform = _collider.transform;
		Calculate(transform.position + motion, transform.rotation);
	}

	public IEnumerator CalculateCoroutine(Vector3 position)
	{
		Vector3 vector = position;
		RefreshNeighbours(vector, Vector3.zero);
		int num = 0;
		for (int i = 0; i < _E002; i++)
		{
			Collider collider = this.m__E001[i];
			if (!_E001(collider))
			{
				if (Physics.ComputePenetration(_collider, vector, _collider.transform.rotation, collider, collider.transform.position, collider.transform.rotation, out var direction, out var distance))
				{
					vector += direction * (distance + 0.001f);
					num++;
				}
				yield return _E005;
			}
		}
		if (num > 0)
		{
			_E004 = vector - position;
		}
		else
		{
			_E004 = Vector3.zero;
		}
	}

	public void Calculate(Vector3 position, Quaternion rotation, float depenetrationModification = 0.001f)
	{
		RefreshNeighbours(position, Vector3.zero);
		_E004 = CalculateDepenetration(position, rotation, depenetrationModification);
	}

	public void RefreshNeighbours(Vector3 position, Vector3 motion)
	{
		_E000();
		Vector3 position2 = position + (_collider.bounds.center - _collider.transform.position) + motion / 2f;
		_E002 = Physics.OverlapSphereNonAlloc(position2, _E003 + motion.magnitude / 2f, this.m__E001, _castMask, QueryTriggerInteraction.Ignore);
	}

	public Vector3 CalculateDepenetration(Vector3 position, Quaternion rotation, float depenetrationModification = 0.001f)
	{
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < _E002; i++)
		{
			Collider collider = this.m__E001[i];
			if (!_E001(collider) && Physics.ComputePenetration(_collider, position, rotation, collider, collider.transform.position, collider.transform.rotation, out var direction, out var distance))
			{
				zero += direction * (distance + depenetrationModification);
			}
		}
		return zero;
	}

	public Vector3 GetDepenetration()
	{
		return _E004;
	}
}
