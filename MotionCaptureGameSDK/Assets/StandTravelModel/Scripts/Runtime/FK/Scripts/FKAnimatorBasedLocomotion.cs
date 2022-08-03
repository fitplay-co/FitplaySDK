using System.Collections.Generic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.FK.Scripts
{
    public class FKAnimatorBasedLocomotion : MonoBehaviour
    {
        // Start is called before the first frame update
        public Transform bindAvatar;
        public Transform bindHipCenter;
        public Transform bindLeftFoot;
        public Transform bindRightFoot;

        public float motionScale = 1.0f;

        public bool isDebug;
        public bool grounding;

        public GameObject debugLeft1;
        public GameObject debugLeft2;
        public GameObject debugLeft3;
        public GameObject debugRight1;
        public GameObject debugRight2;
        public GameObject debugRight3;

        float previousOSGroundx = 0.0f;
        float previousOSGroundz = 0.0f;
        float currentGroundx = 0.0f;
        float currentGroundz = 0.0f;
    
        private Vector3 predictHipPos = Vector3.zero;
        private Vector3 predictLFootPos = Vector3.zero;
        private Vector3 predictRFootPos = Vector3.zero;

        private Vector3 previousFramePositionFootAvatarL = new Vector3(0, 0 ,0);
        private Vector3 previousFramePositionFootAvatarR = new Vector3(0, 0, 0);
    
        private Vector3 previousFrameLFoot2d = new Vector3(0, 0, 0);
        private Vector3 previousFrameRFoot2d = new Vector3(0, 0, 0);
        private Vector3 lfootFrameOffset = new Vector3(0, 0, 0);
        private Vector3 rfootFrameOffset = new Vector3(0, 0, 0);
        private float squareSpeedLeftFootDetector = 0.0f;
        private float squareSpeedRightFootDetector = 0.0f;

        //simple foot lock grounder, you can involve more gorunder calculation based on foot lock detection 
        private bool leftSideFootLock = false;
        private bool rightSideFootLock = false;
        private int lastLockFoot = 0; //0 for left leg and 1 for right leg , if not jump, will have at least one lock foot

        void Start() {
        
        }

        // public void resetGround {
        //     currentGroundx = 0.0f;
        //     currentGroundz = 0.0f;
        // }

        public void updateGroundLocationHint(IMotionDataModel motionDataModel) {
            GroundLocationItem groundLocationItem = motionDataModel.GetGroundLocationData();
            ActionDetectionItem actionDetectionData = motionDataModel.GetActionDetectionData();
            List<Vector3> keyPoints = motionDataModel.GetIKPointsData(false, true);

            WalkActionItem walkDetection = actionDetectionData.walk;
            float xHipFrameOffset = groundLocationItem.x - previousOSGroundx;
            float zHipFrameOffset = groundLocationItem.z - previousOSGroundz;
        
            currentGroundx = currentGroundx + xHipFrameOffset;
            currentGroundz = currentGroundz + zHipFrameOffset;

            predictHipPos.x = - currentGroundx;
            predictHipPos.z = - currentGroundz;
            predictHipPos.y = 0;

            //3 update position to hip position
            // bindAvatar.position = predictHipPos;

            //4 predict foot lock 
            Vector3 leftFoot2d = keyPoints[(int)GameKeyPointsType.LeftFootIndex];
            Vector3 rightfoot2d = keyPoints[(int)GameKeyPointsType.RightFootIndex];
            bool leftFoot2dStable = false;
            bool rightFoot2dStable = false;
            bool footLockChange = false;

            float squareSpeedThisFrameLeft = Mathf.Abs(leftFoot2d.x - previousFrameLFoot2d.x) + Mathf.Abs((leftFoot2d.y - previousFrameLFoot2d.y));
            float squareSpeedThisFrameRight = Mathf.Abs(rightfoot2d.x - previousFrameRFoot2d.x) + Mathf.Abs((rightfoot2d.y - previousFrameRFoot2d.y));
            squareSpeedLeftFootDetector = squareSpeedLeftFootDetector * 0.7f + squareSpeedThisFrameLeft * 0.3f;
            squareSpeedRightFootDetector = squareSpeedRightFootDetector * 0.7f + squareSpeedThisFrameRight * 0.3f;

            if(squareSpeedLeftFootDetector < squareSpeedRightFootDetector) {
                leftFoot2dStable = true;
                if(lastLockFoot == 1) {
                    footLockChange = true;
                }
                lastLockFoot = 0;
            } else {
                rightFoot2dStable = true;
                if(lastLockFoot == 0) {
                    footLockChange = true;
                }
                lastLockFoot = 1;
            }

            if(squareSpeedLeftFootDetector < 0.001 && squareSpeedRightFootDetector < 0.001) {
                leftFoot2dStable = true; 
                rightFoot2dStable = true;
            }

            if(walkDetection.leftLeg == 0 && leftFoot2dStable) {
                leftSideFootLock = true;
            } else {
                leftSideFootLock = false;
            }

            if(walkDetection.rightLeg == 0 && rightFoot2dStable) {
                rightSideFootLock = true;
            } else {
                rightSideFootLock = false;
            }

            previousOSGroundx = groundLocationItem.x;
            previousOSGroundz = groundLocationItem.z;
            previousFrameLFoot2d = leftFoot2d;
            previousFrameRFoot2d = rightfoot2d;

            if(isDebug) {
                // Debug.Log("predict location x : " + bindHip.position.x + " z: " + bindHip.position.z);
                if(footLockChange) {
                    if(lastLockFoot == 0) {
                        debugLeft1.SetActive(true);
                        debugLeft2.SetActive(false);
                    } else {
                        debugLeft1.SetActive(false);
                        debugLeft2.SetActive(true);
                    }

                    if(lastLockFoot == 1) {
                        debugRight1.SetActive(true);
                        debugRight2.SetActive(false);
                    } else {
                        debugRight1.SetActive(false);
                        debugRight2.SetActive(true);
                    }
                }

                // if(walkDetection.leftLeg == 1) {
                //     debugLeft1.SetActive(true);
                // }
                // if(walkDetection.leftLeg == 0) {
                //     debugLeft2.SetActive(true);
                // }
                // if(walkDetection.leftLeg == -1) {
                //     debugLeft3.SetActive(true);
                // }
                //  if(walkDetection.rightLeg == 1) {
                //     debugRight1.SetActive(true);
                // }
                // if(walkDetection.rightLeg == 0) {
                //     debugRight2.SetActive(true);
                // }
                // if(walkDetection.rightLeg == -1) {
                //     debugRight3.SetActive(true);
                // }
            }
        }

        // Update is called once per frame
        void Update() {
       
        }
    
        // In late update round apply grounder 
        void LateUpdate() {
            if(grounding) {
                Vector3 positionFromHipToFootL = bindLeftFoot.position - bindHipCenter.position;
                Vector3 positionFromHipToFootR = bindRightFoot.position - bindHipCenter.position;

                lfootFrameOffset = positionFromHipToFootL - previousFramePositionFootAvatarL;
                rfootFrameOffset = positionFromHipToFootR - previousFramePositionFootAvatarR;
                if(isDebug) {
                    Debug.Log("current: x " + previousFramePositionFootAvatarL.x + " y " + previousFramePositionFootAvatarL.y + " z " + previousFramePositionFootAvatarL.z);
                    Debug.Log("prvious: x " + previousFrameLFoot2d.x + " y " + previousFrameLFoot2d.y + " z " + previousFrameLFoot2d.z);
                    Debug.Log("offset : x " + lfootFrameOffset.x + " y " + lfootFrameOffset.y + " z " + lfootFrameOffset.z);
                }

                previousFramePositionFootAvatarL = positionFromHipToFootL;
                previousFramePositionFootAvatarR = positionFromHipToFootR;

                float bindFootGrounder = bindLeftFoot.position.y;
                if(bindLeftFoot.position.y < bindRightFoot.position.y) {
                    bindFootGrounder = bindLeftFoot.position.y;
                } else {
                    bindFootGrounder = bindRightFoot.position.y;
                }

                if(lastLockFoot == 0 ){
                    //last lock foot left 
                    //grounding 
                    bindHipCenter.position -= new Vector3(0, bindFootGrounder , 0);
                    bindAvatar.position -= new Vector3(lfootFrameOffset.x, 0 , lfootFrameOffset.z);
                }  else {
                    // bindHip.position += lfootFrameOffset;
                    bindHipCenter.position -= new Vector3(0, bindFootGrounder, 0);
                    bindAvatar.position -= new Vector3(rfootFrameOffset.x, 0 , rfootFrameOffset.z);
                }
            
                if(isDebug) {
                    // Debug.Log("after lfootOffset: x " + lfootFrameOffset.x + " y " + lfootFrameOffset.y + " z " + lfootFrameOffset.z);
                }
            }
        }

    }
}
