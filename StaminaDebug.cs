using EFT;
using UnityEngine;

public class StaminaDebug : MonoBehaviour
{
	public _E338 Physical;

	private GUIStyle m__E000;

	private Texture2D m__E001;

	private Player m__E002;

	private _E388 _E003 = new _E388(5);

	private int _E004;

	private Vector3 _E005;

	public void SetDebugObject(Player toDebug)
	{
		this.m__E002 = toDebug;
		Physical = toDebug.Physical;
		_E000();
	}

	private void _E000()
	{
		if (this.m__E000 == null)
		{
			this.m__E000 = new GUIStyle
			{
				alignment = TextAnchor.UpperLeft
			};
			this.m__E001 = _E002(2, 2, new Color(0.2f, 0.2f, 0.3f, 0.9f));
			this.m__E000.normal.background = this.m__E001;
			this.m__E000.normal.textColor = Color.white;
		}
	}

	private void OnGUI()
	{
		if (this.m__E002 != null)
		{
			GUI.Box(new Rect(10f, 10f, 220f, 120f), _E001(Physical.Stamina, _ED3E._E000(58985)), this.m__E000);
			GUI.Box(new Rect(230f, 10f, 220f, 120f), _E001(Physical.HandsStamina, _ED3E._E000(58977)), this.m__E000);
			GUI.Box(new Rect(450f, 10f, 220f, 120f), _E001(Physical.Oxygen, _ED3E._E000(59039)), this.m__E000);
			GUI.Box(new Rect(10f, 140f, 220f, 80f), string.Format(_ED3E._E000(59030), this.m__E002.Profile.Nickname, this.m__E002.Physical.PreviousWeight, this.m__E002.Physical.Fatigue, Mathf.Round(_E003.Avarage * 10f) / 10f), this.m__E000);
			GUI.Box(new Rect(230f, 140f, 220f, 80f), string.Format(_ED3E._E000(59042), this.m__E002.Physical.Inertia), this.m__E000);
		}
		if (GUI.Button(new Rect(620f, 10f, 50f, 20f), _ED3E._E000(3113), this.m__E000))
		{
			Physical = null;
			Object.Destroy(this);
		}
		if (GUI.Button(new Rect(620f, 30f, 50f, 20f), _ED3E._E000(59095), this.m__E000))
		{
			Player[] array = Object.FindObjectsOfType<Player>();
			if (array.Length >= 1)
			{
				SetDebugObject(array[_E004 % array.Length]);
				_E004++;
			}
		}
	}

	private void Update()
	{
		if (this.m__E002 != null)
		{
			_E003.AddValue(Vector3.Distance(this.m__E002.Transform.position, _E005) / Time.deltaTime);
			_E005 = this.m__E002.Transform.position;
		}
	}

	private string _E001(_E337 parameter, string title)
	{
		string text = string.Format(_ED3E._E000(59086), title, parameter.Current, parameter.TotalCapacity.Value);
		foreach (_E336._E000 consumption in parameter.Consumptions)
		{
			text += string.Format(_ED3E._E000(59129), consumption.Delta.Value, consumption.ConsumptionType);
		}
		return text + _ED3E._E000(59114) + ((Time.time > parameter.DisableRestoration) ? parameter.SelfRestoration.Value.ToString(_ED3E._E000(56089)) : (_ED3E._E000(59161) + (parameter.DisableRestoration - Time.time).ToString(_ED3E._E000(56089))));
	}

	private Texture2D _E002(int width, int height, Color col)
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
		Object.DestroyImmediate(this.m__E001);
	}
}
