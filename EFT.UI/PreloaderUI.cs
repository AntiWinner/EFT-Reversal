using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using Diz.Utils;
using EFT.Hideout;
using EFT.InputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class PreloaderUI : MonoBehaviourSingleton<PreloaderUI>
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public PreloaderUI _003C_003E4__this;

		public float step;

		public Action _003C_003E9__1;

		internal void _E000()
		{
			if (_003C_003E4__this.m__E004 != null)
			{
				_003C_003E4__this.StopCoroutine(_003C_003E4__this.m__E004);
				_003C_003E4__this.m__E004 = null;
			}
			_003C_003E4__this.m__E004 = _003C_003E4__this.StartCoroutine(_003C_003E4__this._E004(0f, step, delegate
			{
				_003C_003E4__this._overlapBlackImage.gameObject.SetActive(value: false);
			}));
		}

		internal void _E001()
		{
			_003C_003E4__this._overlapBlackImage.gameObject.SetActive(value: false);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public ErrorScreen errorScreen;

		public PreloaderUI _003C_003E4__this;

		public Action acceptCallback;

		public Action endTimeCallback;

		internal void _E000(Action callback)
		{
			callback?.Invoke();
			UnityEngine.Object.Destroy(errorScreen.gameObject);
			_003C_003E4__this._criticalErrorScreenContainer.RemoveChildNode(errorScreen);
		}

		internal void _E001()
		{
			_E000(acceptCallback);
		}

		internal void _E002()
		{
			_E000(endTimeCallback);
		}
	}

	private const float m__E000 = 2f;

	private const float m__E001 = 2f;

	private const string m__E002 = ".png";

	private static readonly string m__E003 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), _ED3E._E000(191834), _ED3E._E000(96795));

	[SerializeField]
	private LocalizedText _alphaVersionLabel;

	[SerializeField]
	private RectTransform _childrenContainer;

	[SerializeField]
	private Image _overlapBlackImage;

	[SerializeField]
	private RaidStartIntro _raidStartIntro;

	[SerializeField]
	private TMP_FPSCounter _fpsCounter;

	[SerializeField]
	private LoadingScreen _loadingScreen;

	[SerializeField]
	private GameObject _rttObject;

	[SerializeField]
	private TextMeshProUGUI _rttLabel;

	[SerializeField]
	private GameObject _lossObject;

	[SerializeField]
	private TextMeshProUGUI _lossLabel;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private ItemUiContext _UIContext;

	[SerializeField]
	private GameObject _bottomPanelBackground;

	[SerializeField]
	private ErrorScreen ErrorScreen;

	[SerializeField]
	private ErrorScreen _criticalErrorScreenTemplate;

	[SerializeField]
	private EmptyInputNode _criticalErrorScreenContainer;

	[SerializeField]
	private BattleUIMalfunctionGlow _malfunctionGlow;

	public Dictionary<KeyCode, string> Binds = new Dictionary<KeyCode, string>();

	public ClientWatermark ClientWatermark;

	public ConsoleScreen Console;

	public UnknownErrorScreen UnknownErrorScreen;

	public NotifierView NotifierView;

	public MenuTaskBar MenuTaskBar;

	private Coroutine m__E004;

	private double m__E005;

	private double m__E006;

	private float _E007;

	private float _E008;

	private string _E009;

	private string _E00A;

	private string _E00B;

	private bool _E00C;

	private double _E00D = 100.0;

	private int _E00E;

	private List<_EC40> _E00F = new List<_EC40>();

	private string _E010;

	private int _E011;

	public InputNode ErrorScreenInputNode => _criticalErrorScreenContainer;

	public BattleUIMalfunctionGlow MalfunctionGlow => _malfunctionGlow;

	public LoadingScreen LoadingScreen => _loadingScreen;

	public TMP_FPSCounter FPSCounter => _fpsCounter;

	public bool RaidInfoVisibility
	{
		set
		{
			_raidStartIntro.AlphaMultiplier = (value ? 1f : 0f);
		}
	}

	public bool CanShowErrorScreen => !UnknownErrorScreen.isActiveAndEnabled;

	public override void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		ItemUiContext.Init(_UIContext);
		Console.InitConsole();
		if (ErrorScreen != null)
		{
			_criticalErrorScreenContainer.AddChildNode(ErrorScreen);
			ErrorScreen.HideGameObject();
		}
		_alphaVersionLabel.SetFormatValues(_E5E9.Current.Major);
		_E009 = _alphaVersionLabel.FormattedText;
		base.Awake();
	}

	private void Update()
	{
		if (!Console.gameObject.activeSelf)
		{
			foreach (var (key, input) in Binds)
			{
				if (Input.GetKeyDown(key))
				{
					Console.TryCommand(input);
				}
			}
		}
		_E002();
		_E003();
	}

	public void InitConsole(Profile profile)
	{
		Console.OnProfileReceive(profile);
	}

	public void SetCanvasAsParent(GameObject @object)
	{
		@object.transform.SetParent(_childrenContainer);
	}

	public void ShowRaidStartInfo(long raidNumber, DateTime registrationDate, _E629 locationTime, EPlayerSide side, string playerName, string location)
	{
		SetBlackImageAlpha(1f);
		FadeBlackScreen(0.5f, -0.3f);
		_raidStartIntro.ShowIntro(raidNumber, registrationDate, locationTime, side, playerName, location);
	}

	public void StartBlackScreenShow(float to, float step, Action callback)
	{
		_overlapBlackImage.gameObject.SetActive(value: true);
		if (this.m__E004 != null)
		{
			StopCoroutine(this.m__E004);
			this.m__E004 = null;
		}
		this.m__E004 = StartCoroutine(_E004(to, step, callback));
	}

	public void SetMenuChatBarVisibility(bool visible)
	{
		MenuTaskBar._E008(visible);
		_bottomPanelBackground.gameObject.SetActive(visible);
	}

	public void FadeBlackScreen(float pause, float step)
	{
		this.WaitSeconds(pause, delegate
		{
			if (this.m__E004 != null)
			{
				StopCoroutine(this.m__E004);
				this.m__E004 = null;
			}
			this.m__E004 = StartCoroutine(_E004(0f, step, delegate
			{
				_overlapBlackImage.gameObject.SetActive(value: false);
			}));
		});
	}

	public void SetBlackImageAlpha(float alpha)
	{
		if (alpha >= 1f)
		{
			_overlapBlackImage.gameObject.SetActive(value: true);
		}
		_overlapBlackImage.color = new Color(0f, 0f, 0f, alpha);
	}

	public void SetLoaderStatus(bool status)
	{
		_loader.SetActive(status);
	}

	public void SetStreamMode(bool status)
	{
		_E00C = status;
		_rttObject.SetActive(value: false);
		_lossObject.SetActive(value: false);
	}

	public void MakeScreenshot()
	{
		if (!Directory.Exists(PreloaderUI.m__E003))
		{
			Directory.CreateDirectory(PreloaderUI.m__E003);
		}
		string text = Path.Combine(PreloaderUI.m__E003, _E5AD.Now.ToString(_ED3E._E000(248098)));
		Camera main = Camera.main;
		if (main != null)
		{
			Transform transform = main.gameObject.transform;
			text += string.Format(_ED3E._E000(248140), transform.position, transform.localRotation).Replace(_ED3E._E000(27312), "").Replace(_ED3E._E000(27308), "");
		}
		TOD_Sky instance = TOD_Sky.Instance;
		if (instance != null)
		{
			text += string.Format(_ED3E._E000(248133), instance.Cycle.Hour);
		}
		string text2 = _E000(text);
		Debug.Log(_ED3E._E000(248191) + text2);
		ScreenCapture.CaptureScreenshot(text2);
		_E857.DisplayMessageNotification(text);
	}

	private string _E000(string path)
	{
		int num = ((_E010 == path) ? (++_E011) : 0);
		_E010 = path;
		_E011 = num;
		do
		{
			path = string.Format(_ED3E._E000(248167), _E010, num, _ED3E._E000(45670));
			num = UnityEngine.Random.Range(100000, 999999);
		}
		while (File.Exists(path));
		return path;
	}

	public void ShowErrorScreen(string header, Exception exception, Action acceptCallback = null)
	{
		Debug.LogException(exception);
		string text = exception.Message ?? "";
		while (exception.InnerException != null)
		{
			exception = exception.InnerException;
			Debug.LogException(exception);
			text = text + _ED3E._E000(2540) + exception.Message;
		}
		ShowErrorScreen(header, text, acceptCallback);
	}

	public void ShowErrorScreen(string header, string message, Action acceptCallback = null)
	{
		if (!AsyncWorker.CheckIsMainThread())
		{
			Debug.LogError(_ED3E._E000(248212));
			return;
		}
		_E00F.Add(new _EC40
		{
			header = header,
			message = message,
			acceptCallback = acceptCallback
		});
		_E001().HandleExceptions();
	}

	public _EC7C ShowCriticalErrorScreen(string header, string message, ErrorScreen.EButtonType buttonType, float waitingTime, Action acceptCallback, Action endTimeCallback)
	{
		_E001 CS_0024_003C_003E8__locals0 = new _E001();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		CS_0024_003C_003E8__locals0.acceptCallback = acceptCallback;
		CS_0024_003C_003E8__locals0.endTimeCallback = endTimeCallback;
		if (!AsyncWorker.CheckIsMainThread())
		{
			Debug.LogError(_ED3E._E000(248212));
			return new _EC7C();
		}
		CS_0024_003C_003E8__locals0.errorScreen = UnityEngine.Object.Instantiate(_criticalErrorScreenTemplate, _criticalErrorScreenContainer.transform, worldPositionStays: false);
		_criticalErrorScreenContainer.AddChildNode(CS_0024_003C_003E8__locals0.errorScreen);
		return CS_0024_003C_003E8__locals0.errorScreen.Show(header, message, delegate
		{
			CS_0024_003C_003E8__locals0._E000(CS_0024_003C_003E8__locals0.acceptCallback);
		}, waitingTime, delegate
		{
			CS_0024_003C_003E8__locals0._E000(CS_0024_003C_003E8__locals0.endTimeCallback);
		}, buttonType);
	}

	public void CloseErrorScreen()
	{
		ErrorScreen.Close();
	}

	public void ClearErrorQueue()
	{
		_E00F.Clear();
		ErrorScreen.Close();
	}

	private async Task _E001()
	{
		if (_E00F.Count != 0 && !ErrorScreen.gameObject.activeSelf)
		{
			_EC40 obj = _E00F[0];
			_E00F.RemoveAt(0);
			await ErrorScreen.Show(obj.header, obj.message, obj.acceptCallback).WindowResult;
			_E001().HandleExceptions();
		}
	}

	private void _E002()
	{
		if (!_E00C)
		{
			double rTT = NetworkGameSession.RTT;
			if (rTT > _E00D)
			{
				_E007 = 0f;
			}
			_E007 += Time.deltaTime;
			_rttObject.SetActive(rTT > 0.0 && _E007 <= 2f && _E7A3.InRaid);
			if (_rttObject.activeSelf && Math.Abs(rTT - this.m__E005) > (double)Mathf.Epsilon)
			{
				this.m__E005 = rTT;
				_rttLabel.text = rTT.ToString(CultureInfo.InvariantCulture);
				_rttLabel.color = _E38D._E000.ToRTTColor(rTT);
			}
		}
	}

	private void _E003()
	{
		int lossPercent = NetworkGameSession.LossPercent;
		bool clientServerConnectionLags = NetworkGameSession.ClientServerConnectionLags;
		bool flag = lossPercent > _E00E || clientServerConnectionLags;
		bool flag2 = Math.Abs((double)lossPercent - this.m__E006) > (double)Mathf.Epsilon || clientServerConnectionLags;
		if (flag)
		{
			_E008 = 0f;
		}
		else
		{
			_E008 += Time.deltaTime;
		}
		_lossObject.SetActive((flag || _E008 < 2f) && _E7A3.InRaid);
		if (_lossObject.activeSelf && flag2)
		{
			this.m__E006 = lossPercent;
			_lossLabel.text = ((lossPercent > 0) ? (lossPercent.ToString(CultureInfo.InvariantCulture) + _ED3E._E000(149464)) : "");
			_lossLabel.color = _E38D._E000.ToLossColor(lossPercent);
		}
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (hasFocus)
		{
			_EC45.SetCursorLockMode(Cursor.visible, Singleton<_E7DE>.Instance.Graphics.Settings.DisplaySettings.Value.FullScreenMode);
		}
	}

	private IEnumerator _E004(float to, float step, Action callback = null)
	{
		float num = ((to >= 1f) ? 0f : 1f);
		while ((step > 0f) ? (_overlapBlackImage.color.a - to < 0.05f) : (_overlapBlackImage.color.a - to > 0.05f))
		{
			num += Time.deltaTime * step;
			SetBlackImageAlpha(num);
			yield return null;
		}
		SetBlackImageAlpha(to);
		callback?.Invoke();
	}

	public void SetSessionId(string id)
	{
		_E00A = id;
		_E005();
	}

	public void SetCommitsNotMatchText(string commitsNotMatchText)
	{
		_E00B = commitsNotMatchText;
		_E005();
	}

	private void _E005()
	{
		string text = _E009;
		if (!string.IsNullOrEmpty(_E00A))
		{
			text = text + _ED3E._E000(29351) + _E00A;
		}
		if (!string.IsNullOrEmpty(_E00B))
		{
			text = text + _ED3E._E000(29351) + _E00B;
		}
		_alphaVersionLabel.LocalizationKey = text;
	}

	public void SetWatermarkStatus(Profile profile, bool watermarkEnabled)
	{
		if (watermarkEnabled)
		{
			_E006(profile);
		}
		else
		{
			StopClientWatermark();
		}
	}

	private void _E006(Profile profile)
	{
		ClientWatermark.Show(profile);
	}

	public void StopClientWatermark()
	{
		if (ClientWatermark.isActiveAndEnabled)
		{
			ClientWatermark.Stop();
		}
	}

	public override void OnDestroy()
	{
		MenuTaskBar.Close();
		base.OnDestroy();
	}

	public void SetNetworkViewSettings(_E5B7._E002 clientSettingsNetworkStateView)
	{
		if (clientSettingsNetworkStateView != null)
		{
			_E00D = clientSettingsNetworkStateView.RttThreshold;
			_E00E = clientSettingsNetworkStateView.LossThreshold;
		}
	}
}
