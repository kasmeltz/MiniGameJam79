namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/LemonSquirt")]
    public class LemonSquirtBehaviour : BehaviourBase
    {
        #region Members

        public Vector2 FireForce;

        protected float AliveCounter { get; set; }

        #endregion

        #region Public Methods

        public void Squirt(int Direction)
        {
            var force = FireForce;

            force.x *= Direction;

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

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            // TODO - damage
            
        }

        #endregion

    }
}