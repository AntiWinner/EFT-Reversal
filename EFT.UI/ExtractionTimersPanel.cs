using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using EFT.Interactive;
using EFT.UI.BattleTimer;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class ExtractionTimersPanel : UIElement
{
	private const string _E0CF = "00:00:00";

	private const string _E0D0 = "Buffer Zone Timer";

	private const float _E0D1 = 143f;

	private const int _E0D2 = 60;

	[SerializeField]
	private AnimationCurve _inCurve;

	[SerializeField]
	private float _animationSpeed = 1f;

	[SerializeField]
	private RectTransform _mainDescription;

	[SerializeField]
	private RectTransform _timersPanel;

	[SerializeField]
	private MainTimerPanel _mainTimerPanel;

	[SerializeField]
	private ExitTimerPanel _timerPanelTemplate;

	[SerializeField]
	private RectTransform _container;

	[SerializeField]
	private ExitTimerPanel _bufferZoneTimerPanelTemplate;

	[SerializeField]
	private RectTransform _mainContainer;

	private ExitTimerPanel _E0D3;

	private readonly Dictionary<string, ExitTimerPanel> _E0D4 = new Dictionary<string, ExitTimerPanel>();

	private StringBuilder _E0D5;

	private Coroutine _E0D6;

	private Coroutine _E0D7;

	private Coroutine _E0D8;

	private CanvasGroup _E0D9;

	private DateTime _E0DA;

	private EPlayerSide _E0DB;

	private bool _E0DC;

	private bool _E0DD;

	private bool _E0DE;

	private Action _E0DF;

	[CompilerGenerated]
	private string _E0E0;

	public string ProfileId
	{
		[CompilerGenerated]
		get
		{
			return _E0E0;
		}
		[CompilerGenerated]
		set
		{
			_E0E0 = value;
		}
	}

	private void Awake()
	{
		_timerPanelTemplate.gameObject.SetActive(value: false);
		_E0D5 = new StringBuilder(_ED3E._E000(253426).Length, _ED3E._E000(253426).Length);
		_E0D9 = base.gameObject.GetOrAddComponent<CanvasGroup>();
	}

	private void Update()
	{
		if (_E0DE)
		{
			int playerBufferZoneUsageTimeLeft = _EBEB.Instance.GetPlayerBufferZoneUsageTimeLeft(ProfileId);
			_E0DD = playerBufferZoneUsageTimeLeft <= 60;
			if (_E0DD && _E0D3 != null && _E0D3.transform.parent != _mainContainer && _E0DC)
			{
				_E0D3.transform.SetParent(_mainContainer);
				_E0D3.RectTransform().sizeDelta = new Vector2(_timersPanel.rect.width, _E0D3.RectTransform().rect.height);
				_E0D3.transform.SetSiblingIndex(0);
			}
			if (!_E0DC)
			{
				_E0D3.HideGameObject();
			}
			if (_E0DD && _E0DC)
			{
				_E0D3.ShowGameObject();
			}
		}
	}

	public void SetTime(DateTime dateTime, EPlayerSide side, float seconds, ExfiltrationPoint[] points)
	{
		_E0DA = dateTime;
		_E0DB = side;
		ShowGameObject();
		_mainTimerPanel.Show(_E0DA.AddSeconds(seconds), _E0D5, EMainTimerState.FindEp);
		for (int i = 0; i < points.Length; i++)
		{
			ExfiltrationPoint exfiltrationPoint = points[i];
			ExitTimerPanel exitTimerPanel = UnityEngine.Object.Instantiate(_timerPanelTemplate, _container);
			exitTimerPanel.Show(_E5AD.UtcNow.AddSeconds(exfiltrationPoint.Settings.MaxTime * 60f), side, i + 1, _E0D5, subscribe: true, ProfileId, exfiltrationPoint);
			_E0D4.Add(exfiltrationPoint.Settings.Name, exitTimerPanel);
		}
		_mainDescription.anchoredPosition = Vector3.zero;
		_timersPanel.anchoredPosition = Vector3.zero;
		_timersPanel.pivot = new Vector2(1f, 0f);
		_E0DE = _EBEB.Instance.IsBufferZoneExists;
		if (_E0DE)
		{
			_E0D3 = UnityEngine.Object.Instantiate(_bufferZoneTimerPanelTemplate, _container);
			_EBEB instance = _EBEB.Instance;
			_E0D3.Show(_E5AD.UtcNow.AddSeconds(instance.GetPlayerBufferZoneUsageTimeLeft(ProfileId)), side, points.Length + 1, _E0D5, subscribe: true, ProfileId);
			_E0DF = _EBAF.Instance.SubscribeOnEvent(delegate(_EBBF invokedEvent)
			{
				_E000(invokedEvent);
			});
			_E0D4.Add(_ED3E._E000(253419), _E0D3);
			if (!_E0DC)
			{
				_E0D4[_ED3E._E000(253419)].HideGameObject();
			}
		}
		ShowTimer(showExits: true);
	}

	private void _E000(_EBBF invokedEvent)
	{
		_E0DC = invokedEvent.InsideZone;
		if (invokedEvent.AfterReconnect && _E0DC)
		{
			_E0D3.ShowGameObject();
		}
	}

	public void SwitchTimers([CanBeNull] ExfiltrationPoint point, bool showOnePoint)
	{
		if (showOnePoint)
		{
			string key = point.Settings.Name;
			foreach (KeyValuePair<string, ExitTimerPanel> item in _E0D4)
			{
				item.Value.HideGameObject();
			}
			if (_E0D4.ContainsKey(key))
			{
				_E0D4[key].UpdateVisitedStatus();
				_E0D4[key].ShowGameObject();
			}
			return;
		}
		foreach (KeyValuePair<string, ExitTimerPanel> item2 in _E0D4)
		{
			if (item2.Key == _ED3E._E000(253419))
			{
				if (_E0DC)
				{
					item2.Value.ShowGameObject();
				}
				else
				{
					item2.Value.HideGameObject();
				}
			}
			else
			{
				item2.Value.UpdateVisitedStatus();
				item2.Value.ShowGameObject();
			}
		}
	}

	public void UpdateExfiltrationTimers(ExfiltrationPoint point, bool availability, bool contains, bool initial, float time, EExfiltrationStatus status)
	{
		string text = point.Settings.Name;
		if (_E0D4.ContainsKey(text))
		{
			if (availability || time < 0f)
			{
				_E0D4[text].Show(_E5AD.UtcNow.AddSeconds(time), _E0DB, _E0D4.Keys.ToList().IndexOf(text) + 1, _E0D5, subscribe: false, ProfileId, point);
			}
			if (contains)
			{
				BlinkPointTimer(text, availability ? new Color32(115, 175, 3, 200) : new Color32(154, 10, 7, 200));
			}
			if (!initial)
			{
				_E0D4[text].SetVisitedStatus((status != EExfiltrationStatus.NotPresent) ? ExitTimerPanel.EVisitedStatus.Visited : ExitTimerPanel.EVisitedStatus.Locked);
			}
		}
	}

	public void BlinkPointTimer(string point, Color color)
	{
		if (_E0D8 != null)
		{
			StopCoroutine(_E0D8);
			_E0D8 = null;
		}
		ShowTimer(showExits: true);
		_E0D8 = StartCoroutine(_E001(_E0D4[point], 4, color, 1f));
	}

	public void SetMainTimerState(string pointName, EMainTimerState status)
	{
		_mainTimerPanel.SetState(status);
		if (_E0D4.ContainsKey(pointName))
		{
			_E0D4[pointName].SetTimerColor(status);
		}
	}

	public void ShowTimer(bool showExits, bool updateExits = false)
	{
		if (showExits)
		{
			if (updateExits)
			{
				SwitchTimers(null, showOnePoint: false);
			}
			if (_E0D7 != null)
			{
				StopCoroutine(_E0D7);
			}
			if (_E0D6 != null)
			{
				StopCoroutine(_E0D6);
			}
			_E0D7 = null;
			_E0D6 = StartCoroutine(_E003());
			if (!_E0DD && _E0DE)
			{
				_E0D3.transform.SetParent(_container);
			}
		}
		else if (!_mainTimerPanel.ForcePull && _E0D6 == null)
		{
			if (_E0D7 != null)
			{
				StopCoroutine(_E0D7);
			}
			_E0D7 = StartCoroutine(_E002());
		}
	}

	private IEnumerator _E001(ExitTimerPanel panel, int count, Color color, float delay = 0f)
	{
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
		for (int i = 0; i < count; i++)
		{
			yield return panel.Co_ChangeColor(color);
		}
		_E0D8 = null;
	}

	private IEnumerator _E002()
	{
		_mainTimerPanel.DisplayTimer();
		yield return new WaitForSeconds(3f);
		yield return _mainTimerPanel.Co_HideTimer(delegate
		{
			_E0D6 = null;
		});
	}

	private IEnumerator _E003()
	{
		_mainTimerPanel.DisplayTimer();
		_timersPanel.gameObject.SetActive(value: true);
		LayoutRebuilder.ForceRebuildLayoutImmediate(_timersPanel);
		Rect rect = _container.rect;
		rect.width -= 143f;
		StartCoroutine(_E004(_mainDescription, new Vector3(0f - rect.width, 0f, 0f)));
		yield return new WaitForSeconds(1f);
		yield return _E005(_timersPanel, Vector2.one);
		yield return new WaitForSeconds(5f);
		StartCoroutine(_E005(_timersPanel, new Vector2(1f, 0f)));
		yield return new WaitForSeconds(0.5f);
		yield return _E004(_mainDescription, Vector3.zero);
		yield return _mainTimerPanel.Co_HideTimer(delegate
		{
			_E0D6 = null;
			_timersPanel.gameObject.SetActive(value: false);
		});
	}

	private IEnumerator _E004(RectTransform panel, Vector3 to)
	{
		float num = 0f;
		float num2 = 1f / Vector2.Distance(panel.anchoredPosition, to) * _animationSpeed;
		while (num < 1f)
		{
			num = Mathf.MoveTowards(num, 1f, Time.deltaTime * num2);
			panel.anchoredPosition = Vector3.Lerp(panel.anchoredPosition, to, _inCurve.Evaluate(num));
			yield return null;
		}
	}

	private IEnumerator _E005(RectTransform panel, Vector2 to)
	{
		float num = 0f;
		float num2 = 1f / Vector2.Distance(panel.pivot, to) * 0.5f;
		while (num < 1f)
		{
			num = Mathf.MoveTowards(num, 1f, Time.deltaTime * num2);
			panel.pivot = Vector3.Lerp(panel.pivot, to, _inCurve.Evaluate(num));
			yield return null;
		}
	}

	public void Hide()
	{
		if (_E0D9 != null)
		{
			_E0D9.alpha = 0f;
		}
	}

	public void Reveal()
	{
		if (_E0D9 != null)
		{
			_E0D9.alpha = 1f;
		}
	}

	public override void Close()
	{
		foreach (KeyValuePair<string, ExitTimerPanel> item in _E0D4)
		{
			item.Value.Close();
		}
		_E0D4.Clear();
		_mainTimerPanel.Close();
		base.Close();
		_E0DF?.Invoke();
		_E0DF = null;
	}

	[CompilerGenerated]
	private void _E006(_EBBF invokedEvent)
	{
		_E000(invokedEvent);
	}

	[CompilerGenerated]
	private void _E007()
	{
		_E0D6 = null;
	}

	[CompilerGenerated]
	private void _E008()
	{
		_E0D6 = null;
		_timersPanel.gameObject.SetActive(value: false);
	}
}
