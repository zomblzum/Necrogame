using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class MinionFormationPoint : MonoBehaviour
{
    void OnDrawGizmos()
    {
        drawString(gameObject.name, transform.position, Color.black);
    }

    static public void drawString(string text, Vector3 worldPos, Color? colour = null)
    {
#if UNITY_EDITOR
        Handles.BeginGUI();

        var restoreColor = GUI.color;

        if (colour.HasValue) GUI.color = colour.Value;
        SceneView view = SceneView.currentDrawingSceneView;
        Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);

        if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
        {
            GUI.color = restoreColor;
            Handles.EndGUI();
            return;
        }

        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
        GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
        GUI.color = restoreColor;
        Handles.EndGUI();
#endif
    }
}
