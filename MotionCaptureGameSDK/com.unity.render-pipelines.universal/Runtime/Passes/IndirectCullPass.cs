using UnityEngine.UI;

namespace UnityEngine.Rendering.Universal.Internal
{
    public class IndirectCullPass: ScriptableRenderPass
    {
        private ProfilingSampler _ProfilingSampler;
        private ComputeShader    cullingCS;
        private Shader           blitShader;
        private int              cullingCSkernel;
        private int              offsetCSkernel;
        private Vector2          m_textureSize;
        private RenderTexture    m_HiZDepthTexture;
        private Material         hzbMat;

        //IndirectDrawData单例引用
        private IndirectDrawData _indirectDrawData;

        //Compute Shader是否设置初始值判断标识符
        private bool _isShaderInitialized = false;

        //需要给Compute Shader和Blit Shader设置的各种变量的ID
        private int ID_InvSize            = Shader.PropertyToID("_InvSize");
        private int ID_ReduceScale        = Shader.PropertyToID("_ReduceScale");
        private int ID_instanceBuffer     = Shader.PropertyToID("instanceBuffer");
        private int ID_visibleList        = Shader.PropertyToID("visibleList");
        private int ID_argsBuffer         = Shader.PropertyToID("argsBuffer");
        private int ID_HiZDepthTexture    = Shader.PropertyToID("_HiZDepthTexture");
        private int ID_size               = Shader.PropertyToID("size");
        private int ID_matrix_VP          = Shader.PropertyToID("matrix_VP");
        private int ID_CameraDepthTexture = Shader.PropertyToID("_CameraDepthTexture");
        private int ID_minMipmapLevel     = Shader.PropertyToID("minMipmapLevel");

        //用于深度图计算的RenderTexture最大尺寸
        private const int MAXSIZEOFTEXTURE = 1024;

        //定义用于HiZ剔除的深度图RenderTexture mipmap从哪一级开始生成(第0级为)
        private const int MINMIPMAPLEVEL = 4;

        internal IndirectCullPass(URPProfileId profileId, RenderPassEvent evt, ComputeShader cShader, Material hizMat)
        {
            _ProfilingSampler = ProfilingSampler.Get(profileId);
            renderPassEvent   = evt;
            this.cullingCS    = cShader;

            _indirectDrawData = IndirectDrawData.GetInstance();

            this.hzbMat = hizMat;

            this.cullingCSkernel = this.cullingCS.FindKernel("CSMain");
        }

        /// <summary>
        /// 在ForwardRenderer的Setup方法中调用。用于设置各类参数初始值
        /// </summary>
        /// <param name="renderingData">用于获取初始化配置中需要传入的参数</param>
        public void Setup(ref RenderingData renderingData)
        {
//Editor中用于控制是否更新裁切结果
#if UNITY_EDITOR
            if(this._indirectDrawData.IsStopCulling)
            {
                return;
            }
#endif

//初始化深度图；设置各种Compute Shader中需要用到的变量
#region SetShaderProperties

            var mainCam = renderingData.cameraData.camera;
            int size    = (int)Mathf.Max(mainCam.pixelWidth, mainCam.pixelHeight);
            size            = (int)Mathf.Min(Mathf.NextPowerOfTwo(size), MAXSIZEOFTEXTURE);
            m_textureSize.x = size;
            m_textureSize.y = size;

            if(m_HiZDepthTexture == null || (m_HiZDepthTexture.width != size || m_HiZDepthTexture.height != size))
            {
                InitializeTexture(mainCam);
            }

            this.cullingCS.SetTexture(this.cullingCSkernel, ID_HiZDepthTexture, this.m_HiZDepthTexture);

            this.cullingCS.SetInt(this.ID_size, size);

            //var m_VP = GL.GetGPUProjectionMatrix(mainCam.projectionMatrix, false) * mainCam.worldToCameraMatrix;
            var m_VP = mainCam.projectionMatrix * mainCam.worldToCameraMatrix;
            this.cullingCS.SetMatrix(this.ID_matrix_VP, m_VP);
            
            this.cullingCS.SetInt(this.ID_minMipmapLevel, MINMIPMAPLEVEL);

#endregion
            
//初始化Compute Shader计算需要用到的Compute Buffer
#region Initialization

            InitializeBuffer(mainCam);

            if(!_isShaderInitialized)
            {
                _isShaderInitialized = InitializeShader(mainCam);
            }

#endregion
        }

