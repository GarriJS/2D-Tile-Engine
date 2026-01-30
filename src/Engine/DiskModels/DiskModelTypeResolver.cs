using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Engine.DiskModels
{
	/// <summary>
	/// Represents a disk model type resolver.
	/// </summary>
	sealed public class DiskModelTypeResolver : DefaultJsonTypeInfoResolver
	{
		/// <summary>
		/// The type discriminator name.
		/// </summary>
		static readonly private string TypeDiscriminatorName = "Type";

		/// <summary>
		/// The type map.
		/// </summary>
		static readonly public Dictionary<string, Type> TypeMap = [];

		/// <summary>
		/// Registers the assembly base disk models.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		static public void RegisterAssembly(Assembly assembly)
		{
			var types = assembly.GetTypes()
								.Where(e => (false == e.IsAbstract) &&
											(true == typeof(BaseDiskModel).IsAssignableFrom(e)))
								.ToArray();

			foreach (var type in types)
				TypeMap[type.Name] = type;
		}

		/// <summary>
		/// Registers the type.
		/// </summary>
		/// <typeparam name="T">The base disk model type.</typeparam>
		static public void RegisterType<T>() where T : BaseDiskModel
		{
			TypeMap[typeof(T).Name] = typeof(T);
		}

		/// <summary>
		/// Gets the type info.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="options">The options.</param>
		/// <returns>The JSON type info.</returns>
		override public JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
		{
			var info = base.GetTypeInfo(type, options);

			if (typeof(BaseDiskModel).IsAssignableFrom(type) && type.IsAbstract)
			{
				info.PolymorphismOptions = new JsonPolymorphismOptions
				{
					TypeDiscriminatorPropertyName = TypeDiscriminatorName,
					IgnoreUnrecognizedTypeDiscriminators = false
				};

				foreach (var (name, derivedType) in TypeMap)
					if (true == type.IsAssignableFrom(derivedType))
						info.PolymorphismOptions.DerivedTypes.Add(
							new JsonDerivedType(derivedType, name));
			}

			return info;
		}
	}
}
