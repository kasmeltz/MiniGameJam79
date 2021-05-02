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
        public void Death() { }
        public void Splash() { }
        public void Tongue() {
            PlayFile(distance > 0.0f ? "Sounds/tongue" : "Sounds/tongue-reverse");
        }

        public void Throw() {
            pitch = 1.5f;
            volume = 0.7f;
            PlayFile(distance > 0.0f ? "Sounds/throw" : "Sounds/throw-reverse");
        }

        public void Spit() {
            PlayFile(distance > 0.0f ? "Sounds/spit" : "Sounds/spit-reverse");
        }

        public void Damage() {
            volume = 0.5f;
            PlayFile("Sounds/damage");
        }

        public void BuyUpgrade() {
            float pitchRange = 1.5f;
            pitch = Random.Range(1.0f/pitchRange, pitchRange);
            volume = 0.3f;
            PlayFile("Sounds/buy-upgrade");
        }

        public void MenuSelect() { PlayFile("Sounds/menu-select"); }


        // DELETEME
        public void Update() {
            if (Input.GetKeyDown(KeyCode.Z)) {
                BuyUpgrade();
            }
        }

        /////// --------- helpers ----------- ///////
        private void DuckMusic() {
            if (MusicLooper.Current) MusicLooper.Current.Duck(0.8f, 0.25f);
        }

        private void PlayFile(string fname) {
            DuckMusic();
            Debug.Log("PlayFile "+fname);
            var clip = Resources.Load<AudioClip>(fname);
            source.panStereo = computePan();
            source.PlayOneShot(clip, computeVolume());
            source.pitch = pitch;
            source.volume = volume;
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
            return 1.0f - dist * dist * dist * dist;
        }
    }
}
