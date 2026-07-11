using Core.Pool;

namespace Core.Utils
{
    public class GameManager : SingletonMono<GameManager>
    {
        #region Variables

        private static PoolManager _poolManager;
        public static PoolManager Pool => _poolManager ??= new PoolManager();

        #endregion

        #region LifecycleMethods

        private void OnDestroy()
        {
            if (_poolManager != null)
            {
                _poolManager.Unregister();
                _poolManager = null;
            }
        }

        #endregion
    }
}