using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
#if UNITY_5_3_OR_NEWER
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
#endif

namespace BrokenVector.RenderSettingsDuplicator
{
    [System.Serializable]
	public class RenderSettingsSnapshot
    {
        public Object RenderSettings;
        public Object LightmapSettings;

        public static RenderSettingsSnapshot MakeSnapshot()
        {
            var snap = new RenderSettingsSnapshot();

            snap.RenderSettings = GameObject.Instantiate(GetRenderSettings());
            snap.LightmapSettings = GameObject.Instantiate(GetLightmapSettings());

            return snap;
        }

        public void ApplySnapshot()
        {
            ApplySnapshot(this);
        }

        public static void ApplySnapshot(RenderSettingsSnapshot snap)
        {
            EditorUtility.CopySerialized(snap.RenderSettings, GetRenderSettings());
            EditorUtility.CopySerialized(snap.LightmapSettings, GetLightmapSettings());

            foreach (EditorWindow window in Resources.FindObjectsOfTypeAll<EditorWindow>())
                window.Repaint();

            #if UNITY_5_3_OR_NEWER
            EditorSceneManager.MarkSceneDirty(SceneManager.GetSceneAt(0));
            #endif
        }

        private static Object GetRenderSettings() 
        {
            return typeof(RenderSettings).GetMethod("GetRenderSettings", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null) as Object;
        }

        private static Object GetLightmapSettings() 
        {
            return typeof(LightmapEditorSettings).GetMethod("GetLightmapSettings", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null) as Object;
        }

    }
}