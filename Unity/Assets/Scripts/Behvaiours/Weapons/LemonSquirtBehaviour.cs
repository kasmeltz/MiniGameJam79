namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("KasJam/LemonSquirt")]
    public class LemonSquirtBehaviour : BehaviourBase
    {
        #region Members

        public float TimeToLive;

        public Vector2 FireForce;

        public float DamagePerSecond;

        public float DamageRange;
        protected float AliveCounter { get; set; }

        protected float DamageTimer { get; set; }

        protected List<EnemyBehaviour> Enemies { get; set; }

        #endregion

        #region Public Methods

        public void Squirt(int direction, float forceMultiplier)
        {
            SoundEffects.Instance.SetDistance(0.5f * Direction);
            SoundEffects.Instance.Throw();

            // [jneen] play this when it lands
            // SoundEffects.Instance.AcidSplash();

            AliveCounter = TimeToLive;

            DamageTimer = 0.25f;

            var force = FireForce * forceMultiplier;

            force.x *= direction;

            GetComponent<SpriteRenderer>()
                .flipX = direction != 1;

            var rigidBody = GetComponent<Rigidbody2D>();

            rigidBody
                .AddForce(force, ForceMode2D.Impulse);            
        }

        public void FadeAway()
        {
            gameObject
                .SetActive(false);
        }

        #endregion

        #region Unity

        protected void Die()
        {
            gameObject
                .SetActive(false);

            AliveCounter = 0;
        }

        protected override void Awake()
        {
            base
                .Awake();

            Enemies = new List<EnemyBehaviour>();
        }

        protected void Update()
        {
            if (AliveCounter > 0)
            {
                AliveCounter -= Time.deltaTime;
                if (AliveCounter <= 0)
                {
                    Die();
                }
            }

            if (AliveCounter >= TimeToLive - 1)
            {
                return;
            }

            DamageTimer -= Time.deltaTime;
            if (DamageTimer <= 0)
            {
                DamageTimer = 0.25f;

                var enemies = FindObjectsOfType<EnemyBehaviour>();

                foreach (var enemy in enemies)
                {
                    if (enemy == null)
                    {
                        continue;
                    }

                    if (enemy.Health <= 0)
                    {
                        continue;
                    }

                    float d = (transform.position - enemy.transform.position).magnitude;

                    if (d <= DamageRange)
                    {
                        enemy
                            .TakeDamage(DamagePerSecond * 0.25f, true);
                    }
                }
            }
        }

        #endregion

    }
}
