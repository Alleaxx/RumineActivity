using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using RumineActivity.Core.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RumineActivity.View
{
    /// <summary>
    /// Интерфейс сервиса по работе с файлами
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Сохранить файл с указанным именем (включая расширения) и текстовым содержанием
        /// </summary>
        Task SaveFile(string fileName, string content);

        /// <summary>
        /// Прочитать текстовый файл и попытаться десериализовать JSON-объект указанного типа
        /// </summary>
        //Task<T> TryReadFile<T>(IBrowserFile file);
    }

    /// <summary>
    /// Сервис работы с файлами по JS
    /// </summary>
    public class FileService : IFileService
    {
        private readonly IJSRuntime jsRuntime;
        private readonly Core.Logging.ILogger logger;
        public FileService(IJSRuntime jSRuntime, Core.Logging.ILogger logger)
        {
            this.jsRuntime = jSRuntime;
            this.logger = logger;
        }

        public async Task SaveFile(string fileName, string content)
        {
            try
            {
                await jsRuntime.InvokeAsync<object>("saveAsFile", fileName, content);
                logger.Log("Файл успешно сохранен");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Log($"Ошибка сохранения в файл {ex.Message}");
            }
        }

        //public Task SaveFile(string fileName, object obj)
        //{
        //    return SaveFile(fileName, jsonService.ConvertToJson(obj));
        //}

        public async Task<T> TryReadFile<T>(IBrowserFile file)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                await file.OpenReadStream().CopyToAsync(ms);
                var bytes = ms.ToArray();
                var res = System.Text.Encoding.Default.GetString(bytes);
                logger.Log("Файл успешно прочитан");
                //return jsonService.CreateFromJson<T>(res);
                return default;
            }
            catch (Exception ex)
            {
                logger.Log($"Ошибка чтения файла {ex.Message}");
            }
            finally
            {
                ms.Close();
            }
            return default;
        }
    }
}
