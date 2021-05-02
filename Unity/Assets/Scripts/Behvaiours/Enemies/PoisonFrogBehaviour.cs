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

        protected float HopStartX { get; set; }

        protected float HopCounter { get; set; }

        protected float SeedCounter { get; set; }

        protected float NoMoveCounter { get; set; }

        #endregion

        #region PlatformerBehaviourBase

        protected override void Die()
        {
            if (IsDead)
            {
                return;
            }

            IsDead = true;
            Health = 0;

            Animator
                .SetTrigger("Dying");

            SoundEffects.Instance.Death();
        }

        #endregion

        #region Animation Callbacks

        public void DyingAnimationFinished()
        {
            gameObject
                .SetActive(false);
        }

        #endregion

        #region Protected Methods

        protected bool CanHop()
        {
            if (IsHopping)
            {
                return false;
            }

            var minX = Bounds.min.x + 1f;
            var maxX = Bounds.max.x - 1f;
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
            if (Direction == 1 && transform.position.x > HopStartX)
            {
                NoMoveCounter = 0;
            }
            else if (Direction == -1 && transform.position.x < HopStartX)
            {
                NoMoveCounter = 0;
            }

            var v = new Vector2(Direction, 1);

            HopStartX = transform.position.x;

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

            float reduction = ((11f - Level) / 10f);
            reduction = Mathf.Min(1f, reduction);
            reduction = Mathf.Max(0.5f, reduction);

            SeedCounter = SeedCooldown * reduction;
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

            NoMoveCounter += Time.deltaTime;
            if (NoMoveCounter > 2)
            {
                SetDirection(-Direction);
                NoMoveCounter = 0;
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