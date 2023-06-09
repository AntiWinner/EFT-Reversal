using System;
using System.Collections.Generic;
using EFT.Interactive;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

public sealed class PerfectCullingDoorPreProcess : PerfectCullingCrossSceneGroupPreProcess
{
	[SerializeField]
	[HideInInspector]
	private GuidReference[] _doorContent = Array.Empty<GuidReference>();

	public IReadOnlyCollection<GuidReference> DoorContent => (IReadOnlyCollection<GuidReference>)(object)_doorContent;

	public override PerfectCullingBakeGroup[] CollectBakeGroups()
	{
		return Array.Empty<PerfectCullingBakeGroup>();
	}

	public override BakeBatch[] CreateBakeBatches(PerfectCullingBakeGroup[] inputGroups)
	{
		return Array.Empty<BakeBatch>();
	}

	public override BakeBatch[] GetBakeBatches()
	{
		BakeBatch bakeBatch = new BakeBatch();
		bakeBatch.Groups = _E005();
		return new BakeBatch[1] { bakeBatch };
	}

	public override PerfectCullingBakeGroup[] PrebakeTransform(BakeBatch batch, ICollection<GameObject> tempObjects, out PerfectCullingBakingBehaviour._E001.Mode writeMode)
	{
		writeMode = PerfectCullingBakingBehaviour._E001.Mode.Overwrite;
		List<PerfectCullingBakeGroup> list = new List<PerfectCullingBakeGroup>();
		int num = 0;
		PerfectCullingBakeGroup[] groups = batch.Groups;
		foreach (PerfectCullingBakeGroup perfectCullingBakeGroup in groups)
		{
			if (perfectCullingBakeGroup.groupType == PerfectCullingBakeGroup.GroupType.Door)
			{
				MeshRenderer meshRenderer = _E004(_E000(perfectCullingBakeGroup));
				tempObjects.Add(meshRenderer.gameObject);
				list.Add(new PerfectCullingBakeGroup
				{
					groupType = PerfectCullingBakeGroup.GroupType.User,
					renderers = new Renderer[1] { meshRenderer },
					serializedGroupId = num
				});
			}
			else if (perfectCullingBakeGroup.groupType == PerfectCullingBakeGroup.GroupType.User)
			{
				list.Add(new PerfectCullingBakeGroup
				{
					groupType = PerfectCullingBakeGroup.GroupType.User,
					renderers = perfectCullingBakeGroup.renderers,
					serializedGroupId = num
				});
			}
			num++;
		}
		return list.ToArray();
	}

	private static WorldInteractiveObject _E000(PerfectCullingBakeGroup cullingGroup)
	{
		Renderer[] renderers = cullingGroup.renderers;
		foreach (Renderer renderer in renderers)
		{
			if (!(renderer == null))
			{
				WorldInteractiveObject componentInParent = renderer.GetComponentInParent<WorldInteractiveObject>();
				if (componentInParent != null)
				{
					return componentInParent;
				}
			}
		}
		return null;
	}

	public override void OnEndContentCollect()
	{
		_E001();
	}

	public override PerfectCullingBakeGroup[] PrepareRuntimeContent()
	{
		return _E005();
	}

	private void _E001()
	{
	}

	private static Vector3 _E002(WorldInteractiveObject door)
	{
		GameObject obj = UnityEngine.Object.Instantiate(door.gameObject);
		obj.transform.position = Vector3.zero;
		obj.transform.rotation = Quaternion.identity;
		Vector3 size = _E003(obj.transform).size;
		UnityEngine.Object.DestroyImmediate(obj.gameObject);
		return size;
	}

	private static Bounds _E003(Transform root)
	{
		Bounds result = default(Bounds);
		MeshRenderer[] componentsInChildren = root.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in componentsInChildren)
		{
			result.Encapsulate(meshRenderer.bounds);
		}
		return result;
	}

	private static MeshRenderer _E004(WorldInteractiveObject doorObj)
	{
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<Collider>());
		gameObject.transform.position = doorObj.transform.position;
		Vector3 vector = _E002(doorObj) * 0.5f;
		switch (doorObj.DoorAxis)
		{
		case WorldInteractiveObject.EDoorAxis.X:
		case WorldInteractiveObject.EDoorAxis.XNegative:
		{
			gameObject.transform.up = doorObj.transform.right;
			float num3 = Mathf.Max(vector.y, vector.z) * 4f;
			gameObject.transform.localScale = new Vector3(num3, vector.x, num3);
			break;
		}
		case WorldInteractiveObject.EDoorAxis.Y:
		case WorldInteractiveObject.EDoorAxis.YNegative:
		{
			gameObject.transform.up = doorObj.transform.up;
			float num2 = Mathf.Max(vector.x, vector.z) * 4f;
			gameObject.transform.localScale = new Vector3(num2, vector.y, num2);
			break;
		}
		case WorldInteractiveObject.EDoorAxis.Z:
		case WorldInteractiveObject.EDoorAxis.ZNegative:
		{
			gameObject.transform.up = doorObj.transform.forward;
			float num = Mathf.Max(vector.x, vector.y) * 4f;
			gameObject.transform.localScale = new Vector3(num, vector.z, num);
			break;
		}
		}
		MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
		component.sharedMaterial = PerfectCullingResourcesLocator.Instance.ProxyLightMaterial;
		return component;
	}

	public override int GetBakeHash()
	{
		return 48879;
	}

	private PerfectCullingBakeGroup[] _E005()
	{
		List<PerfectCullingCrossSceneContentDoors> list = new List<PerfectCullingCrossSceneContentDoors>();
		GuidReference[] doorContent = _doorContent;
		foreach (GuidReference guidReference in doorContent)
		{
			list.Add(guidReference.gameObject.GetComponent<PerfectCullingCrossSceneContentDoors>());
		}
		list.Sort((PerfectCullingCrossSceneContentDoors x, PerfectCullingCrossSceneContentDoors y) => (x.ContentGroupId >= y.ContentGroupId) ? 1 : (-1));
		List<PerfectCullingBakeGroup> list2 = new List<PerfectCullingBakeGroup>();
		foreach (PerfectCullingCrossSceneContentDoors item in list)
		{
			list2.AddRange(item.BakeGroups);
		}
		return list2.ToArray();
	}

	public override SharedOccluder GenerateSharedOccluder()
	{
		return _E4AE.GenerateSharedOccluder(null, base.Group, (IReadOnlyCollection<PerfectCullingBakeGroup>)(object)_E005());
	}
}
