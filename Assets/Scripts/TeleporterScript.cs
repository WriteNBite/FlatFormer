using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class TeleporterScript : MonoBehaviour
    {
        public Transform teleporter;

        public Vector2 getTeleporterPosition()
        {
            return teleporter.position;
        }
    }
}