using System.Runtime.CompilerServices;
using EFT.MovingPlatforms;
using UnityEngine;

namespace EFT.Interactive;

public class CommonTransportee : MonoBehaviour, MovingPlatform._E000
{
	private Transform _E000;

	[CompilerGenerated]
	private MovingPlatform _E001;

	public MovingPlatform Platform
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
		[CompilerGenerated]
		set
		{
			_E001 = value;
		}
	}

	public Transform ParentTransform
	{
		get
		{
			if (!(_E000 == null))
			{
				return _E000;
			}
			return base.transform;
		}
		set
		{
			_E000 = value;
		}
	}

	public void Board(MovingPlatform platform)
	{
		Platform = platform;
		ParentTransform.parent = platform.transform;
	}

	public void GetOff(MovingPlatform platform)
	{
		if (!(Platform != platform))
		{
			Platform = null;
			ParentTransform.parent = null;
		}
	}
}
