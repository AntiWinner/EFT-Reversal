using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;

namespace EFT.Hideout;

public sealed class HideoutAreaTrigger : MonoBehaviour, IPhysicsTrigger
{
	[SerializeField]
	private List<HideoutInteractionTrigger> _levelTriggers;

	private HideoutArea _E000;

	[CompilerGenerated]
	private readonly string _E001 = _ED3E._E000(171201);

	public string Description
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public void Init(HideoutArea area)
	{
		_E000 = area;
		foreach (HideoutInteractionTrigger levelTrigger in _levelTriggers)
		{
			levelTrigger.Init(_E000);
		}
	}

	public void OnTriggerEnter(Collider col)
	{
		if (!(_E000 == null))
		{
			Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
			if (!(playerByCollider == null))
			{
				_E000.Data?.Template.AreaBehaviour.OnEnterLocation(playerByCollider);
			}
		}
	}

	public void OnTriggerExit(Collider col)
	{
		if (!(_E000 == null) && !(Singleton<GameWorld>.Instance.GetPlayerByCollider(col) == null))
		{
			_E000.Data.Template.AreaBehaviour.OnExitLocation();
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
