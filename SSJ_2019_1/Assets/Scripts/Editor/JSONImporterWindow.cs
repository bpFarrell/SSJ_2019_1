using UnityEditor;
using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using System.IO;

public class JSONImporterWindow : EditorWindow
{
    private string text;
    private int count;
    private JSONNode json;
    private List<CardDefinition> cardList = new List<CardDefinition>();

    [MenuItem("Game/JSONImporter")]
    static void Init() {
        JSONImporterWindow window = (JSONImporterWindow)EditorWindow.GetWindow(typeof(JSONImporterWindow));
        window.Show();
    }

    private void OnGUI() {
        text = EditorGUILayout.TextArea(text);
        if (GUILayout.Button("Validate")) {
            json = JSON.Parse(text)["data"];
        }
        bool valid = (json != null && json.IsArray);
        EditorGUI.BeginDisabledGroup(!valid);
        if (GUILayout.Button("Clear")) {
            cardList.Clear();
        }
        GUILayout.Space(20f);
        if (GUILayout.Button("Process"))
        {
            DirectoryCheck();
            for (int i = 0; i < json.Count; i++) {
                CardDefinition card = CreateInstance<CardDefinition>();
                card.FromJSON(json[i]);

                AssetDatabase.CreateAsset(card, "Assets/CardDefinitions/" + card.assetName + ".asset");

                Texture2D tex = new Texture2D(256, 256);
                byte[] data = tex.EncodeToPNG();

                if (!File.Exists(Application.dataPath + "/CardDefinitions/" + card.assetName + ".png")) File.Create(Application.dataPath + "/CardDefinitions/" + card.assetName + ".png").Close();
                File.WriteAllBytes(Application.dataPath + "/CardDefinitions/" + card.assetName + ".png", data);
            }
        }
        GUILayout.Space(20f);
        for (int i = 0; i < cardList.Count; i++) {
            EditorGUILayout.LabelField(cardList[i].name);
        }
        EditorGUI.EndDisabledGroup();
    }
    private void DirectoryCheck() {
        if (!Directory.Exists(Application.dataPath + "/CardDefinitions/"))
            Directory.CreateDirectory(Application.dataPath + "/CardDefinitions/");
    }
}

/*
 * "name": "Basic shot",
 * "type": "P",
 * "cost": 1,
 * "resourcename": "basicShot"
*/
