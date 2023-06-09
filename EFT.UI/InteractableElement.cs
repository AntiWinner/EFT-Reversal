using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI;

public class InteractableElement : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public bool rawText;

		public string tooltip;

		internal string _E000()
		{
			if (!rawText)
			{
				return tooltip.Localized();
			}
			return tooltip;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public bool rawText;

		public string tooltip;

		internal string _E000()
		{
			if (!rawText)
			{
				return tooltip.Localized();
			}
			return tooltip;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public Func<string> tooltipGetter;

		internal string _E000()
		{
			return tooltipGetter().Localized();
		}
	}

	[SerializeField]
	private bool _unavailable;

	private CanvasGroup _E0D9;

	private HoverTooltipArea _E15C;

	private Func<string> _E15D;

	private Func<string> _E15E;

	private CanvasGroup _E000
	{
		get
		{
			if (!(_E0D9 != null))
			{
				return _E0D9 = base.gameObject.GetOrAddComponent<CanvasGroup>();
			}
			return _E0D9;
		}
	}

	private HoverTooltipArea _E001
	{
		get
		{
			if (!(_E15C != null))
			{
				return _E15C = base.gameObject.GetOrAddComponent<HoverTooltipArea>();
			}
			return _E15C;
		}
	}

	public virtual bool Interactable
	{
		get
		{
			return !_unavailable;
		}
		set
		{
			_unavailable = !value;
			this._E000.alpha = (value ? 1f : 0.3f);
			_E000();
		}
	}

	public void SetEnabledTooltip(string tooltip, bool rawText = false)
	{
		_E15D = ((!string.IsNullOrEmpty(tooltip)) ? ((Func<string>)(() => (!rawText) ? tooltip.Localized() : tooltip)) : null);
		_E000();
	}

	public void SetDisabledTooltip(string tooltip, bool rawText = false)
	{
		_E15E = ((!string.IsNullOrEmpty(tooltip)) ? ((Func<string>)(() => (!rawText) ? tooltip.Localized() : tooltip)) : null);
		_E000();
	}

	public void SetEnabledTooltip(Func<string> tooltipGetter, bool rawText = false)
	{
		_E15D = ((tooltipGetter != null && !rawText) ? ((Func<string>)(() => tooltipGetter().Localized())) : tooltipGetter);
		_E000();
	}

	public void SetDisabledTooltip(Func<string> tooltipGetter)
	{
		_E15E = tooltipGetter;
		_E000();
	}

	private void _E000()
	{
		Func<string> func = (Interactable ? _E15D : _E15E);
		this._E001.enabled = func != null;
		this._E001.SetMessageText(func);
	}
}
