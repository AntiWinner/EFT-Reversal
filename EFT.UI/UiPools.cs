using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EFT.UI.DragAndDrop;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI;

public sealed class UiPools : MonoBehaviourSingleton<UiPools>
{
	private const int m__E000 = 120;

	private readonly string[] _E001 = new string[17]
	{
		_ED3E._E000(251273),
		_ED3E._E000(251306),
		_ED3E._E000(251339),
		_ED3E._E000(251362),
		_ED3E._E000(251454),
		_ED3E._E000(251473),
		_ED3E._E000(251503),
		_ED3E._E000(251522),
		_ED3E._E000(251612),
		_ED3E._E000(251641),
		_ED3E._E000(251669),
		_ED3E._E000(251692),
		_ED3E._E000(251715),
		_ED3E._E000(251795),
		_ED3E._E000(251823),
		_ED3E._E000(251851),
		_ED3E._E000(249882)
	};

	private Dictionary<string, TaskCompletionSource<_EC37>> _E002;

	public async Task Init()
	{
		await _E000(_ECE3.Low);
	}

	private async Task _E000(_ECE1 yield)
	{
		_E002 = new Dictionary<string, TaskCompletionSource<_EC37>>();
		string[] array = this._E001;
		foreach (string text in array)
		{
			ResourceRequest resourceRequest = Resources.LoadAsync<ItemView>(text);
			await resourceRequest.Await();
			ItemView original = (ItemView)resourceRequest.asset;
			TaskCompletionSource<_EC37> taskCompletionSource = new TaskCompletionSource<_EC37>();
			_E002[text] = taskCompletionSource;
			Task task = _EC39<ItemView>.Create(Object.Instantiate(original, base.transform), 120, base.transform, taskCompletionSource, yield, CancellationToken.None);
			if (yield.Priority() != 0)
			{
				await task;
			}
			else
			{
				task.HandleExceptions(text);
			}
		}
	}

	[CanBeNull]
	public T GetGameObject<T>(string path)
	{
		if (_E002 == null)
		{
			Debug.LogError(_ED3E._E000(251293));
			return default(T);
		}
		return _E002[path].Task.Result.PopOrCreate().GetComponent<T>();
	}

	public override void OnDestroy()
	{
		if (_E002 == null)
		{
			return;
		}
		foreach (TaskCompletionSource<_EC37> value in _E002.Values)
		{
			value.Task.Result.Dispose();
		}
		base.OnDestroy();
	}
}
