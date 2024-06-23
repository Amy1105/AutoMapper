namespace AutoMapper.Internal;
[DebuggerDisplay("{RequestedTypes.SourceType.Name}, {RequestedTypes.DestinationType.Name} : {RuntimeTypes.SourceType.Name}, {RuntimeTypes.DestinationType.Name}")]
public readonly record struct MapRequest(TypePair RequestedTypes, TypePair RuntimeTypes, MemberMap MemberMap)
{
    public MapRequest(TypePair types) : this(types, types, MemberMap.Instance) { }
    public bool Equals(MapRequest other) => RequestedTypes.Equals(other.RequestedTypes) && RuntimeTypes.Equals(other.RuntimeTypes);
    public override int GetHashCode() => HashCode.Combine(RequestedTypes, RuntimeTypes);
}
[DebuggerDisplay("{SourceType.Name}, {DestinationType.Name}")]
public readonly record struct TypePair(Type SourceType, Type DestinationType)
{
    public bool IsConstructedGenericType => SourceType.IsConstructedGenericType || DestinationType.IsConstructedGenericType;

    /// <summary>
    /// 如果系统为true。Type对象本身是泛型类型参数或具有类型尚未提供特定类型的参数；否则为false。
    /// </summary>
    public bool ContainsGenericParameters => SourceType.ContainsGenericParameters || DestinationType.ContainsGenericParameters;
    public TypePair CloseGenericTypes(TypePair closedTypes)
    {
        var sourceArguments = closedTypes.SourceType.GenericTypeArguments;
        var destinationArguments = closedTypes.DestinationType.GenericTypeArguments;
        if(sourceArguments.Length == 0)
        {
            sourceArguments = destinationArguments;
        }
        else if(destinationArguments.Length == 0)
        {
            destinationArguments = sourceArguments;
        }
        var closedSourceType = SourceType.IsGenericTypeDefinition ? SourceType.MakeGenericType(sourceArguments) : SourceType;
        var closedDestinationType = DestinationType.IsGenericTypeDefinition ? DestinationType.MakeGenericType(destinationArguments) : DestinationType;
        return new(closedSourceType, closedDestinationType);
    }
    public TypePair Reverse() => new(DestinationType, SourceType);
    public Type ITypeConverter() => ContainsGenericParameters ? null : typeof(ITypeConverter<,>).MakeGenericType(SourceType, DestinationType);
    public TypePair GetTypeDefinitionIfGeneric() => new(GetTypeDefinitionIfGeneric(SourceType), GetTypeDefinitionIfGeneric(DestinationType));
    static Type GetTypeDefinitionIfGeneric(Type type) => type.IsGenericType ? type.GetGenericTypeDefinition() : type;
}