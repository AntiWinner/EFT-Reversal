using System;
using System.Collections.Generic;
using UnityEngine;

namespace EFT.UI;

[CreateAssetMenu(fileName = "New PocketMapConfig", menuName = "Scriptable objects/PocketMapConfig")]
public class PocketMapConfig : ScriptableObject
{
	[Serializable]
	public struct TileInfo
	{
		public Texture Tile;

		public int X;

		public int Y;

		public int Scale;
	}

	public int ScalePreset;

	public Texture[] Textures;

	public TileInfo[] Tiles;

	public Texture GetTexture(int x, int y, int scale)
	{
		for (int i = 0; i < Tiles.Length; i++)
		{
			TileInfo tileInfo = Tiles[i];
			if (tileInfo.X == x && tileInfo.Y == y && tileInfo.Scale == scale)
			{
				return tileInfo.Tile;
			}
		}
		return null;
	}

	public void ConvertTexturesToTiles()
	{
		List<TileInfo> list = new List<TileInfo>();
		for (int i = 0; i < Textures.Length; i++)
		{
			Texture texture = Textures[i];
			string[] array = texture.name.Replace(_ED3E._E000(258910), string.Empty).Replace(_ED3E._E000(96989), _ED3E._E000(48793)).Split('_');
			int num = Convert.ToInt32(array[2]);
			if (num == ScalePreset)
			{
				list.Add(new TileInfo
				{
					Tile = texture,
					X = Convert.ToInt32(array[0]),
					Y = Convert.ToInt32(array[1]),
					Scale = num
				});
			}
		}
		Tiles = list.ToArray();
		Textures = new Texture[0];
	}
}
