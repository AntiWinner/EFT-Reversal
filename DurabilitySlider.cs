using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.UI;

public sealed class DurabilitySlider : Slider
{
	[SerializeField]
	private Image _background;

	[SerializeField]
	private Sprite _backgroundDangerZone;

	[SerializeField]
	private Sprite _backgroundDefault;

	[SerializeField]
	private Image _fill;

	[SerializeField]
	private Image _fillWear;

	[SerializeField]
	private Sprite _fillDangerZone;

	[SerializeField]
	private Sprite _fillDefault;

	[SerializeField]
	private GameObject _glow;

	[SerializeField]
	private GameObject _repairRequiredIcon;

	[CompilerGenerated]
	private float m__E000;

	private bool m__E001;

	public bool ShowDurability
	{
		set
		{
			this.m__E001 = value;
			base.gameObject.SetActive(this.m__E001);
			_repairRequiredIcon.SetActive(this.m__E001);
			if (this.m__E001)
			{
				_E002(this.value <= this._E002);
			}
		}
	}

	private float _E002
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		set
		{
			this.m__E000 = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		base.interactable = false;
		_background.sprite = _backgroundDefault;
		_fill.sprite = _fillDefault;
	}

	internal void _E000(RepairableComponent repairable, float durabilityThreshold)
	{
		base.maxValue = repairable.TemplateDurability;
		this._E002 = durabilityThreshold;
		_fillWear.fillAmount = ((float)repairable.TemplateDurability - repairable.MaxDurability) / 100f;
		_E001(repairable.Durability);
	}

	internal void _E001(float durability)
	{
		value = durability;
		if (this.m__E001)
		{
			_E002(durability <= this._E002);
		}
		else if (_repairRequiredIcon != null)
		{
			_repairRequiredIcon.gameObject.SetActive(value: false);
		}
	}

	private void _E002(bool inDangerZone)
	{
		if (_glow != null)
		{
			_glow.gameObject.SetActive(inDangerZone);
		}
		if (_repairRequiredIcon != null)
		{
			_repairRequiredIcon.gameObject.SetActive(inDangerZone);
		}
		_background.sprite = (inDangerZone ? _backgroundDangerZone : _backgroundDefault);
		_fill.sprite = (inDangerZone ? _fillDangerZone : _fillDefault);
	}
}
