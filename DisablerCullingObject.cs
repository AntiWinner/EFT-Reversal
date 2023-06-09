using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using EFT.Interactive;
using UnityEngine;

[ExecuteInEditMode]
public class DisablerCullingObject : DisablerCullingObjectBase
{
	[Serializable]
	public class ComponentState
	{
		public Component component;

		public bool isEnabled;
	}

	[SerializeField]
	protected List<Component> _componentsToTurnOff = new List<Component>();

	[SerializeField]
	protected List<Component> _compsToTurnOffWhoIgnoreInversedColliders = new List<Component>();

	[HideInInspector]
	[SerializeField]
	private List<ComponentState> _componentsToTurnOffDefaultState;

	[SerializeField]
	protected List<GameObject> _gameObjectsToTurnOff;

	[SerializeField]
	protected Bounds _componentsBounds;

	[SerializeField]
	public bool AllowLootCulling = true;

	[SerializeField]
	[Header("Performance:")]
	private int _componentsSwitchPerFrameOnEnable = 25;

	[SerializeField]
	private int _objectsSwitchPerFrameOnEnable = 25;

	[SerializeField]
	private int _componentsSwitchPerFrameOnDisable = 25;

	[SerializeField]
	private int _objectsSwitchPerFrameOnDisable = 25;

	[HideInInspector]
	public bool ExcludeLowPolyColliderLayer = true;

	[HideInInspector]
	public bool ExcludeDefaultLayerWithCollider;

	[HideInInspector]
	public bool ExcludeBallisticCollider;

	[HideInInspector]
	public bool IncludeInactive;

	[Header("View:")]
	public Color GizmosColor = new Color(0f, 1f, 0f, 0.16f);

	public Color GizmosInverseColor = new Color(1f, 0f, 0f, 0.1f);

	[SerializeField]
	private Color _unselectedGizmosColor = new Color(0.5f, 0.5f, 0.5f, 0.16f);

	private Stopwatch m__E000 = new Stopwatch();

	private IEnumerator m__E001;

	private IEnumerator m__E002;

	private static Dictionary<Renderer, Bounds> _E003;

	private bool _E004;

	private static Type[] _E005 = new Type[7]
	{
		typeof(LODGroup),
		typeof(FogLight),
		typeof(BaseLight),
		typeof(Light),
		typeof(LampController),
		typeof(VolumetricLight),
		typeof(MeshRenderer)
	};

	private static Dictionary<Renderer, Bounds> _E006;

	public int ComponentsSwitchPerFrameOnEnable
	{
		get
		{
			return _componentsSwitchPerFrameOnEnable;
		}
		set
		{
			_componentsSwitchPerFrameOnEnable = value;
		}
	}

	public int ComponentsSwitchPerFrameOnDisable
	{
		get
		{
			return _componentsSwitchPerFrameOnDisable;
		}
		set
		{
			_componentsSwitchPerFrameOnDisable = value;
		}
	}

	public List<Component> ComponentsToTurnOff => _componentsToTurnOff;

	public static void NullClean(List<Component> componentsToTurnOff, List<GameObject> gameObjectsToTurnOff)
	{
		if (componentsToTurnOff != null)
		{
			for (int num = componentsToTurnOff.Count - 1; num >= 0; num--)
			{
				Component component = componentsToTurnOff[num];
				if (component == null || component.hideFlags != 0)
				{
					componentsToTurnOff.RemoveAt(num);
				}
			}
		}
		if (gameObjectsToTurnOff == null)
		{
			return;
		}
		for (int num2 = gameObjectsToTurnOff.Count - 1; num2 >= 0; num2--)
		{
			GameObject gameObject = gameObjectsToTurnOff[num2];
			if (gameObject == null || gameObject.hideFlags != 0)
			{
				gameObjectsToTurnOff.RemoveAt(num2);
			}
		}
	}

	public virtual void NullClean()
	{
		NullClean(_componentsToTurnOff, _gameObjectsToTurnOff);
	}

