using UnityEngine;

namespace Managers
{
    public class SkinManager : MonoBehaviour
    {
        [Header("Skin")]
        public int chosenSkinId;
        public static SkinManager Instance;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void SetSkinId(int id) => chosenSkinId = id;
        
        public int GetSkinId() => chosenSkinId;
    }
}
