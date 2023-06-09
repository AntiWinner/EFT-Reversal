using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AmplifyImpostors;
using Comfort.Common;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace EFT.Impostors;

[CreateAssetMenu(fileName = "ImpostorsArray", order = 86)]
public sealed class AmplifyImpostorsArray : SerializedScriptableObject
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public int index;

		public bool failed;

		public IReadOnlyList<int> ids;

		internal ResourceKey _E000(ResourceKey key)
		{
			if (key == null || string.IsNullOrEmpty(key.path))
			{
				Debug.LogError(string.Format(_ED3E._E000(174578), index));
				failed = true;
				return null;
			}
			if (ids[index] < 0)
			{
				index++;
				return null;
			}
			index++;
			return key;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _ED0A assets;

		internal Texture _E000(ResourceKey key)
		{
			return assets.GetAsset<Texture>(key);
		}
	}

	private static readonly Vector3 m__E000 = new Vector3(0f, 0f, -1f);

	[CompilerGenerated]
	private Action m__E001;

	[CompilerGenerated]
	private Action m__E002;

	public const int VERTEX_NUMBER = 8;

	private const string m__E003 = "39addfe8385b88141953ea24c4e80e74";

	private const string m__E004 = "9e2596aaa367fad40a6a4a54118f6622";

	[OdinSerialize]
	[HideInInspector]
	public ImpostorVertex[] _vertices;

	[OdinSerialize]
	[HideInInspector]
	public ImpostorPropBlock[] _propBlocks;

	[OdinSerialize]
	[HideInInspector]
	public ImpostorBounds[] _bounds;

	[OdinSerialize]
	[HideInInspector]
	public Mesh _mesh;

	[HideInInspector]
	[OdinSerialize]
	public Material _arrayMaterial;

	[SerializeField]
	public Shader Shader;

	[SerializeField]
	public ComputeShader CullingShader;

	[SerializeField]
	public ResourceKey[] AlbedoKeys;

	[SerializeField]
	public ResourceKey[] NormalsKeys;

	[SerializeField]
	public ResourceKey Albedo1Key;

	[SerializeField]
	public ResourceKey Normals1Key;

	[SerializeField]
	public ResourceKey Albedo2Key;

	[SerializeField]
	public ResourceKey Normals2Key;

	public int Length
	{
		get
		{
			ResourceKey[] albedoKeys = AlbedoKeys;
			if (albedoKeys == null)
			{
				return 0;
			}
			return albedoKeys.Length;
		}
	}

	public Mesh Mesh => _mesh;

	public Material ArrayMaterial => _arrayMaterial;

	public ImpostorVertex[] Vertices => _vertices;

	public ImpostorPropBlock[] ImpostorPropBlocks => _propBlocks;

	public ImpostorBounds[] Bounds => _bounds;

	public event Action UnregisterEvent
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E001;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E001;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action RegisterEvent
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E002;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E002, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E002;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E002, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private static ResourceKey[] _E000(IReadOnlyCollection<ResourceKey> keys, IReadOnlyList<int> ids)
	{
		if (keys == null || keys.Count == 0)
		{
			throw new InvalidDataException();
		}
		bool failed = false;
		int index = 0;
		ResourceKey[] result = (from key in keys.Select(delegate(ResourceKey key)
			{
				if (key == null || string.IsNullOrEmpty(key.path))
				{
					Debug.LogError(string.Format(_ED3E._E000(174578), index));
					failed = true;
					return null;
				}
				if (ids[index] < 0)
				{
					index++;
					return null;
				}
				index++;
				return key;
			})
			where key != null
			select key).ToArray();
		if (failed)
		{
			throw new InvalidDataException();
		}
		return result;
	}

	private static async Task<(Texture[], _ED0E<_ED08>._E002)> _E001(IReadOnlyCollection<ResourceKey> keys, IReadOnlyList<int> ids)
	{
		ResourceKey[] source = _E000(keys, ids);
		string[] keys2 = source.Select((ResourceKey key) => key.path).Distinct().ToArray();
		_ED0A assets = Singleton<_ED0A>.Instance;
		_ED0E<_ED08>._E002 obj = assets.Retain(keys2);
		await _E612.LoadBundles(obj);
		return (source.Select((ResourceKey key) => assets.GetAsset<Texture>(key)).ToArray(), obj);
	}

	private static async Task<(Texture2DArray, Texture2DArray, _ED0E<_ED08>._E002 token)> _E002(ResourceKey albedoKey, ResourceKey normalsKey)
	{
		_ED0A instance = Singleton<_ED0A>.Instance;
		_ED0E<_ED08>._E002 obj = instance.Retain(new string[2] { albedoKey.path, normalsKey.path });
		Task task = _E612.LoadBundles(obj);
		await task;
		if (task.IsCanceled || task.IsFaulted)
		{
			Debug.LogError(_ED3E._E000(174594) + albedoKey.path + _ED3E._E000(10270) + normalsKey.path);
		}
		Texture2DArray asset = instance.GetAsset<Texture2DArray>(albedoKey);
		if (asset == null)
		{
			Debug.LogError(_ED3E._E000(174638) + albedoKey.path + _ED3E._E000(14057) + albedoKey.rcid);
		}
		Texture2DArray asset2 = instance.GetAsset<Texture2DArray>(normalsKey);
		if (asset2 == null)
		{
			Debug.LogError(_ED3E._E000(174638) + normalsKey.path + _ED3E._E000(14057) + normalsKey.rcid);
		}
		return (asset, asset2, obj);
	}

	private static Texture2DArray _E003(IReadOnlyList<Texture> textures, string name)
	{
		Texture texture = textures[0];
		Texture2DArray texture2DArray = new Texture2DArray(texture.width, texture.height, textures.Count, TextureFormat.DXT5, mipChain: true, linear: false)
		{
			name = name,
			filterMode = FilterMode.Bilinear,
			wrapMode = TextureWrapMode.Repeat,
			anisoLevel = 1
		};
		for (int i = 0; i < textures.Count; i++)
		{
			Texture texture2 = textures[i];
			for (int j = 0; j < texture2.mipmapCount; j++)
			{
				Graphics.CopyTexture(texture2, 0, j, texture2DArray, i, j);
			}
		}
		return texture2DArray;
	}

	private Material _E004(Texture albedoArray = null, Texture normalsArray = null, Material template = null)
	{
		Material material = ((template != null) ? new Material(template) : new Material(Shader)
		{
			name = base.name,
			enableInstancing = true
		});
		if (albedoArray != null)
		{
			material.SetTexture(AmplifyImpostor.Property_AlbedoArray, albedoArray);
		}
		if (normalsArray != null)
		{
			material.SetTexture(AmplifyImpostor.Property_NormalsArray, normalsArray);
		}
		material.EnableKeyword(_ED3E._E000(174531));
		if (Application.isPlaying)
		{
			material.EnableKeyword(_ED3E._E000(174580));
		}
		else
		{
			material.DisableKeyword(_ED3E._E000(174580));
		}
		return material;
	}

	public async Task<(Material, Texture2DArray, Texture2DArray, _ED0E<_ED08>._E002 token)> RegenerateMaterialQualityHigh(IReadOnlyList<int> ids)
	{
		(Texture[], _ED0E<_ED08>._E002) obj = await _E001((IReadOnlyCollection<ResourceKey>)(object)AlbedoKeys, ids);
		Texture[] item = obj.Item1;
		_ED0E<_ED08>._E002 item2 = obj.Item2;
		Texture2DArray texture2DArray = _E003(item, _ED3E._E000(174671));
		item2?.Release();
		(Texture[], _ED0E<_ED08>._E002) obj2 = await _E001((IReadOnlyCollection<ResourceKey>)(object)NormalsKeys, ids);
		Texture[] item3 = obj2.Item1;
		_ED0E<_ED08>._E002 item4 = obj2.Item2;
		Texture2DArray texture2DArray2 = _E003(item3, _ED3E._E000(174716));
		item4?.Release();
		Material item5 = _E004(texture2DArray, texture2DArray2, ArrayMaterial);
		return (item5, texture2DArray, texture2DArray2, null);
	}

	public async Task<(Material, Texture2DArray, Texture2DArray, _ED0E<_ED08>._E002 token)> RegenerateMaterialQualityMedium()
	{
		(Texture2DArray, Texture2DArray, _ED0E<_ED08>._E002) obj = await _E002(Albedo1Key, Normals1Key);
		Texture2DArray item = obj.Item1;
		Texture2DArray item2 = obj.Item2;
		_ED0E<_ED08>._E002 item3 = obj.Item3;
		Material item4 = _E004(item, item2, ArrayMaterial);
		return (item4, null, null, item3);
	}

	public async Task<(Material, Texture2DArray, Texture2DArray, _ED0E<_ED08>._E002 token)> RegenerateMaterialQualityLow()
	{
		(Texture2DArray, Texture2DArray, _ED0E<_ED08>._E002) obj = await _E002(Albedo2Key, Normals2Key);
		Texture2DArray item = obj.Item1;
		Texture2DArray item2 = obj.Item2;
		_ED0E<_ED08>._E002 item3 = obj.Item3;
		Material item4 = _E004(item, item2, ArrayMaterial);
		return (item4, null, null, item3);
	}
}
