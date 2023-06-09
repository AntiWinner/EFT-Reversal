using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CustomPluginExampleBrain : MonoBehaviour
{
	public Text txtCustomRange;

	private _E3AD m__E000 = new _E3AD(0f, 10f);

	private static _E3AE m__E001 = new _E3AE();

	private void Start()
	{
		DOTween.To(CustomPluginExampleBrain.m__E001, () => this.m__E000, delegate(_E3AD x)
		{
			this.m__E000 = x;
		}, new _E3AD(20f, 100f), 4f);
	}

	private void Update()
	{
		txtCustomRange.text = this.m__E000.min + _ED3E._E000(2540) + this.m__E000.max;
	}

	[CompilerGenerated]
	private _E3AD _E000()
	{
		return this.m__E000;
	}

	[CompilerGenerated]
	private void _E001(_E3AD x)
	{
		this.m__E000 = x;
	}
}
