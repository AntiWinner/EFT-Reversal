using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.Communications;
using UnityEngine;

namespace EFT.UI;

public sealed class NotifierView : UIElement
{
	public interface _E000
	{
		void OnProfileChangeApplied(ENotificationRequirements requirements);
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E856 notification;

		internal bool _E000(NotificationView x)
		{
			if (!x.IsHiding)
			{
				return x.HasSameNotification(notification);
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public List<_E86C> notifications;

		public NotifierView _003C_003E4__this;

		public ENotificationRequirements requirement;

		internal void _E000()
		{
			notifications.Clear();
			_003C_003E4__this._E176?.OnProfileChangeApplied(requirement);
		}
	}

	private const string _E171 = "UI/Messaging/applied_profile_changes";

	[SerializeField]
	private NotificationView _notificationPrefab;

	[SerializeField]
	private RectTransform _container;

	[SerializeField]
	private Color _defaultNotificationColor = new Color32(0, 0, 0, 200);

	[SerializeField]
	private Color _questNotificationColor = new Color32(35, 35, 15, 200);

	private _E857 _E172;

	private IEnumerator _E093;

	private const int _E173 = 4;

	private readonly Queue<NotificationView> _E174 = new Queue<NotificationView>(4);

	private readonly List<NotificationView> _E175 = new List<NotificationView>(4);

	private _E000 _E176;

	private _EC7B _E177;

	private void Awake()
	{
		for (int i = 0; i < 4; i++)
		{
			NotificationView notificationView = Object.Instantiate(_notificationPrefab, _container, worldPositionStays: false);
			notificationView.gameObject.SetActive(value: false);
			_E174.Enqueue(notificationView);
		}
	}

	private void OnDisable()
	{
		_E005();
	}

	public void Init()
	{
		ShowGameObject();
		_E005();
		_E172 = Singleton<_E857>.Instance;
		_E172.OnNotificationReceived += _E003;
	}

	public void SetProfileChangeHandlerState(_E000 profileChangeHandler, bool active)
	{
		if (active)
		{
			_E176 = profileChangeHandler;
			_E004();
		}
		else if (_E176 == profileChangeHandler)
		{
			_E176 = null;
		}
	}

	private void _E000(_E856 notification)
	{
		if (!_E175.Any((NotificationView x) => !x.IsHiding && x.HasSameNotification(notification)))
		{
			_E001().Init(notification, EFTHardSettings.Instance.StaticIcons.NotificationSprites[notification.Icon], (notification is _E889) ? _questNotificationColor : _defaultNotificationColor);
			if (Singleton<GUISounds>.Instantiated)
			{
				Singleton<GUISounds>.Instance.PlayNotificationSound();
			}
		}
	}

	private NotificationView _E001()
	{
		NotificationView notificationView = ((_E174.Count > 0) ? _E174.Dequeue() : Object.Instantiate(_notificationPrefab, _container, worldPositionStays: false));
		notificationView.gameObject.SetActive(value: true);
		notificationView.transform.SetAsLastSibling();
		notificationView.OnHideComplete += _E002;
		_E175.Add(notificationView);
		return notificationView;
	}

	private void _E002(NotificationView notificationView)
	{
		if (!(notificationView == null))
		{
			notificationView.OnHideComplete -= _E002;
			_E175.Remove(notificationView);
			notificationView.gameObject.SetActive(value: false);
			_E174.Enqueue(notificationView);
		}
	}

	private void _E003(_E856 notification)
	{
		if (notification is _E86C)
		{
			_E004();
		}
		if (notification.ShowNotification)
		{
			_E000(notification);
		}
	}

	private void _E004()
	{
		if (_E176 == null)
		{
			return;
		}
		List<_E86C> notifications = _E172.ProfileChangeNotifications;
		if (notifications.Any())
		{
			IEnumerable<string> values = notifications.Select((_E86C n) => n.Description);
			ENotificationRequirements requirement = notifications.Max((_E86C n) => n.Requirement);
			string description = _ED3E._E000(247974).Localized(EStringCase.Upper) + _ED3E._E000(248003) + string.Join(_ED3E._E000(2540), values);
			_E177?.CloseSilent();
			_E177 = ItemUiContext.Instance.ShowScrolledMessageWindow(description, delegate
			{
				notifications.Clear();
				_E176?.OnProfileChangeApplied(requirement);
			});
		}
	}

	private void _E005()
	{
		if (_E172 != null)
		{
			_E172.OnNotificationReceived -= _E003;
			_E172 = null;
			_E176 = null;
		}
	}

	private void _E006()
	{
		string description = new string(Enumerable.Repeat('#', Random.Range(0, 50)).ToArray());
		_E000(new _E89D(description));
	}
}
