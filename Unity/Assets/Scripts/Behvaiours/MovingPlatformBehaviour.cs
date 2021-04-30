namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/MovingPlatform")]
    public class MovingPlatformBehaviour : BehaviourBase
    {
        #region Members

        public Rigidbody2D RigidBody;

        public Collider2D Collider;

        public Vector2 Start;

        public Vector2 End;

        public float MoveSpeed;

        public float PauseAtStart;

        public float PauseAtEnd;

        public int Direction;

        public bool IsTransitioningIn { get; set; }

        public bool IsTransitioningOut { get; set; }

        protected float PauseTimer { get; set; }

        #endregion

        #region Public Methods

        public void StartTransition(bool isOut)
        {
            if (isOut)
            {
                IsTransitioningOut = true;
                Collider.isTrigger = true;
            }
            else
            {
                transform.position = new Vector3(Start.x, -5, 0);
                IsTransitioningIn = true;
                Collider.isTrigger = true;
            }            
        }

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            PauseTimer = 0;
            Direction = 1;
            IsTransitioningOut = false;
            IsTransitioningIn = false;
        }

        protected void Update()
        {
            if (IsTransitioningOut)
            {
                transform.position += new Vector3(0, -Time.deltaTime * 4, 0);

                if (transform.position.y <= -4)
                {
                    IsTransitioningOut = false;

                    gameObject
                        .SetActive(false);
                }

                return;
            }

            if (IsTransitioningIn)
            {
                transform.position += new Vector3(0, Time.deltaTime * 4, 0);

                if (transform.position.y >= Start.y)
                {
                    IsTransitioningIn = false;

                    transform.position = new Vector3(Start.x, Start.y, 0);

                    Collider.isTrigger = false;
                }

                return;
            }

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