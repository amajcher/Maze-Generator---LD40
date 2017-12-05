using Assets.Scripts.Gameplay;
using Assets.Scripts.Procedural;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerView : Editor
{

    public override void OnInspectorGUI()
    {

        var game = target as GameManager;
        DrawDefaultInspector();
        if (GUILayout.Button("Build"))
        {
            game.GenerateLevel();
        }
    }
    
}
