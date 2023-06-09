using EFT.Game.Spawning;
using UnityEngine;

public class SpawnResultDebug : MonoBehaviour
{
	private ESpawnCategory _E000;

	public void Init(_E626._E001 data)
	{
		_E000 = data.Category;
		base.transform.position = data.Position;
	}

	private void OnDrawGizmosSelected()
	{
		float a = 0.9f;
		switch (_E000)
		{
		case ESpawnCategory.Boss:
			Gizmos.color = new Color(0.2f, 0.4f, 0.9f, a);
			break;
		case ESpawnCategory.Player:
			Gizmos.color = new Color(0.9f, 0.4f, 0.2f, a);
			break;
		case ESpawnCategory.Bot:
			Gizmos.color = new Color(0.4f, 0.9f, 0.2f, a);
			break;
		case ESpawnCategory.Coop:
			Gizmos.color = new Color(0f, 0f, 0f, a);
			break;
		default:
			Gizmos.color = new Color(0.4f, 0.4f, 0.4f, 0.5f);
			break;
		}
		_E395.DrawCube(base.transform.position + Vector3.up * 150f * 0.5f, base.transform.rotation, new Vector3(0.7f, 150f, 0.7f));
	}
}
