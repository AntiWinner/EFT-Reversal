#define CLIENT
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.AssetsManager;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.Ballistics;

public class BallisticsCalculator : MonoBehaviour, _EC1E
{
	private struct _E000
	{
		public readonly float SimulationTime;

		public readonly _EC26 Shot;

		public _E000(float simulationTime, _EC26 shot)
		{
			SimulationTime = simulationTime;
			Shot = shot;
		}
	}

	public const int RND_COUNT = 512;

	private int m__E000;

	public List<_EC26> Shots = new List<_EC26>(64);

	private List<_EC26> m__E001 = new List<_EC26>(64);

	private readonly Queue<_E000> m__E002 = new Queue<_E000>(64);

	private _E5CD m__E003;

	private readonly List<_EC1F> m__E004 = new List<_EC1F>(64);

	[CompilerGenerated]
	private _EC17 _E005;

	private bool _E006;

	private Predicate<_EC26> _E007;

	private static BallisticCollider _E008;

	public _EC17 Randoms
	{
		[CompilerGenerated]
		get
		{
			return _E005;
		}
		[CompilerGenerated]
		private set
		{
			_E005 = value;
		}
	}

	public static BallisticCollider DefaultHitBody
	{
		get
		{
			if (_E008 == null)
			{
				GameObject obj = Singleton<_ED0A>.Instance.InstantiateAsset<GameObject>(_ED3E._E000(109625));
				obj.transform.position = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
				_E008 = obj.GetComponent<BallisticCollider>();
			}
			return _E008;
		}
	}

	int _EC1E.ActiveShotsCount => Shots.Count;

	BallisticCollider _EC1E.DefaultHitBody => DefaultHitBody;

	public static BallisticsCalculator Create(GameObject gameObject, int seed, _E5CD shotCallback, bool showDebug = true)
	{
		BallisticsCalculator ballisticsCalculator = gameObject.AddComponent<BallisticsCalculator>();
		ballisticsCalculator.Randoms = new _EC17(512, seed);
		ballisticsCalculator.m__E003 = shotCallback;
		ballisticsCalculator._E006 = showDebug;
		ballisticsCalculator._E007 = ballisticsCalculator._E001;
		_ = DefaultHitBody;
		return ballisticsCalculator;
	}

	_EC26 _EC1E.Shoot(_EA12 ammo, Vector3 shotPosition, Vector3 shotDirection, Player player, Item item, float speedFactor, int fragmentIndex)
	{
		_EC26 obj = CreateShot(ammo, shotPosition, shotDirection, this.m__E000++, player, item, speedFactor, fragmentIndex);
		Shoot(obj);
		return obj;
	}

	_EC26 _EC1E.Shoot(_EA12 ammo, Vector3 shotPosition, Vector3 shotDirection, int fireIndex, Player player, Item item, float speedFactor, int fragmentIndex)
	{
		_EC26 obj = CreateShot(ammo, shotPosition, shotDirection, fireIndex, player, item, speedFactor, fragmentIndex);
		Shoot(obj);
		return obj;
	}

	void _EC1E.ShotMultiProjectileShot(_EA12 ammo, Vector3 shotPosition, Vector3 shotDirection, float speedFactor, List<_EC26> preAllocatedShots, Player player, Item item)
	{
		preAllocatedShots.Clear();
		CreateMultiProjectileShot(ammo, shotPosition, shotDirection, speedFactor, preAllocatedShots, player, item);
		for (int i = 0; i < preAllocatedShots.Count; i++)
		{
			Shoot(preAllocatedShots[i]);
		}
	}

	_EC26 _EC1E.GetActiveShot(int shotsIndex)
	{
		return Shots[shotsIndex];
	}

	public _EC26 CreateShot(_EA12 ammo, Vector3 origin, Vector3 direction, int fireIndex, Player player, Item weapon, float speedFactor = 1f, int fragmentIndex = 0)
	{
		int num = UnityEngine.Random.Range(0, 512);
		float num2 = ammo.InitialSpeed * speedFactor;
		return _EC26.Create(ammo, fragmentIndex, num, origin, direction, num2, num2, ammo.BulletMassGram, ammo.BulletDiameterMilimeters, ammo.Damage, GetAmmoPenetrationPower(ammo, num, Randoms), ammo.PenetrationChance, ammo.RicochetChance, ammo.FragmentationChance, 1f, ammo.MinFragmentsCount, ammo.MaxFragmentsCount, DefaultHitBody, Randoms, ammo.BallisticCoeficient, player, weapon, fireIndex, null);
	}

