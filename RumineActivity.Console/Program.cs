using RumineActivity;
using RumineActivity.ConsoleApp;
using RumineActivity.Core;
using RumineActivity.Core.API;
using RumineActivity.Core.Logging;

var httpClient = new HttpClient();
var logger = new ConsoleLogger();

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
    //var fileInfo = JsonFileInfo.FromSqlJson("C:\\Users\\Alleaxx\\source\\repos\\Источники данных\\Статистика активности\\ForumPostsBaseFull921767.json", false);

    //Краткий, из SQL
    //var fileInfo = JsonFileInfo.FromSqlJson("C:\\Users\\Alleaxx\\source\\repos\\Источники данных\\Статистика активности\\ForumPostsBaseDayStartEnd2.json", false);

    //Краткий, из объектов
    var fileInfo = JsonFileInfo.FromObjectJson("C:\\Users\\Alleaxx\\source\\repos\\Источники данных\\Статистика активности\\ForumPostsV4.json", false);

    var fileApi = new ActivityFileApi(httpClient, logger, fileInfo);
    await fileApi.LoadData();
    var forum = await fileApi.GetForum();

    var forumAnalyze = new ForumAnalyze(forum);
    forumAnalyze.Analyze();
}

