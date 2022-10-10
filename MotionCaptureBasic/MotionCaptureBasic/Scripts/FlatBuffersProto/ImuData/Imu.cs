// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace ImuData
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct Imu : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static Imu GetRootAsImu(ByteBuffer _bb) { return GetRootAsImu(_bb, new Imu()); }
  public static Imu GetRootAsImu(ByteBuffer _bb, Imu obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public Imu __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public short DeviceId { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetShort(o + __p.bb_pos) : (short)0; } }
  public int Seq { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public long Timestamp { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetLong(o + __p.bb_pos) : (long)0; } }
  public ImuData.Vec3? Accelerometer { get { int o = __p.__offset(10); return o != 0 ? (ImuData.Vec3?)(new ImuData.Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public ImuData.Vec3? Gyroscope { get { int o = __p.__offset(12); return o != 0 ? (ImuData.Vec3?)(new ImuData.Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public ImuData.Vec3? Magnetometer { get { int o = __p.__offset(14); return o != 0 ? (ImuData.Vec3?)(new ImuData.Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public ImuData.Vec4? Quaternions { get { int o = __p.__offset(16); return o != 0 ? (ImuData.Vec4?)(new ImuData.Vec4()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartImu(FlatBufferBuilder builder) { builder.StartTable(7); }
  public static void AddDeviceId(FlatBufferBuilder builder, short deviceId) { builder.AddShort(0, deviceId, 0); }
  public static void AddSeq(FlatBufferBuilder builder, int seq) { builder.AddInt(1, seq, 0); }
  public static void AddTimestamp(FlatBufferBuilder builder, long timestamp) { builder.AddLong(2, timestamp, 0); }
  public static void AddAccelerometer(FlatBufferBuilder builder, Offset<ImuData.Vec3> accelerometerOffset) { builder.AddStruct(3, accelerometerOffset.Value, 0); }
  public static void AddGyroscope(FlatBufferBuilder builder, Offset<ImuData.Vec3> gyroscopeOffset) { builder.AddStruct(4, gyroscopeOffset.Value, 0); }
  public static void AddMagnetometer(FlatBufferBuilder builder, Offset<ImuData.Vec3> magnetometerOffset) { builder.AddStruct(5, magnetometerOffset.Value, 0); }
  public static void AddQuaternions(FlatBufferBuilder builder, Offset<ImuData.Vec4> quaternionsOffset) { builder.AddStruct(6, quaternionsOffset.Value, 0); }
  public static Offset<ImuData.Imu> EndImu(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<ImuData.Imu>(o);
  }
  public static void FinishImuBuffer(FlatBufferBuilder builder, Offset<ImuData.Imu> offset) { builder.Finish(offset.Value); }
  public static void FinishSizePrefixedImuBuffer(FlatBufferBuilder builder, Offset<ImuData.Imu> offset) { builder.FinishSizePrefixed(offset.Value); }
}


}