using Managers;
using UnityEngine;

namespace UI
{
    public class UIDifficulty : MonoBehaviour
    {
        private DifficultyManager difficultyManager;

        private void Start()
        {
            difficultyManager = DifficultyManager.Instance;
        }

        public void SetEasyMode() => difficultyManager.SetDifficulty(DifficultyType.Easy);
        public void SetNormalMode() => difficultyManager.SetDifficulty(DifficultyType.Normal);
        public void SetHardMode() => difficultyManager.SetDifficulty(DifficultyType.Hard);
    }
}
