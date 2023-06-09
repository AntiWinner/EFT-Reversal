using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[_E2E2(20000)]
[ExecuteInEditMode]
public class CullingObject : MonoBehaviour, _E42A, _E05C
{
	[SerializeField]
	protected float CullDistance = 80f;

	[SerializeField]
	protected float _radius = 1f;

	[SerializeField]
	protected Vector3 _shift;

	[SerializeField]
	private bool _drawSphere = true;

	[SerializeField]
	private bool _cullByDistanceOnly = true;

	[SerializeField]
	protected List<Component> _componentsToTurnOff = new List<Component>();

	[HideInInspector]
	[SerializeField]
	private List<DisablerCullingObject.ComponentState> _componentsToTurnOffDefaultState;

	[SerializeField]
	private List<GameObject> _gameObjectsToTurnOff;

	[SerializeField]
	private Transform _transform;

	[CompilerGenerated]
	private int _E00F;

	private Vector3 _E010;

	[CompilerGenerated]
	private bool _E011;

	[CompilerGenerated]
	private float _E012;

	[CompilerGenerated]
	private bool _E013;

	[SerializeField]
	private int _componentsSwitchPerFrameOnEnable = 25;

	[SerializeField]
	private int _objectsSwitchPerFrameOnEnable = 25;

	[SerializeField]
	private int _componentsSwitchPerFrameOnDisable = 25;

	[SerializeField]
	private int _objectsSwitchPerFrameOnDisable = 25;

	private Stopwatch _E014 = new Stopwatch();

	private IEnumerator _E015;

	public int Index
	{
		[CompilerGenerated]
		get
		{
			return _E00F;
		}
		[CompilerGenerated]
		protected set
		{
			_E00F = value;
		}
	}

	public float CullDistanceSqr => CullDistance * CullDistance;

	public bool CullByDistanceOnly
	{
		get
		{
			return _cullByDistanceOnly;
		}
		set
		{
			_cullByDistanceOnly = value;
		}
	}

	public float Radius
	{
		get
		{
			return _radius;
		}
		protected set
		{
			_radius = value;
		}
	}

	public Vector3 Shift
	{
		get
		{
			return _shift;
		}
		protected set
		{
			_shift = value;
		}
	}

	public Vector3 Position => GetTransform().position + _shift;

	public Vector3 SafeMultithreadedPosition => _E010;

	public Vector3 ClearTransformPosition => GetTransform().position;

	public bool IsVisible
	{
		[CompilerGenerated]
		get
		{
			return _E011;
		}
		[CompilerGenerated]
		private set
		{
			_E011 = value;
		}
	}

	public float SqrCameraDistance
	{
		[CompilerGenerated]
		get
		{
			return _E012;
		}
		[CompilerGenerated]
		set
		{
			_E012 = value;
		}
	}

	public bool IsAutocullVisible
	{
		[CompilerGenerated]
		get
		{
			return _E013;
		}
		[CompilerGenerated]
		private set
		{
			_E013 = value;
		}
	}

	protected virtual void Awake()
	{
	}

	protected virtual void OnValidate()
	{
		OnPreProcess();
	}

	public void OnPreProcess()
	{
		if (_transform == null)
		{
			_transform = base.transform;
		}
		DisablerCullingObject.NullClean(_componentsToTurnOff, _gameObjectsToTurnOff);
		DisablerCullingObject.SortComponentsToTurnOff(_componentsToTurnOff);
	}

	public void Start()
	{
		IsAutocullVisible = true;
		if (CullingManager._instance == null)
		{
			CullingManager.AddEarlyObject(this);
		}
		else
		{
			Register();
		}
	}

	public virtual void CustomUpdate()
	{
		Transform transform = GetTransform();
		if (transform.hasChanged)
		{
			_E010 = ClearTransformPosition;
			CullingManager.Instance.UpdateSphere(this);
			transform.hasChanged = false;
		}
	}

	public void SetAutocullVisibility(bool flag)
	{
		IsAutocullVisible = flag;
		if (flag)
		{
			_E000(IsVisible);
		}
		else
		{
			_E000(isVisible: false);
		}
	}

	public virtual void SetVisibility(bool isVisible)
	{
		if (IsVisible != isVisible)
		{
			IsVisible = isVisible;
			_E000(isVisible);
		}
	}

	public void Register()
	{
		Index = CullingManager.Instance.Register(this);
	}

	private void _E000(bool isVisible)
	{
		_E42B.SetComponentsEnabled(base.gameObject, this, ref _E015, _componentsToTurnOff, _gameObjectsToTurnOff, _componentsSwitchPerFrameOnEnable, _objectsSwitchPerFrameOnEnable, _componentsSwitchPerFrameOnDisable, _objectsSwitchPerFrameOnDisable, isVisible, _E014);
	}

	protected virtual void OnDestroy()
	{
		SetVisibility(isVisible: true);
		if (CullingManager.Instance != null)
		{
			CullingManager.Instance.Unregister(this);
		}
		else
		{
			CullingManager.RemoveEarlyObject(this);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (_drawSphere)
		{
			Gizmos.color = (IsVisible ? Color.yellow : Color.red);
			Gizmos.DrawWireSphere(Position, _radius);
		}
	}

	protected virtual Transform GetTransform()
	{
		return _transform;
	}

	public void SortComponentsToTurnOff()
	{
	}
}
