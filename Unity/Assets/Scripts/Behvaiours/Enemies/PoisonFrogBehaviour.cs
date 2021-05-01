namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System.Linq;
    using UnityEngine;

    [AddComponentMenu("KasJam/PoisonFrog")]
    public class PoisonFrogBehaviour : EnemyBehaviour
    {
        #region Members

        public float HopCooldown;

        public float SeedCooldown;

        public float AttackDamage;

        public float SpawnCooldown;

        public EnemyPatrolAreaBehaviour EnemyPatrolArea;

        protected GameObjectPoolBehaviour PoisonSeedPool { get; set; }

        protected float HopCounter { get; set; }

        protected float SeedCounter { get; set; }

        #endregion

        #region PlatformerBehaviourBase

        protected override void Die()
        {
        }

        #endregion

        #region Protected Methods

        protected bool CanHop()
        {
            if (IsHopping)
            {
                return false;
            }

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
            return SeedCounter <= 0;
        }

        protected void Shoot()
        {
            var seed = PoisonSeedPool
               .GetPooledObject<PoisonSeedBehaviour>();

            if (seed == null)
            {
                return;
            }

            seed.AttackDamage = AttackDamage;

            seed.transform.position = transform.position + new Vector3(0.32f * Direction, 0.32f, 0);

            seed
                .Fire(Direction);

            SeedCounter = SeedCooldown;
        }

        #endregion

        #region Unity

        protected void Update()
        {
            if (HopCounter > 0)
            {
                HopCounter -= Time.deltaTime;
                if (HopCounter <= 0)
                {
                    HopCounter = 0;
                    IsHopping = false;
                }
            }

            if (SeedCounter > 0)
            {
                SeedCounter -= Time.deltaTime;
                if (SeedCounter <= 0)
                {
                    SeedCounter = 0;
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

        protected override void Awake()
        {
            base
                .Awake();

            var pools = FindObjectsOfType<GameObjectPoolBehaviour>(true);

            PoisonSeedPool = pools.FirstOrDefault(o => o
                .ObjectToPool
                .GetComponent<PoisonSeedBehaviour>() != null);
        }

        #endregion
    }
}