using UnityEngine;

namespace Prism.Utils;

public class PrismAnimCurveCreator : MonoBehaviour
{
	public float[] curvePointsX;

	public float[] curvePointsY;

	public AnimationCurve thisCurve;

	[ContextMenu("Generate curve")]
	private void _E000()
	{
		thisCurve = new AnimationCurve();
		for (int i = 0; i < curvePointsX.Length; i++)
		{
			thisCurve.AddKey(curvePointsX[i], curvePointsY[i]);
		}
	}
}
