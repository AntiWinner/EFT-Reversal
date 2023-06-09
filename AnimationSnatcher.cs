using System;
using System.Collections.Generic;
using EFT;
using UnityEngine;

public class AnimationSnatcher : MonoBehaviour
{
	[Serializable]
	public class SnatcherPair
	{
		public enum ECopyMode
		{
			Local,
			World
		}

		public Transform Source;

		public Transform Destination;

		public ECopyMode CopyMode;

		public EPointOfView PointOfView = EPointOfView.ThirdPerson;

		public bool RespectPointOfView = true;

		public float PositionXZWeight = 1f;

		public float PositionYWeight = 1f;

		public float RotationWeight = 1f;

		public virtual bool IsDuplicating { get; set; }

		public SnatcherPair(Transform source, Transform destination)
		{
			Source = source;
			Destination = destination;
		}

		public virtual void SetWeight(float xz, float y, float r)
		{
		}

		public virtual void Process(EPointOfView pointOfView)
		{
			if (IsDuplicating && (!RespectPointOfView || PointOfView == pointOfView) && PositionXZWeight + PositionYWeight + RotationWeight >= float.Epsilon)
			{
				_E000(Source, Destination);
			}
		}

		private void _E000(Transform source, Transform destination)
		{
			if (CopyMode == ECopyMode.Local)
			{
				destination.localPosition = SeparateLerp(destination.localPosition, source.localPosition, PositionXZWeight, PositionYWeight);
				destination.localRotation = Quaternion.Lerp(destination.localRotation, source.localRotation, RotationWeight);
			}
			else if (CopyMode == ECopyMode.World)
			{
				destination.position = SeparateLerp(destination.position, source.position, PositionXZWeight, PositionYWeight);
				destination.rotation = Quaternion.Lerp(destination.rotation, source.rotation, RotationWeight);
			}
		}

		public Vector3 SeparateLerp(Vector3 v1, Vector3 v2, float weightXZ, float weightY)
		{
			float x = Mathf.Lerp(v1.x, v2.x, weightXZ);
			float y = Mathf.Lerp(v1.y, v2.y, weightY);
			float z = Mathf.Lerp(v1.z, v2.z, weightXZ);
			return new Vector3(x, y, z);
		}
	}

	[Serializable]
	public class HierarchySnatcherPair : SnatcherPair
	{
		public string Name;

		public bool ProcessOnlyChildren;

		public bool foldOut = true;

		public bool _isCopyHierarchy;

		public string SearchPrefix = "";

		private bool _isDuplicating;

		public List<SnatcherPair> ChildrenPairs;

		public bool IsCopyHierarchy
		{
			get
			{
				return _isCopyHierarchy;
			}
			set
			{
				if (value != _isCopyHierarchy)
				{
					_isCopyHierarchy = value;
					_E000(_isCopyHierarchy);
				}
			}
		}

		public override bool IsDuplicating
		{
			get
			{
				return _isDuplicating;
			}
			set
			{
				if (_isDuplicating == value)
				{
					return;
				}
				_isDuplicating = value;
				foreach (SnatcherPair childrenPair in ChildrenPairs)
				{
					childrenPair.IsDuplicating = _isDuplicating;
				}
			}
		}

		public override void SetWeight(float xz, float y, float r)
		{
			PositionXZWeight = xz;
			PositionYWeight = y;
			RotationWeight = r;
			for (int i = 0; i < ChildrenPairs.Count; i++)
			{
				SnatcherPair snatcherPair = ChildrenPairs[i];
				snatcherPair.PositionXZWeight = xz;
				snatcherPair.PositionYWeight = y;
				snatcherPair.RotationWeight = r;
			}
		}

		public override void Process(EPointOfView pointOfView)
		{
			if (!RespectPointOfView || pointOfView == PointOfView)
			{
				if (!ProcessOnlyChildren)
				{
					base.Process(pointOfView);
				}
				for (int i = 0; i < ChildrenPairs.Count; i++)
				{
					ChildrenPairs[i].Process(pointOfView);
				}
			}
		}

		public HierarchySnatcherPair(Transform source, Transform destination)
			: base(source, destination)
		{
			ChildrenPairs = new List<SnatcherPair>();
		}

