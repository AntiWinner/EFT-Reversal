using System.IO;
using UnityEngine;

public class MaterialStencilReplacer : MonoBehaviour
{
	[HideInInspector]
	public int StencilType;

	public SearchOption SearchOption = SearchOption.AllDirectories;
}
