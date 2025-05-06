using UnityEngine;

namespace Airport.Development
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;

        public void Spawn()
        {
            Instantiate(prefab, transform.position, transform.rotation);
        }
    }
}