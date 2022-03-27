using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class AutoSave : EditorWindow {
    public float saveTime = 600;
    public float nextSave = 0;

    [MenuItem("Window/AutoSave")]

    private static void Init() {
        AutoSave window = (AutoSave)EditorWindow.GetWindowWithRect(typeof(AutoSave), new Rect(0, 0, 250, 50));
        window.Show();
    }

    private void OnGUI() {
        EditorGUILayout.LabelField("Save Each:", saveTime + "s");
        int timeToSave = (int)(nextSave - EditorApplication.timeSinceStartup);
        EditorGUILayout.LabelField("Next Save:", timeToSave.ToString() + "s");
        Repaint();

        if (EditorApplication.timeSinceStartup > nextSave && !EditorApplication.isPlaying) {
            string[] path = EditorSceneManager.GetActiveScene().path.Split(char.Parse("/"));
            path[path.Length - 1] = path[path.Length - 1];
            bool saveSuccess = EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), string.Join("/", path));
            Debug.Log(saveSuccess ? "Scene Autosave." : "Error saving scene.");
            nextSave = (float)EditorApplication.timeSinceStartup + saveTime;
        }
    }
}