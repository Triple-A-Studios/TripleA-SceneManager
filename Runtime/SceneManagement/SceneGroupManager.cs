using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace TripleA.SceneManagement
{
    public abstract class SceneGroupManager<T> where T : Enum
    {
        /// <summary>
        ///     Fired when a scene is loaded.
        /// </summary>
        public event Action<string> OnSceneLoaded = delegate { };
        
        /// <summary>
        ///     Fired when a scene is unloaded.
        /// </summary>
        public event Action<string> OnSceneUnloaded = delegate { };
        
        /// <summary>
        ///     Fired when a scene group is loaded.
        /// </summary>
        public event Action OnSceneGroupLoaded = delegate { };

        private SceneGroup<T> m_activeSceneGroup;

        /// <summary>
        ///     Loads all the scenes in a scene group.
        /// </summary>
        /// <param name="group">Group of scenes to load</param>
        /// <param name="progress">Progress callback.</param>
        /// <param name="activeSceneType">The type of the active scene in the group.</param>
        /// <param name="reloadDupScenes">If true, duplicate scenes will be reloaded.</param>
        public virtual async Task LoadScenes(SceneGroup<T> group, IProgress<float> progress, T activeSceneType, bool reloadDupScenes = false)
        {
            m_activeSceneGroup = group;

            var loadedScenes = new List<string>();

            await UnloadScenes();

            int sceneCount = SceneManager.sceneCount;

            for (int i = 0; i < sceneCount; i++)
            {
                loadedScenes.Add(SceneManager.GetSceneAt(i).name);
            }

            var totalScenesToLoad = m_activeSceneGroup.scenes.Count;
            var operationGroup = new AsyncOperationGroup(totalScenesToLoad);

            for (int i = 0; i < totalScenesToLoad; i++)
            {
                var sceneData = group.scenes[i];
                if (!reloadDupScenes && loadedScenes.Contains(sceneData.SceneName)) continue;

                var operation = SceneManager.LoadSceneAsync(sceneData.sceneReference.Path, LoadSceneMode.Additive);

                operationGroup.operations.Add(operation);

                OnSceneLoaded?.Invoke(sceneData.SceneName);
            }

            while (!operationGroup.IsDone)
            {
                progress?.Report(operationGroup.Progress);
                await Task.Delay(100);
            }

            Scene activeScene = SceneManager.GetSceneByName(m_activeSceneGroup.FindSceneNameByType(activeSceneType));

            if (activeScene.IsValid())
            {
                SceneManager.SetActiveScene(activeScene);
            }

            OnSceneGroupLoaded?.Invoke();

        }

        /// <summary>
        ///     Unloads all the scenes in the scene group.
        /// </summary>
        /// <param name="doNotUnloadScenes">List of scenes to not unload.</param>
        public virtual async Task UnloadScenes(List<string> doNotUnloadScenes = null)
        {
            var scenes = new List<string>();
            string activeScene = SceneManager.GetActiveScene().name;
            int sceneCount = SceneManager.sceneCount;

            for (int i = 0; i < sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded) continue;

                string sceneName = scene.name;
                if (sceneName.Equals(activeScene)) continue;

                if (doNotUnloadScenes != null && doNotUnloadScenes.Contains(sceneName)) continue;

                scenes.Add(sceneName);
            }
            
            var operationGroup = new AsyncOperationGroup(scenes.Count);

            foreach (var scene in scenes)
            {
                var operation = SceneManager.UnloadSceneAsync(scene);
                if (operation == null) continue;
                
                operationGroup.operations.Add(operation);
                
                OnSceneUnloaded?.Invoke(scene);
            }

            while (!operationGroup.IsDone)
            {
                await Task.Delay(100);
            }
        }
    }
}
