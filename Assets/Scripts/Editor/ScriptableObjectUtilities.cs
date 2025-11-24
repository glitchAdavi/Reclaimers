using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class ScriptableObjectUtilities : MonoBehaviour
{
    public static T CreateScriptableObject<T>(string path, string filename) where T : ScriptableObject
    {
        T file = ScriptableObject.CreateInstance<T>();

        if (!Directory.Exists(Application.dataPath + "/" + path))
            Directory.CreateDirectory(Application.dataPath + "/" + path);

        string finalPath = AssetDatabase.GenerateUniqueAssetPath("Assets/" + path + "/" + filename + ".asset");

        AssetDatabase.CreateAsset(file, finalPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();

        return file;
    }

    public static bool FindScriptableObjectByName<T>(string fileName, out T file) where T : ScriptableObject
    {
        file = null;

        string[] results = AssetDatabase.FindAssets(fileName);

        if (results.Length < 1) return false;

        file = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(results[0]));
        return true;
    }

    /// <summary>
    /// Finds and loads all assets at path beggining at Assets/.
    /// Returns true if it finds something, false otherwise.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="files"></param>
    /// <returns></returns>
    public static bool FindAllScriptableObjectsAtPath<T>(string path, out List<T> files) where T : ScriptableObject
    {
        files = null;

        string[] allFiles = Directory.GetFiles(Application.dataPath + "/" + path, "*.asset");

        if (allFiles.Length < 1) return false;

        files = new List<T>();

        foreach (string absolutePath in allFiles)
        {
            string relativePath;
            if (absolutePath.StartsWith(Application.dataPath))
            {
                relativePath = "Assets" + absolutePath.Substring(Application.dataPath.Length);
                files.Add(AssetDatabase.LoadAssetAtPath<T>(relativePath));
            }
        }

        return true;
    }
}
