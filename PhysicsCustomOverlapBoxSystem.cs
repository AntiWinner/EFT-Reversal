using System;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public sealed class PhysicsCustomOverlapBoxSystem : MonoBehaviour
{
	[DefaultExecutionOrder(5000)]
	private sealed class PhysicsCustomOverlapBoxSystemScheduler : MonoBehaviour
	{
		private PhysicsCustomOverlapBoxSystem _E000;

		private void Awake()
		{
			_E000 = GetComponent<PhysicsCustomOverlapBoxSystem>();
		}

		private void LateUpdate()
		{
			_E000._E001();
		}
	}

	[DefaultExecutionOrder(-5000)]
	private sealed class PhysicsCustomOverlapBoxSystemHandleResult : MonoBehaviour
	{
		private PhysicsCustomOverlapBoxSystem _E000;

		private void Awake()
		{
			_E000 = GetComponent<PhysicsCustomOverlapBoxSystem>();
		}

		private void Update()
		{
			_E000._E000();
		}
	}

	private const int m__E000 = 16;

	private static PhysicsCustomOverlapBoxSystem m__E001;

	private _E31F._E000[] m__E002;

	private List<_E31F> _E003;

	private List<_E31F> _E004;

	[Header("Debug")]
	public int SearcherCount;

	public int TotalTriggersCount;

	public static PhysicsCustomOverlapBoxSystem Instance
	{
		get
		{
			if (PhysicsCustomOverlapBoxSystem.m__E001 == null)
			{
				GameObject obj = new GameObject(_ED3E._E000(56295));
				UnityEngine.Object.DontDestroyOnLoad(obj);
				PhysicsCustomOverlapBoxSystem.m__E001 = obj.AddComponent<PhysicsCustomOverlapBoxSystem>();
				obj.AddComponent<PhysicsCustomOverlapBoxSystemScheduler>();
				obj.AddComponent<PhysicsCustomOverlapBoxSystemHandleResult>();
			}
			return PhysicsCustomOverlapBoxSystem.m__E001;
		}
	}

	private void Awake()
	{
		_E003 = new List<_E31F>(16);
		_E004 = new List<_E31F>(16);
		this.m__E002 = Array.Empty<_E31F._E000>();
	}

	public void RegisterSearcher(_E31F searcher)
	{
		if (!_E003.Contains(searcher))
		{
			_E003.Add(searcher);
		}
	}

	public void UnregisterSearcher(_E31F searcher)
	{
		if (_E003.Contains(searcher))
		{
			_E004.Add(searcher);
			_E003.Remove(searcher);
		}
	}

	private void _E000()
	{
		for (int i = 0; i < this.m__E002.Length; i++)
		{
			if (this.m__E002[i].CommandsCount == 0)
			{
				continue;
			}
			try
			{
				this.m__E002[i].Searcher.WaitScheduleComplete(this.m__E002[i]);
			}
			catch (Exception innerException)
			{
				Debug.LogException(new Exception(_ED3E._E000(50183), innerException));
			}
			finally
			{
				this.m__E002[i].Dispose();
				this.m__E002[i] = default(_E31F._E000);
			}
		}
		_E002();
	}

	private void _E001()
	{
		if (this.m__E002.Length != _E003.Count)
		{
			Array.Resize(ref this.m__E002, _E003.Count);
		}
		for (int i = 0; i < _E003.Count; i++)
		{
			this.m__E002[i] = _E003[i].Schedule();
		}
		JobHandle.ScheduleBatchedJobs();
	}

	private void _E002()
	{
		if (_E004.Count == 0)
		{
			return;
		}
		foreach (_E31F item in _E004)
		{
			item.Clear();
		}
		_E004.Clear();
		if (_E003.Count == 0)
		{
			_E31F.PARAMS_INDEX_COUNTER = 0;
		}
	}

	private void OnDestroy()
	{
		if (_E003 != null)
		{
			_E31F[] array = _E003.ToArray();
			foreach (_E31F searcher in array)
			{
				UnregisterSearcher(searcher);
			}
			_E000();
			_E003.Clear();
			_E004.Clear();
			this.m__E002 = null;
		}
	}
}
