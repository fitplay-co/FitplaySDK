// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace ApplicationControl
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct Control : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static Control GetRootAsControl(ByteBuffer _bb) { return GetRootAsControl(_bb, new Control()); }
  public static Control GetRootAsControl(ByteBuffer _bb, Control obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public Control __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string FeatureId { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetFeatureIdBytes() { return __p.__vector_as_span<byte>(4, 1); }
#else
  public ArraySegment<byte>? GetFeatureIdBytes() { return __p.__vector_as_arraysegment(4); }
#endif
  public byte[] GetFeatureIdArray() { return __p.__vector_as_array<byte>(4); }
  public string Action { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetActionBytes() { return __p.__vector_as_span<byte>(6, 1); }
#else
  public ArraySegment<byte>? GetActionBytes() { return __p.__vector_as_arraysegment(6); }
#endif
  public byte[] GetActionArray() { return __p.__vector_as_array<byte>(6); }
  public ApplicationControl.ControlData? Data { get { int o = __p.__offset(8); return o != 0 ? (ApplicationControl.ControlData?)(new ApplicationControl.ControlData()).__assign(__p.__indirect(o + __p.bb_pos), __p.bb) : null; } }

  public static Offset<ApplicationControl.Control> CreateControl(FlatBufferBuilder builder,
      StringOffset featureIdOffset = default(StringOffset),
      StringOffset actionOffset = default(StringOffset),
      Offset<ApplicationControl.ControlData> dataOffset = default(Offset<ApplicationControl.ControlData>)) {
    builder.StartTable(3);
    Control.AddData(builder, dataOffset);
    Control.AddAction(builder, actionOffset);
    Control.AddFeatureId(builder, featureIdOffset);
    return Control.EndControl(builder);
  }

  public static void StartControl(FlatBufferBuilder builder) { builder.StartTable(3); }
  public static void AddFeatureId(FlatBufferBuilder builder, StringOffset featureIdOffset) { builder.AddOffset(0, featureIdOffset.Value, 0); }
  public static void AddAction(FlatBufferBuilder builder, StringOffset actionOffset) { builder.AddOffset(1, actionOffset.Value, 0); }
  public static void AddData(FlatBufferBuilder builder, Offset<ApplicationControl.ControlData> dataOffset) { builder.AddOffset(2, dataOffset.Value, 0); }
  public static Offset<ApplicationControl.Control> EndControl(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<ApplicationControl.Control>(o);
  }
  public static void FinishControlBuffer(FlatBufferBuilder builder, Offset<ApplicationControl.Control> offset) { builder.Finish(offset.Value); }
  public static void FinishSizePrefixedControlBuffer(FlatBufferBuilder builder, Offset<ApplicationControl.Control> offset) { builder.FinishSizePrefixed(offset.Value); }
}


}