	public override void PrepareCullingObject()
	{
		base.PrepareCullingObject();
		if (_E003 == null)
		{
			_E003 = new Dictionary<Renderer, Bounds>();
		}
		for (int i = 0; i < _componentsToTurnOff.Count; i++)
		{
			if (_componentsToTurnOff[i] is Renderer renderer && !_E003.ContainsKey(renderer))
			{
				_E003.Add(renderer, renderer.bounds);
			}
		}
		NullClean();
		SortComponentsToTurnOff();
		_E000();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	private void _E000()
	{
		bool flag = true;
		for (int i = 0; i < _componentsToTurnOff.Count; i++)
		{
			if (_componentsToTurnOff[i] is Renderer renderer)
			{
				Bounds value;
				Bounds bounds = ((_E003 == null || !_E003.TryGetValue(renderer, out value)) ? renderer.bounds : value);
				if (flag)
				{
					_componentsBounds = bounds;
					flag = false;
				}
				else
				{
					_componentsBounds.Encapsulate(bounds);
				}
			}
		}
		if (flag)
		{
			_componentsBounds = default(Bounds);
		}
	}

	protected override void SetComponentsEnabled(bool isVisible)
	{
		if (!_E004)
		{
			_E42B.SetComponentsEnabled(base.gameObject, this, ref this.m__E001, _componentsToTurnOff, _gameObjectsToTurnOff, _componentsSwitchPerFrameOnEnable, _objectsSwitchPerFrameOnEnable, _componentsSwitchPerFrameOnDisable, _objectsSwitchPerFrameOnDisable, isVisible, this.m__E000);
			_E42B.SetComponentsEnabled(base.gameObject, this, ref this.m__E002, _compsToTurnOffWhoIgnoreInversedColliders, null, _componentsSwitchPerFrameOnEnable, _objectsSwitchPerFrameOnEnable, _componentsSwitchPerFrameOnDisable, _objectsSwitchPerFrameOnDisable, _enteredColliders.Count > 0, this.m__E000);
		}
	}

	public void ForceEnable(bool value)
	{
		bool flag = _E004;
		_E004 = false;
		SetComponentsEnabled(value);
		_E004 = flag;
	}

	public void ForceUpdate()
	{
		UpdateComponentsStatusOnUpdate();
	}

	public void LockState(bool value)
	{
		_E004 = value;
		_updateComponentsStatus = true;
	}

	public override void ManualUpdate(float dt)
	{
		base.ManualUpdate(dt);
		if (_E003 != null)
		{
			_E003 = null;
		}
	}

	public void RegisterComponents<T>(IEnumerable<T> components, bool ignoreInverseColliders = false) where T : Component
	{
		bool flag = false;
		foreach (T component in components)
		{
			if (ignoreInverseColliders)
			{
				_compsToTurnOffWhoIgnoreInversedColliders.Add(component);
			}
			else
			{
				_componentsToTurnOff.Add(component);
			}
			flag = true;
		}
		if (flag)
		{
			_updateComponentsStatus = true;
		}
	}

	public void UnregisterComponents<T>(IEnumerable<T> components, bool ignoreInverseColliders = false) where T : Component
	{
		foreach (T component in components)
		{
			if (ignoreInverseColliders)
			{
				_compsToTurnOffWhoIgnoreInversedColliders.Remove(component);
			}
			else
			{
				_componentsToTurnOff.Remove(component);
			}
		}
	}

	public static void SortComponentsToTurnOff(List<Component> componentsToTurnOff, Dictionary<Renderer, Bounds> cache = null)
	{
		_E006 = cache;
		componentsToTurnOff.Sort(_E002);
		_E006 = null;
	}

	private static int _E001(Component component)
	{
		Type type = component.GetType();
		for (int i = 0; i < _E005.Length; i++)
		{
			Type type2 = _E005[i];
			if (type == type2 || type.IsSubclassOf(type2))
			{
				return i;
			}
		}
		return -1;
	}

	private static int _E002(Component x, Component y)
	{
		int num = _E001(x);
		int num2 = _E001(y);
		if (num < num2)
		{
			return -1;
		}
		if (num > num2)
		{
			return 1;
		}
		Renderer renderer = x as Renderer;
		Renderer renderer2 = y as Renderer;
		if (renderer == null || renderer2 == null)
		{
			return 0;
		}
		Bounds value;
		Vector3 vector = ((_E006 == null || !_E006.TryGetValue(renderer, out value)) ? renderer.bounds.size : value.size);
		Bounds value2;
		Vector3 vector2 = ((_E006 == null || !_E006.TryGetValue(renderer2, out value2)) ? renderer2.bounds.size : value2.size);
		float value3 = vector.x * vector.y * vector.z;
		return (vector2.x * vector2.y * vector2.z).CompareTo(value3);
	}

	public void SortComponentsToTurnOff()
	{
		SortComponentsToTurnOff(_componentsToTurnOff, _E003);
	}

	public Bounds GetBounds()
	{
		return _componentsBounds;
	}
}
