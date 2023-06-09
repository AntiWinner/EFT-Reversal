using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Chat;

public class ChatFriendsPanel : UIElement, IPointerClickHandler, IEventSystemHandler
{
	[SerializeField]
	private GameObject _captionPanel;

	[SerializeField]
	private FriendsListContentButton _friendsButton;

	[SerializeField]
	private FriendsListContentButton _requestsButton;

	[SerializeField]
	private ChatFriendsListPanel _chatFriendsListPanel;

	[SerializeField]
	private ChatFriendsRequestsPanel _chatFriendsRequestsPanel;

	[SerializeField]
	private Button _closeButton;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private GameObject _searchIcon;

	[SerializeField]
	private ValidationInputField _friendsInputField;

	[SerializeField]
	private ChatMembersPanel _chatMembersPanel;

	private const int _E2DA = 1;

	private _E79D _E2C7;

	private bool _E2DB;

	private float _E2DC;

	private void Awake()
	{
		_captionPanel.AddComponent<UIDragComponent>().Init(base.RectTransform, putOnTop: true);
		_friendsButton.onClick.AddListener(delegate
		{
			_friendsButton.UpdateStatus(status: true);
			_requestsButton.UpdateStatus(status: false);
		});
		_requestsButton.onClick.AddListener(delegate
		{
			_requestsButton.UpdateStatus(status: true);
			_friendsButton.UpdateStatus(status: false);
		});
		_closeButton.onClick.AddListener(Close);
		_friendsInputField.onValueChanged.AddListener(delegate
		{
			_E2DC = 0f;
			_E2DB = true;
		});
		_friendsButton.onClick.Invoke();
	}

	public void Show(_E79D social)
	{
		ShowGameObject();
		_E001();
		_E2C7 = social;
		_chatFriendsListPanel.Show(social);
		_chatFriendsRequestsPanel.Show(social);
		_chatMembersPanel.Show(social, social.SearchedFriendsList);
	}

	private void Update()
	{
		if (!_E2DB)
		{
			return;
		}
		if ((bool)_friendsInputField.HasError)
		{
			_E2C7.SearchedFriendsList.Clear();
			_E000(status: false);
			return;
		}
		_E2DC += Time.deltaTime;
		_E000(status: true);
		if (!(_E2DC < 1f))
		{
			_E2DB = false;
			_E2C7.FindAccountByNickname(_friendsInputField.text, delegate
			{
				_E000(status: false);
			});
		}
	}

	private void _E000(bool status)
	{
		_loader.SetActive(status);
		_searchIcon.SetActive(!status);
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		_E001();
	}

	private void _E001()
	{
		base.transform.SetAsLastSibling();
	}

	public override void Close()
	{
		base.Close();
		if (_chatFriendsListPanel.gameObject.activeSelf)
		{
			_chatFriendsListPanel.Close();
		}
		if (_chatFriendsRequestsPanel.gameObject.activeSelf)
		{
			_chatFriendsRequestsPanel.Close();
		}
		if (_chatMembersPanel.gameObject.activeSelf)
		{
			_chatMembersPanel.Close();
		}
		_friendsInputField.text = string.Empty;
		_E2DC = 0f;
	}

	[CompilerGenerated]
	private void _E002()
	{
		_friendsButton.UpdateStatus(status: true);
		_requestsButton.UpdateStatus(status: false);
	}

	[CompilerGenerated]
	private void _E003()
	{
		_requestsButton.UpdateStatus(status: true);
		_friendsButton.UpdateStatus(status: false);
	}

	[CompilerGenerated]
	private void _E004(string arg)
	{
		_E2DC = 0f;
		_E2DB = true;
	}

	[CompilerGenerated]
	private void _E005()
	{
		_E000(status: false);
	}
}
