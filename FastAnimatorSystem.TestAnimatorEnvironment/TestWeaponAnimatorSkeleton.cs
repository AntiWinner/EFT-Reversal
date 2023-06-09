using UnityEngine;

namespace FastAnimatorSystem.TestAnimatorEnvironment;

public class TestWeaponAnimatorSkeleton : MonoBehaviour
{
	public TextAsset fastAnimatorJson;

	private IAnimator _E000;

	public IAnimator Animator => _E000;

	public void Start()
	{
		_E4EB fastAnimatorController = _E508.Deserialize(fastAnimatorJson.bytes);
		_E000 = _E563.CreateAnimator(fastAnimatorController);
		_E000.SetBool(_ED3E._E000(50949), value: true);
		_E000.SetBool(_ED3E._E000(50996), value: true);
		_E000.SetBool(_ED3E._E000(51165), value: true);
		_E000.SetBool(_ED3E._E000(51095), value: true);
		_E000.SetFloat(_ED3E._E000(49291), 1f);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.U))
		{
			_E000.SetTrigger(_ED3E._E000(50004));
		}
		_E000.Update(Time.deltaTime);
	}
}
