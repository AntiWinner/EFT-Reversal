using System;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI.DragAndDrop;

public class ItemViewFactory : MonoBehaviour
{
	public static readonly Quaternion VerticalRotation = Quaternion.Euler(0f, 0f, 270f);

	public static readonly Quaternion HorizontalRotation = Quaternion.identity;

	public const int CellSize = 62;

	public const int BorderSize = 1;

	public const string PrefabLayoutsPath = "Prefabs/UGUI/Layouts/";

	public static T CreateFromPool<T>(string prefabName) where T : UnityEngine.Object
	{
		return MonoBehaviourSingleton<UiPools>.Instance.GetGameObject<T>(_ED3E._E000(237163) + prefabName);
	}

	public static T CreateFromPrefab<T>(string prefabName) where T : UnityEngine.Object
	{
		return UnityEngine.Object.Instantiate(_E905.Pop<T>(_ED3E._E000(237163) + prefabName));
	}

	public static _E313 GetCellPixelSize(_E313 size)
	{
		return new _E313(size.X * 63 + 1, size.Y * 63 + 1);
	}

	public static _E3E2 LoadItemIcon(Item item, int scaleFactor = 1, bool forcedGeneration = false)
	{
		ResourceKey prefab = item.Prefab;
		if (prefab == null || string.IsNullOrEmpty(prefab.path))
		{
			Sprite sprite = _E905.Pop<Sprite>(_ED3E._E000(237201));
			return new _E3E2(_E3E1.Default)
			{
				Sprite = sprite
			};
		}
		_E3E4 instance = Singleton<_E3E4>.Instance;
		_E313 size = scaleFactor * GetCellPixelSize(item.CalculateCellSize());
		return instance.GetItemIcon(item, in size, forcedGeneration);
	}

	public static async Task<Sprite> GetItemSpriteAsync(Item item, int scaleFactor = 1)
	{
		ResourceKey prefab = item.Prefab;
		if (prefab == null || string.IsNullOrEmpty(prefab.path))
		{
			return _E905.Pop<Sprite>(_ED3E._E000(237201));
		}
		_E313 size = scaleFactor * GetCellPixelSize(item.CalculateCellSize());
		return await Singleton<_E3E4>.Instance.GetItemSpriteAsync(item, size);
	}

	[CanBeNull]
	public static Sprite LoadModIconSprite(Item item)
	{
		return LoadModIconSprite(item.GetType());
	}

	[CanBeNull]
	public static Sprite LoadModIconSprite(Type type)
	{
		string modIconName = GetModIconName(type);
		if (string.IsNullOrEmpty(modIconName))
		{
			return null;
		}
		return _E905.Pop<Sprite>(_ED3E._E000(237198) + modIconName);
	}

	public static string GetModIconName(Item item)
	{
		return GetModIconName(item.GetType());
	}

	public static string GetModIconName(Type type)
	{
		return GetModSubclass(type) switch
		{
			EModSubclass.Bipod => _ED3E._E000(237235), 
			EModSubclass.IronSight => _ED3E._E000(237225), 
			EModSubclass.Launcher => _ED3E._E000(237219), 
			EModSubclass.LightLaser => _ED3E._E000(237268), 
			EModSubclass.Mount => _ED3E._E000(237263), 
			EModSubclass.MuzzleMod => _ED3E._E000(105927), 
			EModSubclass.Railcovers => _ED3E._E000(237253), 
			EModSubclass.SightMod => _ED3E._E000(237304), 
			EModSubclass.TacticalCombo => _ED3E._E000(237302), 
			EModSubclass.Magazine => _ED3E._E000(237295), 
			EModSubclass.Gasblock => _ED3E._E000(237280), 
			EModSubclass.Handguard => _ED3E._E000(237337), 
			EModSubclass.Receiver => _ED3E._E000(237331), 
			EModSubclass.PistolGrip => _ED3E._E000(237316), 
			EModSubclass.Auxiliary => _ED3E._E000(237368), 
			EModSubclass.Charge => _ED3E._E000(237364), 
			EModSubclass.Stock => _ED3E._E000(237363), 
			EModSubclass.Barrel => _ED3E._E000(237353), 
			_ => "", 
		};
	}

