using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [Header("Audio Source")]
        [SerializeField] private AudioSource[] sfx;
        [SerializeField] private AudioSource[] bgm;
        [SerializeField] private int bgmIndex;
        
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            if(Instance == null) Instance = this;
            else Destroy(this.gameObject);

            if (bgm.Length <= 0) return;

            InvokeRepeating(nameof(PlayMusicIfNeeded), 0, 2);
        }

        public void PlayMusicIfNeeded()
        {
            if (!bgm[bgmIndex].isPlaying) PlayRandomBGM();
        }

        public void PlaySfx(int sfxToPlay,bool randomPicth = true)
        {
            if (sfxToPlay >= sfx.Length) return;
            if(randomPicth) sfx[sfxToPlay].pitch = Random.Range(.9f, 1.1f);

            sfx[sfxToPlay].Play();
        }

        public void StopSfx(int sfxToStop) => sfx[sfxToStop].Stop();

        private void PlayRandomBGM()
        {
            bgmIndex = Random.Range(0, bgm.Length);
            PlayBGM(bgmIndex);
        }

        private void PlayBGM(int bgmToPlay)
        {
            if (bgm.Length <= 0)
            {
                Debug.LogWarning("You have no music on audio manager!");
                return;
            }

            foreach (AudioSource audioFile in bgm)
            {
                audioFile.Stop();
            }

            bgmIndex = bgmToPlay;
            bgm[bgmToPlay].Play();
        }
    }
}
