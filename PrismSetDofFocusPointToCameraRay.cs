using UnityEngine;

[RequireComponent(typeof(PrismEffects))]
[ExecuteInEditMode]
public class PrismSetDofFocusPointToCameraRay : MonoBehaviour
{
	public float lerpSpeed = 2f;

	public float sphereRadius = 0.2f;

	public GameObject currentHitObject;

	private void Update()
	{
		if (Physics.Raycast(base.transform.position, base.transform.forward, out var hitInfo))
		{
			PrismEffects component = GetComponent<PrismEffects>();
			component.dofFocusPoint = Mathf.MoveTowards(component.dofFocusPoint, Vector3.Distance(base.transform.position, hitInfo.point), Time.deltaTime * lerpSpeed);
			currentHitObject = hitInfo.collider.gameObject;
			Debug.DrawRay(base.transform.position, base.transform.TransformDirection(base.transform.forward) * hitInfo.distance);
		}
	}
}
