using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace EFT;

public class LocalPlayer : Player
{
	public static async Task<LocalPlayer> Create(int playerId, Vector3 position, Quaternion rotation, string layerName, string prefix, EPointOfView pointOfView, Profile profile, bool aiControl, EUpdateQueue updateQueue, EUpdateMode armsUpdateMode, EUpdateMode bodyUpdateMode, CharacterControllerSpawner.Mode characterControllerMode, Func<float> getSensitivity, Func<float> getAimingSensitivity, _E759 statisticsManager, _E60F filter, _E935 questController, bool isYourPlayer)
	{
		LocalPlayer localPlayer = Player.Create<LocalPlayer>(_E5D2.PLAYER_BUNDLE_NAME, playerId, position, updateQueue, armsUpdateMode, bodyUpdateMode, characterControllerMode, getSensitivity, getAimingSensitivity, prefix, aiControl);
		localPlayer.IsYourPlayer = isYourPlayer;
		SinglePlayerInventoryController inventoryController = new SinglePlayerInventoryController(localPlayer, profile);
		if (questController == null && !aiControl)
		{
			questController = new _E941(profile, inventoryController, null, fromServer: true);
			questController.Run();
		}
		await localPlayer.Init(rotation, layerName, pointOfView, profile, inventoryController, new _E9CA(profile.Health, localPlayer, inventoryController, profile.Skills, aiControl), statisticsManager, questController, filter, EVoipState.NotAvailable, aiControl, async: false);
		foreach (_EA6A item in localPlayer.Inventory.NonQuestItems.OfType<_EA6A>())
		{
			localPlayer._E0DE.StrictCheckMagazine(item, status: true, localPlayer.Profile.MagDrillsMastering, notify: false, useOperation: false);
		}
		localPlayer._handsController = EmptyHandsController._E000<EmptyHandsController>(localPlayer);
		localPlayer._handsController.Spawn(1f, delegate
		{
		});
		localPlayer.AIData = new _E279(null, localPlayer);
		localPlayer.AggressorFound = false;
		localPlayer._animators[0].enabled = true;
		return localPlayer;
	}

	protected override void OnSkillLevelChanged(_E74E skill)
	{
		base.OnSkillLevelChanged(skill);
		if (!base.IsAI)
		{
			_E857.DisplayNotification(new _E89B(skill));
		}
	}

	protected override void OnWeaponMastered(_E750 masterSkill)
	{
		base.OnWeaponMastered(masterSkill);
		if (!base.IsAI)
		{
			_E857.DisplayMessageNotification(string.Format(_ED3E._E000(193759).Localized(), masterSkill.MasteringGroup.Id.Localized(), masterSkill.Level.ToString()));
		}
	}

	public override void PauseAllEffectsOnPlayer()
	{
		base.ActiveHealthController.PauseAllEffects();
	}

	public override void UnpauseAllEffectsOnPlayer()
	{
		base.ActiveHealthController.UnpauseAllEffects();
	}
}
