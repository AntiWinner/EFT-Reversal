using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UI.Hideout;

namespace EFT.Hideout;

[Serializable]
public sealed class RelatedQteData
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public QteData.EQteActivityType type;

		internal bool _E000(QteHandleData d)
		{
			return d.Type == type;
		}
	}

	public List<QteHandleData> Data = new List<QteHandleData>();

	public void UpdateBackend(QteData[] backendQteData)
	{
		if (!backendQteData.IsNullOrEmpty())
		{
			for (int i = 0; i < backendQteData.Length; i++)
			{
				QteHandleData item = new QteHandleData(backendQteData[i]);
				Data.Add(item);
			}
		}
	}

	public void UpdateClient(RelatedQteData backendQteData)
	{
		if (backendQteData.Data.IsNullOrEmpty())
		{
			return;
		}
		foreach (QteHandleData datum in backendQteData.Data)
		{
			QteData.EQteActivityType type = datum.Type;
			(Data.FirstOrDefault((QteHandleData d) => d.Type == type) ?? throw new Exception(string.Format(_ED3E._E000(165734), type))).Update(datum);
		}
	}
}
