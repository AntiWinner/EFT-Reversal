using System;

namespace EFT.AssetsManager;

[Serializable]
public class PlayerRigidbodySleepHierarchy
{
	public EBodyPartColliderType BodyPart;

	public RigidbodySpawner RigidbodySpawner;

	public PlayerRigidbodySleepHierarchy Parent;

	public bool MustBeSleeping;

	public bool CanSleep
	{
		get
		{
			if (MustBeSleeping)
			{
				return true;
			}
			if (Parent == null || Parent.MustBeSleeping)
			{
				return RigidbodySpawner.Rigidbody.GetMassNormalizedKineticEnergy() < EFTHardSettings.Instance.CorpseEnergyToSleep;
			}
			return false;
		}
	}

	public bool TryPutToSleep()
	{
		if (CanSleep)
		{
			RigidbodySpawner.Rigidbody.Sleep();
			MustBeSleeping = true;
			return true;
		}
		return false;
	}

	public void Reset()
	{
		MustBeSleeping = false;
	}

	public void WakeUp()
	{
		RigidbodySpawner.Rigidbody.WakeUp();
		MustBeSleeping = false;
	}
}
