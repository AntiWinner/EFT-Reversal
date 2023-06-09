using System;

namespace EFT;

[Serializable]
public class EnvironmentVariety
{
	public DistanceVarity[] Clips = new DistanceVarity[3]
	{
		new DistanceVarity(),
		new DistanceVarity(),
		new DistanceVarity()
	};

	public DistanceVarity this[int i]
	{
		get
		{
			return Clips[i];
		}
		set
		{
			Clips[i] = value;
		}
	}

	public int Length => Clips.Length;
}
