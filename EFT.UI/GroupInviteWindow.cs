using EFT.InputSystem;
using TMPro;
using UnityEngine;

namespace EFT.UI;

public sealed class GroupInviteWindow : DialogWindow<_EC7B>
{
	private const string m__E000 = "GroupInviteDescription";

	[SerializeField]
	private TextMeshProUGUI _description;

	[SerializeField]
	private Transform _groupListContainer;

	[SerializeField]
	private GroupMemberView _groupMemberTemplate;

	private ChatSpecialIconSettings _E001;

	public _EC7B Show(_E551 groupInvite)
	{
		UIEventSystem.Instance.Enable();
		_EC7B result = Show(null, null, delegate
		{
		});
		_E000(groupInvite);
		return result;
	}

	private void _E000(_E551 invite)
	{
		if (_E001 == null)
		{
			_E001 = _E3A2.Load<ChatSpecialIconSettings>(_ED3E._E000(250600));
		}
		_E54F info = invite.FromProfile.Info;
		ChatSpecialIconSettings.IconsData dataByMemberCategory = _E001.GetDataByMemberCategory(info.MemberCategory);
		string arg = info.Nickname.SetColor(dataByMemberCategory.IconColor);
		_description.SetText(string.Format(_ED3E._E000(255631).Localized(), arg));
		UI.AddDisposable(new _EC79<_E550, GroupMemberView>(invite.Members, _groupMemberTemplate, _groupListContainer, delegate(_E550 player, GroupMemberView playerPanel)
		{
			playerPanel.Show(player);
		}));
	}

	protected override void TranslateAxes(ref float[] axes)
	{
		axes = null;
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		base.TranslateCommand(command);
		return ETranslateResult.Block;
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.ShowCursor;
	}
}
