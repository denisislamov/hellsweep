using UnityEditor;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Utilities.Editor
{
	[CustomPropertyDrawer(typeof(LayerAttribute))]
	internal class LayerAttributeEditor : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			property.intValue = EditorGUI.LayerField(position, label,  property.intValue);
		}
	}
}