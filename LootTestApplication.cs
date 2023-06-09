using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT;
using EFT.InputSystem;
using EFT.InventoryLogic;
using UnityEngine;

public class LootTestApplication : ClientApplication<_E78D._E000>
{
	protected override async Task Start()
	{
		await base.Start();
		IOperation<_EC35, InputTree> obj = await _E77D.Execute(resetSettings: false, PlayerUpdateQueue, BundleLock);
		if (obj.Failed)
		{
			UnityEngine.Debug.LogError(_ED3E._E000(36601));
		}
		Result<_EC35, InputTree> result = obj.Result;
		ClientBackEnd = _E78D._E000(_ED3E._E000(36632));
		Init(result.Value0, result.Value1);
		await ClientBackEnd.Login(_ED3E._E000(36626), _ED3E._E000(36626));
		_E63B obj2 = await base.Session.CreateItemFactory(new _E63E());
		Singleton<_E63B>.Create(obj2);
		await base.Session.LoadCustomization();
		Singleton<_E760>.Create(new _E760(Singleton<_ED0A>.Instance, obj2.ItemTemplates, null));
		await _E5DB.Manager.LoadBundlesAsync((from x in obj2.ItemTemplates.Where((KeyValuePair<string, ItemTemplate> x) => x.Value.Prefab != null).SelectMany((KeyValuePair<string, ItemTemplate> x) => x.Value.AllResources)
			select x.path).ToArray());
		int num = 0;
		foreach (Item item in obj2.CreateAllItemsEver())
		{
			await Task.Yield();
			GameObject obj3 = Singleton<_E760>.Instance.CreateLootPrefab(item);
			obj3.transform.position = new Vector3(num % 16, 0f, (float)num / 16f);
			obj3.SetActive(value: true);
			num++;
		}
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _E000()
	{
		return base.Start();
	}
}
