using System.Collections;
using UnityEngine;

namespace EFT.UI.SessionEnd;

public class SessionExperiencePanel : MonoBehaviour
{
	private const float m__E000 = 4f;

	[SerializeField]
	private PlayerExperiencePanel _playerExperiencePanel;

	private int _E001;

	private float _E002;

	private float _E003;

	public void Set(int baseExperience, int newExperience)
	{
		_E001 = baseExperience;
		_E003 = _E001;
		_E002 = (float)newExperience / 4f;
		StartCoroutine(_E000(baseExperience + newExperience));
	}

	private IEnumerator _E000(int target)
	{
		float num = _E002 * Time.deltaTime;
		while ((float)target - _E003 > num)
		{
			_E003 += _E002 * Time.deltaTime;
			_playerExperiencePanel.Set(_E001, Mathf.RoundToInt(_E003));
			yield return null;
			num = _E002 * Time.deltaTime;
		}
		_playerExperiencePanel.Set(_E001, target);
	}
}
