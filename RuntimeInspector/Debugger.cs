using System;
using System.Reflection;
using EFT.Weather;
using UnityEngine;

namespace RuntimeInspector;

public class Debugger : MonoBehaviour
{
	public Color BackGroundColor;

	public Color TextColor;

	public Color TypeTextColor;

	public Color SelectedTextColor;

	public Color SelectedBackGroundColor;

	public Texture2D ClassTex;

	public Texture2D FieldTex;

	public Texture2D MethodTex;

	private Rect m__E000;

	private Rect m__E001;

	private Rect m__E002;

	private static Vector2 m__E003;

	public static Texture2D classTex;

	public static Texture2D fieldTex;

	public static Texture2D methodTex;

	public static GUIStyle normal;

	public static GUIStyle selected;

	public static GUIStyle ClassTypeStyle;

	public static GUIStyle ClassTypeStyle2;

	public static GUIStyle ClassTypeStyleSelected;

	public static GUIStyle ColorField;

	private static GUIStyle _E004;

	private static bool _E005;

	public static _E43E Info;

	public static _E436 Explorer;

	private static bool _E006;

	private static Component[] _E007;

	private void Awake()
	{
		_E005 = false;
		_E002();
		Info = new _E43E();
		this.m__E000 = new Rect(Screen.width - 600, 0f, 600f, Screen.height);
		float num = this.m__E000.width - 300f;
		this.m__E002 = new Rect(num, 24f, 300f, this.m__E000.height);
		this.m__E001 = new Rect(0f, 17f, num, this.m__E000.height);
	}

	private object _E000(string parameters)
	{
		parameters = parameters.Trim();
		if (parameters == "")
		{
			return null;
		}
		string[] array = parameters.Split('.');
		if (array.Length == 0)
		{
			return null;
		}
		Debug.Log(parameters);
		Type[] types = typeof(WeatherController).Assembly.GetTypes();
		for (int i = 0; i < types.Length; i++)
		{
			Type type = types[i];
			if (!type.IsClass || type.Name != array[0])
			{
				continue;
			}
			object obj = null;
			for (int j = 1; j < array.Length; j++)
			{
				if (obj is Component && array[j].Substring(0, _ED3E._E000(94043).Length) == _ED3E._E000(94043))
				{
					string type2 = array[j].Substring(_ED3E._E000(94024).Length, array[j].Length - _ED3E._E000(94078).Length);
					obj = ((Component)obj).GetComponent(type2);
					if (obj != null)
					{
						type = obj.GetType();
						continue;
					}
					Debug.Log(array[j] + _ED3E._E000(94063));
				}
				FieldInfo field = type.GetField(array[j]);
				if (field != null)
				{
					type = field.FieldType;
					obj = field.GetValue(obj);
					continue;
				}
				PropertyInfo property = type.GetProperty(array[j]);
				if (property != null)
				{
					type = property.PropertyType;
					obj = property.GetValue(obj, null);
					continue;
				}
				return null;
			}
			return obj;
		}
		return null;
	}

	private void _E001()
	{
		_E006 = true;
	}

	private void _E002()
	{
		classTex = ClassTex;
		fieldTex = FieldTex;
		methodTex = MethodTex;
		normal = new GUIStyle();
		normal.stretchWidth = false;
		normal.normal.textColor = TextColor;
		ClassTypeStyle = new GUIStyle();
		ClassTypeStyle.normal.textColor = TypeTextColor;
		ClassTypeStyle2 = new GUIStyle(ClassTypeStyle);
		ClassTypeStyle2.stretchWidth = false;
		selected = new GUIStyle(normal);
		selected.normal.textColor = SelectedTextColor;
		selected.normal.background = _E003(SelectedBackGroundColor, 4, 4);
		ClassTypeStyleSelected = new GUIStyle(selected);
		ClassTypeStyleSelected.stretchWidth = true;
		_E004 = new GUIStyle();
		_E004.normal.background = _E003(BackGroundColor, 4, 4);
		ColorField = new GUIStyle();
		ColorField.normal.background = _E003(Color.white, 4, 4);
	}

	private void OnValidate()
	{
		_E002();
	}

	public static bool LastRectClick()
	{
		if (!_E005)
		{
			return false;
		}
		return GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition);
	}

	private void Update()
	{
		if (_E006)
		{
			Camera camera = _E8A8.Instance.Camera;
			if (!(camera == null) && Input.GetKey(KeyCode.F5) && Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out var hitInfo))
			{
				_E007 = hitInfo.collider.gameObject.GetComponents<Component>();
			}
		}
	}

	private void OnGUI()
	{
		if (_E8A8.Instance.Camera != null && _E007 != null && _E007.Length != 0)
		{
			Vector2 vector = _E8A8.Instance.Camera.WorldToScreenPoint(_E007[0].transform.position);
			vector.x = Mathf.Clamp(vector.x, 0f, Screen.width - 200);
			vector.y = Mathf.Clamp(vector.y, 0f, Screen.height - 200);
			GUILayout.BeginArea(new Rect(vector.x, vector.y, 800f, 2000f));
			GUILayout.Label(_E007[0].gameObject.name);
			Component[] array = _E007;
			foreach (Component component in array)
			{
				if (!(component == null) && GUILayout.Button(component.GetType().Name))
				{
					Open(component);
					_E007 = null;
					_E006 = false;
				}
			}
			GUILayout.EndArea();
		}
		if (Explorer != null)
		{
			GUI.Box(this.m__E000, "", _E004);
			GUILayout.BeginArea(this.m__E000);
			_E005 = Event.current.type == EventType.MouseDown;
			Debugger.m__E003 = GUILayout.BeginScrollView(Debugger.m__E003, GUILayout.ExpandWidth(expand: false));
			GUILayout.BeginHorizontal();
			Explorer.DrawHistory();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button(_ED3E._E000(94109), GUILayout.ExpandWidth(expand: false), GUILayout.Width(24f), GUILayout.Height(16f)))
			{
				Close();
			}
			GUILayout.EndHorizontal();
			GUI.BeginClip(this.m__E001);
			GUILayout.BeginArea(this.m__E001);
			Explorer.DrawFields();
			GUILayout.EndArea();
			GUI.EndClip();
			GUILayout.EndScrollView();
			GUILayout.BeginArea(this.m__E002);
			if (Info != null)
			{
				Info.Draw();
			}
			GUILayout.EndArea();
			GUILayout.EndArea();
		}
	}

	public static void Open(object obj)
	{
		if (obj == null)
		{
			Debug.Log(_ED3E._E000(94111));
		}
		else
		{
			Explorer = new _E436(obj);
		}
	}

	public static void Close()
	{
		Explorer = null;
		_E006 = false;
		_E007 = null;
	}

	private static Texture2D _E003(Color color, int width, int height)
	{
		Texture2D texture2D = new Texture2D(width, height);
		texture2D.name = _ED3E._E000(94132);
		int num = width * height;
		Color[] array = new Color[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = color;
		}
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}
}
