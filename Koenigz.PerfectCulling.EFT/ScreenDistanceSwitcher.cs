using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

[DisallowMultipleComponent]
[RequireComponent(typeof(PerfectCullingCrossSceneGroup))]
public sealed class ScreenDistanceSwitcher : MonoBehaviour
{
	[SerializeField]
	private bool _enableSwitch;

	[SerializeField]
	private float _size = 1f;

	[Range(0f, 1f)]
	[SerializeField]
	private float _relativeScreenSwitchHeight;

	[SerializeField]
	private int _numRenderersSwitch = 10;

	[SerializeField]
	private Transform _centerOverride;

	[SerializeField]
	private Transform _bakedLOD;

	[HideInInspector]
	[SerializeField]
	private Bounds _visibleBounds;

	private volatile bool m__E000;

	public volatile bool IsAutocullVisible;

	private _E4B1[] m__E001;

	private Renderer[] m__E002;

	internal static readonly List<ScreenDistanceSwitcher> _E003 = new List<ScreenDistanceSwitcher>();

	[CompilerGenerated]
	private static bool m__E004;

	internal static float _E005 = 1f;

	[HideInInspector]
	[SerializeField]
	private bool _firstSetupWasPerformed;

	private Vector3 _E006;

	public float Size => _size;

	public float RelativeScreenSwitchHeight => _relativeScreenSwitchHeight;

	public int NumRenderersSwitch => _numRenderersSwitch;

	public bool IsVisible => this.m__E000;

	internal static bool _E007
	{
		[CompilerGenerated]
		get
		{
			return ScreenDistanceSwitcher.m__E004;
		}
		[CompilerGenerated]
		set
		{
			ScreenDistanceSwitcher.m__E004 = value;
		}
	}

	public Vector3 CenterPoint => _E006;

	private void Awake()
	{
		_E002();
	}

	private void OnDestroy()
	{
		_E003();
	}

	private void Update()
	{
		if (_enableSwitch)
		{
			_E005();
		}
	}

	private void OnValidate()
	{
		_E001();
	}

	internal void _E000(bool flag)
	{
		if (flag)
		{
			_enableSwitch = true;
			_bakedLOD.transform.root.gameObject.SetActive(value: true);
		}
		else
		{
			_enableSwitch = false;
			Show(flag: false);
			_bakedLOD.transform.root.gameObject.SetActive(value: false);
		}
	}

	private void _E001()
	{
		if (!_firstSetupWasPerformed && _bakedLOD != null)
		{
			LODGroup[] componentsInChildren = _bakedLOD.GetComponentsInChildren<LODGroup>();
			if (componentsInChildren != null && componentsInChildren.Length != 0)
			{
				LODGroup lODGroup = componentsInChildren[0];
				_firstSetupWasPerformed = true;
				LOD[] lODs = lODGroup.GetLODs();
				LOD lOD = lODs[lODs.Length - 1];
				_relativeScreenSwitchHeight = lOD.screenRelativeTransitionHeight;
				_size = lODGroup.size;
			}
		}
		if (_firstSetupWasPerformed && _bakedLOD == null)
		{
			_firstSetupWasPerformed = false;
		}
	}

	internal void _E002()
	{
		ScreenDistanceSwitcher._E003.Add(this);
		this.m__E001 = GetComponents<_E4B1>();
		_E006 = ((_centerOverride == null) ? base.transform.position : _centerOverride.position);
		_visibleBounds = _E004();
		this.m__E002 = _bakedLOD.GetComponentsInChildren<Renderer>(includeInactive: true);
		LODGroup component = _bakedLOD.transform.root.GetComponent<LODGroup>();
		if (component != null)
		{
			component.enabled = false;
		}
		Show(flag: false);
	}

	internal void _E003()
	{
		ScreenDistanceSwitcher._E003.Remove(this);
		this.m__E001 = null;
	}

	private Bounds _E004()
	{
		Renderer[] componentsInChildren = _bakedLOD.GetComponentsInChildren<Renderer>();
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return new Bounds(base.transform.position, Vector3.one);
		}
		Bounds bounds = componentsInChildren[0].bounds;
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			bounds.Encapsulate(renderer.bounds);
		}
		return bounds;
	}

	private void Show(bool flag)
	{
		this.m__E000 = flag;
		Renderer[] array = this.m__E002;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = flag;
		}
		_E4B1[] array2 = this.m__E001;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i]?.OnBakedLODVisbilityChanged(this, flag);
		}
	}

	private void _E005()
	{
		if (_E007 && _enableSwitch && this.m__E000 != IsAutocullVisible)
		{
			Show(IsAutocullVisible);
		}
	}
}
