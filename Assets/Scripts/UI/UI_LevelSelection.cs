using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UILevelSelection : MonoBehaviour
    {
        [Header("Level buttons")]
        [SerializeField] private UILevelButton buttonPrefab;
        [SerializeField] private Transform buttonsParent;
        [SerializeField] private bool[] levelsUnlocked;


        private void Start()
        {
            LoadLevelsInfo();
            CreateLevelButtons();
        }

        private void CreateLevelButtons()
        {
            int levelsAmount = SceneManager.sceneCountInBuildSettings - 1;

            for (int i = 1; i < levelsAmount; i++)
            {
                if (IsLevelUnlocked(i) == false)
                    return;

                UILevelButton newButton = Instantiate(buttonPrefab, buttonsParent);
                newButton.SetupButton(i);
            }
        }

        private bool IsLevelUnlocked(int levelIndex) => levelsUnlocked[levelIndex];

        private void LoadLevelsInfo()
        {
            int levelsAmount = SceneManager.sceneCountInBuildSettings - 1;

            levelsUnlocked = new bool[levelsAmount];

            for (int i = 1; i < levelsAmount; i++)
            {
                bool levelUnlocked = PlayerPrefs.GetInt("Level" + i + "Unlocked", 0) == 1;
                if (levelUnlocked) levelsUnlocked[i] = true;
            }
            levelsUnlocked[1] = true;
        }
    }
}
