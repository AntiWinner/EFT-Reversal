using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI;

public class ServicesListView : UIElement
{
	[Serializable]
	public class ServiceInfo
	{
		public string Name;

		public Sprite IconBackground;

		public Sprite Icon;

		public ServiceView ServiceView;
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public _E8B2 trader;

		public ServicesListView _003C_003E4__this;

		internal bool _E000(ServiceInfo info)
		{
			return info.ServiceView.CheckAvailable(trader);
		}

		internal void _E001(ServiceInfo service, ServiceListItem serviceView)
		{
			serviceView.Init(_003C_003E4__this._E000, service);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E8B2 trader;

		internal bool _E000(ServiceInfo info)
		{
			return info.ServiceView.CheckAvailable(trader);
		}
	}

	[SerializeField]
	private RectTransform _serviceListContainer;

	[SerializeField]
	private ServiceListItem _serviceItemTemplate;

	[SerializeField]
	private ServiceInfo[] _possibleServices;

	private ServiceListItem _E19F;

	private _E8B2 _E1BB;

	private Profile _E0B7;

	private _EAED _E092;

	private _E9C4 _E0AE;

	private _E934 _E1BC;

	private ItemUiContext _E118;

	private _E796 _E17F;

	public void Show(_E8B2 trader, Profile profile, _EAED inventoryController, _E9C4 healthController, _E934 quests, ItemUiContext context, _E796 session)
	{
		ShowGameObject();
		_E1BB = trader;
		_E0B7 = profile;
		_E092 = inventoryController;
		_E0AE = healthController;
		_E1BC = quests;
		_E118 = context;
		_E17F = session;
		List<ServiceInfo> list = _possibleServices.Where((ServiceInfo info) => info.ServiceView.CheckAvailable(trader)).ToList();
		if (list.Count > 0)
		{
			_EC79<ServiceInfo, ServiceListItem> source = UI.AddViewList(list, _serviceItemTemplate, _serviceListContainer, delegate(ServiceInfo service, ServiceListItem serviceView)
			{
				serviceView.Init(_E000, service);
			});
			_E000(source.First().Value);
		}
	}

	public bool CheckAvailableServices(_E8B2 trader)
	{
		return _possibleServices.Any((ServiceInfo info) => info.ServiceView.CheckAvailable(trader));
	}

	private void _E000(ServiceListItem serviceItem)
	{
		if (_E19F != null)
		{
			if (serviceItem == _E19F)
			{
				return;
			}
			_E19F.ServiceInfo.ServiceView.Close();
			_E19F.UpdateView(selected: false);
			_E19F.ServiceInfo.ServiceView.Hide();
		}
		serviceItem.UpdateView(selected: true);
		serviceItem.ServiceInfo.ServiceView.Show(_E1BB, _E0B7, _E092, (_E981)_E0AE, _E1BC, _E118, _E17F);
		_E19F = serviceItem;
	}

	public override void Close()
	{
		if (_E19F != null)
		{
			_E19F.ServiceInfo.ServiceView.Close();
		}
		base.Close();
	}
}
