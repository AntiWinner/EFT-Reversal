using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Sequences : MonoBehaviour
{
	public Transform cube;

	public float duration = 4f;

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(1f);
		Sequence sequence = DOTween.Sequence();
		sequence.Append(cube.DOMoveX(6f, duration).SetRelative().SetEase(Ease.InOutQuad));
		sequence.Insert(0f, cube.DORotate(new Vector3(0f, 45f, 0f), duration / 2f).SetEase(Ease.InQuad).SetLoops(2, LoopType.Yoyo));
		sequence.Insert(duration / 2f, cube.GetComponent<Renderer>().material.DOColor(Color.yellow, duration / 2f));
		sequence.SetLoops(-1, LoopType.Yoyo);
	}
}
