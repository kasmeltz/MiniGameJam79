namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/MovingPlatform")]
    public class MovingPlatformBehaviour : BehaviourBase
    {
        #region Members

        public Rigidbody2D RigidBody;

        public Vector2 Start;

        public Vector2 End;

        public float MoveSpeed;

        public float PauseAtStart;

        public float PauseAtEnd;

        public int Direction;

        protected float PauseTimer { get; set; }

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            PauseTimer = 0;
            Direction = 1;
        }

        protected void Update()
        {
            if (PauseTimer > 0)
            {
                PauseTimer -= Time.deltaTime;
                if (PauseTimer <= 0)
                {
                    PauseTimer = 0;
                }

                return;
            }

            Vector3 moveDirection;
            
            if (Direction == 1)
            {
                moveDirection = End - Start;
            }
            else
            {
                moveDirection = Start - End;
            }

            moveDirection = moveDirection.normalized;

            RigidBody.velocity = new Vector2(moveDirection.x, moveDirection.y);

            RigidBody.velocity *= MoveSpeed;

            if (transform.position.x > End.x)
            {
                transform.position = End;
                PauseTimer = PauseAtEnd;
                RigidBody.velocity *= 0;
                Direction = -1;
            }

            if (transform.position.x < Start.x)
            {
                transform.position = Start;
                PauseTimer = PauseAtStart;
                RigidBody.velocity *= 0;
                Direction = 1;
            }
        }

        #endregion
    }
}