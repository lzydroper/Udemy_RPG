using System.Collections.Generic;
using Core.Utils;
using UnityEngine;

namespace Core.Pool
{
    public class PoolDataBinder : MonoBehaviour
    {
        [SerializeField] private List<PoolDataSO> poolDataList;

        private void Start()
        {
            foreach (var data in poolDataList)
            {
                GameManager.Pool.Register(data);
            }
        }
    }
}