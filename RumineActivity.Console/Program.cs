using RumineActivity;
using RumineActivity.ConsoleApp;
using RumineActivity.Core;
using RumineActivity.Core.API;
using RumineActivity.Core.Logging;
using RumineActivity.Core.Models;
using System.Text.Json;

var httpClient = new HttpClient();
var logger = new ConsoleLogger();

await CreateJsonPostsFileFromCsvAsync();

await FullAnalyze();

//await JsAnalyze();

Console.ReadLine();

async Task JsAnalyze()
{
    string path = "C:\\Users\\Alleaxx\\source\\repos\\Источники данных\\Статистика активности\\ForumPosts.txt";
    var JsLogAnalyze = new JsLogFileAnalyze(logger, path);
    await JsLogAnalyze.Start();
}
async Task FullAnalyze()
{
    //Полный, из SQL
    //var fileInfo = JsonFileInfo.FromSqlJson("C:\Users\Alleaxx\source\repos\Данные\Статистика активности\\ForumPostsBaseFull921767.json", false);

    //Краткий, из SQL
    //var fileInfo = JsonFileInfo.FromSqlJson("C:\Users\Alleaxx\source\repos\Данные\Статистика активности\\ForumPostsBaseDayStartEnd2.json", false);


    //Краткий, из объектов
    var fileInfo = JsonFileInfo.FromObjectJson("C:\\Users\\Alleaxx\\source\\repos\\Данные\\Статистика активности\\ForumPostsCsvUnited.json", false);

    var fileApi = new ActivityFileApi(httpClient, logger, fileInfo);
    await fileApi.LoadData();
    var forum = await fileApi.GetForum();

    var forumAnalyze = new ForumAnalyze(forum);
    forumAnalyze.Analyze();
}

async Task<string> CreateJsonPostsFileFromCsvAsync()
{
    string[] csvFiles = new string[]
    {
        "C:\\Users\\Alleaxx\\source\\repos\\Данные\\Статистика Румине (по запросам)\\2023-03-09\\dle_be_message.csv",
        "C:\\Users\\Alleaxx\\source\\repos\\Данные\\Статистика Румине (по запросам)\\2025-05-07\\dle_be_message.csv",
    };
    Dictionary<int, DateTime> csvPosts = new Dictionary<int, DateTime>();
    foreach (var file in csvFiles)
    {
        Console.WriteLine($"Чтение файла: {file}");
        var lines = await File.ReadAllLinesAsync(file);
        foreach (var line in lines)
        {
            var splitted = line.Split(',');
            var idText = splitted[0];
            var dateText = splitted[splitted.Length - 1];

            if (idText.StartsWith('"'))
            {
                idText = idText.Substring(1, idText.Length - 2);
                dateText = dateText.Substring(1, dateText.Length - 2);
            }

            var numID = int.Parse(idText);
            DateTime? date = null;
            if(int.TryParse(dateText, out int timestamp))
            {
                date = UnixTimeStampToDateTime(timestamp);
            }
            else if(DateTime.TryParse(dateText, out DateTime dateTime))
            {
                date = dateTime;
            }

            if(!date.HasValue)
            {
                throw new Exception($"Не удалось вычислить дату поста {numID}: {dateText}");
            }

            if (!csvPosts.ContainsKey(numID))
            {
                csvPosts.Add(numID, date.Value);
            }
            else
            {
                Console.WriteLine("&&&");
            }
        }
    }

    Console.WriteLine($"Итого прочитано {csvPosts.Count:#,0} постов");

    var posts = csvPosts.Select(p => new Post(p.Key, p.Value)).ToArray();
    var json = JsonSerializer.Serialize(posts);

    string path = "C:\\Users\\Alleaxx\\source\\repos\\Данные\\Статистика активности\\ForumPostsCsvUnited.json";
    await File.WriteAllTextAsync(path, json);

    Console.WriteLine($"Посты записаны в JSON файл");
    return path;
}
static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
{
    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
    return dateTime;
}
