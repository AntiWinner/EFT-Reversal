using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Diz.Jobs;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Build.Pipeline;

namespace Diz.Resources;

public sealed class EasyAssets : MonoBehaviour, _ED0A
{
	private struct _E000
	{
		public string FileName;

		public uint Crc;

		public string[] Dependencies;
	}

	public List<string> Log = new List<string>();

	[CompilerGenerated]
	private _ED0E<_ED08> m__E000;

	private _ED0C[] _E001;

	public CompatibilityAssetBundleManifest Manifest;

	public _ED0E<_ED08> System
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E000 = value;
		}
	}

	public static async Task<EasyAssets> Create(GameObject gameObject, [CanBeNull] _ED09 bundleLock, string defaultKey, string rootPath, string platformName, [CanBeNull] Func<string, bool> shouldExclude, [CanBeNull] Func<string, Task> bundleCheck)
	{
		EasyAssets easyAssets = gameObject.AddComponent<EasyAssets>();
		await easyAssets._E000(bundleLock, defaultKey, rootPath, platformName, shouldExclude, bundleCheck);
		return easyAssets;
	}

	private async Task _E000([CanBeNull] _ED09 bundleLock, string defaultKey, string rootPath, string platformName, [CanBeNull] Func<string, bool> shouldExclude, [CanBeNull] Func<string, Task> bundleCheck)
	{
		string text = rootPath.Replace(_ED3E._E000(108551), "").Replace(_ED3E._E000(245363), "") + _ED3E._E000(30703) + platformName + _ED3E._E000(30703);
		Dictionary<string, BundleDetails> results = JsonConvert.DeserializeObject<Dictionary<string, _E000>>(File.ReadAllText(text + platformName + _ED3E._E000(245355))).ToDictionary((KeyValuePair<string, _E000> x) => x.Key, delegate(KeyValuePair<string, _E000> x)
		{
			BundleDetails result = default(BundleDetails);
			result.FileName = x.Value.FileName;
			result.Crc = x.Value.Crc;
			result.Dependencies = x.Value.Dependencies;
			return result;
		});
		Manifest = ScriptableObject.CreateInstance<CompatibilityAssetBundleManifest>();
		Manifest.SetResults(results);
		string[] allAssetBundles = Manifest.GetAllAssetBundles();
		this._E001 = new _ED0C[allAssetBundles.Length];
		if (bundleLock == null)
		{
			bundleLock = new _ED0B(int.MaxValue);
		}
		for (int i = 0; i < allAssetBundles.Length; i++)
		{
			this._E001[i] = new _ED0C(allAssetBundles[i], text, Manifest, bundleLock, bundleCheck);
			await JobScheduler.Yield();
		}
		EasyAssets easyAssets = this;
		_ED08[] loadables = this._E001;
		easyAssets.System = new _ED0E<_ED08>(loadables, defaultKey, shouldExclude);
	}

	private void Update()
	{
		System?.Update();
	}
}
