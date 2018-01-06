using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Crypto
{
    /// <summary>
    /// Provides cached access to a type's property getters.
    /// </summary>
    public sealed class PropertyReader
    {
        /// <summary>
        /// A bitmask indicating which types of properties are cached in this <see cref="PropertyReader"/> instance.
        /// </summary>
        public BindingFlags BindingFlags { get; }

        /// <summary>
        /// Returns the read-only cache of property getters that have been initialized.
        /// </summary>
        public IReadOnlyDictionary<string, Func<object, object>> GetterCache { get; }

        /// <summary>
        /// Returns the type that this <see cref="PropertyReader"/> instance was generated from.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyReader"/> class.
        /// </summary>
        /// <param name="type">The type that property getters will be derived from.</param>
        /// <param name="includePublic">Indicates whether public properties should be included.</param>
        /// <param name="includePrivate">Indicates whether private properties should be included.</param>
        /// <param name="includeInstance">Indicates whether instance properties should be included.</param>
        /// <param name="includeStatic">Indicates whether static properties should be included.</param>
        public PropertyReader(Type type, bool includePublic, bool includePrivate, bool includeInstance, bool includeStatic)
        {
            if (type.IsNull())
            {
                throw new ArgumentNullException(paramName: nameof(type));
            }

            if (type.UnwrapIfNullable().IsValueType)
            {
                throw new InvalidOperationException("unable to construct property getters on value type " + type.ToString());
            }

            BindingFlags = (
                (includeInstance ? BindingFlags.Instance : BindingFlags.Default)
              | (includePrivate ? BindingFlags.NonPublic : BindingFlags.Default)
              | (includePublic ? BindingFlags.Public : BindingFlags.Default)
              | (includeStatic ? BindingFlags.Static : BindingFlags.Default)
            );
            GetterCache = new SortedDictionary<string, Func<object, object>>(
                dictionary: type
                    .GetProperties(BindingFlags)
                    .Where(a => a.GetIndexParameters().IsEmpty())
                    .Select(a => new {
                        DeclaringType = a.DeclaringType,
                        GetMethod = a.GetGetMethod(includePrivate),
                        Name = a.Name,
                        PropertyType = a.PropertyType,
                    })
                    .Where(a => a.GetMethod.IsNotNull())
                    .OrderBy(a => a.Name)
                    .ToDictionary(
                        k => k.Name,
                        v => BuildGetAccessor(v.DeclaringType, v.PropertyType, v.GetMethod)
                    ),
                comparer: StringComparer.OrdinalIgnoreCase
            );
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyReader"/> class.
        /// </summary>
        /// <param name="type">The type that property getters will be derived from.</param>
        public PropertyReader(Type type) : this(type: type, includePublic: true, includePrivate: false, includeInstance: true, includeStatic: false) { }

        /// <summary>
        /// Uses the <see cref="PropertyReader"/> to get the value of a property by name.
        /// </summary>
        /// <param name="instance">The instance to get a property value from.</param>
        /// <param name="propertyName">The name of the property to retrieve.</param>
        public object GetValue(object instance, string propertyName)
        {
            return GetterCache[propertyName](instance);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyReader"/> class.
        /// </summary>
        /// <param name="type">The type that property getters will be derived from.</param>
        /// <param name="includePublic">Indicates whether public properties should be included.</param>
        /// <param name="includePrivate">Indicates whether private properties should be included.</param>
        /// <param name="includeInstance">Indicates whether instance properties should be included.</param>
        /// <param name="includeStatic">Indicates whether static properties should be included.</param>
        public static PropertyReader Create(Type type, bool includePublic, bool includePrivate, bool includeInstance, bool includeStatic)
        {
            return new PropertyReader(type, includePublic, includePrivate, includeInstance, includeStatic);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyReader"/> class.
        /// </summary>
        /// <param name="type">The type that property getters will be derived from.</param>
        public static PropertyReader Create(Type type)
        {
            return new PropertyReader(type);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyReader"/> class.
        /// </summary>
        /// <typeparam name="T">The type that property getters will be derived from.</typeparam>
        /// <param name="includePublic">Indicates whether public properties should be included.</param>
        /// <param name="includePrivate">Indicates whether private properties should be included.</param>
        /// <param name="includeInstance">Indicates whether instance properties should be included.</param>
        /// <param name="includeStatic">Indicates whether static properties should be included.</param>
        public static PropertyReader Create<T>(bool includePublic, bool includePrivate, bool includeInstance, bool includeStatic)
        {
            return Create(typeof(T), includePublic, includePrivate, includeInstance, includeStatic);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyReader"/> class.
        /// </summary>
        /// <typeparam name="T">The type that property getters will be derived from.</typeparam>
        public static PropertyReader Create<T>()
        {
            return Create(typeof(T));
        }

        private static Func<object, object> BuildGetAccessor(Type instanceType, Type propertyType, MethodInfo getMethod)
        {
            var getMethodDynamicCall = new DynamicMethod(
                (getMethod.Name + "_DynamicGetter_" + Guid.NewGuid().ToString("N").ToUpper()),
                typeof(object),
                new[] { typeof(object) },
                instanceType,
                true
            );
            var il = getMethodDynamicCall.GetILGenerator();

            if (getMethod.IsStatic)
            {
                il.EmitCall(OpCodes.Call, getMethod, null);
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);

                if (instanceType.IsValueType)
                {
                    il.Emit(OpCodes.Unbox, instanceType);
                    il.EmitCall(OpCodes.Call, getMethod, null);
                }
                else
                {
                    il.Emit(OpCodes.Castclass, instanceType);
                    il.EmitCall(OpCodes.Callvirt, getMethod, null);
                }
            }

            if (propertyType.IsValueType)
            {
                il.Emit(OpCodes.Box, propertyType);
            }

            il.Emit(OpCodes.Ret);

            return (getMethodDynamicCall.CreateDelegate(typeof(Func<,>).MakeGenericType(typeof(object), typeof(object))) as Func<object, object>);
        }
    }
}