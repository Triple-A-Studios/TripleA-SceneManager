using UnityEditor;
using UnityEngine;
using TripleA.Samples.AsyncSceneLoader;

namespace TripleA.SceneManager.Editor
{
    [CustomEditor(typeof(AsyncSceneLoader))]
    public class AsyncSceneLoaderEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            AsyncSceneLoader sceneLoader = (AsyncSceneLoader) target;

            if (EditorApplication.isPlaying && GUILayout.Button("Load First Scene Group")) {
                LoadSceneGroup(sceneLoader, 0);
            }
            
            if (EditorApplication.isPlaying && GUILayout.Button("Load Second Scene Group")) {
                LoadSceneGroup(sceneLoader, 1);
                
            }
            if (EditorApplication.isPlaying && GUILayout.Button("Load Third Scene Group")) {
                LoadSceneGroup(sceneLoader, 2);
            }
        }

        static async void LoadSceneGroup(AsyncSceneLoader sceneLoader, int index) {
            await sceneLoader.LoadSceneGroup(index);
        }
    }
}
