using TripleA.SceneManagement;

namespace TripleA.Samples.AsyncSceneLoader
{
	public enum MySceneTypes
	{
		Scene1,
		Scene2,
		Scene3,
		Scene4,
		ActiveScene,
		Bootstrapper,
	}
	
	[System.Serializable]
	public class MySceneGroup : SceneGroup<MySceneTypes>
	{
        
	}
}