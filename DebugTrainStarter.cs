using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT;
using EFT.MovingPlatforms;
using UnityEngine;

public class DebugTrainStarter : MonoBehaviour, IPhysicsTrigger
{
	public bool TrainComeAction = true;

	public GameObject GameObject;

	public GameObject TrainEffect;

	[CompilerGenerated]
	private readonly string _E000;

	public string Description
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	private void Awake()
	{
		GameObject.SetActive(TrainComeAction);
		base.gameObject.layer = LayerMask.NameToLayer(_ED3E._E000(25347));
	}

	public void OnTriggerEnter(Collider other)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(other);
		if (!(playerByCollider == null) && playerByCollider.AIData != null && !playerByCollider.AIData.IsAI)
		{
			_E307 instance = Singleton<_E307>.Instance;
			if (instance != null)
			{
				Debug.LogError(_ED3E._E000(25396) + TrainComeAction);
				Locomotive.ETravelState val = (TrainComeAction ? Locomotive.ETravelState.OnRouteToDestination : Locomotive.ETravelState.OnRouteBack);
				instance.TrainCome(val);
				TrainEffect.SetActive(!TrainEffect.activeInHierarchy);
			}
		}
	}

	public void OnTriggerExit(Collider col)
	{
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
