using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.Communications;
using UnityEngine;

namespace EFT.UI.Chat;

public sealed class ProfileEventsWindow : Window<_EC7C>
{
	[SerializeField]
	private Transform _eventsList;

	[SerializeField]
	private ProfileEventView _eventViewTemplate;

	[SerializeField]
	private DefaultUIButton _applyAllButton;

	[SerializeField]
	private DefaultUIButton _cancelButton;

	private IEnumerable<ProfileChangeEvent> m__E000;

	private Action<IEnumerable<ProfileChangeEvent>> m__E001;

	protected override void Awake()
	{
		base.Awake();
		_applyAllButton.OnClick.AddListener(_E001);
		_cancelButton.OnClick.AddListener(Close);
	}

	public _EC7C Show(IEnumerable<ProfileChangeEvent> profileEvents, Action<IEnumerable<ProfileChangeEvent>> onAccept)
	{
		this.m__E000 = profileEvents;
		this.m__E001 = onAccept;
		_applyAllButton.gameObject.SetActive(this.m__E000.Count() > 1);
		_EC7C result = Show(null);
		UI.AddDisposable(new _EC79<ProfileChangeEvent, ProfileEventView>(this.m__E000, _eventViewTemplate, _eventsList, delegate(ProfileChangeEvent item, ProfileEventView view)
		{
			view.Show(item);
			UI.SubscribeEvent(view.OnRedeem, delegate(ProfileChangeEvent @event)
			{
				_E000(new ProfileChangeEvent[1] { @event });
			});
		}));
		return result;
	}

	private void _E000(IEnumerable<ProfileChangeEvent> events)
	{
		this.m__E001?.Invoke(events);
		Close();
	}

	private void _E001()
	{
		_E000(this.m__E000.Where((ProfileChangeEvent @event) => !@event.Redeemed));
	}

	[CompilerGenerated]
	private void _E002(ProfileChangeEvent item, ProfileEventView view)
	{
		view.Show(item);
		UI.SubscribeEvent(view.OnRedeem, delegate(ProfileChangeEvent @event)
		{
			_E000(new ProfileChangeEvent[1] { @event });
		});
	}

	[CompilerGenerated]
	private void _E003(ProfileChangeEvent @event)
	{
		_E000(new ProfileChangeEvent[1] { @event });
	}
}
