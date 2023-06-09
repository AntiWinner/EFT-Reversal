using System;
using System.Text;
using EFT;
using UnityEngine;

public class BotInfoDataPanel : MonoBehaviour
{
	public DebugTextPanelWithManualLayout TextPanel;

	private string m__E000 = new Color(0.8f, 0.8f, 0.8f).GetRichTextColor();

	private string m__E001 = new Color(0.25f, 1f, 0.2f).GetRichTextColor();

	private readonly StringBuilder m__E002 = new StringBuilder();

	private RectTransform m__E003;

	public RectTransform RectTransform => this.m__E003 ?? (this.m__E003 = base.transform.RectTransform());

	public void UpdateData(_E10B actorData, EBotInfoMode mode, bool showHint)
	{
		string text;
		try
		{
			text = GetInfoText(actorData, mode, showHint);
		}
		catch (Exception)
		{
			text = _ED3E._E000(4672);
		}
		TextPanel.UpdateText(text);
	}

	public string GetInfoText(_E10B botData, EBotInfoMode mode, bool showHints)
	{
		return mode switch
		{
			EBotInfoMode.Behaviour => _E005(botData, showHints), 
			EBotInfoMode.BattleState => _E006(botData, showHints), 
			EBotInfoMode.Health => _E007(botData), 
			EBotInfoMode.Specials => _E004(botData, showHints), 
			EBotInfoMode.Custom => _E003(botData, showHints), 
			_ => null, 
		};
	}

