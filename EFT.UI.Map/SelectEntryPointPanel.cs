using System;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Map;

public class SelectEntryPointPanel : UIElement
{
	[SerializeField]
	private GameObject _randomPointObject;

	[SerializeField]
	private GameObject _selectedPointObject;

	[SerializeField]
	private CustomTextMeshProUGUI _entryPointIndex;

	[SerializeField]
	private CustomTextMeshProUGUI _entryPointLabel;

	[SerializeField]
	private Button _middleButton;

	[SerializeField]
	private Button _leftButton;

	[SerializeField]
	private Button _rightButton;

	private _ECB5 _E322;

	private Action<EntryPoint> _E03D;

	public EntryPoint SelectedPoint => _E322.SelectedPoint;

	private void Awake()
	{
		if (_middleButton != null)
		{
			_middleButton.onClick.AddListener(_E001);
		}
		_leftButton.onClick.AddListener(_E000);
		_rightButton.onClick.AddListener(_E001);
	}

	public void Show(bool allowSelection, EntryPoint[] points, Action<EntryPoint> onSelected = null)
	{
		ShowGameObject();
		_E322 = new _ECB5(allowSelection, points);
		_E03D = onSelected;
		Select(_E322.LastPoint);
		if (points.Length == 1)
		{
			_entryPointIndex.text = string.Empty;
			_entryPointLabel.text = _ED3E._E000(233254).Localized();
		}
		bool active = points.Length > 1 && onSelected != null;
		_leftButton.gameObject.SetActive(active);
		_rightButton.gameObject.SetActive(active);
		if (_middleButton != null)
		{
			_middleButton.gameObject.SetActive(active);
		}
		base.gameObject.SetActive(allowSelection);
	}

	private void _E000()
	{
		_E322.SelectPrev();
		EntryPoint selectedPoint = _E322.SelectedPoint;
		_E002(selectedPoint);
		_E03D?.Invoke(selectedPoint);
	}

	private void _E001()
	{
		_E322.SelectNext();
		EntryPoint selectedPoint = _E322.SelectedPoint;
		_E002(selectedPoint);
		_E03D?.Invoke(selectedPoint);
	}

	public void Select(EntryPoint point)
	{
		_E322.Select(point);
		_E002(point);
		_E03D?.Invoke(point);
	}

	private void _E002(EntryPoint point)
	{
		_randomPointObject.SetActive(point == null);
		_selectedPointObject.SetActive(point != null);
		_entryPointIndex.text = ((point == null) ? string.Empty : point.Index.ToString());
		_entryPointLabel.text = ((point == null) ? _ED3E._E000(233254).Localized() : point.Name.Localized());
	}

	public override void Close()
	{
		_E322?.Free();
		_E322 = null;
		base.Close();
	}
}
