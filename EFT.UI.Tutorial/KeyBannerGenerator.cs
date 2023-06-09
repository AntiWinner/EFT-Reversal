using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InputSystem;
using UnityEngine;

namespace EFT.UI.Tutorial;

public class KeyBannerGenerator : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public KeyCombination keyCombination;

		internal bool _E000(AxisGroup e)
		{
			return e.axisName == keyCombination.Axis.AxisPairName;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public EGameKey key;

		internal bool _E000(KeyGroup e)
		{
			return e.keyName == key;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public EGameKey secondKey;

		internal bool _E000(KeyGroup e)
		{
			return e.keyName == secondKey;
		}
	}

	public KeyBanner[] Banners;

	public KeyBindingBannerView KeyBindingBannerView;

	private readonly HashSet<KeyCode> m__E000 = new HashSet<KeyCode>
	{
		KeyCode.Mouse0,
		KeyCode.Mouse1,
		KeyCode.Mouse2
	};

	public Sprite KeyBig;

	public Sprite KeyComma;

	public Sprite KeyMouseBoth;

	public Sprite KeyMouseLeft;

	public Sprite KeyMouseMiddle;

	public Sprite KeyMouseRight;

	public Sprite KeyPlus;

	public Sprite KeySmall;

	public KeyBindingBannerView GetKeyBindingBanner(Transform container)
	{
		KeyBanner obj = Banners[UnityEngine.Random.Range(0, Banners.Length)];
		List<_EC82> list = new List<_EC82>();
		KeyCombination[] keys = obj.Keys;
		foreach (KeyCombination keyCombination in keys)
		{
			try
			{
				_EC82 item = _E001(keyCombination);
				list.Add(item);
			}
			catch (NullReferenceException)
			{
				Debug.LogError(_ED3E._E000(254208) + keyCombination.LocalizationKey + _ED3E._E000(11164));
			}
		}
		GameObject obj2 = UnityEngine.Object.Instantiate(KeyBindingBannerView.gameObject, container);
		obj2.SetActive(value: true);
		KeyBindingBannerView component = obj2.GetComponent<KeyBindingBannerView>();
		component._E000(list.ToArray());
		return component;
	}

	private _EC81 _E000(KeyCode keyCode, string axisName = "")
	{
		string text = _E7DA.GetKeyNameAlias(keyCode);
		Sprite sprite = _E003(keyCode, text, axisName);
		if (this.m__E000.Contains(keyCode))
		{
			text = string.Empty;
		}
		return new _EC81(sprite, text);
	}

	private _EC82 _E001(KeyCombination keyCombination)
	{
		_E7F2 settings = Singleton<_E7DE>.Instance.Control.Settings;
		List<_EC81> list = new List<_EC81>();
		bool flag = false;
		IEnumerable<KeyGroup> source = settings.UserKeyBindings.Value.Concat(_E7F2.ReservedKeys);
		for (int i = 0; i < keyCombination.Keys.Length; i++)
		{
			if (i != 0)
			{
				list.Add(new _EC81(KeyComma));
			}
			EGameKey key = keyCombination.Keys[i];
			KeyGroup keyGroup = source.FirstOrDefault((KeyGroup e) => e.keyName == key);
			if (keyGroup == null)
			{
				Debug.LogWarningFormat(_ED3E._E000(254245), key);
				continue;
			}
			if (keyGroup.variants == null || keyGroup.variants.All((InputSource keyVariant) => keyVariant?.IsEmpty ?? true))
			{
				Debug.LogWarningFormat(_ED3E._E000(254333), key);
				continue;
			}
			InputSource inputSource = keyGroup.variants.FirstOrDefault((InputSource keyVariant) => !keyVariant.IsEmpty);
			if (keyCombination.Keys.Length > 1)
			{
				string axisName = inputSource.axisName;
				if (axisName != null && axisName.Contains(_ED3E._E000(168949)) && i == 0)
				{
					EGameKey secondKey = keyCombination.Keys[i + 1];
					KeyGroup keyGroup2 = source.FirstOrDefault((KeyGroup e) => e.keyName == secondKey);
					if (keyGroup2 != null)
					{
						InputSource inputSource2 = keyGroup2.variants.FirstOrDefault((InputSource keyVariant) => !keyVariant.IsEmpty);
						if (inputSource2 != null)
						{
							if (inputSource2.axisName.Contains(_ED3E._E000(168949)))
							{
								flag = true;
							}
						}
						else
						{
							Debug.LogWarningFormat(_ED3E._E000(254333), key);
						}
					}
					else
					{
						Debug.LogWarningFormat(_ED3E._E000(254245), key);
					}
				}
			}
			if (inputSource.isAxis)
			{
				_EC81 item = _E000(KeyCode.None, inputSource.axisName);
				list.Add(item);
			}
			for (int j = 0; j < inputSource.keyCode.Count; j++)
			{
				if (j > 0)
				{
					_EC81 item2 = new _EC81(KeyPlus);
					list.Add(item2);
				}
				_EC81 item3 = _E000(inputSource.keyCode[j]);
				list.Add(item3);
				if (keyGroup.pressType == EPressType.DoubleClick)
				{
					_EC81 item4 = new _EC81(KeyPlus);
					list.Add(item4);
					list.Add(item3);
				}
			}
			if (flag)
			{
				break;
			}
		}
		List<AxisGroup> value = settings.UserAxisBindings.Value;
		if (keyCombination.Axis != null && (keyCombination.Axis.UseNegativeAxis || keyCombination.Axis.UsePositiveAxis))
		{
			AxisGroup axisGroup = value.FirstOrDefault((AxisGroup e) => e.axisName == keyCombination.Axis.AxisPairName);
			if (axisGroup != null)
			{
				if (keyCombination.Axis.UsePositiveAxis != keyCombination.Axis.UseNegativeAxis)
				{
					AxisGroup.AxisPair axisPair = axisGroup.pairs.FirstOrDefault();
					if (axisPair != null)
					{
						InputSource axisInput = axisPair.positive;
						if (keyCombination.Axis.UseNegativeAxis)
						{
							axisInput = axisPair.negative;
						}
						_E002(axisInput, list);
					}
					else
					{
						Debug.LogWarningFormat(_ED3E._E000(254367), keyCombination.Axis.AxisPairName);
					}
				}
				else
				{
					AxisGroup.AxisPair axisPair2 = axisGroup.pairs.FirstOrDefault();
					if (axisPair2 != null)
					{
						_E002(axisPair2.positive, list);
						_EC81 item5 = new _EC81(KeyComma);
						list.Add(item5);
						_E002(axisPair2.negative, list);
					}
					else
					{
						Debug.LogWarningFormat(_ED3E._E000(254367), keyCombination.Axis.AxisPairName);
					}
				}
			}
			else
			{
				Debug.LogWarningFormat(_ED3E._E000(254379), keyCombination.Axis.AxisPairName);
			}
		}
		string text = _E7AD._E010._E004(keyCombination.LocalizationKey);
		if (flag)
		{
			text = _E7AD._E010._E004(_ED3E._E000(254460)) + text;
		}
		return new _EC82(list.ToArray(), text);
	}

	private void _E002(InputSource axisInput, List<_EC81> keys)
	{
		if (!string.IsNullOrEmpty(axisInput.axisName))
		{
			_EC81 item = _E000(KeyCode.None, axisInput.axisName);
			keys.Add(item);
		}
		for (int i = 0; i < axisInput.keyCode.Count; i++)
		{
			if (i != 0 || !string.IsNullOrEmpty(axisInput.axisName))
			{
				_EC81 item2 = new _EC81(KeyPlus);
				keys.Add(item2);
			}
			_EC81 item3 = _E000(axisInput.keyCode[i]);
			keys.Add(item3);
		}
	}

	private Sprite _E003(KeyCode keyCode, string keyAlias, string axisName = "")
	{
		Sprite result = KeySmall;
		if (keyAlias.Length > 1)
		{
			result = KeyBig;
		}
		switch (keyCode)
		{
		case KeyCode.Mouse0:
			result = KeyMouseLeft;
			break;
		case KeyCode.Mouse1:
			result = KeyMouseRight;
			break;
		case KeyCode.Mouse2:
			result = KeyMouseMiddle;
			break;
		}
		if (!string.IsNullOrEmpty(axisName) && axisName.Contains(_ED3E._E000(168949)))
		{
			result = KeyMouseMiddle;
		}
		return result;
	}
}
