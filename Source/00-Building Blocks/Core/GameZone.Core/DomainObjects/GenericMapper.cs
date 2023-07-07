using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace GameZone.Core.DomainObjects
{
    public static class GenericMapper
    {
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        /// <summary>
        /// Static cache of TypeMappers. Each TypeMapper knows how to map from one single source Type
        /// to another, structurally equivalent destination Type.
        /// </summary>
        private static readonly IDictionary<TypeMapperKey, TypeMapper> _typeMappers
            = new Dictionary<TypeMapperKey, TypeMapper>();

        /// <summary>
        /// Map source to a new instance of TResult
        /// </summary>
        /// <typeparam name="TResult">The target type</typeparam>
        /// <param name="source">The source instance</param>
        /// <returns>A new TResult</returns>
        public static TResult MapTo<TResult>(object source)
            where TResult : class
        {
            Contract.ArgumentNotNull(source, "source");

            return (TResult)MapTo(source, typeof(TResult));
        }

        /// <summary>
        /// Map source to a new instance of resultType
        /// </summary>
        /// <param name="source">The source instance</param>
        /// <param name="resultType">The result type</param>
        /// <returns>A new instance of resultType</returns>
        public static object MapTo(object source, Type resultType)
        {
            Contract.ArgumentNotNull(source, "source");
            Contract.ArgumentNotNull(resultType, "resultType");

            IEnumerable enumerable = source as IEnumerable;

            if (enumerable != null)
            {
                IList results = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(resultType));

                foreach (object o in enumerable)
                {
                    results.Add(MapTo(o, resultType));
                }

                return results;
            }

            return MapInternal(source, null, resultType, new Dictionary<object, object>(), false);
        }

        /// <summary>
        /// Maps source "over" result.
        /// </summary>
        /// <param name="source">The source instance</param>
        /// <param name="result">The target instance</param>
        public static void MapTo(object source, object result)
        {
            MapTo(source, result, false);
        }

        /// <summary>
        /// Maps source "over" result, optionally performing a shallow mapping - 
        /// i.e. only scalar properties are mapped.
        /// </summary>
        /// <param name="source">The source instance</param>
        /// <param name="result">The target instance</param>
        /// <param name="shallow">True, to perform a shallow mapping</param>
        public static void MapTo(object source, object result, bool shallow)
        {
            Contract.ArgumentNotNull(source, "source");
            Contract.ArgumentNotNull(result, "result");

            MapInternal(source, result, result.GetType(), new Dictionary<object, object>(), shallow);
        }

        /// <summary>
        /// Perform the mapping. Main purpose is to encapsulate interaction with the TypeMapper cache.
        /// </summary>
        private static object MapInternal(object source, object result,
            Type resultType, IDictionary<object, object> cache, bool shallow)
        {
            if (source == null)
            {
                return null;
            }

            Type sourceType = source.GetType();
            TypeMapperKey typeMapperKey = new TypeMapperKey(sourceType, resultType);

            TypeMapper typeMapper;

            _lock.EnterUpgradeableReadLock();

            try
            {
                if (!_typeMappers.TryGetValue(typeMapperKey, out typeMapper))
                {
                    _lock.EnterWriteLock();

                    try
                    {
                        typeMapper = new TypeMapper(sourceType, resultType);

                        _typeMappers.Add(typeMapperKey, typeMapper);
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }

            return typeMapper.Map(source, result, cache, shallow);
        }

        #region TypeMapperKey

        /// <summary>
        /// Key by which TypeMappers are cached. Simply the combination of source and
        /// destination Type.
        /// </summary>
        private sealed class TypeMapperKey
        {
            private readonly Type _resultType;
            private readonly Type _sourceType;

            public TypeMapperKey(Type sourceType, Type resultType)
            {
                _sourceType = sourceType;
                _resultType = resultType;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                TypeMapperKey typeMapper = (TypeMapperKey)obj;

                return Equals(typeMapper._sourceType, _sourceType)
                       && Equals(typeMapper._resultType, _resultType);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((_sourceType != null ? _sourceType.GetHashCode() : 0) * 397) ^
                           (_resultType != null ? _resultType.GetHashCode() : 0);
                }
            }
        }

        #endregion

        #region TypeMapper

        /// <summary>
        /// Class that can map from a source Type to a result Type.
        /// 
        /// Works by dynamically building a set of mapping expressions
        /// that are subsequently used to perform the mapping.
        /// </summary>
        private sealed class TypeMapper
        {
            private const string IdentityPropertyName = "Id";

            private readonly List<NonScalarMapping> _collectionMappings
                = new List<NonScalarMapping>();

            private readonly Delegate _objectActivator;

            private readonly List<ReferenceMapping> _referenceMappings
                = new List<ReferenceMapping>();

            private readonly List<Delegate> _scalarMappings
                = new List<Delegate>();

            public TypeMapper(Type sourceType, Type resultType)
            {
                // this is just a faster way of instantiating instances.
                _objectActivator = Expression.Lambda(Expression.New(resultType)).Compile();

                // select all the properties that have the same name between source and destination
                List<PropertyPair> propertyPairs = new List<PropertyPair>(
                    from s in sourceType.GetProperties()
                    join r in resultType.GetProperties() on s.Name equals r.Name
                    where s.CanRead && r.CanRead
                    select new PropertyPair { SourceProperty = s, ResultProperty = r });

                CreateScalarMappings(sourceType, resultType, propertyPairs);
                //CreateCollectionMappings(sourceType, resultType, propertyPairs);
                CreateReferenceMappings(sourceType, resultType, propertyPairs);
            }

            /// <summary>
            /// Handle recursively mapping from source to result. The cache variable is used as an
            /// Identity Map to handle graph cycles.
            /// </summary>
            public object Map(object source, object result, IDictionary<object, object> cache, bool shallow)
            {
                object cachedResult;

                if (cache.TryGetValue(source, out cachedResult))
                {
                    return cachedResult;
                }

                if (result == null)
                {
                    result = _objectActivator.DynamicInvoke();
                }

                cache.Add(source, result);

                MapScalars(source, result);

                if (!shallow)
                {
                    MapReferences(source, result, cache);
                    MapCollections(source, result, cache);
                }

                return result;
            }

            private void MapScalars(object source, object result)
            {
                foreach (Delegate scalarMapping in _scalarMappings)
                {
                    scalarMapping.DynamicInvoke(source, result);
                }
            }

            private void MapReferences(object source, object result, IDictionary<object, object> cache)
            {
                foreach (ReferenceMapping referenceMapping in _referenceMappings)
                {
                    object sourceItem = referenceMapping.Source.DynamicInvoke(source);
                    object resultItem = referenceMapping.Result.DynamicInvoke(result);

                    if (sourceItem == null)
                    {
                        referenceMapping.Setter.DynamicInvoke(result, null);
                    }
                    else if ((resultItem != null)
                             && (bool)referenceMapping.IdentityComparer.DynamicInvoke(sourceItem, resultItem))
                    {
                        MapInternal(sourceItem, resultItem, referenceMapping.ResultItemType, cache, true);
                    }
                    else
                    {
                        referenceMapping.Setter.DynamicInvoke(result,
                            MapInternal(sourceItem, null, referenceMapping.ResultItemType, cache, true));
                    }
                }
            }

            private void MapCollections(object source, object result, IDictionary<object, object> cache)
            {
                // Collection mapping is the most complicated.
                //   - Items in source that are not is result must be created.
                //   - Items in both source and result must be mapped.
                //   - Items in result but not in source must be removed.

                foreach (NonScalarMapping mapping in _collectionMappings)
                {
                    IEnumerable sourceCollection = (IEnumerable)mapping.Source.DynamicInvoke(source);
                    if (sourceCollection != null)
                    {
                        object maybeList = mapping.Result.DynamicInvoke(result);
                        if (maybeList != null)
                        {
                            IList resultList = maybeList as IList ?? ((IListSource)maybeList).GetList();

                            RemoveOldCollectionItems(mapping, sourceCollection, resultList);
                            AddOrUpdateNewCollectionItems(mapping, sourceCollection, resultList, cache);
                        }
                    }
                }
            }

            private static void RemoveOldCollectionItems(NonScalarMapping mapping,
                IEnumerable sourceCollection, IList resultList)
            {
                for (int i = resultList.Count - 1; i >= 0; i--)
                {
                    bool found = false;

                    foreach (object sourceItem in sourceCollection)
                    {
                        if ((bool)mapping.IdentityComparer.DynamicInvoke(sourceItem, resultList[i]))
                        {
                            found = true;

                            break;
                        }
                    }

                    if (!found)
                    {
                        resultList.RemoveAt(i);
                    }
                }
            }

            private static void AddOrUpdateNewCollectionItems(NonScalarMapping mapping, IEnumerable sourceCollection,
                IList resultList, IDictionary<object, object> cache)
            {
                foreach (object sourceItem in sourceCollection)
                {
                    object resultItem = null;

                    foreach (object r in resultList)
                    {
                        if ((bool)mapping.IdentityComparer.DynamicInvoke(sourceItem, r))
                        {
                            resultItem = r;

                            break;
                        }
                    }

                    bool found = resultItem != null;

                    resultItem = MapInternal(sourceItem, resultItem, mapping.ResultItemType, cache, true);

                    if (!found)
                    {
                        resultList.Add(resultItem);
                    }
                }
            }

            private void CreateScalarMappings(Type sourceType, Type resultType, IEnumerable<PropertyPair> propertyPairs)
            {
                ParameterExpression sourceParameter = Expression.Parameter(sourceType, "source");
                ParameterExpression resultParameter = Expression.Parameter(resultType, "result");

                // Create a set of mappings for properties that are trivially mapped
                _scalarMappings.AddRange(
                    from m in propertyPairs
                    where m.ResultProperty.CanWrite
                          && m.SourceProperty.PropertyType == m.ResultProperty.PropertyType
                          && ((m.SourceProperty.PropertyType.IsValueType && m.ResultProperty.PropertyType.IsValueType)
                              || (typeof(string) == m.SourceProperty.PropertyType && typeof(string) == m.ResultProperty.PropertyType))
                    select
                        Expression.Lambda(
                        Expression.Call(resultParameter, m.ResultProperty.GetSetMethod(true),
                        Expression.Property(sourceParameter, m.SourceProperty)),
                        new[] { sourceParameter, resultParameter }).Compile());

                // Create a set of mappings where we can map from source to target as a string
                _scalarMappings.AddRange(
                    from m in propertyPairs
                    where m.SourceProperty.PropertyType != m.ResultProperty.PropertyType
                          && typeof(string) == m.ResultProperty.PropertyType
                    select
                        Expression.Lambda(
                        Expression.Call(resultParameter, m.ResultProperty.GetSetMethod(true),
                        Expression.Call(typeof(Convert).GetMethod("ToString", new[] { typeof(object) }),
                        Expression.Call(
                        Expression.Property(sourceParameter, m.SourceProperty),
                        m.SourceProperty.PropertyType.GetMethod("ToString", new Type[] { })))),
                        new[] { sourceParameter, resultParameter }).Compile());
            }

            private void CreateReferenceMappings(Type sourceType, Type resultType, IEnumerable<PropertyPair> propertyPairs)
            {
                ParameterExpression resultParameter = Expression.Parameter(resultType, "result");

                // Create a set of reference property mappings
                _referenceMappings.AddRange(
                    from m in propertyPairs
                    where m.ResultProperty.CanWrite
                          && !m.SourceProperty.PropertyType.IsValueType
                          && !m.ResultProperty.PropertyType.IsValueType
                          && typeof(string) != m.SourceProperty.PropertyType
                          && typeof(string) != m.ResultProperty.PropertyType
                          && !typeof(IEnumerable).IsAssignableFrom(m.SourceProperty.PropertyType)
                          && !typeof(IEnumerable).IsAssignableFrom(m.ResultProperty.PropertyType)
                    let valueParameter = Expression.Parameter(m.ResultProperty.PropertyType, "value")
                    select new ReferenceMapping
                    {
                        Source = CreatePropertyAccessor(sourceType, m.SourceProperty),
                        Result = CreatePropertyAccessor(resultType, m.ResultProperty),
                        ResultItemType = m.ResultProperty.PropertyType,
                        //IdentityComparer = CreateIdentityComparer(m.SourceProperty.PropertyType, m.ResultProperty.PropertyType),
                        Setter =
                Expression.Lambda(Expression.Call(resultParameter, m.ResultProperty.GetSetMethod(true), valueParameter),
                new[] { resultParameter, valueParameter }).Compile()
                    }
                    );
            }

            private void CreateCollectionMappings(Type sourceType, Type resultType, IEnumerable<PropertyPair> propertyPairs)
            {
                // Create a set of collection property mappings
                _collectionMappings.AddRange(
                    from m in propertyPairs
                    where m.SourceProperty.PropertyType.IsGenericType
                          && m.ResultProperty.PropertyType.IsGenericType
                    let sourceItemType = m.SourceProperty.PropertyType.GetGenericArguments().First()
                    let resultItemType = m.ResultProperty.PropertyType.GetGenericArguments().First()
                    let sourceCollectionType = typeof(IEnumerable<>).MakeGenericType(sourceItemType)
                    let resultCollectionType = typeof(ICollection<>).MakeGenericType(resultItemType)
                    where sourceCollectionType.IsAssignableFrom(m.SourceProperty.PropertyType)
                          && resultCollectionType.IsAssignableFrom(m.ResultProperty.PropertyType)
                    select new NonScalarMapping
                    {
                        Source = CreatePropertyAccessor(sourceType, m.SourceProperty),
                        Result = CreatePropertyAccessor(resultType, m.ResultProperty),
                        ResultItemType = resultItemType,
                        //IdentityComparer = CreateIdentityComparer(sourceItemType, resultItemType)
                    }
                    );
            }

            /// <summary>
            /// Build a fast identity comparer
            /// </summary>
            private static Delegate CreateIdentityComparer(Type sourceType, Type resultType)
            {
                ParameterExpression sourceParameter = Expression.Parameter(sourceType, "s");
                ParameterExpression resultParameter = Expression.Parameter(resultType, "r");

                PropertyInfo sourceProperty = sourceType.GetProperty(IdentityPropertyName);
                PropertyInfo resultProperty = resultType.GetProperty(IdentityPropertyName);

                return Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(sourceParameter, sourceProperty),
                        Expression.Property(resultParameter, resultProperty)),
                    new[] { sourceParameter, resultParameter }).Compile();
            }

            /// <summary>
            /// Build a fast property accessor
            /// </summary>
            private static Delegate CreatePropertyAccessor(Type type, PropertyInfo propertyInfo)
            {
                ParameterExpression sourceParameter = Expression.Parameter(type, "s");

                return Expression.Lambda(Expression.Property(sourceParameter, propertyInfo),
                    new[] { sourceParameter }).Compile();
            }

            #region Nested type: NonScalarMapping

            private class NonScalarMapping
            {
                public Delegate Source { get; set; }
                public Delegate Result { get; set; }
                public Type ResultItemType { get; set; }
                public Delegate IdentityComparer { get; set; }
            }

            #endregion

            #region Nested type: PropertyPair

            /// <summary>
            /// A pair of properties making up a mapping
            /// </summary>
            private class PropertyPair
            {
                public PropertyInfo SourceProperty { get; set; }
                public PropertyInfo ResultProperty { get; set; }
            }

            #endregion

            #region Nested type: ReferenceMapping

            private class ReferenceMapping : NonScalarMapping
            {
                public Delegate Setter { get; set; }
            }

            #endregion
        }

        #endregion
    }
}
