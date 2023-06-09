using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EFT.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class TraderAvatar : UIElement
{
	[SerializeField]
	private GameObject _availableToStartQuestsIcon;

	[SerializeField]
	private GameObject _availableToFinishQuestsIcon;

	[SerializeField]
	private Image _avatar;

	[SerializeField]
	private GameObject _loader;

	private Profile._E001 _E1FB;

	private _E935 _E19B;

	private _E934 _E000 => _E19B.Quests;

	public void Show(Profile._E001 trader, _E935 questController)
	{
		ShowGameObject();
		_E1FB = trader;
		_E19B = questController;
		_E000();
		_E19B.OnQuestStatusChanged += _E000;
		UI.AddDisposable(delegate
		{
			_E19B.OnQuestStatusChanged -= _E000;
		});
		_E001().HandleExceptions();
	}

	private void _E000()
	{
		if (!(base.gameObject == null) && base.gameObject.activeSelf)
		{
			bool flag = this._E000.GetQuestsCount(_E1FB.Id, EQuestStatus.AvailableForStart) > 0;
			bool flag2 = this._E000.GetQuestsCount(_E1FB.Id, EQuestStatus.AvailableForFinish) > 0;
			bool available = _E1FB.Available;
			_availableToStartQuestsIcon.SetActive(available && flag);
			_availableToFinishQuestsIcon.SetActive(available && flag2);
		}
	}

	private async Task _E001()
	{
		_loader.SetActive(value: true);
		if (await _E1FB.Settings.GetAndAssignAvatar(_avatar, base.CancellationToken))
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

	[CompilerGenerated]
	private void _E002()
	{
		_E19B.OnQuestStatusChanged -= _E000;
	}
}
