using UnityEngine;

public class UBHelper : MonoBehaviour
{
	private class _E000
	{
		public GUIStyle header = _ED3E._E000(36021);

		public GUIStyle headerArrow = _ED3E._E000(36001);

		private RectOffset m__E000;

		private float _E001;

		private Vector2 _E002;

		private TextAnchor _E003;

		private FontStyle _E004;

		private int _E005;

		internal _E000()
		{
			header.font = new GUIStyle(_ED3E._E000(53836)).font;
		}

		public void Backup()
		{
			m__E000 = UBHelper.m__E000.header.border;
			_E001 = UBHelper.m__E000.header.fixedHeight;
			_E002 = UBHelper.m__E000.header.contentOffset;
			_E003 = UBHelper.m__E000.header.alignment;
			_E004 = UBHelper.m__E000.header.fontStyle;
			_E005 = UBHelper.m__E000.header.fontSize;
		}

		public void Apply()
		{
			UBHelper.m__E000.header.border = new RectOffset(7, 7, 4, 4);
			UBHelper.m__E000.header.fixedHeight = 22f;
			UBHelper.m__E000.header.contentOffset = new Vector2(20f, -2f);
			UBHelper.m__E000.header.alignment = TextAnchor.MiddleLeft;
			UBHelper.m__E000.header.fontStyle = FontStyle.Bold;
			UBHelper.m__E000.header.fontSize = 12;
		}

		public void Revert()
		{
			UBHelper.m__E000.header.border = m__E000;
			UBHelper.m__E000.header.fixedHeight = _E001;
			UBHelper.m__E000.header.contentOffset = _E002;
			UBHelper.m__E000.header.alignment = _E003;
			UBHelper.m__E000.header.fontStyle = _E004;
			UBHelper.m__E000.header.fontSize = _E005;
		}
	}

	private static _E000 m__E000;

	static UBHelper()
	{
		UBHelper.m__E000 = new _E000();
	}

	public static bool GroupHeader(string text, bool isExpanded)
	{
		Rect rect = GUILayoutUtility.GetRect(16f, 22f, UBHelper.m__E000.header);
		UBHelper.m__E000.Backup();
		UBHelper.m__E000.Apply();
		if (Event.current.type == EventType.Repaint)
		{
			UBHelper.m__E000.header.Draw(rect, text, isExpanded, isExpanded, isExpanded, isExpanded);
		}
		Event current = Event.current;
		if (current.type == EventType.MouseDown && rect.Contains(current.mousePosition))
		{
			isExpanded = !isExpanded;
			current.Use();
		}
		UBHelper.m__E000.Revert();
		return isExpanded;
	}
}
