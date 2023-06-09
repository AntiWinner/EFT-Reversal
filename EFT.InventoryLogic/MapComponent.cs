using System.Collections.Generic;
using JetBrains.Annotations;

namespace EFT.InventoryLogic;

public class MapComponent : _EB19
{
	[_E63C]
	public List<MapMarker> Markers = new List<MapMarker>();

	private readonly _E9E3 _E000;

	public string ConfigPathStr => _E000.ConfigPathStr;

	public int MaxMarkersCount => _E000.MaxMarkersCount;

	public float ScaleMin => _E000.ScaleMin;

	public float ScaleMax => _E000.ScaleMax;

	public MapComponent(Item item, _E9E3 template)
		: base(item)
	{
		_E000 = template;
	}

	[CanBeNull]
	public MapMarker GetMarker(int x, int y)
	{
		foreach (MapMarker marker in Markers)
		{
			if (marker.X != x || marker.Y != y)
			{
				continue;
			}
			return marker;
		}
		return null;
	}

	public _ECD8<_EB57> CreateMarker(MapMarker marker, bool simulate)
	{
		if (Markers.Count + 1 >= MaxMarkersCount)
		{
			return new _ECD2(_ED3E._E000(215742));
		}
		if (!simulate)
		{
			Markers.Add(marker);
		}
		return new _EB57(this, marker);
	}

	public _ECD8<_EB59> DeleteMarker(int x, int y, bool simulate)
	{
		MapMarker marker = GetMarker(x, y);
		if (marker == null)
		{
			return new _ECD2(string.Format(_ED3E._E000(215767), x, y));
		}
		if (!simulate)
		{
			Markers.Remove(marker);
		}
		return new _EB59(this, x, y, marker);
	}

	public _ECD8<_EB58> EditMarker(MapMarker marker, int x, int y, bool simulate)
	{
		MapMarker marker2 = GetMarker(x, y);
		if (marker2 == null)
		{
			return new _ECD2(string.Format(_ED3E._E000(215790), x, y));
		}
		MapMarker originalMarker = (simulate ? marker2 : ((MapMarker)marker2.Clone()));
		if (!simulate)
		{
			marker2.CopyFieldsFrom(marker);
		}
		return new _EB58(this, marker, x, y, originalMarker);
	}
}
