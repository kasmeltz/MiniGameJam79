namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/StrawberrySeed")]
    public class StrawberrySeedBehaviour : BehaviourBase
    {
        #region Members

        public float TimeToLive;

        public Vector2 FireForce;

        protected float AliveCounter { get; set; }

        #endregion

        #region Public Methods

        public void Fire(int Direction)
        {
            AliveCounter = TimeToLive;

            var force = FireForce;

            force.x *= Direction;

            var rigidBody = GetComponent<Rigidbody2D>();

            rigidBody
                .AddForce(force, ForceMode2D.Impulse);
        }

        #endregion

        #region Unity

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            // TODO - damage
            
        }


        protected void Update()
        {
            if (AliveCounter > 0)
            {
                AliveCounter -= Time.deltaTime;
                if (AliveCounter <= 0)
                {
                    gameObject
                        .SetActive(false);
                }
            }            
        }

        #endregion

    }
}