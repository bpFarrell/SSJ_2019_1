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
        if (GUILayout.Button("Process")) {
            DirectoryCheck();
            Texture2D tex = new Texture2D(256, 256);
            Color[] color = tex.GetPixels();
            for (int y = 0; y < tex.width; y++) {
                if (y > 42 && y < tex.width - 42) continue;
                for (int x = 0; x < tex.height; x++) {
                    color[x + (y * tex.width)] = Color.black;
                }
            }
            tex.SetPixels(color);
            byte[] data = tex.EncodeToPNG();

            for (int i = 0; i < json.Count; i++) {
                CardDefinition card = CreateInstance<CardDefinition>();
                card.FromJSON(json[i]);

                AssetDatabase.CreateAsset(card, "Assets/CardDefinitions/" + card.assetName + ".asset");

                if (!File.Exists(Application.dataPath + "/CardDefinitions/Resources/" + card.assetName + ".png")) {
                    File.Create(Application.dataPath + "/CardDefinitions/Resources/" + card.assetName + ".png").Close();
                    File.WriteAllBytes(Application.dataPath + "/CardDefinitions/Resources/" + card.assetName + ".png", data);
                }
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
        if (!Directory.Exists(Application.dataPath + "/CardDefinitions/Resources/"))
            Directory.CreateDirectory(Application.dataPath + "/CardDefinitions/Resources/");
    }
}

/*
 * "name": "Basic shot",
 * "type": "P",
 * "cost": 1,
 * "resourcename": "basicShot"
*/
