using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UISkinSelection : MonoBehaviour
    {
        private UILevelSelection levelSelectionUI;
        private UIMainMenu mainMenuUI;
        [SerializeField] private Skin[] skinList;

        [Header("UI details")]
        [SerializeField] private int skinIndex;
        [SerializeField] private int maxIndex;
        [SerializeField] private Animator skinDisplay;

        [SerializeField] private TextMeshProUGUI buySelectText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI bankText;

        private void Start()
        {
            LoadSkinUnlocks();
            UpdateSkinDisplay();

            mainMenuUI = GetComponentInParent<UIMainMenu>();
            levelSelectionUI = mainMenuUI.GetComponentInChildren<UILevelSelection>(true);
        }

        private void LoadSkinUnlocks()
        {
            for (int i = 0; i < skinList.Length; i++)
            {
                string skinName = skinList[i].skinName;
                bool skinUnlocked = PlayerPrefs.GetInt(skinName + "Unlocked", 0) == 1;

                if(skinUnlocked || i == 0)
                    skinList[i].unlocked = true;
            }
        }

        public void SelectSkin()
        {
            if (skinList[skinIndex].unlocked == false)
                BuySkin(skinIndex);
            else
            {
                SkinManager.Instance.SetSkinId(skinIndex);
                mainMenuUI.SwitchUI(levelSelectionUI.gameObject);
            }

            AudioManager.Instance.PlaySfx(4);

            UpdateSkinDisplay();
        }

        public void NextSkin()
        {
            skinIndex++;

            if (skinIndex > maxIndex)
                skinIndex = 0;

            AudioManager.Instance.PlaySfx(4);

            UpdateSkinDisplay();
        }

        public void PreviousSkin()
        {
            skinIndex--;

            if (skinIndex < 0)
                skinIndex = maxIndex;

            AudioManager.Instance.PlaySfx(4);

            UpdateSkinDisplay();
        }

        private void UpdateSkinDisplay()
        {
            bankText.text = "Bank: " + FruitsInBank();

            for (int i = 0; i < skinDisplay.layerCount; i++)
            {
                skinDisplay.SetLayerWeight(i, 0);
            }

            skinDisplay.SetLayerWeight(skinIndex, 1);


            if (skinList[skinIndex].unlocked)
            {
                priceText.transform.parent.gameObject.SetActive(false);
                buySelectText.text = "Select";
            }
            else
            {
                priceText.transform.parent.gameObject.SetActive(true);
                priceText.text = "Price: " + skinList[skinIndex].skinPrice;
                buySelectText.text = "Buy";

            }

        }

        private void BuySkin(int index)
        {
            if (HaveEnoughFruits(skinList[index].skinPrice) == false)
            {
                AudioManager.Instance.PlaySfx(6);
                Debug.Log("Not enough fruits");
                return;
            }



            AudioManager.Instance.PlaySfx(10);
            string skinName = skinList[skinIndex].skinName;
            skinList[skinIndex].unlocked = true;

            PlayerPrefs.SetInt(skinName + "Unlocked", 1);
        }

        private int FruitsInBank() => PlayerPrefs.GetInt("TotalFruitsAmount");

        private bool HaveEnoughFruits(int price)
        {
            if (FruitsInBank() <= price) return false;
            PlayerPrefs.SetInt("TotalFruitsAmount", FruitsInBank() - price);
            return true;
        } 
    }
    
    [System.Serializable]
    public struct Skin
    {
        public string skinName;
        public int skinPrice;
        public bool unlocked;
    }
}