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

        protected bool IsHopping { get; set; }

        #endregion

        #region Protected Members

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

            SetDirection(1);
        }        

        #endregion
    }
}