using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TripleA.SceneManagement;

namespace TripleA.Samples.AsyncSceneLoader
{
    public class AsyncSceneLoader : MonoBehaviour
    {
        [SerializeField] private Image m_fill;
        [SerializeField] private float m_fillSpeed = 0.5f;
        [SerializeField] private Canvas m_canvas;
        [SerializeField] private Camera m_cam;
        [SerializeField] private MySceneGroup[] m_sceneGroup;

        private float m_targetProgress;
        private bool m_isLoading;
        
        private MySceneGroupManager m_sceneGroupManager;

        private void Awake()
        {
            m_sceneGroupManager = new(new List<string>{"AsyncSceneLoader"});
            m_sceneGroupManager.OnSceneGroupLoaded += () => Debug.Log("Scene Group Loaded");
            m_sceneGroupManager.OnSceneUnloaded += sceneName => Debug.Log($"Unloaded : {sceneName}");
            m_sceneGroupManager.OnSceneLoaded += sceneName => Debug.Log($"Loaded : {sceneName}");
        }

        private async void Start()
        {
            await LoadSceneGroup(0);
        }

        private void Update()
        {
            if (!m_isLoading) return;
            
            float currentFillAmount = m_fill.fillAmount;
            float progressDifference = Mathf.Abs(currentFillAmount - m_targetProgress);

            float dynamicFillSpeed = progressDifference * m_fillSpeed;
            
            m_fill.fillAmount = Mathf.Lerp(currentFillAmount, m_targetProgress, Time.deltaTime * dynamicFillSpeed);
        }

        public async Task LoadSceneGroup(int index)
        {
            m_fill.fillAmount = 0f;
            m_targetProgress = 1f;

            if (index < 0 || index >= m_sceneGroup.Length)
            {
                Debug.LogError($"Invalid Scene Group Index: {index}");
                return;
            }

            LoadingProgress progress = new();
            progress.Progressed += target => m_targetProgress = Mathf.Max(target, m_targetProgress);
            
            EnableLoadingCanvas();

            await m_sceneGroupManager.LoadScenes(m_sceneGroup[index], progress, MySceneTypes.ActiveScene);

            EnableLoadingCanvas(false);
        }

        private void EnableLoadingCanvas(bool enable = true)
        {
            m_isLoading = enable;
            m_cam.gameObject.SetActive(enable);
            m_canvas.gameObject.SetActive(enable);
        }
    }
}
