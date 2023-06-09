using System;

namespace EFT.InventoryLogic;

[Serializable]
[_EBED]
public class MapMarker : ICloneable
{
	public MapMarkerType Type;

	public int X;

	public int Y;

	public string Note;

	[NonSerialized]
	public const int CHARACTER_LIMIT = 75;

	public MapMarker()
	{
	}

	public MapMarker(MapMarkerType type, int x, int y, string note)
	{
		Type = type;
		X = x;
		Y = y;
		Note = note;
	}

	public void CopyFieldsFrom(MapMarker marker)
	{
		Type = marker.Type;
		X = marker.X;
		Y = marker.Y;
		Note = marker.Note;
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
