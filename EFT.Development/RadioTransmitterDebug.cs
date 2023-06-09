using System.Collections.Generic;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.Development;

public class RadioTransmitterDebug : MonoBehaviour
{
	private bool m__E000;

	private const string m__E001 = "<color=red>RED</color>";

	private const string m__E002 = "<color=green>GREEN</color>";

	private const string m__E003 = "<color=yellow>YELLOW</color>";

	private const string m__E004 = "NOT INITIALIZED";

	private const string m__E005 = "NO RADIO TRANSMITTER";

	private const string m__E006 = "<color=red>AGRESSOR!</color>";

	private bool m__E007;

	private Player _E008;

	private RadioTransmitterRecodableComponent _E009;

	private List<string> _E00A = new List<string>();

	private List<string> _E00B = new List<string>();

	private readonly EquipmentSlot[] _E00C = new EquipmentSlot[1] { EquipmentSlot.Pockets };

	public void Initialize(PlayerOwner playerOwner)
	{
		_E008 = playerOwner.Player;
		this.m__E000 = playerOwner.Player is LocalPlayer;
		if (!this.m__E000)
		{
			ClientPlayer obj = (ClientPlayer)_E008;
			obj.OnSyncLighthouseTraderZoneData += _E007;
			obj.SetActiveLighthouseTraderZoneDebug += _E005;
		}
		_E008._E0DE.AddItemEvent += _E002;
		_E008._E0DE.RemoveItemEvent += _E003;
		if (_E008.RecodableItemsHandler != null)
		{
			_E001();
			if (this.m__E000)
			{
				_E000();
			}
		}
	}

	private void _E000()
	{
		if (_E009.IsEncoded)
		{
			_E009.SetStatus(RadioTransmitterStatus.Yellow);
		}
		else
		{
			_E009.SetStatus(RadioTransmitterStatus.Red);
		}
	}

	private void _E001()
	{
		_E009 = _E008.RecodableItemsHandler.GetRecodableComponent<RadioTransmitterRecodableComponent>();
	}

	private void _E002(_EAF2 args)
	{
		_E001();
	}

	private void _E003(_EAF3 args)
	{
		_E001();
	}

	private void OnDisable()
	{
		if (!this.m__E000)
		{
			ClientPlayer obj = (ClientPlayer)_E008;
			obj.OnSyncLighthouseTraderZoneData -= _E007;
			obj.SetActiveLighthouseTraderZoneDebug -= _E005;
		}
		_E008._E0DE.AddItemEvent -= _E002;
		_E008._E0DE.RemoveItemEvent -= _E003;
	}

	private void OnGUI()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
		gUIStyle.alignment = TextAnchor.MiddleCenter;
		string text = null;
		if (_E009 != null)
		{
			switch (_E009.Status)
			{
			case RadioTransmitterStatus.Red:
				text = _ED3E._E000(210907);
				break;
			case RadioTransmitterStatus.Green:
				text = _ED3E._E000(210882);
				break;
			case RadioTransmitterStatus.Yellow:
				text = _ED3E._E000(210917);
				break;
			case RadioTransmitterStatus.NotInitialized:
				text = _ED3E._E000(208906);
				break;
			}
		}
		else
		{
			text = _ED3E._E000(208954);
		}
		GUILayout.BeginVertical(GUI.skin.box);
		GUILayout.BeginVertical(GUI.skin.box);
		GUILayout.Label(_ED3E._E000(208935), gUIStyle);
		GUILayout.EndVertical();
		GUILayout.BeginHorizontal();
		GUILayout.Label(_ED3E._E000(48442));
		GUILayout.Label(text ?? "", gUIStyle);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (_E008.IsAgressorInLighthouseTraderZone)
		{
			GUILayout.Label(_ED3E._E000(208964));
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(_ED3E._E000(209001));
		GUILayout.Label(string.Format(_ED3E._E000(55940), this.m__E007), gUIStyle);
		GUILayout.EndHorizontal();
		if (_E00A.Count > 0)
		{
			GUILayout.BeginVertical(GUI.skin.box);
			GUILayout.Label(_ED3E._E000(209053), gUIStyle);
			GUILayout.BeginVertical();
			foreach (string item in _E00A)
			{
				GUILayout.Label(item ?? "");
			}
			GUILayout.EndVertical();
			GUILayout.EndVertical();
		}
		if (_E00B.Count > 0)
		{
			GUILayout.BeginVertical(GUI.skin.box);
			GUILayout.Label(_ED3E._E000(209029), gUIStyle);
			GUILayout.BeginVertical();
			foreach (string item2 in _E00B)
			{
				GUILayout.Label(item2 ?? "");
			}
			GUILayout.EndVertical();
			GUILayout.EndVertical();
		}
		if (!this.m__E000)
		{
			GUILayout.BeginHorizontal();
			flag = GUILayout.Button(_ED3E._E000(209071));
			flag2 = GUILayout.Button(_ED3E._E000(209059));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			flag3 = GUILayout.Button(_ED3E._E000(209111));
			flag4 = GUILayout.Button(_ED3E._E000(209089));
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
		if (flag)
		{
			_E006(IsEncoded: true);
		}
		if (flag2)
		{
			_E006(IsEncoded: false);
		}
		if (flag3)
		{
			_E004(value: true);
		}
		if (flag4)
		{
			_E004(value: false);
		}
	}

	private void _E004(bool value)
	{
		((ClientPlayer)_E008).DevelopSetActiveLighthouseTraderZoneDebug(value);
	}

	private void _E005(bool value)
	{
		this.m__E007 = value;
		if (!this.m__E007)
		{
			_E00A.Clear();
			_E00B.Clear();
		}
	}

	private void _E006(bool IsEncoded)
	{
		((ClientPlayer)_E008).DevelopSetEncodedRadioTransmitter(IsEncoded);
	}

	private void _E007(_E634 data)
	{
		_E00A.Clear();
		_E00B.Clear();
		for (int i = 0; i < data.AllowedPlayers.Length; i++)
		{
			_E00A.Add(_ED3E._E000(103076) + data.AllowedPlayers[i].Nickname + _ED3E._E000(59467));
		}
		for (int j = 0; j < data.UnallowedPlayers.Length; j++)
		{
			if (data.UnallowedPlayers[j].Status == RadioTransmitterStatus.Yellow)
			{
				_E00B.Add(_ED3E._E000(103041) + data.UnallowedPlayers[j].Nickname + _ED3E._E000(59467));
			}
			else if (data.UnallowedPlayers[j].Status == RadioTransmitterStatus.Red)
			{
				_E00B.Add(_ED3E._E000(103088) + data.UnallowedPlayers[j].Nickname + _ED3E._E000(59467));
			}
			else
			{
				_E00B.Add(data.UnallowedPlayers[j].Nickname);
			}
		}
	}
}
