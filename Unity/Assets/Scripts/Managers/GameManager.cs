namespace KasJam.MiniJam79.Unity.Managers
{
    public class GameManager
    {
        #region Singleton

        private static volatile GameManager instance;
        private static object syncRoot = new object();

        private GameManager()
        {
        }

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new GameManager();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Public Members

        public bool IsPaused { get; set; }

        protected bool IsInitialized { get; set; }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            IsInitialized = true;
        }

        #endregion
    }
}
