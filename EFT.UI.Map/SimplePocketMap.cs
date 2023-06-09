using System;
using System.Collections.Generic;
using EFT.InventoryLogic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Map;

public class SimplePocketMap : UIElement
{
	public class JsonMapConfig
	{
		public string bundlePath;

		public int mapSizeX;

		public int mapSizeY;

		public int tileSizeX;

		public int tileSizeY;

		public int scaledVariantsCount;
	}

	[SerializeField]
	private ScrollRect _scrollRect;

	[SerializeField]
	private PocketMapTile _pocketMapTile;

	[SerializeField]
	private RectTransform _tilesContainer;

	[SerializeField]
	private GameObject _loader;

	protected MapComponent Map;

	protected Vector2 MapSize;

	private Vector2 _tileSize;

	private string _bundlePath;

	private PocketMapConfig _pocketMapConfig;

	protected readonly List<PocketMapTile> Tiles = new List<PocketMapTile>();

	private readonly List<int> _tileScaleVariants = new List<int>();

	private _ECB7 _bundleLoader;

	public bool Shown { get; private set; }

	protected virtual void Awake()
	{
		_scrollRect.onValueChanged.AddListener(delegate
		{
			CheckTilesForUpdate();
		});
	}

	public virtual void Show(MapComponent map)
	{
		Map = map;
		ShowGameObject();
		LoadConfig(Map.ConfigPathStr);
		Shown = true;
		_loader.SetActive(value: true);
		_bundleLoader = new _ECB7(_bundlePath);
		_bundleLoader.BundleReceived += BundleLoaded;
		_bundleLoader.RequestBundle(1);
	}

	private void BundleLoaded(PocketMapConfig config)
	{
		if (_bundleLoader != null)
		{
			_bundleLoader.BundleReceived -= BundleLoaded;
			_pocketMapConfig = config;
			ShowOnPath(Map.ConfigPathStr);
		}
	}

	public void ShowOnPath(string path)
	{
		ShowGameObject();
		_loader.SetActive(value: false);
		int num = Mathf.CeilToInt(MapSize.x / _tileSize.x);
		int num2 = Mathf.CeilToInt(MapSize.y / _tileSize.y);
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				PocketMapTile pocketMapTile = UnityEngine.Object.Instantiate(_pocketMapTile, _tilesContainer);
				Transform obj = pocketMapTile.transform;
				obj.position = Vector3.one;
				obj.localScale = Vector3.one;
				obj.SetSiblingIndex(0);
				Tiles.Add(pocketMapTile);
				pocketMapTile.Init(_tileScaleVariants, _tileSize, $"{_pocketMapTile.name}_{j}x{i}", j, i, _bundleLoader);
				((RectTransform)obj).anchoredPosition3D = new Vector3((float)j * _tileSize.x, (float)(-i) * _tileSize.y);
			}
		}
		base.transform.localPosition = Vector3.zero;
		CheckTilesForUpdate();
	}

	protected void CheckTilesForUpdate()
	{
		foreach (PocketMapTile tile in Tiles)
		{
			tile.CheckForUpdate();
		}
	}

	private void LoadConfig(string configRelativePath)
	{
		TextAsset textAsset = _E905.Pop<TextAsset>(configRelativePath);
		if (!(textAsset == null))
		{
			JsonMapConfig jsonMapConfig = textAsset.text.ParseJsonTo<JsonMapConfig>(Array.Empty<JsonConverter>());
			MapSize = new Vector2(jsonMapConfig.mapSizeX, jsonMapConfig.mapSizeY);
			((RectTransform)base.transform).sizeDelta = MapSize;
			_tileSize = new Vector2(jsonMapConfig.tileSizeX, jsonMapConfig.tileSizeY);
			_bundlePath = jsonMapConfig.bundlePath;
			_tileScaleVariants.Clear();
			int scaledVariantsCount = jsonMapConfig.scaledVariantsCount;
			int num = 1;
			for (int i = 0; i < scaledVariantsCount; i++)
			{
				_tileScaleVariants.Add(num);
				num *= 2;
			}
			_tileScaleVariants.Sort((int x, int y) => y - x);
		}
	}

	public override void Close()
	{
		UnloadMapBundle();
		foreach (PocketMapTile tile in Tiles)
		{
			UnityEngine.Object.Destroy(tile.gameObject);
		}
		Tiles.Clear();
		Shown = false;
		base.Close();
	}

	private void UnloadMapBundle()
	{
		_pocketMapConfig = null;
		if (_bundleLoader != null)
		{
			_bundleLoader.UnloadMapBundle();
			_bundleLoader = null;
		}
	}
}
