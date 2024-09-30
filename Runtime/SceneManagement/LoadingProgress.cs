using System;

namespace TripleA.SceneManagement
{
	public class LoadingProgress : IProgress<float>
	{
		public event Action<float> Progressed = delegate { };
		private const float k_ratio = 1f;

		public void Report(float value)
		{
			Progressed?.Invoke(value / k_ratio);
		}
	}
}