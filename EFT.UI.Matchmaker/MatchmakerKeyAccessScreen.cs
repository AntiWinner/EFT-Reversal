using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InputSystem;
using EFT.InventoryLogic;
using EFT.UI.Screens;
using EFT.UI.WeaponModding;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI.Matchmaker;

public sealed class MatchmakerKeyAccessScreen : MatchmakerEftScreen<_EC9D, MatchmakerKeyAccessScreen>
{
	[CompilerGenerated]
	private new sealed class _E000
	{
		public _EAE6 itemController;

		public string[] requiredKeys;

		internal bool _E000(Item equippedItem)
		{
			if (itemController.Examined(equippedItem))
			{
				return requiredKeys.Contains(equippedItem.TemplateId);
			}
			return false;
		}
	}

	[SerializeField]
	private DefaultUIButton _backButton;

	[SerializeField]
	private GameObject _previewPanel;

	[SerializeField]
	private WeaponPreview _preview;

	[SerializeField]
	private CameraImage _previewImage;

	[SerializeField]
	private float _autoRotationSpeed;

	[SerializeField]
	private CardSelectionPanel _cardSelection;

	[SerializeField]
	private GameObject _noKeycardPanel;

	private new bool m__E000;

	private bool m__E001;

	private Item m__E002;

	private Item m__E003;

	private Item[] m__E004;

	private void Start()
	{
		DragTrigger dragTrigger = _previewPanel.GetComponent<DragTrigger>();
		if (dragTrigger == null)
		{
			dragTrigger = _previewPanel.gameObject.AddComponent<DragTrigger>();
		}
		dragTrigger.onBeginDrag += delegate
		{
			this.m__E000 = true;
		};
		dragTrigger.onEndDrag += delegate
		{
			this.m__E000 = false;
		};
		this.m__E001 = Mathf.Abs(_autoRotationSpeed) > 0.01f;
		_backButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
	}

	private void Update()
	{
		if ((this.m__E003 != null || this.m__E002 != null) && !this.m__E000 && this.m__E001)
		{
			_preview.Rotate(_autoRotationSpeed);
		}
	}

	public override void Show(_EC9D controller)
	{
		base.Show(controller);
		Show(controller.ItemController, controller.RequiredKeys);
	}

	private void Show(_EAE6 itemController, string[] requiredKeys)
	{
		ShowGameObject();
		string text = requiredKeys.FirstOrDefault();
		if (text != null)
		{
			this.m__E003 = Singleton<_E63B>.Instance.CreateItem(new MongoID(newProcessId: false), text, null);
		}
		this.m__E004 = (from equippedItem in itemController.Inventory.GetAllEquipmentItems()
			where itemController.Examined(equippedItem) && requiredKeys.Contains(equippedItem.TemplateId)
			select equippedItem).ToArray();
		if (this.m__E004.Length != 0)
		{
			this.m__E002 = this.m__E004[0];
		}
		_E000();
		_preview.Init();
		_preview.ResetRotator(-1.5f);
		_previewImage.InitCamera(_preview.WeaponPreviewCamera);
		_cardSelection.Show(this.m__E002, this.m__E004, _E001);
		_noKeycardPanel.SetActive(this.m__E002 == null);
	}

	private void _E000()
	{
		Item item = this.m__E002 ?? this.m__E003;
		if (item != null)
		{
			_preview.SetupItemPreview(item);
		}
	}

	private void _E001(Item selectedItem)
	{
		if (this.m__E002 != selectedItem)
		{
			this.m__E002 = selectedItem;
			_E000();
			_noKeycardPanel.SetActive(this.m__E002 == null);
		}
	}

	protected override void TranslateAxes(ref float[] axes)
	{
		if (this.m__E000)
		{
			_preview.Rotate(axes[2] * 2f, (0f - axes[3]) * 2f);
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command == ECommand.Escape)
		{
			ScreenController.CloseScreen();
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	private void _E002()
	{
		this.m__E000 = false;
		_cardSelection.Hide();
	}

	public override void Close()
	{
		_E002();
		_preview.SetupItemPreview(null);
		_previewImage.InitCamera(null);
		base.Close();
	}

	[CompilerGenerated]
	private void _E003(PointerEventData pointerData)
	{
		this.m__E000 = true;
	}

	[CompilerGenerated]
	private void _E004(PointerEventData pointerData)
	{
		this.m__E000 = false;
	}

	[CompilerGenerated]
	private void _E005()
	{
		ScreenController.CloseScreen();
	}
}
