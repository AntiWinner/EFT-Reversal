using UnityEngine;

public class ParticlePlexusDemo_CameraRig : MonoBehaviour
{
	public Transform pivot;

	private Vector3 _E000;

	[Range(0f, 90f)]
	public float rotationLimit = 90f;

	public float rotationSpeed = 2f;

	public float rotationLerpSpeed = 4f;

	private Vector3 _E001;

	private void Start()
	{
		_E001 = pivot.localEulerAngles;
		_E000 = _E001;
	}

	private void Update()
	{
		float axis = Input.GetAxis(_ED3E._E000(23415));
		float axis2 = Input.GetAxis(_ED3E._E000(23422));
		if (Input.GetKeyDown(KeyCode.R))
		{
			_E000 = _E001;
		}
		axis *= rotationSpeed;
		axis2 *= rotationSpeed;
		_E000.y += axis;
		_E000.x += axis2;
		_E000.x = Mathf.Clamp(_E000.x, 0f - rotationLimit, rotationLimit);
		_E000.y = Mathf.Clamp(_E000.y, 0f - rotationLimit, rotationLimit);
		Vector3 localEulerAngles = pivot.localEulerAngles;
		localEulerAngles.x = Mathf.LerpAngle(localEulerAngles.x, _E000.x, Time.deltaTime * rotationLerpSpeed);
		localEulerAngles.y = Mathf.LerpAngle(localEulerAngles.y, _E000.y, Time.deltaTime * rotationLerpSpeed);
		pivot.localEulerAngles = localEulerAngles;
	}
}
