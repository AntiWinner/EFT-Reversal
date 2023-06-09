using UnityEngine;

public class qb_Group : MonoBehaviour
{
	public string groupName;

	private bool _E000;

	private bool _E001;

	public void AddObject(GameObject newObject)
	{
		newObject.transform.parent = base.transform;
	}

	public void Hide()
	{
		_E000 = false;
	}

	public void Show()
	{
		_E000 = true;
	}

	public void Freeze()
	{
		_E001 = true;
	}

	public void UnFreeze()
	{
		_E001 = false;
	}

	public void CleanUp()
	{
		Object.DestroyImmediate(base.gameObject);
	}
}
