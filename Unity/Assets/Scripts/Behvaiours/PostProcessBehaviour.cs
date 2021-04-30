namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("KasJam/PostProcess")]
    public class PostProcessBehaviour : BehaviourBase
    {
        #region Members

        public Material Material;

        #endregion
        
        #region Unuity

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics
                .Blit(source, destination, Material);
        }

        #endregion
    }
}