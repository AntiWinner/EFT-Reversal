using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public class CompactCharacteristicDropdownPanel : CompactCharacteristicPanel
{
	[SerializeField]
	private GameObject _dropdownImage;

	private string _E123;

	protected override void Awake()
	{
		HoverTrigger obj = _dropdownImage.GetComponent<HoverTrigger>() ?? _dropdownImage.AddComponent<HoverTrigger>();
		obj.OnHoverStart += delegate
		{
			Tooltip.Show(_E123);
		};
		obj.OnHoverEnd += delegate
		{
			Tooltip.Close();
		};
	}

	public override void SetValues()
	{
		if (!(ValueText == null))
		{
			ValueText.gameObject.SetActive(value: false);
			_E123 = string.Empty;
			_E123 += (Examined ? ItemAttribute.StringValue() : _ED3E._E000(252232));
		}
	}

	[CompilerGenerated]
	private void _E000(PointerEventData arg)
	{
		Tooltip.Show(_E123);
	}

	[CompilerGenerated]
	private void _E001(PointerEventData arg)
	{
		Tooltip.Close();
	}
}
