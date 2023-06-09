using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Systems.Effects;
using Comfort.Common;
using EFT.Interactive;
using EFT.MovingPlatforms;
using UnityEngine;

namespace EFT;

internal abstract class ClientGameWorld : GameWorld
{
	[CompilerGenerated]
	private new sealed class _E001
	{
		public ClientWorld clientWorld;

		public ClientGameWorld _003C_003E4__this;

		internal void _E000()
		{
			clientWorld.OnRpcLampStateChanged -= _003C_003E4__this._E001;
		}

		internal void _E001()
		{
			clientWorld.OnRpcWindowHit -= _003C_003E4__this._E002;
		}
	}

	private int _E01E;

	private ulong _E01F;

	private ulong _E020;

	[CompilerGenerated]
	private float _E021;

	internal new int _E000 => _E01E++;

	public override ulong TotalOutgoingBytes => _E020;

	public float LastServerWorldTime
	{
		[CompilerGenerated]
		get
		{
			return _E021;
		}
		[CompilerGenerated]
		private set
		{
			_E021 = value;
		}
	}

	public override async Task InitLevel(_E63B itemFactory, _E761 config, bool loadBundlesAndCreatePools = true, List<ResourceKey> resources = null, IProgress<_E5BB> progress = null, CancellationToken ct = default(CancellationToken))
	{
		await base.InitLevel(itemFactory, config, loadBundlesAndCreatePools, resources, progress, ct);
		SynchronizableObjectLogicProcessor = new _E90E();
		_ = ObservedPlayerInterpolationSettings.Instance;
	}

	internal override void _E012(World world)
	{
		base._E012(world);
		ClientWorld clientWorld;
		if ((object)(clientWorld = world as ClientWorld) != null)
		{
			clientWorld.OnRpcLampStateChanged += _E001;
			CompositeDisposable.AddDisposable(delegate
			{
				clientWorld.OnRpcLampStateChanged -= _E001;
			});
			clientWorld.OnRpcWindowHit += _E002;
			CompositeDisposable.AddDisposable(delegate
			{
				clientWorld.OnRpcWindowHit -= _E002;
			});
		}
		else
		{
			UnityEngine.Debug.LogError(_ED3E._E000(180239) + world);
		}
	}

	private void _E000(Vector3 pos)
	{
		UnityEngine.Debug.DrawRay(pos + Vector3.up / 4f, Vector3.down / 2f, Color.red, 10f);
		UnityEngine.Debug.DrawRay(pos + Vector3.left / 4f, Vector3.right / 2f, Color.red, 10f);
		UnityEngine.Debug.DrawRay(pos + Vector3.forward / 4f, Vector3.back / 2f, Color.red, 10f);
	}

	protected override async Task PreloadAdditionalData()
	{
		await base.PreloadAdditionalData();
		BetterAudio betterAudio = _E3AA.FindUnityObjectOfType<BetterAudio>();
		if (betterAudio == null)
		{
			GameObject obj = new GameObject(_ED3E._E000(88578));
			obj.transform.SetParent(base.gameObject.transform);
			betterAudio = obj.AddComponent<BetterAudio>();
		}
		Singleton<BetterAudio>.Create(betterAudio);
		betterAudio.PreloadCoroutine().HandleExceptions();
	}

	private void _E001(int netId, Turnable.EState state)
	{
		if (Turnables.TryGetValue(netId, out var value))
		{
			value.Switch(state);
		}
	}

	private void _E002(int netId, Vector3 hitPosition)
	{
		if (Windows.TryGetByKey(netId, out var value))
		{
			WindowBreaker windowBreaker = value;
			_EC23 hitInfo = new _EC23
			{
				HitPoint = hitPosition
			};
			windowBreaker.MakeHit(in hitInfo);
		}
	}

