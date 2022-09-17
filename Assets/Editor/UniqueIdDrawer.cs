using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using System;

// Place this file inside Assets/Editor
[CustomPropertyDrawer(typeof(UniqueIdentifierAttribute))]
public class UniqueIdentifierDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		// Place a label so it can't be edited by accident
		Rect textFieldPosition = position;
		textFieldPosition.height = 16;
		DrawLabelField(textFieldPosition, prop, label);

		Rect test = new Rect(position.x+140, position.y, 65, position.height-2);

		if (GUI.Button(test, "Generate"))
		{
			Guid guid = Guid.NewGuid();
			prop.stringValue = guid.ToString();
		}
	}

	void DrawLabelField(Rect position, SerializedProperty prop, GUIContent label)
	{
		EditorGUI.LabelField(position, label, new GUIContent(prop.stringValue));
	}
}