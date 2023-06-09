using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.Hideout;

public sealed class VideocardInstaller : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E001
	{
		public TaskCompletionSource<bool> source;

		internal void _E000()
		{
			source.SetResult(result: true);
		}
	}

	[SerializeField]
	private Transform[] _points;

	[CompilerGenerated]
	private static bool _E000;

	private readonly List<GameObject> m__E001 = new List<GameObject>();

	private int _E002;

	private _E760 _E003;

	private static Item _E004;

	private static _ED0E<_ED08>._E002 _E005;

	public static bool IsInited
	{
		[CompilerGenerated]
		get
		{
			return VideocardInstaller._E000;
		}
		[CompilerGenerated]
		private set
		{
			VideocardInstaller._E000 = value;
		}
	}

	private _E760 _E006 => _E003 ?? (_E003 = Singleton<_E760>.Instance);

	public void GetFromOther(VideocardInstaller oldInstaller)
	{
		_E002 = oldInstaller.m__E001.Count;
		for (int i = 0; i < _E002; i++)
		{
			GameObject gameObject = oldInstaller.m__E001[i];
			this.m__E001.Add(gameObject);
			gameObject.transform.SetParent(_points[i], worldPositionStays: false);
			gameObject.transform.localRotation = Quaternion.identity;
		}
	}

	public async Task AttachCard(int count = 1)
	{
		if (_E004 == null || count <= 0 || _E002 >= _points.Length)
		{
			return;
		}
		_E002 += count;
		if (IsInited)
		{
			List<Task<GameObject>> list = new List<Task<GameObject>>();
			for (int i = 0; i < count; i++)
			{
				list.Add(_E006.CreateCleanLootPrefabAsync(_E004));
			}
			GameObject[] array = await Task.WhenAll(list);
			for (int j = 0; j < count; j++)
			{
				int count2 = this.m__E001.Count;
				Transform parent = _points[count2];
				GameObject gameObject = array[j];
				gameObject.SetActive(value: true);
				gameObject.transform.SetParent(parent, worldPositionStays: false);
				gameObject.transform.localRotation = Quaternion.identity;
				this.m__E001.Add(gameObject);
			}
		}
	}

	public void DetachCard()
	{
		if (_E002 <= 0)
		{
			return;
		}
		_E002--;
		if (IsInited)
		{
			int num = _E002;
			if (num >= this.m__E001.Count)
			{
				Debug.LogErrorFormat(_ED3E._E000(170537), num, this.m__E001.Count);
			}
			else
			{
				GameObject obj = this.m__E001[num];
				this.m__E001.RemoveAt(num);
				Object.Destroy(obj);
			}
		}
	}

	public static Task Init(string templateId)
	{
		_E004 = Singleton<_E63B>.Instance.GetPresetItem(templateId);
		TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();
		string[] keys = new string[1] { _E004.Prefab.path };
		_E005 = Singleton<_ED0A>.Instance.Retain(keys);
		_E612.WaitForAllBundles(_E005, delegate
		{
			source.SetResult(result: true);
		});
		IsInited = true;
		return source.Task;
	}

	public static void ReleaseBundles()
	{
		_E005?.Release();
		_E005 = null;
		IsInited = false;
	}
}
