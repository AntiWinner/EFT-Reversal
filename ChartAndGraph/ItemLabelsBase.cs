using System;
using UnityEngine;
using UnityEngine.UI;

namespace ChartAndGraph;

[Serializable]
public abstract class ItemLabelsBase : ChartSettingItemBase, ISerializationCallbackReceiver
{
	[Tooltip("This prefab will be used to create all the text associated with the chart. If the prefab is null no labels will be shown")]
	[SerializeField]
	private Text textPrefab;

	[Tooltip(" determine the formatting of the label data. when the values are available , you can use the predefined macros : '\\n' for newline , '<?category>' for the current category and '<?group>' for the current group")]
	[SerializeField]
	private TextFormatting textFormat = new TextFormatting();

	[Tooltip("the font size for the labels")]
	[SerializeField]
	private int fontSize = 14;

	[Range(1f, 3f)]
	[SerializeField]
	[Tooltip("adjusts the sharpness of the font")]
	private float fontSharpness = 1f;

	[Tooltip("the seperation of each label from it's origin")]
	[SerializeField]
	private float seperation = 1f;

	[Tooltip("the location of the label relative to the item")]
	[SerializeField]
	private ChartOrientedSize location = new ChartOrientedSize(0f, 0f);

	public Text TextPrefab
	{
		get
		{
			return textPrefab;
		}
		set
		{
			textPrefab = value;
			RaiseOnChanged();
		}
	}

	public TextFormatting TextFormat
	{
		get
		{
			return textFormat;
		}
		set
		{
			textFormat = value;
			RaiseOnUpdate();
		}
	}

	public int FontSize
	{
		get
		{
			return fontSize;
		}
		set
		{
			fontSize = value;
			RaiseOnUpdate();
		}
	}

	public float FontSharpness
	{
		get
		{
			return fontSharpness;
		}
		set
		{
			fontSharpness = value;
			RaiseOnUpdate();
		}
	}

	public float Seperation
	{
		get
		{
			return seperation;
		}
		set
		{
			seperation = value;
			RaiseOnUpdate();
		}
	}

	public ChartOrientedSize Location
	{
		get
		{
			return location;
		}
		set
		{
			location = value;
			RaiseOnUpdate();
		}
	}

	protected ItemLabelsBase()
	{
		_E000();
	}

	private void _E000()
	{
		if (textFormat != null)
		{
			AddInnerItem(textFormat);
		}
	}

	public virtual void ValidateProperties()
	{
		fontSize = Mathf.Max(fontSize, 0);
		fontSharpness = Mathf.Clamp(fontSharpness, 1f, 3f);
	}

	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		_E000();
	}
}
