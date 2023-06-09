using UnityEngine;

public class AGSMachineryBones : MonoBehaviour
{
	[SerializeField]
	private Transform _target;

	[SerializeField]
	private Transform _lever;

	[SerializeField]
	private Transform _verticalPart;

	[SerializeField]
	private Transform _scope;

	public Transform Target => _target;

	public Transform Lever => _lever;

	public Transform VerticalPart => _verticalPart;

	public Transform Scope => _scope;
}
