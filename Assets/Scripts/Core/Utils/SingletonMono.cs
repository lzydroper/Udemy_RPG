using UnityEngine;

namespace Core.Utils
{
    public abstract class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private bool isDontDestroyOnLoad = true;
        public static T I { get; private set; }

        protected virtual void Awake()
        {
            if (I != null && I != this as T)
            {
                Debug.LogWarning($"[Singleton] 发现重复的 {typeof(T).Name} 实例，销毁后来者：{gameObject.name}");
                Destroy(gameObject);
                return;
            }
            I = this as T;
            if (isDontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }
    }
}