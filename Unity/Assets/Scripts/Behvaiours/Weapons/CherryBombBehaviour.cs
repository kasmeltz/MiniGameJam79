namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    public class CherryBombBehaviour : MonoBehaviour
    {
        #region Members

        public Vector2 ThrowForce;

        public float ExplosionRange;       

        #endregion

        public void Throw(int Direction)
        {
            var force = ThrowForce;

            force.x *= Direction;

            var rigidBody = GetComponent<Rigidbody2D>();

            rigidBody
                .AddForce(force, ForceMode2D.Impulse);

            rigidBody.angularVelocity = Random
                .Range(-360, 360);
        }

        public void Exploded()
        {
            // TODO - DAMAGE TO ENEMIES

            gameObject
                .SetActive(false);
        }
    }
}