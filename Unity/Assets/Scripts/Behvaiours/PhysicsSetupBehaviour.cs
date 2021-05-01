namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/PhysicsSetup")]
    public class PhysicsSetupBehaviour : BehaviourBase
    {
        #region Members

        public int PlayerLayer;
        public int CherryBombLayer;
        public int LemonSquirtLayer;

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();


            Physics2D
                .IgnoreLayerCollision(PlayerLayer, CherryBombLayer);

            Physics2D
                .IgnoreLayerCollision(PlayerLayer, LemonSquirtLayer);

            Physics2D
                .IgnoreLayerCollision(CherryBombLayer, LemonSquirtLayer);

            Physics2D
                .IgnoreLayerCollision(LemonSquirtLayer, LemonSquirtLayer);
        }

        #endregion
    }
}