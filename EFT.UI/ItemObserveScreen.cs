using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.AssetsManager;
using EFT.InputSystem;
using EFT.InventoryLogic;
using EFT.UI.Screens;
using EFT.UI.WeaponModding;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public abstract class ItemObserveScreen<TScreenController, TScreen> : EftScreen<TScreenController, TScreen>, _EC5D, _EB65._E000, _EB66._E000 where TScreenController : _EC92._E000<TScreenController, TScreen> where TScreen : EftScreen<TScreenController, TScreen>
{
	[CompilerGenerated]
	private new sealed class _E000
	{
		public ItemObserveScreen<TScreenController, TScreen> _003C_003E4__this;

		public _EA40 compoundItem;

		internal void _E000()
		{
			_003C_003E4__this.RefreshingWeapon = true;
			_003C_003E4__this._loader.SetActive(_003C_003E4__this.RefreshingWeapon);
		}

		internal void _E001()
		{
			_003C_003E4__this.RefreshingWeapon = false;
			_003C_003E4__this._loader.SetActive(_003C_003E4__this.RefreshingWeapon);
		}

		internal void _E002(IResult error)
		{
			_003C_003E4__this._E003(compoundItem);
			_003C_003E4__this.ToggleChangedHandler();
			_003C_003E4__this.UpdatePositions();
			_003C_003E4__this.CheckForVitalParts();
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Vector2 center;

		internal Vector2 _E000(Vector2 point)
		{
			return point - center;
		}
	}

	private new const string m__E000 = "NO BUILD SELECTED";

	[SerializeField]
	private TextMeshProUGUI _weaponName;

	[SerializeField]
	private DropDownMenu _menu;

	[SerializeField]
	private DefaultUIButton _backButton;

	[SerializeField]
	private CameraViewporter _viewporter;

	[SerializeField]
	private RectTransform _slotsContainer;

	[SerializeField]
	private WeaponPreview _weaponPreview;

	[SerializeField]
	private CharacteristicsPanel _characteristics;

	[SerializeField]
	protected Toggle[] ModClassToggles;

	[SerializeField]
	private RectTransform _ellipseRect;

	[SerializeField]
	private GameObject _loader;

	[CompilerGenerated]
	private Action<Item, bool> m__E001;

	public Camera LineCamera;

	[CanBeNull]
	protected Item Item;

	private Item m__E002;

	protected readonly Dictionary<Transform, ModdingScreenSlotView> ModIcons = new Dictionary<Transform, ModdingScreenSlotView>();

	protected EModClass VisibleModClasses;

	protected bool RefreshingWeapon;

	protected ItemUiContext _itemUiContext;

	private Slot[] m__E003;

	private ModdingScreenSlotView m__E004;

	private readonly _E306 m__E005 = new _E306();

	private _EB6A m__E006;

	private bool m__E007;

	private bool m__E008;

	private Vector2 m__E009;

	private Light[] m__E00A;

	private Transform[] m__E00B;

	private HighLightMesh m__E00C;

	private _EB65 m__E00D;

	[CompilerGenerated]
	private _EAED m__E00E;

	protected _EAED InventoryController
	{
		[CompilerGenerated]
		get
		{
			return this._E00E;
		}
		[CompilerGenerated]
		private set
		{
			this._E00E = value;
		}
	}

	public event Action<Item, bool> OnItemSelected
	{
		[CompilerGenerated]
		add
		{
			Action<Item, bool> action = this._E001;
			Action<Item, bool> action2;
			do
			{
				action2 = action;
				Action<Item, bool> value2 = (Action<Item, bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this._E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Item, bool> action = this._E001;
			Action<Item, bool> action2;
			do
			{
				action2 = action;
				Action<Item, bool> value2 = (Action<Item, bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this._E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	protected virtual void Awake()
	{
		this._E004 = _E905.Pop<ModdingScreenSlotView>(_ED3E._E000(247808));
		_backButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
		DragTrigger orAddComponent = _viewporter.GetOrAddComponent<DragTrigger>();
		_itemUiContext = ItemUiContext.Instance;
		orAddComponent.onBeginDrag += delegate
		{
			this._E007 = true;
		};
		orAddComponent.onEndDrag += delegate
		{
			this._E007 = false;
		};
		Toggle[] modClassToggles = ModClassToggles;
		for (int i = 0; i < modClassToggles.Length; i++)
		{
			modClassToggles[i].onValueChanged.AddListener(delegate
			{
				ToggleChangedHandler();
				UpdatePositions();
			});
		}
	}

	protected abstract _EB6A CreateBuildManipulation();

	public override void Show(TScreenController controller)
	{
		throw new NotImplementedException();
	}

	protected void Show([CanBeNull] Item item, _EAED inventoryController)
	{
		InventoryController = inventoryController;
		this._E00A = (from x in _E3AA.FindUnityObjectsOfType<Light>().Except(GetComponentsInChildren<Light>(includeInactive: true))
			where x.enabled
			select x).ToArray();
		Light[] array = this._E00A;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		UI.AddDisposable(_characteristics);
		this._E00D = new _EB65(EItemViewType.WeaponModdingInteractable, this).CreateModdingChild(item);
		UI.AddDisposable(this._E00D);
		UI.AddDisposable(_menu);
		UpdateManipulation(CreateBuildManipulation());
		Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuModdingOpen);
		ShowGameObject();
	}

	protected void UpdateManipulation(_EB6A manipulation)
	{
		this._E006 = manipulation;
	}

	bool _EB65._E000.IsInventoryItem(Item item)
	{
		return this._E006.InPlayerCollection(item);
	}

	bool _EB66._E000.IsActive(_EB66 context, out string tooltip)
	{
		tooltip = string.Empty;
		if (context.TargetAddress == null)
		{
			return true;
		}
		_ECD1 error;
		bool result = this._E006.SelectCheck(context.Item, context.TargetAddress, out error);
		tooltip = ((error is InventoryError inventoryError) ? inventoryError.GetLocalizedDescription() : error?.ToString().Localized());
		return result;
	}

	bool _EB66._E000.IsSelected(Item item)
	{
		return false;
	}

	void _EB66._E000.ToggleSelection(_EB66 context)
	{
		if (this._E006.Select(context.Item, context.TargetAddress, out var error))
		{
			if (_menu.Open)
			{
				_menu.Close();
			}
			WeaponUpdate();
		}
		else
		{
			_E857.DisplaySingletonWarningNotification((error is InventoryError inventoryError) ? inventoryError.GetLocalizedDescription() : error.ToString().Localized());
		}
	}

	void _EB65._E000.Highlight(_EB66 context, bool selected)
	{
		if (context?.TargetAddress != null)
		{
			if (!selected)
			{
				_E000((Slot)context.TargetAddress.Container);
			}
			else
			{
				_E001((Slot)context.TargetAddress.Container, context.Item);
			}
		}
	}

	IEnumerable<Item> _EB65._E000.GetAllItems(ItemAddress itemAddress)
	{
		return this._E006.GetItemCollections(itemAddress);
	}

	protected virtual void UpdateItem([CanBeNull] Item newItem)
	{
		Item = newItem;
		this._E002 = newItem?.CloneItem();
		RefreshWeapon();
		_E005();
		_characteristics.Show(Item, this._E002);
		_weaponPreview.ResetRotator(-1f);
		CheckForVitalParts();
	}

	public Vector3 ScreenToWorldPoint(Vector3 position)
	{
		return LineCamera.ScreenToWorldPoint(position);
	}

	private void _E000(Slot slot)
	{
		_characteristics.ResetCompare(slot);
	}

	private void _E001(Slot slot, Item item)
	{
		_characteristics.OnCompareRequired(item, slot);
	}

	protected virtual void WeaponUpdate()
	{
		Item?.UpdateAttributes();
		_E005();
		_characteristics.Show(Item, this._E002);
		RefreshWeapon();
	}

	protected virtual void CheckForVitalParts()
	{
	}

	protected void RefreshWeapon()
	{
		_E004();
		_EA40 compoundItem = Item as _EA40;
		if (compoundItem != null)
		{
			_E002(compoundItem);
		}
		if (Item == null)
		{
			RefreshingWeapon = false;
			_loader.SetActive(RefreshingWeapon);
		}
		_weaponPreview.SetupItemPreview(Item, delegate
		{
			RefreshingWeapon = true;
			_loader.SetActive(RefreshingWeapon);
		}, delegate
		{
			RefreshingWeapon = false;
			_loader.SetActive(RefreshingWeapon);
		}, delegate
		{
			_E003(compoundItem);
			ToggleChangedHandler();
			UpdatePositions();
			CheckForVitalParts();
		}, setAsClosest: false, new Vector3(0f, -90f, 0f));
		if (Item != null)
		{
			this._E00C = _weaponPreview.WeaponPreviewCamera.GetComponent<HighLightMesh>();
			return;
		}
		HideModHighlight();
		this._E00C = null;
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.RightArrow))
		{
			_weaponPreview.Rotate(_E2E8.WeaponRotationSpeed);
			UpdatePositions();
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			_weaponPreview.Rotate(0f - _E2E8.WeaponRotationSpeed);
			UpdatePositions();
		}
		if (this._E008 || _viewporter.TargetCamera == null)
		{
			return;
		}
		if (!this._E007 && Physics.Raycast(_viewporter.TargetCamera.ScreenPointToRay(Input.mousePosition.ClampMousePositionToScreenRect()), out var hitInfo, _E37B.WeaponPreview))
		{
			Transform transform = hitInfo.collider.transform;
			if (ModIcons.TryGetValue(transform, out var value))
			{
				HighlightMod(transform, value.Color);
			}
		}
		else
		{
			HideModHighlight();
		}
	}

	private void _E002(_EA40 weapon)
	{
		this._E003 = weapon.AllSlots.Where((Slot x) => !x.ID.StartsWith(_ED3E._E000(225081)) && !x.ID.StartsWith(_ED3E._E000(155651)) && !x.ID.StartsWith(_ED3E._E000(247237))).ToArray();
	}

	private void _E003(_EA40 weapon)
	{
		_E333 containerCollectionView = _weaponPreview.Rotator.GetComponentInChildren<AssetPoolObject>().ContainerCollectionView;
		Slot[] array = this._E003;
		foreach (Slot slot in array)
		{
			Transform boneForSlot = containerCollectionView.GetBoneForSlot(slot);
			if (boneForSlot != null)
			{
				ModdingScreenSlotView moddingScreenSlotView = UnityEngine.Object.Instantiate(this._E004);
				RectTransform obj = (RectTransform)moddingScreenSlotView.transform;
				obj.SetParent(_slotsContainer, worldPositionStays: false);
				obj.localScale = Vector3.one;
				obj.localPosition = Vector3.zero;
				obj.sizeDelta = Vector3.zero;
				moddingScreenSlotView.Show(this._E00D, this, slot, weapon, _menu, boneForSlot, InventoryController, _itemUiContext);
				ModIcons[boneForSlot] = moddingScreenSlotView;
			}
			else
			{
				Debug.LogWarning(_ED3E._E000(247841) + slot.ID + _ED3E._E000(135679));
			}
		}
	}

	private void _E004()
	{
		foreach (ModdingScreenSlotView value in ModIcons.Values)
		{
			value.Close();
			UnityEngine.Object.DestroyImmediate(value.gameObject);
		}
		ModIcons.Clear();
		if (_menu.Open)
		{
			_menu.Close();
		}
	}

	protected virtual void ToggleChangedHandler()
	{
		VisibleModClasses = (ModClassToggles[0].isOn ? EModClass.Master : EModClass.None) | (ModClassToggles[1].isOn ? EModClass.Functional : EModClass.None) | (ModClassToggles[2].isOn ? EModClass.Gear : EModClass.None);
		foreach (ModdingScreenSlotView value in ModIcons.Values)
		{
			value.CheckVisibility(VisibleModClasses);
		}
	}

	public void HighlightMod(Transform modTransform, Color color, bool overriding = false)
	{
		if (overriding)
		{
			this._E008 = true;
		}
		this._E00C.Target = modTransform;
		this._E00C.enabled = true;
		this._E00C.Color = color;
	}

	public void HideModHighlight(bool overriding = false)
	{
		if (overriding)
		{
			this._E008 = false;
		}
		if (!(this._E00C == null))
		{
			this._E00C.Target = null;
			this._E00C.enabled = false;
		}
	}

	private void _E005()
	{
		if (Item == null)
		{
			_weaponName.text = _ED3E._E000(247903).Localized();
		}
		else
		{
			_weaponName.text = Item.Name.Localized();
		}
	}

	protected void UpdatePositions()
	{
		this._E009 = ((RectTransform)_viewporter.transform).rect.size * 0.5f;
		Rect rect = _ellipseRect.rect;
		float num = rect.width * 0.5f;
		float num2 = rect.height * 0.5f;
		if (ModIcons.Count <= 0)
		{
			return;
		}
		List<KeyValuePair<float, ModdingScreenSlotView>> list = new List<KeyValuePair<float, ModdingScreenSlotView>>(ModIcons.Count);
		foreach (var (transform2, moddingScreenSlotView2) in ModIcons)
		{
			if (moddingScreenSlotView2.gameObject.activeSelf)
			{
				Vector2 vector = _viewporter.WorldToLocalScreenPosition(transform2.position);
				moddingScreenSlotView2.SetBoneScreenPosition(vector);
				float angle = _E007((_E38D.True ? _E006(new Vector2[1] { vector }, this._E009, num, num2) : _E00B(new Vector2[1] { vector }, this._E009, num, num2))[0], this._E009, num, num2);
				float key = _E009(angle, num, num2);
				list.Add(new KeyValuePair<float, ModdingScreenSlotView>(key, moddingScreenSlotView2));
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		list.Sort((KeyValuePair<float, ModdingScreenSlotView> pair1, KeyValuePair<float, ModdingScreenSlotView> pair2) => pair1.Key.CompareTo(pair2.Key));
		float length = this._E005.GetLength(num2, num);
		_E3F6.SpaceOut(list, 110f, length);
		foreach (KeyValuePair<float, ModdingScreenSlotView> item in list)
		{
			item.Value.SetSlotScreenPosition(_E008(_E00A(item.Key, num, num2), this._E009, num, num2));
		}
	}

	private static Vector2[] _E006(Vector2[] points, Vector2 center, float a, float b)
	{
		if (a <= 0f || b <= 0f)
		{
			throw new ArgumentException(_ED3E._E000(247881));
		}
		if (a < b)
		{
			throw new ArgumentException(_ED3E._E000(247910));
		}
		int num = points.Length;
		Vector2[] array = new Vector2[num];
		float num2 = a * a;
		float num3 = b * b;
		float num4 = a * b;
		for (int i = 0; i < num; i++)
		{
			float f = Mathf.Atan2(points[i].y - center.y, points[i].x - center.x);
			float num5 = Mathf.Sin(f);
			float num6 = Mathf.Cos(f);
			float num7 = 1f / Mathf.Sqrt(num3 * num6 * num6 + num2 * num5 * num5) * num4;
			array[i].x = num7 * num6;
			array[i].y = num7 * num5;
		}
		for (int j = 0; j < array.Length; j++)
		{
			array[j] += center;
		}
		return array;
	}

	private static float _E007(Vector2 point, Vector2 center, float a, float b)
	{
		Vector2 vector = point - center;
		return Mathf.Atan2(vector.y * a, vector.x * b);
	}

	private static Vector2 _E008(float angle, Vector2 center, float a, float b)
	{
		return new Vector2(Mathf.Cos(angle) * a, Mathf.Sin(angle) * b) + center;
	}

	private float _E009(float angle, float a, float b)
	{
		return this._E005.LengthFromAngle(b, a, angle * 57.29578f);
	}

	private float _E00A(float pos, float a, float b)
	{
		return this._E005.AngleFromLength(b, a, pos) * ((float)Math.PI / 180f);
	}

	private static Vector2[] _E00B(Vector2[] points, Vector2 center, float a, float b)
	{
		if (a <= 0f || b <= 0f)
		{
			throw new ArgumentException(_ED3E._E000(247881));
		}
		if (a < b)
		{
			throw new ArgumentException(_ED3E._E000(247910));
		}
		int num = points.Length;
		Vector2[] array = new Vector2[num];
		float num2 = a * a;
		float num3 = b * b;
		float num4 = (a - b) * (a + b);
		float num5 = 1f / num4;
		Vector2[] array2 = points.Select((Vector2 point) => point - center).ToArray();
		for (int i = 0; i < num; i++)
		{
			float num6 = Mathf.Abs(array2[i].x);
			float num7 = Mathf.Abs(array2[i].y);
			float num8 = Mathf.Max(a * num6 - num4, b * num7);
			if (num8 <= 0f && num4 <= 0f)
			{
				array[i].x = 0f;
				array[i].y = b;
				continue;
			}
			if (num8 <= 0f)
			{
				array[i].x = num2 * array2[i].x * num5;
				float num9 = a * num6 * num5;
				float num10 = 1f - num9 * num9;
				array[i].y = ((num10 > 0f) ? (b * Mathf.Sqrt(num10)) : 0f);
				continue;
			}
			for (int j = 1; j < 100; j++)
			{
				float num11 = 1f / num8;
				float num12 = 1f / (num8 + num4);
				float num13 = a * num6 * num12;
				num13 *= num13;
				float num14 = b * num7 * num11;
				num14 *= num14;
				float num15 = num13 + num14 - 1f;
				if (num15 <= 0f)
				{
					break;
				}
				float num16 = -2f * (num13 * num12 + num14 * num11);
				float num17 = num15 / num16;
				if (Math.Abs(num8 - (num8 - num17)) < Mathf.Epsilon)
				{
					break;
				}
				num8 -= num17;
			}
			float num18 = num2 * num6 / (num8 + num4);
			float num19 = num18 / a;
			float num20 = 1f - num19 * num19;
			float num21 = ((num20 > 0f) ? (b * Mathf.Sqrt(num20)) : 0f);
			float num22 = num3 * num7 / num8;
			float num23 = num22 / b;
			float num24 = 1f - num23 * num23;
			float num25 = ((num24 > 0f) ? (a * Mathf.Sqrt(num24)) : 0f);
			float num26 = (num18 - num6) * (num18 - num6) + (num21 - num7) * (num21 - num7);
			float num27 = (num25 - num6) * (num25 - num6) + (num22 - num7) * (num22 - num7);
			if (num26 < num27)
			{
				array[i].x = num18;
				array[i].y = num21;
			}
			else
			{
				array[i].x = num25;
				array[i].y = num22;
			}
			if (array2[i].x < 0f)
			{
				array[i].x = 0f - array[i].x;
			}
			if (array2[i].y < 0f)
			{
				array[i].y = 0f - array[i].y;
			}
		}
		for (int k = 0; k < array.Length; k++)
		{
			array[k] += center;
		}
		return array;
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (!command.IsCommand(ECommand.Escape))
		{
			return InputNode.GetDefaultBlockResult(command);
		}
		return ETranslateResult.BlockAll;
	}

	protected override void TranslateAxes(ref float[] axes)
	{
		if (this._E007)
		{
			_weaponPreview.Rotate(axes[2] * 2f, (0f - axes[3]) * 2f);
			UpdatePositions();
		}
	}

	protected override ECursorResult ShouldLockCursor()
	{
		if (!this._E007)
		{
			return ECursorResult.ShowCursor;
		}
		return ECursorResult.LockCursor;
	}

	public override void Close()
	{
		Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuModdingClose);
		Item = null;
		if (this._E00C != null)
		{
			this._E00C.Target = null;
			this._E00C = null;
		}
		foreach (Light item in this._E00A.Where((Light x) => x != null))
		{
			item.enabled = true;
		}
		this._E007 = false;
		_E004();
		base.Close();
	}

	[CompilerGenerated]
	private void _E00C()
	{
		ScreenController.CloseScreen();
	}

	[CompilerGenerated]
	private void _E00D(PointerEventData pointerData)
	{
		this._E007 = true;
	}

	[CompilerGenerated]
	private void _E00E(PointerEventData pointerData)
	{
		this._E007 = false;
	}

	[CompilerGenerated]
	private void _E00F(bool x)
	{
		ToggleChangedHandler();
		UpdatePositions();
	}
}
