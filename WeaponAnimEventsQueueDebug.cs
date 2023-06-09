using EFT;
using EFT.UI;
using UnityEngine;

public class WeaponAnimEventsQueueDebug : MonoBehaviour
{
	private GUIStyle m__E000;

	private GUIStyle m__E001;

	private GUIStyle _E002;

	private Texture2D _E003;

	private Player _E004;

	private float _E005;

	private float _E006;

	private float _E007;

	public void SetDebugObject(Player toDebug)
	{
		_E004 = toDebug;
		_E000();
	}

	private void OnGUI()
	{
		if (_E004 == null || MonoBehaviourSingleton<PreloaderUI>.Instance.Console.IsConsoleVisible)
		{
			return;
		}
		Player.FirearmController firearmController = _E004.HandsController as Player.FirearmController;
		if (firearmController == null || firearmController.AnimationEventsEmitter == null)
		{
			return;
		}
		int num = 0;
		foreach (_E571._E000 item in firearmController.AnimationEventsEmitter.EventsSequenceData.AnimationEventsDebugQueue)
		{
			num++;
			string animStateByNameHash = _E327.GetAnimStateByNameHash(item.StateNameShortHash);
			Rect position = new Rect(690f, 15 + num * 20, 320f, 20f);
			if (item.ConditionPassed)
			{
				GUI.Box(position, animStateByNameHash + _ED3E._E000(31756) + item.EventName, this.m__E000);
			}
			else
			{
				GUI.Box(position, animStateByNameHash + _ED3E._E000(31756) + item.EventName + _ED3E._E000(59144), this.m__E001);
			}
		}
	}

	private void _E000()
	{
		if (this.m__E000 == null)
		{
			this.m__E000 = new GUIStyle
			{
				alignment = TextAnchor.UpperLeft
			};
			_E003 = _E001(2, 2, new Color(0.2f, 0.2f, 0.3f, 0.9f));
			this.m__E000.normal.background = _E003;
			this.m__E000.normal.textColor = Color.white;
		}
		if (this.m__E001 == null)
		{
			this.m__E001 = new GUIStyle
			{
				alignment = TextAnchor.UpperLeft
			};
			this.m__E001.normal.background = _E003;
			this.m__E001.normal.textColor = Color.red;
		}
		if (_E002 == null)
		{
			_E002 = new GUIStyle
			{
				alignment = TextAnchor.UpperLeft
			};
			_E002.normal.background = _E003;
			_E002.normal.textColor = Color.green;
		}
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

	private void OnDestroy()
	{
		Object.DestroyImmediate(_E003);
	}
}
