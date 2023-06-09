using Comfort.Common;

namespace EFT.Hideout;

public class WaterCollectorBehaviour : _E831, _E830
{
	public _E80E ResourceConsumer
	{
		get
		{
			if (Singleton<_E815>.Instance.ProductionController.GetProducer(base.Data) is _E824 obj)
			{
				return obj.ResourceConsumer;
			}
			return null;
		}
	}
}
