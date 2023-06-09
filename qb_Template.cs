using System;
using UnityEngine;

[Serializable]
public class qb_Template
{
	public bool live;

	public string brushName = string.Empty;

	public float brushRadius = 0.5f;

	public float brushRadiusMin = 0.2f;

	public float brushRadiusMax = 5f;

	public float brushSpacing = 0.2f;

	public float brushSpacingMin = 0.02f;

	public float brushSpacingMax = 2f;

	public float scatterRadius;

	public bool alignToNormal = true;

	public bool flipNormalAlign;

	public bool alignToStroke = true;

	public bool flipStrokeAlign;

	public Vector3 rotationRangeMin = new Vector3(0f, 0f, 0f);

	public Vector3 rotationRangeMax = new Vector3(0f, 0f, 0f);

	public Vector3 positionOffset = Vector3.zero;

	public float scaleMin = 0.1f;

	public float scaleMax = 3f;

	public Vector3 scaleRandMin = new Vector3(1f, 1f, 1f);

	public Vector3 scaleRandMax = new Vector3(1f, 1f, 1f);

	public float scaleRandMinUniform = 1f;

	public float scaleRandMaxUniform = 1f;

	public bool scaleUniform = true;

	public bool paintToSelection;

	public bool paintToLayer;

	public int layerIndex;

	public bool groupObjects;

	public int groupIndex;

	public bool eraseByGroup;

	public bool eraseBySelected;

	public qb_PrefabObject[] prefabGroup = new qb_PrefabObject[0];

	public bool dirty;
}
