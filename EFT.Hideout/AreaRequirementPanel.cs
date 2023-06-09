using System;
using System.Runtime.CompilerServices;
using EFT.UI;
using UnityEngine;

namespace EFT.Hideout;

public sealed class AreaRequirementPanel : AreaPanel, _E83B, IUIView, IDisposable
{
	[SerializeField]
	private Color _fulfilledColor;

	[SerializeField]
	private Color _failedColor;

	[SerializeField]
	private RequirementFulfilledStatus _fulfilledStatus;

	private AreaRequirement _E033;

	public void Show(ItemUiContext itemUiContext, _EAED inventoryController, Requirement requirement, EAreaType areaType, bool ignoreFulfillment)
	{
		_E033 = (AreaRequirement)requirement;
		AreaData areaData = _E033.AreaData;
		if (areaData == null)
		{
			Debug.LogError(string.Format(_ED3E._E000(165698), _E033.AreaType));
			return;
		}
		if (areaData.Template.Type == _E033.AreaType && areaData.CurrentLevel >= _E033.RequiredLevel)
		{
			Close();
			return;
		}
		Init(areaData, delegate(AreaPanel arg)
		{
			AreasController areasController = arg.Data.Template.AreaBehaviour?.Controller;
			if (areasController != null)
			{
				areasController.Select(arg.Data, wait: true);
			}
		});
		if (base.AreaIconCreated == null)
		{
			base.AreaIconCreated = UnityEngine.Object.Instantiate(base.AreaIcon as AreaRequirementIcon, base.Container);
		}
		((AreaRequirementIcon)base.AreaIconCreated).Show(base.Data, _E033);
		UI.AddDisposable(delegate
		{
			base.AreaIconCreated.Close();
			UnityEngine.Object.Destroy(base.AreaIconCreated.gameObject);
			base.AreaIconCreated = null;
		});
	}

	protected override void SetInfo()
	{
		if (!(this == null))
		{
			bool fulfilled = _E033.Fulfilled;
			base.AreaName.text = base.Data.Template.Name;
			base.AreaName.color = (fulfilled ? _fulfilledColor : _failedColor);
			_fulfilledStatus.Show(fulfilled);
		}
	}

	public override void Close()
	{
		_fulfilledStatus.Close();
		base.Close();
	}

	[CompilerGenerated]
	private void _E000()
	{
		base.AreaIconCreated.Close();
		UnityEngine.Object.Destroy(base.AreaIconCreated.gameObject);
		base.AreaIconCreated = null;
	}
}
