using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT.Ballistics;
using UnityEngine;

namespace EFT.Interactive;

[_E2E2(22000)]
public sealed class WindowBreakerManager : MonoBehaviour
{
	public interface _E000
	{
		void Break(WindowBreakingConfig.Crack crack, in Vector3 position, in Vector3 force, float angle);
	}

	private struct _E001
	{
		public _E000 Breakable;

		public WindowBreakingConfig.Crack Crack;

		public Vector3 Position;

		public Vector3 Force;

		public float Angle;
	}

	private const int m__E000 = 5000;

	[CompilerGenerated]
	private static WindowBreakerManager m__E001;

	private static readonly Queue<_E001> m__E002 = new Queue<_E001>();

	[SerializeField]
	private BrokenWindowPieceTemplate _stuckPiecePrefab;

	private Queue<_EC12> _E003 = new Queue<_EC12>();

	public static WindowBreakerManager Instance
	{
		[CompilerGenerated]
		get
		{
			return WindowBreakerManager.m__E001;
		}
		[CompilerGenerated]
		private set
		{
			WindowBreakerManager.m__E001 = value;
		}
	}

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError(_ED3E._E000(206058), base.gameObject);
			Object.DestroyImmediate(Instance);
		}
		Instance = this;
		WindowBreaker.OnWindowHitAction += _E000;
		_E001();
	}

	private void Update()
	{
		if (WindowBreakerManager.m__E002.Count != 0)
		{
			_E001 obj = WindowBreakerManager.m__E002.Dequeue();
			obj.Breakable.Break(obj.Crack, in obj.Position, in obj.Force, obj.Angle);
		}
	}

	private void OnDestroy()
	{
		Instance = null;
		_E003.Clear();
		WindowBreaker.OnWindowHitAction -= _E000;
	}

	public void ReturnToPool(_EC12 piece)
	{
		if (piece.Transform != null)
		{
			piece.Transform.parent = Instance.transform;
			piece.Transform.localScale = Vector3.one;
		}
		else
		{
			Debug.LogError(_ED3E._E000(206082));
		}
		if (piece.MeshRenderer != null)
		{
			piece.MeshRenderer.enabled = false;
			piece.MeshRenderer.sharedMaterial = null;
		}
		else
		{
			Debug.LogError(_ED3E._E000(206122));
		}
		if (piece.MeshFilter != null)
		{
			piece.MeshFilter.sharedMesh = null;
		}
		else
		{
			Debug.LogError(_ED3E._E000(206157));
		}
		if (piece.MeshCollider != null)
		{
			piece.MeshCollider.sharedMesh = null;
		}
		else
		{
			Debug.LogError(_ED3E._E000(206198));
		}
		_E003.Enqueue(piece);
	}

	public _EC12 GetBrokenWindowPiece()
	{
		if (_E003 == null || _E003.Count == 0)
		{
			return _E002();
		}
		return _E003.Dequeue();
	}

	private static void _E000(WindowBreaker windowBreaker, _EC23 info, WindowBreakingConfig.Crack brokenWindowCrack, float angle)
	{
		WindowBreakerManager.m__E002.Enqueue(new _E001
		{
			Breakable = windowBreaker,
			Crack = brokenWindowCrack,
			Position = info.HitPoint,
			Force = info.Direction.normalized,
			Angle = angle
		});
	}

	private void _E001()
	{
		_E003 = new Queue<_EC12>();
		for (int i = 0; i < 5000; i++)
		{
			_E003.Enqueue(_E002());
		}
	}

	private _EC12 _E002()
	{
		return Object.Instantiate(_stuckPiecePrefab, base.transform).GetPiece();
	}

	public static _EC12 CreateBrokenWindowPiece()
	{
		GameObject gameObject = new GameObject();
		gameObject.SetActive(value: false);
		gameObject.transform.localScale = Vector3.one;
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshRenderer.enabled = false;
		MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
		meshCollider.convex = true;
		BallisticCollider ballisticCollider = gameObject.AddComponent<BallisticCollider>();
		GameObject gameObject2 = new GameObject();
		gameObject2.layer = LayerMask.NameToLayer(_ED3E._E000(25347));
		gameObject2.transform.SetParent(gameObject.transform);
		BoxCollider boxCollider = gameObject2.AddComponent<BoxCollider>();
		boxCollider.isTrigger = true;
		BrokenWindowPieceCollider brokenWindowPieceCollider = gameObject2.AddComponent<BrokenWindowPieceCollider>();
		boxCollider.enabled = false;
		brokenWindowPieceCollider.Collider = boxCollider;
		_EC12 result = default(_EC12);
		result.GameObject = gameObject;
		result.Transform = gameObject.transform;
		result.MeshFilter = meshFilter;
		result.MeshRenderer = meshRenderer;
		result.MeshCollider = meshCollider;
		result.BallisticCollider = ballisticCollider;
		result.ChildTransform = gameObject2.transform;
		result.ChildBoxCollider = boxCollider;
		result.ChildFragileCollider = brokenWindowPieceCollider;
		return result;
	}
}