	public static EModSubclass GetModSubclass(Type modType)
	{
		if (typeof(_EA34).IsAssignableFrom(modType))
		{
			return EModSubclass.Bipod;
		}
		if (typeof(_EA58).IsAssignableFrom(modType))
		{
			return EModSubclass.IronSight;
		}
		if (typeof(_EA62).IsAssignableFrom(modType))
		{
			return EModSubclass.Launcher;
		}
		if (typeof(_EA64).IsAssignableFrom(modType))
		{
			return EModSubclass.LightLaser;
		}
		if (typeof(_EA78).IsAssignableFrom(modType))
		{
			return EModSubclass.Mount;
		}
		if (typeof(_EACB).IsAssignableFrom(modType))
		{
			return EModSubclass.MuzzleMod;
		}
		if (typeof(_EA89).IsAssignableFrom(modType))
		{
			return EModSubclass.Railcovers;
		}
		if (typeof(_EAD5).IsAssignableFrom(modType))
		{
			return EModSubclass.SightMod;
		}
		if (typeof(_EAA8).IsAssignableFrom(modType) || typeof(_EA4A).IsAssignableFrom(modType))
		{
			return EModSubclass.TacticalCombo;
		}
		if (typeof(_EA6A).IsAssignableFrom(modType))
		{
			return EModSubclass.Magazine;
		}
		if (typeof(_EA4E).IsAssignableFrom(modType))
		{
			return EModSubclass.Gasblock;
		}
		if (typeof(_EA52).IsAssignableFrom(modType))
		{
			return EModSubclass.Handguard;
		}
		if (typeof(_EA8B).IsAssignableFrom(modType))
		{
			return EModSubclass.Receiver;
		}
		if (typeof(_EA7E).IsAssignableFrom(modType))
		{
			return EModSubclass.PistolGrip;
		}
		if (typeof(_EA46).IsAssignableFrom(modType))
		{
			return EModSubclass.LightLaser;
		}
		if (typeof(_EA1A).IsAssignableFrom(modType))
		{
			return EModSubclass.Auxiliary;
		}
		if (typeof(_EA36).IsAssignableFrom(modType))
		{
			return EModSubclass.Charge;
		}
		if (typeof(_EAA6).IsAssignableFrom(modType))
		{
			return EModSubclass.Stock;
		}
		if (typeof(_EA1C).IsAssignableFrom(modType))
		{
			return EModSubclass.Barrel;
		}
		return EModSubclass.None;
	}

	[CanBeNull]
	public static Sprite LoadModClassIconSprite(Item item)
	{
		return LoadModClassIconSprite(item.GetType());
	}

	[CanBeNull]
	public static Sprite LoadModClassIconSprite(Type type)
	{
		string modClassIconName = GetModClassIconName(type);
		if (string.IsNullOrEmpty(modClassIconName))
		{
			return null;
		}
		return _E905.Pop<Sprite>(_ED3E._E000(237344) + modClassIconName);
	}

	public static string GetModClassIconName(Item item)
	{
		return GetModClassIconName(item.GetType());
	}

	public static string GetModClassIconName(Type type)
	{
		return GetModClass(type) switch
		{
			EModClass.Functional => _ED3E._E000(237385), 
			EModClass.Gear => _ED3E._E000(237377), 
			EModClass.Master => _ED3E._E000(237438), 
			EModClass.Auxiliary => _ED3E._E000(237377), 
			_ => "", 
		};
	}

	public static EModClass GetModClass(Type modType)
	{
		if (typeof(_EA4C).IsAssignableFrom(modType))
		{
			return EModClass.Functional;
		}
		if (typeof(_EA50).IsAssignableFrom(modType))
		{
			return EModClass.Gear;
		}
		if (typeof(_EA6E).IsAssignableFrom(modType))
		{
			return EModClass.Master;
		}
		if (typeof(_EA1A).IsAssignableFrom(modType))
		{
			return EModClass.Auxiliary;
		}
		return EModClass.None;
	}

	public static EModClass GetSlotModClass(EWeaponModType modType)
	{
		switch (modType)
		{
		case EWeaponModType.mod_mount:
		case EWeaponModType.mod_stock:
		case EWeaponModType.mod_magazine:
		case EWeaponModType.mod_barrel:
		case EWeaponModType.mod_handguard:
		case EWeaponModType.mod_reciever:
		case EWeaponModType.mod_charge:
		case EWeaponModType.mod_pistol_grip:
		case EWeaponModType.mod_launcher:
		case EWeaponModType.mod_mag_shaft:
			return EModClass.Gear;
		default:
			return EModClass.Functional;
		}
	}

