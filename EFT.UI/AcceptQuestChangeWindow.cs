using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class AcceptQuestChangeWindow : DialogWindow<_EC7B>
{
	[SerializeField]
	private TextMeshProUGUI _traderReplica;

	[SerializeField]
	private TextMeshProUGUI _timer;

	[SerializeField]
	private Image _avatar;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private GameObject _priceContainer;

	[SerializeField]
	private GameObject[] _timerElements;

	[SerializeField]
	private PriceRow _priceRowOriginal;

	private readonly List<PriceRow> m__E000 = new List<PriceRow>();

	private _EC7A m__E001;

	private bool m__E002;

	private DateTime m__E003;

	protected override bool CloseOnAccept => false;

	public _EC7B Show(_EC7A arguments)
	{
		_EC7B result = Show(_ED3E._E000(250339).Localized(), delegate
		{
		}, delegate
		{
		});
		this.m__E001 = arguments;
		_traderReplica.text = arguments.TraderCancelText;
		_E004().HandleExceptions();
		_E003();
		_E002();
		return result;
	}

	public override void Accept()
	{
		if (this.m__E001.QuestToChange.IsChangeAllowed)
		{
			_E000(status: false);
			base.Accept();
		}
	}

	public override void Decline()
	{
		if (!this.m__E002)
		{
			base.Decline();
		}
		else
		{
			CloseSilent();
		}
	}

	private void _E000(bool status)
	{
		this.m__E002 = !status;
		base.AcceptButton.Interactable = status;
	}

	protected override void Update()
	{
		if (!this.m__E001.QuestToChange.IsChangeAllowed)
		{
			Close();
		}
		else
		{
			_E001();
		}
		base.Update();
	}

	private void _E001()
	{
		if (this.m__E001.QuestToChange != null && this.m__E001.QuestToChange.NeedCountdown && !(this.m__E003 + TimeSpan.FromSeconds(1.0) > _E5AD.UtcNow) && this.m__E001.QuestToChange.ExpirationDate - _E5AD.UtcNowUnixInt >= 0)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds(this.m__E001.QuestToChange.ExpirationDate - _E5AD.UtcNowUnixInt);
			_timer.SetMonospaceText(timeSpan.DailyQuestFormat());
			this.m__E003 = _E5AD.UtcNow - TimeSpan.FromMilliseconds(timeSpan.Milliseconds);
		}
	}

	private void _E002()
	{
		_timerElements.ForEach(delegate(GameObject te)
		{
			te.SetActive(this.m__E001.QuestToChange.NeedCountdown);
		});
	}

	private void _E003()
	{
		_priceContainer.SetActive(this.m__E001.Price.Lines.Any());
		this.m__E001.Price.Lines.ForEach(delegate(_EC7F priceInfo)
		{
			PriceRow priceRow = UnityEngine.Object.Instantiate(_priceRowOriginal, _priceContainer.transform, worldPositionStays: false);
			this.m__E000.Add(priceRow);
			priceRow.Show(priceInfo);
		});
	}

	private async Task _E004()
	{
		_loader.SetActive(value: true);
		_avatar.gameObject.SetActive(value: false);
		if (await Singleton<_E5CB>.Instance.TradersSettings[this.m__E001.QuestToChange.Template.TraderId].GetAndAssignAvatar(_avatar, UI.CancellationToken))
		{
			if (_loader != null)
			{
				_loader.SetActive(value: false);
			}
			if (_avatar != null)
			{
				_avatar.gameObject.SetActive(value: true);
			}
		}
	}

	public override void Close()
	{
		base.Close();
		foreach (PriceRow item in this.m__E000)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		this.m__E000.Clear();
		_E000(status: true);
		this.m__E003 = default(DateTime);
	}

	[CompilerGenerated]
	private void _E005(GameObject te)
	{
		te.SetActive(this.m__E001.QuestToChange.NeedCountdown);
	}

	[CompilerGenerated]
	private void _E006(_EC7F priceInfo)
	{
		PriceRow priceRow = UnityEngine.Object.Instantiate(_priceRowOriginal, _priceContainer.transform, worldPositionStays: false);
		this.m__E000.Add(priceRow);
		priceRow.Show(priceInfo);
	}
}
