using UnityEngine;

public class DragTarget : MonoBehaviour
{
	private Transform _E000;

	private Camera _E001;

	private Vector3 _E002;

	private void Start()
	{
		_E000 = base.transform;
		_E001 = Camera.main;
	}

	private void OnMouseDown()
	{
		Vector2 vector = Input.mousePosition;
		float z = _E001.WorldToScreenPoint(_E000.position).z;
		Vector3 vector2 = _E001.ScreenToWorldPoint(new Vector3(vector.x, vector.y, z));
		_E002 = _E000.position - vector2;
	}

	private void OnMouseDrag()
	{
		Vector2 vector = Input.mousePosition;
		float z = _E001.WorldToScreenPoint(_E000.position).z;
		_E000.position = _E001.ScreenToWorldPoint(new Vector3(vector.x, vector.y, z)) + _E002;
	}
}
