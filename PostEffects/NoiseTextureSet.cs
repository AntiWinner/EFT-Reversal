using UnityEngine;

namespace PostEffects;

public sealed class NoiseTextureSet : ScriptableObject
{
	[SerializeField]
	private Texture2D[] _textures;

	public Texture2D GetTexture()
	{
		return GetTexture(Time.frameCount);
	}

	public Texture2D GetTexture(int frameCount)
	{
		return _textures[frameCount % _textures.Length];
	}
}
