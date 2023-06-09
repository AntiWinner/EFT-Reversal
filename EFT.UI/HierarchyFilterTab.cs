using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class HierarchyFilterTab : FilterTab
{
	[SerializeField]
	private Sprite _subtabNormal;

	[SerializeField]
	private Sprite _subtabHover;

	[SerializeField]
	private Sprite _subtabSelected;

	[SerializeField]
	private Image _subgroupArrow;

	[SerializeField]
	private Color _normalArrowColor = new Color(1f, 1f, 1f, 0.5f);

	[SerializeField]
	private Color _selectedArrowColor = new Color(1f, 1f, 1f, 1f);

	private HierarchyFilterTab _E004;

	[CompilerGenerated]
	private IEnumerable<Tab> _E005;

	public IEnumerable<Tab> SubTabs
	{
		[CompilerGenerated]
		get
		{
			return _E005;
		}
		[CompilerGenerated]
		private set
		{
			_E005 = value;
		}
	}

	public void Init(IEnumerable<Tab> subTabs)
	{
		SubTabs = subTabs;
		_subgroupArrow.color = _normalArrowColor;
	}

	public void Init(HierarchyFilterTab parent)
	{
		_E004 = parent;
		if (!(parent == null))
		{
			_normalSprite = _subtabNormal;
			_hoverSprite = _subtabHover;
			GetComponent<LayoutElement>().minWidth = 37f;
			Image component = _normalVersion.GetComponent<Image>();
			component.sprite = _normalSprite;
			_normalVersion.GetComponent<RectTransform>().sizeDelta = new Vector2(component.preferredWidth, component.preferredHeight);
			Image component2 = _selectedVersion.GetComponent<Image>();
			component2.sprite = _subtabSelected;
			RectTransform component3 = _selectedVersion.GetComponent<RectTransform>();
			component3.sizeDelta = new Vector2(component2.preferredWidth, component2.preferredHeight);
			component3.anchoredPosition = new Vector2(1f, component3.anchoredPosition.y);
			Init(new Tab[0]);
		}
	}

	public override void Select(bool sendCallback = true)
	{
		HierarchyFilterTab hierarchyFilterTab = _E004;
		while (hierarchyFilterTab != null)
		{
			hierarchyFilterTab.UpdateVisual(selected: true, uiOnly: true);
			hierarchyFilterTab = hierarchyFilterTab._E004;
		}
		UpdateVisual(selected: true);
		if (sendCallback)
		{
			Controller?.Show();
		}
	}

	public override void Hover(bool isHovered)
	{
		base.Hover(isHovered);
		if (_E004 != null)
		{
			_E004.Hover(isHovered);
		}
		_subgroupArrow.gameObject.SetActive(isHovered && Interactable && SubTabs != null && SubTabs.Count() > 0);
	}

	protected override void UpdateVisual(bool selected, bool uiOnly = false)
	{
		base.UpdateVisual(selected, uiOnly);
		_subgroupArrow.color = (selected ? _selectedArrowColor : _normalArrowColor);
	}

	public override async Task<bool> Deselect()
	{
		bool flag = Controller != null;
		if (flag)
		{
			flag = !(await Controller.TryHide());
		}
		if (flag)
		{
			return false;
		}
		HierarchyFilterTab hierarchyFilterTab = _E004;
		while (hierarchyFilterTab != null)
		{
			hierarchyFilterTab.UpdateVisual(selected: false, uiOnly: true);
			hierarchyFilterTab = hierarchyFilterTab._E004;
		}
		UpdateVisual(selected: false);
		return true;
	}
}
