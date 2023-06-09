using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Comfort.Common;
using EFT;
using EFT.InventoryLogic;
using EFT.UI;
using UnityEngine;

public class MalfunctionDebug : MonoBehaviour
{
	private GUIStyle m__E000;

	private GUIStyle m__E001;

	private GUIStyle m__E002;

	private Texture2D m__E003;

	private Player m__E004;

	private float m__E005 = -1f;

	private int _E006;

	private List<_E375<Weapon.EMalfunctionSource>._E000<float, Weapon.EMalfunctionSource>> _E007 = new List<_E375<Weapon.EMalfunctionSource>._E000<float, Weapon.EMalfunctionSource>>(5);

	private Dictionary<Weapon, int> _E008 = new Dictionary<Weapon, int>();

	private Dictionary<Weapon, int> _E009 = new Dictionary<Weapon, int>();

	public static void SimulateShotsAndGatherStats(int shotsCount, Player.FirearmController firearmController, string filePath = null)
	{
		float ammoMalfChanceMult = Singleton<_E5CB>.Instance.Malfunction.AmmoMalfChanceMult;
		float magazineMalfChanceMult = Singleton<_E5CB>.Instance.Malfunction.MagazineMalfChanceMult;
		Weapon item = firearmController.Item;
		_EA12 ammoToFire = item.Chambers[0].ContainedItem as _EA12;
		bool flag = item.GetCurrentMagazine() != null;
		bool hasAmmoInMag = flag && item.GetCurrentMagazine().Count > 0;
		bool isBoltCatch = item.IsBoltCatch;
		int num = 0;
		int num2 = 0;
		List<int> list = new List<int>();
		Dictionary<Weapon.EMalfunctionState, int> dictionary = new Dictionary<Weapon.EMalfunctionState, int>();
		float weaponDurability = Mathf.Floor(item.Repairable.Durability / item.Repairable.MaxDurability * 100f);
		_E5CB instance = Singleton<_E5CB>.Instance;
		float modsCoolFactor;
		float currentOverheat = item.GetCurrentOverheat(_E62A.PastTime, instance.Overheat, out modsCoolFactor);
		for (int i = 0; i < shotsCount; i++)
		{
			Weapon.EMalfunctionSource malfunctionSource;
			Weapon.EMalfunctionState malfunctionState = firearmController.GetMalfunctionState(ammoToFire, hasAmmoInMag, isBoltCatch, flag, currentOverheat, instance.Overheat.FixSlideOverheat, out malfunctionSource);
			if (malfunctionState == Weapon.EMalfunctionState.None)
			{
				num2++;
				continue;
			}
			if (dictionary.ContainsKey(malfunctionState))
			{
				dictionary[malfunctionState]++;
			}
			else
			{
				dictionary.Add(malfunctionState, 1);
			}
			list.Add(num2);
			num2 = 0;
			num++;
		}
		int num3 = 0;
		int num4 = int.MinValue;
		int num5 = int.MaxValue;
		double durabilityMalfChance;
		float overheatMalfChance;
		float magMalfChance;
		float ammoMalfChance;
		float totalMalfunctionChance = firearmController.GetTotalMalfunctionChance(ammoToFire, currentOverheat, out durabilityMalfChance, out magMalfChance, out ammoMalfChance, out overheatMalfChance, out weaponDurability);
		magMalfChance /= magazineMalfChanceMult;
		ammoMalfChance /= ammoMalfChanceMult;
		List<_E375<Weapon.EMalfunctionSource>._E000<float, Weapon.EMalfunctionSource>> list2 = new List<_E375<Weapon.EMalfunctionSource>._E000<float, Weapon.EMalfunctionSource>>();
		firearmController.GetMalfunctionSources(list2, durabilityMalfChance, magMalfChance, ammoMalfChance, overheatMalfChance, hasAmmoInMag, flag);
		foreach (int item2 in list)
		{
			num3 += item2;
			num4 = Mathf.Max(item2, num4);
			num5 = Mathf.Min(item2, num5);
		}
		num3 = ((list.Count != 0) ? (num3 / list.Count) : 0);
		Debug.Log(_ED3E._E000(62422) + shotsCount + _ED3E._E000(62408) + item.Name.Localized() + _ED3E._E000(62459));
		Debug.Log(_ED3E._E000(62445) + item.BaseMalfunctionChance);
		Debug.Log(_ED3E._E000(64539) + magMalfChance);
		Debug.Log(_ED3E._E000(64573) + magazineMalfChanceMult);
		Debug.Log(_ED3E._E000(64550) + ammoMalfChance);
		Debug.Log(_ED3E._E000(64588) + ammoMalfChanceMult);
		Debug.Log(_ED3E._E000(64630) + weaponDurability);
		Debug.Log(_ED3E._E000(64609) + overheatMalfChance);
		Debug.Log(_ED3E._E000(64655) + totalMalfunctionChance);
		foreach (_E375<Weapon.EMalfunctionSource>._E000<float, Weapon.EMalfunctionSource> item3 in list2)
		{
			Debug.Log(item3.Item2.ToString().ToLower() + _ED3E._E000(64689) + item3.Item1);
		}
		Debug.Log(_ED3E._E000(64682) + num);
		Debug.Log(_ED3E._E000(64727) + num4);
		Debug.Log(_ED3E._E000(64705) + num5);
		Debug.Log(_ED3E._E000(64755) + num3);
		foreach (KeyValuePair<Weapon.EMalfunctionState, int> item4 in dictionary)
		{
			Debug.Log(item4.Key.ToString().ToLower() + _ED3E._E000(64797) + item4.Value + _ED3E._E000(64791));
		}
		if (string.IsNullOrEmpty(filePath))
		{
			return;
		}
		string text = _ED3E._E000(62422) + shotsCount + _ED3E._E000(62408) + item.Name.Localized() + _ED3E._E000(62459);
		text = text + _ED3E._E000(64783) + item.BaseMalfunctionChance;
		text = text + _ED3E._E000(64822) + magMalfChance;
		text = text + _ED3E._E000(64857) + magazineMalfChanceMult;
		text = text + _ED3E._E000(64835) + ammoMalfChance;
		text = text + _ED3E._E000(64874) + ammoMalfChanceMult;
		text = text + _ED3E._E000(64909) + weaponDurability;
		Debug.Log(_ED3E._E000(64609) + overheatMalfChance);
		text = text + _ED3E._E000(64953) + totalMalfunctionChance;
		foreach (_E375<Weapon.EMalfunctionSource>._E000<float, Weapon.EMalfunctionSource> item5 in list2)
		{
			text = text + _ED3E._E000(2540) + item5.Item2.ToString().ToLower() + _ED3E._E000(64689) + item5.Item1;
		}
		text = text + _ED3E._E000(64988) + num;
		text = text + _ED3E._E000(64970) + num4;
		text = text + _ED3E._E000(65013) + num5;
		text = text + _ED3E._E000(64992) + num3;
		foreach (KeyValuePair<Weapon.EMalfunctionState, int> item6 in dictionary)
		{
			text = text + _ED3E._E000(2540) + item6.Key.ToString().ToLower() + _ED3E._E000(64797) + item6.Value + _ED3E._E000(64791);
		}
		text += _ED3E._E000(65043);
		foreach (int item7 in list)
		{
			text = text + _ED3E._E000(2540) + item7;
		}
		File.WriteAllText(filePath, text);
	}

