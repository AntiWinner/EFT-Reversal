using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PhysicsCustomOverlapBoxSimulationTest : MonoBehaviour
{
	public class BoxColliderGizmoDrawer : MonoBehaviour
	{
		public BoxCollider Tester;

		public BoxCollider Collider;

		public bool IsOverlapDetected;

		public bool IsTriggerDetected;

		public bool IsTriggerDetectedPrevFrame;

		public Func<bool> AllowSimulatePhysics;

		private void OnDrawGizmos()
		{
			if (!AllowSimulatePhysics())
			{
				IsTriggerDetected = IsOverlapDetected;
			}
			if (IsOverlapDetected && IsTriggerDetected)
			{
				Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
			}
			else if (IsOverlapDetected && !IsTriggerDetected)
			{
				Gizmos.color = new Color(1f, 0f, 0f, 0.1f);
			}
			else if (!IsOverlapDetected && IsTriggerDetected)
			{
				Gizmos.color = new Color(0f, 1f, 0f, 0.1f);
			}
			else
			{
				Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
			}
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawCube(Collider.center, Collider.size);
			if (AllowSimulatePhysics() && IsTriggerDetectedPrevFrame != IsOverlapDetected)
			{
				Debug.LogError(string.Format(_ED3E._E000(56229), IsTriggerDetectedPrevFrame, IsOverlapDetected));
			}
			IsTriggerDetectedPrevFrame = IsTriggerDetected;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!(other != Tester))
			{
				IsTriggerDetected = true;
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (!(other != Tester))
			{
				IsTriggerDetected = false;
			}
		}
	}

	[SerializeField]
	private BoxCollider _tester;

	[SerializeField]
	private LayerMask _layerMask;

	[SerializeField]
	private bool _manualMode;

	[SerializeField]
	private bool _simulateTriggers;

	private BoxCollider[] m__E000;

	private Dictionary<Collider, BoxColliderGizmoDrawer> m__E001;

	private BoxColliderGizmoDrawer[] m__E002;

	private _E31F m__E003;

	private Collider[] _E004;

	private Collider[][] _E005;

	private Transform _E006;

	private bool _E007;

	private void Awake()
	{
		Physics.autoSimulation = false;
		Physics.autoSyncTransforms = false;
		Physics2D.autoSimulation = false;
		Physics2D.autoSyncTransforms = false;
		this.m__E000 = (from col in UnityEngine.Object.FindObjectsOfType<BoxCollider>()
			where col != _tester
			select col).ToArray();
		this.m__E003 = new _E31F();
		_E006 = _tester.transform;
	}

	private void Start()
	{
		this.m__E001 = new Dictionary<Collider, BoxColliderGizmoDrawer>();
		this.m__E002 = new BoxColliderGizmoDrawer[this.m__E000.Length];
		_E004 = new Collider[this.m__E000.Length];
		for (int i = 0; i < this.m__E000.Length; i++)
		{
			BoxCollider boxCollider = this.m__E000[i];
			this.m__E003.RegisterCollider(boxCollider);
			BoxColliderGizmoDrawer boxColliderGizmoDrawer = boxCollider.gameObject.AddComponent<BoxColliderGizmoDrawer>();
			boxColliderGizmoDrawer.Collider = boxCollider;
			boxColliderGizmoDrawer.Tester = _tester;
			boxColliderGizmoDrawer.AllowSimulatePhysics = () => _simulateTriggers;
			this.m__E001[boxCollider] = boxColliderGizmoDrawer;
			this.m__E002[i] = boxColliderGizmoDrawer;
		}
		_E005 = new Collider[1][];
		_E005[0] = new Collider[this.m__E000.Length];
		Physics.SyncTransforms();
		StartCoroutine(_E000());
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			_E007 = true;
		}
		if (!_manualMode && _simulateTriggers)
		{
			Physics.Simulate(Time.deltaTime);
		}
	}

	private void OnDestroy()
	{
		this.m__E003.Dispose();
		this.m__E003 = null;
	}

	private IEnumerator _E000()
	{
		_E320._E003._E004 obj = new _E320._E003._E004
		{
			Results = _E004,
			Buffers = _E005
		};
		while (true)
		{
			if (_manualMode && !_E007)
			{
				yield return null;
				continue;
			}
			_E007 = false;
			obj.Reset();
			obj.TotalWorldsCount = 1;
			this.m__E003.BoxOverlap(0, _E006.position, _tester.size / 2f, _E006.rotation, _E005[0], _layerMask, QueryTriggerInteraction.Collide, obj);
			while (!obj.IsComplete)
			{
				yield return null;
			}
			int count = obj.Count;
			_E001(count, _E004);
		}
	}

	private void _E001(int count, Collider[] result)
	{
		for (int i = 0; i < this.m__E002.Length; i++)
		{
			this.m__E002[i].IsOverlapDetected = false;
		}
		for (int j = 0; j < count; j++)
		{
			this.m__E001[result[j]].IsOverlapDetected = true;
		}
	}

	[CompilerGenerated]
	private bool _E002(BoxCollider col)
	{
		return col != _tester;
	}

	[CompilerGenerated]
	private bool _E003()
	{
		return _simulateTriggers;
	}
}
