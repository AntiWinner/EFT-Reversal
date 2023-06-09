using EFT.UI.SessionEnd;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class StatGroupView : UIElement
{
	[SerializeField]
	private GameObject _headerGameObject;

	[SerializeField]
	private CustomTextMeshProUGUI _caption;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private Transform _itemsContainer;

	[SerializeField]
	private StatItemView _statItemViewTemplate;

	public void Show(SessionResultStatistics._E001 statGroup)
	{
		ShowGameObject();
		if (!_headerGameObject.gameObject.activeSelf)
		{
			_headerGameObject.gameObject.SetActive(value: true);
		}
		_caption.text = statGroup.Caption;
		if (_icon != null && (bool)statGroup.Icon)
		{
			_icon.sprite = statGroup.Icon;
		}
		UI.AddViewList(statGroup.StatItems, _statItemViewTemplate, _itemsContainer, delegate(_E34D._E000 statItem, StatItemView statItemView)
		{
			statItemView.Show(statItem);
		});
	}
}