	public static string GetModSlotName(EWeaponModType modType)
	{
		return modType switch
		{
			EWeaponModType.mod_magazine => _ED3E._E000(237428), 
			EWeaponModType.mod_barrel => _ED3E._E000(237421), 
			EWeaponModType.mod_foregrip => _ED3E._E000(237412), 
			EWeaponModType.mod_reciever => _ED3E._E000(237469), 
			EWeaponModType.mod_stock => _ED3E._E000(64016), 
			EWeaponModType.mod_charge => _ED3E._E000(237462), 
			EWeaponModType.mod_pistol_grip => _ED3E._E000(237453), 
			EWeaponModType.mod_mag_shaft => _ED3E._E000(237451), 
			_ => modType.ToString(), 
		};
	}

	public static bool IsSecureContainer(Item item)
	{
		if (item is _EA74)
		{
			return ((_EA74)item).isSecured;
		}
		return false;
	}

	public static string GetSpecialIcon(Item item)
	{
		if (item is _EA74)
		{
			return _ED3E._E000(212891);
		}
		if (item is _EA9A)
		{
			return _ED3E._E000(237441);
		}
		if (item is _EA8D)
		{
			return _ED3E._E000(237441);
		}
		return "";
	}

	[CanBeNull]
	public static Sprite LoadItemTypeSprite(Type type)
	{
		Sprite itemTypeIcon = EFTHardSettings.Instance.StaticIcons.GetItemTypeIcon(GetItemType(type));
		if (itemTypeIcon == null)
		{
			Debug.LogWarning(_ED3E._E000(237494) + type);
		}
		return itemTypeIcon;
	}

	public static EItemType GetItemType(Type itemType)
	{
		if (typeof(_EA12).IsAssignableFrom(itemType) || typeof(AmmoBox).IsAssignableFrom(itemType))
		{
			return EItemType.Ammo;
		}
		if (typeof(_EA1E).IsAssignableFrom(itemType))
		{
			return EItemType.Barter;
		}
		if (typeof(_EA74).IsAssignableFrom(itemType))
		{
			return EItemType.Container;
		}
		if (typeof(_EA48).IsAssignableFrom(itemType))
		{
			return EItemType.Food;
		}
		if (typeof(_EAB3).IsAssignableFrom(itemType))
		{
			return EItemType.Backpack;
		}
		if (typeof(_EAE3).IsAssignableFrom(itemType))
		{
			return EItemType.Goggles;
		}
		if (typeof(_EA80).IsAssignableFrom(itemType))
		{
			return EItemType.Rig;
		}
		if (typeof(_EAE1).IsAssignableFrom(itemType))
		{
			return EItemType.Equipment;
		}
		if (typeof(_EA16).IsAssignableFrom(itemType))
		{
			return EItemType.Armor;
		}
		if (typeof(_EA42).IsAssignableFrom(itemType))
		{
			return EItemType.Equipment;
		}
		if (typeof(_EADF).IsAssignableFrom(itemType))
		{
			return EItemType.Grenade;
		}
		if (typeof(_EAC1).IsAssignableFrom(itemType))
		{
			return EItemType.Info;
		}
		if (typeof(_EA6C).IsAssignableFrom(itemType))
		{
			return EItemType.Info;
		}
		if (typeof(_EA5B).IsAssignableFrom(itemType))
		{
			return EItemType.Keys;
		}
		if (typeof(_EA60).IsAssignableFrom(itemType))
		{
			return EItemType.Knife;
		}
		if (typeof(_EA6A).IsAssignableFrom(itemType))
		{
			return EItemType.Magazine;
		}
		if (typeof(_EA72).IsAssignableFrom(itemType))
		{
			return EItemType.Meds;
		}
		if (typeof(Mod).IsAssignableFrom(itemType))
		{
			return EItemType.Mod;
		}
		if (typeof(_EA9A).IsAssignableFrom(itemType))
		{
			return EItemType.Special;
		}
		if (typeof(Weapon).IsAssignableFrom(itemType))
		{
			return EItemType.Weapon;
		}
		return EItemType.None;
	}
}
