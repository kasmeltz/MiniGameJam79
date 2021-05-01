namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/BrambleBush")]
    public class BrambleBushBehaviour : BehaviourBase
    {
        #region Members

        public float Damage;

        #endregion

        #region Unity

        protected void OnTriggerEnter2D(Collider2D collider)
        {
            var hero = collider
                .GetComponent<PlatformerBehaviour>();

            if (hero == null)
            {
                return;
            }

            hero
                .TakeDamage(Damage);
        }

        #endregion
    }
}