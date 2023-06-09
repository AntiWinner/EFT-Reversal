using System;
using System.Runtime.CompilerServices;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Matchmaker;

public sealed class GroupPlayerPanel : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public string youStatus;

		public GroupPlayerPanel _003C_003E4__this;

		internal void _E000(_EC9B owner)
		{
			string text = youStatus;
			_003C_003E4__this._E29A = owner;
			if (owner != null)
			{
				text = ((owner.AccountId == _003C_003E4__this._E094.AccountId) ? (_ED3E._E000(235114).Localized() + youStatus) : (_ED3E._E000(235121).Localized() + youStatus));
				_003C_003E4__this.UI.BindEvent(owner.StatusChanged, _003C_003E4__this._E000);
			}
			_003C_003E4__this._statusLabel.text = text;
		}
	}

	private const float _E295 = 0.76f;

	[CompilerGenerated]
	private Action<_EC9B, Vector2> _E296;

	[SerializeField]
	private Image _backgroundImage;

	[SerializeField]
	private ChatSpecialIcon _icon;

	[SerializeField]
	private Color _defaultImageColor;

	[SerializeField]
	private Color _readyColor;

	[SerializeField]
	private Color _notReadyColor;

	[SerializeField]
	private CustomTextMeshProUGUI _nameLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _sideLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _statusLabel;

	private _EC9B _E094;

	private bool _E297;

	private bool _E298;

	private bool _E299;

	private _EC9B _E29A;

	private bool _E29B;

	public event Action<_EC9B, Vector2> OnPlayerClick
	{
		[CompilerGenerated]
		add
		{
			Action<_EC9B, Vector2> action = _E296;
			Action<_EC9B, Vector2> action2;
			do
			{
				action2 = action;
				Action<_EC9B, Vector2> value2 = (Action<_EC9B, Vector2>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E296, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<_EC9B, Vector2> action = _E296;
			Action<_EC9B, Vector2> action2;
			do
			{
				action2 = action;
				Action<_EC9B, Vector2> value2 = (Action<_EC9B, Vector2>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E296, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void Show(_EC9B player, bool friendPanel, string profileAid, _ECF5<_EC9B> groupOwner)
	{
		ShowGameObject();
		_E094 = player;
		_E298 = friendPanel;
		_nameLabel.text = player.GetCorrectedNickname();
		_sideLabel.text = _ED3E._E000(253257) + player.Info.Level + _ED3E._E000(235128) + player.Info.Side.LocalizedShort(EStringCase.Upper) + _ED3E._E000(11164);
		_icon.Show((player.Info.Side != EPlayerSide.Savage) ? player.Info.MemberCategory : EMemberCategory.Default);
		_E297 = profileAid == player.AccountId;
		string youStatus = (_E297 ? (_ED3E._E000(54246) + _ED3E._E000(235125).Localized() + _ED3E._E000(27308)) : string.Empty);
		UI.AddDisposable(groupOwner.Bind(delegate(_EC9B owner)
		{
			string text = youStatus;
			_E29A = owner;
			if (owner != null)
			{
				text = ((owner.AccountId == _E094.AccountId) ? (_ED3E._E000(235114).Localized() + youStatus) : (_ED3E._E000(235121).Localized() + youStatus));
				UI.BindEvent(owner.StatusChanged, _E000);
			}
			_statusLabel.text = text;
		}));
		UI.BindEvent(_E094.StatusChanged, _E001);
	}

	private void _E000()
	{
		_E29B = _E29A?.IsReady ?? false;
		_E001();
	}

	private void _E001()
	{
		_E002(_E299);
	}

	private void _E002(bool hover)
	{
		float alpha = (hover ? 1f : 0.76f);
		bool flag = !_E29B || _E094.IsReady;
		_backgroundImage.color = ((!flag) ? _notReadyColor.SetAlpha(alpha) : ((_E297 || _E298) ? _readyColor.SetAlpha(alpha) : _defaultImageColor.SetAlpha(alpha)));
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		_E299 = true;
		_E002(_E299);
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		_E299 = false;
		_E002(_E299);
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right || eventData.button == PointerEventData.InputButton.Left)
		{
			_E296?.Invoke(_E094, eventData.position);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
	}

	public void OnPointerDown(PointerEventData eventData)
	{
	}
}
