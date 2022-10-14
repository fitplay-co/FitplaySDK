// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace OsOutput
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct OutputMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static OutputMessage GetRootAsOutputMessage(ByteBuffer _bb) { return GetRootAsOutputMessage(_bb, new OutputMessage()); }
  public static OutputMessage GetRootAsOutputMessage(ByteBuffer _bb, OutputMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public OutputMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Version { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetVersionBytes() { return __p.__vector_as_span<byte>(4, 1); }
#else
  public ArraySegment<byte>? GetVersionBytes() { return __p.__vector_as_arraysegment(4); }
#endif
  public byte[] GetVersionArray() { return __p.__vector_as_array<byte>(4); }
  public string Type { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetTypeBytes() { return __p.__vector_as_span<byte>(6, 1); }
#else
  public ArraySegment<byte>? GetTypeBytes() { return __p.__vector_as_arraysegment(6); }
#endif
  public byte[] GetTypeArray() { return __p.__vector_as_array<byte>(6); }
  public string SensorType { get { int o = __p.__offset(8); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetSensorTypeBytes() { return __p.__vector_as_span<byte>(8, 1); }
#else
  public ArraySegment<byte>? GetSensorTypeBytes() { return __p.__vector_as_arraysegment(8); }
#endif
  public byte[] GetSensorTypeArray() { return __p.__vector_as_array<byte>(8); }
  public PoseData.Pose? Pose { get { int o = __p.__offset(10); return o != 0 ? (PoseData.Pose?)(new PoseData.Pose()).__assign(__p.__indirect(o + __p.bb_pos), __p.bb) : null; } }
  public ActionData.Action? DetectionResult { get { int o = __p.__offset(12); return o != 0 ? (ActionData.Action?)(new ActionData.Action()).__assign(__p.__indirect(o + __p.bb_pos), __p.bb) : null; } }
  public OsOutput.TimeProfiling? TimeProfiling { get { int o = __p.__offset(14); return o != 0 ? (OsOutput.TimeProfiling?)(new OsOutput.TimeProfiling()).__assign(__p.__indirect(o + __p.bb_pos), __p.bb) : null; } }

  public static Offset<OsOutput.OutputMessage> CreateOutputMessage(FlatBufferBuilder builder,
      StringOffset versionOffset = default(StringOffset),
      StringOffset typeOffset = default(StringOffset),
      StringOffset sensorTypeOffset = default(StringOffset),
      Offset<PoseData.Pose> poseOffset = default(Offset<PoseData.Pose>),
      Offset<ActionData.Action> detectionResultOffset = default(Offset<ActionData.Action>),
      Offset<OsOutput.TimeProfiling> timeProfilingOffset = default(Offset<OsOutput.TimeProfiling>)) {
    builder.StartTable(6);
    OutputMessage.AddTimeProfiling(builder, timeProfilingOffset);
    OutputMessage.AddDetectionResult(builder, detectionResultOffset);
    OutputMessage.AddPose(builder, poseOffset);
    OutputMessage.AddSensorType(builder, sensorTypeOffset);
    OutputMessage.AddType(builder, typeOffset);
    OutputMessage.AddVersion(builder, versionOffset);
    return OutputMessage.EndOutputMessage(builder);
  }

  public static void StartOutputMessage(FlatBufferBuilder builder) { builder.StartTable(6); }
  public static void AddVersion(FlatBufferBuilder builder, StringOffset versionOffset) { builder.AddOffset(0, versionOffset.Value, 0); }
  public static void AddType(FlatBufferBuilder builder, StringOffset typeOffset) { builder.AddOffset(1, typeOffset.Value, 0); }
  public static void AddSensorType(FlatBufferBuilder builder, StringOffset sensorTypeOffset) { builder.AddOffset(2, sensorTypeOffset.Value, 0); }
  public static void AddPose(FlatBufferBuilder builder, Offset<PoseData.Pose> poseOffset) { builder.AddOffset(3, poseOffset.Value, 0); }
  public static void AddDetectionResult(FlatBufferBuilder builder, Offset<ActionData.Action> detectionResultOffset) { builder.AddOffset(4, detectionResultOffset.Value, 0); }
  public static void AddTimeProfiling(FlatBufferBuilder builder, Offset<OsOutput.TimeProfiling> timeProfilingOffset) { builder.AddOffset(5, timeProfilingOffset.Value, 0); }
  public static Offset<OsOutput.OutputMessage> EndOutputMessage(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<OsOutput.OutputMessage>(o);
  }
  public static void FinishOutputMessageBuffer(FlatBufferBuilder builder, Offset<OsOutput.OutputMessage> offset) { builder.Finish(offset.Value); }
  public static void FinishSizePrefixedOutputMessageBuffer(FlatBufferBuilder builder, Offset<OsOutput.OutputMessage> offset) { builder.FinishSizePrefixed(offset.Value); }
}


}
