namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/StrawberrySeed")]
    public class StrawberrySeedBehaviour : BehaviourBase
    {
        #region Members

        public float TimeToLive;

        public Vector2 FireForce;

        public float AttackDamage;

        protected float AliveCounter { get; set; }

        #endregion

        #region Public Methods

        public void Fire(int Direction)
        {
            var soundEffects = FindObjectOfType<SoundEffects>();
            soundEffects.SetDistance(Direction * 0.5f);
            soundEffects.Spit();

            AliveCounter = TimeToLive;

            var force = FireForce;

            force.x *= Direction;

            GetComponent<SpriteRenderer>()
                .flipX = Direction != 1;

            var rigidBody = GetComponent<Rigidbody2D>();

            rigidBody
                .AddForce(force, ForceMode2D.Impulse);
        }

        protected void Die()
        {
            gameObject
                .SetActive(false);

            AliveCounter = 0;
        }

        #endregion

        #region Unity

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            var hero = collision
                .collider
                .GetComponent<PoisonFrogBehaviour>();

            if (hero != null)
            {
                hero
                    .TakeDamage(AttackDamage);
            }

            Die();
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
        }

        #endregion

    }
}
