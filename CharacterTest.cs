using UnityEngine;

public class CharacterTest : MonoBehaviour
{
	public Transform DestPoint;

	private CharacterController _E000;

	private void Start()
	{
		_E000 = GetComponent<CharacterController>();
	}

	private void Update()
	{
		_E000.Move((DestPoint.position - base.transform.position).normalized);
	}
}
