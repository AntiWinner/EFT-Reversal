using System.Collections.Generic;
using EFT.Interactive;

namespace EFT.Hideout.ShootingRange;

public abstract class InteractiveShootingRange : InteractableObject
{
	protected bool _enabled;

	public virtual void Enable()
	{
		_enabled = true;
	}

	public virtual void Disable()
	{
		_enabled = false;
	}

	public virtual _EC3F InteractionStates(HideoutPlayerOwner owner)
	{
		return new _EC3F
		{
			Actions = new List<_EC3E>()
		};
	}
}
