using System;
using System.Linq;
using Eflatun.SceneReference;
using System.Collections.Generic;

namespace TripleA.SceneManagement
{
    [Serializable]
    public abstract class SceneGroup<T> where T : Enum
    {
        public string groupName = "New Scene Group";
        public List<SceneData<T>> scenes;

        public string FindSceneNameByType(T s) => scenes.FirstOrDefault(scene => scene.sceneType.Equals(s))?.SceneName;
    }

    [Serializable]
    public class SceneData<T> where T : Enum
    {
        public SceneReference sceneReference;
        public string SceneName => sceneReference.Name;
        public T sceneType;
    }
}

