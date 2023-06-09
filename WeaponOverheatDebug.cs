using Comfort.Common;
using EFT;
using EFT.InventoryLogic;
using EFT.UI;
using UnityEngine;

public class WeaponOverheatDebug : MonoBehaviour
{
	private GUIStyle m__E000;

	private GUIStyle m__E001;

	private GUIStyle _E002;

	private Texture2D _E003;

	private Player _E004;

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
			AmmoTemplate currentAmmoTemplate = item.CurrentAmmoTemplate;
			float lastShotOverheat = item.MalfState.LastShotOverheat;
			float lastShotTime = item.MalfState.LastShotTime;
			float num = _E62A.PastTime - item.MalfState.LastShotTime;
			float heatFactorGun = item.Template.HeatFactorGun;
			float coolFactorGun = item.Template.CoolFactorGun;
			float num2 = currentAmmoTemplate?.HeatFactor ?? 1f;
			_E5CB._E02D overheat = Singleton<_E5CB>.Instance.Overheat;
			float modHeatFactor = overheat.ModHeatFactor;
			float modCoolFactor = overheat.ModCoolFactor;
			float modsCoolFactor;
			float currentOverheat = item.GetCurrentOverheat(_E62A.PastTime, overheat, out modsCoolFactor);
			float modsHeatFactor;
			float shotOverheat = item.GetShotOverheat(num2, modHeatFactor, out modsHeatFactor);
			if (!item.AllowOverheat)
			{
				GUI.Box(new Rect(1010f, 35f, 320f, 20f), _ED3E._E000(59386), this.m__E000);
				return;
			}
			GUI.Box(new Rect(1010f, 35f, 240f, 20f), string.Format(_ED3E._E000(57350), currentOverheat), (currentOverheat < 100f) ? _E002 : this.m__E001);
			GUI.Box(new Rect(1010f, 55f, 240f, 20f), string.Format(_ED3E._E000(57396), lastShotOverheat), this.m__E000);
			GUI.Box(new Rect(1010f, 75f, 240f, 20f), string.Format(_ED3E._E000(57436), lastShotTime), this.m__E000);
			GUI.Box(new Rect(1010f, 95f, 240f, 20f), string.Format(_ED3E._E000(57416), num), this.m__E000);
			GUI.Box(new Rect(1010f, 115f, 240f, 20f), string.Format(_ED3E._E000(57457), shotOverheat), this.m__E000);
			GUI.Box(new Rect(1010f, 135f, 240f, 20f), string.Format(_ED3E._E000(57497), heatFactorGun), this.m__E000);
			GUI.Box(new Rect(1010f, 155f, 240f, 20f), string.Format(_ED3E._E000(57472), coolFactorGun), this.m__E000);
			GUI.Box(new Rect(1010f, 175f, 240f, 20f), string.Format(_ED3E._E000(57519), num2), this.m__E000);
			GUI.Box(new Rect(1010f, 195f, 240f, 20f), string.Format(_ED3E._E000(57556), modsHeatFactor), this.m__E000);
			GUI.Box(new Rect(1010f, 215f, 240f, 20f), string.Format(_ED3E._E000(57537), modsCoolFactor), this.m__E000);
			GUI.Box(new Rect(1010f, 235f, 240f, 20f), string.Format(_ED3E._E000(57582), modHeatFactor), this.m__E000);
			GUI.Box(new Rect(1010f, 255f, 240f, 20f), string.Format(_ED3E._E000(57617), modCoolFactor), this.m__E000);
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
		Object.DestroyImmediate(_E003);
	}
}
