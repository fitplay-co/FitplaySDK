// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace FeatureConfigs
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct FeatureConfigList : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static FeatureConfigList GetRootAsFeatureConfigList(ByteBuffer _bb) { return GetRootAsFeatureConfigList(_bb, new FeatureConfigList()); }
  public static FeatureConfigList GetRootAsFeatureConfigList(ByteBuffer _bb, FeatureConfigList obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public FeatureConfigList __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public FeatureConfigs.FeatureConfig? Configs(int j) { int o = __p.__offset(4); return o != 0 ? (FeatureConfigs.FeatureConfig?)(new FeatureConfigs.FeatureConfig()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int ConfigsLength { get { int o = __p.__offset(4); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<FeatureConfigs.FeatureConfigList> CreateFeatureConfigList(FlatBufferBuilder builder,
      VectorOffset configsOffset = default(VectorOffset)) {
    builder.StartTable(1);
    FeatureConfigList.AddConfigs(builder, configsOffset);
    return FeatureConfigList.EndFeatureConfigList(builder);
  }

  public static void StartFeatureConfigList(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddConfigs(FlatBufferBuilder builder, VectorOffset configsOffset) { builder.AddOffset(0, configsOffset.Value, 0); }
  public static VectorOffset CreateConfigsVector(FlatBufferBuilder builder, Offset<FeatureConfigs.FeatureConfig>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateConfigsVectorBlock(FlatBufferBuilder builder, Offset<FeatureConfigs.FeatureConfig>[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartConfigsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<FeatureConfigs.FeatureConfigList> EndFeatureConfigList(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<FeatureConfigs.FeatureConfigList>(o);
  }
  public static void FinishFeatureConfigListBuffer(FlatBufferBuilder builder, Offset<FeatureConfigs.FeatureConfigList> offset) { builder.Finish(offset.Value); }
  public static void FinishSizePrefixedFeatureConfigListBuffer(FlatBufferBuilder builder, Offset<FeatureConfigs.FeatureConfigList> offset) { builder.FinishSizePrefixed(offset.Value); }
}


}
