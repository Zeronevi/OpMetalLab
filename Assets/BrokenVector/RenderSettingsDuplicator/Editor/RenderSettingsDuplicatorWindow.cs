using UnityEngine;
using UnityEditor;

namespace BrokenVector.RenderSettingsDuplicator
{
    public class RenderSettingsDuplicatorWindow : EditorWindow
    {
        private static RenderSettingsSnapshot copiedSnapshot;

        [MenuItem(Constants.WINDOW_PATH), MenuItem(Constants.WINDOW_PATH_ALT)]
        private static void OpenWindow()
        {
            CreateWindow();
        }

        private static void CreateWindow()
        {
            var window = EditorWindow.GetWindow<RenderSettingsDuplicatorWindow>();
            #if UNITY_5_1_OR_NEWER
            window.titleContent = new GUIContent(Constants.WINDOW_TITLE);
            #else
            window.title = Constants.WINDOW_TITLE;
            #endif
            window.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Copy RenderSettings from current scene"))
            {
                copiedSnapshot = RenderSettingsSnapshot.MakeSnapshot();
            }

            if (copiedSnapshot != null && GUILayout.Button("Apply copied settings to current scene"))
            {
                RenderSettingsSnapshot.ApplySnapshot(copiedSnapshot);
            }
        }
    }
}
