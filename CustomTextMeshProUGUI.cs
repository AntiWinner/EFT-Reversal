using System;
using TMPro;
using UnityEngine;

[Obsolete("Use TextMeshProUGUI instead")]
public class CustomTextMeshProUGUI : TextMeshProUGUI
{
	public void SetFontStyle(FontStyle style)
	{
		FontStyles fontStyles = FontStyles.Normal;
		switch (style)
		{
		case FontStyle.Bold:
			fontStyles = FontStyles.Bold;
			break;
		case FontStyle.Italic:
			fontStyles = FontStyles.Italic;
			break;
		case FontStyle.BoldAndItalic:
			fontStyles = FontStyles.Italic;
			break;
		}
		base.fontStyle = fontStyles;
	}
}
