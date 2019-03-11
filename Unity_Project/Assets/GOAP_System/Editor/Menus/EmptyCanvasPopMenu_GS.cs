﻿using UnityEditor;
using UnityEngine;
using GOAP_S.AI;

namespace GOAP_S.UI
{
    public sealed class EmptyCanvasPopMenu_GS : PopupWindowContent
    {
        //Constructors ================
        public EmptyCanvasPopMenu_GS()
        {

        }

        //Loop Methods ================
        public override void OnGUI(Rect rect)
        {
            GUILayout.BeginVertical();

            //Set window size
            if (Selection.activeGameObject != null)
            {
                editorWindow.minSize = new Vector2(180.0f, 115.0f);
                editorWindow.maxSize = new Vector2(180.0f, 115.0f);
            }
            else
            {
                editorWindow.minSize = new Vector2(180.0f, 85.0f);
                editorWindow.maxSize = new Vector2(180.0f, 85.0f);
            }

            //Menu title
            GUILayout.BeginHorizontal("Box");
            GUILayout.FlexibleSpace();
            GUILayout.Label("Agent Canvas Menu", UIConfig_GS.Instance.select_menu_title_style, GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            //Selected gameobject case
            if (Selection.activeGameObject != null)
            {
                //Add agent button
                if (GUILayout.Button(new GUIContent("Add Agent", "Add agent to the selected gameobject"),
                    GUILayout.ExpandWidth(true),
                    GUILayout.Height(25)))
                {
                    //Add a new agent component to the selected gameobject
                    Selection.activeGameObject.AddComponent<Agent_GS>();
                    //Close this window
                    editorWindow.Close();
                }
            }

            //Close agent canvas button
            if (GUILayout.Button(new GUIContent("Close Agent Canvas", "Close GoapSystem agent canvas"),
                GUILayout.ExpandWidth(true),
                GUILayout.Height(25)))
            {
                //Close node editor
                NodeEditor_GS.Instance.Close();
                NodePlanning_GS.Instance.Close();
                //Close this window
                editorWindow.Close();
            }

            GUILayout.EndVertical();
        }
    }
}
