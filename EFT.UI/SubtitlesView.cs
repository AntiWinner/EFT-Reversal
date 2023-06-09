using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace EFT.UI;

[UsedImplicitly]
public sealed class SubtitlesView : UIElement
{
	[SerializeField]
	private TMP_Text _textField;

	private ESubtitlesSource _E066;

	private readonly _E3A4 _E21E = new _E3A4();

	private readonly HashSet<_E787._E000> _E21F = new HashSet<_E787._E000>();

	public void Show(ESubtitlesSource source)
	{
		HideGameObject();
		UI.Dispose();
		_E066 = source;
		UI.AddDisposable(_E21E);
		UI.AddDisposable(_EBAF.Instance.SubscribeOnEvent<_EBC3>(_E000));
	}

	private void _E000(_EBC3 subtitlesEvent)
	{
		_E21E.Dispose();
		if (subtitlesEvent.SubtitlesSource == _E066)
		{
			_E001(subtitlesEvent.LocalizationKey).HandleExceptions();
		}
	}

	private async Task _E001(string localizationKey)
	{
		_E787 obj = new _E787(localizationKey);
		foreach (_E787._E000 currentLine in obj.CurrentLines)
		{
			_E002(currentLine);
		}
		TaskCompletionSource taskCompletionSource = new TaskCompletionSource();
		obj.OnNewCCLine += _E002;
		obj.OnSubtitlesEnded += taskCompletionSource.TryComplete;
		_E21E.AddDisposable(taskCompletionSource.TryComplete);
		_E21E.AddDisposable(obj);
		await taskCompletionSource.Task;
		obj.OnNewCCLine -= _E002;
		obj.OnSubtitlesEnded -= taskCompletionSource.TryComplete;
		obj.Dispose();
		_E21F.Clear();
		_textField.text = string.Empty;
	}

	private void _E002(_E787._E000 line)
	{
		_E003(line).HandleExceptions();
	}

	private async Task _E003(_E787._E000 line)
	{
		if (_E21F.Add(line))
		{
			_E004();
			if (await line.Task.Await(_E21E.CancellationToken))
			{
				_E21F.Remove(line);
				_E004();
			}
		}
	}

	private void _E004()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (_E787._E000 item in _E21F)
		{
			stringBuilder.AppendLine(item.Line.Text);
		}
		string text = stringBuilder.ToString();
		_textField.text = text;
		if (string.IsNullOrEmpty(text))
		{
			HideGameObject();
		}
		else
		{
			ShowGameObject();
		}
	}
}
