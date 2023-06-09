using UnityEngine;

public class DressItem : MonoBehaviour
{
	public GameObject LootPrefab;

	public GameObject DressPrefab;

	public void EnableLoot(bool on)
	{
		LootPrefab.SetActive(on);
		DressPrefab.SetActive(!on);
	}
}
