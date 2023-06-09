using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class NotesTaskDescription : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public NotesTaskDescription _003C_003E4__this;

		public _E933 quest;

		internal void _E000(Result<Texture2D> result)
		{
			if (result.Succeed)
			{
				Sprite sprite = Sprite.Create(result.Value, new Rect(0f, 0f, result.Value.width, result.Value.height), Vector2.one * 0.5f);
				_003C_003E4__this._questImage.sprite = sprite;
				_003C_003E4__this._questImage.enabled = true;
				_003C_003E4__this._loader.SetActive(value: false);
				quest.Sprite = sprite;
			}
			else
			{
				_003C_003E4__this._questImage.sprite = null;
				Debug.LogError(result.Error);
			}
		}
	}

	[SerializeField]
	private CustomTextMeshProUGUI _title;

	[SerializeField]
	private CustomTextMeshProUGUI _location;

	[SerializeField]
	private CustomTextMeshProUGUI _scavIdentification;

	[SerializeField]
	private CustomTextMeshProUGUI _description;

	[SerializeField]
	private Image _titleBack;

	[SerializeField]
	private CustomTextMeshProUGUI _status;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private Image _questImage;

	[SerializeField]
	private Image _typeIcon;

	[SerializeField]
	private CustomTextMeshProUGUI _type;

	[SerializeField]
	private GameObject _lockedIcon;

	[SerializeField]
	private GameObject _timeIcon;

	[SerializeField]
	private CustomTextMeshProUGUI _timeLimitedText;

	[SerializeField]
	private GameObject _dailyBackground;

	[SerializeField]
	private ColorMap _titleBackColors = new ColorMap(new ColorMap.Data[5]
	{
		new ColorMap.Data(_ED3E._E000(192960), new Color32(155, 43, 43, 60)),
		new ColorMap.Data(_ED3E._E000(258532), new Color32(84, 88, 91, 60)),
		new ColorMap.Data(_ED3E._E000(258588), new Color32(85, 156, 195, 60)),
		new ColorMap.Data(_ED3E._E000(258575), new Color32(133, 91, 36, 60)),
		new ColorMap.Data(_ED3E._E000(258567), new Color32(84, 88, 91, 60))
	});

	[SerializeField]
	private ColorMap _textColors = new ColorMap(new ColorMap.Data[10]
	{
		new ColorMap.Data(_ED3E._E000(258609), new Color32(197, 195, 178, byte.MaxValue)),
		new ColorMap.Data(_ED3E._E000(258607), new Color32(182, 193, 199, byte.MaxValue)),
		new ColorMap.Data(_ED3E._E000(258597), new Color32(0, 0, 0, byte.MaxValue)),
		new ColorMap.Data(_ED3E._E000(258654), new Color32(231, 141, 25, byte.MaxValue)),
		new ColorMap.Data(_ED3E._E000(258636), new Color32(117, 185, 222, byte.MaxValue)),
		new ColorMap.Data(_ED3E._E000(258677), new Color32(89, 89, 89, byte.MaxValue)),
		new ColorMap.Data(_ED3E._E000(258667), new Color32(125, 203, 50, byte.MaxValue)),
		new ColorMap.Data(_ED3E._E000(258657), new Color32(89, 89, 89, byte.MaxValue)),
		new ColorMap.Data(_ED3E._E000(258708), new Color32(217, 0, 0, byte.MaxValue)),
		new ColorMap.Data(_ED3E._E000(258697), new Color32(117, 185, 222, byte.MaxValue))
	});

	internal void Show(_E933 quest, _E796 session)
	{
		ShowGameObject();
		_title.text = quest.Template.Name;
		_location.text = (quest.Template.LocationId + _ED3E._E000(70087)).Localized();
		_description.text = quest.Template.Description;
		_scavIdentification.gameObject.SetActive(quest.Template.PlayerGroup.IsScav());
		_dailyBackground.SetActive(quest is _E931);
		_titleBack.color = _titleBackColors[quest.QuestStatus];
		_status.text = (_ED3E._E000(258558) + quest.QuestStatus).Localized();
		_status.color = _textColors[_ED3E._E000(131275) + quest.QuestStatus];
		_type.text = (_ED3E._E000(258546) + quest.QuestTypeName).Localized();
		_typeIcon.sprite = EFTHardSettings.Instance.StaticIcons.QuestIconTypeSprites[quest.IconType];
		_lockedIcon.gameObject.SetActive(quest.QuestStatus == EQuestStatus.Locked);
		bool active = (quest.QuestStatus == EQuestStatus.AvailableForStart || quest.QuestStatus == EQuestStatus.FailRestartable) && quest.FailTime > 0;
		_timeIcon.gameObject.SetActive(active);
		_timeLimitedText.gameObject.SetActive(active);
		if (quest.Sprite == null)
		{
			_questImage.enabled = false;
			_loader.SetActive(value: true);
			session.LoadTextureMain(quest.Template.Image, delegate(Result<Texture2D> result)
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
			});
		}
		else
		{
			_loader.SetActive(value: false);
			_questImage.sprite = quest.Sprite;
		}
	}
}
