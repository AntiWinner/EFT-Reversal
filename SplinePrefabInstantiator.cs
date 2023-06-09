using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class SplinePrefabInstantiator : MonoBehaviour
{
	[Serializable]
	public class PrefabContainer
	{
		public GameObject Prefab;

		[Range(0f, 100f)]
		public float Probability;
	}

	public struct _E000
	{
		public Vector3 Position;

		public Vector3 Upward;

		public Vector3 Forward;

		public float Length;
	}

	public enum Flip
	{
		None,
		Flip180,
		Flip90
	}

	[CompilerGenerated]
	private Action m__E000;

	[CompilerGenerated]
	private Action<Transform, _E000, int> _E001;

	public SplinePrefabInstantiator Parent;

	public Vector3 ShiftRelativeToParent;

	public PrefabContainer[] Prefabs;

	public Vector3[] Positins = new Vector3[2]
	{
		Vector3.zero,
		Vector3.forward
	};

	public Vector3[] Normals = new Vector3[2]
	{
		Vector3.forward,
		Vector3.forward
	};

	public float DistanceBetweenPoints = 0.2f;

	public float Shift;

	public int Parts = 16;

	public bool PlaceBetweenPoints;

	public Vector3 EulerRotation;

	public bool HoldSplineRotation;

	public Vector3 RandomRotationEulers;

	public float Scale = 1f;

	public Vector3 RandomScale = Vector3.zero;

	public Flip FlipX;

	public Flip FlipY;

	public Flip FlipZ;

	public bool ProjectOnTerrain;

	public LayerMask Layer;

	public float CheckRange = 200f;

	public float TerrainOffset = 0.01f;

	protected Vector3 _transformPosition;

	protected Vector3 _parentFixPosition;

	public _E000[] QuantizedPoints;

	public event Action OnGenerate
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<Transform, _E000, int> OnUpdatePoint
	{
		[CompilerGenerated]
		add
		{
			Action<Transform, _E000, int> action = _E001;
			Action<Transform, _E000, int> action2;
			do
			{
				action2 = action;
				Action<Transform, _E000, int> value2 = (Action<Transform, _E000, int>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Transform, _E000, int> action = _E001;
			Action<Transform, _E000, int> action2;
			do
			{
				action2 = action;
				Action<Transform, _E000, int> value2 = (Action<Transform, _E000, int>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}
}
