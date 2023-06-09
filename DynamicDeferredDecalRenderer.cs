using System;
using System.Runtime.CompilerServices;
using EFT.Ballistics;
using UnityEngine;

public class DynamicDeferredDecalRenderer : MonoBehaviour
{
	[CompilerGenerated]
	private Material _E000;

	[CompilerGenerated]
	private Transform _E001;

	[CompilerGenerated]
	private GameObject _E002;

	[CompilerGenerated]
	private int _E003;

	private Action<BallisticCollider> _E004;

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(44332));

	public Material DecalMaterial
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
		[CompilerGenerated]
		private set
		{
			_E000 = value;
		}
	}

	public Matrix4x4 ModelMatrix => base.transform.localToWorldMatrix;

	public Vector3 Position => base.transform.position;

	public Transform TransformHelper
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
		[CompilerGenerated]
		set
		{
			_E001 = value;
		}
	}

	public GameObject GameObjectHelper
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		set
		{
			_E002 = value;
		}
	}

	public int CullingGroupSphereIndex
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
		[CompilerGenerated]
		private set
		{
			_E003 = value;
		}
	}

	public void Init(Material mat, Mesh mesh, Vector3 projectionDirection, Vector4 uvStartEnd, bool tiled, int cullingGroupIndex)
	{
		DecalMaterial = (tiled ? new Material(mat) : mat);
		CullingGroupSphereIndex = cullingGroupIndex;
		DecalMaterial.SetVector(_E005, uvStartEnd);
	}

	public bool ManualUpdate()
	{
		if (TransformHelper == null || !TransformHelper.hasChanged)
		{
			return false;
		}
		base.transform.position = TransformHelper.position;
		base.transform.rotation = TransformHelper.rotation;
		TransformHelper.hasChanged = false;
		return true;
	}
}
