using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;
        //Handles.color = Color.white;
        //Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.veiwRadius);
        //Vector3 viewAngleA = fow.DirFormAngle(-fow.veiwAngle / 2, false);
        //Vector3 viewAngleB = fow.DirFormAngle(fow.veiwAngle / 2, false);
        //
        //Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.veiwRadius);
        //Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.veiwRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fow.visibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
        }
    }
}
