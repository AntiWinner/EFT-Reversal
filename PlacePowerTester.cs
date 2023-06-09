using UnityEngine;

public class PlacePowerTester : MonoBehaviour
{
	public float LastPower;

	public float Do(Vector3 p)
	{
		BotZone botZone = _E000(p);
		if (botZone == null)
		{
			return 0f;
		}
		return _E115.GetPower(botZone, p);
	}

	private BotZone _E000(Vector3 p)
	{
		BotZone[] array = Object.FindObjectsOfType<BotZone>();
		float num = float.MaxValue;
		BotZone result = null;
		BotZone[] array2 = array;
		foreach (BotZone botZone in array2)
		{
			BoxCollider[] componentsInChildren = botZone.gameObject.GetComponentsInChildren<BoxCollider>();
			Vector3 zero = Vector3.zero;
			BoxCollider[] array3 = componentsInChildren;
			foreach (BoxCollider boxCollider in array3)
			{
				Vector3 vector = boxCollider.transform.position + boxCollider.center;
				zero += vector;
			}
			float sqrMagnitude = (zero / componentsInChildren.Length - p).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				result = botZone;
				num = sqrMagnitude;
			}
		}
		return result;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0.4f, 0.9f, 0.2f, 0.8f);
		float num = (LastPower = Do(base.transform.position)) / 20f;
		Vector3 vector = Vector3.up * num * 0.5f;
		Vector3 vector2 = base.transform.position + vector;
		Gizmos.DrawWireSphere(vector2 - vector, 8f);
		_E395.DrawCube(vector2, base.transform.rotation, new Vector3(1f, num, 1f));
	}
}
