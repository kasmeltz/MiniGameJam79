namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/CherryBomb")]
    public class CherryBombBehaviour : BehaviourBase
    {
        #region Members

        public Vector2 ThrowForce;

        public float ExplosionRange;

        public float ExplosionDamage;

        #endregion

        public void Throw(int Direction, float forceMultiplier)
        {
            var soundEffects = SoundEffects.Instance;
            Debug.Log("Throw: "+Direction);
            soundEffects.SetDistance(Direction * 0.5f);
            soundEffects.Throw();

            var force = ThrowForce * forceMultiplier;

            force.x *= Direction;

            GetComponent<SpriteRenderer>()
                .flipX = Direction != 1;

            var rigidBody = GetComponent<Rigidbody2D>();

            rigidBody
                .AddForce(force, ForceMode2D.Impulse);

            rigidBody.angularVelocity = Random
                .Range(-360, 360);
        }

        public void Exploded()
        {
            SoundEffects.Instance.Bomb();

            var enemies = FindObjectsOfType<EnemyBehaviour>();

            foreach(var enemy in enemies)
            {
                if(enemy == null)
                {
                    continue;
                }

                if (enemy.Health <= 0)
                {
                    continue;
                }

                float d = (transform.position - enemy.transform.position).magnitude;

                if (d <= ExplosionRange)
                {
                    enemy
                        .TakeDamage(ExplosionDamage, false);
                }
            }

            gameObject
                .SetActive(false);
        }
    }
}
