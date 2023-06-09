using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class InteractionButton : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _EC3F interaction;

		public _EC3E action;

		public InteractionButton _003C_003E4__this;

		internal void _E000()
		{
			bool flag = interaction.SelectedAction == action;
			_003C_003E4__this._image.color = (flag ? _E0C9 : _E0CB);
			_003C_003E4__this._text.color = (flag ? _E0CB : (action.Disabled ? _E0CA : _E0C9));
		}
	}

	[SerializeField]
	private CustomTextMeshProUGUI _text;

	[SerializeField]
	private Image _image;

	private static readonly Color _E0C9 = new Color(0.514f, 0.514f, 0.514f, 1f);

	private static readonly Color _E0CA = new Color(0.25f, 0.25f, 0.25f, 1f);

	private static readonly Color _E0CB = new Color(0f, 0f, 0f, 1f);

	public void Show(_EC3F interaction, _EC3E action)
	{
		ShowGameObject();
		_text.text = action.Name.Localized().ToUpper();
		UI.BindEvent(interaction.CurrentActionChanged, delegate
		{
			bool flag = interaction.SelectedAction == action;
			_image.color = (flag ? _E0C9 : _E0CB);
			_text.color = (flag ? _E0CB : (action.Disabled ? _E0CA : _E0C9));
		});
	}
}
