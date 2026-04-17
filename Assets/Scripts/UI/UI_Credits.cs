using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UICredits : MonoBehaviour
    {
        [Header("Credits Scrolling")]
        [SerializeField] private RectTransform rectT;
        [SerializeField] private float scrollSpeed = 200;
        [SerializeField] private float offScreenPosition = 1800;
        [SerializeField] private string mainMenuSceneName = "MainMenu";
        
        private bool creditsSkipped;
        private UIFadeEffect fadeEffect;

        private void Awake()
        {
            fadeEffect = GetComponentInChildren<UIFadeEffect>();
            if (!fadeEffect) Debug.LogError("No Fade Effect on Credits Screen");
            fadeEffect.ScreenFade(0, 1.5f);
        }

        private void Update()
        {
            rectT.anchoredPosition += Vector2.up * (scrollSpeed * Time.deltaTime);

            if (rectT.anchoredPosition.y > offScreenPosition) GoToMainMenu();
        }

        public void SkipCredits()
        {
            if (creditsSkipped == false)
            {
                scrollSpeed *= 10;
                creditsSkipped = true;
            }
            else
            {
                GoToMainMenu();
            }
        }

        private void GoToMainMenu() => fadeEffect.ScreenFade(1, 1, SwitchToMenuScene);

        private void SwitchToMenuScene() => SceneManager.LoadScene(mainMenuSceneName);
    }
}