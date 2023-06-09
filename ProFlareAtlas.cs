using System;
using System.Collections.Generic;
using UnityEngine;

public class ProFlareAtlas : MonoBehaviour
{
	[Serializable]
	public class Element
	{
		public string name = _ED3E._E000(88246);

		public Rect UV = new Rect(0f, 0f, 1f, 1f);

		public bool Imported;
	}

	public Texture2D texture;

	public int elementNumber;

	public bool editElements;

	[SerializeField]
	public List<Element> elementsList = new List<Element>();

	public string[] elementNameList;

	public void UpdateElementNameList()
	{
		elementNameList = new string[elementsList.Count];
		for (int i = 0; i < elementNameList.Length; i++)
		{
			elementNameList[i] = elementsList[i].name;
		}
	}
}
