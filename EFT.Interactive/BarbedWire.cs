using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;

namespace EFT.Interactive;

public class BarbedWire : DamageTrigger
{
	private const float _E000 = 0.15f;

	private const float _E001 = 0.5f;

	private float _E002;

	[SerializeField]
	private SoundBank _soundBank;

	[CompilerGenerated]
	private readonly string _E003 = _ED3E._E000(212463);

	protected override bool IsStatic => true;

	public override string Description
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
	}

	protected override void ProceedDamage(Player player, BodyPartCollider bodyPart)
	{
		if (player.IsSprintEnabled)
		{
			player.EnableSprint(enable: false);
		}
		if (bodyPart.ProceedBarb())
		{
			PlaySound();
		}
	}

	protected override void AddPenalty(Player player)
	{
		player.AddStateSpeedLimit(0.15f, Player.ESpeedLimit.BarbedWire);
	}

	protected override void RemovePenalty(Player player)
	{
		player.RemoveStateSpeedLimit(Player.ESpeedLimit.BarbedWire);
	}

	protected override void PlaySound()
	{
		if (Time.time - _E002 >= 0.5f)
		{
			if (Singleton<BetterAudio>.Instantiated)
			{
				Singleton<BetterAudio>.Instance.PlayAtPoint(base.transform.position, _soundBank, _E8A8.Instance.Distance(base.transform.position), 1f, -1f, EnvironmentType.Outdoor, EOcclusionTest.Fast);
			}
			_E002 = Time.time;
		}
	}
}
