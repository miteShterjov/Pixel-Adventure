using Managers;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIMainMenu : MonoBehaviour
    {
        public string firstLevelName;

        [Header("UI Elements")]
        [SerializeField] private GameObject[] uiElements;
        [SerializeField] private GameObject continueButton;
        [Header("Interactive Camera")]
        [SerializeField] private MenuCharacter menuCharacter;
        [SerializeField] private CinemachineCamera cinemachine;
        [SerializeField] private Transform mainMenuPoint;
        [SerializeField] private Transform skinSelectionPoint;

        private UIFadeEffect fadeEffect;

        private void Awake()
        {
            fadeEffect = GetComponentInChildren<UIFadeEffect>();
        }

        private void Start()
        {
            if (HasLevelProgression())
                continueButton.SetActive(true);

            fadeEffect.ScreenFade(0, 1.5f);
        }

        private void Update()
        {
            if (PlayerPrefs.GetInt("ContinueLevelNumber", 0) == 0) continueButton.SetActive(false);
        }
            

        public void SwitchUI(GameObject uiToEnable)
        {
            foreach (GameObject ui in uiElements)
            {
                ui.SetActive(false);
            }

            uiToEnable.SetActive(true);

            AudioManager.Instance.PlaySfx(4);
        }

        public void NewGame()
        {
            fadeEffect.ScreenFade(1, 1.5f,LoadLevelScene);
            if (Time.timeScale == 0f) Time.timeScale = 1f;
            AudioManager.Instance.PlaySfx(4);
        }

        public void ContinueGame()
        {
            int difficultyIndex =  PlayerPrefs.GetInt("GameDifficulty",1);
            int levelToLoad = PlayerPrefs.GetInt("ContinueLevelNumber", 0);
            int lastSavedSkin = PlayerPrefs.GetInt("LastUsedSkin");

            SkinManager.Instance.SetSkinId(lastSavedSkin);

            DifficultyManager.Instance.LoadDifficulty(difficultyIndex);

            if (levelToLoad == 0)
            {
                continueButton.SetActive(false);
                return;
            }

            if (Time.timeScale == 0f) Time.timeScale = 1f;
            SceneManager.LoadScene("Level_" + levelToLoad);
            AudioManager.Instance.PlaySfx(4);
        }

        public void MoveCameraToMainMenu()
        {
            menuCharacter.MoveTo(mainMenuPoint);
            cinemachine.Follow = mainMenuPoint;
        }

        public void MoveCameraToSkinMenu()
        {
            menuCharacter.MoveTo(skinSelectionPoint);
            cinemachine.Follow = skinSelectionPoint;
        }
        
        private void LoadLevelScene() => SceneManager.LoadScene(firstLevelName);

        private bool HasLevelProgression()
        {
            bool hasLevelProgression = PlayerPrefs.GetInt("ContinueLevelNumber", 0) > 0;

            return hasLevelProgression;
        }
    }
}
