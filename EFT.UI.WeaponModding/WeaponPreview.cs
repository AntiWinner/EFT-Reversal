using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.AssetsManager;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace EFT.UI.WeaponModding;

public sealed class WeaponPreview : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _ED0E<_ED08>._E002 tokensToLoad;

		public WeaponPreview _003C_003E4__this;

		public Action onLoadingFinished;

		public Item item;

		public bool setAsClosest;

		public Callback onFinished;

		internal void _E000()
		{
			_E001(tokensToLoad);
		}

		internal void _E001(_ED0E<_ED08>._E002 tokens)
		{
			_003C_003E4__this._E004();
			_003C_003E4__this.m__E004 = _003C_003E4__this.m__E003;
			_003C_003E4__this.m__E003 = tokens;
			onLoadingFinished?.Invoke();
			_003C_003E4__this._E004();
			_003C_003E4__this._E007();
			_003C_003E4__this.m__E002 = Singleton<_E760>.Instance.CreateCleanLootPrefab(item);
			_003C_003E4__this.m__E002.SetActive(value: true);
			_003C_003E4__this._E005(_003C_003E4__this.m__E002, _003C_003E4__this._E00D);
			_003C_003E4__this._E009();
			_003C_003E4__this._E00E.SetParent(_003C_003E4__this.Rotator, worldPositionStays: false);
			_003C_003E4__this.m__E005 = GetBounds(_003C_003E4__this._E00E.gameObject);
			_003C_003E4__this.enabled = true;
			_E38B.SetLayersRecursively(_003C_003E4__this.m__E002, _E37B.WeaponPreview);
			if (setAsClosest)
			{
				_003C_003E4__this.WeaponPreviewCamera.PlaceInClosestPosition(_003C_003E4__this.m__E005.Value, _003C_003E4__this.m__E009.TemplateId, new _E3A0<float>
				{
					Min = -2.7f,
					Max = -0.45f
				});
			}
			onFinished?.Succeed();
		}
	}

	[SerializeField]
	private GameObject _weaponPreviewLights;

	[FormerlySerializedAs("WeaponPreviewCamera")]
	[SerializeField]
	private Camera _cameraTemplate;

	private Camera m__E000;

	public Transform Rotator;

	private Transform m__E001;

	private GameObject m__E002;

	private _ED0E<_ED08>._E002 m__E003;

	private _ED0E<_ED08>._E002 m__E004;

	private Bounds? m__E005;

	private readonly List<LightComponent> m__E006 = new List<LightComponent>(16);

	private readonly List<(Light light, int mask)> m__E007 = new List<(Light, int)>(16);

	private CancellationTokenSource m__E008;

	private Item m__E009;

	private float m__E00A;

	private float m__E00B;

	private Action m__E00C;

	private Vector3? _E00D;

	private Transform _E00E;

	private Transform _E00F;

	public Camera WeaponPreviewCamera => this.m__E000;

	private void Awake()
	{
		_cameraTemplate.gameObject.SetActive(value: false);
	}

	public void Init()
	{
		base.gameObject.SetActive(value: true);
		if (this.m__E001 == null)
		{
			_E001();
		}
		if (Rotator == null)
		{
			_E000();
		}
		if (WeaponPreviewCamera == null)
		{
			_E002();
		}
	}

	public void SetupItemPreview([CanBeNull] Item item, Action onLoadingStart = null, Action onLoadingFinished = null, Callback onFinished = null, bool setAsClosest = true, Vector3? initialRotation = null, bool enableWeaponLights = true)
	{
		_E000 CS_0024_003C_003E8__locals0 = new _E000();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		CS_0024_003C_003E8__locals0.onLoadingFinished = onLoadingFinished;
		CS_0024_003C_003E8__locals0.item = item;
		CS_0024_003C_003E8__locals0.setAsClosest = setAsClosest;
		CS_0024_003C_003E8__locals0.onFinished = onFinished;
		this.m__E008?.Cancel();
		if (CS_0024_003C_003E8__locals0.item == null)
		{
			_E004();
			_E008();
			return;
		}
		_E00D = initialRotation;
		Init();
		if (this.m__E003 != null)
		{
			this.m__E008?.Cancel();
			this.m__E008 = null;
		}
		if (this.m__E009 != CS_0024_003C_003E8__locals0.item)
		{
			ResetRotator(-1f);
		}
		this.m__E00C = Singleton<_E7DE>.Instance.Graphics.Settings.DisplaySettings.Bind(_E003);
		this.m__E009 = CS_0024_003C_003E8__locals0.item;
		this.m__E006.Clear();
		_E00A();
		Weapon weapon;
		if ((weapon = CS_0024_003C_003E8__locals0.item as Weapon) != null && enableWeaponLights)
		{
			foreach (LightComponent item2 in from x in weapon.Mods.GetComponents<LightComponent>()
				where !x.IsActive
				select x)
			{
				this.m__E006.Add(item2);
				item2.IsActive = true;
			}
		}
		onLoadingStart?.Invoke();
		CS_0024_003C_003E8__locals0.tokensToLoad = CS_0024_003C_003E8__locals0.item.GetAllBundleTokens();
		_E612.WaitForAllBundles(CS_0024_003C_003E8__locals0.tokensToLoad, delegate
		{
			CS_0024_003C_003E8__locals0._E001(CS_0024_003C_003E8__locals0.tokensToLoad);
		}, (this.m__E008 = new CancellationTokenSource()).Token);
	}

	private void _E000()
	{
		Rotator = new GameObject(_ED3E._E000(257824)).transform;
		Rotator.SetParent(this.m__E001);
		Rotator.ResetTransform();
	}

	private void _E001()
	{
		this.m__E001 = new GameObject(_ED3E._E000(257880)).transform;
		this.m__E001.SetParent(null, worldPositionStays: false);
		this.m__E001.ResetTransform();
		this.m__E001.position = new Vector3(0f, base.transform.localPosition.y, 0f);
	}

	private void _E002()
	{
		this.m__E000 = UnityEngine.Object.Instantiate(_cameraTemplate, this.m__E001);
		Transform obj = WeaponPreviewCamera.transform;
		obj.ResetTransform();
		obj.position = new Vector3(0f, 0f, _cameraTemplate.transform.position.z);
		WeaponPreviewCamera.gameObject.SetActive(value: true);
		WeaponPreviewCamera.enabled = true;
	}

	private void _E003(_E7E4 _)
	{
		this.WaitOneFrame(delegate
		{
			if (!(_weaponPreviewLights == null))
			{
				_weaponPreviewLights.transform.position = Vector3.zero;
				_weaponPreviewLights.transform.localScale = Vector3.one;
			}
		});
	}

	private void _E004()
	{
		if (this.m__E004 != null)
		{
			this.m__E004.Release();
			this.m__E004 = null;
		}
	}

	private void _E005(GameObject itemGameObject, Vector3? initialRotation = null)
	{
		itemGameObject.transform.ResetTransform();
		if (_E00F == null)
		{
			_E00F = new GameObject(_ED3E._E000(257871)).transform;
		}
		if (_E00E == null)
		{
			_E00E = new GameObject(_ED3E._E000(257917)).transform;
		}
		PreviewPivot component = itemGameObject.GetComponent<PreviewPivot>();
		if (component != null)
		{
			itemGameObject.transform.localPosition = -component.pivotPosition;
			_E00F.localRotation = ((!initialRotation.HasValue) ? component.Icon.rotation : Quaternion.Euler(initialRotation.Value));
		}
		else
		{
			_E00F.localPosition = GetBounds(itemGameObject).center;
		}
		ModPlacer component2 = itemGameObject.GetComponent<ModPlacer>();
		if (component2 != null)
		{
			component2.enabled = false;
		}
		itemGameObject.transform.SetParent(_E00F, worldPositionStays: false);
		Animator[] componentsInChildren = _E00F.GetComponentsInChildren<Animator>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
		_E00F.SetParent(_E00E.transform, worldPositionStays: false);
		_E00F.localScale = ((component != null) ? component.scale : Vector3.one);
	}

	public static Bounds GetBounds(GameObject itemGameObject)
	{
		IEnumerable<Bounds> enumerable = from x in itemGameObject.GetComponentsInChildren<Renderer>(includeInactive: false)
			where !x.name.Contains(_ED3E._E000(257903)) && x.name != _ED3E._E000(88327)
			select x into r
			select r.bounds into b
			where b.extents != Vector3.zero
			select b;
		Bounds[] source = (enumerable as Bounds[]) ?? enumerable.ToArray();
		if (!source.Any())
		{
			return default(Bounds);
		}
		return source.Aggregate(delegate(Bounds current, Bounds next)
		{
			current.Encapsulate(next);
			return current;
		});
	}

	public void ResetRotator(float defaultZ)
	{
		if (!(WeaponPreviewCamera == null))
		{
			Rotator.localRotation = Quaternion.identity;
			Transform obj = WeaponPreviewCamera.transform;
			Vector3 position = obj.position;
			position = new Vector3(position.x, position.y, 0f);
			obj.position = position;
			Vector3 localPosition = obj.localPosition;
			localPosition.z = defaultZ;
			obj.localPosition = localPosition;
			this.m__E00A = 0f;
			this.m__E00B = 0f;
		}
	}

	public void Rotate(float angle, float tilt = 0f, float minTilt = -30f, float maxTilt = 30f)
	{
		if (!(Rotator == null))
		{
			this.m__E00A -= angle * 0.3f;
			if (this.m__E00A >= 360f)
			{
				this.m__E00A -= 360f;
			}
			else if (this.m__E00A < -360f)
			{
				this.m__E00A += 360f;
			}
			int num = ((Mathf.Cos((float)Math.PI / 180f * this.m__E00A) >= 0f) ? 1 : (-1));
			this.m__E00B += tilt * 0.3f * (float)num;
			if (Math.Abs(minTilt) > Mathf.Epsilon && Math.Abs(maxTilt) > Mathf.Epsilon)
			{
				this.m__E00B = Mathf.Clamp(this.m__E00B, minTilt, maxTilt);
			}
			Rotator.localRotation = Quaternion.Euler(this.m__E00B, this.m__E00A, 0f);
		}
	}

	public void Zoom(float zoom)
	{
		if (!this.m__E005.HasValue)
		{
			return;
		}
		Transform transform = WeaponPreviewCamera.transform;
		transform.Translate(new Vector3(0f, 0f, zoom));
		if (zoom > 0f && !WeaponPreviewCamera.IsFullyVisibleByCamera(this.m__E005.Value))
		{
			do
			{
				transform.Translate(new Vector3(0f, 0f, -0.01f));
			}
			while (!WeaponPreviewCamera.IsFullyVisibleByCamera(this.m__E005.Value));
		}
		Vector3 localPosition = transform.localPosition;
		localPosition.z = Mathf.Clamp(localPosition.z, -2.7f, -0.45f);
		transform.localPosition = localPosition;
	}

	private void OnDisable()
	{
		Hide();
	}

	private void OnDestroy()
	{
		_E008();
	}

	public void Hide()
	{
		this.m__E00C?.Invoke();
		this.m__E00C = null;
		if (this.m__E009 is Weapon weapon)
		{
			foreach (LightComponent item in from x in weapon.Mods.GetComponents<LightComponent>()
				where this.m__E006.Contains(x)
				select x)
			{
				item.IsActive = false;
			}
		}
		_E00A();
		SetupItemPreview(null);
	}

	private void _E006<_E077>([CanBeNull] ref _E077 @object) where _E077 : Component
	{
		if (!((UnityEngine.Object)@object == (UnityEngine.Object)null))
		{
			UnityEngine.Object.DestroyImmediate(@object.gameObject);
			@object = null;
		}
	}

	private void _E007()
	{
		if (!(this.m__E002 == null))
		{
			this.m__E002.SetActive(value: false);
			AssetPoolObject.ReturnToPool(this.m__E002);
			this.m__E002 = null;
		}
	}

	private void _E008()
	{
		_E007();
		_E006(ref Rotator);
		_E006(ref this.m__E000);
		_E006(ref this.m__E001);
		_E006(ref _E00E);
	}

	private void _E009()
	{
		Light[] componentsInChildren = _E00E.GetComponentsInChildren<Light>(includeInactive: true);
		foreach (Light light in componentsInChildren)
		{
			this.m__E007.Add((light, light.cullingMask));
			light.cullingMask = 1 << _E37B.WeaponPreview;
		}
	}

	private void _E00A()
	{
		foreach (var item3 in this.m__E007)
		{
			Light item = item3.light;
			int item2 = item3.mask;
			item.cullingMask = item2;
		}
		this.m__E007.Clear();
	}

	[CompilerGenerated]
	private void _E00B()
	{
		if (!(_weaponPreviewLights == null))
		{
			_weaponPreviewLights.transform.position = Vector3.zero;
			_weaponPreviewLights.transform.localScale = Vector3.one;
		}
	}

	[CompilerGenerated]
	private bool _E00C(LightComponent x)
	{
		return this.m__E006.Contains(x);
	}
}
