using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace ChartAndGraph;

[Serializable]
public class ChartDivisionInfo : IInternalSettings
{
	public enum DivisionMessure
	{
		TotalDivisions,
		DataUnits
	}

	[SerializeField]
	[_ED12]
	[Tooltip("messure used to create divisions")]
	[_ED13]
	protected DivisionMessure messure;

	[_ED12]
	[Tooltip("data units per division")]
	[SerializeField]
	[_ED13]
	protected float unitsPerDivision;

	[SerializeField]
	[_ED12]
	[Tooltip("total division lines")]
	[_ED13]
	private int total = 3;

	[Tooltip("Material for the line of the division")]
	[_ED13]
	[_ED12]
	[SerializeField]
	private Material material;

	[Tooltip("Material tiling for the division lines. Use this to strech or tile the material along the line")]
	[_ED12]
	[_ED13]
	[SerializeField]
	private MaterialTiling materialTiling = new MaterialTiling(enable: false, 100f);

	[SerializeField]
	[Tooltip("The length of the far side of the division lines. This is used only by 3d chart when MarkDepth >0")]
	[_ED13]
	[_ED14]
	private AutoFloat markBackLength = new AutoFloat(automatic: true, 0.5f);

	[Tooltip("The length of the the division lines.")]
	[_ED13]
	[_ED12]
	[_ED14]
	[SerializeField]
	private AutoFloat markLength = new AutoFloat(automatic: true, 0.5f);

	[_ED14]
	[_ED13]
	[SerializeField]
	private AutoFloat markDepth = new AutoFloat(automatic: true, 0.5f);

	[_ED14]
	[_ED12]
	[SerializeField]
	[_ED13]
	[Tooltip("the thickness of the division lines")]
	private float markThickness = 0.1f;

	[SerializeField]
	[_ED14]
	[_ED12]
	[_ED13]
	[Tooltip("A prefab for the division labels")]
	private Text textPrefab;

	[Tooltip("prefix for the axis labels")]
	[_ED12]
	[_ED14]
	[SerializeField]
	[_ED13]
	private string textPrefix;

	[_ED12]
	[SerializeField]
	[_ED14]
	[Tooltip("suffix for the axis labels")]
	[_ED13]
	private string textSuffix;

	[_ED14]
	[Range(0f, 7f)]
	[_ED12]
	[Tooltip("the number of fraction digits in text labels")]
	[SerializeField]
	[_ED13]
	private int fractionDigits = 2;

	[SerializeField]
	[Tooltip("Label font size")]
	[_ED12]
	[_ED14]
	[_ED13]
	private int fontSize = 12;

	[Range(1f, 3f)]
	[_ED12]
	[_ED13]
	[Tooltip("makes the labels sharper if they are blurry")]
	[_ED14]
	[SerializeField]
	private float fontSharpness = 1f;

	[Tooltip("depth seperation the division lables")]
	[SerializeField]
	[_ED13]
	[_ED14]
	private float textDepth;

	[_ED14]
	[Tooltip("breadth seperation for the division labels")]
	[_ED12]
	[_ED13]
	[SerializeField]
	private float textSeperation;

	[_ED14]
	[_ED12]
	[_ED13]
	[SerializeField]
	[Tooltip("Alignment of the division lables")]
	private ChartDivisionAligment alignment = ChartDivisionAligment.Standard;

	[CompilerGenerated]
	private EventHandler OnDataUpdate;

	[CompilerGenerated]
	private EventHandler OnDataChanged;

	public int Total
	{
		get
		{
			return total;
		}
		set
		{
			RaiseOnChanged();
		}
	}

	public Material Material
	{
		get
		{
			return material;
		}
		set
		{
			material = value;
			RaiseOnChanged();
		}
	}

	public MaterialTiling MaterialTiling
	{
		get
		{
			return materialTiling;
		}
		set
		{
			materialTiling = value;
			RaiseOnChanged();
		}
	}

	public AutoFloat MarkBackLength
	{
		get
		{
			return markBackLength;
		}
		set
		{
			markBackLength = value;
			RaiseOnChanged();
		}
	}

	public AutoFloat MarkLength
	{
		get
		{
			return markLength;
		}
		set
		{
			markLength = value;
			RaiseOnChanged();
		}
	}

	public AutoFloat MarkDepth
	{
		get
		{
			return markDepth;
		}
		set
		{
			markDepth = value;
			RaiseOnChanged();
		}
	}

	public float MarkThickness
	{
		get
		{
			return markThickness;
		}
		set
		{
			markThickness = value;
			RaiseOnChanged();
		}
	}

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

	public string TextPrefix
	{
		get
		{
			return textPrefix;
		}
		set
		{
			textPrefix = value;
			RaiseOnChanged();
		}
	}

	public string TextSuffix
	{
		get
		{
			return textSuffix;
		}
		set
		{
			textSuffix = value;
			RaiseOnChanged();
		}
	}

	public int FractionDigits
	{
		get
		{
			return fractionDigits;
		}
		set
		{
			fractionDigits = value;
			RaiseOnChanged();
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
			RaiseOnChanged();
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
			RaiseOnChanged();
		}
	}

	public float TextDepth
	{
		get
		{
			return textDepth;
		}
		set
		{
			textDepth = value;
			RaiseOnChanged();
		}
	}

	public float TextSeperation
	{
		get
		{
			return textSeperation;
		}
		set
		{
			textSeperation = value;
			RaiseOnChanged();
		}
	}

	public ChartDivisionAligment Alignment
	{
		get
		{
			return alignment;
		}
		set
		{
			alignment = value;
			RaiseOnChanged();
		}
	}

	private event EventHandler _E000
	{
		[CompilerGenerated]
		add
		{
			EventHandler eventHandler = OnDataUpdate;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref OnDataUpdate, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler eventHandler = OnDataUpdate;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref OnDataUpdate, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	private event EventHandler _E001
	{
		[CompilerGenerated]
		add
		{
			EventHandler eventHandler = OnDataChanged;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref OnDataChanged, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler eventHandler = OnDataChanged;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref OnDataChanged, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	event EventHandler IInternalSettings.InternalOnDataUpdate
	{
		add
		{
			_E000 += value;
		}
		remove
		{
			_E000 -= value;
		}
	}

	event EventHandler IInternalSettings.InternalOnDataChanged
	{
		add
		{
			_E001 += value;
		}
		remove
		{
			_E001 -= value;
		}
	}

	public void ValidateProperites()
	{
		if (total < 0)
		{
			total = 0;
		}
		fontSharpness = Mathf.Clamp(fontSharpness, 1f, 3f);
		fontSize = Mathf.Max(fontSize, 0);
		fractionDigits = Mathf.Clamp(fractionDigits, 0, 7);
		markThickness = Mathf.Max(markThickness, 0f);
		materialTiling.TileFactor = Mathf.Max(materialTiling.TileFactor, 0f);
		textPrefix = ((textPrefix == null) ? "" : textPrefix);
		textSuffix = ((textSuffix == null) ? "" : textSuffix);
	}

	protected virtual float ValidateTotal(float total)
	{
		return total;
	}

	protected virtual void RaiseOnChanged()
	{
		if (OnDataChanged != null)
		{
			OnDataChanged(this, EventArgs.Empty);
		}
	}
}
