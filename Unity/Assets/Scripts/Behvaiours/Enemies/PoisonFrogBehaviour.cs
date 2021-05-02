namespace KasJam.MiniJam79.Unity.Behaviours
{
    using KasJam.MiniJam79.Unity.Managers;
    using System.Linq;
    using UnityEngine;

    [AddComponentMenu("KasJam/PoisonFrog")]
    public class PoisonFrogBehaviour : EnemyBehaviour
    {
        #region Members

        public float HopCooldown;

        public float SeedCooldown;

        public float SpawnCooldown;

        public EnemyPatrolAreaBehaviour EnemyPatrolArea;

        public Bounds Bounds;

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

            var minX = Bounds.min.x + 2f;
            var maxX = Bounds.max.x - 2f;
            var posX = transform.position.x;

            if (Direction == 1 && posX >= maxX)
            {
                return false;
            }

            if (Direction == -1 && posX <= minX)
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

            Animator
                .SetTrigger("Hopping");
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

        protected override void Update()
        {
            if (GameManager
              .Instance
              .IsPaused)
            {
                return;
            }

            if (Health <= 0)
            {
                return;
            }

            base
                .Update();

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

            if (EnemyPatrolArea != null)
            {
                Bounds = EnemyPatrolArea.Bounds;
            }

            var minX = Bounds.min.x + 2f;
            var maxX = Bounds.max.x - 2f;
            var posX = transform.position.x;

            if (Direction == 1 && posX >= maxX)
            {
                SetDirection(-1);
            } 
            else if (Direction == -1 && posX <= minX)
            {
                SetDirection(1);
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