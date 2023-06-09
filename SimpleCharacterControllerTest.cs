using UnityEngine;

public class SimpleCharacterControllerTest : MonoBehaviour
{
	private SimpleCharacterController _E000;

	[SerializeField]
	private CapsuleCollider _mainCollider;

	private Vector3 _E001;

	public bool IsGrounded;

	private void Awake()
	{
		_E000 = base.gameObject.AddComponent<SimpleCharacterController>();
		_E000.Init(_mainCollider.transform, new Collider[1] { _mainCollider }, _mainCollider, EFTHardSettings.Instance.SIMPLE_CHARACTER_CONTROLLER_FIXED_DELTA_DISTANCE, 1, 0.25f, 60f);
	}

	private void Update()
	{
		Vector3 motion = base.transform.position - _E001;
		_E000.Move(motion, Time.deltaTime);
		base.transform.position = _E000.GetCollider().transform.position;
		_E001 = base.transform.position;
		IsGrounded = _E000.isGrounded;
	}
}
