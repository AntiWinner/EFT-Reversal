using UnityEngine;

[ExecuteInEditMode]
public class RainFollow : MonoBehaviour
{
	[SerializeField]
	private Transform _target;

	[SerializeField]
	private float _offset = 10f;

	private Transform _E000;

	private void Start()
	{
		_E000 = GetComponent<Transform>();
	}

	private void Update()
	{
		_E000.position = _target.position + new Vector3(0f, _offset, 0f);
	}
}
