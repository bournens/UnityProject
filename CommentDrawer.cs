using UnityEditor;
using UnityEngine;
public class CommentDrawer : PropertyAttribute
{
    public string comment;

    public CommentDrawer(string comment)
    {
        this.comment = comment;
    }
}