	private string _E000(_E10B actorDataStruct)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(string.Format(_ED3E._E000(48330), actorDataStruct.BotData.PatrolPointStatus));
		stringBuilder.AppendLine(string.Format(_ED3E._E000(48380), actorDataStruct.BotData.PatrolWayStatus));
		return stringBuilder.ToString();
	}

	private string _E001(bool val)
	{
		if (val)
		{
			return _ED3E._E000(48372);
		}
		return "";
	}

	private string _E002(bool val)
	{
		if (val)
		{
			return _ED3E._E000(48369);
		}
		return "";
	}

	private string _E003(_E10B actorDataStruct, bool showLabels)
	{
		_E10A botData = actorDataStruct.BotData;
		this.m__E002.Clear();
		this.m__E002.AppendLabeledValue(_ED3E._E000(48362), botData.LayerName, this.m__E000, this.m__E000, showLabels);
		if (string.IsNullOrEmpty(botData.CustomData))
		{
			this.m__E002.AppendLine(_ED3E._E000(48352));
		}
		else
		{
			this.m__E002.AppendLine(botData.CustomData);
		}
		return this.m__E002.ToString();
	}

	private string _E004(_E10B actorDataStruct, bool showLabels)
	{
		_E10A botData = actorDataStruct.BotData;
		this.m__E002.Clear();
		StringBuilder builder = this.m__E002;
		if (actorDataStruct.ProfileId.Length > 12)
		{
			builder.AppendLabeledValue(_ED3E._E000(48407), actorDataStruct.ProfileId.Substring(0, 12), Color.white, Color.white, showLabels);
			if (actorDataStruct.ProfileId.Length <= 24)
			{
				builder.AppendLabeledValue("", actorDataStruct.ProfileId.Substring(12), Color.white, Color.white, labelEnabled: false);
			}
			else
			{
				builder.AppendLabeledValue("", actorDataStruct.ProfileId.Substring(12, 12), Color.white, Color.white, labelEnabled: false);
				builder.AppendLabeledValue("", actorDataStruct.ProfileId.Substring(24), Color.white, Color.white, labelEnabled: false);
			}
		}
		this.m__E002.AppendLabeledValue(_ED3E._E000(48402), botData.IsInSpawnWeapon.ToString(), Color.white, Color.white, showLabels);
		this.m__E002.AppendLabeledValue(_ED3E._E000(48388), botData.HaveAxeEnemy.ToString(), Color.white, Color.white, showLabels);
		this.m__E002.AppendLabeledValue(_ED3E._E000(48445), botData.BotRole.ToString(), Color.white, Color.white, showLabels);
		this.m__E002.AppendLabeledValue(_ED3E._E000(48442), botData.PlayerState.ToString(), Color.white, Color.white, showLabels);
		string data;
		try
		{
			data = actorDataStruct.PlayerOwner.MovementContext.CurrentState.Name.ToString();
		}
		catch (Exception)
		{
			data = _ED3E._E000(48432);
		}
		this.m__E002.AppendLabeledValue(_ED3E._E000(48424), data, Color.white, Color.white, showLabels);
		this.m__E002.AppendLabeledValue(_ED3E._E000(48416), actorDataStruct.BotData.ToString(), Color.white, Color.white, showLabels);
		return this.m__E002.ToString();
	}

	private string _E005(_E10B actorDataStruct, bool showLabels)
	{
		_E10A botData = actorDataStruct.BotData;
		this.m__E002.Clear();
		_ = botData.MainTactic;
		string strategyName = botData.StrategyName;
		this.m__E002.AppendLabeledValue(_ED3E._E000(48478), string.Format(_ED3E._E000(53834), botData.Id, strategyName), Color.white, Color.white, showLabels);
		this.m__E002.AppendLabeledValue(_ED3E._E000(48362), botData.LayerName, Color.yellow, Color.yellow, showLabels);
		this.m__E002.AppendLabeledValue(_ED3E._E000(48467), botData.NodeName, Color.white, Color.white, showLabels);
		this.m__E002.AppendLabeledValue(_ED3E._E000(48456), botData.Reason, this.m__E001, this.m__E001, showLabels);
		if (!string.IsNullOrEmpty(botData.PrevNodeName))
		{
			this.m__E002.AppendLabeledValue(_ED3E._E000(48448), botData.PrevNodeName, this.m__E000, this.m__E000, showLabels);
			this.m__E002.AppendLabeledValue(_ED3E._E000(48505), botData.PrevNodeExitReason, this.m__E000, this.m__E000, showLabels);
		}
		return this.m__E002.ToString();
	}

	private string _E006(_E10B actorDataStruct, bool showLabels)
	{
		_E10A botData = actorDataStruct.BotData;
		this.m__E002.Clear();
		try
		{
			if (actorDataStruct.PlayerOwner == null)
			{
				return _ED3E._E000(48496);
			}
			Player.FirearmController firearmController = actorDataStruct.PlayerOwner.GetPlayer.HandsController as Player.FirearmController;
			if (firearmController != null)
			{
				int chamberAmmoCount = firearmController.Item.ChamberAmmoCount;
				int currentMagazineCount = firearmController.Item.GetCurrentMagazineCount();
				this.m__E002.AppendLabeledValue(_ED3E._E000(48487), _ED3E._E000(48540) + chamberAmmoCount + _ED3E._E000(48543) + currentMagazineCount + _ED3E._E000(48539) + botData.Ammo.ToString(_ED3E._E000(27314)), Color.white, Color.white, showLabels);
			}
			this.m__E002.AppendLabeledValue(_ED3E._E000(48535), botData.HitsOnMe + _ED3E._E000(30703) + botData.ShootsOnMe, Color.white, Color.white, showLabels);
			this.m__E002.AppendLabeledValue(_ED3E._E000(48524), botData.Reloading.ToString(), Color.white, Color.white, showLabels);
			this.m__E002.AppendLabeledValue(_ED3E._E000(48518), botData.CoverIndex.ToString(), Color.white, Color.white, showLabels);
		}
		catch (Exception)
		{
			Debug.LogError(_ED3E._E000(48574));
		}
		return this.m__E002.ToString();
	}

	private string _E007(_E10B actorDataStruct)
	{
		bool labelEnabled = true;
		_E109 heathsData = actorDataStruct.HeathsData;
		this.m__E002.Clear();
		this.m__E002.AppendLabeledValue(_ED3E._E000(48545), heathsData.HealthHead + _E001(heathsData.HealthHeadBL) + _E002(heathsData.HealthHeadBroken), Color.white, Color.white, labelEnabled);
		this.m__E002.AppendLabeledValue(_ED3E._E000(48606), heathsData.HealthBody + _E001(heathsData.HealthBodyBL) + _E002(heathsData.HealthHeadBroken), Color.white, Color.white, labelEnabled);
		this.m__E002.AppendLabeledValue(_ED3E._E000(48596), heathsData.HealthStomach + _E001(heathsData.HealthStomachBL), Color.white, Color.white, labelEnabled);
		this.m__E002.AppendLabeledValue(_ED3E._E000(48588), heathsData.HealthLeftArm + _E001(heathsData.HealthLeftArmBL) + _E002(heathsData.HealthLeftArmBroken) + _ED3E._E000(18502) + heathsData.HealthRightArm + _E001(heathsData.HealthRightArmBL) + _E002(heathsData.HealthRightArmBroken), Color.white, Color.white, labelEnabled);
		this.m__E002.AppendLabeledValue(_ED3E._E000(48585), heathsData.HealthLeftLeg + _E001(heathsData.HealthLeftLegBL) + _E002(heathsData.HealthLeftLegBroken) + _ED3E._E000(18502) + heathsData.HealthRightLeg + _E001(heathsData.HealthRightLegBL) + _E002(heathsData.HealthRightLegBroken), Color.white, Color.white, labelEnabled);
		return this.m__E002.ToString();
	}
}
