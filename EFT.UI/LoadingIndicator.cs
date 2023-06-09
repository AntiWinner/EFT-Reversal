using DG.Tweening;
using TMPro;
using UnityEngine;

namespace EFT.UI;

public sealed class LoadingIndicator : MonoBehaviour
{
	[SerializeField]
	private RectTransform _spinner;

	[SerializeField]
	private TMP_Text _text;

	[SerializeField]
	private TMP_Text _dots;

	private int m__E000 = -1;

	private void OnEnable()
	{
		_E000();
		if (!(_spinner == null))
		{
			_spinner.rotation = Quaternion.identity;
			_spinner.DORotate(new Vector3(0f, 0f, -360f), 2f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
		}
	}

	private void OnDisable()
	{
		if (_spinner != null)
		{
			_spinner.DOKill();
		}
	}

	private void Update()
	{
		if (_dots == null)
		{
			return;
		}
		int num = _E5AD.UtcNowUnixInt % 4;
		if (this.m__E000 != num)
		{
			this.m__E000 = num;
			string text = string.Empty;
			for (int i = 0; i < this.m__E000; i++)
			{
				text += _ED3E._E000(31756);
			}
			_dots.text = text;
		}
	}

	private void _E000()
	{
		if (!(_text == null))
		{
			string text = _text.text;
			if (text[text.Length - 1] == '.')
			{
				_text.text = text.TrimEnd('.');
			}
		}
	}
}
