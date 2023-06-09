using UnityEngine;

public class ExtendedFlycam : MonoBehaviour
{
	public float cameraSensitivity = 90f;

	public float climbSpeed = 4f;

	public float normalMoveSpeed = 10f;

	public float slowMoveFactor = 0.25f;

	public float fastMoveFactor = 3f;

	private float _E000;

	private float _E001;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update()
	{
		if (Cursor.lockState != 0)
		{
			_E000 += Input.GetAxis(_ED3E._E000(23374)) * cameraSensitivity * Time.deltaTime;
			_E001 += Input.GetAxis(_ED3E._E000(23366)) * cameraSensitivity * Time.deltaTime;
		}
		_E001 = Mathf.Clamp(_E001, -90f, 90f);
		Quaternion b = Quaternion.AngleAxis(_E000, Vector3.up);
		b *= Quaternion.AngleAxis(_E001, Vector3.left);
		base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, b, Time.deltaTime * 5f);
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			base.transform.position += base.transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis(_ED3E._E000(23422)) * Time.deltaTime;
			base.transform.position += base.transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis(_ED3E._E000(23415)) * Time.deltaTime;
			if (Input.GetKey(KeyCode.Q))
			{
				base.transform.position += Vector3.up * climbSpeed * fastMoveFactor * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.E))
			{
				base.transform.position -= Vector3.up * climbSpeed * fastMoveFactor * Time.deltaTime;
			}
		}
		else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
		{
			base.transform.position += base.transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis(_ED3E._E000(23422)) * Time.deltaTime;
			base.transform.position += base.transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis(_ED3E._E000(23415)) * Time.deltaTime;
			if (Input.GetKey(KeyCode.Q))
			{
				base.transform.position += Vector3.up * climbSpeed * slowMoveFactor * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.E))
			{
				base.transform.position -= Vector3.up * climbSpeed * slowMoveFactor * Time.deltaTime;
			}
		}
		else
		{
			base.transform.position += base.transform.forward * normalMoveSpeed * Input.GetAxis(_ED3E._E000(23422)) * Time.deltaTime;
			base.transform.position += base.transform.right * normalMoveSpeed * Input.GetAxis(_ED3E._E000(23415)) * Time.deltaTime;
			if (Input.GetKey(KeyCode.Q))
			{
				base.transform.position += Vector3.up * climbSpeed * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.E))
			{
				base.transform.position -= Vector3.up * climbSpeed * Time.deltaTime;
			}
		}
		if (Input.GetKeyDown(KeyCode.End) || Input.GetKeyDown(KeyCode.Escape))
		{
			if (Cursor.lockState == CursorLockMode.None)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			else
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
	}
}
