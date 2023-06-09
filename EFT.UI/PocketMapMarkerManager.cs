using System;
using System.Collections.Generic;
using System.Linq;
using Comfort.Common;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class PocketMapMarkerManager : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	[Serializable]
	public sealed class ToggleData
	{
		public MapMarkerType Type;

		public Toggle Toggle;

		public CustomTextMeshProUGUI Label;
	}

	[SerializeField]
	private GameObject _markerPrefab;

	[SerializeField]
	private RectTransform _container;

	[SerializeField]
	private SpriteMap _spriteMap;

	[SerializeField]
	private ToggleData[] _toggles;

	[SerializeField]
	private CustomTextMeshProUGUI _markersCounter;

	[SerializeField]
	private CustomToggle _legendButton;

	[SerializeField]
	private GameObject _legend;

	private MapComponent _map;

	private IItemOwner _mapOwner;

	private _EAE6 _itemController;

	private MapMarkerWindow _markerWindow;

	private readonly List<PocketMapMarker> _markers = new List<PocketMapMarker>();

	public bool Shown { get; private set; }

	private void Awake()
	{
		ToggleData[] toggles = _toggles;
		foreach (ToggleData toggleData in toggles)
		{
			ToggleData toggleDataSave = toggleData;
			toggleData.Toggle.onValueChanged.AddListener(delegate(bool value)
			{
				SetMarkersVisible(toggleDataSave.Type, value);
			});
		}
		_legendButton.onValueChanged.AddListener(delegate
		{
			_legend.SetActive(!_legend.activeSelf);
		});
	}

	public void Show(MapComponent map, _EAE6 itemController, MapMarkerWindow markerWindow)
	{
		Shown = true;
		_map = map;
		_mapOwner = map.Item.Parent.GetOwner();
		_itemController = itemController;
		_markerWindow = markerWindow;
		_mapOwner.CreateMapMarkerEvent += OnCreateMapMarkerEvent;
		_mapOwner.EditMapMarkerEvent += OnEditMapMarkerEvent;
		_mapOwner.DeleteMapMarkerEvent += OnDeleteMapMarkerEvent;
		foreach (MapMarker marker in _map.Markers)
		{
			CreateMarkerView(marker);
		}
		UpdateLegend();
	}

	public void Hide()
	{
		_mapOwner.CreateMapMarkerEvent -= OnCreateMapMarkerEvent;
		_mapOwner.EditMapMarkerEvent -= OnEditMapMarkerEvent;
		_mapOwner.DeleteMapMarkerEvent -= OnDeleteMapMarkerEvent;
		foreach (PocketMapMarker marker in _markers)
		{
			UnityEngine.Object.Destroy(marker.gameObject);
		}
		_markers.Clear();
		Shown = false;
		_markerWindow.Hide();
	}

	private void OnCreateMapMarkerEvent(_EB04 args)
	{
		if (args.Map == _map)
		{
			switch (args.Status)
			{
			case CommandStatus.Begin:
				CreateMarkerView(args.MapMarker);
				break;
			case CommandStatus.Failed:
				RemoveMarkerView(args.MapMarker.X, args.MapMarker.Y);
				break;
			}
			UpdateLegend();
		}
	}

	private void OnEditMapMarkerEvent(_EB05 args)
	{
		if (args.Map == _map)
		{
			switch (args.Status)
			{
			case CommandStatus.Begin:
				EditMarkerView(args.X, args.Y, args.MapMarker);
				break;
			case CommandStatus.Failed:
				DiscardEditMarkerView(args.X, args.Y);
				break;
			}
			UpdateLegend();
		}
	}

	private void OnDeleteMapMarkerEvent(_EB06 args)
	{
		if (args.Map != _map)
		{
			return;
		}
		switch (args.Status)
		{
		case CommandStatus.Begin:
			RemoveMarkerView(args.X, args.Y);
			break;
		case CommandStatus.Failed:
		{
			MapMarker marker = _map.GetMarker(args.X, args.Y);
			if (marker != null)
			{
				CreateMarkerView(marker);
			}
			else
			{
				Debug.LogError("Marker is null");
			}
			break;
		}
		}
		UpdateLegend();
	}

	private void CreateMarkerView(MapMarker marker)
	{
		GameObject obj = UnityEngine.Object.Instantiate(_markerPrefab, _container, worldPositionStays: true);
		obj.transform.localScale = Vector3.one;
		obj.SetActive(IsMarkerTypeVisible(marker.Type));
		PocketMapMarker component = obj.GetComponent<PocketMapMarker>();
		component.Init(marker, this);
		_markers.Add(component);
		RectTransform rectTransform = _container.RectTransform();
		Vector2 size = rectTransform.rect.size;
		Vector2 pivot = rectTransform.pivot;
		Vector2 vector = size * pivot;
		Vector2 anchoredPosition = new Vector2(marker.X, marker.Y);
		anchoredPosition += vector;
		component.RectTransform().anchoredPosition = anchoredPosition;
	}

	private void EditMarkerView(int x, int y, MapMarker marker)
	{
		PocketMapMarker markerView = GetMarkerView(x, y);
		if (markerView == null)
		{
			Debug.LogErrorFormat("cant find pocket map marker view at {0}x{1} (edit)", x, y);
		}
		else
		{
			markerView.Init(marker, this);
		}
	}

	private void DiscardEditMarkerView(int x, int y)
	{
		MapMarker marker = _map.GetMarker(x, y);
		if (marker == null)
		{
			Debug.LogErrorFormat("cant find pocket map marker at {0}x{1} (discard)", x, y);
			return;
		}
		PocketMapMarker markerView = GetMarkerView(x, y);
		if (markerView == null)
		{
			Debug.LogErrorFormat("cant find pocket map marker view at {0}x{1} (discard)", x, y);
		}
		else
		{
			markerView.Init(marker, this);
		}
	}

	private void RemoveMarkerView(int x, int y)
	{
		using IEnumerator<PocketMapMarker> enumerator = _markers.Where((PocketMapMarker mapMarker) => mapMarker.Marker.X == x && mapMarker.Marker.Y == y).GetEnumerator();
		if (enumerator.MoveNext())
		{
			PocketMapMarker current = enumerator.Current;
			UnityEngine.Object.Destroy(current.gameObject);
			_markers.Remove(current);
		}
	}

	[CanBeNull]
	private PocketMapMarker GetMarkerView(int x, int y)
	{
		return _markers.FirstOrDefault((PocketMapMarker mapMarker) => mapMarker.Marker.X == x && mapMarker.Marker.Y == y);
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(_container, eventData.position, null, out var localPoint);
		if (!_container.rect.Contains(localPoint) || _map == null)
		{
			return;
		}
		switch (eventData.button)
		{
		case PointerEventData.InputButton.Right:
			ShowCreateContextMenu(eventData.position);
			break;
		case PointerEventData.InputButton.Left:
			if (eventData.clickCount > 1)
			{
				MarkerCreate(eventData.position);
			}
			break;
		}
	}

	private void ShowCreateContextMenu(Vector2 position)
	{
		ItemUiContext.Instance.ContextMenu.Show(position, new _EC56(null, position, MarkerCreate, null, null), new Dictionary<EPocketMapContextInteractions, string> { 
		{
			EPocketMapContextInteractions.MarkerCreate,
			"MarkerCreate".Localized()
		} }, new Dictionary<EPocketMapContextInteractions, Sprite> { 
		{
			EPocketMapContextInteractions.MarkerCreate,
			_spriteMap["create"]
		} });
	}

	public void ShowEditContextMenu(Vector2 position, MapMarker marker)
	{
		ItemUiContext.Instance.ContextMenu.Show(position, new _EC56(marker, position, null, MarkerDelete, MarkerEdit), new Dictionary<EPocketMapContextInteractions, string>
		{
			{
				EPocketMapContextInteractions.MarkerDelete,
				"MarkerDelete".Localized()
			},
			{
				EPocketMapContextInteractions.MarkerEdit,
				"MarkerEdit".Localized()
			}
		}, new Dictionary<EPocketMapContextInteractions, Sprite>
		{
			{
				EPocketMapContextInteractions.MarkerDelete,
				_spriteMap["delete"]
			},
			{
				EPocketMapContextInteractions.MarkerEdit,
				_spriteMap["edit"]
			}
		});
	}

	private void HideMarkerWindow()
	{
		_markerWindow.Hide();
	}

	private void MarkerCreate(Vector2 position)
	{
		if (_map.Markers.Count + 1 >= _map.MaxMarkersCount)
		{
			ItemUiContext.Instance.ShowMessageWindow("PocketMapMarkersLimitReached".Localized(), delegate
			{
			}, null, "Alert".Localized());
			return;
		}
		_markerWindow.Show(position, delegate(MapMarkerType type, string note)
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuMapTag);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(_container, position, null, out var localPoint);
			MapMarker marker = new MapMarker(type, (int)localPoint.x, (int)localPoint.y, note);
			_itemController.TryRunNetworkTransaction(_map.CreateMarker(marker, simulate: true));
			HideMarkerWindow();
		}, HideMarkerWindow);
	}

	private void MarkerEdit(Vector2 position, MapMarker marker)
	{
		_markerWindow.Show(position, delegate(MapMarkerType type, string note)
		{
			MapMarker mapMarker = (MapMarker)marker.Clone();
			mapMarker.Type = type;
			mapMarker.Note = note;
			_itemController.TryRunNetworkTransaction(_map.EditMarker(mapMarker, marker.X, marker.Y, simulate: true));
			HideMarkerWindow();
		}, HideMarkerWindow);
		_markerWindow.FillWithData(marker);
	}

	private void MarkerDelete(MapMarker marker)
	{
		_itemController.TryRunNetworkTransaction(_map.DeleteMarker(marker.X, marker.Y, simulate: true));
	}

	private int GetMarkersCount(MapMarkerType type)
	{
		return _map.Markers.Count((MapMarker marker) => marker.Type == type);
	}

	private void UpdateLegend()
	{
		ToggleData[] toggles = _toggles;
		foreach (ToggleData toggleData in toggles)
		{
			int markersCount = GetMarkersCount(toggleData.Type);
			toggleData.Label.text = string.Format("{0} ({1})", ("MapMarkerType_" + toggleData.Type).Localized(), markersCount);
		}
		_markersCounter.text = $"{_map.Markers.Count}/{_map.MaxMarkersCount}";
	}

	private bool IsMarkerTypeVisible(MapMarkerType type)
	{
		return (from toggleData in _toggles
			where toggleData.Type == type
			select toggleData.Toggle.isOn).FirstOrDefault();
	}

	public void SetMarkersVisible(MapMarkerType type, bool value)
	{
		foreach (PocketMapMarker marker in _markers)
		{
			if (marker.Marker.Type == type)
			{
				marker.gameObject.SetActive(value);
			}
		}
	}
}
