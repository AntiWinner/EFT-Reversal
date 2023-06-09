using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;
using UnityEngine.UI;

namespace EFT;

public class PhraseTestView : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E76C tmpSpeaker;

		public PhraseTestView _003C_003E4__this;

		internal void _E000()
		{
			_003C_003E4__this.UpdateSoundList(tmpSpeaker);
		}
	}

	[SerializeField]
	private GameObject _speakersPanel;

	[SerializeField]
	private GameObject _mainPanel;

	[SerializeField]
	private Button _showUi;

	[SerializeField]
	private Button _speakerLayoutButton;

	private int m__E000;

	private void Awake()
	{
		if (_showUi != null)
		{
			_showUi.onClick.AddListener(delegate
			{
				_mainPanel.SetActive(!_mainPanel.activeSelf);
			});
		}
	}

	private void _E000()
	{
		_E001(_speakersPanel);
		foreach (_E76C speaker in Singleton<GameWorld>.Instance.SpeakerManager.Speakers)
		{
			Button button = Object.Instantiate(_speakerLayoutButton);
			button.transform.SetParent(_speakersPanel.transform);
			button.GetComponent<RectTransform>().localScale = Vector3.one;
			button.gameObject.SetActive(value: true);
			button.GetComponent<Image>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
			_E76C tmpSpeaker = speaker;
			button.onClick.AddListener(delegate
			{
				UpdateSoundList(tmpSpeaker);
			});
		}
	}

	public void UpdateSoundList(_E76C speaker)
	{
	}

	private void _E001(GameObject panel)
	{
		Button[] componentsInChildren = panel.GetComponentsInChildren<Button>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.DestroyImmediate(componentsInChildren[i].gameObject);
		}
	}

	[CompilerGenerated]
	private void _E002()
	{
		_mainPanel.SetActive(!_mainPanel.activeSelf);
	}
}
