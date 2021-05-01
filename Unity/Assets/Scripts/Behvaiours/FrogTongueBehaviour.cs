namespace KasJam.MiniJam79.Unity.Behaviours
{
    using KasJam.MiniJam79.Unity.Events;
    using KasJam.MiniJam79.Unity.Managers;
    using System;
    using UnityEngine;

    [AddComponentMenu("KasJam/FrogTongue")]
    public class FrogTongueBehaviour : BehaviourBase
    {
        #region Members

        public SpriteRenderer SpriteRenderer;

        public BoxCollider2D BoxCollider2d;

        public float ExpandSpeed;

        public int PixelsPerExpand;
        public int RetractPixelsPerExpand;

        public int MaxLength;

        protected FlyBehaviour CapturedFly { get; set; }

        protected bool IsExpanding { get; set; }
        
        protected bool IsShooting { get; set; }

        protected int Length { get; set; }

        protected float ExpandTimer { get; set; }

        protected int Direction { get; set; }

        #endregion

        #region Events

        public event EventHandler<FlyBehaviourEventArgs> FlyGobbled;
        private SoundEffects soundEffects;

        protected void OnFlyGobbled(FlyBehaviour fly)
        {
            FlyGobbled?
                .Invoke(this, new FlyBehaviourEventArgs { Fly = fly });
        }

        #endregion

        #region Public Methods

        public void Shoot(int direction)
        {
            if (IsShooting)
            {
                return;
            }

            if (CapturedFly != null)
            {
                return;
            }

            SetDirection(direction);

            Length = 0;
            IsShooting = true;
            IsExpanding = true;
            ExpandTimer = 0;

            soundEffects.SetDistance(direction * 1.0f);
            soundEffects.Tongue();

            UpdateSprite();
        }

        public void SetDirection(int direction)
        {
            if (direction == 1)
            {
                transform.localPosition = new Vector3(0.06f, 0.20f, 0);
                SpriteRenderer.flipX = false;
            }
            else
            {
                transform.localPosition = new Vector3(-0.06f, 0.20f, 0);
                SpriteRenderer.flipX = true;
            }

            Direction = direction;
        }

        #endregion

        #region Protected Methods

        protected void UpdateSprite()
        {
            SpriteRenderer.size = new Vector2(0.02f * Length, 0.02f);

            if (IsShooting)
            {
                BoxCollider2d.size = new Vector2(0.08f, 0.08f);
                BoxCollider2d.offset = new Vector2(Length * 0.02f * Direction, 0);
            }
            else
            {
                BoxCollider2d.size = new Vector2(0, 0);
                BoxCollider2d.offset = new Vector2(0, 0);
            }

            if (CapturedFly != null)
            {
                var position = transform.position;

                position.x += Length * 0.04f * Direction;

                CapturedFly.transform.position = position;
            }
        }

        #endregion

        #region Unity

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            // TODO - USE THIS?
        }

        protected void OnTriggerEnter2D(Collider2D collider)
        {
            if (CapturedFly != null)
            {
                return;
            }

            var fly = collider
                .GetComponent<FlyBehaviour>();

            if (fly == null)
            {
                return;
            }

            CapturedFly = fly;
            CapturedFly.IsCaptured = true;
        }

        protected void Update()
        {
            if (GameManager
                .Instance
                .IsPaused)
            {
                return;
            }

            if (!IsShooting)
            {
                return;
            }

            ExpandTimer += Time.deltaTime;
            if (ExpandTimer >= ExpandSpeed)
            {
                ExpandTimer -= ExpandSpeed;

                if (IsExpanding)
                {
                    Length += PixelsPerExpand;
                    if (Length >= MaxLength)
                    {
                        Length = MaxLength;
                        IsExpanding = false;
                    }
                }
                else
                {
                    Length -= RetractPixelsPerExpand;
                    if (Length <= 0)
                    {
                        Length = 0;
                        IsShooting = false;
                        if (CapturedFly != null)
                        {
                            OnFlyGobbled(CapturedFly);

                            CapturedFly
                                .gameObject
                                .SetActive(false);

                            CapturedFly = null;
                        }
                    }
                }
            }

            UpdateSprite();
        }

        protected override void Awake()
        {
            base
                .Awake();

            soundEffects = FindObjectOfType<SoundEffects>();

            IsShooting = false;
            IsExpanding = false;
            Length = 0;

            UpdateSprite();
        }

        #endregion
    }
}
