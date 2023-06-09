using System;

[Serializable]
public class RundomRange
{
	public float Min;

	public float Range;

	private bool _random;

	public float Value
	{
		get
		{
			if (!_random)
			{
				return Min;
			}
			return Min + _E8EE.Float() * Range;
		}
	}

	public void Init()
	{
		_random = Range > float.Epsilon;
	}
}
