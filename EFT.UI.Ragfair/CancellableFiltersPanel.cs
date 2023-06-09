using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public sealed class CancellableFiltersPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _ECBD ragfair;

		internal void _E000(_ECC0 item, CancellableFilterPanel view)
		{
			view.Show(ragfair, item);
		}

		internal void _E001(_ECC0 item, CancellableFilterPanel view)
		{
			view.Show(ragfair, item);
		}
	}

	[SerializeField]
	private GameObject _verticalPanel;

	[SerializeField]
	private Button _expandButton;

	[SerializeField]
	private CancellableFilterPanel _horizontalFilterPanel;

	[SerializeField]
	private RectTransform _horizontalContainer;

	[SerializeField]
	private CancellableFilterPanel _verticalFilterPanel;

	[SerializeField]
	private RectTransform _verticalContainer;

	private _ECC3 _E324;

	private bool _E325;

	private Coroutine _E326;

	private void Awake()
	{
		_expandButton.onClick.AddListener(delegate
		{
			_E000(!_E325);
		});
	}

	public void Show(_ECBD ragfair)
	{
		_E324 = ragfair.CancellableFilters;
		ShowGameObject();
		UI.AddDisposable(new _EC71<_ECC0, CancellableFilterPanel>(_E324.HorizontalFilters, _horizontalFilterPanel, _horizontalContainer, delegate(_ECC0 item, CancellableFilterPanel view)
		{
			view.Show(ragfair, item);
		}));
		UI.AddDisposable(new _EC71<_ECC0, CancellableFilterPanel>(_E324.VerticalFilters, _verticalFilterPanel, _verticalContainer, delegate(_ECC0 item, CancellableFilterPanel view)
		{
			view.Show(ragfair, item);
		}));
		UI.AddDisposable(_E324.HorizontalFilters.ItemsChanged.Subscribe(_E003));
		_E002();
		UI.AddDisposable(_E324.HorizontalFilters.ItemsChanged.Subscribe(_E002));
		UI.AddDisposable(_E324.VerticalFilters.ItemsChanged.Subscribe(_E001));
		_E001();
		_E003();
		_E000(status: false);
	}

	private void _E000(bool status)
	{
		_E325 = status;
		_verticalPanel.gameObject.SetActive(status);
		if (status)
		{
			_E003();
		}
	}

	private void _E001()
	{
		bool active = _E324.VerticalFilters.Count > 0;
		_verticalContainer.gameObject.SetActive(active);
		_expandButton.gameObject.SetActive(active);
	}

	private void _E002()
	{
		bool active = _E324.HorizontalFilters.Count > 0;
		_horizontalContainer.gameObject.SetActive(active);
	}

	private void _E003()
	{
		if (!(this == null))
		{
			if (_E326 != null)
			{
				StaticManager.KillCoroutine(_E326);
			}
			_E326 = StaticManager.BeginCoroutine(_E004());
		}
	}

	private IEnumerator _E004()
	{
		yield return null;
		_verticalPanel.transform.position = _expandButton.transform.position;
		_E326 = null;
	}

	[CompilerGenerated]
	private void _E005()
	{
		_E000(!_E325);
	}
}
