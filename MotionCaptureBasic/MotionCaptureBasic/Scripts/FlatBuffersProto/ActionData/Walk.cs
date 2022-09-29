// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace ActionData
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct Walk : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static Walk GetRootAsWalk(ByteBuffer _bb) { return GetRootAsWalk(_bb, new Walk()); }
  public static Walk GetRootAsWalk(ByteBuffer _bb, Walk obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public Walk __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int LeftLeg { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int RightLeg { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public float LeftFrequency { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float RightFrequency { get { int o = __p.__offset(10); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float LeftHipAng { get { int o = __p.__offset(12); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float RightHipAng { get { int o = __p.__offset(14); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float LeftStepLength { get { int o = __p.__offset(16); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float RightStepLength { get { int o = __p.__offset(18); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float LeftProgress { get { int o = __p.__offset(20); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float RightProgress { get { int o = __p.__offset(22); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float Turn { get { int o = __p.__offset(24); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float StepRate { get { int o = __p.__offset(26); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float StepLen { get { int o = __p.__offset(28); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float Velocity { get { int o = __p.__offset(30); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float VelocityThreshold { get { int o = __p.__offset(32); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float RealtimeLeftLeg { get { int o = __p.__offset(34); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float RealtimeRightLeg { get { int o = __p.__offset(36); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }

  public static Offset<ActionData.Walk> CreateWalk(FlatBufferBuilder builder,
      int leftLeg = 0,
      int rightLeg = 0,
      float leftFrequency = 0.0f,
      float rightFrequency = 0.0f,
      float leftHipAng = 0.0f,
      float rightHipAng = 0.0f,
      float leftStepLength = 0.0f,
      float rightStepLength = 0.0f,
      float leftProgress = 0.0f,
      float rightProgress = 0.0f,
      float turn = 0.0f,
      float stepRate = 0.0f,
      float stepLen = 0.0f,
      float velocity = 0.0f,
      float velocityThreshold = 0.0f,
      float realtimeLeftLeg = 0.0f,
      float realtimeRightLeg = 0.0f) {
    builder.StartTable(17);
    Walk.AddRealtimeRightLeg(builder, realtimeRightLeg);
    Walk.AddRealtimeLeftLeg(builder, realtimeLeftLeg);
    Walk.AddVelocityThreshold(builder, velocityThreshold);
    Walk.AddVelocity(builder, velocity);
    Walk.AddStepLen(builder, stepLen);
    Walk.AddStepRate(builder, stepRate);
    Walk.AddTurn(builder, turn);
    Walk.AddRightProgress(builder, rightProgress);
    Walk.AddLeftProgress(builder, leftProgress);
    Walk.AddRightStepLength(builder, rightStepLength);
    Walk.AddLeftStepLength(builder, leftStepLength);
    Walk.AddRightHipAng(builder, rightHipAng);
    Walk.AddLeftHipAng(builder, leftHipAng);
    Walk.AddRightFrequency(builder, rightFrequency);
    Walk.AddLeftFrequency(builder, leftFrequency);
    Walk.AddRightLeg(builder, rightLeg);
    Walk.AddLeftLeg(builder, leftLeg);
    return Walk.EndWalk(builder);
  }

  public static void StartWalk(FlatBufferBuilder builder) { builder.StartTable(17); }
  public static void AddLeftLeg(FlatBufferBuilder builder, int leftLeg) { builder.AddInt(0, leftLeg, 0); }
  public static void AddRightLeg(FlatBufferBuilder builder, int rightLeg) { builder.AddInt(1, rightLeg, 0); }
  public static void AddLeftFrequency(FlatBufferBuilder builder, float leftFrequency) { builder.AddFloat(2, leftFrequency, 0.0f); }
  public static void AddRightFrequency(FlatBufferBuilder builder, float rightFrequency) { builder.AddFloat(3, rightFrequency, 0.0f); }
  public static void AddLeftHipAng(FlatBufferBuilder builder, float leftHipAng) { builder.AddFloat(4, leftHipAng, 0.0f); }
  public static void AddRightHipAng(FlatBufferBuilder builder, float rightHipAng) { builder.AddFloat(5, rightHipAng, 0.0f); }
  public static void AddLeftStepLength(FlatBufferBuilder builder, float leftStepLength) { builder.AddFloat(6, leftStepLength, 0.0f); }
  public static void AddRightStepLength(FlatBufferBuilder builder, float rightStepLength) { builder.AddFloat(7, rightStepLength, 0.0f); }
  public static void AddLeftProgress(FlatBufferBuilder builder, float leftProgress) { builder.AddFloat(8, leftProgress, 0.0f); }
  public static void AddRightProgress(FlatBufferBuilder builder, float rightProgress) { builder.AddFloat(9, rightProgress, 0.0f); }
  public static void AddTurn(FlatBufferBuilder builder, float turn) { builder.AddFloat(10, turn, 0.0f); }
  public static void AddStepRate(FlatBufferBuilder builder, float stepRate) { builder.AddFloat(11, stepRate, 0.0f); }
  public static void AddStepLen(FlatBufferBuilder builder, float stepLen) { builder.AddFloat(12, stepLen, 0.0f); }
  public static void AddVelocity(FlatBufferBuilder builder, float velocity) { builder.AddFloat(13, velocity, 0.0f); }
  public static void AddVelocityThreshold(FlatBufferBuilder builder, float velocityThreshold) { builder.AddFloat(14, velocityThreshold, 0.0f); }
  public static void AddRealtimeLeftLeg(FlatBufferBuilder builder, float realtimeLeftLeg) { builder.AddFloat(15, realtimeLeftLeg, 0.0f); }
  public static void AddRealtimeRightLeg(FlatBufferBuilder builder, float realtimeRightLeg) { builder.AddFloat(16, realtimeRightLeg, 0.0f); }
  public static Offset<ActionData.Walk> EndWalk(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<ActionData.Walk>(o);
  }
}


}
