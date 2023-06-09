using UnityEngine;

public class FloatingObject : UpdateInEditorSystemComponent<FloatingObject>
{
	[SerializeField]
	private float _angle = 10f;

	[SerializeField]
	private Vector3 _buoyancy = new Vector3(0.1f, 0.1f, 0.1f);

	[SerializeField]
	private float _angleAmplitude = 1f;

	[SerializeField]
	private float _buoyancyAmplitude = 1f;

	private Vector3 m__E000;

	private Quaternion _E001;

	private float _E002;

	private float _E003;

	private float _E004;

	private void Start()
	{
		_E004 = Random.Range(0.1f, 10f);
		this.m__E000 = base.gameObject.transform.position;
		_E001 = base.gameObject.transform.rotation;
	}

	public override void ManualUpdate(float dt)
	{
		float num = _E004 + Time.time;
		_E002 = num * _angleAmplitude;
		_E003 = num * _buoyancyAmplitude;
		_E000(_E003, _E002);
	}

	private void _E000(float positionFloatingTime, float rotationFloatingTime)
	{
		float num = Mathf.Sin(positionFloatingTime);
		float num2 = Mathf.Cos(positionFloatingTime);
		float x = this.m__E000.x + num * num2 * _buoyancy.x;
		float y = this.m__E000.y + num * _buoyancy.y;
		float z = this.m__E000.z + num2 * _buoyancy.z;
		float num3 = Mathf.Sin(rotationFloatingTime);
		float num4 = Mathf.Cos(rotationFloatingTime);
		float x2 = num3 * num4 * _angle;
		float z2 = num4 * _angle;
		base.transform.SetPositionAndRotation(new Vector3(x, y, z), Quaternion.Euler(x2, 0f, z2) * _E001);
	}
}