	public void CreateMultiProjectileShot(_EA12 ammo, Vector3 origin, Vector3 direction, float speedFactor, List<_EC26> preallocatedShots, Player player, Item weapon)
	{
		int projectileCount = ammo.ProjectileCount;
		int fireIndex = this.m__E000++;
		for (int i = 0; i < projectileCount; i++)
		{
			preallocatedShots.Add(CreateShot(ammo, origin, (direction * 100f + ammo.buckshotDispersion * Randoms.GetRandomDirection(UnityEngine.Random.Range(0, 512))).normalized, fireIndex, player, weapon, speedFactor, i * 2));
		}
	}

	public void ManualUpdate(float deltaTime)
	{
		_E002(deltaTime);
		_E000();
	}

	[Conditional("CLIENT")]
	private void _E000()
	{
		foreach (_EC1F item in this.m__E004)
		{
			item.UpdatePosition();
		}
		for (int num = this.m__E004.Count - 1; num >= 0; num--)
		{
			_EC1F obj = this.m__E004[num];
			if (obj.ShotIsFinished())
			{
				AssetPoolObject.ReturnToPool(obj.VisualAmmoGameObject, immediate: false);
				this.m__E004.RemoveAt(num);
			}
		}
	}

	private bool _E001(_EC26 shot)
	{
		bool isShotAndParentFinished = shot.IsShotAndParentFinished;
		if (isShotAndParentFinished)
		{
			this.m__E001.Add(shot);
		}
		return isShotAndParentFinished;
	}

	private void _E002(float simulationTime)
	{
		if (Shots == null || Shots.Count == 0)
		{
			return;
		}
		Shots.RemoveAll(_E007);
		for (int i = 0; i < Shots.Count; i++)
		{
			this.m__E002.Enqueue(new _E000(simulationTime, Shots[i]));
		}
		while (this.m__E002.Count > 0)
		{
			_E000 obj = this.m__E002.Dequeue();
			_EC26 shot = obj.Shot;
			float simulationTime2 = obj.SimulationTime;
			try
			{
				shot.Tick(simulationTime2);
				if (shot.HasAchievedTarget && !shot.ShotProcessed)
				{
					this.m__E003(shot);
					shot.ShotProcessed = true;
					for (int j = 0; j < shot.Fragments.Count; j++)
					{
						_EC26 obj2 = shot.Fragments[j];
						if (!Shots.Contains(obj2))
						{
							Shots.Add(obj2);
						}
						this.m__E002.Enqueue(new _E000(shot.TimeLeftSinceFrameStart, obj2));
					}
				}
				else if (!Shots.Contains(shot))
				{
					Shots.Add(shot);
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
				if (Shots.Remove(shot))
				{
					_EC26.Release(shot);
				}
			}
		}
		for (int num = this.m__E001.Count - 1; num >= 0; num--)
		{
			_EC26 obj3 = this.m__E001[num];
			if (obj3.AreAllFragmentsFinished)
			{
				this.m__E001.RemoveAt(num);
				_EC26.Release(obj3);
			}
		}
	}

	public void Shoot(_EC26 shot)
	{
		if (shot.Ammo is _EA12 obj && obj.ShowBullet)
		{
			_E003(shot);
		}
		Shots.Add(shot);
	}

	[Conditional("CLIENT")]
	private void _E003(_EC26 shot)
	{
		GameObject gameObject = Singleton<_E760>.Instance.CreateItem(shot.Ammo, isAnimated: false);
		gameObject.SetActive(value: true);
		gameObject.GetComponent<AmmoPoolObject>();
		_EC1F item = new _EC1F(shot, gameObject.transform, gameObject);
		this.m__E004.Add(item);
		item.UpdatePosition();
	}

	public void SimulateShot(_EC26 shot, float simulationTime, float simulationStep)
	{
		_E004(shot, simulationTime, simulationStep);
		_EC26.Release(shot);
	}

	public void PreWarmerSimulateShotNoPool(_EC26 shot, float simulationTime, float simulationStep)
	{
		_E004(shot, simulationTime, simulationStep);
	}

	private void _E004(_EC26 shot, float simulationTime, float simulationStep)
	{
		Shots.Add(shot);
		float num = 0f;
		while (num < simulationTime)
		{
			num += simulationStep;
			_E002(simulationStep);
		}
		Shots.Remove(shot);
	}

	public static float GetAmmoPenetrationPower(_EA12 ammo, int randomInt, _EC13 randoms)
	{
		return randoms.GetNormalDistributedRandomFloat(randomInt, ammo.PenetrationPower, ammo.PenetrationPowerDiviation);
	}

	private void OnDestroy()
	{
		foreach (_EC26 shot in Shots)
		{
			_EC26.Release(shot);
		}
		Shots.Clear();
		foreach (_EC26 item in this.m__E001)
		{
			_EC26.Release(item);
		}
		this.m__E001.Clear();
	}
}
