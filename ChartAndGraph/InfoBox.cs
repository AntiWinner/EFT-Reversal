using UnityEngine;
using UnityEngine.UI;

namespace ChartAndGraph;

public class InfoBox : MonoBehaviour
{
	public GraphChartBase[] GraphChart;

	public Text infoText;

	private void _E000(GraphChartBase._E000 args)
	{
		infoText.text = ((args.Magnitude < 0f) ? string.Format(_ED3E._E000(238714), args.Category, args.XString, args.YString) : string.Format(_ED3E._E000(238626), args.Category, args.XString, args.YString, args.Magnitude));
	}

	private void _E001(GraphChartBase._E000 args)
	{
		infoText.text = ((args.Magnitude < 0f) ? string.Format(_ED3E._E000(238720), args.Category, args.XString, args.YString) : string.Format(_ED3E._E000(238688), args.Category, args.XString, args.YString, args.Magnitude));
	}

	private void _E002()
	{
		infoText.text = "";
	}

	public void HookChartEvents()
	{
		if (GraphChart == null)
		{
			return;
		}
		GraphChartBase[] graphChart = GraphChart;
		foreach (GraphChartBase graphChartBase in graphChart)
		{
			if (!(graphChartBase == null))
			{
				graphChartBase.PointClicked.AddListener(_E000);
				graphChartBase.PointHovered.AddListener(_E001);
				graphChartBase.NonHovered.AddListener(_E002);
			}
		}
	}

	private void Start()
	{
		HookChartEvents();
	}
}
