using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Engine.DiskModels
{
	/// <summary>
	/// Provides methods for deserializes base disk models.
	/// </summary>
	static public class DiskModelDeserializer
	{
		/// <summary>
		/// Gets the type map.
		/// </summary>
		static Dictionary<string, Type> TypeMap { get; } = [];

		/// <summary>
		/// Registers the assembly base disk models.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		public static void RegisterAssembly(Assembly assembly)
		{
			var types = assembly.GetTypes()
								.Where(e => (false == e.IsAbstract) && 
											(true == typeof(BaseDiskModel).IsAssignableFrom(e)));

			foreach (var type in types)
			{
				TypeMap[type.Name] = type;
			}
		}

		/// <summary>
		/// Registers the type.
		/// </summary>
		/// <typeparam name="T">The base disk model type.</typeparam>
		public static void RegisterType<T>() where T : BaseDiskModel
		{
			TypeMap[typeof(T).Name] = typeof(T);
		}

		/// <summary>
		/// Deserializes the JSON of the base disk models.
		/// </summary>
		/// <param name="json">The JSON of the base disk models.</param>
		/// <param name="options">The options.</param>
		/// <returns>The base disk model.</returns>
		public static BaseDiskModel Deserialize(string json, JsonSerializerOptions options = null)
		{
			if (true == string.IsNullOrEmpty(json))
				return null;

			var node = JsonNode.Parse(json);
			var result = DeserializeNode(node, options);

			return result;
		}

		/// <summary>
		/// Deserializes the JSON of the base disk models.
		/// </summary>
		/// <param name="node">The JSON node of the base disk models.</param>
		/// <param name="options">The options</param>
		/// <returns>The base disk model.</returns>
		private static BaseDiskModel DeserializeNode(JsonNode node, JsonSerializerOptions options)
		{
			if (null == node) 
				return null;

			if (node is not JsonObject objNode)
				throw new Exception("Expected JsonObject at BaseDiskModel level");

			if (null == objNode["type"])
				throw new Exception("Missing 'type' property in serialized object");

			var typeName = objNode["type"].GetValue<string>();

			if (false == TypeMap.TryGetValue(typeName, out var targetType))
				throw new Exception($"Unknown type: {typeName}");

			var model = (BaseDiskModel)objNode.Deserialize(targetType, options);

			foreach (var property in targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				if ((false == property.CanRead) ||
					(false == property.CanWrite))
					continue;

				var propertyValue = property.GetValue(model);
				
				if (propertyValue == null) 
					continue;

				var propertyType = property.PropertyType;

				if ((true == typeof(BaseDiskModel).IsAssignableFrom(propertyType)) && 
					(propertyValue is JsonNode childNode))
				{
					property.SetValue(model, DeserializeNode(childNode, options));
				}
				else if ((true == typeof(IEnumerable).IsAssignableFrom(propertyType)) && 
					     (true == propertyType.IsGenericType))
				{
					var itemType = propertyType.GetGenericArguments()[0];

					if (true == typeof(BaseDiskModel).IsAssignableFrom(itemType))
					{
						var jsonArray = node[property.Name].AsArray();
						var listInstance = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));
						foreach (var child in jsonArray)
							listInstance.Add(DeserializeNode(child, options));
						property.SetValue(model, listInstance);
					}
				}
			}

			return model;
		}
	}
}
