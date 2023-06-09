using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT;
using EFT.InputSystem;
using EFT.UI;
using Newtonsoft.Json;
using UnityEngine;

public class UtilityApplication : ClientApplication<_E78D._E000>
{
	[SerializeField]
	private string _locale = _ED3E._E000(36786);

	[SerializeField]
	private bool _loadTemplatesFromBackend = true;

	[SerializeField]
	private bool _dropCache;

	protected _E63B ItemFactory;

	protected _E63E ItemRegistry;

	protected override async Task Start()
	{
		await base.Start();
		await _E000();
	}

	private async Task _E000()
	{
		_E77D obj = _E77D.Execute(resetSettings: false, PlayerUpdateQueue, BundleLock);
		await obj;
		if (obj.Failed)
		{
			throw new NotImplementedException();
		}
		if (_dropCache)
		{
			new WebClient().DownloadString(_E2B6.BackendUrl + _ED3E._E000(36781));
		}
		Result<_EC35, InputTree> result = obj.Result;
		ClientBackEnd = _E78D._E000(_ED3E._E000(36632), 7777, _loadTemplatesFromBackend);
		Init(result.Value0, result.Value1);
		Result<_E3C4> result2 = await ClientBackEnd.Login("", "");
		if (result2.Failed)
		{
			throw new NotImplementedException();
		}
		_E3C4 value = result2.Value;
		_E7AD._E010._E011 = _locale;
		while (base.Session == null && value == null)
		{
			await Task.Yield();
		}
		await _E77F.ReloadLocale(base.Session);
		ItemRegistry = new _E63E();
		ItemFactory = await base.Session.CreateItemFactory(ItemRegistry);
		Singleton<_E63B>.Create(ItemFactory);
		Singleton<_E63E>.Create(ItemRegistry);
		_ED0A instance = Singleton<_ED0A>.Instance;
		_E760 obj2 = new _E760(instance, ItemFactory.ItemTemplates, null);
		Singleton<_E760>.Create(obj2);
		await base.gameObject.AddComponent<UiPools>().Init();
		_E3E4 obj3 = new _E3E4(instance, obj2);
		await obj3.Init();
		Singleton<_E3E4>.Create(obj3);
		await base.Session.LoadCustomization();
		if (Singleton<_E5CB>.Instantiated)
		{
			Singleton<_E5CB>.Release(Singleton<_E5CB>.Instance);
		}
		Singleton<_E5CB>.Create((await base.Session.GetGlobalConfig()).Config);
		if (Singleton<_E5B7>.Instantiated)
		{
			Singleton<_E5B7>.Release(Singleton<_E5B7>.Instance);
		}
		Singleton<_E5B7>.Create(_E3A2.Load<TextAsset>(_ED3E._E000(36829)).text.ParseJsonTo<_E2BF<_E55D>>(Array.Empty<JsonConverter>()).data.ClientSettings);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _E001()
	{
		return base.Start();
	}
}
