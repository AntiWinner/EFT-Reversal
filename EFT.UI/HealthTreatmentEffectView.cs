using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI;

public sealed class HealthTreatmentEffectView : HealthTreatmentView
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public SimpleTooltip tooltip;

		internal void _E000(_E98F effect, EffectIcon view)
		{
			view.Show(effect, tooltip, blinking: false);
		}
	}

	[SerializeField]
	private RectTransform _effectViewsContainer;

	[SerializeField]
	private EffectIcon _effectIconTemplate;

	private _EC89 _E1D6;

	private _ECEF<_E98F> _E0C6 = new _ECEF<_E98F>();

	public void Show(_EC89 effectObserver)
	{
		_E1D6 = effectObserver;
		_E0C6.Clear();
		SimpleTooltip tooltip = ItemUiContext.Instance.Tooltip;
		while (_effectViewsContainer.childCount > 0)
		{
			Object.DestroyImmediate(_effectViewsContainer.GetChild(0).gameObject);
		}
		Show((_EC83)effectObserver);
		UI.AddDisposable(new _EC71<_E98F, EffectIcon>(_E0C6, _effectIconTemplate, _effectViewsContainer, delegate(_E98F effect, EffectIcon view)
		{
			view.Show(effect, tooltip, blinking: false);
		}));
		UI.AddDisposable(effectObserver.Subscribe(_E000));
		_E000();
	}

	private void _E000()
	{
		_E0C6.Clear();
		base.CostTotal = _E1D6.TreatmentCost;
		List<_E991> effects = _E1D6.Effects;
		base.Active = effects.Any();
		if (!base.Active)
		{
			return;
		}
		foreach (_E991 item in effects)
		{
			_E98F[] displayableVariations = item.DisplayableVariations;
			_E0C6.AddRange(displayableVariations);
		}
		UpdateCost();
	}
}
