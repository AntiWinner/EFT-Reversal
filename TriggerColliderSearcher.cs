using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;

public class TriggerColliderSearcher : MonoBehaviour
{
	private class _E000
	{
		private string m__E000;

		private Collider m__E001;

		private LayerMask m__E002;

		private Func<Bounds> m__E003;

		public Transform OverrideTransformPosition;

		private _E320._E003.EWorldType _E004;

		private Collider[] _E005 = new Collider[512];

		private Collider[][] _E006;

		private List<Collider> _E007 = new List<Collider>(512);

		private List<Collider> _E008 = new List<Collider>(512);

		private Dictionary<Collider, List<IPhysicsTrigger>> _E009 = new Dictionary<Collider, List<IPhysicsTrigger>>(512);

		private _E384<IPhysicsTrigger> _E00A = new _E384<IPhysicsTrigger>(512, 2);

		private List<Component> _E00B = new List<Component>(4);

		[CompilerGenerated]
		private Action<IPhysicsTrigger> _E00C;

		[CompilerGenerated]
		private Action<IPhysicsTrigger> _E00D;

		private IEnumerator _E00E;

		private long _E00F;

		private Stopwatch _E010 = new Stopwatch();

		public event Action<IPhysicsTrigger> OnEnter
		{
			[CompilerGenerated]
			add
			{
				Action<IPhysicsTrigger> action = _E00C;
				Action<IPhysicsTrigger> action2;
				do
				{
					action2 = action;
					Action<IPhysicsTrigger> value2 = (Action<IPhysicsTrigger>)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref _E00C, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action<IPhysicsTrigger> action = _E00C;
				Action<IPhysicsTrigger> action2;
				do
				{
					action2 = action;
					Action<IPhysicsTrigger> value2 = (Action<IPhysicsTrigger>)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref _E00C, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public event Action<IPhysicsTrigger> OnExit
		{
			[CompilerGenerated]
			add
			{
				Action<IPhysicsTrigger> action = _E00D;
				Action<IPhysicsTrigger> action2;
				do
				{
					action2 = action;
					Action<IPhysicsTrigger> value2 = (Action<IPhysicsTrigger>)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref _E00D, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action<IPhysicsTrigger> action = _E00D;
				Action<IPhysicsTrigger> action2;
				do
				{
					action2 = action;
					Action<IPhysicsTrigger> value2 = (Action<IPhysicsTrigger>)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref _E00D, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public _E000(string name, Collider collider, float timeBreakerInterval = 9999f)
		{
			this.m__E000 = name;
			this.m__E001 = collider;
			_E00E = _E000();
			_E00F = (long)(timeBreakerInterval * 10000f);
			_E006 = new Collider[8][];
			for (int i = 0; i < 8; i++)
			{
				_E006[i] = new Collider[512];
			}
		}

		public void Connect(Func<Bounds> boundsGetter)
		{
			this.m__E003 = boundsGetter;
		}

		public void ManualUpdate(_E320._E003.EWorldType physicsWorldTypeMask, LayerMask castLayerMask)
		{
			_E004 = physicsWorldTypeMask;
			this.m__E002 = castLayerMask;
			_E00E.MoveNext();
			for (int i = 0; i < _E008.Count; i++)
			{
				Collider collider = _E008[i];
				_E003(collider);
			}
		}

		private IEnumerator _E000()
		{
			_E320._E003._E004 complete = new _E320._E003._E004
			{
				Results = _E005,
				Buffers = _E006
			};
			while (true)
			{
				Bounds bounds = this.m__E003();
				_E320._E003._E004 obj = _E320._E003.OverlapBoxAsync(center: (!(OverrideTransformPosition != null)) ? bounds.center : OverrideTransformPosition.position, worldTypeMask: _E004, halfExtents: bounds.extents, buffers: _E006, orientation: this.m__E001.transform.rotation, mask: this.m__E002, queryTriggerInteraction: QueryTriggerInteraction.Collide, complete: complete);
				yield return null;
				while (!obj.IsComplete)
				{
					yield return null;
				}
				int count = obj.Count;
				_E010.Reset();
				_E010.Start();
				int count2 = _E007.Count;
				long num = _E010.ElapsedTicks;
				for (int i = 0; i < count2; i++)
				{
					if (num >= _E00F)
					{
						yield return null;
						_E010.Restart();
						num = 0L;
					}
					Collider collider = _E007[i];
					bool flag = false;
					for (int j = 0; j < count; j++)
					{
						if ((object)_E005[j] == collider)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						_E002(collider, i);
						count2 = _E007.Count;
					}
				}
				for (int i = 0; i < count; i++)
				{
					if (num >= _E00F)
					{
						yield return null;
						_E010.Restart();
						num = 0L;
					}
					Collider collider2 = _E005[i];
					bool flag2 = false;
					for (int k = 0; k < count2; k++)
					{
						if ((object)collider2 == _E007[k])
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						_E001(collider2);
						count2 = _E007.Count;
					}
				}
				_E010.Stop();
			}
		}

		private void _E001(Collider collider)
		{
			_E007.Add(collider);
			bool flag = false;
			collider.GetComponents(typeof(IPhysicsTrigger), _E00B);
			List<IPhysicsTrigger> list = _E00A.Withdraw();
			for (int i = 0; i < _E00B.Count; i++)
			{
				IPhysicsTrigger physicsTrigger = _E00B[i] as IPhysicsTrigger;
				list.Add(physicsTrigger);
				if (physicsTrigger is _E31B)
				{
					flag = true;
				}
				try
				{
					physicsTrigger.OnTriggerEnter(this.m__E001);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
				try
				{
					_E00C?.Invoke(physicsTrigger);
				}
				catch (Exception exception2)
				{
					UnityEngine.Debug.LogException(exception2);
				}
			}
			_E009[collider] = list;
			if (flag)
			{
				_E008.Add(collider);
			}
		}

		private void _E002(Collider collider, int index)
		{
			_E007.RemoveAt(index);
			_E008.Remove(collider);
			List<IPhysicsTrigger> list = _E009[collider];
			for (int i = 0; i < list.Count; i++)
			{
				IPhysicsTrigger physicsTrigger = list[i];
				try
				{
					physicsTrigger.OnTriggerExit(this.m__E001);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
				try
				{
					_E00D?.Invoke(physicsTrigger);
				}
				catch (Exception exception2)
				{
					UnityEngine.Debug.LogException(exception2);
				}
			}
			_E00A.Return(list);
			_E009.Remove(collider);
		}

		private void _E003(Collider collider)
		{
			List<IPhysicsTrigger> list = _E009[collider];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] is _E31B obj)
				{
					try
					{
						obj.OnTriggerStay(this.m__E001);
					}
					catch (Exception exception)
					{
						UnityEngine.Debug.LogException(exception);
					}
				}
			}
		}
	}

	public const int MAX_COLLIDERS = 512;

	public const int MAX_WORLDS_COLLIDERS = 8;

	[SerializeField]
	private Collider _collider;

	[SerializeField]
	private LayerMask _castLayerMask;

	private _E33A m__E000;

	private Transform _E001;

	private _E000 _E002;

	[CanBeNull]
	private _E000 _E003;

	private List<_E000> _E004 = new List<_E000>();

	public bool IsEnabled;

	[CompilerGenerated]
	private bool _E005;

	public Transform OverrideTransformPosition
	{
		get
		{
			return _E001;
		}
		set
		{
			_E001 = value;
			foreach (_E000 item in _E004)
			{
				item.OverrideTransformPosition = _E001;
			}
		}
	}

	public bool IsInited
	{
		[CompilerGenerated]
		get
		{
			return _E005;
		}
		[CompilerGenerated]
		private set
		{
			_E005 = value;
		}
	}

	public event Action<IPhysicsTrigger> OnEnter
	{
		add
		{
			foreach (_E000 item in _E004)
			{
				item.OnEnter += value;
			}
		}
		remove
		{
			foreach (_E000 item in _E004)
			{
				item.OnEnter -= value;
			}
		}
	}

	public event Action<IPhysicsTrigger> OnExit
	{
		add
		{
			foreach (_E000 item in _E004)
			{
				item.OnExit += value;
			}
		}
		remove
		{
			foreach (_E000 item in _E004)
			{
				item.OnExit -= value;
			}
		}
	}

	public void Init(Collider collider, LayerMask castLayerMask)
	{
		_collider = collider;
		_castLayerMask = castLayerMask;
		_E004.Clear();
		_E002 = new _E000(_ED3E._E000(50392), _collider);
		_E004.Add(_E002);
		if (((int)_castLayerMask & _E37B.DisablerCullingObjectLayerMask) != 0)
		{
			_E003 = new _E000(_ED3E._E000(50380), _collider, EFTHardSettings.Instance.HoboCastTimeBreakInterval);
			_E004.Add(_E003);
		}
		IsInited = true;
	}

	public void ConnectToCharacterController(_E33A characterController)
	{
		this.m__E000 = characterController;
		foreach (_E000 item in _E004)
		{
			item.Connect(() => this.m__E000.bounds);
		}
	}

	public void ManualUpdate(float deltaTime, bool isCloseToCamera = true)
	{
		if (IsEnabled)
		{
			_E320._E003.EWorldType eWorldType = (_E320._E003.EWorldType)0;
			eWorldType |= _E320._E003.EWorldType.VolumePropagationAndEnvironmentSwitcherTriggers;
			if (isCloseToCamera)
			{
				eWorldType |= _E320._E003.EWorldType.Default;
			}
			_E002.ManualUpdate(eWorldType, _castLayerMask);
			_E003?.ManualUpdate(_E320._E003.EWorldType.DisablerCullingObjectTriggers, _castLayerMask);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (IsEnabled)
		{
			Gizmos.color = new Color(0f, 1f, 0f, Mathf.PingPong(Time.time * 0.2f, 0.1f));
			Gizmos.matrix = _collider.transform.localToWorldMatrix;
			Bounds bounds = this.m__E000.bounds;
			Gizmos.DrawCube(_collider.transform.InverseTransformPoint(bounds.center), bounds.size);
		}
	}

	[CompilerGenerated]
	private Bounds _E000()
	{
		return this.m__E000.bounds;
	}
}
