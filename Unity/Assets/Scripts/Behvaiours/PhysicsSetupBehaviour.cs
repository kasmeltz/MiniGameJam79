namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/PhysicsSetup")]
    public class PhysicsSetupBehaviour : BehaviourBase
    {
        #region Members

        public int PlayerLayer;
        public int CherryBombLayer;

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            Physics2D
                .IgnoreLayerCollision(PlayerLayer, CherryBombLayer);
        }

       #endregion
    }
}