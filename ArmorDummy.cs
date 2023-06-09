using Comfort.Common;
using EFT.Ballistics;
using UnityEngine;

public class ArmorDummy : MonoBehaviour
{
	[Header("Armor class [1..10]")]
	public int ArmorClass = 4;

	public float Durability;

	public int MaxDurability;

	public int MaxHealth;

	public float Health;

	public Renderer[] Body;

	public CustomTextMeshProUGUI PopupText;

	public RectTransform ArmorImage;

	public RectTransform HealthImage;

	public BallisticCollider Coolider;

	public Transform PivotBars;

	public RectTransform Panel;

	private void Start()
	{
		Coolider.OnHitAction += CooliderOnOnHitAction;
	}

	public void CooliderOnOnHitAction(_EC23 damageInfo)
	{
		float damage = damageInfo.Damage;
		float penetrationPower = damageInfo.PenetrationPower;
		float num = -1f;
		float num2 = Durability / (float)MaxDurability * 100f;
		float num3 = Singleton<_E5CB>.Instance.Armor.GetArmorClass(ArmorClass).Resistance;
		float num4 = (136f - 5000f / (36f + num2)) * num3 * 0.01f;
		float num5 = ((num4 >= penetrationPower + 15f) ? 0f : ((num4 >= penetrationPower) ? (0.4f * (num4 - penetrationPower - 15f) * (num4 - penetrationPower - 15f)) : (100f + penetrationPower / (0.9f * num4 - penetrationPower))));
		if (num5 - Random.Range(0f, 100f) >= 0f)
		{
			num = damageInfo.PenetrationPower * damageInfo.ArmorDamage * Mathf.Clamp(num3 / penetrationPower, 0.5f, 0.9f);
			damage = damageInfo.Damage * (0.8f + Mathf.Clamp((penetrationPower - num4) / 100f, -0.2f, 0.2f));
		}
		else
		{
			num = damageInfo.PenetrationPower * damageInfo.ArmorDamage * Mathf.Clamp(num3 / penetrationPower, 0.6f, 1.1f);
			damage = 1f;
		}
		Durability -= num;
		if (Durability < 0f)
		{
			Durability = 0f;
		}
		Health -= damage;
		PopupText.text = string.Format(_ED3E._E000(49073), (int)damage, (int)num, (int)num5);
		if (Health <= 0f)
		{
			Renderer[] body = Body;
			for (int i = 0; i < body.Length; i++)
			{
				body[i].material.color = Color.red;
			}
			PopupText.text += _ED3E._E000(49140);
			Health = MaxHealth;
			Durability = MaxDurability;
		}
		else
		{
			Renderer[] body = Body;
			for (int i = 0; i < body.Length; i++)
			{
				body[i].material.color = Color.white;
			}
		}
		ArmorImage.sizeDelta = new Vector2(296f * Durability / (float)MaxDurability, ArmorImage.sizeDelta.y);
		HealthImage.sizeDelta = new Vector2(296f * Health / (float)MaxHealth, HealthImage.sizeDelta.y);
	}

	private void Update()
	{
		PopupText.transform.parent.gameObject.SetActive(Body[0].isVisible);
		if (Camera.main != null)
		{
			Vector2 vector = Camera.main.WorldToViewportPoint(base.transform.position + Vector3.up / 2f);
			PopupText.rectTransform.anchorMin = vector;
			PopupText.rectTransform.anchorMax = vector;
			Vector2 vector2 = Camera.main.WorldToViewportPoint(PivotBars.position);
			Panel.anchorMax = vector2;
			Panel.anchorMin = vector2;
		}
	}
}
