using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Comfort.Common;
using EFT.Interactive;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.BattleTimer;

public class ExitTimerPanel : TimerPanel
{
	public enum EVisitedStatus
	{
		NotVisited,
		Visited,
		Locked,
		BufferZone
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public string profileId;

		internal bool _E000(Player x)
		{
			return x.ProfileId == profileId;
		}
	}

	private const string _E282 = "Buffer_zone_timer";

	private const int _E283 = 10;

	[SerializeField]
	protected GameObject _itemsObject;

	[SerializeField]
	private CustomTextMeshProUGUI _itemsToBringLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _pointStatusLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _pointName;

	[SerializeField]
	private Image _backgroundImage;

	[SerializeField]
	private Image _requirementImage;

	[SerializeField]
	private Image _timerImage;

	[SerializeField]
	private Image _bufferZoneTimerIcon;

	[SerializeField]
	private Color _visitedTextColor;

	[SerializeField]
	private Color _notVisitedTextColor;

	[SerializeField]
	private Color _lockedTextColor;

	[SerializeField]
	private Color _bufferZoneBackgroundColor;

	[SerializeField]
	private Color _bufferZoneTextColor;

	[SerializeField]
	private Color _defaultTimerColor;

	[SerializeField]
	private Color _warningTimerColor;

	[SerializeField]
	private Color _defaultTimerBackgroundColor;

	[SerializeField]
	private Color _warningTimerBackgroundColor;

	[SerializeField]
	private GameObject _discountStats;

	[SerializeField]
	private TextMeshProUGUI _totalDiscount;

	[SerializeField]
	private TextMeshProUGUI _fencerRep;

	private Color _E284;

	protected ExfiltrationPoint _point;

	private Coroutine _E285;

	private DateTime _E286;

	private EVisitedStatus _E287;

	private EPlayerSide _E0DB;

	private int _E288;

	private string _E289;

	private bool _E28A;

	private bool _E28B;

	private int _E28C;

	public virtual void Show(DateTime dateTime, EPlayerSide side, int index, StringBuilder stringBuilder, bool subscribe, string profileId, ExfiltrationPoint point = null, bool isActiveBufferZoneTimer = false)
	{
		_itemsObject.SetActive(value: false);
		_E286 = dateTime;
		_point = point;
		_E0DB = side;
		_E288 = index;
		_E289 = profileId;
		_E28A = _EBEB.Instance.IsBufferZoneExists;
		_E28B = _point == null;
		Show(dateTime, stringBuilder);
		if (subscribe)
		{
			_E000();
			UI.AddDisposable(_E7AD._E010.AddLocaleUpdateListener(_E000));
		}
		_E284 = _defaultTimerBackgroundColor;
		SetVisitedStatus(EVisitedStatus.NotVisited);
		if (_discountStats != null)
		{
			_discountStats.SetActive(value: false);
		}
	}

	private void _E000()
	{
		if (!(this == null))
		{
			_pointName.text = ((_E28A && _E28B) ? _ED3E._E000(234802).Localized() : _point.Settings.Name.Localized());
			_pointName.color = ((_E28A && _E28B) ? _bufferZoneTextColor : _defaultTimerColor);
			if (_E28A && _E28B)
			{
				_E002();
			}
			else
			{
				_pointStatusLabel.text = ((_E0DB == EPlayerSide.Savage) ? _ED3E._E000(249941).Localized() : _ED3E._E000(234844).Localized()) + ((_E288 >= 10) ? _E288.ToString() : (_ED3E._E000(27314) + _E288));
			}
			UpdateVisitedStatus();
		}
	}

	public void SetVisitedStatus(EVisitedStatus visited)
	{
		_E287 = (_E28B ? EVisitedStatus.BufferZone : visited);
		UpdateVisitedStatus();
		SetTimerTextActive();
	}

	public void UpdateVisitedStatus()
	{
		_pointStatusLabel.color = ((_E287 == EVisitedStatus.NotVisited) ? _notVisitedTextColor : ((_E287 == EVisitedStatus.Visited) ? _visitedTextColor : _lockedTextColor));
		if (_E287 == EVisitedStatus.Visited && _point.HasRequirements)
		{
			_itemsObject.SetActive(_point.UnmetRequirements(_E289).Any());
			float num = _E001(_E289);
			_totalDiscount.SetText(_ED3E._E000(234842), num);
			_fencerRep.SetText(_ED3E._E000(234833), _point.FenceRep);
			if (_discountStats != null)
			{
				_discountStats.SetActive(_itemsObject.activeSelf);
			}
			if (num.ApproxEquals(0f))
			{
				_discountStats.SetActive(value: false);
			}
			string[] tips = _point.GetTips(_E289);
			_itemsToBringLabel.text = string.Join(_ED3E._E000(2540), tips);
		}
	}

	private float _E001(string profileId)
	{
		Player player = Singleton<GameWorld>.Instance.RegisteredPlayers.FirstOrDefault((Player x) => x.ProfileId == profileId);
		if (player == null)
		{
			return 0f;
		}
		return Mathf.Clamp01(1f - (float)player.Profile.GetExfiltrationPrice(100000) / 100000f) * 100f;
	}

