using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIInGame : MonoBehaviour
    {
        public static UIInGame Instance;
        public UIFadeEffect FadeEffect { get; private set; }

        [Header("UI elements")]
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI fruitText;
        [SerializeField] private GameObject pauseUI;

        private bool isPaused;

        private void Awake()
        {
            Instance = this;

            FadeEffect = GetComponentInChildren<UIFadeEffect>();
        }

        private void Start()
        {
            FadeEffect.ScreenFade(0, 1);
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame) PauseButton();
        }

        public void GoToMainMenuButton() => SceneManager.LoadScene("MainMenu");
        
        public void UpdateFruitUI(int collectedFruits, int totalFruits) => fruitText.text = collectedFruits + " / " + totalFruits;
        
        public void UpdateTimerUI(float timer) => timerText.text = timer.ToString("00");
        
        private void PauseButton()
        {
            if (isPaused)
            {
                isPaused = false;
                Time.timeScale = 1;
                pauseUI.SetActive(false);
            }
            else
            {
                isPaused = true;
                Time.timeScale = 0;
                pauseUI.SetActive(true);
            }
        }
    }
}
