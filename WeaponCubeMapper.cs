using UnityEngine;

public class WeaponCubeMapper : CubeMapper
{
	private float _E004;

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(84576));

	private static readonly int _E006 = Shader.PropertyToID(_ED3E._E000(84625));

	protected override void Update()
	{
		float num = Vector3.Dot(-base.transform.up, -Vector3.up);
		Shader.SetGlobalFloat(_E005, num);
		Shader.SetGlobalFloat(_E006, num - _E004);
		_E004 = num;
		base.Update();
	}
}
