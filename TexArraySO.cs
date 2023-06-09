using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "TextureArray", menuName = "Scriptable objects/TextureArray")]
[PreferBinarySerialization]
public class TexArraySO : ScriptableObject
{
	[SerializeField]
	[Delayed]
	private int _width = 256;

	[SerializeField]
	[Delayed]
	private int _height = 256;

	[SerializeField]
	private TextureFormat _format = TextureFormat.ARGB32;

	[SerializeField]
	private bool _mips = true;

	[SerializeField]
	private Texture2D[] _textures;

	[HideInInspector]
	[SerializeField]
	private Texture2DArray _textureArray;

	[HideInInspector]
	[SerializeField]
	private bool _hadMips;

	private static void _E000(Texture2DArray textureArray, Texture2D[] textures)
	{
		for (int i = 0; i < textures.Length; i++)
		{
			Color32[] colors = _E001(textures[i], textureArray.width, textureArray.height);
			textureArray.SetPixels32(colors, i);
		}
		textureArray.Apply(updateMipmaps: true, makeNoLongerReadable: true);
	}

	private static Color32[] _E001(Texture2D texture, int width, int height)
	{
		RenderTexture active = RenderTexture.active;
		RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
		Graphics.Blit(texture, temporary);
		RenderTexture.active = temporary;
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, mipChain: false);
		texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0, recalculateMipMaps: false);
		texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: false);
		Color32[] pixels = texture2D.GetPixels32();
		Object.DestroyImmediate(texture2D);
		RenderTexture.active = active;
		RenderTexture.ReleaseTemporary(temporary);
		return pixels;
	}

	private bool _E002(Texture2D[] textures, ref string errorMessage, ref InfoMessageType? messageType)
	{
		if (textures.Length < 1)
		{
			errorMessage = _ED3E._E000(88964);
			messageType = InfoMessageType.Error;
			return false;
		}
		HashSet<Texture2D> hashSet = new HashSet<Texture2D>();
		for (int i = 0; i < textures.Length; i++)
		{
			if (hashSet.Contains(textures[i]))
			{
				errorMessage = string.Format(_ED3E._E000(89000), textures[i].name);
				messageType = InfoMessageType.Error;
				return false;
			}
			hashSet.Add(textures[i]);
		}
		return true;
	}

	private bool _E003(Texture2D[] textures, ref string errorMessage, ref InfoMessageType? messageType)
	{
		new HashSet<Texture2D>();
		for (int i = 0; i < textures.Length; i++)
		{
			if (textures[i].width < _width || textures[i].height < _height)
			{
				errorMessage = string.Format(_ED3E._E000(89077), textures[i].name);
				messageType = InfoMessageType.Warning;
				return false;
			}
		}
		return true;
	}
}