        /// <summary>
        /// 生成深度图mipmap；执行Compute Shader进行裁切和剔除
        /// </summary>
        /// <param name="context"></param>
        /// <param name="renderingData"></param>
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
//Editor中用于控制是否更新裁切结果
#if UNITY_EDITOR
            if(this._indirectDrawData.IsStopCulling)
            {
                return;
            }
#endif
            if(!_isShaderInitialized || m_HiZDepthTexture == null)
            {
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get();
            using(new ProfilingScope(cmd, _ProfilingSampler))
            {
//生成10级深度图的mipmap
#region BlitDepthShader

                int w = (int)m_textureSize.x;
                int h = (int)m_textureSize.y;
                
                int level  = 0;
                int reduce = 0;

                RenderTexture lastRt = null;
                RenderTexture tempRT;
                
                hzbMat.SetVector(ID_InvSize, new Vector4(1.0f / w, 1.0f / h, 0, 0));
                tempRT            = RenderTexture.GetTemporary(w, h, 0, m_HiZDepthTexture.format);
                tempRT.filterMode = FilterMode.Point;
                Graphics.Blit(Shader.GetGlobalTexture(ID_CameraDepthTexture), tempRT);
                Graphics.CopyTexture(tempRT, 0, 0, m_HiZDepthTexture, 0, level);
                lastRt =  tempRT;
                
                //从MINMIPMAPLEVEL级的mipmap开始生成，也就是需要按原始尺寸除以2的MINMIPMAPLEVEL次方
                w >>= MINMIPMAPLEVEL;
                h >>= MINMIPMAPLEVEL;
                
                level  += MINMIPMAPLEVEL;
                reduce =  1 << MINMIPMAPLEVEL;
                
                while(h > 0)
                {
                    hzbMat.SetVector(ID_InvSize, new Vector4(1.0f / w, 1.0f / h, 0, 0));
                    hzbMat.SetInt(this.ID_ReduceScale, reduce);

                    tempRT            = RenderTexture.GetTemporary(w, h, 0, m_HiZDepthTexture.format);
                    tempRT.filterMode = FilterMode.Point;

                    Graphics.Blit(lastRt, tempRT, hzbMat);
                    RenderTexture.ReleaseTemporary(lastRt);

//#if UNITY_EDITOR
//                    if(level == 4)
//                    {
//                        var canvasImage = GameObject.Find("MapImage");
//                        if(canvasImage != null)
//                        {
//                            var rawImage = canvasImage.GetComponent(typeof(RawImage)) as RawImage;
//                            rawImage.texture = tempRT;
//                        }
//                    }
//#endif

                    Graphics.CopyTexture(tempRT, 0, 0, m_HiZDepthTexture, 0, level);
                    lastRt = tempRT;

                    w >>= 1;
                    h >>= 1;
                    level++;
                    reduce = 2;
                }

                RenderTexture.ReleaseTemporary(lastRt);

#endregion

//用于调试深度图的代码
//#if UNITY_EDITOR
//               var canvasImage = GameObject.Find("MapImage");
//               if(canvasImage != null)
//               {
//                   var rawImage = canvasImage.GetComponent(typeof(RawImage)) as RawImage;
//                   rawImage.texture = m_HiZDepthTexture;
//               }
//#endif

//执行Compute Shader，进行视锥剔除和遮挡剔除处理
#region DispatchComputeShader

                int jobs = Mathf.CeilToInt(this._indirectDrawData.InstanceTotalCount / 64.0f);

                cmd.DispatchCompute(this.cullingCS, this.cullingCSkernel, jobs, 1, 1);

#endregion
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        /// <summary>
        /// 初始化Compute Buffer
        /// 如果根据Camera的HashCode从字典能取到Compute Buffer，就不用重新生成新的Buffer
        /// </summary>
        /// <param name="targetCamera">渲染目标相机</param>
        private void InitializeBuffer(Camera targetCamera)
        {
            CameraBufferInfo camBuffInfo   = null;
            var              cameraHash    = targetCamera.GetHashCode();
            var              cameraBuffers = this._indirectDrawData.CameraBuffers;
            var              insCnt        = this._indirectDrawData.InstanceTotalCount;
            var              meshCnt       = this._indirectDrawData.IndirectDrawInfos.Count;

            //没有时直接创建；有就直接取，但如果发生instance数量或者mesh种类数量变化时就要release了重新生成
            if(cameraBuffers.ContainsKey(cameraHash))
            {
                camBuffInfo = cameraBuffers[cameraHash];
                if(camBuffInfo.instanceCount != insCnt)
                {
                    cameraBuffers[cameraHash].cullResultBuffer.Release();
                    ComputeBuffer cullResultBuff = new ComputeBuffer(insCnt, 64);
                    cameraBuffers[cameraHash].cullResultBuffer = cullResultBuff;
                    cameraBuffers[cameraHash].instanceCount    = insCnt;
                    _isShaderInitialized                       = false;
                }

                if(camBuffInfo.meshCount != meshCnt)
                {
                    cameraBuffers[cameraHash].argsBuffer.Release();
                    ComputeBuffer argsBuff = new ComputeBuffer(meshCnt * 5, 4, ComputeBufferType.IndirectArguments);
                    cameraBuffers[cameraHash].argsBuffer = argsBuff;
                    cameraBuffers[cameraHash].meshCount  = meshCnt;
                    _isShaderInitialized                 = false;
                }
            }
            else
            {
                ComputeBuffer cullResultBuff = new ComputeBuffer(insCnt,      64);
                ComputeBuffer argsBuff       = new ComputeBuffer(meshCnt * 5, 4, ComputeBufferType.IndirectArguments);
                camBuffInfo               = new CameraBufferInfo(insCnt, meshCnt, ref cullResultBuff, ref argsBuff);
                cameraBuffers[cameraHash] = camBuffInfo;
                _isShaderInitialized      = false;
            }

            //重设argsBuffer的初始值
            uint[] argsList          = new uint[meshCnt * 5];
            var    indirectDrawInfos = this._indirectDrawData.IndirectDrawInfos;

            for(int i = 0; i < meshCnt; i++)
            {
                argsList[i * 5 + 0] = indirectDrawInfos[i].mesh.GetIndexCount(0); // 0 - index count per instance,
                argsList[i * 5 + 1] = 0;                                          // 1 - instance count
                argsList[i * 5 + 2] = 0;                                          // 2 - start index location
                argsList[i * 5 + 3] = 0;                                          // 3 - base vertex location
                if(i > 0)
                {
                    //当前种类Mesh的instance偏移等于上一种mesh的偏移加instance总数
                    argsList[i * 5 + 4] = (uint)indirectDrawInfos[i - 1].count + argsList[(i - 1) * 5 + 4]; // 4 - start instance location
                }
                else
                {
                    argsList[i * 5 + 4] = 0; // 4 - start instance location
                }
                
                cameraBuffers[cameraHash].argsBuffer.SetData(argsList);
            }
        }

        /// <summary>
        /// 设置Compute Shader计算需要用到的Compute Buffer
        /// </summary>
        /// <param name="targetCam"></param>
        /// <returns></returns>
        private bool InitializeShader(Camera targetCam)
        {
            ComputeBuffer originalBuffer = this._indirectDrawData.IndirectDrawInfoBuff;
            ComputeBuffer resultBuffer   = null;
            ComputeBuffer argsBuffer     = null;

            var camHash = targetCam.GetHashCode();

            if(this._indirectDrawData.CameraBuffers.ContainsKey(camHash))
            {
                resultBuffer = this._indirectDrawData.CameraBuffers[camHash].cullResultBuffer;
                argsBuffer   = this._indirectDrawData.CameraBuffers[camHash].argsBuffer;
            }

            if(originalBuffer == null || resultBuffer == null || argsBuffer == null)
            {
                return false;
            }

            this.cullingCS.SetBuffer(this.cullingCSkernel, this.ID_instanceBuffer, originalBuffer);
            this.cullingCS.SetBuffer(this.cullingCSkernel, this.ID_visibleList,    resultBuffer);
            this.cullingCS.SetBuffer(this.cullingCSkernel, this.ID_argsBuffer,     argsBuffer);

            return true;
        }

        /// <summary>
        /// 初始化用于存储深度图Template的RenderTexture
        /// </summary>
        /// <param name="mainCam"></param>
        private void InitializeTexture(Camera mainCam)
        {
            m_HiZDepthTexture = new RenderTexture(
                (int)m_textureSize.x, (int)m_textureSize.y,
                0, RenderTextureFormat.RGHalf, RenderTextureReadWrite.Linear
            );
            m_HiZDepthTexture.filterMode       = FilterMode.Point;
            m_HiZDepthTexture.useMipMap        = true;
            m_HiZDepthTexture.autoGenerateMips = false;
            m_HiZDepthTexture.Create();

            mainCam.depthTextureMode |= DepthTextureMode.Depth;
        }
    }
}