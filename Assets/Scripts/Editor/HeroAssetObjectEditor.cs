
using System;
using Data.Hero;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[CustomEditor(typeof(HeroAssetObject))]
public class HeroAssetObjectEditor : Editor
{
    private static GUIStyle titleStyle;
    private static GUIStyle headerStyle;
    private static GUIStyle bodyStyle;

    private static int minHeath = 1;
    private static int maxHeath = 1;

    private static int minAttack = 1;
    private static int maxAttack = 1;

    private const string HeroSpritePath = "Assets/Sprites/Hero";

    public static void UpdateStyles()
    {
        if (bodyStyle == null)
        {
            bodyStyle = new GUIStyle(EditorStyles.label);
            bodyStyle.wordWrap = true;
            bodyStyle.fontSize = 14;

            titleStyle = new GUIStyle(bodyStyle);
            titleStyle.fontSize = 26;
            titleStyle.alignment = TextAnchor.MiddleCenter;

            headerStyle = new GUIStyle(bodyStyle);
            headerStyle.fontSize = 18 ;
        }
    }

    private void OnEnable()
    {
        var idObj = serializedObject.FindProperty("_id");
        if (string.IsNullOrEmpty(idObj.stringValue))
        {
            serializedObject.FindProperty("_id").stringValue = Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedProperties();
        }

    }

    public override void OnInspectorGUI()
    {

        UpdateStyles();

        EditorGUILayout.LabelField("---Hero----", titleStyle);
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Edit Hero Values", headerStyle);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("_attributes"),  GUIContent.none);
        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_viewAsset"),  GUIContent.none);
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Generate Random Hero Values", headerStyle);

        minHeath = Math.Max(1, EditorGUILayout.IntField("Minimum Health", minHeath));
        maxHeath = Math.Max(minHeath + 1, EditorGUILayout.IntField("Maximum Health", maxHeath));

        minAttack = Math.Max(1, EditorGUILayout.IntField("Minimum Attack Point", minAttack));
        maxAttack = Math.Max(minAttack + 1, EditorGUILayout.IntField("Maximum Attack Point", maxAttack));

        if (GUILayout.Button("Randomize"))
        {
            Randomize();
        }

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }

    private void Randomize()
    {
        var attributes = serializedObject.FindProperty("_attributes");
        attributes.FindPropertyRelative("_name").stringValue = HeroNameHelper.ReadRandomHeroName();
        attributes.FindPropertyRelative("_health").intValue = Random.Range(minHeath, maxHeath);
        attributes.FindPropertyRelative("_attack").intValue = Random.Range(minAttack, maxAttack);

        var view = serializedObject.FindProperty("_viewAsset");
        GetInternalRandomHeroPath(out string path);
        if (!string.IsNullOrEmpty(path))
        {
            Sprite heroSprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            view.FindPropertyRelative("_view").objectReferenceValue = heroSprite;
            view.FindPropertyRelative("_color").colorValue = new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            );
        }
        serializedObject.ApplyModifiedProperties();
    }

    private static bool GetInternalRandomHeroPath(out string fullPath)
    {
        var fileGUIDs = AssetDatabase.FindAssets(null, new[] { HeroSpritePath });

        if (fileGUIDs == null || fileGUIDs.Length == 0)
        {
            Debug.LogError($"Hero sprite not found or exist in the asset project path : {HeroSpritePath}");
            fullPath = String.Empty;
            return false;
        }

        fullPath = AssetDatabase.GUIDToAssetPath(fileGUIDs[Random.Range(0, fileGUIDs.Length)]);
        return true;
    }
}