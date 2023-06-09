using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class CollimatorSight : MonoBehaviour
{
	[CompilerGenerated]
	private static Action<CollimatorSight> _E000;

	[CompilerGenerated]
	private static Action<CollimatorSight> _E001;

	[CompilerGenerated]
	private static Action<CollimatorSight> _E002;

	private static readonly Quaternion _E003 = Quaternion.Euler(-90f, 0f, 0f);

	public MeshRenderer CollimatorMeshRenderer;

	public Material CollimatorMaterial;

	public static event Action<CollimatorSight> OnCollimatorEnabled
	{
		[CompilerGenerated]
		add
		{
			Action<CollimatorSight> action = _E000;
			Action<CollimatorSight> action2;
			do
			{
				action2 = action;
				Action<CollimatorSight> value2 = (Action<CollimatorSight>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<CollimatorSight> action = _E000;
			Action<CollimatorSight> action2;
			do
			{
				action2 = action;
				Action<CollimatorSight> value2 = (Action<CollimatorSight>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<CollimatorSight> OnCollimatorDisabled
	{
		[CompilerGenerated]
		add
		{
			Action<CollimatorSight> action = _E001;
			Action<CollimatorSight> action2;
			do
			{
				action2 = action;
				Action<CollimatorSight> value2 = (Action<CollimatorSight>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<CollimatorSight> action = _E001;
			Action<CollimatorSight> action2;
			do
			{
				action2 = action;
				Action<CollimatorSight> value2 = (Action<CollimatorSight>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<CollimatorSight> OnCollimatorUpdated
	{
		[CompilerGenerated]
		add
		{
			Action<CollimatorSight> action = _E002;
			Action<CollimatorSight> action2;
			do
			{
				action2 = action;
				Action<CollimatorSight> value2 = (Action<CollimatorSight>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<CollimatorSight> action = _E002;
			Action<CollimatorSight> action2;
			do
			{
				action2 = action;
				Action<CollimatorSight> value2 = (Action<CollimatorSight>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		CollimatorMeshRenderer = GetComponent<MeshRenderer>();
		CollimatorMaterial = CollimatorMeshRenderer.sharedMaterial;
	}

	private void OnEnable()
	{
		_E000?.Invoke(this);
	}

	private void OnDisable()
	{
		_E001?.Invoke(this);
	}

	public void LookAt(Vector3 point, Vector3 worldUp)
	{
		base.transform.LookAt(point, worldUp);
		base.transform.localRotation *= _E003;
		_E002?.Invoke(this);
	}
}
