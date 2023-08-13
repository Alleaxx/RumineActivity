using RumineActivity.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace RumineActivity.View
{
    /// <summary>
    /// Интерфейс сервиса по по сериализации и десереализации JSON-объектов
    /// </summary>
    public interface IJsonService
    {
        /// <summary>
        /// Пытается десериализовать объект указанного типа из JSON.
        /// При неудаче возвращает default и записывает лог
        /// </summary>
        T CreateFromJson<T>(string text);

        /// <summary>
        /// Сериализует объект в JSON
        /// </summary>
        string ConvertToJson<T>(T obj);
    }

    /// <summary>
    /// Стандартный сервис по сериализации и десереализации JSON-объектов
    /// </summary>
    public class JsonServiceDefault : IJsonService
    {
        private JsonSerializerOptions Options { get; init; }
        private Core.Logging.ILogger Logger { get; init; }
        public JsonServiceDefault(Core.Logging.ILogger logger)
        {
            Logger = logger;
            Options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IgnoreNullValues = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            };
        }

        public string ConvertToJson<T>(T obj)
        {
            try
            {
                string json = JsonSerializer.Serialize(obj, Options);
                return json;
            }
            catch (JsonException ex)
            {
                Logger.Log($"Ошибка сериализации в JSON объекта {typeof(T).Name} {ex.Message}");
                return null;
            }
        }


        public T CreateFromJson<T>(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return default;
            }
            try
            {
                T obj = JsonSerializer.Deserialize<T>(text, Options);
                Logger.Log($"Успешная десериализация {typeof(T).Name}");
                return obj;
            }
            catch (JsonException ex)
            {
                Logger.Log($"Ошибка при десериализации JSON {ex.Message}");
                return default;
            }
        }
    }
}
