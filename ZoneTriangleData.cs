using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class ZoneTriangleData
{
	private const bool AS_JSON = true;

	private const string resourcePath = "AIZoneData/Zone_{0}_{1}";

	private const int TRIANGLE_CACHE_SIDE_SIZE = 8;

	private static string PATH_TO_SAVE2 = _ED3E._E000(25442);

	private static string PATH_TO_SAVE = _ED3E._E000(25505) + PATH_TO_SAVE2;

	public TriangleData[] Triangles;

	public Dictionary<int, TriangleData> TrianglesD = new Dictionary<int, TriangleData>();

	private TriangleCache[,] _triangleCache;

	private readonly List<TriangleData> emptyList = new List<TriangleData>();

	public bool IsLoaded { get; private set; }

	public static void Save(TriangleData[] triangles, string zoneName, string mapName)
	{
		_E000(triangles, string.Format(PATH_TO_SAVE, mapName, zoneName));
	}

	public static TriangleData[] Parse(NavTriangle[] triangles)
	{
		TriangleData[] array = new TriangleData[triangles.Length];
		for (int i = 0; i < array.Length; i++)
		{
			TriangleData triangleData = new TriangleData(triangles[i]);
			array[i] = triangleData;
		}
		return array;
	}

	[CanBeNull]
	public static TriangleData[] LoadAsJson(string path)
	{
		TextAsset textAsset = _E3A2.Load<TextAsset>(path);
		if (textAsset == null)
		{
			return null;
		}
		TriangleData[] array = textAsset.text.ParseJsonTo<TriangleData[]>(Array.Empty<JsonConverter>());
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ReworkToDictionary();
		}
		return array;
	}

	private static void _E000(TriangleData[] triangles, string path)
	{
		string value = triangles.ToJson();
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		File.Create(path).Dispose();
		StreamWriter streamWriter = new StreamWriter(path);
		streamWriter.Write(value);
		streamWriter.Flush();
		streamWriter.Close();
	}

	private static void _E001(TriangleData[] triangles, string path)
	{
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		File.Create(path).Dispose();
		BinaryWriter binaryWriter = new BinaryWriter(File.Open(path, FileMode.Open));
		binaryWriter.Write(triangles.Length);
		foreach (TriangleData triangleData in triangles)
		{
			binaryWriter.Write(triangleData.CenterX);
			binaryWriter.Write(triangleData.CenterY);
			binaryWriter.Write(triangleData.CenterZ);
			int num = triangleData.Dists.Length;
			binaryWriter.Write(num);
			for (int j = 0; j < num; j++)
			{
				int value = triangleData.Ids[j];
				float value2 = triangleData.Dists[j];
				binaryWriter.Write(value);
				binaryWriter.Write(value2);
			}
		}
		binaryWriter.Flush();
		binaryWriter.Close();
	}

	public ZoneTriangleData(string zoneName, string mapName)
	{
		Triangles = LoadAsJson(string.Format(_ED3E._E000(25466), mapName, zoneName));
		if (Triangles != null)
		{
			TriangleData[] triangles = Triangles;
			foreach (TriangleData triangleData in triangles)
			{
				TrianglesD.Add(triangleData.Id, triangleData);
			}
			IsLoaded = true;
		}
	}

	public List<TriangleData> GetNearestTriangles(Vector3 pos)
	{
		if (_triangleCache == null)
		{
			_E002();
		}
		float num = float.MaxValue;
		TriangleCache triangleCache = null;
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				float num2 = pos.SqrDistance(_triangleCache[i, j].centerPosition);
				if (num2 < num)
				{
					num = num2;
					triangleCache = _triangleCache[i, j];
				}
			}
		}
		if (triangleCache == null)
		{
			if (emptyList.Count > 0)
			{
				emptyList.Clear();
			}
			return emptyList;
		}
		return triangleCache.triangles;
	}

	private void _E002()
	{
		TriangleData[] triangles = Triangles;
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		float num3 = float.MinValue;
		float num4 = float.MinValue;
		foreach (TriangleData triangleData in triangles)
		{
			if (triangleData.CenterX < num)
			{
				num = triangleData.CenterX;
			}
			if (triangleData.CenterX > num3)
			{
				num3 = triangleData.CenterX;
			}
			if (triangleData.CenterZ < num2)
			{
				num2 = triangleData.CenterZ;
			}
			if (triangleData.CenterZ > num4)
			{
				num4 = triangleData.CenterZ;
			}
		}
		float num5 = (num4 - num2) / 8f;
		float num6 = (num4 - num2) / 8f;
		_triangleCache = new TriangleCache[8, 8];
		for (int j = 0; j < 8; j++)
		{
			for (int k = 0; k < 8; k++)
			{
				_triangleCache[j, k] = new TriangleCache();
				_triangleCache[j, k].centerPosition = new Vector3(num + num5 * (float)j, 0f, num2 + num6 * (float)k);
			}
		}
		for (int l = 0; l < triangles.Length; l++)
		{
			for (int m = 0; m < 8; m++)
			{
				for (int n = 0; n < 8; n++)
				{
					Vector3 center = triangles[l].GetCenter();
					if (Mathf.Abs(_triangleCache[m, n].centerPosition.x - center.x) <= num5 || Mathf.Abs(_triangleCache[m, n].centerPosition.z - center.z) <= num6)
					{
						_triangleCache[m, n].triangles.Add(triangles[l]);
					}
				}
			}
		}
	}

	private void _E003(string zoneName, string mapName)
	{
		string path = string.Format(PATH_TO_SAVE, mapName, zoneName);
		if (File.Exists(path))
		{
			BinaryReader file = new BinaryReader(File.Open(path, FileMode.Open));
			_E004(file);
		}
	}

	private void _E004(BinaryReader file)
	{
		int num = file.ReadInt32();
		Triangles = new TriangleData[num];
		for (int i = 0; i < num; i++)
		{
			float centerX = file.ReadSingle();
			float centerY = file.ReadSingle();
			float centerZ = file.ReadSingle();
			TriangleData triangleData = new TriangleData(centerX, centerY, centerZ);
			Triangles[i] = triangleData;
			int num2 = file.ReadInt32();
			triangleData.Ids = new int[num2];
			triangleData.Dists = new float[num2];
			for (int j = 0; j < num2; j++)
			{
				triangleData.Ids[j] = file.ReadInt32();
				triangleData.Dists[j] = file.ReadSingle();
			}
			triangleData.ReworkToDictionary();
		}
		_E002();
		IsLoaded = true;
	}
}
