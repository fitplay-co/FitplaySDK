// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace ActionData
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct Gaze : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static Gaze GetRootAsGaze(ByteBuffer _bb) { return GetRootAsGaze(_bb, new Gaze()); }
  public static Gaze GetRootAsGaze(ByteBuffer _bb, Gaze obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public Gaze __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public float X { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float Y { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float Z { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }

  public static Offset<ActionData.Gaze> CreateGaze(FlatBufferBuilder builder,
      float x = 0.0f,
      float y = 0.0f,
      float z = 0.0f) {
    builder.StartTable(3);
    Gaze.AddZ(builder, z);
    Gaze.AddY(builder, y);
    Gaze.AddX(builder, x);
    return Gaze.EndGaze(builder);
  }

  public static void StartGaze(FlatBufferBuilder builder) { builder.StartTable(3); }
  public static void AddX(FlatBufferBuilder builder, float x) { builder.AddFloat(0, x, 0.0f); }
  public static void AddY(FlatBufferBuilder builder, float y) { builder.AddFloat(1, y, 0.0f); }
  public static void AddZ(FlatBufferBuilder builder, float z) { builder.AddFloat(2, z, 0.0f); }
  public static Offset<ActionData.Gaze> EndGaze(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<ActionData.Gaze>(o);
  }
}


}
