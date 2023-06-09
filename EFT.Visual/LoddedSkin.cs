using System;
using System.Collections.Generic;
using System.Linq;
using Diz.Skinning;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace EFT.Visual;

public class LoddedSkin : MonoBehaviour
{
	[FormerlySerializedAs("Skins")]
	[SerializeField]
	private AbstractSkin[] _lods;

	private List<HotObject> m__E000 = new List<HotObject>();

	public void Init(Skeleton skeleton, PlayerBody playerBody)
	{
		AbstractSkin[] lods = _lods;
		foreach (AbstractSkin abstractSkin in lods)
		{
			if (abstractSkin is Skin)
			{
				((Skin)abstractSkin).Init(skeleton);
			}
			else if (abstractSkin is TorsoSkin)
			{
				((TorsoSkin)abstractSkin).Init(skeleton, playerBody);
			}
		}
		_E000();
	}

	public void Skin()
	{
		AbstractSkin[] lods = _lods;
		for (int i = 0; i < lods.Length; i++)
		{
			lods[i].ApplySkin();
		}
	}

	public void Unskin()
	{
		AbstractSkin[] lods = _lods;
		for (int i = 0; i < lods.Length; i++)
		{
			lods[i].Unskin();
		}
	}

	public void EnableRenderers(bool on)
	{
		AbstractSkin[] lods = _lods;
		for (int i = 0; i < lods.Length; i++)
		{
			lods[i].SkinnedMeshRenderer.enabled = on;
		}
	}

	public IEnumerable<Renderer> GetRenderers()
	{
		return ((IEnumerable<AbstractSkin>)_lods).Select((Func<AbstractSkin, Renderer>)((AbstractSkin x) => x.SkinnedMeshRenderer));
	}

	public void SetShadowCastingMode(ShadowCastingMode shadowCastingMode)
	{
		AbstractSkin[] lods = _lods;
		for (int i = 0; i < lods.Length; i++)
		{
			lods[i].SkinnedMeshRenderer.shadowCastingMode = shadowCastingMode;
		}
	}

	public bool IsVisible()
	{
		AbstractSkin[] lods = _lods;
		for (int i = 0; i < lods.Length; i++)
		{
			if (lods[i].SkinnedMeshRenderer.isVisible)
			{
				return true;
			}
		}
		return false;
	}

	public void SetLayer(int layer)
	{
		base.gameObject.layer = layer;
		AbstractSkin[] lods = _lods;
		for (int i = 0; i < lods.Length; i++)
		{
			lods[i].gameObject.layer = layer;
		}
	}

	public void SetTemperature(float tempCelsio)
	{
		foreach (HotObject item in this.m__E000)
		{
			item.TemperatureCelsio = tempCelsio;
			item.SetTemperatureToRenderer();
		}
	}

	private void _E000()
	{
		this.m__E000.Clear();
		AbstractSkin[] lods = _lods;
		foreach (AbstractSkin abstractSkin in lods)
		{
			HotObject hotObject = abstractSkin.gameObject.GetComponent<HotObject>();
			if (hotObject == null)
			{
				hotObject = abstractSkin.gameObject.AddComponent<HotObject>();
				hotObject.IsApplyAllMaterials = true;
				hotObject.Temperature = new Vector3(0f, 1f, 2.5f);
			}
			hotObject.TemperatureCelsio = 36.6f;
			hotObject.SetTemperatureToRenderer();
			this.m__E000.Add(hotObject);
		}
	}
}
