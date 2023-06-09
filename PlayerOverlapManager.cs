using UnityEngine;

public class PlayerOverlapManager : MonoBehaviour
{
	public enum EExtrusionCollider
	{
		Default,
		Prone
	}

	[SerializeField]
	private ColliderExtrusion _headColliderExtrusion;

	[SerializeField]
	private ColliderExtrusion _cameraColliderExtrusion;

	[SerializeField]
	private Collider[] _headColliders;

	[SerializeField]
	private Transform _proneColliderPivot;

	private Vector3 m__E000;

	private EExtrusionCollider _E001;

	public Collider Collider => _headColliderExtrusion.Collider;

	public void Init(Collider playerCollider)
	{
		_headColliderExtrusion.Init(_E37B.PlayerCollisionTestMask);
		_headColliderExtrusion.AddIgnoredCollider(playerCollider);
		_cameraColliderExtrusion.Init(_E37B.PlayerCollisionTestMask);
		_cameraColliderExtrusion.AddIgnoredCollider(playerCollider);
		Off();
	}

	public void IgnoreCollider(Collider collider, bool ignore = true)
	{
		if (ignore)
		{
			_headColliderExtrusion.AddIgnoredCollider(collider);
		}
		else
		{
			_headColliderExtrusion.RemoveIgnoredCollider(collider);
		}
		if (ignore)
		{
			_cameraColliderExtrusion.AddIgnoredCollider(collider);
		}
		else
		{
			_cameraColliderExtrusion.RemoveIgnoredCollider(collider);
		}
	}

	public void SetCollider(EExtrusionCollider colliderIndex)
	{
		_headColliderExtrusion.SetCollider(_headColliders[(int)colliderIndex]);
		_E001 = colliderIndex;
	}

	public void On()
	{
		Collider[] headColliders = _headColliders;
		for (int i = 0; i < headColliders.Length; i++)
		{
			headColliders[i].enabled = true;
		}
		_cameraColliderExtrusion.Collider.enabled = true;
		_E38B.SetLayersRecursively(_cameraColliderExtrusion.gameObject, _E37B.PlayerCollisionTestLayer);
		headColliders = _headColliders;
		for (int i = 0; i < headColliders.Length; i++)
		{
			_E38B.SetLayersRecursively(headColliders[i].gameObject, _E37B.PlayerCollisionTestLayer);
		}
		_headColliderExtrusion.gameObject.SetActive(value: true);
	}

	public Vector3 GetOffsetXZ()
	{
		return new Vector3(this.m__E000.x, 0f, this.m__E000.z);
	}

	public Vector3 GetOffset()
	{
		return this.m__E000;
	}

	public void Off()
	{
		Collider[] headColliders = _headColliders;
		for (int i = 0; i < headColliders.Length; i++)
		{
			headColliders[i].enabled = false;
		}
		_cameraColliderExtrusion.Collider.enabled = false;
	}

	public void Calculate(Vector3 motion)
	{
		if (_E001 == EExtrusionCollider.Prone)
		{
			_E000();
		}
		_headColliderExtrusion.CalculateThroughMotion(motion);
		this.m__E000 = _headColliderExtrusion.GetDepenetration();
	}

	private void _E000()
	{
		Vector3 localPosition = _headColliderExtrusion.Collider.transform.parent.InverseTransformPoint(_proneColliderPivot.position);
		localPosition.y = Mathf.Min(localPosition.y, 0.2f);
		_headColliderExtrusion.Collider.transform.localPosition = localPosition;
	}

	public void Calculate(Vector3 motion, Quaternion rotation)
	{
		if (_E001 == EExtrusionCollider.Prone)
		{
			_E000();
		}
		Transform transform = Collider.transform;
		_headColliderExtrusion.Calculate(transform.position + motion, rotation);
		this.m__E000 = _headColliderExtrusion.GetDepenetration();
	}

	public void ExtrudeCamera()
	{
		_cameraColliderExtrusion.Calculate();
		Vector3 depenetration = _cameraColliderExtrusion.GetDepenetration();
		_cameraColliderExtrusion.transform.position += depenetration;
	}
}
