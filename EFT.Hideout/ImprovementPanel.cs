using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EFT.UI;
using UnityEngine;

namespace EFT.Hideout;

public sealed class ImprovementPanel : AbstractPanel<bool>
{
	[SerializeField]
	private ImprovementView _improvementViewTemplate;

	[SerializeField]
	private RectTransform _container;

	private Dictionary<_E828, _E825> m__E000;

	private _EC6D<_E829, ProduceView> m__E001;

	public override async Task ShowContents()
	{
		_EC6D<_E81B, ImprovementView> obj = new _EC6D<_E81B, ImprovementView>();
		UI.AddDisposable(obj);
		await obj.InitAsync(base.AreaData.CurrentStage.Improvements.Data, (_E81B arg) => _improvementViewTemplate, (_E81B arg) => _container, delegate(_E81B improvement, ImprovementView view)
		{
			view.Show(ItemUiContext.Instance, base.Player._E0DE, improvement, base.AreaData);
		});
	}

	public override void SetInfo()
	{
	}

	[CompilerGenerated]
	private ImprovementView _E000(_E81B arg)
	{
		return _improvementViewTemplate;
	}

	[CompilerGenerated]
	private Transform _E001(_E81B arg)
	{
		return _container;
	}

	[CompilerGenerated]
	private void _E002(_E81B improvement, ImprovementView view)
	{
		view.Show(ItemUiContext.Instance, base.Player._E0DE, improvement, base.AreaData);
	}
}
