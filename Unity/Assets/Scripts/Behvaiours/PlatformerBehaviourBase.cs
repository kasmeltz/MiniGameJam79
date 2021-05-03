namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    public abstract class PlatformerBehaviourBase : BehaviourBase
    {
        #region Members

        public SpriteRenderer SpriteRenderer;

        protected int Direction { get; set; }

        public float Health;

        public float MaxHealth;

        public Vector2 HopVelocity;

        public Rigidbody2D RigidBody;

        public float DamageDelay;

        public float DamageToTakeMultiplier = 1; //1 - takes full damage, 0.5f - takes half of the damage...

        public float AttackDamage;

        public bool IsDead { get; set; }

        protected Animator Animator { get; set; }

        protected bool IsHopping { get; set; }

        protected bool IsJumping { get; set; }

        protected float DamageCounter { get; set; }
        
        #endregion       

        #region Public Methods

        public void TakeDamage(float amount, bool isDOT)
        {
            if (Health <= 0)
            {
                return;
            }

            if (DamageCounter > 0 && !isDOT)
            {
                return;
            }

            Health -= amount * DamageToTakeMultiplier;

            DamageCounter = DamageDelay;

            if (Health < 1)
            {
                Health = 0;
                Die();
            }
        }

        public void ReduceDamageTaken(int resistPercent)
        {
            DamageToTakeMultiplier *= (float)(100 - resistPercent) / 100;
        }

        #endregion

        #region Protected Methods
        protected abstract void Die();

        protected void SetDirection(int dir)
        {
            Direction = dir;

            if (dir < 0)
            {
                SpriteRenderer.flipX = true;
            }
            else
            {
                SpriteRenderer.flipX = false;
            }
        }

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            Animator = GetComponent<Animator>();

            SetDirection(1);
        }        

        protected virtual void Update()
        {
            if (DamageCounter > 0) 
            {
                DamageCounter -= Time.deltaTime;
                if (DamageCounter <= 0)
                {
                    DamageCounter = 0;
                }
            }
        }

        #endregion
    }
}
