using System;
using System.Collections.Generic;
using System.Threading;

namespace UnityEngine.Rendering.Universal
{
    public class IndirectDrawData
    {
        private static IndirectDrawData instance;

        private static readonly object _Synchronized = new object();

        public List<IndirectDrawInfo> IndirectDrawInfos;

        public ComputeBuffer IndirectDrawInfoBuff;

        public Dictionary<int, CameraBufferInfo> CameraBuffers;

        private int _totalCount = 0;

#if UNITY_EDITOR
        public bool IsStopCulling = false;
#endif

        public int InstanceTotalCount
        {
            get => _totalCount;
        }

        private IndirectDrawData()
        {
            IndirectDrawInfos    = null;
            IndirectDrawInfoBuff = null;
            CameraBuffers        = new Dictionary<int, CameraBufferInfo>();
        }

        ~IndirectDrawData()
        {
            ClearIndirectDrawInfos();
            ReleaseCameraBuffers();
            ReleaseBuffers();
        }

        public static IndirectDrawData GetInstance()
        {
            if(instance == null)
            {
                lock(_Synchronized)
                {
                    if(instance == null)
                    {
                        instance = new IndirectDrawData();
                    }
                }
            }

            return instance;
        }

        public void ReleaseBuffers()
        {
            if(IndirectDrawInfoBuff != null)
            {
                IndirectDrawInfoBuff.Release();
                IndirectDrawInfoBuff = null;
            }
        }

        public void GenerateBuffer(int newTotalCount, List<IndirectDrawInfostct> infoStctList)
        {
            Debug.Log($"ThreadID: {Thread.CurrentThread.ManagedThreadId}");
            Debug.Log($"Indirect Draw Data Count: {newTotalCount}.");
            var prevCount = this._totalCount;
            this._totalCount = Mathf.CeilToInt((float)newTotalCount / 64) * 64;

            if(prevCount != this._totalCount)
            {
                ReleaseBuffers();
                IndirectDrawInfoBuff = new ComputeBuffer(_totalCount, 16 + 16 + 64);
            }

            IndirectDrawInfoBuff.SetData(infoStctList);

            Debug.Log("Generate compute buffer successed");
        }

        /// <summary>
        /// 以IndirectDrawInfo List的形式传入绘制目标对象物体信息，并生成Compute Buffer
        /// </summary>
        /// <param name="infos"></param>
        public void SetIndirectDrawInfos(List<IndirectDrawInfo> infos)
        {
            if(infos != null)
            {
                this.IndirectDrawInfos = infos;

                if(IndirectDrawInfos.Count == 0)
                {
                    return;
                }

                int tempCount = 0;

                List<IndirectDrawInfostct> infoStctList = new List<IndirectDrawInfostct>();

                for(int i = 0; i < this.IndirectDrawInfos.Count; i++)
                {
                    var objs = IndirectDrawInfos[i].instances;
                    for(int j = 0; j < objs.Count; j++)
                    {
                        var trans = objs[j].transform;
                        var bound = CalculateBounds(objs[j]);

                        var pos = trans.position;

                        var infoStct = new IndirectDrawInfostct()
                        {
                            position     = new Vector4(pos.x,           pos.y,           pos.z,           i),
                            extents      = new Vector4(bound.extents.x, bound.extents.y, bound.extents.z, i),
                            localToWorld = trans.localToWorldMatrix
                        };

                        infoStctList.Add(infoStct);

                        tempCount++;
                    }
                }

                Debug.Log("infoStctList is ready");

                this.GenerateBuffer(tempCount, infoStctList);
            }
        }

        /// <summary>
        /// 清除IndirectDrawInfo List中的数据
        /// </summary>
        public void ClearIndirectDrawInfos()
        {
            if(this.IndirectDrawInfos != null)
            {
                this._totalCount = 0;
                IndirectDrawInfos.Clear();
                IndirectDrawInfos = null;
            }
        }

        /// <summary>
        /// 释放Camera Buffer字典中存储的Compute Buffer
        /// </summary>
        public void ReleaseCameraBuffers()
        {
            if(this.CameraBuffers == null)
            {
                return;
            }

            foreach(var key in this.CameraBuffers.Keys)
            {
                ReleaseCameraBufferInfo(key);
            }

            this.CameraBuffers.Clear();
        }

        public void ReleaseCameraBufferInfo(int camHash)
        {
            CameraBuffers[camHash].argsBuffer.Release();
            CameraBuffers[camHash].argsBuffer = null;
            CameraBuffers[camHash].cullResultBuffer.Release();
            CameraBuffers[camHash].cullResultBuffer = null;
        }

        //用于生成Compute Buffer的初始化数据List用的数据结构
        public struct IndirectDrawInfostct
        {
            public Vector4   position;     //4*4=16 bytes
            public Vector4   extents;      //4*4=16 bytes
            public Matrix4x4 localToWorld; //4*4*4=64 bytes
        }

        /// <summary>
        /// 计算物体的Bounds
        /// </summary>
        /// <param name="obj">目标实例的GameObject对象</param>
        /// <returns></returns>
        public Bounds CalculateBounds(GameObject obj)
        {
            Renderer[] rends = obj.GetComponentsInChildren<Renderer>();
            Bounds     b     = new Bounds();
            if(rends.Length > 0)
            {
                b = new Bounds(rends[0].bounds.center, rends[0].bounds.size);
                for(int r = 1; r < rends.Length; r++)
                {
                    b.Encapsulate(rends[r].bounds);
                }
            }

            b.center = obj.transform.position;

            return b;
        }
    }

    //用于存储不同相机的Indirect Drew绘制数据
    public class CameraBufferInfo
    {
        //实例数量
        public int instanceCount;

        //Mesh种类数量
        public int meshCount;

        //用于存储剔除计算结果的Compute Buffer
        public ComputeBuffer cullResultBuffer;

        //用于存储最终绘制使用的参数信息的Compute Buffer
        public ComputeBuffer argsBuffer;

        public CameraBufferInfo(int insCnt, int meshCnt, ref ComputeBuffer cullBuff, ref ComputeBuffer argsBuff)
        {
            this.instanceCount    = insCnt;
            this.meshCount        = meshCnt;
            this.cullResultBuffer = cullBuff;
            this.argsBuffer       = argsBuff;
        }
    }

    [Serializable]
    public class IndirectDrawInfo
    {
        public Mesh             mesh;
        public Material         material;
        public List<GameObject> instances;
        public int              count;

        public IndirectDrawInfo(Mesh mesh, Material material, List<GameObject> objs, int cnt)
        {
            this.mesh      = mesh;
            this.material  = material;
            this.instances = objs;
            this.count     = cnt;
        }
    }
}