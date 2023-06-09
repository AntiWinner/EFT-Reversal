using System;
using UnityEngine;

namespace EFT.UI.Matchmaker;

public class LocationPath : UIElement
{
	public void Show(_E554.Location source, _E554.Location destination)
	{
		ShowGameObject();
		Vector2 relativeMapPos = source.RelativeMapPos;
		Vector2 vector = destination.RelativeMapPos - relativeMapPos;
		float magnitude = vector.magnitude;
		float z = (float)(Math.Atan2(vector.y, vector.x) * 180.0 / Math.PI);
		RectTransform obj = (RectTransform)base.transform;
		obj.anchorMin = relativeMapPos;
		obj.anchorMax = new Vector2(relativeMapPos.x + magnitude, relativeMapPos.y);
		obj.rotation = Quaternion.Euler(0f, 0f, z);
	}
}
