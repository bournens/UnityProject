using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CommentDrawer))]
public class CommentDrawerEditor : PropertyDrawer
{
    // Define a unique ID for the foldout state per property
    private string foldoutId;

    // Custom GUIStyle for larger text in the HelpBox
    private GUIStyle commentStyle;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Get the comment from the CommentDrawer attribute
        CommentDrawer commentAttribute = (CommentDrawer)attribute;
        string comment = commentAttribute.comment;

        // Get the unique ID for the foldout state based on the property's name
        foldoutId = property.name;

        // Check if the group is folded out or not
        bool foldout = EditorPrefs.GetBool(foldoutId, false);

        EditorGUI.BeginProperty(position, label, property);

        // Calculate the position for the foldout arrow
        Rect foldoutPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        // Draw the collapsible header
        foldout = EditorGUI.Foldout(foldoutPosition, foldout, label, true);

        // Save the foldout state to EditorPrefs
        EditorPrefs.SetBool(foldoutId, foldout);

        // Calculate the position for the property field
        Rect propertyFieldPosition = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);

        if (foldout)
        {
            // Indent the variables inside the group
            EditorGUI.indentLevel++;

            // Draw the property fields for the variables inside the group
            EditorGUI.PropertyField(propertyFieldPosition, property, true);

            // Initialize the custom GUIStyle with larger text
            if (commentStyle == null)
            {
                commentStyle = new GUIStyle(EditorStyles.label);
                commentStyle.fontSize = 13; // Set the font size to 12 (you can adjust as needed)
                commentStyle.wordWrap = true;
                commentStyle.normal.textColor = Color.white; // Set the text color to white
            }

            // Calculate the height of the comment text based on the font size
            GUIContent commentContent = new GUIContent(comment);
            float commentHeight = commentStyle.CalcHeight(commentContent, position.width);

            // Center the comment text vertically within the HelpBox
            Rect commentPosition = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 2.0f, position.width, commentHeight);

            // Draw the HelpBox background
            GUI.Box(commentPosition, GUIContent.none, EditorStyles.helpBox);

            // Draw the comment text using the custom GUIStyle
            EditorGUI.LabelField(commentPosition, comment, commentStyle);

            // Restore the indent level
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Check if the group is folded out or not
        bool foldout = EditorPrefs.GetBool(foldoutId, false);

        if (foldout)
        {
            // Calculate the height of the comment text based on the font size
            GUIContent commentContent = new GUIContent(((CommentDrawer)attribute).comment);
            float commentHeight = commentStyle.CalcHeight(commentContent, EditorGUIUtility.currentViewWidth);

            // Increase the property height to make space for the comment description and collapsible group,
            // and add extra padding to create some space between the comment and the next variable
            return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight * 3.0f + commentHeight;
        }
        else
        {
            // Only show the property field height without the comment description
            return base.GetPropertyHeight(property, label);
        }
    }
}
