using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class GroupMemberView : ButtonFeedback, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public GroupMemberView _003C_003E4__this;

		public _E550 player;

		internal void _E000(_EC9B owner)
		{
			if (owner != null)
			{
				_003C_003E4__this._E0FE = player.AccountId == owner.AccountId;
			}
			_003C_003E4__this._border.gameObject.SetActive(!_003C_003E4__this._E0FE);
			_003C_003E4__this._borderLeader.gameObject.SetActive(_003C_003E4__this._E0FE);
		}
	}

	[CompilerGenerated]
	private Action<_E550, PointerEventData> _E0FD;

	[SerializeField]
	private Image _bgImage;

	[SerializeField]
	private Image _border;

	[SerializeField]
	private Image _borderLeader;

	[SerializeField]
	private Sprite _bgIdle;

	[SerializeField]
	private Sprite _bgHover;

	[SerializeField]
	private Sprite _borderIdle;

	[SerializeField]
	private Sprite _borderHover;

	[SerializeField]
	private Sprite _borderLeaderIdle;

	[SerializeField]
	private Sprite _borderLeaderHover;

	[SerializeField]
	private TextMeshProUGUI _nicknameLetter;

	[SerializeField]
	private ChatSpecialIcon _playerNickname;

	private SimpleTooltip _E02A;

	private _E550 _E094;

	private bool _E0FE;

	public event Action<_E550, PointerEventData> OnGroupMemberClick
	{
		[CompilerGenerated]
		add
		{
			Action<_E550, PointerEventData> action = _E0FD;
			Action<_E550, PointerEventData> action2;
			do
			{
				action2 = action;
				Action<_E550, PointerEventData> value2 = (Action<_E550, PointerEventData>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E0FD, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<_E550, PointerEventData> action = _E0FD;
			Action<_E550, PointerEventData> action2;
			do
			{
				action2 = action;
				Action<_E550, PointerEventData> value2 = (Action<_E550, PointerEventData>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E0FD, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void Show(_E550 player, _ECF5<_EC9B> groupOwner)
	{
		_E000(player);
		_E001(hover: false);
		UI.AddDisposable(groupOwner.Bind(delegate(_EC9B owner)
		{
			if (owner != null)
			{
				_E0FE = player.AccountId == owner.AccountId;
			}
			_border.gameObject.SetActive(!_E0FE);
			_borderLeader.gameObject.SetActive(_E0FE);
		}));
	}

	public void Show(_E550 player)
	{
		_E000(player);
		_E0FE = player.IsLeader;
		_border.gameObject.SetActive(!_E0FE);
		_borderLeader.gameObject.SetActive(_E0FE);
	}

	private void _E000(_E550 player)
	{
		ShowGameObject();
		_E094 = player;
		_bgImage.sprite = _bgIdle;
		string correctedNickname = _E094.GetCorrectedNickname();
		if (!correctedNickname.IsNullOrEmpty())
		{
			_nicknameLetter.SetText(correctedNickname.FirstOrDefault().ToString());
		}
		if (_playerNickname != null)
		{
			_playerNickname.Show(player.Info.MemberCategory, correctedNickname);
		}
	}

	private void _E001(bool hover)
	{
		_bgImage.sprite = (hover ? _bgHover : _bgIdle);
		_border.sprite = (hover ? _borderHover : _borderIdle);
		_borderLeader.sprite = (hover ? _borderLeaderHover : _borderLeaderIdle);
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		base.OnPointerClick(eventData);
		_E0FD?.Invoke(_E094, eventData);
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		_E001(hover: true);
		_E02A = ItemUiContext.Instance.Tooltip;
		if (_E094 != null)
		{
			string correctedNickname = _E094.GetCorrectedNickname();
			if (_E02A != null && !string.IsNullOrEmpty(correctedNickname))
			{
				_E02A.Show(correctedNickname);
			}
		}
	}

	public new void OnPointerExit(PointerEventData eventData)
	{
		_E001(hover: false);
		if (!(_E02A == null))
		{
			_E02A.Close();
			_E02A = null;
		}
	}
}
