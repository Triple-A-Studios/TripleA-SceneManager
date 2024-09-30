using UnityEngine;
using UnityEngine.SceneManagement;

namespace TripleA.Samples.AsyncSceneLoader
{
	public class Bootstrapper : MonoBehaviour
	{
		// make a persistent singleton
		private static Bootstrapper m_s_instance;
		public static Bootstrapper Instance
		{
			get
			{
				if (m_s_instance == null)
				{
					m_s_instance = GameObject.FindObjectOfType<Bootstrapper>();
				}

				return m_s_instance;
			}
		}

		private void Awake()
		{
			if (m_s_instance == null)
			{
				m_s_instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Init()
		{
			Debug.Log("BootStrapper Init...");
			SceneManager.LoadScene("AsyncSceneLoader", LoadSceneMode.Single);
		}
	}
}