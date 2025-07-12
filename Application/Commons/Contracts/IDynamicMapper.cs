namespace Application.Commons.Contracts;

public interface IDynamicMapper
{
    object? Map(object source, Type sourceType, Type destinationType);
    TDestination Map<TDestination>(object? source);
    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
    List<TDestination> MapList<TSource, TDestination>(List<TSource> sourceList) where TDestination : class;
}
