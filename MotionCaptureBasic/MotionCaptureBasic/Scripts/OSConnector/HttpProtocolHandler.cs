using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using FlatBuffers;
using Newtonsoft.Json;
using OsOutput;
using PoseData;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MotionCaptureBasic.OSConnector
{
    public class HttpProtocolHandler
    {
        private static HttpProtocolHandler instance;
        private static readonly object _Synchronized = new object();

        private bool isDebug;
        //private float lastTime;
        private Action onConnect;
        private Action onClosed;
        private Action onError;
        private IKBodyUpdateMessage _bodyMessageBase;

        private const int PoseLandmarkLength = 33;
        private const int FittingLength = 18;

        public IKBodyUpdateMessage BodyMessageBase => _bodyMessageBase;

        public static readonly string OsIpKeyName = "OsAddress";

        private HttpProtocolHandler()
        {
            _bodyMessageBase = new IKBodyUpdateMessage
            {
                fitting = new Fitting
                {
                    keypoints3D = RepeatedDefaultInstance<FittingPositionItem>(FittingLength),
                    rotation = RepeatedDefaultInstance<FittingRotationItem>(FittingLength),
                    mirrorRotation = RepeatedDefaultInstance<FittingRotationItem>(FittingLength),
                    localRotation = RepeatedDefaultInstance<FittingRotationItem>(FittingLength),
                    mirrorLocalRotation = RepeatedDefaultInstance<FittingRotationItem>(FittingLength)
                },
                pose_landmark = new PoseLandmarkItem
                {
                    keypoints = RepeatedDefaultInstance<KeyPointItem>(PoseLandmarkLength),
                    keypoints3D = RepeatedDefaultInstance<KeyPointItem>(PoseLandmarkLength),
                    timestamp = 0,
                    version = ""
                },
                timeProfiling = new TimeProfiling(),
                type = "",
                ground_location = new GroundLocationItem(),
                gaze_tracking = new GazeTracking(),
                action_detection = new ActionDetectionItem
                {
                    version = "",
                    walk = new WalkActionItem(),
                    jump = new JumpActionItem()
                },
                monitor = new MonitorItem(),
                stand_detection = new StandDetection(),
                general_detection = new GeneralDetectionItem()
            };
        }
        
        public enum ConnectStatus
        {
            OnConnected,
            OnError,
            OnDisConnect
            
        }

        ~HttpProtocolHandler()
        {
        }

        public void StartWebSocket(string url, bool useJson)
        {
            var app = WebsocketOSClient.GetInstance();
            app.SetUseJson(useJson);
            app.InitConnect(url);
            app.OnReceived += OnReceived;
            app.OnReceivedBytes += OnReceivedBytes;
            app.OnConnect += OnConnect;
            app.OnClosed += OnClosed;
            app.OnError += OnError;
        }

        public void ReleaseWebSocket()
        {
            var app = WebsocketOSClient.GetInstance();
            app.OnReceived -= OnReceived;
            app.OnReceivedBytes -= OnReceivedBytes;
            app.OnConnect -= OnConnect;
            app.OnClosed -= OnClosed;
            app.OnError -= OnError;
            app.ReleaseConnect();
        }

        public static HttpProtocolHandler GetInstance()
        {
            if (instance == null)
            {
                lock(_Synchronized)
                {
                    if(instance == null)
                    {
                        instance = new HttpProtocolHandler();
                    }
                }
            }

            return instance;
        }
        
        public void AddConnectEvent(Action onConnect, Action onClosed = null, Action onError = null)
        {
            this.onConnect += onConnect;
            this.onClosed += onClosed;
            this.onError += onError;
        }

        public void SetDebug(bool isDebug)
        {
            this.isDebug = isDebug;
        }

        private void WriteLogFile(string logContent)
        {
            using (FileStream fileStream = new FileStream("Logs/OsMsgLog.log", FileMode.Append))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.WriteLine(logContent);
                }
            }
        }

        private void OnReceived(string message)
        {
            //var diff = Time.time - lastTime;
            //lastTime = Time.time;
            //message = RemoveBetween(message, "\"flatbuffersData\":{", "},");
            if (string.IsNullOrEmpty(message)) return;
            if (isDebug)
            {
                WriteLogFile(message);
            }
            //只解析数据类型
            //目前有三种类型数据，camera, imu, input
            MessageType dataType = Protocol.UnMarshalType(message);
            if (dataType == null)
            {
                Debug.LogError("Data Error, don't include sensor_type!");
                return;
            }
            //imu和input数据处理通道
            if (dataType.sensor_type != null)
            {
                string sType = dataType.sensor_type.ToLower();
                if (sType == SensorType.IMU || sType == SensorType.INPUT)
                {
                    BasicEventHandler.DispatchImuDataEvent(message);
                    return;
                }
            }
            
            //动捕数据处理通道
            _bodyMessageBase = Protocol.UnMarshal(message) as IKBodyUpdateMessage;
            //var d  = _bodyMessageBase.timeProfiling.beforeSendTime - _bodyMessageBase.timeProfiling.startTime;
            //var nowTime = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
           
            //     Console.WriteLine("本级时间戳-startTime:" + (nowTime -  _bodyMessageBase.timeProfiling.startTime) + "，" + nowTime + " ，" + _bodyMessageBase.timeProfiling.startTime);
            //     Console.WriteLine($"上一帧和当前帧相差时间：{diff * 1000} 毫秒,服务器处理的时间：{d } 毫秒");
        }
        
        private string RemoveBetween(string s, string begin, string end) 
        {
            Regex r = new Regex(begin);
            Match m = r.Match(s); 
            if (m.Success)
            {
                int start = m.Index;
                r = new Regex(end);
                m = r.Match(s, start + begin.Length);
                if (m.Success)
                {
                    int e = m.Index;
                    return s.Remove(start, e - start + end.Length);
                }
            }
            return s;
        }

        private void OnReceivedBytes(byte[] streamData)
        {
            if (streamData.Length == 0)
            {
                Debug.LogError("flatbuffers data invalid, because stream length is 0");
                return;
            }

            var buf = new ByteBuffer(streamData);
            
            OutputMessage outputMessage = OutputMessage.GetRootAsOutputMessage(buf);

            var poseData = outputMessage.Pose;
            var actionData = outputMessage.DetectionResult;
            var timeProfilingData = outputMessage.TimeProfiling;

            #region Convert pose landmark data

            if (poseData?.KeypointsLength != PoseLandmarkLength || 
                poseData.Value.Keypoints3DLength != PoseLandmarkLength)
            {
                Debug.LogError("flatbuffers data invalid, pose landmark data has error");
                return;
            }

            var poseLandmarkItem = _bodyMessageBase.pose_landmark;

            poseLandmarkItem.keypoints = KeyPointListConverter(poseLandmarkItem.keypoints, poseData.Value.Keypoints);
            poseLandmarkItem.keypoints3D =
                KeyPointListConverter(poseLandmarkItem.keypoints3D, poseData.Value.Keypoints3D);

            _bodyMessageBase.pose_landmark = poseLandmarkItem;

            #endregion

            #region Convert fitting data

            var fittingData = actionData?.Fitting;

            if (fittingData?.RotationLength != FittingLength ||
                fittingData.Value.LocalRotationLength != FittingLength ||
                fittingData.Value.MirrorRotationLength != FittingLength ||
                fittingData.Value.MirrorLocalRotationLength != FittingLength/* ||
                fittingData.Value.FittedLandmarksLength != FittingLength*/)
            {
                Debug.LogError("flatbuffers data invalid, fitting data has error");
                return;
            }

            var fittingItem = _bodyMessageBase.fitting;

            fittingItem.rotation = FKJointListConverter(fittingItem.rotation, fittingData.Value.Rotation);
            fittingItem.localRotation =
                FKJointListConverter(fittingItem.localRotation, fittingData.Value.LocalRotation);
            fittingItem.mirrorRotation =
                FKJointListConverter(fittingItem.mirrorRotation, fittingData.Value.MirrorRotation);
            fittingItem.mirrorLocalRotation =
                FKJointListConverter(fittingItem.mirrorLocalRotation, fittingData.Value.MirrorLocalRotation);

            /*var fkKeypoints3D = fittingItem.keypoints3D;
            for (int i = 0; i < FittingLength; i++)
            {
                var fkLandmark = fittingData.Value.FittedLandmarks(i);

                if (fkLandmark != null)
                {
                    var fkKeypoints3DItem = fkKeypoints3D[i];
                    fkKeypoints3DItem.x = fkLandmark.Value.X;
                    fkKeypoints3DItem.y = fkLandmark.Value.Y;
                    fkKeypoints3DItem.z = fkLandmark.Value.Z;
                    fkKeypoints3DItem.name = fkLandmark.Value.Name;
                    fkKeypoints3D[i] = fkKeypoints3DItem;
                }
            }

            fittingItem.keypoints3D = fkKeypoints3D;*/

            _bodyMessageBase.fitting = fittingItem;

            #endregion

            #region Convert time profiling data

            if (timeProfilingData != null)
            {
                var timeProfilingItem = _bodyMessageBase.timeProfiling;
                timeProfilingItem.beforeSendTime = timeProfilingData.Value.BeforeSendTime;
                timeProfilingItem.processingTime = timeProfilingData.Value.ProcessingTime;
                _bodyMessageBase.timeProfiling = timeProfilingItem;
            }
            else if (isDebug)
            {
                Debug.LogWarning("no time profiling data");
            }

            #endregion

            #region Convert ground location data

            var groundLocationData = actionData?.Ground;

            if (groundLocationData != null)
            {
                var groundLocationItem = _bodyMessageBase.ground_location;
                groundLocationItem.x = groundLocationData.Value.X;
                groundLocationItem.y = groundLocationData.Value.Y;
                groundLocationItem.z = groundLocationData.Value.Z;
                //groundLocationItem.tracing = groundLocationData.Value.Tracing;
                groundLocationItem.legLength = groundLocationData.Value.LegLength;
                _bodyMessageBase.ground_location = groundLocationItem;
            }
            else if (isDebug)
            {
                Debug.LogWarning("no ground location data");
            }

            #endregion

            #region Convert gaze tracking data

            var gazeTrackingData = actionData?.Gaze;

            if (gazeTrackingData != null)
            {
                var gazeTrackingItem = _bodyMessageBase.gaze_tracking;
                gazeTrackingItem.x = gazeTrackingData.Value.X;
                gazeTrackingItem.y = gazeTrackingData.Value.Y;
                gazeTrackingItem.z = gazeTrackingData.Value.Z;
                _bodyMessageBase.gaze_tracking = gazeTrackingItem;
            }
            else if (isDebug)
            {
                Debug.LogWarning("no gaze tracking data");
            }

            #endregion

            #region Convert action detection data

            var actionDetectionItem = _bodyMessageBase.action_detection;
            
            var walkData = actionData?.Walk;
            if (walkData != null)
            {
                var walkItem = actionDetectionItem.walk;
                walkItem.leftLeg = walkData.Value.LeftLeg;
                walkItem.rightLeg = walkData.Value.RightLeg;
                walkItem.leftStepLength = walkData.Value.LeftStepLength;
                walkItem.rightStepLength = walkData.Value.RightStepLength;
                walkItem.leftHipAng = walkData.Value.LeftHipAng;
                walkItem.rightHipAng = walkData.Value.RightHipAng;
                walkItem.leftFrequency = walkData.Value.LeftFrequency;
                walkItem.rightFrequency = walkData.Value.RightFrequency;
                walkItem.velocity = walkData.Value.Velocity;
                walkItem.velocityThreshold = walkData.Value.VelocityThreshold;
                walkItem.stepRate = walkData.Value.StepRate;
                walkItem.stepLen = walkData.Value.StepLen;
                walkItem.realtimeLeftLeg = walkData.Value.RealtimeLeftLeg;
                walkItem.realtimeRightLeg = walkData.Value.RealtimeRightLeg;
                actionDetectionItem.walk = walkItem;
            }
            else if (isDebug)
            {
                Debug.LogWarning("no walk data");
            }

            var jumpData = actionData?.Jump;
            if (jumpData != null)
            {
                var jumpItem = actionDetectionItem.jump;
                jumpItem.velocity = jumpData.Value.Velocity;
                jumpItem.onTheGround = jumpData.Value.OnTheGround;
                actionDetectionItem.jump = jumpItem;
            }
            else if (isDebug)
            {
                Debug.LogWarning("no jump data");
            }

            _bodyMessageBase.action_detection = actionDetectionItem;

            #endregion

            #region Convert stand detection data

            var standDetectionData = actionData?.Stand;
            if (standDetectionData != null)
            {
                var standDetectionItem = _bodyMessageBase.stand_detection;
                standDetectionItem.mode = standDetectionData.Value.Mode;
                _bodyMessageBase.stand_detection = standDetectionItem;
            }
            else if (isDebug)
            {
                Debug.LogWarning("no stand detection data");
            }

            #endregion

            #region Convert general detection data

            var generalDetectionData = actionData?.General;
            if (generalDetectionData != null)
            {
                var generalDetectionItem = _bodyMessageBase.general_detection;
                generalDetectionItem.confidence = (int) generalDetectionData.Value.Confidence;
                _bodyMessageBase.general_detection = generalDetectionItem;
            }
            else if (isDebug)
            {
                Debug.LogWarning("no general detection data");
            }

            #endregion

            if (isDebug)
            {
                var jsonData = JsonConvert.SerializeObject(_bodyMessageBase/*, Formatting.None,
                    new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}*/);
                WriteLogFile(jsonData);
            }
        }

        private delegate PoseData.Point? GetPointDelegate(int i);

        private List<KeyPointItem> KeyPointListConverter(List<KeyPointItem> origin, GetPointDelegate getPoint)
        {
            for (int i = 0; i < PoseLandmarkLength; i++)
            {
                var point = getPoint(i);
                
                if (point != null)
                {
                    var item = origin[i];
                    item.x = point.Value.X;
                    item.y = point.Value.Y;
                    item.z = point.Value.Z;
                    item.score = point.Value.Score;
                    item.name = point.Value.Name;
                    origin[i] = item;
                }
            }

            return origin;
        }

        private delegate ActionData.Joint? GetJointDelegate(int i);

        private List<FittingRotationItem> FKJointListConverter(List<FittingRotationItem> origin,
            GetJointDelegate getJoint)
        {
            for (int i = 0; i < FittingLength; i++)
            {
                var joint = getJoint(i);

                if (joint != null)
                {
                    var item = origin[i];
                    item.x = joint.Value.X;
                    item.y = joint.Value.Y;
                    item.z = joint.Value.Z;
                    item.w = joint.Value.W;
                    item.name = joint.Value.Name;
                    origin[i] = item;
                }
            }

            return origin;
        }

        private List<T> RepeatedDefaultInstance<T>(int count)
        {
            List<T> ret = new List<T>(count);
            for (var i = 0; i < count; i++)
            {
                ret.Add((T)Activator.CreateInstance(typeof(T)));
            }
            return ret;
        }

        private void OnConnect()
        {
            onConnect?.Invoke();
        }
        
        private void OnClosed()
        {
            ReleaseWebSocket();
            onClosed?.Invoke();
        }
        
        private void OnError()
        {
            ReleaseWebSocket();
            onError?.Invoke();
        }
    }
}