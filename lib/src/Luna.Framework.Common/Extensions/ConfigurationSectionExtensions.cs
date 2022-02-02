// using Luna.Common.Infrastructure.Exceptions;
// using Microsoft.Extensions.Configuration;
//
// namespace Luna.Common.Infrastructure.Extensions;
//
// public static class ConfigurationSectionExtensions
// {
//   public static string GetRequiredString(this IConfigurationSection section, string key)
//   {
//     var value = section?.GetString(key);
//     if (string.IsNullOrEmpty(value))
//     {
//       throw new FrameworkException($"Configuration value not found or empty: {section.Key}:{key}");
//     }
//
//     return value;
//   }
//
//   public static string GetString(this IConfigurationSection section, string key)
//   {
//     return section?.GetValue<string>(key);
//   }
//
//   public static T GetRequiredValue<T>(this IConfigurationSection section, string key)
//   {
//     if (typeof(T) == typeof(string))
//       return (T)(object)GetRequiredString(section, key);
//
//     if (section == null)
//       return default(T);
//
//     var value = section.GetValue<T>(key);
//     if (value == null)
//     {
//       throw new FrameworkException($"Configuration value not found: {section.Key}:{key}");
//     }
//
//     return value;
//   }
//
//   public static T GetRequiredObject<T>(this IConfigurationSection section, string key)
//   {
//     if (typeof(T) == typeof(string))
//       return (T)(object)GetRequiredString(section, key);
//
//     if (section == null)
//       return default(T);
//
//     var value = section.Get<T>();
//     if (value == null)
//     {
//       throw new FrameworkException($"Configuration value not found: {section.Key}:{key}");
//     }
//
//     return value;
//   }
// }
