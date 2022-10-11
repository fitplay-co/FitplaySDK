using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Foot : MonoBehaviour
    {
        public int index;
        
        [NonSerialized]
        public Vector3[] track = new Vector3[2];

        public Vector3 Speed
        {
            get => (track[1] - track[0]) / Time.deltaTime;
        }

        private void Update()
        {
            track[0] = track[1];
            track[1] = transform.position;
        }
    }
}