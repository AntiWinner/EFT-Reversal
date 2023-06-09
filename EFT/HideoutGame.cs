using System;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.UI;
using JsonType;
using UnityEngine;

namespace EFT;

internal sealed class HideoutGame : BaseLocalGame<HideoutPlayerOwner>
{
	private new _EB61 m__E000;

	private new _E9C4 _E001;

	internal new static HideoutGame _E000(_E7FD inputTree, Profile profile, _EB61 originalInventory, _E629 backendDateTime, _ECB1 insurance, MenuUI menuUI, CommonUI commonUI, PreloaderUI preloaderUI, GameUI gameUI, _E554.Location location, TimeAndWeatherSettings timeAndWeather, WavesSettings wavesSettings, EDateTime dateTime, Callback<ExitStatus, TimeSpan, _E907> callback, float fixedDeltaTime, EUpdateQueue updateQueue, _E796 backEndSession, _E9C4 healthController)
	{
		HideoutGame hideoutGame = BaseLocalGame<HideoutPlayerOwner>._E000<HideoutGame>(inputTree, profile, backendDateTime, insurance, menuUI, commonUI, preloaderUI, gameUI, location, timeAndWeather, wavesSettings, dateTime, callback, fixedDeltaTime, updateQueue, backEndSession, null);
		hideoutGame._E001 = healthController;
		hideoutGame.m__E000 = originalInventory;
		return hideoutGame;
	}

	protected override async Task<LocalPlayer> _E019(int playerId, Vector3 position, Quaternion rotation, string layerName, string prefix, EPointOfView pointOfView, Profile profile, bool aiControl, EUpdateQueue updateQueue, Player.EUpdateMode armsUpdateMode, Player.EUpdateMode bodyUpdateMode, CharacterControllerSpawner.Mode characterControllerMode, Func<float> getSensitivity, Func<float> getAimingSensitivity, _E759 statisticsManager, _E935 questController = null)
	{
		HideoutPlayer obj = await HideoutPlayer.Create(playerId, position, rotation, _ED3E._E000(60679), "", EPointOfView.FirstPerson, profile, aiControl: false, base.UpdateQueue, armsUpdateMode, Player.EUpdateMode.Auto, _E2B6.Config.CharacterController.ClientPlayerMode, () => Singleton<_E7DE>.Instance.Control.Settings.MouseSensitivity, () => Singleton<_E7DE>.Instance.Control.Settings.MouseAimingSensitivity, new _E757(), questController, _E001, this.m__E000);
		obj.MovementContext.SmoothedPoseLevel = 1f;
		return obj;
	}

	protected override void _E018(float timeBeforeDeploy)
	{
		GameUi.gameObject.SetActive(value: true);
		GameUi.TimerPanel.ProfileId = ProfileId;
	}

	protected override void _E01C()
	{
		base.GameTimer.Start();
		_E007();
		_E005 = _E5AD.Now;
		base.Status = GameStatus.Started;
		ConsoleScreen.ApplyStartCommands();
	}

	protected override void _E01A()
	{
		Stop(base._E001.Id, ExitStatus.Left, null);
	}

	public void Stop()
	{
		base.PlayerOwner.Player.OnGameSessionEnd(ExitStatus.Left, base.PastTime, base._E002.Id, string.Empty);
	}
}
