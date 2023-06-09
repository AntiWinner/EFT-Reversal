using System.Runtime.CompilerServices;
using UnityEngine;

public class MineDirectionalColliders : MonoBehaviour, IPhysicsTrigger
{
	private MineDirectional _E000;

	public string Description => _ED3E._E000(55513);

	private void Awake()
	{
		_E000 = GetComponentInParent<MineDirectional>();
		if (!(_E000 != null))
		{
			Debug.LogError(base.gameObject.name + _ED3E._E000(55474));
			Object.Destroy(this);
		}
	}

	public void OnTriggerEnter(Collider collider)
	{
		_E000.OnTriggerEnter(collider);
	}

	public void OnTriggerExit(Collider collider)
	{
		_E000.OnTriggerExit(collider);
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
