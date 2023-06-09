using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace EFT.InputSystem;

[Serializable]
public sealed class InputSource
{
	private static readonly KeyCode[] ValidKeyCodes = _E3A5<KeyCode>.Values.Except(new KeyCode[3]
	{
		KeyCode.F13,
		KeyCode.F14,
		KeyCode.F15
	}).ToArray();

	private static readonly string[] ValidAxes = new string[1] { _ED3E._E000(168949) };

	private static readonly Dictionary<EGameKey, List<KeyCode>> ForbiddenForAxisWithDefaultKey = new Dictionary<EGameKey, List<KeyCode>> { 
	{
		EGameKey.Shoot,
		new List<KeyCode> { KeyCode.Mouse0 }
	} };

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public bool isAxis;

	public List<KeyCode> keyCode = new List<KeyCode>();

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public string axisName;

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public bool positiveAxis;

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public float deadZone;

	[DefaultValue(1f)]
	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public float sensitivity = 1f;

	[JsonIgnore]
	public bool IsEmpty
	{
		get
		{
			if (keyCode.Count == 0)
			{
				return string.IsNullOrEmpty(axisName);
			}
			return false;
		}
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!string.IsNullOrEmpty(axisName))
		{
			stringBuilder.Append(_ED3E._E000(147706));
			stringBuilder.Append(axisName);
			stringBuilder.Append(positiveAxis ? _ED3E._E000(29692) : _ED3E._E000(29690));
			stringBuilder.Append(_ED3E._E000(147706));
		}
		for (int i = 0; i < keyCode.Count; i++)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(_ED3E._E000(91195));
			}
			stringBuilder.Append(_ED3E._E000(147706));
			stringBuilder.Append(_E7DA.GetKeyNameAlias(keyCode[i]));
			stringBuilder.Append(_ED3E._E000(147706));
		}
		return stringBuilder.ToString();
	}

	public InputSource Clone()
	{
		return new InputSource
		{
			isAxis = isAxis,
			keyCode = new List<KeyCode>(keyCode),
			axisName = axisName,
			positiveAxis = positiveAxis,
			deadZone = deadZone,
			sensitivity = sensitivity
		};
	}

	public void AddKey(KeyCode key)
	{
		for (int i = 0; i < keyCode.Count; i++)
		{
			if (key <= keyCode[i])
			{
				keyCode.Insert(i, key);
				return;
			}
		}
		keyCode.Add(key);
	}

	public bool Check(EGameKey gameKey)
	{
		if (ForbiddenForAxisWithDefaultKey.TryGetValue(gameKey, out var value) && (isAxis || !string.IsNullOrEmpty(axisName)))
		{
			isAxis = false;
			axisName = null;
			keyCode = value;
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is InputSource inputSource))
		{
			return false;
		}
		if (isAxis != inputSource.isAxis || ((!string.IsNullOrEmpty(axisName) || !string.IsNullOrEmpty(inputSource.axisName)) && !string.Equals(axisName, inputSource.axisName)) || positiveAxis != inputSource.positiveAxis || !(Math.Abs(deadZone - inputSource.deadZone) <= float.Epsilon) || keyCode.Count != inputSource.keyCode.Count)
		{
			return false;
		}
		for (int i = 0; i < keyCode.Count; i++)
		{
			if (keyCode[i] != inputSource.keyCode[i])
			{
				return false;
			}
		}
		return true;
	}

	public static EKeyPress Listen(ref InputSource inputSource, EGameKey gameKey)
	{
		KeyCode[] validKeyCodes = ValidKeyCodes;
		foreach (KeyCode keyCode in validKeyCodes)
		{
			if (Input.GetKey(keyCode))
			{
				if (!inputSource.keyCode.Contains(keyCode))
				{
					inputSource.isAxis = false;
					inputSource.AddKey(keyCode);
					return EKeyPress.Down;
				}
			}
			else if (inputSource.keyCode.Contains(keyCode))
			{
				return EKeyPress.Up;
			}
		}
		if (ForbiddenForAxisWithDefaultKey.ContainsKey(gameKey))
		{
			return EKeyPress.None;
		}
		string[] validAxes = ValidAxes;
		foreach (string text in validAxes)
		{
			float axisRaw = Input.GetAxisRaw(text);
			if (axisRaw >= 0.5f || axisRaw <= -0.5f)
			{
				inputSource.isAxis = true;
				inputSource.axisName = text;
				inputSource.positiveAxis = axisRaw > 0f;
				return EKeyPress.Up;
			}
		}
		return EKeyPress.None;
	}
}
