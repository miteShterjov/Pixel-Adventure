using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("Level Management")]
        [SerializeField] private float levelTimer;
        [SerializeField] private int currentLevelIndex;
        [SerializeField] private int nextLevelIndex;
        [Header("Fruits Management")]
        public bool fruitsAreRandom;
        public int fruitsCollected;
        public int totalFruits;
        public Transform fruitParent;
        [Header("Checkpoints")]
        public bool canReactivate;
        [Header("Managers")]
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private SkinManager skinManager;
        [SerializeField] private DifficultyManager difficultyManager;
        [SerializeField] private ObjectCreator objectCreator;
        
        private UIInGame inGameUI;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            inGameUI = UIInGame.Instance;

            currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
            nextLevelIndex = currentLevelIndex + 1;

            CollectFruitsInfo();
            CreateManagersIfNeeded();
        }

        private void Update()
        {
            levelTimer += Time.deltaTime;

            inGameUI.UpdateTimerUI(levelTimer);
        }
        
        public void AddFruit()
        {
            fruitsCollected++;
            inGameUI.UpdateFruitUI(fruitsCollected,totalFruits);
        }

        public void RemoveFruit()
        {
            fruitsCollected--;
            inGameUI.UpdateFruitUI(fruitsCollected, totalFruits);
        }

        public int FruitsCollected() => fruitsCollected;

        public bool FruitsHaveRandomLook() => fruitsAreRandom;

        public void LevelFinished()
        {
            SaveLevelProgression();
            SaveBestTime();
            SaveFruitsInfo();
            LoadNextScene();
        }
        
        public void RestartLevel() => UIInGame.Instance.FadeEffect.ScreenFade(1, .75f, LoadCurrentScene);
        
        private void CreateManagersIfNeeded()
        {
            if (AudioManager.Instance == null) Instantiate(audioManager);
            if (PlayerManager.Instance == null) Instantiate(playerManager);
            if(SkinManager.Instance == null) Instantiate(skinManager);
            if(DifficultyManager.Instance == null) Instantiate(difficultyManager);
            if(ObjectCreator.Instance == null) Instantiate(objectCreator);
        }

        private void CollectFruitsInfo()
        {
            Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
            totalFruits = allFruits.Length;

            inGameUI.UpdateFruitUI(fruitsCollected, totalFruits);

            PlayerPrefs.SetInt("Level" + currentLevelIndex + "TotalFruits", totalFruits);
        }

        [ContextMenu("Parent All Fruits")]
        private void ParentAllTheFruits()
        {
            if (fruitParent == null) return;

            Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);

            foreach (Fruit fruit in allFruits) fruit.transform.parent = fruitParent;
        }
        
        private void SaveFruitsInfo()
        {
            int fruitsCollectedBefore = PlayerPrefs.GetInt("Level" + currentLevelIndex + "FruitsCollected");
            if(fruitsCollectedBefore < fruitsCollected) PlayerPrefs.SetInt("Level" + currentLevelIndex + "FruitsCollected",fruitsCollected);
            int totalFruitsInBank = PlayerPrefs.GetInt("TotalFruitsAmount");
            PlayerPrefs.SetInt("TotalFruitsAmount", totalFruitsInBank + fruitsCollected);
        }
        
        private void SaveBestTime()
        {
            float lastTime = PlayerPrefs.GetFloat("Level" + currentLevelIndex + "BestTime", 99);

            if(levelTimer < lastTime) PlayerPrefs.SetFloat("Level" + currentLevelIndex + "BestTime", levelTimer);
        }
        
        private void SaveLevelProgression()
        {
            PlayerPrefs.SetInt("Level" + nextLevelIndex + "Unlocked", 1);

            if (NoMoreLevels()) return;
            PlayerPrefs.SetInt("ContinueLevelNumber", nextLevelIndex);

            SkinManager skinManagerInstance = SkinManager.Instance;
            
            if(skinManagerInstance != null) PlayerPrefs.SetInt("LastUsedSkin", skinManagerInstance.GetSkinId());
        }

        private void LoadCurrentScene() => SceneManager.LoadScene("Level_" + currentLevelIndex);
        
        private void LoadTheEndScene() => SceneManager.LoadScene("TheEnd");
        
        private void LoadNextLevel() => SceneManager.LoadScene("Level_" + nextLevelIndex);
        
        private void LoadNextScene()
        {
            UIFadeEffect fadeEffect = UIInGame.Instance.FadeEffect;

            if (NoMoreLevels()) fadeEffect.ScreenFade(1, 1.5f, LoadTheEndScene);
            else fadeEffect.ScreenFade(1, 1.5f, LoadNextLevel);
        }
        private bool NoMoreLevels()
        {
            int lastLevelIndex = SceneManager.sceneCountInBuildSettings - 2;
            bool noMoreLevels = currentLevelIndex == lastLevelIndex;

            return noMoreLevels;
        }
    }
}
