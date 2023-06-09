using System.Collections.Generic;
using Comfort.Common;
using EFT;
using EFT.CameraControl;
using UnityEngine;

public class BulletSoundPlayersController : MonoBehaviour
{
	[SerializeField]
	private List<BulletSoundPlayer> _bulletSoundPlayers;

	private Camera m__E000;

	private PlayerCameraController m__E001;

	private void Awake()
	{
		PlayerCameraController.OnPlayerCameraControllerCreated += _E000;
	}

	private void Update()
	{
		_E001();
	}

	private void OnDestroy()
	{
		PlayerCameraController.OnPlayerCameraControllerCreated -= _E000;
	}

	private void _E000(PlayerCameraController playerCameraController, Camera camera)
	{
		InitCamera(playerCameraController, camera);
	}

	public void InitCamera(PlayerCameraController playerCameraController, Camera camera)
	{
		this.m__E000 = camera;
		this.m__E001 = playerCameraController;
		foreach (BulletSoundPlayer bulletSoundPlayer in _bulletSoundPlayers)
		{
			bulletSoundPlayer.Init(this.m__E000, playerCameraController);
		}
	}

	private void _E001()
	{
		if (!(this.m__E000 == null) && !(this.m__E001 == null) && Singleton<GameWorld>.Instantiated && Singleton<GameWorld>.Instance.SharedBallisticsCalculator != null)
		{
			_EC1E sharedBallisticsCalculator = Singleton<GameWorld>.Instance.SharedBallisticsCalculator;
			for (int i = 0; i < sharedBallisticsCalculator.ActiveShotsCount; i++)
			{
				_E002(sharedBallisticsCalculator.GetActiveShot(i), this.m__E000);
			}
		}
	}

	private void _E002(_EC26 shot, Camera camera)
	{
		if (shot.Player == this.m__E001.Player || shot.Player == null)
		{
			return;
		}
		int currentIndex = shot.PositionHistory.Count - 1;
		string id = shot.Ammo.Id;
		if (shot.Player.HandsController is Player.FirearmController firearmController && firearmController.WeaponSoundPlayer.IsOccludedToListener)
		{
			return;
		}
		foreach (BulletSoundPlayer bulletSoundPlayer in _bulletSoundPlayers)
		{
			bulletSoundPlayer.TryShot(shot, camera, id, currentIndex);
		}
	}
}
