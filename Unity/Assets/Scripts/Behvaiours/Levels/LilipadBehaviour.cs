namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/Lilipad")]
    public class LilipadBehaviour : BehaviourBase
    {
        #region Members

        public float Springiness;

        #endregion

        #region Unity

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            var hero = collision
                .collider
                .GetComponent<PlatformerBehaviour>();

            if (hero == null)
            {
                return;
            }

            SoundEffects.Instance.Bounce();
            var velocity = hero.RigidBody.velocity;
            velocity.y = Springiness;
            hero.RigidBody.velocity = velocity;
        }

        #endregion
    }
}
