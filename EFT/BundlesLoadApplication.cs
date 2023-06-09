using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.AssetsManager;
using EFT.InventoryLogic;
using Newtonsoft.Json;
using UnityEngine;

namespace EFT;

public class BundlesLoadApplication : AbstractApplication
{
	public string BundleName;

	public string BundleName1;

	private _ED0A m__E006;

	private Task _E007;

	private GameObject _E008;

	public string LocationLootFile;

	public bool LoadAllLoot;

	private string _E009 = _ED3E._E000(144853);

	[SerializeField]
	private int _maxConcurrentLoadOperations = 1;

	private _ED0B _E00A;

	private _E554.Location _E00B;

	public bool LoadLootFromTheStart;

	public string _itemTemplateId;

	protected override EUpdateQueue PlayerUpdateQueue => EUpdateQueue.Update;

	protected override _E316 CreateLogConfigurator()
	{
		return _E778.Create();
	}

	protected virtual async Task Start()
	{
		GameObject target = new GameObject(_ED3E._E000(144878));
		UnityEngine.Object.DontDestroyOnLoad(target);
		_E00A = new _ED0B(_maxConcurrentLoadOperations);
		Singleton<_ED0A>.Create(await _E5DC.CreateEasyAssets(target, null, _E00A, _ED3E._E000(108637), null));
		await _E5DC.CreateAssetsManager();
		this.m__E006 = Singleton<_ED0A>.Instance;
		if (LoadLootFromTheStart)
		{
			await _E001();
		}
	}

	private void OnValidate()
	{
		BundleName = BundleName.ToLower();
		BundleName1 = BundleName1.ToLower();
	}

	protected override void Update()
	{
		_E00A.MaxConcurrentOperations = _maxConcurrentLoadOperations;
		if (_E007 == null)
		{
			if (Input.GetKeyUp(KeyCode.Space))
			{
				_ED0E<_ED08>._E002 poolBundles = this.m__E006.Retain(BundleName, null, new _ECCE<float>(delegate
				{
				}));
				_E007 = _E000(poolBundles);
				Debug.Log(_ED3E._E000(144819));
			}
			else if (Input.GetKeyUp(KeyCode.Home))
			{
				_ED0E<_ED08>._E002 poolBundles2 = this.m__E006.Retain(BundleName1, null, new _ECCE<float>(delegate
				{
				}));
				_E007 = _E000(poolBundles2);
				Debug.Log(_ED3E._E000(144819));
			}
			else if (Input.GetKeyUp(KeyCode.Return))
			{
				_E001().HandleExceptions();
			}
		}
		else if (_E007.IsCompleted)
		{
			Debug.Log(_ED3E._E000(144769));
			_E007 = null;
		}
	}

	private static async Task _E000(_ED0E<_ED08>._E002 poolBundles)
	{
		await _E612.LoadBundles(new _ED0E<_ED08>._E002[1] { poolBundles });
	}

	protected void OnGUI()
	{
		GUILayout.Label(_E009);
		if (_E007 != null)
		{
			GUILayout.Label(_ED3E._E000(144801));
		}
	}

	private async Task _E001()
	{
		_E002(_ED3E._E000(148428));
		_E78D obj = _E78D._E000(_ED3E._E000(36632));
		await obj.Login(_ED3E._E000(36626), _ED3E._E000(36626));
		_E78D._E000 session = obj.Session;
		await session.LoadCustomization();
		_E63E obj2 = new _E63E();
		Singleton<_E63E>.Create(obj2);
		_E63B obj3 = await session.CreateItemFactory(obj2);
		Singleton<_E63B>.Create(obj3);
		string text = _E3A2.Load<TextAsset>(_ED3E._E000(144866)).text;
		if (Singleton<_E5CB>.Instantiated)
		{
			Singleton<_E5CB>.Release(Singleton<_E5CB>.Instance);
		}
		_E55C data = text.ParseJsonTo<_E2BF<_E55C>>(Array.Empty<JsonConverter>()).data;
		Singleton<_E5CB>.Create(data.Config);
		Singleton<_E63B>.Instance.SetItemPresets(data.ItemPresets.Values.ToArray());
		string text2 = _E3A2.Load<TextAsset>(_ED3E._E000(36829)).text;
		if (Singleton<_E5B7>.Instantiated)
		{
			Singleton<_E5B7>.Release(Singleton<_E5B7>.Instance);
		}
		Singleton<_E5B7>.Create(text2.ParseJsonTo<_E2BF<_E55D>>(Array.Empty<JsonConverter>()).data.ClientSettings);
		_E002(_ED3E._E000(144918));
		await session.LoadCustomization();
		_E002(_ED3E._E000(144906));
		Singleton<_E760>.Create(new _E760(Singleton<_ED0A>.Instance, obj3.ItemTemplates, null));
		_E3B7 config = _E2B6.Config;
		_E761 config2 = _E761.Create(config.Pools.AmmoPoolSize, config.Pools.WeaponsPoolSize, config.Pools.MagsPoolSize, config.Pools.ItemsPoolSize, config.Pools.PlayersPoolSize);
		Singleton<_E760>.Instance.RegisterPools(_E760.PoolsCategory.Raid, base.transform, config2);
		_E002(_ED3E._E000(144950));
		Item[] source;
		if (!LoadAllLoot)
		{
			_E00B = _E3A2.Load<TextAsset>(LocationLootFile).text.ParseJsonTo<_E554.Location>(Array.Empty<JsonConverter>());
			source = _E00B.Loot.Select((_E545 x) => x.Item).ToArray();
		}
		else
		{
			source = (from pair in data.ItemPresets
				select pair.Value.Item into item
				where item.TemplateId == _itemTemplateId || string.IsNullOrEmpty(_itemTemplateId)
				select item).ToArray();
		}
		ResourceKey[] resources = source.OfType<ContainerCollection>().GetAllItemsFromCollections().Concat(source.Where((Item x) => !(x is ContainerCollection)))
			.SelectMany((Item x) => x.Template.AllResources)
			.ToArray();
		await Singleton<_E760>.Instance.LoadBundlesAndCreatePools(_E760.PoolsCategory.Raid, _E760.AssemblyType.Online, resources, _ECE3.General, new _ECCE<_E5BB>(delegate(_E5BB p)
		{
			_E002(string.Format(_ED3E._E000(144838), p.Stage, p.Progress));
		}));
		_E002(_ED3E._E000(144933));
		Debug.LogError(_E37D.GetCounters());
		Debug.LogError(_ED3E._E000(144930) + GC.GetTotalMemory(forceFullCollection: true));
	}

	private void _E002(string log)
	{
		_E009 = log;
		Debug.Log(log);
	}

	private void _E003()
	{
		Item presetItem = Singleton<_E63B>.Instance.GetPresetItem(_itemTemplateId);
		_E008 = Singleton<_E760>.Instance.CreateItem(presetItem, isAnimated: false);
		_E008.transform.parent = null;
		_E008.SetActive(value: true);
	}

	private void _E004()
	{
		if (_E008 != null)
		{
			AssetPoolObject.ReturnToPool(_E008);
		}
		_E008 = null;
	}

	[CompilerGenerated]
	private bool _E005(Item item)
	{
		if (!(item.TemplateId == _itemTemplateId))
		{
			return string.IsNullOrEmpty(_itemTemplateId);
		}
		return true;
	}

	[CompilerGenerated]
	private void _E006(_E5BB p)
	{
		_E002(string.Format(_ED3E._E000(144838), p.Stage, p.Progress));
	}
}
