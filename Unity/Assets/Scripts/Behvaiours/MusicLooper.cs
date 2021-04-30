namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    [AddComponentMenu("KasJam/AudioManager")]
    public class MusicLooper : BehaviourBase
    {
        // percentage per second
        private float FADE_SPEED = 2.0f;
        private float FADE_CLAMP = 0.1f;

        #region Members
        public AudioSource intro;
        public AudioSource[] loops;
        public float maxVolume = 1.0f;
        #endregion

        public int index = 0;

        public void Start() {
            startPlaying();
        }

        public void Update() {
            int i = 0;
            foreach (AudioSource source in loops) {
                float targetVolume = (i == index) ? maxVolume : 0.0f;
                source.volume = towards(source.volume, targetVolume, Time.deltaTime);
                i += 1;
            }

            if (Input.GetKeyDown(KeyCode.Q)) {
                MoveToLoop((index + 1) % loops.Length);
            }
        }

        public void MoveToLoop(int i) {
            index = i;
            Debug.Log("index: " + index);
            Debug.Log("loops: " + loops.Length);
        }

        private float towards(float current, float target, float dt) {
            float change = dt * FADE_SPEED * (target - current);
            float next = current + change;

            if (Mathf.Abs(next - target) < FADE_CLAMP) {
                return target;
            } else {
                return next;
            }
        }

        private float introLength() {
            if (intro == null) return 0.0f;
            return intro.clip.length;
        }

        private void startPlaying() {
            double loopStart = AudioSettings.dspTime + introLength();

            if (intro != null) {
                intro.Play();
            }

            Debug.Log("intro: " + introLength());
            Debug.Log("loopStart: " + loopStart);

            int i = 0;
            foreach (AudioSource source in loops) {
                if (i == index) source.volume = maxVolume;
                else source.volume = 0.0f;

                source.loop = true;
                source.PlayScheduled(loopStart);
                i += 1;
            }
        }
    }
}
