using UnityEngine;
using UnityEngine.UI;

public class BattleUINoiseLevel : MonoBehaviour
{
	public Image Image;

	public Sprite[] Sprites;

	public void SetNoiseLevel(int index)
	{
		if (index < 0 || index >= Sprites.Length)
		{
			Debug.LogErrorFormat(_ED3E._E000(48301), index);
		}
		else
		{
			Image.sprite = Sprites[index];
		}
	}
}
