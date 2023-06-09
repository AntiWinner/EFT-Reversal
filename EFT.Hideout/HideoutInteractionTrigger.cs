using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.Interactive;
using UnityEngine;

namespace EFT.Hideout;

public sealed class HideoutInteractionTrigger : InteractableObject, _E812, IPhysicsTrigger
{
	[CompilerGenerated]
	private HideoutArea _E000;

	[CompilerGenerated]
	private readonly string _E001 = _ED3E._E000(171237);

	public HideoutArea Area
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
		[CompilerGenerated]
		private set
		{
			_E000 = value;
		}
	}

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
		Area = area;
	}

	public void OnTriggerEnter(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (!(playerByCollider == null))
		{
			HideoutPlayerOwner component = playerByCollider.GetComponent<HideoutPlayerOwner>();
			if (!(component == null))
			{
				component.HideoutAreaInteraction(Area, inSight: true);
			}
		}
	}

	public void OnTriggerExit(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (!(playerByCollider == null))
		{
			HideoutPlayerOwner component = playerByCollider.GetComponent<HideoutPlayerOwner>();
			if (!(component == null))
			{
				component.HideoutAreaInteraction(Area, inSight: false);
			}
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
