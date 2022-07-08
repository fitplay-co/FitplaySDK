using System;
using UnityEngine;

namespace StandTravelModel.TestDemo
{
    public class ExMoveController : MonoBehaviour
    {
        private CharacterController charaControl;
        private StandTravelModelManager standTravelModelManager;
        private Vector3 deltaMovement;

        public void Awake()
        {
            charaControl = GetComponent<CharacterController>();
            standTravelModelManager = GetComponent<StandTravelModelManager>();
        }

        public void Update()
        {
            if (charaControl != null && standTravelModelManager != null)
            {
                deltaMovement = standTravelModelManager.GetMoveVelocity() * Time.deltaTime;
                charaControl.Move(deltaMovement);
            }
        }
    }
}