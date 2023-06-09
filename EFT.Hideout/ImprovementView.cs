using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.UI;
using TMPro;
using UnityEngine;

namespace EFT.Hideout;

public class ImprovementView : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ImprovementView _003C_003E4__this;

		public AreaData area;

		internal void _E000()
		{
			_003C_003E4__this.UpdateView();
		}

		internal void _E001(Requirement requirement, ItemRequirementPanel itemView)
		{
			itemView.Show(_003C_003E4__this._E001, _003C_003E4__this._E000, requirement, area.Template.Type, ignoreFulfillment: false);
		}
	}

	[SerializeField]
	[Space]
	private DefaultUIButton _startButton;

	[SerializeField]
	[Space]
	private GameObject _percentagePanel;

	[SerializeField]
	private TextMeshProUGUI _percentageText;

	[SerializeField]
	private GameObject _expectedTimePanel;

	[SerializeField]
	private TextMeshProUGUI _expectedTime;

	[SerializeField]
	[Space]
	private TextMeshProUGUI _status;

	[SerializeField]
	[Space]
	private ItemRequirementPanel _requiredItemTemplate;

	[SerializeField]
	private Transform _requiredItemsContainer;

	[CompilerGenerated]
	private _EAED _E00E;

	[CompilerGenerated]
	private ItemUiContext _E00F;

	[CompilerGenerated]
	private AreaData _E010;

	protected const string REQUIREMENTS_NOT_FULFILLED = "hideout/Requirements are not fulfilled";

	private const int _E011 = 1;

	private DateTime _E012;

	private static readonly string _E013 = _ED3E._E000(171591);

	private static readonly string _E014 = _ED3E._E000(171618);

	private _EC79<Requirement, ItemRequirementPanel> _E015;

	private bool _E016;

	private bool _E017;

	private _E81B _E018;

	private _EAED _E000
	{
		[CompilerGenerated]
		get
		{
			return _E00E;
		}
		[CompilerGenerated]
		set
		{
			_E00E = value;
		}
	}

	private ItemUiContext _E001
	{
		[CompilerGenerated]
		get
		{
			return _E00F;
		}
		[CompilerGenerated]
		set
		{
			_E00F = value;
		}
	}

	private AreaData _E002
	{
		[CompilerGenerated]
		get
		{
			return _E010;
		}
		[CompilerGenerated]
		set
		{
			_E010 = value;
		}
	}

	public void Show(ItemUiContext itemUiContext, _EAED inventoryController, _E81B improvement, AreaData area)
	{
		this._E001 = itemUiContext;
		this._E000 = inventoryController;
		this._E002 = area;
		_E018 = improvement;
		_startButton.OnClick.AddListener(_E005);
		UI.AddDisposable(area.ImprovementsUpdated.Subscribe(delegate
		{
			UpdateView();
		}));
		_E015 = UI.AddViewList(improvement.Requirements, _requiredItemTemplate, _requiredItemsContainer, delegate(Requirement requirement, ItemRequirementPanel itemView)
		{
			itemView.Show(this._E001, this._E000, requirement, area.Template.Type, ignoreFulfillment: false);
		});
		_E000();
		ShowGameObject();
	}

	private void _E000()
	{
		_E001();
		_E002();
		if (_E018.Waiting)
		{
			_E006();
		}
		if (_E018.Processing)
		{
			_E007();
		}
		if (_E018.Completed)
		{
			_E008();
		}
	}

	private void _E001()
	{
		bool flag = _E003();
		string tooltip = (flag ? string.Empty : _ED3E._E000(171571));
		_startButton.SetDisabledTooltip(tooltip);
		_startButton.Interactable = flag;
	}

	private void _E002()
	{
		string text = TimeSpan.FromSeconds(_E018.ImprovementTime).TimeLeftShortFormat('\n');
		_expectedTime.SetMonospaceText(text);
	}

	public new void ShowGameObject()
	{
		base.gameObject.SetActive(value: true);
	}

	private bool _E003()
	{
		return _E018.Requirements.All((Requirement v) => v.Fulfilled);
	}

	public void UpdateView()
	{
		if (this == null)
		{
			return;
		}
		_E000();
		if (_E015 == null)
		{
			return;
		}
		foreach (var (requirement2, itemRequirementPanel2) in _E015)
		{
			itemRequirementPanel2.Show(this._E001, this._E000, requirement2, this._E002.Template.Type, ignoreFulfillment: false);
		}
	}

	private void Update()
	{
		DateTime utcNow = _E5AD.UtcNow;
		if (!((utcNow - _E012).TotalSeconds < 1.0))
		{
			_E012 = utcNow;
			_E004();
		}
	}

	private void _E004()
	{
		if (_E018.Completed)
		{
			_status.SetMonospaceText(_E014.Localized());
		}
		if (_E018.Processing)
		{
			float f = (float)_E018.TimePassed() / (float)_E018.ImprovementTime * 100f;
			_status.SetMonospaceText(_E013.Localized() + _ED3E._E000(54246) + _E018.TimeLeft().ToTimeString() + _ED3E._E000(27308));
			_percentageText.SetMonospaceText(string.Format(_ED3E._E000(171594), Mathf.FloorToInt(f)));
		}
	}

	private void _E005()
	{
		if (_E003())
		{
			Singleton<_E815>.Instance.StartImprovement(_E018, this._E002.Template.Type).HandleExceptions();
		}
	}

	private void _E006()
	{
		_startButton.gameObject.SetActive(value: true);
		_percentagePanel.SetActive(value: false);
		_expectedTimePanel.SetActive(value: true);
		_status.gameObject.SetActive(value: false);
	}

	private void _E007()
	{
		_startButton.gameObject.SetActive(value: false);
		_percentagePanel.SetActive(value: true);
		_expectedTimePanel.SetActive(value: false);
		_status.gameObject.SetActive(value: true);
	}

	private void _E008()
	{
		_startButton.gameObject.SetActive(value: false);
		_percentagePanel.SetActive(value: false);
		_expectedTimePanel.SetActive(value: false);
		_status.gameObject.SetActive(value: true);
	}
}
