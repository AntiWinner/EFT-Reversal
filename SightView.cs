using UnityEngine;

public class SightView : MonoBehaviour
{
	public GameObject[] open;

	public GameObject[] close;

	public void Open()
	{
		GameObject[] array = open;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: true);
		}
		array = close;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
	}

	public void Close()
	{
		GameObject[] array = open;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
		array = close;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: true);
		}
	}
}
