using System.Collections.Generic;
using System.Threading.Tasks;
using TripleA.SceneManagement;

namespace TripleA.Samples.AsyncSceneLoader
{
	public class MySceneGroupManager : SceneGroupManager<MySceneTypes>
	{
		private List<string> m_doNotUnloadScenes;

		public MySceneGroupManager(List<string> doNotUnloadScenes = null)
		{
			m_doNotUnloadScenes = new List<string>();

			if (doNotUnloadScenes != null)
			{
				m_doNotUnloadScenes.AddRange(doNotUnloadScenes);
			}
		}

		public override async Task UnloadScenes(List<string> doNotUnloadScenes = null)
		{
			int index = m_doNotUnloadScenes.Count;
			int count = -1;
			if (doNotUnloadScenes != null)
			{
				count = doNotUnloadScenes.Count;
				m_doNotUnloadScenes.AddRange(doNotUnloadScenes);
			}
			await base.UnloadScenes(m_doNotUnloadScenes);
			if (count != -1) m_doNotUnloadScenes.RemoveRange(index, count);
		}
	}
}