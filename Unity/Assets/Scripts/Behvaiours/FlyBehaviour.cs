namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/Fly")]
    public class FlyBehaviour : BehaviourBase
    {
        #region Members

        public bool IsCaptured { get; set; }

        public float MaxMoveSpeedX;
        
        public float MaxMoveSpeedY;

        #endregion

        #region Unity

        protected void Update()
        {
            if (IsCaptured)
            {
                return;
            }

            float x = Random.Range(-MaxMoveSpeedX, MaxMoveSpeedX);
            float y = Random.Range(-MaxMoveSpeedY, MaxMoveSpeedY);

            x *= Time.deltaTime;
            y *= Time.deltaTime;

            transform.position += new Vector3(x, y, 0);
        }

        #endregion
    }
}