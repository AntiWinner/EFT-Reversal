using System.Text;
using EFT;
using UnityEngine;

public class DebugPlayerAIData : MonoBehaviour
{
	public RectTransform IconsPanel;

	public DebugTextPanelWithManualLayout TextPanel;

	public BotInfoIcon IsInTree;

	public BotInfoIcon IsAggressor;

	public BotInfoIcon BossNoAttack;

	public BotInfoIcon CanBeFreeKilled;

	public BotInfoIcon IsPursuitable;

	public BotInfoIcon ScavAttacker;

	public BotInfoIcon IsAxe;

	private RectTransform _E000;

	public RectTransform RectTransform => _E000 ?? (_E000 = base.transform.RectTransform());

	public void UpdateData(_E10B actorData)
	{
		_E108 aIData = actorData.AIData;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(aIData.Nickname ?? "");
		stringBuilder.AppendLine(_ED3E._E000(48578) + aIData.PowerOfEquipment.ToString(_ED3E._E000(48633)));
		stringBuilder.AppendLine(string.Format(_ED3E._E000(48629), aIData.PlaceInfoId));
		stringBuilder.AppendLine(_ED3E._E000(48623) + aIData.shotNameWeapon);
		stringBuilder.AppendLine(_ED3E._E000(48613) + aIData.hashName);
		stringBuilder.AppendLine(_ED3E._E000(48611) + aIData.currentOpName);
		IsInTree.SetBool(aIData.IsInTree);
		IsAggressor.SetBool(aIData.IsAggressor);
		if (!aIData.IsAI && actorData.PlayerOwner?.Profile?.Info != null && actorData.PlayerOwner.Profile.Info.Side == EPlayerSide.Savage)
		{
			BossNoAttack.SetBool(aIData.BossNoAttack);
		}
		else
		{
			BossNoAttack.SetBool(value: false);
		}
		CanBeFreeKilled.SetBool(aIData.CanBeFreeKilled);
		IsPursuitable.SetBool(aIData.IsPursuitable);
		ScavAttacker.SetBool(aIData.ScavAttacker);
		IsAxe.SetBool(aIData.IsAxe);
		TextPanel.UpdateText(stringBuilder.ToString());
		RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, IconsPanel.rect.height + TextPanel.RectTransform.rect.height);
	}
}
