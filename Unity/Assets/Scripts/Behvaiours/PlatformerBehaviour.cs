namespace KasJam.MiniJam79.Unity.Behaviours
{
    using KasJam.MiniJam79.Unity.Managers;
    using KasJam.MiniJam79.Unity.ScriptableObjects;
    using System.Text;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [AddComponentMenu("KasJam/Platformer")]
    public class PlatformerBehaviour : BehaviourBase
    {
        #region Members

        public Bounds LevelBounds;

        public LevelManagerBehaviour LevelManger;

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

        public Text DebugText;

        public float CoyoteTimeLimit;

        public float JumpMoveTimeLimit;

        public FrogTongueBehaviour Tongue;

        public float FlyPowerDuration;

        public SpriteRenderer SpriteRenderer;

        public float DeathVelocity;

        public float Health;

        public float MaxHealth;

        public KeyCode TongueKey;

        public KeyCode FlyPowerKey;

        public Image GameOverPanel;

        public SoundEffects soundEffects;

        public GameObjectPoolBehaviour CherryBombPool;

        public float[] FlyPowerCooldownsAmount;

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

        protected FlyType FlyPower { get; set; }

        protected float FlyPowerCooldown { get; set; }
        
        #endregion

        #region Event Handlers

        public event UnityAction<int> FliesEatenHasChanged;

        private void LevelManger_LevelStarted(object sender, System.EventArgs e)
        {
            Restart();
        }

        private void Tongue_FlyGobbled(object sender, Events.FlyBehaviourEventArgs e)
        {
            soundEffects.SetDistance(0.0f);
            soundEffects.Powerup();

            ActualJumpVelocity = JumpVelocity * 1.25f;
            FliesEaten++;
            FliesEatenHasChanged?.Invoke(FliesEaten);

            if (e.Fly.FlyType == FlyType.Poison)
            {
                TakeDamage(25);
            }
            else
            {
                FlyPowerTimer = FlyPowerDuration;
                FlyPower = e.Fly.FlyType;
            }

            UpdateUI();
        }

        #endregion

        #region Public Methods

        public void StartLevel()
        {

        }

        public bool TrySpendFlies(int cost)
        {
            if(FliesEaten >= cost)
            {
                FliesEaten -= cost;
                FliesEatenHasChanged?.Invoke(FliesEaten);
                return true;
            }

            return false;
        }

        #endregion

        #region Protected Methods

        protected void TakeDamage(float amount)
        {
            Health -= amount;

            if (Health < 1)
            {
                Health = 0;
                Die();
            }
        }

        protected void UpdateUI()
        {
            //FliesEatenText.text = FliesEaten
                //.ToString();

            FlyPowerProgressBar
                .gameObject
                .SetActive(false);

            string frogName = "FrogBaseController";
            int spriteIndex = -1;

            if (FlyPower != FlyType.None)
            {
                frogName = $"{FlyPower}FrogController";

                FlyPowerProgressBar
                    .gameObject
                    .SetActive(true);

                spriteIndex = (int)FlyPower + 1;
            }

            var animationController = Resources
                .Load<RuntimeAnimatorController>($"Animations/Frogs/{frogName}");

            Animator.runtimeAnimatorController = animationController;

            if (spriteIndex >= 0)
            {
                var sprites = Resources
                    .LoadAll<Sprite>($"Images/UI/cooldownsandhealth");

                FlyPowerProgressBar.BackgroundImage.sprite = sprites[spriteIndex];
                FlyPowerProgressBar.ForegroundImage.sprite = sprites[spriteIndex + 3];
            }
        }

        protected void RemoveFlyPowers()
        {
            ActualJumpVelocity = JumpVelocity;

            FlyPower = FlyType.None;

            UpdateUI();
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
            soundEffects.SetDistance(0.0f);
            soundEffects.Jump();

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

        protected void Die()
        {
            PauseGame(true);

            GameOverPanel
                .gameObject
                .SetActive(true);
        }

        protected void Restart()
        {
            RigidBody.velocity = new Vector2(0, 0);
            FliesEaten = 0;
            ImpactVelocity = 0;
            ActualJumpVelocity = JumpVelocity;
            FlyPower = FlyType.None;
            FlyPowerTimer = 0;
            JumpMoveTimer = 0;
            IsJumping = false;
            JumpStartY = 0;
            IsJumpRequested = false;
            IsHopping = false;
            HopStartY = 0;
            IsCoyoteTime = false;
            CoyoteTimer = 0;
            IsOnGround = false;
            Health = MaxHealth;
            transform.position = RespawnPosition;
            SetDirection(1);

            UpdateUI();
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

        protected void DoFlyPower()
        {
            if (FlyPower == FlyType.None)
            {
                return;
            }

            if (FlyPowerTimer <= 0)
            {
                return;
            }

            if (FlyPowerCooldown > 0)
            {
                return;
            }

            switch (FlyPower)
            {
                case FlyType.Cherry:
                    ThrowCherryBomb();
                    break;
            }
        
            FlyPowerCooldown = FlyPowerCooldownsAmount[(int)FlyPower];
        }

        protected void ThrowCherryBomb()
        {
            var bomb = CherryBombPool
                .GetPooledObject()
                .GetComponent<CherryBombBehaviour>();

            if (bomb == null)
            {
                return;
            }

            bomb.transform.position = transform.position + new Vector3(0.32f * Direction, 0.32f, 0);

            bomb
                .Throw(Direction);
        }

        protected void DoGroundTest()
        {
            Collider2D collider;
            var o = new Vector2(transform.position.x, transform.position.y + 0.16f);
            var s = new Vector2(0.32f, 0.32f);

            var raycastHit = Physics2D
                .BoxCast(o, s, 0, Vector2.down, 0.04f, GroundLayer);

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
                    soundEffects
                        .Death();

                    Die();

                    return;
                }

                if (GroundCollider
                    .name
                    .ToLower()
                    .Contains("water"))
                {
                    soundEffects
                        .Splash();

                    Die();

                    return;
                }

                // TO DO - USE IMPACT VELOCITY FOR?
                ImpactVelocity = 0;

                if (IsJumping)
                {
                    IsJumping = false;

                    soundEffects
                        .Land();
                }

                if (IsHopping)
                {
                    IsHopping = false;
                }

                IsCoyoteTime = false;
                IsOnGround = true;

                MovingPlatformBehaviour movingPlatform = GroundCollider
                    .GetComponent<MovingPlatformBehaviour>();
                if (movingPlatform != null)
                {
                    RigidBody.velocity = movingPlatform.RigidBody.velocity;
                }

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

            /*
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
            */
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

            LevelManger.LevelStarted += LevelManger_LevelStarted;

            soundEffects = FindObjectOfType<SoundEffects>(true);
            if (soundEffects == null) Debug.Log("soundEffects is null");
            else Debug.Log("soundEffects: " + soundEffects.ToString());

            Animator = GetComponent<Animator>();

            Restart();

            SetDirection(1);

            Tongue.FlyGobbled += Tongue_FlyGobbled;

            ActualJumpVelocity = JumpVelocity;

            FlyPowerProgressBar
                .SetValue(0, FlyPowerDuration);

            HealthProgressBar
                .SetValue(Health, MaxHealth);

            FliesEaten = 0;
            FliesEatenHasChanged?.Invoke(FliesEaten);

            foreach (var upgrade in Upgrades)
            {
                upgrade.Level = 0;
            }

            UpdateUI();
        }

        protected void Update()
        {
            if (GameManager
                .Instance
                .IsPaused)
            {
                return;
            }

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

            if (FlyPowerCooldown > 0)
            {
                FlyPowerCooldown -= Time.deltaTime;
                if (FlyPowerCooldown <= 0)
                {
                    FlyPowerCooldown = 0;
                }
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
                Die();
            }

            if (Input
                .GetKeyDown(TongueKey))
            {
                Tongue
                    .Shoot(Direction);
            }

            if (Input
               .GetKeyDown(FlyPowerKey))
            {
                DoFlyPower();
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