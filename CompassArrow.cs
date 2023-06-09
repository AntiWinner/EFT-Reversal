using UnityEngine;

public class CompassArrow : MonoBehaviour
{
	[SerializeField]
	private BetterSpring _spring = new BetterSpring();

	public Vector3 NorthDirection = Vector3.forward;

	private Transform _E000;

	private float _E001;

	private float _E002;

	private void Start()
	{
		_E000 = base.transform;
		_spring.Cache();
		_spring.EquilibriumPos = Vector3.zero;
		_spring.ApplyPosition(_spring.EquilibriumPos);
	}

	private void Update()
	{
		Quaternion quaternion = Quaternion.LookRotation(_E000.parent.forward, -NorthDirection);
		Quaternion quaternion2 = Quaternion.Inverse(base.transform.parent.rotation) * quaternion;
		Vector3 eulerAngles = quaternion2.eulerAngles;
		_E002 = quaternion2.eulerAngles.z;
		float num = _E001 - quaternion2.eulerAngles.z;
		num = ((num > 180f) ? (num - 360f) : ((num < -180f) ? (num + 360f) : num));
		_spring.ApplyPosition(new Vector3(0f, 0f, num));
		_spring.Process(Time.deltaTime);
		_E001 = (eulerAngles + _spring.Value).z;
		base.transform.localRotation = Quaternion.Euler(eulerAngles + _spring.Value);
	}

	public int PanelValue()
	{
		int num = 180 - (int)_E002;
		if (num >= 0)
		{
			return num;
		}
		return num + 360;
	}
}
