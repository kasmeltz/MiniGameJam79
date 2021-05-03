namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;
    using System;

    [AddComponentMenu("KasJam/MusicLooper")]
    public class MusicLooper : BehaviourBase
    {
        // percentage per second
        private float FADE_SPEED = 2.0f;
        private float FADE_CLAMP = 0.1f;
        public bool IsPlaying = false;

        #region Members
        public AudioSource intro;
        public AudioSource[] loops;
        public float maxVolume = 1.0f;
        #endregion

        public AudioSource[] nextLoops;

        private SoundEffects soundEffects;

        private AudioSource currentLoop;
        private float loopSize;

        public static MusicLooper Current = null;

        public int index = 0;

        ///// -------  Start / Stop -------- /////
        public void EnsurePlaying() { EnsurePlaying(0.5f); }
        public void EnsurePlaying(float delayTime) {
            if (IsPlaying) return;

            Current = this;

            startPlaying(delayTime);
        }

        public void EnsureNotPlaying() {
            if (!IsPlaying) return;

            IsPlaying = false;
        }

        public void MoveToLoop(int i) {
            index = i;
            Debug.Log("index: " + index);
            Debug.Log("loops: " + loops.Length);
        }

        ///// -------- DUCKING ----------- /////
        private float duckTime = 0.0f;
        private float duckDepth = 0.0f;
        private double duckStart = 0.0d;
        private float duckVolume = 1.0f;

        // [jneen] depth = [0.0 .. 1.0], how deep to duck. 1 for no duck, 0 for complete silence
        // length = how long to duck for
        public void Duck(float depth, float length) {
            double now = Now();

            if (duckStart + duckTime < now + length) {
                duckStart = now;
                duckTime = length;
            }

            duckDepth = Mathf.Min(depth, computeDuckVolume(now));

            Debug.Log("Duck to "+duckDepth+" over "+duckTime+"s");
        }

        private float computeDuckVolume(double now) {
            double duckProgress = now - duckStart;
            return Mathf.Lerp(duckDepth, 1.0f, (float) (duckProgress / duckTime));
        }

        /////// Unity Hooks /////////

        protected override void Awake()
        {
            loopSize = loops[0].clip.length;
            DontDestroyOnLoad(gameObject);
        }

        public void Update() {
            double now = Now();

            if (duckTime > 0.0f) {
                duckVolume = computeDuckVolume(now);
            } else {
                duckTime = 0.0f;
                duckVolume = 1.0f;
                duckDepth = 0.0f;
            }

            float musicVolume = IsPlaying ? maxVolume * duckVolume : 0.0f;

            intro.volume = towards(intro.volume, musicVolume, Time.deltaTime);

            SetTargetVolumes(loops, musicVolume);
            if (nextLoops != null) SetTargetVolumes(nextLoops, musicVolume);

            if (!IsPlaying) return;

            if (nextLoops != null && nextLoops[0].time > 0.01f) {
                foreach (AudioSource loop in loops) {
                    loop.Stop();
                    Destroy(loop.gameObject);
                }

                loops = nextLoops;
                nextLoops = null;
            }

            if (nextLoops == null && loops[0].time > 0.75f * loopSize) {
                Debug.Log("starting next loop at "+loops[0].timeSamples+" (sampleRate: "+AudioSettings.outputSampleRate+")");
                double startAt = AudioSettings.dspTime - loops[0].timeSamples / (float) AudioSettings.outputSampleRate + loopSize;
                nextLoops = Array.ConvertAll(loops, Instantiate);

                foreach (AudioSource loop in nextLoops) {
                    DontDestroyOnLoad(loop);
                    loop.PlayScheduled(startAt);
                }
            }
        }

        private void SetTargetVolumes(AudioSource[] sources, float musicVolume) {
            int i = 0;
            foreach (AudioSource source in sources) {
                float targetVolume = (i == index) ? musicVolume : 0.0f;
                source.volume = towards(source.volume, targetVolume, Time.deltaTime);
                if (source.volume < 0.01f && !IsPlaying) source.Stop();
                i += 1;
            }
        }


        ///// -------- Helpers ---------- //////

        private double Now() { return AudioSettings.dspTime; }

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

        private void startPlaying(float delayTime) {
            IsPlaying = true;
            nextLoops = null;

            double now = Now();

            double introStart = now + delayTime;
            double loopStart = introStart;

            if (intro) {
                loopStart += intro.clip.length;
                intro.volume = maxVolume;
                intro.PlayScheduled(introStart);
            }

            //Debug.Log("intro: " + introLength());
            //Debug.Log("loopStart: " + loopStart);

            int i = 0;
            foreach (AudioSource source in loops) {
                if (i == index) source.volume = maxVolume;
                else source.volume = 0.0f;

                // [jneen] busted on WebGL build
                // source.loop = true;
                source.PlayScheduled(loopStart);
                i += 1;
            }

            currentLoop = loops[index];
        }
    }
}
