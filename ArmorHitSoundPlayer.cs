using Comfort.Common;
using EFT;
using EFT.CameraControl;
using UnityEngine;

public class ArmorHitSoundPlayer : BulletSoundPlayer
{
	[SerializeField]
	private AudioClip[] _fpSounds;

	[SerializeField]
	private SoundBank _closeDistandeSound;

	private PlayerCameraController m__E000;

	public override void Init(Camera camera, PlayerCameraController playerCameraController)
	{
		this.m__E000 = playerCameraController;
	}

	private void Start()
	{
		_E3BF.OnArmorHit += _E000;
	}

	private void OnDestroy()
	{
		_E3BF.OnArmorHit -= _E000;
	}

	private void OnDisable()
	{
		_E3BF.OnArmorHit -= _E000;
	}

	private void _E000(Player player, Player enemy, Vector3 directionFromPlayer)
	{
		if (!(this.m__E000 == null))
		{
			if (this.m__E000.Player == player)
			{
				_E001(this.m__E000.Camera.transform.forward, directionFromPlayer);
				return;
			}
			float magnitude = (this.m__E000.Player.Position - player.Position).magnitude;
			_E002(player, magnitude);
		}
	}

	private void _E001(Vector3 forward, Vector3 normal)
	{
		int num = Random.Range(0, _fpSounds.Length - 1);
		float panStereo = _E3BF.CalculatePan(forward, normal);
		Singleton<BetterAudio>.Instance.PlayNonspatial(_fpSounds[num], BetterAudio.AudioSourceGroupType.Impacts, panStereo);
	}

	private void _E002(Player player, float distance)
	{
		Singleton<BetterAudio>.Instance.PlayAtPoint(player.Position, _closeDistandeSound, distance);
	}
}
