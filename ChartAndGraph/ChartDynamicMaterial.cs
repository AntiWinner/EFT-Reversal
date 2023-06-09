using System;
using UnityEngine;

namespace ChartAndGraph;

[Serializable]
public class ChartDynamicMaterial
{
	public Material Normal;

	public Color Hover;

	public Color Selected;

	public ChartDynamicMaterial()
		: this(null, Color.clear, Color.clear)
	{
	}

	public ChartDynamicMaterial(Material normal)
		: this(normal, Color.clear, Color.clear)
	{
	}

	public ChartDynamicMaterial(Material normal, Color hover, Color selected)
	{
		Normal = normal;
		Hover = hover;
		Selected = selected;
	}
}
