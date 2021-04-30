namespace KasJam.MiniJam79.Unity.Behaviours
{
    using KasJam.MiniJam79.Unity.ScriptableObjects;
    using System.Text;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("KasJam/Platformer")]
    public class PlatformerBehaviour : BehaviourBase
    {
        #region Members

        public Bounds LevelBounds;

        public HeroUpgradeScriptableObject[] Upgrades;

        public CompositeCollider2D OneWayCollider;

        public ProgressBarBehaviour FlyPowerProgressBar;

        public ProgressBarBehaviour HealthProgressBar;

        public Rigidbody2D RigidBody;

        public Vector2 JumpVelocity;

        public Vector2 HopVelocity;

        public float MoveVelocity;

        public Vector3 RespawnPosition;

        public LayerMask GroundLayer;

        public LayerMask WallsLayer;

        public Text DebugText;

        public Text FliesEatenText;

        public float CoyoteTimeLimit;

        public float JumpMoveTimeLimit;

        public FrogTongueBehaviour Tongue;

        public float FlyPowerDuration;

        public SpriteRenderer SpriteRenderer;

        public float DeathVelocity;

        public float Health;

        public float MaxHealth;

        public int FliesEaten { get; set; }

        protected bool IsJumping { get; set; }

        protected float JumpMoveTimer { get; set; }

        protected bool IsOnGround { get; set; }

        protected bool IsAgainstWall { get; set; }

        protected bool IsAgainstLeftWall { get; set; }

        protected bool IsAgainstRightWall { get; set; }

        protected bool IsJumpRequested { get; set; }

        protected bool IsHopping { get; set; }

        protected bool IsCoyoteTime { get; set; }

        protected float CoyoteTimer { get; set; }

        protected float HopStartY { get; set; }

        protected float JumpStartY { get; set; } 

        protected float ImpactVelocity { get; set; }

        protected int Direction { get; set; }

        protected Vector2 ActualJumpVelocity { get; set; }

        protected float FlyPowerTimer { get; set; }

        protected Collider2D GroundCollider { get; set; }

        protected Animator Animator { get; set; }

        #endregion

        #region Event Handlers

        private void Tongue_FlyGobbled(object sender, Events.FlyBehaviourEventArgs e)
        {
            ActualJumpVelocity = JumpVelocity * 1.25f;
            FliesEaten++;
            FlyPowerTimer = FlyPowerDuration;

            UpdateUI();
        }

        #endregion

        #region Protected Methods

        protected void UpdateUI()
        {
            FliesEatenText.text = FliesEaten
                .ToString();
        }

        protected void RemoveFlyPowers()
        {
            ActualJumpVelocity = JumpVelocity;
        }

        protected bool CanJump()
        {
            if (IsJumping)
            {
                return false;
            }

            return (IsAgainstWall || IsOnGround || IsHopping || IsCoyoteTime);
        }

        protected void Jump()
        {
            JumpStartY = transform.position.y;
            
            var vel = new Vector2(0, 0);
            
            if (IsOnGround || IsHopping || IsCoyoteTime)
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    vel.x = -ActualJumpVelocity.x;
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    vel.x = ActualJumpVelocity.x;
                }

                vel.y = ActualJumpVelocity.y;

                IsOnGround = false;
            }
            else if (IsAgainstWall)
            {
                if (IsAgainstLeftWall)
                {
                    vel.x = ActualJumpVelocity.x;
                    vel.y = ActualJumpVelocity.y;

                    IsAgainstLeftWall = false;
                }
                else if (IsAgainstRightWall)
                {
                    vel.x = -ActualJumpVelocity.x;
                    vel.y = ActualJumpVelocity.y;

                    IsAgainstRightWall = false;
                }

                IsAgainstWall = false;
            }

            RigidBody
                .velocity = vel;

            JumpMoveTimer = JumpMoveTimeLimit;
            IsJumping = true;

            Animator
                .SetTrigger("Jumping");
        }

        protected void ReSpawn()
        {
            transform.position = RespawnPosition;
            IsJumping = false;
            JumpMoveTimer = 0;
            IsOnGround = false;
        }

        protected bool CanHop()
        {
            return IsOnGround && !IsHopping && !IsJumping;
        }

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

        protected void Hop(int dir)
        {
            HopStartY = transform.position.y;

            var v = new Vector2(dir, 1);

            RigidBody
                .velocity = v.normalized * HopVelocity;

            IsOnGround = false;
            IsHopping = true;
        }

        protected void HandleHorizontalInput(int dir)
        {
            SetDirection(dir);

            if (CanHop())
            {
                Hop(dir);
            }
            else
            {
                if (JumpMoveTimer <= 0)
                {
                    var v = new Vector2(MoveVelocity * dir, RigidBody.velocity.y);
                    RigidBody.velocity = v;
                }
            }
        }

        protected void DrawDebug()
        {
            StringBuilder sb = new StringBuilder();

            if (IsCoyoteTime)
            {
                sb
                    .AppendLine($"COYOTTE TIMER {CoyoteTimer}");
            }
            else
            {
                sb
                    .AppendLine("");
            }

            if (IsJumping)
            {
                sb
                    .AppendLine("JUMPING");
            }
            else
            {
                sb
                    .AppendLine("");
            }

            if (JumpMoveTimer > 0)
            {
                sb
                    .AppendLine($"JUMP MOVE TIMER {JumpMoveTimer}");
            }
            else
            {
                sb
                    .AppendLine("");
            }

            if (ImpactVelocity != 0)
            {
                sb
                    .AppendLine($"IMPACT VELOCITY {ImpactVelocity}");
            }
            else
            {
                sb
                    .AppendLine("");
            }

            if (IsJumpRequested)
            {
                sb
                    .AppendLine("JUMP REQUESTED");
            }
            else
            {
                sb
                    .AppendLine("");
            }

            if (IsHopping)
            {
                sb
                    .AppendLine("HOPPING");
            }
            else
            {
                sb
                    .AppendLine("");
            }

            if (IsOnGround)
            {
                sb
                    .AppendLine("ON GROUND");
            }
            else
            {
                sb
                    .AppendLine("");
            }

            if (IsAgainstLeftWall)
            {
                sb
                    .AppendLine("AGAINST WALL TO THE LEFT");
            }
            else
            {
                sb
                    .AppendLine("");
            }

            if (IsAgainstRightWall)
            {
                sb
                    .AppendLine("AGAINST WALL TO THE RIGHT");
            }
            else
            {
                sb
                    .AppendLine("");
            }

            DebugText.text = sb
                .ToString();
        }

        protected void StartCoyoteTime()
        {
            IsCoyoteTime = true;
            CoyoteTimer = CoyoteTimeLimit;
        }

        protected void DoGroundTest()
        {
            Collider2D collider;
            var o = new Vector2(transform.position.x, transform.position.y - 0.08f);
            var s = new Vector2(0.32f, 0.32f);

            var raycastHit = Physics2D
                .BoxCast(o, s, 0, Vector2.down, 0.02f, GroundLayer);

            var hitGround = false;

            if (RigidBody.velocity.y <= 0.01)
            {
                collider = raycastHit.collider;
                if (collider != null)
                {
                    GroundCollider = collider;

                    if (!collider.isTrigger)
                    {
                        if (transform.position.y >= raycastHit.point.y)
                        {
                            hitGround = true;
                        }
                    }
                }
            }

            if (hitGround)
            {
                if (ImpactVelocity < DeathVelocity)
                {
                    ReSpawn();
                }

                if (GroundCollider
                    .name
                    .ToLower()
                    .Contains("water"))
                {
                    ReSpawn();
                }

                MovingPlatformBehaviour movingPlatform = GroundCollider
                    .GetComponent<MovingPlatformBehaviour>();
                if (movingPlatform != null)
                {
                    RigidBody.velocity = movingPlatform.RigidBody.velocity;
                }
                
                ImpactVelocity = 0;
                IsJumping = false;
                IsHopping = false;
                IsCoyoteTime = false;
                IsOnGround = true;

                if (IsJumpRequested)
                {
                    IsJumpRequested = false;
                    Jump();
                }
            }
            else
            {
                if (IsOnGround)
                {
                    StartCoyoteTime();
                }

                IsOnGround = false;
            }

            o = new Vector2(transform.position.x - 0.08f, transform.position.y);
            s = new Vector2(0.32f, 0.32f);

            raycastHit = Physics2D
                .BoxCast(o, s, 0, Vector2.left, 0.02f, WallsLayer);

            var hitWall = false;
            collider = raycastHit.collider;
            if (collider != null)
            {
                hitWall = true;
            }

            if (hitWall)
            {
                IsAgainstLeftWall = true;                
            }
            else
            {
                IsAgainstLeftWall = false;
            }

            o = new Vector2(transform.position.x + 0.08f, transform.position.y);
            s = new Vector2(0.32f, 0.32f);

            raycastHit = Physics2D
                .BoxCast(o, s, 0, Vector2.right, 0.02f, WallsLayer);

            hitWall = false;
            collider = raycastHit.collider;
            if (collider != null)
            {
                hitWall = true;
            }

            if (hitWall)
            {
                IsAgainstRightWall = true;
            }
            else
            {
                IsAgainstRightWall = false;
            }

            IsAgainstWall = IsAgainstLeftWall | IsAgainstRightWall;

            if (IsAgainstWall)
            {
                IsJumping = false;
                IsHopping = false;
                IsCoyoteTime = false;

                if (IsJumpRequested)
                {
                    IsJumpRequested = false;
                    Jump();
                }
            }
        }

        #endregion

        #region Unity

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            DoGroundTest();
        }

        protected void OnCollisionExit2D(Collision2D collision)
        {
            DoGroundTest();
        }

        protected void OnTriggerEnter2D(Collider2D collider)
        {
            DoGroundTest();
        }

        protected void OnTriggerExit2D(Collider2D collider)
        {
            if (collider == OneWayCollider)
            {
                OneWayCollider
                    .isTrigger = false;
            }

            DoGroundTest();
        }

        protected override void Awake()
        {
            base
                .Awake();

            Animator = GetComponent<Animator>();

            SetDirection(1);

            Tongue.FlyGobbled += Tongue_FlyGobbled;

            ActualJumpVelocity = JumpVelocity;

            FlyPowerProgressBar
                .SetValue(0, FlyPowerDuration);

            HealthProgressBar
                .SetValue(Health, MaxHealth);

            FliesEaten = 0;

            foreach(var upgrade in Upgrades)
            {
                upgrade.Level = 0;
            }
         
            UpdateUI();
        }

        protected void Update()
        {
            if (Input
                .GetKey(KeyCode.DownArrow))
            {
                if (GroundCollider == OneWayCollider)
                {
                    OneWayCollider.isTrigger = true;
                    DoGroundTest();
                }
            }

            if (RigidBody.velocity.y < -1)
            {
                ImpactVelocity = RigidBody.velocity.y;
            }

            if (CoyoteTimer > 0)
            {
                CoyoteTimer -= Time.deltaTime;
                if (CoyoteTimer <= 0)
                {
                    IsCoyoteTime = false;
                    CoyoteTimer = 0;
                }
            }

            if (JumpMoveTimer > 0)
            {
                JumpMoveTimer -= Time.deltaTime;
                if (JumpMoveTimer <= 0)
                {
                    JumpMoveTimer = 0;
                }
            }

            if (FlyPowerTimer > 0)
            {
                FlyPowerTimer -= Time.deltaTime;
                if (FlyPowerTimer <= 0)
                {
                    FlyPowerTimer = 0;

                    RemoveFlyPowers();
                }

                FlyPowerProgressBar
                    .SetValue(FlyPowerTimer, FlyPowerDuration);
            }

            HealthProgressBar
                .SetValue(Health, MaxHealth);

            if (IsHopping && transform.position.y < HopStartY)
            {
                IsHopping = false;

                StartCoyoteTime();
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                HandleHorizontalInput(-1);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                HandleHorizontalInput(1);
            }

            if (Input
                .GetKeyDown(KeyCode
                .UpArrow))
            {
                if (CanJump())
                {
                    Jump();
                }
                else
                {
                    IsJumpRequested = true;
                }
            }

            if (IsJumpRequested && !Input
                .GetKey(KeyCode.UpArrow))
            {
                IsJumpRequested = false;
            }

            if (transform.position.y <= -7)
            {
                ReSpawn();
            }

            if (Input
                .GetKeyDown(KeyCode.A))
            {
                Tongue
                    .Shoot(Direction);
            }

            if (IsAgainstWall)
            {
                if (IsAgainstLeftWall)
                {
                    Tongue
                        .SetDirection(1);
                }
                else if (IsAgainstRightWall)
                {
                    Tongue
                        .SetDirection(-1);
                }
            }
            else
            {
                Tongue
                    .SetDirection(Direction);
            }


            DrawDebug();
            
            var pos = transform.position;

            if (pos.x < LevelBounds.min.x)
            {
                pos.x = LevelBounds.min.x;
            }

            if (pos.x > LevelBounds.max.x)
            {
                pos.x = LevelBounds.max.x;
            }

            pos.z = 0;

            transform.position = pos;

            pos.z = -10;

            if (pos.x >= LevelBounds.min.x + 6 && pos.x <= LevelBounds.max.x - 6)
            {
                var newPos = Vector3
                    .Lerp(Camera.main.transform.position, pos, 0.5f);

                Camera.main.transform.position = newPos;
            }
        }

        #endregion
    }
}