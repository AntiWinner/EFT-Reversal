using RootMotion.FinalIK;
using UnityEngine;

namespace EFT;

public class HitReactionCharacter : MonoBehaviour
{
	[SerializeField]
	private string mixingAnim;

	[SerializeField]
	private Transform recursiveMixingTransform;

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private HitReaction hitReaction;

	[SerializeField]
	private float hitForce = 1f;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo = default(RaycastHit);
			if (Physics.Raycast(ray, out hitInfo, 100f))
			{
				hitReaction.Hit(hitInfo.collider, ray.direction * hitForce, hitInfo.point);
				Debug.Log(_ED3E._E000(182070));
			}
		}
	}
}
