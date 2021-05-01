namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/SoundEffects")]
    public class SoundEffects : BehaviourBase
    {
        #region Members
        public AudioSource source;
        #endregion

        // set by the music when it's playing
        public MusicLooper music;

        public void Jump() { PlayFile("Sounds/jump"); }
        public void Powerup() { PlayFile("Sounds/powerup-1"); }
        public void Land() { PlayFile("Sounds/land"); }
        public void Death() { }
        public void Splash() { }
        public void Tongue() {
            PlayFile(distance > 0.0f ? "Sounds/tongue" : "Sounds/tongue-reverse");
        }

        public void MenuSelect() { PlayFile("Sounds/menu-select"); }

        private void DuckMusic() {
            if (music) music.Duck(0.8f, 0.25f);
        }

        private float distance;
        public void SetDistance(float theDistance) { distance = theDistance / 5.0f; }

        private void PlayFile(string fname) {
            DuckMusic();
            Debug.Log("PlayFile "+fname);
            var clip = Resources.Load<AudioClip>(fname);
            source.panStereo = computePan();
            source.PlayOneShot(clip, computeVolume());
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
