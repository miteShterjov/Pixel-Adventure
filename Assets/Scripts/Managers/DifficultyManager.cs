using UnityEngine;

namespace Managers
{
    public class DifficultyManager : MonoBehaviour
    {
        public static DifficultyManager Instance;
        public DifficultyType difficulty;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void SetDifficulty(DifficultyType newDifficulty) => difficulty = newDifficulty;
        
        public void LoadDifficulty(int difficultyIndex) => difficulty = (DifficultyType) difficultyIndex;
    }

    public enum DifficultyType { Easy = 1, Normal, Hard}
}