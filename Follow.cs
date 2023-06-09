using DG.Tweening;
using UnityEngine;

public class Follow : MonoBehaviour
{
	public Transform target;

	private Vector3 _E000;

	private Tweener _E001;

	private void Start()
	{
		_E001 = base.transform.DOMove(target.position, 2f).SetAutoKill(autoKillOnCompletion: false);
		_E000 = target.position;
	}

	private void Update()
	{
		if (!(_E000 == target.position))
		{
			_E001.ChangeEndValue(target.position, snapStartValue: true).Restart();
			_E000 = target.position;
		}
	}
}
