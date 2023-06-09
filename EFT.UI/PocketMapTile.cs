using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class PocketMapTile : MonoBehaviour
{
	[SerializeField]
	private RawImage _image;

	private List<int> m__E000;

	private List<float> m__E001 = new List<float>();

	private int m__E002 = -1;

	private int _E003;

	private int _E004;

	private Rect _E005;

	private _ECB6 _E006;

	public void Init(List<int> tileScaleVariants, Vector2 size, string objName, int x, int y, _ECB6 bundleLoader)
	{
		base.gameObject.SetActive(value: true);
		base.gameObject.name = objName;
		this.m__E000 = tileScaleVariants;
		_E006 = bundleLoader;
		_E003 = x;
		_E004 = y;
		foreach (int item in this.m__E000)
		{
			this.m__E001.Add(1f / (float)item);
		}
		this.m__E001.Add(0f);
		this.m__E001.Sort();
		((RectTransform)base.transform).sizeDelta = size;
		_E005 = new Rect(0f, 0f, Screen.width, Screen.height);
		_E006.BundleReceived += _E001;
	}

	private void _E000()
	{
		if (!(_image.texture != null))
		{
			_image.color = Color.clear;
			_E006.RequestBundle(this.m__E002);
		}
	}

	public void UnloadImage()
	{
		_image.texture = null;
		_image.color = Color.clear;
	}

	private void _E001(PocketMapConfig config)
	{
		if (!(_image.texture != null))
		{
			Texture texture = config.GetTexture(_E003, _E004, this.m__E002);
			_image.texture = texture;
			if (!(texture == null))
			{
				_image.color = Color.white;
				((RectTransform)base.transform).sizeDelta = new Vector2(texture.width * this.m__E002, texture.height * this.m__E002);
			}
		}
	}

	private int _E002()
	{
		for (int i = 0; i < this.m__E000.Count; i++)
		{
			if (this.m__E001[i] < base.transform.lossyScale.x && base.transform.lossyScale.x <= this.m__E001[i + 1])
			{
				return this.m__E000[i];
			}
		}
		return this.m__E000[this.m__E000.Count - 1];
	}

	public void CheckForUpdate()
	{
		int num = _E002();
		if (num != this.m__E002)
		{
			this.m__E002 = num;
			UnloadImage();
		}
		Vector3[] array = new Vector3[4];
		((RectTransform)base.transform).GetWorldCorners(array);
		Vector3 vector = RectTransformUtility.WorldToScreenPoint(null, array[0]);
		Vector3 vector2 = RectTransformUtility.WorldToScreenPoint(null, array[2]);
		Rect other = new Rect(vector.x, vector.y, vector2.x - vector.x, vector2.y - vector.y);
		if (_E005.Overlaps(other))
		{
			_E000();
		}
		else
		{
			UnloadImage();
		}
	}

	public RawImage GetImage()
	{
		return _image;
	}

	public int GetCurrentTileScaleVariant()
	{
		return this.m__E002;
	}

	private void OnDestroy()
	{
		_E006.BundleReceived -= _E001;
	}
}
