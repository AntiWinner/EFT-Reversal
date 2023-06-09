using System;
using System.Collections.Generic;
using Audio.SpatialSystem;
using UnityEngine;

namespace Audio.DebugTools;

public sealed class SoundOcclusionDebugger : MonoBehaviour
{
	private const int m__E000 = 10;

	private const int m__E001 = 25;

	private readonly List<string> m__E002 = new List<string>();

	private _E48E m__E003;

	private string m__E004 = string.Empty;

	private string _E005 = string.Empty;

	private float _E006;

	private float _E007;

	private float _E008;

	private GUIStyle _E009;

	private GUIStyle _E00A;

	private readonly Color _E00B = new Color(0f, 0f, 0f, 0.8f);

	private void Awake()
	{
		_E009 = _E000(TextAnchor.UpperLeft);
		_E00A = _E000(TextAnchor.UpperRight);
	}

	public void UpdateInfo(_E48E sourceContainer, float obstructionEffect, float propagationEffect, float fullEffect)
	{
		if (sourceContainer.OcclusionTest == EOcclusionTest.Fast)
		{
			_E003(sourceContainer, obstructionEffect, propagationEffect, fullEffect);
		}
		this.m__E003 = sourceContainer;
		_E006 = obstructionEffect;
		_E007 = propagationEffect;
		this.m__E004 = (((object)sourceContainer.CurrentAudioRoom == null) ? "" : sourceContainer.CurrentAudioRoom.roomName);
		SpatialAudioRoom listenerCurrentRoom = MonoBehaviourSingleton<SpatialAudioSystem>.Instance.ListenerCurrentRoom;
		_E005 = (((object)listenerCurrentRoom == null) ? "" : listenerCurrentRoom.roomName);
		_E008 = fullEffect * 100f;
	}

	private GUIStyle _E000(TextAnchor anchor)
	{
		return new GUIStyle
		{
			alignment = anchor,
			normal = 
			{
				background = _E001(2, 3, _E00B),
				textColor = Color.white
			}
		};
	}

	private Texture2D _E001(int width, int height, Color col)
	{
		Color[] array = new Color[width * height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = col;
		}
		Texture2D texture2D = new Texture2D(width, height);
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	private void OnGUI()
	{
		if (!(EFTHardSettings.Instance == null) && EFTHardSettings.Instance.SoundDebugConsole)
		{
			GUI.Label(new Rect(10f, 20f, 900f, 20f), _ED3E._E000(73639) + _E005 + _ED3E._E000(73677) + this.m__E004, _E009);
			if (this.m__E003 != null)
			{
				_E004();
				_E002();
			}
		}
	}

	private void _E002()
	{
		if (this.m__E003.OcclusionTest != EOcclusionTest.Fast)
		{
			List<BetterSource> sources = this.m__E003.GetSources();
			for (int i = 0; i < sources.Count; i++)
			{
				BetterSource betterSource = sources[i];
				string text = betterSource.name;
				string text2 = Math.Round(betterSource.OcclusionVolumeFactor, 1, MidpointRounding.AwayFromZero).ToString();
				string text3 = Mathf.RoundToInt(betterSource.LowPassFilterFrequency).ToString();
				string text4 = _ED3E._E000(73717) + text + _ED3E._E000(73710) + text2 + _ED3E._E000(73697) + _E008 + _ED3E._E000(71697) + _E006 * 100f + _ED3E._E000(71738) + _E007 * 100f + _ED3E._E000(71715) + text3;
				GUI.Box(new Rect(10f, (float)(i * 25) + 40f, 900f, 20f), text4, _E009);
			}
		}
	}

	private void _E003(_E48E sourceContainer, float obstructionEffect, float propagationEffect, float fullEffect)
	{
		if (sourceContainer == null || sourceContainer.OcclusionTest != EOcclusionTest.Fast)
		{
			return;
		}
		List<BetterSource> sources = sourceContainer.GetSources();
		for (int i = 0; i < sources.Count; i++)
		{
			BetterSource betterSource = sources[i];
			string text = betterSource.name;
			string text2 = Math.Round(betterSource.OcclusionVolumeFactor, 1, MidpointRounding.AwayFromZero).ToString();
			string text3 = Mathf.RoundToInt(betterSource.LowPassFilterFrequency).ToString();
			string item = _ED3E._E000(73717) + text + _ED3E._E000(73710) + text2 + _ED3E._E000(73697) + fullEffect + _ED3E._E000(71697) + obstructionEffect * 100f + _ED3E._E000(71738) + propagationEffect * 100f + _ED3E._E000(71715) + text3;
			this.m__E002.Insert(0, item);
			if (this.m__E002.Count > 10)
			{
				this.m__E002.RemoveAt(this.m__E002.Count - 1);
			}
		}
	}

	private void _E004()
	{
		if (this.m__E002.Count != 0)
		{
			float x = (float)Screen.width - 910f;
			for (int i = 0; i < this.m__E002.Count; i++)
			{
				GUI.Box(new Rect(x, (float)(i * 25) + 40f, 900f, 20f), this.m__E002[i], _E00A);
			}
		}
	}

	private void OnDestroy()
	{
		this.m__E002.Clear();
		UnityEngine.Object.DestroyImmediate(_E00A.normal.background);
		UnityEngine.Object.DestroyImmediate(_E009.normal.background);
	}
}
