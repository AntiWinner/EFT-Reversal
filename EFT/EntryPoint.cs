using UnityEngine;
using UnityEngine.SceneManagement;

namespace EFT;

public sealed class EntryPoint : MonoBehaviour
{
	private void Awake()
	{
		string selectedGame = new _E777().SelectedGame;
		if (!(selectedGame == _ED3E._E000(134011)))
		{
			if (selectedGame == _ED3E._E000(144990))
			{
				SceneManager.LoadScene(_E785.ArenMainScene.rcid);
			}
		}
		else
		{
			SceneManager.LoadScene(_E785.EftMainScene.rcid);
		}
	}
}
