using UnityEngine;

namespace EFT;

public sealed class GameWorldUnityTickListener : MonoBehaviour
{
	private GameWorld _E000;

	public static GameWorldUnityTickListener Create(GameObject gameObject, GameWorld gameWorld)
	{
		GameWorldUnityTickListener gameWorldUnityTickListener = gameObject.AddComponent<GameWorldUnityTickListener>();
		gameWorldUnityTickListener._E000 = gameWorld;
		return gameWorldUnityTickListener;
	}

	public void Update()
	{
		if (!(_E8A8.Instance.Camera == null))
		{
			if (_E000.UpdateQueue == EUpdateQueue.Update)
			{
				_E000.DoWorldTick(_E000.DeltaTime);
			}
			else
			{
				_E000.DoOtherWorldTick(_E000.DeltaTime);
			}
		}
	}

	public void FixedUpdate()
	{
		if (!(_E8A8.Instance.Camera == null))
		{
			if (_E000.UpdateQueue == EUpdateQueue.FixedUpdate)
			{
				_E000.DoWorldTick(Time.fixedDeltaTime);
			}
			else
			{
				_E000.DoOtherWorldTick(Time.fixedDeltaTime);
			}
		}
	}
}