	public override void ShotDelegate(_EC26 shotResult)
	{
		if (shotResult.IsFlyingOutOfTime)
		{
			return;
		}
		_EC23 damageInfo = new _EC23(EDamageType.Bullet, shotResult);
		_EC22 shotID = new _EC22(shotResult.Ammo.Id, shotResult.FragmentIndex);
		_E6FF playerHitInfo = ((shotResult.HittedBallisticCollider != null) ? shotResult.HittedBallisticCollider.ApplyHit(damageInfo, shotID) : null);
		shotResult.AddClientHitPosition(playerHitInfo);
		_E9D6 itemComponent = shotResult.Ammo.GetItemComponent<_E9D6>();
		if (itemComponent != null && shotResult.TimeSinceShot >= itemComponent.Template.FuzeArmTimeSec)
		{
			if (Singleton<Effects>.Instantiated)
			{
				string explosionType = itemComponent.Template.ExplosionType;
				if (!string.IsNullOrEmpty(explosionType) && shotResult.IsFirstHit)
				{
					Singleton<Effects>.Instance.EmitGrenade(explosionType, shotResult.HitPoint, shotResult.HitNormal, shotResult.IsForwardHit ? 1 : 0);
				}
				if (itemComponent.Template.ShowHitEffectOnExplode)
				{
					Singleton<Effects>.Instance.EffectsCommutator.PlayHitEffect(shotResult, playerHitInfo);
				}
			}
			Grenade.Explosion(null, itemComponent, shotResult.HitPoint, shotResult.Player, base.SharedBallisticsCalculator, shotResult.Weapon, shotResult.HitNormal * 0.08f);
		}
		else if (Singleton<Effects>.Instantiated)
		{
			Singleton<Effects>.Instance.EffectsCommutator.PlayHitEffect(shotResult, playerHitInfo);
		}
	}

	public override _E6FF HackShot(_EC23 damageInfo)
	{
		_E6FF result = base.HackShot(damageInfo);
		if (_effectsCommutator == null)
		{
			_effectsCommutator = Singleton<Effects>.Instance.EffectsCommutator;
		}
		_effectsCommutator.PlayKnifeHitEffect(damageInfo);
		return result;
	}

	protected override void OnDestroy()
	{
		Singleton<Effects>.Release(Singleton<Effects>.Instance);
		base.OnDestroy();
	}

	public override void RegisterPlayer(Player player)
	{
		base.RegisterPlayer(player);
		ClientPlayer clientPlayer = player as ClientPlayer;
		if (clientPlayer != null)
		{
			clientPlayer.OnPacketSent += _E003;
		}
		ObservedPlayer observedPlayer = player as ObservedPlayer;
		if (observedPlayer != null)
		{
			observedPlayer.OnPacketReceived += _E004;
		}
		if (SpeedLimitsEnabled && (player.PointOfView == EPointOfView.FirstPerson || player.IsYourPlayer || player is ClientPlayer))
		{
			player.MovementContext.CreateSpeedLimiter(SpeedLimits);
		}
	}

	public override void UnregisterPlayer(Player player)
	{
		if (!RegisteredPlayers.Contains(player))
		{
			return;
		}
		if ((object)player != null)
		{
			if (!(player is ClientPlayer clientPlayer))
			{
				if (player is ObservedPlayer observedPlayer)
				{
					observedPlayer.OnPacketReceived -= _E004;
				}
			}
			else
			{
				clientPlayer.OnPacketSent -= _E003;
			}
		}
		base.UnregisterPlayer(player);
	}

	protected override void OtherElseWorldTick(float dt)
	{
		base.OtherElseWorldTick(dt);
		if (SynchronizableObjectLogicProcessor != null && SynchronizableObjectLogicProcessor.AirdropManager != null)
		{
			SynchronizableObjectLogicProcessor.AirdropManager.ManualUpdate(dt);
		}
	}

	private void LateUpdate()
	{
		MovingPlatform._E001[] platformAdapters = PlatformAdapters;
		for (int i = 0; i < platformAdapters.Length; i++)
		{
			platformAdapters[i].ApplyStoredPackets();
		}
	}

	private void _E003(ClientPlayer player, int bytes, ulong totalBytes)
	{
		_E020 += (ulong)bytes;
	}

	private void _E004(ObservedPlayer player, int bytes, ulong totalBytes)
	{
		_E01F += (ulong)bytes;
	}

	protected override void SpawnLootCorpse(_E546 lootItem)
	{
		SpawnLootCorpse<ObservedCorpse>(lootItem);
	}

	public void ProcessWordPacket(_E5BC packet)
	{
		LastServerWorldTime = packet.ServerWorldTime;
	}

	[DebuggerHidden]
	[CompilerGenerated]
	private Task _E005(_E63B itemFactory, _E761 config, bool loadBundlesAndCreatePools, List<ResourceKey> resources, IProgress<_E5BB> progress, CancellationToken ct)
	{
		return base.InitLevel(itemFactory, config, loadBundlesAndCreatePools, resources, progress, ct);
	}

	[DebuggerHidden]
	[CompilerGenerated]
	private Task _E006()
	{
		return base.PreloadAdditionalData();
	}
}