	public void SetTimerColor(EMainTimerState status)
	{
		_timerImage.color = ((status == EMainTimerState.StayInEpOutside) ? _warningTimerBackgroundColor : _defaultTimerBackgroundColor);
		if (_E28B && _E28A)
		{
			bool flag = _EBEB.Instance.GetPlayerBufferZoneUsageTimeLeft(_E289) <= 10;
			_timerImage.color = (flag ? _warningTimerBackgroundColor : _bufferZoneBackgroundColor);
			base.TimerText.color = (flag ? _warningTimerColor : _bufferZoneTextColor);
		}
	}

	private void _E002()
	{
		_pointStatusLabel.gameObject.SetActive(value: false);
		_bufferZoneTimerIcon.gameObject.SetActive(value: true);
		_backgroundImage.color = ((_EBEB.Instance.GetPlayerBufferZoneUsageTimeLeft(_E289) <= 10) ? _warningTimerBackgroundColor : _bufferZoneBackgroundColor);
	}

	protected virtual void SetTimerTextActive()
	{
		if (_point == null)
		{
			base.TimerText.gameObject.SetActive(_E28A);
			return;
		}
		bool num = _point.Settings.Chance < 100f && _E287 != EVisitedStatus.Visited;
		bool flag = _point.Settings.MaxTime > 0f;
		bool flag2 = _E286.CompareTo(_E5AD.UtcNow) > 0;
		bool active = num || flag || flag2 || _E003();
		base.TimerText.gameObject.SetActive(active);
	}

	private bool _E003()
	{
		if (_point == null || !_point.HasRequirements)
		{
			return false;
		}
		ExfiltrationRequirement[] requirements = _point.Requirements;
		for (int i = 0; i < requirements.Length; i++)
		{
			if (requirements[i].Requirement == ERequirementState.Timer)
			{
				return true;
			}
		}
		return false;
	}

	protected TimeSpan GetTimerRequirementTime()
	{
		float num = Singleton<AbstractGame>.Instance.GameTimer.PastTimeSeconds() - (float)_point.Settings.StartTime;
		int seconds = 0;
		if (num < 0f)
		{
			seconds = Mathf.Abs(Mathf.RoundToInt(num));
		}
		return new TimeSpan(0, 0, seconds);
	}

	private TimeSpan _E004()
	{
		float num = Singleton<AbstractGame>.Instance.GameTimer.EscapeTimeSeconds();
		_E28C = _EBEB.Instance.GetPlayerBufferZoneUsageTimeLeft(_E289);
		bool flag = num <= 600f;
		if (!flag)
		{
			return new TimeSpan(0, 0, _E28C);
		}
		if (flag && (float)_E28C > num)
		{
			return new TimeSpan(0, 0, (int)num);
		}
		return new TimeSpan(0, 0, _E28C);
	}

	protected override void UpdateTimer()
	{
		base.UpdateTimer();
		if (!_E28A)
		{
			bool flag = TimeSpan.TotalSeconds <= 10.0;
			if (_E287 == EVisitedStatus.Visited)
			{
				base.TimerText.color = (flag ? _warningTimerColor : _defaultTimerColor);
			}
			if (_E287 == EVisitedStatus.BufferZone)
			{
				_backgroundImage.color = (flag ? _warningTimerColor : _bufferZoneBackgroundColor);
				base.TimerText.color = (flag ? _warningTimerColor : _defaultTimerColor);
			}
		}
		else if (_EBEB.Instance.GetPlayerBufferZoneUsageTimeLeft(_E289) <= 10)
		{
			_timerImage.color = _warningTimerBackgroundColor;
			base.TimerText.color = _warningTimerColor;
			_backgroundImage.color = _warningTimerColor;
		}
	}

	protected override void SetTimerText(TimeSpan timeSpan)
	{
		if (_E003() && _E287 == EVisitedStatus.Visited)
		{
			base.SetTimerText(GetTimerRequirementTime());
		}
		else if (_E287 == EVisitedStatus.BufferZone)
		{
			base.SetTimerText(_E004());
		}
		else if (_E287 == EVisitedStatus.Visited)
		{
			base.SetTimerText(timeSpan);
		}
		else
		{
			base.TimerText.text = _ED3E._E000(234831);
		}
	}

	public IEnumerator Co_ChangeColor(Color to)
	{
		float num = 0f;
		Image requirementImage = _requirementImage;
		Color color2 = (_backgroundImage.color = _E284);
		requirementImage.color = color2;
		while (num < 1f)
		{
			num += Time.deltaTime * 6f;
			Image requirementImage2 = _requirementImage;
			color2 = (_backgroundImage.color = Color.Lerp(_backgroundImage.color, to, num));
			requirementImage2.color = color2;
			yield return null;
		}
		num = 0f;
		while (num < 1f)
		{
			num += Time.deltaTime * 6f;
			Image requirementImage3 = _requirementImage;
			color2 = (_backgroundImage.color = Color.Lerp(_backgroundImage.color, _E284, num));
			requirementImage3.color = color2;
			yield return null;
		}
	}
}
