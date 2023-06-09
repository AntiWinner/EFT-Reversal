using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Diz.Utils;

public sealed class AsyncWorker : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Action function;

		public TaskCompletionSource<object> completionSource;

		public Action _003C_003E9__1;

		internal Action _E000()
		{
			try
			{
				function();
				return delegate
				{
					completionSource.SetResult(true);
				};
			}
			catch (Exception ex)
			{
				_E001 CS_0024_003C_003E8__locals0 = new _E001
				{
					CS_0024_003C_003E8__locals1 = this
				};
				Exception e = ex;
				CS_0024_003C_003E8__locals0.e = e;
				return delegate
				{
					CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.completionSource.SetException(CS_0024_003C_003E8__locals0.e);
				};
			}
		}

		internal void _E001()
		{
			completionSource.SetResult(true);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Exception e;

		public _E000 CS_0024_003C_003E8__locals1;

		internal void _E000()
		{
			CS_0024_003C_003E8__locals1.completionSource.SetException(e);
		}
	}

	[CompilerGenerated]
	private sealed class _E002<_E0A5>
	{
		public Func<_E0A5> function;

		public TaskCompletionSource<_E0A5> completionSource;

		internal Action _E000()
		{
			try
			{
				return new _E003<_E0A5>
				{
					CS_0024_003C_003E8__locals1 = this,
					result = function()
				}._E000;
			}
			catch (Exception ex)
			{
				_E004<_E0A5> CS_0024_003C_003E8__locals0 = new _E004<_E0A5>
				{
					CS_0024_003C_003E8__locals2 = this
				};
				Exception e = ex;
				CS_0024_003C_003E8__locals0.e = e;
				return delegate
				{
					CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals2.completionSource.SetException(CS_0024_003C_003E8__locals0.e);
				};
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E003<_E0A5>
	{
		public _E0A5 result;

		public _E002<_E0A5> CS_0024_003C_003E8__locals1;

		internal void _E000()
		{
			CS_0024_003C_003E8__locals1.completionSource.SetResult(result);
		}
	}

	[CompilerGenerated]
	private sealed class _E004<_E0A5>
	{
		public Exception e;

		public _E002<_E0A5> CS_0024_003C_003E8__locals2;

		internal void _E000()
		{
			CS_0024_003C_003E8__locals2.completionSource.SetException(e);
		}
	}

	[CompilerGenerated]
	private static readonly _ECCB m__E000 = new _ECCB();

	private static _ECCB _E001
	{
		[CompilerGenerated]
		get
		{
			return AsyncWorker.m__E000;
		}
	}

	private void Start()
	{
		AsyncWorker._E001.CreateThread();
	}

	private void Update()
	{
		AsyncWorker._E001.CheckForFinishedTasks();
	}

	private void FixedUpdate()
	{
		AsyncWorker._E001.CheckForFinishedTasks();
	}

	private void OnDestroy()
	{
		AsyncWorker._E001.Kill();
	}

	public static void RunInMainTread(Action action)
	{
		AsyncWorker._E001.RunInMainTread(action);
	}

	public static Task RunOnBackgroundThread(Action function)
	{
		TaskCompletionSource<object> completionSource = new TaskCompletionSource<object>();
		AsyncWorker._E001.AddTask(delegate
		{
			try
			{
				function();
				return delegate
				{
					completionSource.SetResult(true);
				};
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				Exception e = ex2;
				return delegate
				{
					completionSource.SetException(e);
				};
			}
		});
		return completionSource.Task;
	}

	public static Task<TResult> RunOnBackgroundThread<TResult>(Func<TResult> function)
	{
		TaskCompletionSource<TResult> completionSource = new TaskCompletionSource<TResult>();
		AsyncWorker._E001.AddTask(delegate
		{
			try
			{
				TResult result = function();
				return delegate
				{
					completionSource.SetResult(result);
				};
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				Exception e = ex2;
				return delegate
				{
					completionSource.SetException(e);
				};
			}
		});
		return completionSource.Task;
	}

	public static bool CheckIsMainThread()
	{
		return Thread.CurrentThread.ManagedThreadId == AsyncWorker._E001.MainThreadId;
	}
}
