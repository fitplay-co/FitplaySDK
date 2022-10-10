// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace ActionData
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct Jump : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static Jump GetRootAsJump(ByteBuffer _bb) { return GetRootAsJump(_bb, new Jump()); }
  public static Jump GetRootAsJump(ByteBuffer _bb, Jump obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public Jump __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int OnTheGround { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public float Velocity { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }

  public static Offset<ActionData.Jump> CreateJump(FlatBufferBuilder builder,
      int onTheGround = 0,
      float velocity = 0.0f) {
    builder.StartTable(2);
    Jump.AddVelocity(builder, velocity);
    Jump.AddOnTheGround(builder, onTheGround);
    return Jump.EndJump(builder);
  }

  public static void StartJump(FlatBufferBuilder builder) { builder.StartTable(2); }
  public static void AddOnTheGround(FlatBufferBuilder builder, int onTheGround) { builder.AddInt(0, onTheGround, 0); }
  public static void AddVelocity(FlatBufferBuilder builder, float velocity) { builder.AddFloat(1, velocity, 0.0f); }
  public static Offset<ActionData.Jump> EndJump(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<ActionData.Jump>(o);
  }
}


}