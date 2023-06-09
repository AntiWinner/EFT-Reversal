using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.Console.Core;
using EFT.InputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class ConsoleScreen : UIInputNode
{
	public const string CONSOLE_START_COMMANDS_SAVE_KEY = "ConsoleStartCommands";

	[SerializeField]
	private Slider _consoleSlider;

	[SerializeField]
	private TMP_InputField _enterCommandField;

	[SerializeField]
	private TextMeshProUGUI _timePanel;

	[SerializeField]
	private TMP_InputFieldNoScroll _logsPanel;

	[SerializeField]
	private ConsoleAutoCompletePanel _consoleAutoCompletePanel;

	public static readonly ConsoleProcessor Processor = new ConsoleProcessor();

	[CompilerGenerated]
	private static bool _E05C = true;

	[CompilerGenerated]
	private static bool _E05D = true;

	[CompilerGenerated]
	private static bool _E05E = true;

	private readonly LinkedList<string> _E05F = new LinkedList<string>();

	private readonly LinkedList<string> _E060 = new LinkedList<string>();

	private Dictionary<string, string> _E061;

	private int _E062;

	private bool _E063;

	private string _E064;

	private bool _E065;

	private const int _E066 = 60;

	private int _E067;

	private EMemberCategory _E068;

	private readonly List<string> _E069 = new List<string>();

	private Coroutine _E06A;

	private bool _E06B;

	private bool _E06C;

	public bool IsConsoleVisible => _E063;

	public static bool WarningVisibility
	{
		[CompilerGenerated]
		get
		{
			return _E05C;
		}
		[CompilerGenerated]
		set
		{
			_E05C = value;
		}
	}

	public static bool ErrorsVisibility
	{
		[CompilerGenerated]
		get
		{
			return _E05D;
		}
		[CompilerGenerated]
		set
		{
			_E05D = value;
		}
	}

	public static bool LogsVisibility
	{
		[CompilerGenerated]
		get
		{
			return _E05E;
		}
		[CompilerGenerated]
		set
		{
			_E05E = value;
		}
	}

	private int _E000
	{
		get
		{
			return _E062;
		}
		set
		{
			_E062 = value;
			_consoleSlider.value = _E062;
		}
	}

	private static Player _E001 => _E3AA.FindUnityObjectOfType<Player>();

	private void _E000()
	{
		Processor.RegisterCommandGroup<_E8D2>();
		Processor.RegisterCommand<_E8D4>();
		Processor.RegisterCommand<_E8D1>();
		Processor.RegisterCommand<_E8D1._E000>();
	}

	public static void ApplyStartCommands()
	{
		string @string = PlayerPrefs.GetString(_ED3E._E000(249981));
		if (@string != null)
		{
			string[] array = @string.Split('\n');
			foreach (string input in array)
			{
				MonoBehaviourSingleton<PreloaderUI>.Instance.Console.TryCommand(input);
			}
		}
	}

	public static void SwitchDebugLogEnabled()
	{
		_E372.DebugLogsEnabled(!_E372.IsDebugLogEnabled());
	}

	public static void SwitchStackTraceEnabled()
	{
		_E372.StackTraceEnabled(!_E372.IsStackTraceEnabled());
	}

	private void Awake()
	{
	}

	public void OnProfileReceive(Profile profile)
	{
		_E068 = profile.Info.MemberCategory;
		if (_E068.Is(EMemberCategory.Developer))
		{
			_E003();
		}
	}

	public void InitConsole()
	{
		base.gameObject.SetActive(value: true);
		_E001(isActive: false);
		if (_E06C)
		{
			return;
		}
		_E06C = true;
		_E061 = new Dictionary<string, string>
		{
			{
				_ED3E._E000(249962),
				_ED3E._E000(57821)
			},
			{
				_ED3E._E000(249959),
				_ED3E._E000(29694)
			}
		};
		Application.logMessageReceived += _E004;
		_enterCommandField.onEndEdit.AddListener(delegate(string txt)
		{
			if (_E063)
			{
				TryCommand(txt);
			}
		});
		_E000();
	}

	public static void Log(string statement)
	{
		MonoBehaviourSingleton<PreloaderUI>.Instance.Console._E005(statement, string.Empty, LogType.Log);
	}

	public static void LogWarning(string statement)
	{
		MonoBehaviourSingleton<PreloaderUI>.Instance.Console._E005(statement, string.Empty, LogType.Warning);
	}

	public static void LogFormat(string statement, params object[] args)
	{
		MonoBehaviourSingleton<PreloaderUI>.Instance.Console._E005(string.Format(statement, args), string.Empty, LogType.Log);
	}

	public static void LogError(string statement)
	{
		MonoBehaviourSingleton<PreloaderUI>.Instance.Console._E005(statement, string.Empty, LogType.Error);
	}

	public static void LogException(Exception exception)
	{
		MonoBehaviourSingleton<PreloaderUI>.Instance.Console._E005(exception.Message, exception.StackTrace, LogType.Error);
	}

	private void _E001(bool isActive)
	{
		_E063 = isActive;
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = base.transform.GetChild(i).gameObject;
			if (gameObject != base.gameObject && gameObject != _consoleAutoCompletePanel.gameObject)
			{
				gameObject.SetActive(isActive);
			}
		}
	}

	private void OnEnable()
	{
		this._E000 = 0;
		_E008();
		_consoleSlider.onValueChanged.AddListener(_E002);
		_consoleAutoCompletePanel.OnSelect += _E00F;
	}

	private void _E002(float value)
	{
		this._E000 = (int)value;
		_E008();
	}

	public void Show()
	{
		if (this == null || base.gameObject == null || _E063 || _E06A != null)
		{
			return;
		}
		UIEventSystem.Instance.SetTemporaryStatus(isActive: true);
		base.gameObject.SetActive(value: true);
		_E001(isActive: true);
		base.transform.SetAsLastSibling();
		_logsPanel.Show(_E010);
		if (_enterCommandField.isFocused)
		{
			return;
		}
		_E06A = this.WaitOneFrame(delegate
		{
			if (this == null || !base.gameObject.activeSelf)
			{
				_E06A = null;
			}
			else
			{
				if (_E009(_enterCommandField.text))
				{
					_enterCommandField.text = "";
				}
				_enterCommandField.ActivateInputField();
				_enterCommandField.Select();
				_E008();
				_E06A = null;
			}
		});
		OnEnable();
	}

	private void _E003()
	{
		Processor.RegisterCommand(_ED3E._E000(250015), delegate(bool isEnabled)
		{
			_E06B = isEnabled;
		});
		Processor.RegisterCommand(_ED3E._E000(250002), delegate
		{
			_E06B = true;
		});
	}

	private void _E004(string condition, string stacktrace, LogType type)
	{
		if (type == LogType.Log || type == LogType.Warning)
		{
			return;
		}
		switch ((int)type)
		{
		case 2:
			if (WarningVisibility)
			{
				goto default;
			}
			break;
		case 0:
			if (ErrorsVisibility)
			{
				goto default;
			}
			break;
		case 4:
			if (ErrorsVisibility)
			{
				goto default;
			}
			break;
		case 1:
			if (ErrorsVisibility)
			{
				goto default;
			}
			break;
		case 3:
			if (LogsVisibility)
			{
				goto default;
			}
			break;
		default:
			_E005(condition, stacktrace, type);
			break;
		}
	}

	private void _E005(string condition, string stacktrace, LogType type)
	{
		string text = null;
		if (condition.Length > 250)
		{
			condition = condition.Substring(0, 250) + _ED3E._E000(59476);
		}
		foreach (KeyValuePair<string, string> item in _E061)
		{
			_E39D.Deconstruct(item, out var key, out var value);
			string text2 = key;
			string color = value;
			int num = condition.IndexOf(text2, StringComparison.OrdinalIgnoreCase);
			if (num != -1)
			{
				condition = condition.Remove(num, text2.Length);
				text = _E006(color);
				break;
			}
		}
		string text3 = string.Format(_ED3E._E000(249999), type, condition);
		if (stacktrace != null && (type == LogType.Exception || type == LogType.Error || type == LogType.Assert))
		{
			text3 = stacktrace.Split('\n').Aggregate(text3, (string current, string item) => item + _ED3E._E000(2540) + current);
		}
		bool flag = false;
		if (text == null)
		{
			switch (type)
			{
			case LogType.Warning:
				text = _E006(_ED3E._E000(60646));
				break;
			case LogType.Error:
			case LogType.Assert:
			case LogType.Exception:
				text = _E006(_ED3E._E000(29694));
				if (ErrorsVisibility && !_E06B)
				{
					flag = true;
				}
				break;
			default:
				text = _E006(_ED3E._E000(60648));
				break;
			}
		}
		string[] array = text3.Split('\n');
		foreach (string log in array)
		{
			_E007(log, text);
		}
		if (flag)
		{
			PullUpConsole();
		}
	}

	public void PullUpConsole()
	{
		_ = _E068;
		if (_E068.Is(EMemberCategory.Developer))
		{
			Show();
		}
	}

	private static string _E006(string color)
	{
		return _ED3E._E000(47183) + color + _ED3E._E000(59465) + _E5AD.Now.ToString(_ED3E._E000(249987)) + _ED3E._E000(250038);
	}

	private void _E007(string log, string statement)
	{
		if (!(this == null) && !(base.gameObject == null))
		{
			_E05F.AddFirst(log);
			_E060.AddFirst(statement);
			_E067 = 0;
			if (this._E000 > 0)
			{
				this._E000 += log.Split('\n').Length;
			}
			if (_logsPanel.isActiveAndEnabled)
			{
				_E008();
			}
		}
	}

	private void _E008()
	{
		_E062 = Mathf.Clamp(this._E000, 0, Mathf.Max(0, _E05F.Count));
		_timePanel.SetText(string.Join(_ED3E._E000(2540), _E060.Skip(this._E000).Take(60).ToArray()).ConvertToMonospace());
		_logsPanel.text = string.Join(_ED3E._E000(2540), _E05F.Skip(this._E000).Take(60).ToArray());
		_consoleSlider.maxValue = _E05F.Count;
	}

	public void TryCommand(IConsoleCommand input)
	{
		string input2 = Processor.PrintCommand(input);
		TryCommand(input2);
	}

	public void TryCommand(string input)
	{
		input = input.Trim();
		if (string.IsNullOrEmpty(input))
		{
			return;
		}
		bool flag = false;
		if (input.StartsWith(_ED3E._E000(55115)))
		{
			flag = true;
			input = input.Remove(0, 1);
		}
		(IConsoleCommand, ConstructorFindResult)[] array = Processor.Parse(input);
		for (int i = 0; i < array.Length; i++)
		{
			var (consoleCommand, constructorFindResult) = array[i];
			if (constructorFindResult.Exception != null && !_E009(input))
			{
				Exception exception = constructorFindResult.Exception;
				if (exception != null && exception is CommandSearchException)
				{
					_E005(constructorFindResult.Exception.Message, null, LogType.Error);
				}
				else
				{
					Debug.LogException(constructorFindResult.Exception);
				}
			}
			else if (constructorFindResult.Exception == null && consoleCommand != null)
			{
				if (flag)
				{
					Processor.ExecuteImmediately(new IConsoleCommand[1] { consoleCommand });
				}
				else
				{
					Processor.Execute(consoleCommand);
				}
			}
		}
		_E069.Add(input);
		_enterCommandField.text = "";
		_enterCommandField.ActivateInputField();
		_enterCommandField.Select();
		_E00E();
	}

	public override void Close()
	{
		this.WaitForEndOfFrame(_E011);
	}

	public void Clear()
	{
		_E05F.Clear();
		_E060.Clear();
		this._E000 = 0;
		_E008();
	}

	private bool _E009(string input)
	{
		if (!(input == _ED3E._E000(189387)) && !(input == _ED3E._E000(47833)))
		{
			return input == _ED3E._E000(47674);
		}
		return true;
	}

	private void _E00A()
	{
		if (_E069.Count > 0)
		{
			_E067 = Mathf.Clamp(_E067, 1, _E069.Count);
			_enterCommandField.text = (_E064 = _E069[_E069.Count - _E067]);
			_enterCommandField.caretPosition = _enterCommandField.text.Length;
		}
	}

	public void LateUpdate()
	{
		if (_E063)
		{
			_E00C();
			_E00B();
		}
	}

	private void _E00B()
	{
		if (!_E065 && _E069.Count > 0)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				_E067--;
				_E00A();
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				_E067++;
				_E00A();
			}
		}
	}

	private void _E00C()
	{
		string text = _enterCommandField.text;
		if (text.Length <= 0)
		{
			_E00E();
		}
		else if (!(_E064 == text))
		{
			_E00D(text);
		}
	}

	private void _E00D(string input)
	{
		_E064 = input;
		AutoCompleteItem[] array = Processor.CalculateAutoCompleteVariants(_E064);
		_E065 = array.Length != 0;
		if (_E065)
		{
			_consoleAutoCompletePanel.UpdateList(array);
		}
		_consoleAutoCompletePanel.gameObject.SetActive(_E065);
	}

	private void _E00E()
	{
		if (_E065)
		{
			_E065 = false;
			_consoleAutoCompletePanel.gameObject.SetActive(_E065);
			_E064 = _enterCommandField.text;
		}
	}

	private void _E00F(AutoCompleteItem item)
	{
		if (_E065)
		{
			_enterCommandField.text = item.Result;
			if (!_enterCommandField.text.EndsWith(ConsoleSerializer.COMMAND_NAME_SEPARATOR.ToString()))
			{
				_E00E();
			}
			int num = _enterCommandField.text.IndexOf(_ED3E._E000(18502), _enterCommandField.caretPosition, StringComparison.Ordinal);
			_enterCommandField.caretPosition = ((num > -1) ? (num + 1) : _enterCommandField.text.Length);
			_enterCommandField.ActivateInputField();
			_enterCommandField.Select();
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape) && _E063)
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
			_E011();
			return ETranslateResult.BlockAll;
		}
		if (command.IsCommand(ECommand.ShowConsole))
		{
			if (_E063)
			{
				_E011();
				return ETranslateResult.BlockAll;
			}
			Show();
		}
		if (command.IsCommand(ECommand.MakeScreenshot))
		{
			return ETranslateResult.Ignore;
		}
		if (!_E063)
		{
			return ETranslateResult.Ignore;
		}
		return ETranslateResult.BlockAll;
	}

	protected override void TranslateAxes(ref float[] axes)
	{
		if (_E063)
		{
			axes = null;
		}
	}

	protected override ECursorResult ShouldLockCursor()
	{
		if (!_E063)
		{
			return ECursorResult.Ignore;
		}
		return ECursorResult.ShowCursor;
	}

	private void _E010(PointerEventData eventData)
	{
		this._E000 += ((eventData.scrollDelta.y > 0f) ? (-2) : 2);
		_consoleSlider.value = this._E000;
		_E008();
	}

	private void _E011()
	{
		_E00E();
		_E001(isActive: false);
		UIEventSystem.Instance.SetTemporaryStatus(isActive: false);
		OnDisable();
	}

	private void OnDisable()
	{
		_consoleSlider.onValueChanged.RemoveListener(_E002);
		_consoleAutoCompletePanel.OnSelect -= _E00F;
		_E06A = null;
	}

	private void OnDestroy()
	{
		Application.logMessageReceived -= _E004;
		_E06C = false;
	}

	[CompilerGenerated]
	private void _E012(string txt)
	{
		if (_E063)
		{
			TryCommand(txt);
		}
	}

	[CompilerGenerated]
	private void _E013()
	{
		if (this == null || !base.gameObject.activeSelf)
		{
			_E06A = null;
			return;
		}
		if (_E009(_enterCommandField.text))
		{
			_enterCommandField.text = "";
		}
		_enterCommandField.ActivateInputField();
		_enterCommandField.Select();
		_E008();
		_E06A = null;
	}

	[CompilerGenerated]
	private void _E014(bool isEnabled)
	{
		_E06B = isEnabled;
	}

	[CompilerGenerated]
	private void _E015()
	{
		_E06B = true;
	}
}
