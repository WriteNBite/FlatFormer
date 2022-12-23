using UnityEngine;

namespace DefaultNamespace
{
    public class HealScript : MonoBehaviour
    {
        public int healingPoints = 5;

        public int Heal()
        {
            return healingPoints;
        }
    }
}