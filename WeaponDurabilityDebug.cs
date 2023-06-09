using System;
using Comfort.Common;
using EFT;
using EFT.InventoryLogic;
using EFT.UI;
using UnityEngine;

public class WeaponDurabilityDebug : MonoBehaviour
{
	private GUIStyle m__E000;

	private GUIStyle m__E001;

	private GUIStyle _E002;

	private Texture2D _E003;

	private Player _E004;

	private float _E005;

	private float _E006;

	private float _E007;

	public void SetDebugObject(Player toDebug)
	{
		_E004 = toDebug;
		_E000();
	}

	private void OnGUI()
	{
		if (_E004 == null || MonoBehaviourSingleton<PreloaderUI>.Instance.Console.IsConsoleVisible)
		{
			return;
		}
		Player.FirearmController firearmController = _E004.HandsController as Player.FirearmController;
		if (!(firearmController == null))
		{
			Weapon item = firearmController.Item;
			double num = Math.Round(item.Repairable.Durability, 3);
			double num2 = Math.Round(item.Repairable.MaxDurability, 3);
			float operatingResource = item.Template.OperatingResource;
			int templateDurability = item.Repairable.TemplateDurability;
			double num3 = num / (double)((float)templateDurability / operatingResource);
			AmmoTemplate currentAmmoTemplate = item.CurrentAmmoTemplate;
			float modsBurnRatio = 1f;
			_E5CB._E02D overheat = Singleton<_E5CB>.Instance.Overheat;
			float modsCoolFactor;
			float num4 = Mathf.Clamp(item.GetCurrentOverheat(_E62A.PastTime, overheat, out modsCoolFactor) / 100f, 1f, 2f);
			float num5 = currentAmmoTemplate?.DurabilityBurnModificator ?? 1f;
			float value = _E004.Skills.WeaponDurabilityLosOnShotReduce.Value;
			float num6 = ((currentAmmoTemplate == null) ? 0f : item.GetDurabilityLossOnShot(currentAmmoTemplate.DurabilityBurnModificator, num4, value, out modsBurnRatio));
			if (Time.time - _E005 > 1f)
			{
				_E005 = Time.time;
				_E006 = num6;
				_E007 = num6 / ((float)templateDurability / operatingResource);
			}
			GUI.Box(new Rect(450f, 35f, 240f, 20f), string.Format(_ED3E._E000(59189), num, num2, templateDurability), this.m__E000);
			GUI.Box(new Rect(450f, 55f, 240f, 20f), string.Format(_ED3E._E000(59223), num3, operatingResource), this.m__E000);
			GUI.Box(new Rect(450f, 75f, 240f, 20f), _ED3E._E000(59252) + item.DurabilityBurnRatio, this.m__E000);
			GUI.Box(new Rect(450f, 95f, 240f, 20f), _ED3E._E000(59238) + num5, this.m__E000);
			GUI.Box(new Rect(450f, 115f, 240f, 20f), _ED3E._E000(59286) + modsBurnRatio, this.m__E000);
			GUI.Box(new Rect(450f, 135f, 240f, 20f), _ED3E._E000(59270) + num4, this.m__E000);
			GUI.Box(new Rect(450f, 155f, 240f, 20f), _ED3E._E000(59319) + value, this.m__E000);
			GUI.Box(new Rect(450f, 175f, 240f, 20f), string.Format(_ED3E._E000(59352), _E007, _E006), this.m__E000);
		}
	}

	private void _E000()
	{
		if (this.m__E000 == null)
		{
			this.m__E000 = new GUIStyle
			{
				alignment = TextAnchor.UpperLeft
			};
			_E003 = _E001(2, 2, new Color(0.2f, 0.2f, 0.3f, 0.9f));
			this.m__E000.normal.background = _E003;
			this.m__E000.normal.textColor = Color.white;
		}
		if (this.m__E001 == null)
		{
			this.m__E001 = new GUIStyle
			{
				alignment = TextAnchor.UpperLeft
			};
			this.m__E001.normal.background = _E003;
			this.m__E001.normal.textColor = Color.red;
		}
		if (_E002 == null)
		{
			_E002 = new GUIStyle
			{
				alignment = TextAnchor.UpperLeft
			};
			_E002.normal.background = _E003;
			_E002.normal.textColor = Color.green;
		}
	}

	private Texture2D _E001(int width, int height, Color col)
	{
		Color[] array = new Color[width * height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = col;
		}
		Texture2D texture2D = new Texture2D(width, height);
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	private void OnDestroy()
	{
		UnityEngine.Object.DestroyImmediate(_E003);
	}
}