		private void _E000(bool enabled)
		{
			if (enabled)
			{
				ChildrenPairs = new List<SnatcherPair>();
				{
					foreach (Transform item in _E001(Source))
					{
						Transform transform = _E38B.FindTransformRecursive(Destination, item.name + SearchPrefix);
						if (null == transform)
						{
							transform = _E38B.FindTransformRecursive(Destination, item.name + SearchPrefix + _ED3E._E000(30437));
						}
						if (null == transform)
						{
							Debug.LogWarning(_ED3E._E000(30432) + item.name + SearchPrefix);
							continue;
						}
						SnatcherPair snatcherPair = new SnatcherPair(item, transform);
						snatcherPair.IsDuplicating = IsDuplicating;
						snatcherPair.CopyMode = CopyMode;
						snatcherPair.PointOfView = PointOfView;
						ChildrenPairs.Add(snatcherPair);
					}
					return;
				}
			}
			ChildrenPairs.Clear();
		}

		private List<Transform> _E001(Transform source)
		{
			Queue<Transform> queue = new Queue<Transform>();
			List<Transform> list = new List<Transform>();
			foreach (Transform item3 in source)
			{
				queue.Enqueue(item3);
			}
			while (queue.Count > 0)
			{
				Transform transform = queue.Dequeue();
				list.Add(transform);
				foreach (Transform item4 in transform)
				{
					queue.Enqueue(item4);
				}
			}
			return list;
		}
	}

	public List<HierarchySnatcherPair> MainPairs = new List<HierarchySnatcherPair>(1);

	public List<HierarchySnatcherPair> LeftHandPost = new List<HierarchySnatcherPair>(1);

	public List<HierarchySnatcherPair> RightHandPost = new List<HierarchySnatcherPair>(1);

	public const string PALM_LEFT_TRANSFORM = "Base HumanLPalm";

	public const string PALM_RIGHT_TRANSFORM = "Base HumanRPalm";

	public const string WEAPON_ROOT_TRANSFORM = "Weapon_root";

	public const string RIBCAGE_TRANSFORM_NAME = "Base HumanRibcage";

	public const string LEFT_ARM = "Base HumanLCollarbone";

	public const string RIGHT_ARM = "Base HumanRCollarbone";

	public Transform First;

	public Transform Third;

	public List<string> Ignore = new List<string>();

	private EPointOfView m__E000;

	private void Start()
	{
		Ignore.Add(_ED3E._E000(30281));
	}

	public void Reset(Transform from, Transform to, EPointOfView pointOfView)
	{
		UpdateWeaponRoot(from, to);
		SetPointOfView(pointOfView);
	}

