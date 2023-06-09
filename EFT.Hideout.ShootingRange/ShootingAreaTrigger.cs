using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.Hideout.ShootingRange;

public class ShootingAreaTrigger : MonoBehaviour, IPhysicsTrigger
{
	[SerializeField]
	private InteractiveShootingRange[] _targetControllers;

	[CompilerGenerated]
	private readonly string _E000 = _ED3E._E000(164619);

	public string Description
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public void OnTriggerEnter(Collider collider)
	{
		InteractiveShootingRange[] targetControllers = _targetControllers;
		for (int i = 0; i < targetControllers.Length; i++)
		{
			targetControllers[i].Enable();
		}
	}

	public void OnTriggerExit(Collider collider)
	{
		InteractiveShootingRange[] targetControllers = _targetControllers;
		for (int i = 0; i < targetControllers.Length; i++)
		{
			targetControllers[i].Disable();
		}
	}

	[SpecialName]
	bool IPhysicsTrigger.get_enabled()
	{
		return base.enabled;
	}

	[SpecialName]
	void IPhysicsTrigger.set_enabled(bool value)
	{
		base.enabled = value;
	}
}
