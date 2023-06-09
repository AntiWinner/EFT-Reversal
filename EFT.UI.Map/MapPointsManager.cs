using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI.Map;

public class MapPointsManager : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public EntryPoint entryPoint;

		internal bool _E000(ExtractionPoint extraction)
		{
			return entryPoint.OpenExtractionPoints.Contains(extraction.Name);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public ExtractionPoint exfil;

		internal bool _E000(KeyValuePair<EntryPoint, EntryPointView> x)
		{
			return x.Key.OpenExtractionPoints.Contains(exfil.Name);
		}
	}

	[SerializeField]
	private SimplePocketMap _pocketMap;

	[SerializeField]
	private SelectEntryPointPanel _entryPointPanel;

	[SerializeField]
	private EntryPointView _entryPointTemplate;

	[SerializeField]
	private ExtractionPointView _extractionPointTemplate;

	[SerializeField]
	private bool _editPointPositions;

	private bool _E31B;

	private MapPoints _E31F;

	private readonly Dictionary<EntryPoint, EntryPointView> _E320 = new Dictionary<EntryPoint, EntryPointView>();

	private readonly List<ExtractionPointView> _E321 = new List<ExtractionPointView>();

	public void Show(bool allowSelection, [CanBeNull] MapPoints mapPoints)
	{
		_E31B = allowSelection;
		_E31F = mapPoints;
		ShowGameObject();
		Load();
	}

	private void _E000(EntryPoint entryPoint)
	{
		if (!entryPoint.IsActive)
		{
			Debug.LogWarning(_ED3E._E000(233277) + entryPoint.Name + _ED3E._E000(233269));
			return;
		}
		entryPoint.OnDisable += _E001;
		EntryPointView entryPointView = Object.Instantiate(_entryPointTemplate, _pocketMap.transform);
		entryPointView.Show(_E31B, entryPoint, _editPointPositions, delegate(EntryPoint arg)
		{
			SelectPoint(arg);
			_entryPointPanel.Select(arg);
		}, delegate(EntryPoint arg)
		{
			arg.Index = _E320.Count + 1;
			_E000(arg);
		});
		_E320.Add(entryPoint, entryPointView);
	}

	private void _E001(EntryPoint entryPoint)
	{
		_E320[entryPoint].gameObject.SetActive(value: false);
	}

	private void _E002(ExtractionPoint extractionPoint)
	{
		ExtractionPointView extractionPointView = Object.Instantiate(_extractionPointTemplate, _pocketMap.transform);
		extractionPointView.Show(extractionPoint, _E002);
		_E321.Add(extractionPointView);
	}

	private IEnumerable<ExtractionPoint> _E003(EntryPoint entryPoint)
	{
		return _E31F.ExtractionPoints.Where((ExtractionPoint extraction) => entryPoint.OpenExtractionPoints.Contains(extraction.Name));
	}

	private int _E004(ExtractionPoint exfil)
	{
		return _E320.Count((KeyValuePair<EntryPoint, EntryPointView> x) => x.Key.OpenExtractionPoints.Contains(exfil.Name));
	}

	public void SelectPoint([CanBeNull] EntryPoint entryPoint)
	{
		if (!_E31B)
		{
			return;
		}
		if (entryPoint == null)
		{
			foreach (KeyValuePair<EntryPoint, EntryPointView> item in _E320)
			{
				item.Value.Deselect();
			}
			{
				foreach (ExtractionPointView item2 in _E321)
				{
					item2.gameObject.SetActive(value: false);
				}
				return;
			}
		}
		foreach (KeyValuePair<EntryPoint, EntryPointView> item3 in _E320)
		{
			item3.Value.Deselect();
		}
		_E320[entryPoint].Select();
		foreach (ExtractionPointView item4 in _E321)
		{
			item4.gameObject.SetActive((from x in _E003(entryPoint)
				select x.Name).Contains(item4.ExtractionPoint.Name));
		}
	}

	public void Load()
	{
		_E005();
		for (int i = 0; i < _E31F.EntryPoints.Count; i++)
		{
			EntryPoint entryPoint = _E31F.EntryPoints[i];
			entryPoint.Index = i + 1;
			_E000(entryPoint);
		}
		foreach (ExtractionPoint extractionPoint in _E31F.ExtractionPoints)
		{
			_E002(extractionPoint);
		}
		foreach (ExtractionPointView item in _E321)
		{
			item.gameObject.SetActive(value: false);
		}
		if (!_E31B)
		{
			foreach (ExtractionPointView item2 in _E321)
			{
				int num = _E004(item2.ExtractionPoint);
				foreach (KeyValuePair<EntryPoint, EntryPointView> item3 in _E320)
				{
					if (item3.Key.OpenExtractionPoints.Contains(item2.ExtractionPoint.Name))
					{
						item2.SetMainColor((num == 1) ? item3.Key.MainColor : new Color32(66, 66, 66, byte.MaxValue));
					}
				}
			}
		}
		if (_E31B)
		{
			return;
		}
		foreach (KeyValuePair<EntryPoint, EntryPointView> item4 in _E320)
		{
			item4.Value.Select();
		}
		foreach (ExtractionPointView item5 in _E321)
		{
			item5.gameObject.SetActive(value: true);
		}
	}

	private void _E005()
	{
		for (int num = _E320.Count - 1; num >= 0; num--)
		{
			Object.DestroyImmediate(_E320.ElementAt(num).Value.gameObject);
			_E320.ElementAt(num).Key.OnDisable -= _E001;
		}
		_E320.Clear();
		for (int num2 = _E321.Count - 1; num2 >= 0; num2--)
		{
			Object.DestroyImmediate(_E321.ElementAt(num2).gameObject);
		}
		_E321.Clear();
	}

	public override void Close()
	{
		_E005();
		base.Close();
	}

	[CompilerGenerated]
	private void _E006(EntryPoint arg)
	{
		SelectPoint(arg);
		_entryPointPanel.Select(arg);
	}

	[CompilerGenerated]
	private void _E007(EntryPoint arg)
	{
		arg.Index = _E320.Count + 1;
		_E000(arg);
	}
}
