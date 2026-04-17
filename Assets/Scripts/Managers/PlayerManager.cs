using System;
using System.Collections;
using Checkpoint;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public static event Action OnPlayerRespawn;
        public static PlayerManager Instance;

        [Header("Player")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform respawnPoint;
        [SerializeField] private float respawnDelay;
        public Player.Player player;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            if (respawnPoint == null) respawnPoint = FindFirstObjectByType<StartPoint>().transform;

            if (player == null) player = FindFirstObjectByType<Player.Player>();
            if (player == null) SpawnPlayerAtStart();
        }

        private void Update()
        {
            if (player && player.GetComponent<Rigidbody2D>().gravityScale == 0) 
            {
                player.GetComponent<Rigidbody2D>().gravityScale = 3.5f;  
            }
        }
        
        public void RespawnPlayer()
        {
            DifficultyManager difficultyManager = DifficultyManager.Instance;

            if (difficultyManager != null && difficultyManager.difficulty == DifficultyType.Hard) return;

            StartCoroutine(RespawnCoroutine());
        }

        public void UpdateRespawnPosition(Transform newRespawnPoint) => respawnPoint = newRespawnPoint;
        
        private IEnumerator RespawnCoroutine()
        {
            yield return new WaitForSeconds(respawnDelay);

            GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
            player = newPlayer.GetComponent<Player.Player>();
        
            OnPlayerRespawn?.Invoke();
        }

        private void SpawnPlayerAtStart()
        {
            // its never used but if there is an issue its prob here
            // DifficultyManager difficultyManager = DifficultyManager.Instance;
            StartCoroutine(RespawnCoroutine());
        }
    }
}
