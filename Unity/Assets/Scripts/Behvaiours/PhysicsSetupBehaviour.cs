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
        public int EnemyLayer;
        public int EnemyWeaponLayer;

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            Physics2D
                .IgnoreLayerCollision(PlayerLayer, CherryBombLayer);

            Physics2D
                .IgnoreLayerCollision(PlayerLayer, EnemyLayer);

            Physics2D
                .IgnoreLayerCollision(PlayerLayer, LemonSquirtLayer);

            Physics2D
                .IgnoreLayerCollision(CherryBombLayer, LemonSquirtLayer);

            Physics2D
                .IgnoreLayerCollision(LemonSquirtLayer, LemonSquirtLayer);

            Physics2D
                .IgnoreLayerCollision(EnemyLayer, LemonSquirtLayer);

            Physics2D
                .IgnoreLayerCollision(EnemyLayer, EnemyLayer);

            Physics2D
                .IgnoreLayerCollision(EnemyWeaponLayer, EnemyLayer);

            Physics2D
                .IgnoreLayerCollision(EnemyWeaponLayer, CherryBombLayer);

            Physics2D
                .IgnoreLayerCollision(EnemyWeaponLayer, LemonSquirtLayer);
        }

        #endregion
    }
}