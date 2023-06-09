using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public sealed class MerchantInfoView : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _ECBD ragfair;

		public MerchantInfoView _003C_003E4__this;

		internal void _E000()
		{
			ragfair.OnRatingUpdated -= _003C_003E4__this._E000;
		}
	}

	[SerializeField]
	private Image _avatar;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private Sprite _defaultAvatar;

	[SerializeField]
	private ChatSpecialIcon _specialIcon;

	[SerializeField]
	private RankPanel _rankPanel;

	[SerializeField]
	private GameObject _reputationIcon;

	[SerializeField]
	private GameObject _standingIcon;

	[SerializeField]
	private TextMeshProUGUI _merchantName;

	[SerializeField]
	private TextMeshProUGUI _merchantReputation;

	[SerializeField]
	private GameObject _positiveReputation;

	[SerializeField]
	private GameObject _negativeReputation;

	private Color32 _E000 => new Color32(197, 195, 178, byte.MaxValue);

	private Color32 _E001 => new Color32(232, 0, 0, byte.MaxValue);

	public void Show(_ECBD ragfair, Offer._E000 merchant, bool isMyOffer)
	{
		UI.Dispose();
		ShowGameObject();
		_specialIcon.Show(merchant.MemberType);
		_rankPanel.Close();
		float num = (isMyOffer ? ragfair.MyRating : merchant.Rating);
		bool flag = (isMyOffer ? ragfair.IsRatingGrowing : merchant.IsRatingGrowing);
		_merchantName.text = merchant.CorrectedNickname;
		_merchantReputation.text = num.ToString(_ED3E._E000(56089));
		_positiveReputation.SetActive(flag);
		_negativeReputation.SetActive(!flag);
		_merchantReputation.color = ((num < 0f) ? this._E001 : this._E000);
		_reputationIcon.SetActive(value: true);
		_standingIcon.SetActive(value: false);
		_E001(merchant.Avatar).HandleExceptions();
		if (isMyOffer)
		{
			ragfair.OnRatingUpdated += _E000;
			UI.AddDisposable(delegate
			{
				ragfair.OnRatingUpdated -= _E000;
			});
		}
	}

	private void _E000(float rating, bool isRatingGrowing)
	{
		if (!(this == null))
		{
			_merchantReputation.text = rating.ToString(_ED3E._E000(56089));
			_positiveReputation.SetActive(isRatingGrowing);
			_negativeReputation.SetActive(!isRatingGrowing);
		}
	}

	public void Show([CanBeNull] Profile._E001 traderInfo)
	{
		UI.Dispose();
		ShowGameObject();
		if (traderInfo == null)
		{
			_rankPanel.HideGameObject();
			_merchantName.text = string.Empty;
			_merchantReputation.text = string.Empty;
			return;
		}
		_specialIcon.Close();
		_rankPanel.Show(traderInfo.LoyaltyLevel, traderInfo.MaxLoyaltyLevel);
		_merchantName.text = traderInfo.Settings.Nickname.Localized();
		_merchantReputation.text = traderInfo.Standing.ToString(_ED3E._E000(56089));
		_merchantReputation.color = this._E000;
		_positiveReputation.SetActive(value: false);
		_negativeReputation.SetActive(value: false);
		_reputationIcon.SetActive(value: false);
		_standingIcon.SetActive(value: true);
		_E002(traderInfo).HandleExceptions();
	}

	private async Task _E001(string path)
	{
		_E003(visible: true);
		CancellationToken cancellationToken = base.CancellationToken;
		Sprite sprite = await _ECBD.IconsLoader.LoadAvatar(path);
		if (!cancellationToken.IsCancellationRequested)
		{
			if (_avatar != null)
			{
				_avatar.sprite = ((sprite != null) ? sprite : _defaultAvatar);
			}
			_E003(visible: false);
		}
	}

	private async Task _E002(Profile._E001 traderInfo)
	{
		if (traderInfo != null)
		{
			_E003(visible: true);
			if (await traderInfo.Settings.GetAndAssignAvatar(_avatar, base.CancellationToken))
			{
				_E003(visible: false);
			}
		}
	}

	private void _E003(bool visible)
	{
		if (_loader != null)
		{
			_loader.SetActive(visible);
		}
		if (_avatar != null)
		{
			_avatar.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)((!visible) ? byte.MaxValue : 0));
		}
	}

	public void SetExpandedStatus(bool status)
	{
		_avatar.gameObject.SetActive(status);
	}
}