	public void SetDebugObject(Player toDebug)
	{
		this.m__E004 = toDebug;
		_E000();
	}

	private void _E000()
	{
		if (this.m__E000 == null)
		{
			this.m__E000 = new GUIStyle
			{
				alignment = TextAnchor.UpperLeft
			};
			this.m__E003 = _E005(2, 2, new Color(0.2f, 0.2f, 0.3f, 0.9f));
			this.m__E000.normal.background = this.m__E003;
			this.m__E000.normal.textColor = Color.white;
		}
		if (this.m__E001 == null)
		{
			this.m__E001 = new GUIStyle
			{
				alignment = TextAnchor.UpperLeft
			};
			this.m__E001.normal.background = this.m__E003;
			this.m__E001.normal.textColor = Color.red;
		}
		if (this.m__E002 == null)
		{
			this.m__E002 = new GUIStyle
			{
				alignment = TextAnchor.UpperLeft
			};
			this.m__E002.normal.background = this.m__E003;
			this.m__E002.normal.textColor = Color.green;
		}
	}

	private void _E001(int value)
	{
		_E006 = value;
	}

	private int _E002(int incVal)
	{
		int result = _E006;
		_E006 += incVal;
		return result;
	}

	private void OnGUI()
	{
		if (this.m__E004 == null || MonoBehaviourSingleton<PreloaderUI>.Instance.Console.IsConsoleVisible)
		{
			return;
		}
		Player.FirearmController firearmController = this.m__E004.HandsController as Player.FirearmController;
		if (firearmController == null)
		{
			return;
		}
		bool isTriggerPressed = firearmController.IsTriggerPressed;
		Weapon item = firearmController.Item;
		bool flag = item.IsBoltCatch && _E326.GetBoolBoltCatch(firearmController.FirearmsAnimator.Animator);
		float floatAmmoInChamber = _E326.GetFloatAmmoInChamber(firearmController.FirearmsAnimator.Animator);
		float floatAmmoInMag = _E326.GetFloatAmmoInMag(firearmController.FirearmsAnimator.Animator);
		if (item.GetCurrentMagazine() is _EB13 obj)
		{
			string text = _ED3E._E000(65063);
			int num = 0;
			float num2 = (item.CylinderHammerClosed ? (item.DoubleActionAccuracyPenalty * (1f - firearmController.BuffInfo.DoubleActionRecoilReduce) * item.StockDoubleActionAccuracyPenaltyMult) : 0f);
			Vector2 recoilStrengthXy = this.m__E004.ProceduralWeaponAnimation.Shootingg.RecoilStrengthXy;
			_E001(420);
			GUI.Box(new Rect(10f, _E002(20), 320f, 20f), _ED3E._E000(65117) + item.CylinderHammerClosed.ToString(CultureInfo.InvariantCulture), this.m__E002);
			GUI.Box(new Rect(10f, _E002(20), 320f, 20f), _ED3E._E000(65101) + num2.ToString(CultureInfo.InvariantCulture), this.m__E002);
			GUI.Box(new Rect(10f, _E002(20), 320f, 20f), _ED3E._E000(65132) + recoilStrengthXy, this.m__E002);
			GUI.Box(new Rect(10f, _E002(20), 320f, 20f), _ED3E._E000(65122) + this.m__E004.ProceduralWeaponAnimation.AimingSpeed, this.m__E002);
			for (int i = 0; i < obj.Camoras.Length; i++)
			{
				num++;
				Rect position = new Rect(10f, 500 + num * 20, 320f, 20f);
				string text2 = ((obj.Camoras[i].ContainedItem != null) ? obj.Camoras[i].ContainedItem.Name : text);
				GUI.Box(position, text2, (obj.CurrentCamoraIndex == i) ? this.m__E002 : this.m__E000);
			}
		}
		int shortNameHash = firearmController.FirearmsAnimator.Animator.GetCurrentAnimatorStateInfo(1).shortNameHash;
		string animStateByNameHash = _E327.GetAnimStateByNameHash(shortNameHash);
		string currentHandsOperationName = (this.m__E004.HandsController as Player.ItemHandsController).CurrentHandsOperationName;
		_E001(35);
		GUI.Box(new Rect(10f, _E002(20), 220f, 20f), item.ShortName.Localized(), this.m__E000);
		GUI.Box(new Rect(10f, _E002(20), 220f, 20f), currentHandsOperationName, this.m__E000);
		GUI.Box(new Rect(10f, _E002(20), 220f, 20f), string.Format(_ED3E._E000(65175), animStateByNameHash, shortNameHash), this.m__E000);
		if (!item.AllowMalfunction)
		{
			GUI.Box(new Rect(10f, 35f, 220f, 20f), _ED3E._E000(65161), this.m__E000);
			return;
		}
		_E5CB instance = Singleton<_E5CB>.Instance;
		float modsCoolFactor;
		float currentOverheat = item.GetCurrentOverheat(_E62A.PastTime, instance.Overheat, out modsCoolFactor);
		_EA12 obj2 = item.Chambers[0].ContainedItem as _EA12;
		bool flag2 = item.GetCurrentMagazine() != null;
		bool flag3 = flag2 && item.GetCurrentMagazine().Count > 0;
		bool isBoltCatch = item.IsBoltCatch;
		float nextMalfunctionRandom = firearmController.GetNextMalfunctionRandom();
		double durabilityMalfChance;
		float magMalfChance;
		float ammoMalfChance;
		float overheatMalfChance;
		float weaponDurability;
		float totalMalfunctionChance = firearmController.GetTotalMalfunctionChance(obj2, currentOverheat, out durabilityMalfChance, out magMalfChance, out ammoMalfChance, out overheatMalfChance, out weaponDurability);
		if (!_E008.ContainsKey(item))
		{
			_E008.Add(item, -1);
		}
		if (!_E009.ContainsKey(item))
		{
			_E009.Add(item, 0);
		}
		if (Math.Abs(this.m__E005 - nextMalfunctionRandom) > Mathf.Epsilon)
		{
			_E008[item]++;
			_E009[item] = _E008[item];
			if (item.MalfState.State != 0)
			{
				_E008[item] = 0;
			}
		}
		int num3 = ((item.MalfState.State == Weapon.EMalfunctionState.None) ? _E008[item] : _E009[item]);
		_E001(95);
		GUI.Box(new Rect(10f, _E002(20), 220f, 20f), _ED3E._E000(65231) + num3, this.m__E000);
		GUI.Box(new Rect(10f, _E002(20), 220f, 20f), _ED3E._E000(65265) + isTriggerPressed, this.m__E000);
		GUI.Box(new Rect(10f, _E002(20), 220f, 20f), _ED3E._E000(65251) + flag, this.m__E000);
		GUI.Box(new Rect(10f, _E002(20), 220f, 20f), _ED3E._E000(65303) + totalMalfunctionChance, this.m__E000);
		string text3 = ((obj2 == null) ? _ED3E._E000(65338) : _ED3E._E000(65285));
		if (flag2)
		{
			_EA6A currentMagazine = item.GetCurrentMagazine();
			GUI.Box(new Rect(10f, _E002(20), 220f, 20f), string.Format(_ED3E._E000(65320), currentMagazine.Count, currentMagazine.MaxCount, text3), this.m__E000);
		}
		else
		{
			GUI.Box(new Rect(10f, _E002(20), 220f, 20f), text3 ?? "", this.m__E000);
		}
		if (flag2)
		{
			GUI.Box(new Rect(10f, _E002(20), 220f, 20f), string.Format(_ED3E._E000(65360), floatAmmoInMag, floatAmmoInChamber), this.m__E000);
		}
		else
		{
			GUI.Box(new Rect(10f, _E002(20), 220f, 20f), string.Format(_ED3E._E000(65394), floatAmmoInChamber), this.m__E000);
		}
		if (item.MalfState.State == Weapon.EMalfunctionState.None)
		{
			GUI.Box(new Rect(10f, _E002(20), 220f, 20f), _ED3E._E000(65379) + nextMalfunctionRandom, this.m__E000);
		}
		else
		{
			GUI.Box(new Rect(10f, _E002(20), 220f, 20f), _ED3E._E000(65427) + this.m__E005, this.m__E000);
		}
		if (item.MalfState.State != 0)
		{
			GUI.Box(new Rect(10f, _E002(20), 220f, 20f), _ED3E._E000(65470) + item.MalfState.State, this.m__E001);
			bool flag4 = item.MalfState.IsKnownMalfunction(this.m__E004.Profile.Id);
			bool flag5 = item.MalfState.IsKnownMalfType(this.m__E004.Profile.Id);
			GUI.Box(new Rect(10f, _E002(20), 220f, 20f), _ED3E._E000(65454) + flag4, this.m__E000);
			GUI.Box(new Rect(10f, _E002(20), 220f, 20f), _ED3E._E000(65493) + flag5, this.m__E000);
			float lastMalfunctionTime = item.MalfState.LastMalfunctionTime;
			bool flag6 = item.CanQuickdrawPistolAfterMalf(_E62A.PastTime, instance.Malfunction);
			GUI.Box(new Rect(10f, _E002(20), 220f, 20f), _ED3E._E000(65474) + lastMalfunctionTime, this.m__E000);
			GUI.Box(new Rect(10f, _E002(20), 220f, 20f), _ED3E._E000(65523) + flag6, flag6 ? this.m__E002 : this.m__E001);
		}
		else if (item.MalfState.SlideOnOverheatReached && currentOverheat >= instance.Overheat.FixSlideOverheat)
		{
			GUI.Box(new Rect(10f, _E002(20), 220f, 20f), _ED3E._E000(63514), this.m__E001);
		}
		else if (nextMalfunctionRandom <= totalMalfunctionChance)
		{
			GUI.Box(new Rect(10f, _E002(20), 220f, 20f), _ED3E._E000(63551), this.m__E001);
		}
		_E007.Clear();
		firearmController.GetMalfunctionSources(_E007, durabilityMalfChance, magMalfChance, ammoMalfChance, overheatMalfChance, flag3, flag2);
		if (nextMalfunctionRandom <= totalMalfunctionChance && item.MalfState.State == Weapon.EMalfunctionState.None)
		{
			Weapon.EMalfunctionSource eMalfunctionSource = _E375<Weapon.EMalfunctionSource>.GenerateDrop(_E007, nextMalfunctionRandom);
			_E001(35);
			GUI.Box(new Rect(230f, _E002(20), 220f, 120f), _ED3E._E000(63520) + eMalfunctionSource, this.m__E002);
			GUI.Box(new Rect(230f, _E002(20), 220f, 120f), _E003(_E007), this.m__E000);
			bool shouldCheckJam = flag3 || !isBoltCatch || !flag2;
			List<_E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>> list = new List<_E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>>();
			firearmController.GetSpecificMalfunctionVariants(list, obj2, eMalfunctionSource, weaponDurability, flag3, flag2, shouldCheckJam);
			GUI.Box(new Rect(230f, 175f, 220f, 120f), _E004(eMalfunctionSource, list), this.m__E000);
			GUI.Box(new Rect(230f, 55f, 220f, 120f), _E003(_E007), this.m__E000);
		}
		else if (item.MalfState.State != 0)
		{
			GUI.Box(new Rect(230f, 35f, 220f, 120f), _ED3E._E000(63520) + item.MalfState.Source, this.m__E002);
		}
		else
		{
			GUI.Box(new Rect(230f, 35f, 220f, 120f), _E003(_E007), this.m__E000);
		}
		this.m__E005 = nextMalfunctionRandom;
	}

	private string _E003(List<_E375<Weapon.EMalfunctionSource>._E000<float, Weapon.EMalfunctionSource>> sources)
	{
		string text = _ED3E._E000(63568);
		foreach (_E375<Weapon.EMalfunctionSource>._E000<float, Weapon.EMalfunctionSource> source in sources)
		{
			text = string.Concat(text, _ED3E._E000(2540), source.Item2, _ED3E._E000(18502), source.Item1);
		}
		return text;
	}

	private string _E004(Weapon.EMalfunctionSource currentSource, List<_E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState>> states)
	{
		string text = _ED3E._E000(63613) + currentSource;
		foreach (_E375<Weapon.EMalfunctionState>._E000<float, Weapon.EMalfunctionState> state in states)
		{
			text = string.Concat(text, _ED3E._E000(2540), state.Item2, _ED3E._E000(18502), state.Item1);
		}
		return text;
	}

	private Texture2D _E005(int width, int height, Color col)
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
		UnityEngine.Object.DestroyImmediate(this.m__E003);
	}
}
