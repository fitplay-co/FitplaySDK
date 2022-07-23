using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MotionCaptureBasic;
using MotionCaptureBasic.Interface;
using MotionCaptureBasic.OSConnector;

public class FKAnimatorTestBehavior : MonoBehaviour
{
    private IMotionDataModel motionDataModel;
    private bool _osConnected = false;
    public bool osConnected => _osConnected;
    public FKAnimatorIk fkSolver = null;
    public bool isDebug = false;

    public void Awake() {
        InitMotionDataModel();
    }

    // Start is called before the first frame update
    void Start() {
        motionDataModel.SetDebug(isDebug);
    }

    // Update is called once per frame
    void Update() {
        Fitting fittingFrame = motionDataModel.GetFitting();
        List<Vector3> ikData = motionDataModel.GetIKPointsData(true, false);
        if(fittingFrame != null) {
            if(fkSolver != null) {
                fkSolver.updateFkInfo(fittingFrame, ikData);
            }
        }
    }

     public void ResetGroundLocation() {
        motionDataModel.ResetGroundLocation();
    }

    private void InitMotionDataModel() {
        motionDataModel = MotionDataModelFactory.Create(MotionDataModelType.Http);
        motionDataModel.AddConnectEvent(SubscribeMessage);
    }

    private void SubscribeMessage() {
        motionDataModel.SubscribeActionDetection();
        motionDataModel.SubscribeGroundLocation();
        motionDataModel.SubscribeFitting();
        _osConnected = true;
    }
}
