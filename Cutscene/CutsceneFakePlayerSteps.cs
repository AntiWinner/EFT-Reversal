using System.Collections;
using EFT;
using UnityEngine;

namespace Cutscene;

public class CutsceneFakePlayerSteps : MonoBehaviour
{
	[SerializeField]
	private float _timeBetweenSteps;

	[SerializeField]
	private int _maxStepsCount = -1;

	private Coroutine m__E000;

	private Player m__E001;

	private int _E002;

	private void OnDestroy()
	{
		ToggleFakeStepsSound(null, toggle: false);
	}

	public void ToggleFakeStepsSound(Player onPlayer, bool toggle)
	{
		if (!toggle)
		{
			if (this.m__E000 != null)
			{
				StopCoroutine(this.m__E000);
				this.m__E000 = null;
			}
		}
		else if (this.m__E000 == null)
		{
			_E002 = 0;
			this.m__E001 = onPlayer;
			this.m__E000 = StartCoroutine(_E001());
		}
	}

	private void _E000()
	{
		_E002++;
		if (_maxStepsCount == -1 || _maxStepsCount >= _E002)
		{
			this.m__E001.PlayStepSound();
		}
	}

	private IEnumerator _E001()
	{
		while (true)
		{
			_E000();
			float num = 0f;
			while (num <= 1f)
			{
				num += Time.deltaTime / _timeBetweenSteps;
				yield return null;
			}
			yield return null;
		}
	}
}
