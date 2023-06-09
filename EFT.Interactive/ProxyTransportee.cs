using EFT.MovingPlatforms;
using UnityEngine;

namespace EFT.Interactive;

public class ProxyTransportee : MonoBehaviour, MovingPlatform._E000
{
	public MovingPlatform._E000 Parent;

	public void Board(MovingPlatform platform)
	{
		Parent.Board(platform);
	}

	public void GetOff(MovingPlatform platform)
	{
		Parent.GetOff(platform);
	}
}
