using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayoutGroup : GridLayoutGroup
{
	public override float preferredWidth => 0f;

	public override float minWidth => 0f;

	public override void SetLayoutHorizontal()
	{
		if (m_Constraint == Constraint.FixedColumnCount)
		{
			float x = (base.rectTransform.rect.width - (float)(m_ConstraintCount - 1) * m_Spacing.x - (float)m_Padding.horizontal) / (float)m_ConstraintCount;
			base.cellSize = new Vector2(x, m_CellSize.y);
		}
		base.SetLayoutHorizontal();
	}
}
