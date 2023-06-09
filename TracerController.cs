using UnityEngine;

public class TracerController : MonoBehaviour
{
	private TracerSystem _E000;

	private void Start()
	{
		_E000 = _E3AA.FindUnityObjectOfType<TracerSystem>();
	}

	public void DrawTracers(_EC26 shotResults)
	{
		if (shotResults.PositionHistory.Count != 0)
		{
			for (int i = 1; i < shotResults.PositionHistory.Count; i++)
			{
				Vector3 start = shotResults.PositionHistory[i - 1];
				Vector3 end = shotResults.PositionHistory[i];
				_E000.Add(start, end, Color.red, 0.01f, 0.01f);
			}
		}
	}
}
