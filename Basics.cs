using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Basics : MonoBehaviour
{
	public Transform redCube;

	public Transform greenCube;

	public Transform blueCube;

	public Transform purpleCube;

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(1f);
		redCube.DOMove(new Vector3(0f, 4f, 0f), 2f);
		greenCube.DOMove(new Vector3(0f, 4f, 0f), 2f).From();
		blueCube.DOMove(new Vector3(0f, 4f, 0f), 2f).SetRelative();
		purpleCube.DOMove(new Vector3(6f, 0f, 0f), 2f).SetRelative();
		purpleCube.GetComponent<Renderer>().material.DOColor(Color.yellow, 2f).SetLoops(-1, LoopType.Yoyo);
	}
}
