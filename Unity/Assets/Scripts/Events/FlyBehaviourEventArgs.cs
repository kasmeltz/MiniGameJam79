namespace KasJam.MiniJam79.Unity.Events
{
    using KasJam.MiniJam79.Unity.Behaviours;
    using System;

    public class FlyBehaviourEventArgs : EventArgs
    {
        public FlyBehaviour Fly { get; set; }
    }
}
