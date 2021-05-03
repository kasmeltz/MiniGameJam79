namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/SoundEffects")]
    public class SoundEffects : BehaviourBase
    {
        #region Members
        public AudioSource source;
        #endregion

        #region Singletone
        public static SoundEffects Instance = null;
        void Start()
        { 
            if (Instance == null)
            { 
                Instance = this;
            }
            else if (Instance == this)
            {
                Destroy(gameObject);
            }
        }

        #endregion

        // -------- options --------- //
        private float distance = 0.0f;
        private float pitch = 1.0f;
        private float volume = 1.0f;
        public void SetDistance(float theDistance) { distance = theDistance / 5.0f; }
        public void SetPitch(float thePitch) { pitch = thePitch; }


        // --------- sounds -------- //
        public void Jump() { PlayFile("Sounds/jump"); }
        public void Powerup() { PlayFile("Sounds/powerup-1"); }
        public void Land() { PlayFile("Sounds/land"); }
        public void Tongue() {
            PlayFile(distance > 0.0f ? "Sounds/tongue-reverse" : "Sounds/tongue");
        }

        public void Throw() {
            pitch = 1.5f;
            volume = 0.7f;
            PlayFile(distance > 0.0f ? "Sounds/throw" : "Sounds/throw-reverse");
        }

        public void Spit() {
            PlayFile(distance > 0.0f ? "Sounds/spit" : "Sounds/spit-reverse");
        }

        public void Bomb() { volume = 0.7f; PlayFile("Sounds/bomb"); }

        public void Damage() {
            volume = 0.5f;
            PlayFile("Sounds/damage");
        }

        public void TongueConnect() { volume = 1.0f; PlayFile("Sounds/tongue-connect"); }
        public void Death() { volume = 0.55f; pitch = 0.9f; PlayFile("Sounds/death"); }

        // bouncing on bouncy things
        public void Bounce() { volume = 1.0f; PlayFile("Sounds/bounce"); }

        public void AcidDamage() { volume = 1.0f; PlayFile("Sounds/acid-damage"); }

        // starting levels
        public void MenuGo() { volume = 1.0f; PlayFile("Sounds/menu-go"); }

        // acid hits the ground
        public void AcidSplash() { volume = 1.0f; PlayFile("Sounds/acid-splash"); }

        public void DialogueKiki() {
            distance = -0.4f;
            volume = 0.2f;
            RandomDiatonic(new int[] { -3, -2, 0, 2, 4, 6, 7 });
            PlayFile("Sounds/dialogue-kiki");
        }

        public void DialogueWiz() {
            distance = 0.4f;
            volume = 0.3f;
            RandomDiatonic(new int[] { -4, -2, 0, 1, 3, 5, 7 });
            PlayFile("Sounds/dialogue-wiz");
        }

        // hitting "confirm" in any menu (except upgrade-buy and start-level)
        public void MenuSelect() { PlayFile("Sounds/menu-select"); }
        public void BuyUpgrade() {
            float pitchRange = 1.5f;
            pitch = Random.Range(1.0f/pitchRange, pitchRange);
            volume = 0.3f;
            PlayFile("Sounds/buy-upgrade");
        }

        protected override void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /////// --------- helpers ----------- ///////
        private void DuckMusic() {
            if (MusicLooper.Current) MusicLooper.Current.Duck(0.8f, 0.25f);
        }

        private void RandomDiatonic(int[] notes) {
            int note = notes[Random.Range(0, notes.Length)];
            pitch *= Mathf.Pow(2.0f, ((float) note)/12.0f);
        }

        private void PlayFile(string fname) {
            // [jneen] doing this in the unity mixer instead
            // DuckMusic();
            Debug.Log("PlayFile "+fname);
            var clip = Resources.Load<AudioClip>(fname);
            DontDestroyOnLoad(clip);
            source.panStereo = computePan();
            source.pitch = pitch;
            source.volume = computeVolume();
            source.PlayOneShot(clip, computeVolume());
            volume = 1.0f;
            distance = 0.0f;
            pitch = 1.0f;
        }

        private float computePan() {
            if (distance <= -1.0f) return -1.0f;
            if (distance >= 1.0f) return 1.0f;
            return distance;
        }

        private float computeVolume() {
            if (distance < -1.0f) return 0.0f;
            if (distance > 1.0f) return 0.0f;
            float dist = Mathf.Abs(distance) / 5.0f;
            if (dist < 0.1f) return 1.0f;
            return volume * Mathf.Lerp(0.5f, 1.0f, 1.0f - dist * dist * dist * dist);
        }
    }
}
