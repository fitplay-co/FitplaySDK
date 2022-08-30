using System.Collections.Generic;
using ILRuntime.Mono.Cecil.Cil;
using Unity.Collections;

namespace UnityEngine.Rendering.Universal.Internal
{
    public class IndirectDrawPass: ScriptableRenderPass
    {
        private ProfilingSampler _ProfilingSampler;

        private IndirectDrawData _indirectDrawData;

        private int ID_IndirectDrawInfos = Shader.PropertyToID("_IndirectDrawInfos");
        private int ID_ArgsBuffer        = Shader.PropertyToID("_ArgsBuffer");
        private int ID_ArgsOffset        = Shader.PropertyToID("_ArgsOffset");

        private AsyncGPUReadbackRequest m_debugGPUArgsRequest;

        internal IndirectDrawPass(URPProfileId profileId, RenderPassEvent evt)
        {
            _ProfilingSampler = ProfilingSampler.Get(profileId);
            renderPassEvent   = evt;
            _indirectDrawData = IndirectDrawData.GetInstance();
        }

        /// <summary>
        /// 执行Indirect Draw绘制
        /// </summary>
        /// <param name="context"></param>
        /// <param name="renderingData"></param>
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var indirectDrawInfos = _indirectDrawData.IndirectDrawInfos;

            var cameraHash = renderingData.cameraData.camera.GetHashCode();

            CameraBufferInfo cameraBuffInfo = null;

            if(!this._indirectDrawData.CameraBuffers.TryGetValue(cameraHash, out cameraBuffInfo))
            {
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get();
            using(new ProfilingScope(cmd, _ProfilingSampler))
            {
                var targetCam = renderingData.cameraData.camera;
                for(int i = 0; i < indirectDrawInfos.Count; i++)
                {
                    MaterialPropertyBlock materialBlock = new MaterialPropertyBlock();
                    materialBlock.SetBuffer(ID_IndirectDrawInfos, cameraBuffInfo.cullResultBuffer);

                    //由于每一个mesh的drawcall都是按每一个mesh的instance数来计算，而我们用的cullResultBuffer是所有mesh共用的，
                    //所以需要设置两个CBuffer，即_ArgsBuffer和_ArgsOffset来通过instance id计算正确的cullResultBuffer的index
                    //注：vulkan、metal API不需要进行偏移计算，Shader中会通过预编译信息控制是否进行Instance ID偏移计算
                    materialBlock.SetInt(ID_ArgsOffset, i * 5 + 4);
                    materialBlock.SetBuffer(ID_ArgsBuffer, cameraBuffInfo.argsBuffer);

                    cmd.DrawMeshInstancedIndirect(
                        indirectDrawInfos[i].mesh, 0, indirectDrawInfos[i].material, 0,
                        cameraBuffInfo.argsBuffer, i * 4 * 5, materialBlock
                    );
                }
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);

            //DebugInfo(cameraBuffInfo);
        }

        //debug获取Compute Buffer数据用
        private void DebugInfo(CameraBufferInfo info)
        {
            var argBuffer = info.argsBuffer;
            if(m_debugGPUArgsRequest.hasError)
            {
                m_debugGPUArgsRequest = AsyncGPUReadback.Request(argBuffer);
            }
            else if(m_debugGPUArgsRequest.done)
            {
                NativeArray<uint> argsList = m_debugGPUArgsRequest.GetData<uint>();
            }
        }
    }
}