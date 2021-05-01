namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/PoisonFrog")]
    public class PoisonFrogBehaviour : EnemyBehaviour
    {
        #region Members

        public float HopCooldown;

        public float SeedCooldown;

        public float Damage;

        public float SpawnCooldown;

        public EnemyPatrolAreaBehaviour EnemyPatrolArea;

        protected float HopCounter { get; set; }

        #endregion

        #region Protected Methods

        protected bool CanHop()
        {
            if (IsHopping)
            {
                return false;
            }

            var pos = transform.position.x;
            float minX = EnemyPatrolArea.Bounds.min.x;
            float maxX = EnemyPatrolArea.Bounds.max.x;

            if (transform.position.x >= EnemyPatrolArea.Bounds.max.x - 1.5f)
            {
                return false;
            }

            if (transform.position.x <= EnemyPatrolArea.Bounds.min.x + 1.5f)
            {
                return false;
            }

            return true;
        }

        protected void Hop()
        {
            var v = new Vector2(Direction, 1);

            RigidBody
                .velocity = v.normalized * HopVelocity;

            IsHopping = true;
            HopCounter = HopCooldown;
        }

        protected bool CanShoot()
        {
            return SeedCooldown <= 0;
        }

        protected void Shoot()
        {

        }

        #endregion

        #region Unity

        protected void Update()
        {
            if (HopCounter> 0)
            {
                HopCounter -= Time.deltaTime;
                if (HopCounter <= 0)
                {
                    HopCounter = 0;
                    IsHopping = false;
                }
            }

            if (CanHop())
            {
                Hop();
            }

            if (CanShoot())
            {
                Shoot();
            }
           
        }

        #endregion
    }
}