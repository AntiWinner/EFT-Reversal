using System;
using System.Runtime.CompilerServices;
using EFT.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.Hideout;

public sealed class SkillRequirementPanel : UIElement, _E83B, IUIView, IDisposable
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public SkillRequirement skillRequirement;

		public SkillRequirementPanel _003C_003E4__this;

		public bool ignoreFulfillment;

		internal void _E000(bool hover, PointerEventData eventData)
		{
			if (hover)
			{
				string arg = skillRequirement.SkillName.Localized();
				int skillLevel = skillRequirement.SkillLevel;
				_003C_003E4__this._E02A.Show(string.Format(_ED3E._E000(163889).Localized(), arg, skillLevel));
			}
			else
			{
				_003C_003E4__this._E02A.Close();
			}
		}

		internal void _E001()
		{
			_003C_003E4__this._fulfilledStatus.Show(skillRequirement.Fulfilled);
			_003C_003E4__this._fulfilledStatus.gameObject.SetActive(!ignoreFulfillment);
		}
	}

	private const string _E048 = "Required skill <b>{0}</b>, level <b>{1}</b>";

	[SerializeField]
	private SkillRequirementIcon _skillIcon;

	[SerializeField]
	private RequirementFulfilledStatus _fulfilledStatus;

	private SimpleTooltip _E02A;

	public void Show(ItemUiContext itemUiContext, _EAED inventoryController, Requirement requirement, EAreaType areaType, bool ignoreFulfillment)
	{
		SkillRequirement skillRequirement = (SkillRequirement)requirement;
		ShowGameObject();
		_E02A = ItemUiContext.Instance.Tooltip;
		_skillIcon.Show(skillRequirement.FakeSkill, delegate(bool hover, PointerEventData eventData)
		{
			if (hover)
			{
				string arg = skillRequirement.SkillName.Localized();
				int skillLevel = skillRequirement.SkillLevel;
				_E02A.Show(string.Format(_ED3E._E000(163889).Localized(), arg, skillLevel));
			}
			else
			{
				_E02A.Close();
			}
		});
		UI.AddDisposable(requirement.OnFulfillmentChange.Bind(delegate
		{
			_fulfilledStatus.Show(skillRequirement.Fulfilled);
			_fulfilledStatus.gameObject.SetActive(!ignoreFulfillment);
		}));
	}

	public override void Close()
	{
		_fulfilledStatus.Close();
		_skillIcon.Close();
		if (_E02A != null)
		{
			_E02A.Close();
		}
		base.Close();
	}
}
