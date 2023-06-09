using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class NotesTaskDescriptionShort : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public NotesTaskDescriptionShort _003C_003E4__this;

		public _E933 quest;
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public CancellationToken cancellationToken;

		public _E000 CS_0024_003C_003E8__locals1;

		internal void _E000(Result<Texture2D> result)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				if (result.Succeed)
				{
					Sprite sprite = Sprite.Create(result.Value, new Rect(0f, 0f, result.Value.width, result.Value.height), Vector2.one * 0.5f);
					CS_0024_003C_003E8__locals1._003C_003E4__this._questImage.sprite = sprite;
					CS_0024_003C_003E8__locals1._003C_003E4__this._questImage.enabled = true;
					CS_0024_003C_003E8__locals1._003C_003E4__this._loader.SetActive(value: false);
					CS_0024_003C_003E8__locals1.quest.Sprite = sprite;
				}
				else
				{
					CS_0024_003C_003E8__locals1._003C_003E4__this._questImage.sprite = null;
					Debug.LogError(result.Error);
				}
			}
		}
	}

	[SerializeField]
	private TextMeshProUGUI _description;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private Image _questImage;

	[SerializeField]
	private QuestObjectivesView _questObjectivesView;

	[SerializeField]
	private GameObject _initialsContainer;

	[SerializeField]
	private GameObject _rewardsContainer;

	[SerializeField]
	private GameObject _rewardListPrefab;

	private _E933 _E199;

	private _EAE6 _E19A;

	public void Show(_E933 quest, _EAE6 controller, _E935 questController, _E796 session)
	{
		ShowGameObject();
		_E199 = quest;
		_E19A = controller;
		_description.text = quest.Template.Description;
		if (quest.Sprite == null)
		{
			_questImage.enabled = false;
			_loader.SetActive(value: true);
			CancellationToken cancellationToken = UI.CancellationToken;
			session.LoadTextureMain(quest.Template.Image, delegate(Result<Texture2D> result)
			{
				if (!cancellationToken.IsCancellationRequested)
				{
					if (result.Succeed)
					{
						Sprite sprite = Sprite.Create(result.Value, new Rect(0f, 0f, result.Value.width, result.Value.height), Vector2.one * 0.5f);
						_questImage.sprite = sprite;
						_questImage.enabled = true;
						_loader.SetActive(value: false);
						quest.Sprite = sprite;
					}
					else
					{
						_questImage.sprite = null;
						Debug.LogError(result.Error);
					}
				}
			});
		}
		else
		{
			_loader.SetActive(value: false);
			_questImage.sprite = quest.Sprite;
		}
		_questObjectivesView.Show(questController, _E19A, _E199.AvailableForFinishConditions, _E199);
		_E000(_E199.Template.Rewards[EQuestStatus.Started].Count > 0);
		_E001(_E199.Template.Rewards[EQuestStatus.Success].Count > 0);
	}

	private void _E000(bool show)
	{
		_initialsContainer.DestroyAllChildren(onlyActive: true);
		_initialsContainer.gameObject.SetActive(show);
		if (show)
		{
			_initialsContainer.InstantiatePrefab<QuestRewardList>(_rewardListPrefab).Init(_ED3E._E000(258737).Localized(), _E199.Template.Rewards[EQuestStatus.Started], _E199.QuestStatus >= EQuestStatus.Started);
		}
	}

	private void _E001(bool show)
	{
		_rewardsContainer.DestroyAllChildren(onlyActive: true);
		_rewardsContainer.gameObject.SetActive(show);
		if (show)
		{
			_rewardsContainer.InstantiatePrefab<QuestRewardList>(_rewardListPrefab).Init(_ED3E._E000(258776).Localized(), _E199.Template.Rewards[EQuestStatus.Success], _E199.QuestStatus == EQuestStatus.AvailableForFinish || _E199.QuestStatus == EQuestStatus.Success);
			List<_E936> list = _E199.Template.Rewards[EQuestStatus.Fail];
			if (list.Count > 0)
			{
				_rewardsContainer.InstantiatePrefab<QuestRewardList>(_rewardListPrefab).Init(_ED3E._E000(103088) + _ED3E._E000(258762).Localized() + _ED3E._E000(59467), list, _E199.QuestStatus == EQuestStatus.AvailableForFinish || _E199.QuestStatus == EQuestStatus.Success);
			}
		}
	}

	public override void Close()
	{
		_questObjectivesView.Close();
		_E000(show: false);
		_E001(show: false);
		base.Close();
	}
}
