using System.Collections.Generic;
using System.Threading.Tasks;
using Comfort.Common;
using EFT;
using EFT.EnvironmentEffect;
using JetBrains.Annotations;
using UnityEngine;

public class EnvironmentManagerBase : MonoBehaviour
{
	public interface _E000
	{
		void Reinit();

		IndoorTrigger Check(Vector3 pos);
	}

	protected Dictionary<int, IndoorTrigger> _cachedPlayerTrigger = new Dictionary<int, IndoorTrigger>();

	protected TriggerGroup _rootTriggerGroup;

	private bool m__E000;

	private bool m__E001 = true;

	protected static EnvironmentManagerBase _instance;

	public static EnvironmentManagerBase Instance => _instance;

	private void Awake()
	{
		Init();
	}

	protected virtual void Init()
	{
		_instance = this;
		InitIndoors();
		_E000().HandleExceptions();
		this.m__E000 = _rootTriggerGroup != null;
		this.m__E001 = true;
	}

	protected void InitIndoors()
	{
		_rootTriggerGroup = base.gameObject.GetComponentInChildren<TriggerGroup>();
	}

	private async Task _E000()
	{
		while (this.m__E001)
		{
			if (Singleton<GameWorld>.Instance == null)
			{
				await Task.Yield();
				continue;
			}
			List<Player> allPlayers = Singleton<GameWorld>.Instance.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				Player player = allPlayers[i];
				if (!(player == null))
				{
					Collider collider = player.CharacterControllerCommon.GetCollider();
					IndoorTrigger indoorTrigger = _E001(collider.bounds.center);
					_cachedPlayerTrigger.TryGetValue(player.Id, out var value);
					if (value == indoorTrigger)
					{
						await Task.Yield();
						continue;
					}
					_cachedPlayerTrigger[player.Id] = indoorTrigger;
					SetTriggerForPlayer(player, indoorTrigger);
					await Task.Yield();
				}
			}
			await Task.Yield();
		}
	}

	[CanBeNull]
	private IndoorTrigger _E001(Vector3 pos)
	{
		if (!this.m__E000)
		{
			return null;
		}
		return _rootTriggerGroup.Check(pos);
	}

	public int TryFindEnvironmentIdByPos(Vector3 pos)
	{
		if (!this.m__E000)
		{
			return 0;
		}
		IndoorTrigger indoorTrigger = _rootTriggerGroup.Check(pos);
		if (indoorTrigger == null)
		{
			return 0;
		}
		return indoorTrigger.AreaAutoId;
	}

	protected virtual void SetTriggerForPlayer(Player player, IndoorTrigger trigger)
	{
		player.AIData.SetEnvironment(trigger);
	}

	public void Stop()
	{
		this.m__E001 = false;
	}
}
