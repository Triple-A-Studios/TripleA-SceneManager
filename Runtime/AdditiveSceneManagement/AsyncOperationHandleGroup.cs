using System.Collections.Generic;
using System.Linq;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace TripleA.SceneManagement.AdditiveSceneManagement
{
	public readonly struct AsyncOperationHandleGroup 
	{
		public readonly List<AsyncOperationHandle<SceneInstance>> handles;

		public float Progress => handles.Count == 0 ? 0 : handles.Average(h => h.PercentComplete);

		public bool IsDone => handles.All(h => h.IsDone);

		public AsyncOperationHandleGroup(int initialCapacity)
		{
			handles = new List<AsyncOperationHandle<SceneInstance>>(initialCapacity);
		}
	};
}