	public void InitHands(Transform HandsContainerTransform, Transform bodyTransform)
	{
		First = HandsContainerTransform;
		Third = bodyTransform;
		LeftHandPost.Clear();
		RightHandPost.Clear();
		Transform source = _E38B.FindTransformRecursive(First, _ED3E._E000(30335));
		Transform destination = _E38B.FindTransformRecursive(Third, _ED3E._E000(30335));
		RightHandPost.Add(new HierarchySnatcherPair(source, destination)
		{
			IsCopyHierarchy = true,
			ProcessOnlyChildren = true,
			IsDuplicating = true,
			Name = _ED3E._E000(30319)
		});
		RightHandPost.Add(new HierarchySnatcherPair(source, destination)
		{
			CopyMode = SnatcherPair.ECopyMode.World,
			PositionXZWeight = 0f,
			PositionYWeight = 0f,
			IsDuplicating = true,
			Name = _ED3E._E000(30365)
		});
		Transform source2 = _E38B.FindTransformRecursive(First, _ED3E._E000(30345));
		Transform destination2 = _E38B.FindTransformRecursive(Third, _ED3E._E000(30345));
		LeftHandPost.Add(new HierarchySnatcherPair(source2, destination2)
		{
			IsCopyHierarchy = true,
			ProcessOnlyChildren = true,
			IsDuplicating = true,
			Name = _ED3E._E000(30393)
		});
		Transform source3 = _E38B.FindTransformRecursive(First, _ED3E._E000(30382));
		Transform destination3 = _E38B.FindTransformRecursive(Third, _ED3E._E000(30382));
		HierarchySnatcherPair hierarchySnatcherPair = new HierarchySnatcherPair(source3, destination3);
		hierarchySnatcherPair.CopyMode = SnatcherPair.ECopyMode.World;
		hierarchySnatcherPair.PointOfView = EPointOfView.ThirdPerson;
		hierarchySnatcherPair.Name = _ED3E._E000(30426);
		hierarchySnatcherPair._isCopyHierarchy = true;
		hierarchySnatcherPair.ChildrenPairs = new List<SnatcherPair>();
		string[] obj = new string[3]
		{
			_ED3E._E000(30413),
			_ED3E._E000(30457),
			_ED3E._E000(30345)
		};
		int num = 0;
		string[] array = obj;
		foreach (string text in array)
		{
			Transform source4 = _E38B.FindTransformRecursive(First, text);
			Transform destination4 = _E38B.FindTransformRecursive(Third, text);
			HierarchySnatcherPair hierarchySnatcherPair2 = new HierarchySnatcherPair(source4, destination4);
			hierarchySnatcherPair2.CopyMode = ((num >= 2) ? SnatcherPair.ECopyMode.World : SnatcherPair.ECopyMode.Local);
			hierarchySnatcherPair2.PointOfView = EPointOfView.ThirdPerson;
			hierarchySnatcherPair.ChildrenPairs.Add(hierarchySnatcherPair2);
			num++;
		}
		hierarchySnatcherPair.IsDuplicating = true;
		hierarchySnatcherPair.ProcessOnlyChildren = true;
		hierarchySnatcherPair.SetWeight(0f, 0f, 1f);
		LeftHandPost.Add(hierarchySnatcherPair);
	}

	public void UpdateWeaponRoot(Transform HandsContainerTransform, Transform bodyTransform)
	{
		First = HandsContainerTransform;
		Third = bodyTransform;
		MainPairs.Clear();
		SetWeaponSnatchingPreferences(SnatcherPair.ECopyMode.World, respect: false);
		SetWeaponSnatchingWeights(1f, 1f, 0f);
	}

	public Transform[] TransformToArray(Transform parent)
	{
		List<Transform> list = new List<Transform>();
		foreach (Transform item in parent)
		{
			if (!Ignore.Contains(item.gameObject.name))
			{
				list.Add(item);
				list.AddRange(TransformToArray(item));
			}
		}
		return list.ToArray();
	}

	public void SetPointOfView(EPointOfView pointOfView)
	{
		this.m__E000 = pointOfView;
	}

	public void SetWeaponSnatchingPreferences(SnatcherPair.ECopyMode mode, bool respect)
	{
		if (MainPairs != null && MainPairs.Count > 0)
		{
			MainPairs[0].CopyMode = mode;
			MainPairs[0].RespectPointOfView = respect;
		}
	}

	public void SetWeaponSnatchingWeights(float xz, float y, float r)
	{
		if (MainPairs != null && MainPairs.Count > 0)
		{
			MainPairs[0].PositionXZWeight = xz;
			MainPairs[0].PositionYWeight = y;
			MainPairs[0].RotationWeight = r;
		}
	}

	public void SetPalmSnatchingWeight(float left, float right)
	{
		if (LeftHandPost.Count > 1)
		{
			LeftHandPost[0].SetWeight(left, left, left);
			LeftHandPost[1].SetWeight(0f, 0f, left);
		}
		if (RightHandPost.Count > 1)
		{
			RightHandPost[0].SetWeight(right, right, right);
			RightHandPost[1].SetWeight(0f, 0f, right);
		}
	}

	internal void _E000()
	{
		if (base.enabled)
		{
			for (int i = 0; i < MainPairs.Count; i++)
			{
				MainPairs[i].Process(this.m__E000);
			}
		}
	}

	internal void _E001()
	{
		for (int i = 0; i < LeftHandPost.Count; i++)
		{
			LeftHandPost[i].Process(this.m__E000);
		}
		for (int j = 0; j < RightHandPost.Count; j++)
		{
			RightHandPost[j].Process(this.m__E000);
		}
	}
}
