using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class StatApp
    {
        public readonly HttpClient Client;
        private readonly ActivityWebApi WebApi;
        private bool OnlineMode { get; set; } = false;


        //Разделы приложения
        public StatisticsReport Report { get; set; }
        public Comparison Comparison { get; private set; }
        public ViewOptions ViewOptions { get; private set; }

        //Данные
        public List<Post> Posts { get; private set; }
        public List<Topic> Topics { get; private set; }

        private void AddLocal(Post post)
        {
            Posts.Add(post);
        }
        private void AddLocal(Topic topic)
        {
            Topics.Add(topic);
        }


        public StatApp(HttpClient client)
        {
            Client = client;
            WebApi = new ActivityWebApi("https://localhost:44341", Client);
            SetOfflineSource();

            ViewOptions = new ViewOptions();
            Comparison = new Comparison(Topics);

            ReportsFactory.SetDefaultData(this);
            Report = ReportsFactory.CreateReport(Reports.Periodical, new ReportCreatorOptions(), new ForumSource(Posts, Topics));

            LoadDataFromApi();
        }
        private async void LoadDataFromApi()
        {
            if (OnlineMode)
            {
                await SetOnlineSource();
            }
        }

        public async Task SetOnlineSource()
        {
            var posts = await WebApi.GetAllPosts();
            var topics = await WebApi.GetAllTopics();

            if(posts != null && topics != null)
            {
                Topics = new List<Topic>(topics);
                Posts = new List<Post>(posts);
            }
        }

        private void SetOfflineSource()
        {
            Posts = new List<Post>();
            Topics = new List<Topic>();
            AddLocal(new Topic() { Id = 15361, IsChat = true, Name = "Форумный чат" });
            AddLocal(new Topic() { Id = 13408, IsChat = true, Name = "[ФОРYМHЫИ ЧRT]" });
            AddLocal(new Topic() { Id = 11371, IsChat = true, Name = "TYXЛRЧ0К" });

            AddLocal(new Post() { Id = 891466, TopicId = 11371, TopicIndex = 1, Date = new DateTime(2014, 7, 21) });
            AddLocal(new Post() { Id = 906988, TopicId = 11371, TopicIndex = 11472, Date = new DateTime(2014, 8, 1) });
            AddLocal(new Post() { Id = 973813, TopicId = 11371, TopicIndex = 43147, Date = new DateTime(2014, 9, 1) });
            AddLocal(new Post() { Id = 989246, TopicId = 11371, TopicIndex = 54536, Date = new DateTime(2014, 10, 1) });
            AddLocal(new Post() { Id = 1004209, TopicId = 11371, TopicIndex = 66275, Date = new DateTime(2014, 11, 1) });
            AddLocal(new Post() { Id = 1016646, TopicId = 11371, TopicIndex = 75422, Date = new DateTime(2014, 12, 1) });
            AddLocal(new Post() { Id = 1036078, TopicId = 11371, TopicIndex = 86701, Date = new DateTime(2015, 1, 1) });
            AddLocal(new Post() { Id = 1063961, TopicId = 11371, TopicIndex = 108719, Date = new DateTime(2015, 2, 1) });
            AddLocal(new Post() { Id = 1079034, TopicId = 11371, TopicIndex = 121312, Date = new DateTime(2015, 3, 1) });
            AddLocal(new Post() { Id = 1090150, TopicId = 11371, TopicIndex = 130062, Date = new DateTime(2015, 4, 1) });
            AddLocal(new Post() { Id = 1094867, TopicId = 11371, TopicIndex = 134060, Date = new DateTime(2015, 4, 10, 21, 0, 0) });
            AddLocal(new Post() { Id = 1094942, TopicId = 13408, TopicIndex = 1, Date = new DateTime(2015, 4, 10, 21, 54, 0) });
            AddLocal(new Post() { Id = 1101882, TopicId = 13408, TopicIndex = 5835, Date = new DateTime(2015, 5, 1) });
            AddLocal(new Post() { Id = 1109911, TopicId = 13408, TopicIndex = 12475, Date = new DateTime(2015, 6, 1) });
            AddLocal(new Post() { Id = 1116139, TopicId = 13408, TopicIndex = 17386, Date = new DateTime(2015, 7, 1) });
            AddLocal(new Post() { Id = 1130308, TopicId = 13408, TopicIndex = 28879, Date = new DateTime(2015, 8, 1) });
            AddLocal(new Post() { Id = 1145394, TopicId = 13408, TopicIndex = 41810, Date = new DateTime(2015, 9, 1) });
            AddLocal(new Post() { Id = 1152127, TopicId = 13408, TopicIndex = 47585, Date = new DateTime(2015, 10, 1) });
            AddLocal(new Post() { Id = 1158425, TopicId = 13408, TopicIndex = 52971, Date = new DateTime(2015, 11, 1) });
            AddLocal(new Post() { Id = 1164668, TopicId = 13408, TopicIndex = 58119, Date = new DateTime(2015, 12, 1) });
            AddLocal(new Post() { Id = 1169757, TopicId = 13408, TopicIndex = 61836, Date = new DateTime(2016, 1, 1) });
            AddLocal(new Post() { Id = 1173984, TopicId = 13408, TopicIndex = 64883, Date = new DateTime(2016, 2, 1) });
            AddLocal(new Post() { Id = 1177698, TopicId = 13408, TopicIndex = 67430, Date = new DateTime(2016, 3, 1) });
            AddLocal(new Post() { Id = 1191412, TopicId = 13408, TopicIndex = 79349, Date = new DateTime(2016, 4, 1) });
            AddLocal(new Post() { Id = 1202086, TopicId = 13408, TopicIndex = 88946, Date = new DateTime(2016, 5, 1) });
            AddLocal(new Post() { Id = 1208486, TopicId = 13408, TopicIndex = 94450, Date = new DateTime(2016, 6, 1) });
            AddLocal(new Post() { Id = 1215008, TopicId = 13408, TopicIndex = 99636, Date = new DateTime(2016, 7, 1) });
            AddLocal(new Post() { Id = 1235106, TopicId = 13408, TopicIndex = 118139, Date = new DateTime(2016, 8, 1) });
            AddLocal(new Post() { Id = 1239049, TopicId = 13408, TopicIndex = 121930, Date = new DateTime(2016, 8, 3, 0,0,0) });
            AddLocal(new Post() { Id = 1239050, TopicId = 15361, TopicIndex = 1, Date = new DateTime(2016, 8, 3,1,0,0) });
            AddLocal(new Post() { Id = 1266015, TopicId = 15361, TopicIndex = 25108, Date = new DateTime(2016, 9, 1) });
            AddLocal(new Post() { Id = 1276411, TopicId = 15361, TopicIndex = 34968, Date = new DateTime(2016, 10, 1) });
            AddLocal(new Post() { Id = 1281285, TopicId = 15361, TopicIndex = 39317, Date = new DateTime(2016, 11, 1) });
            AddLocal(new Post() { Id = 1283146, TopicId = 15361, TopicIndex = 40687, Date = new DateTime(2016, 12, 1) });
            AddLocal(new Post() { Id = 1287860, TopicId = 15361, TopicIndex = 44202, Date = new DateTime(2017, 1, 1) });
            AddLocal(new Post() { Id = 1296431, TopicId = 15361, TopicIndex = 52009, Date = new DateTime(2017, 2, 1) });
            AddLocal(new Post() { Id = 1306895, TopicId = 15361, TopicIndex = 61870, Date = new DateTime(2017, 3, 1) });
            AddLocal(new Post() { Id = 1317647, TopicId = 15361, TopicIndex = 71955, Date = new DateTime(2017, 4, 1) });
            AddLocal(new Post() { Id = 1323059, TopicId = 15361, TopicIndex = 76978, Date = new DateTime(2017, 5, 1) });
            AddLocal(new Post() { Id = 1327463, TopicId = 15361, TopicIndex = 81044, Date = new DateTime(2017, 6, 1) });
            AddLocal(new Post() { Id = 1336051, TopicId = 15361, TopicIndex = 89203, Date = new DateTime(2017, 7, 1) });
            AddLocal(new Post() { Id = 1340855, TopicId = 15361, TopicIndex = 93441, Date = new DateTime(2017, 8, 1) });
            AddLocal(new Post() { Id = 1347935, TopicId = 15361, TopicIndex = 99564, Date = new DateTime(2017, 9, 1) });
            AddLocal(new Post() { Id = 1353460, TopicId = 15361, TopicIndex = 104444, Date = new DateTime(2017, 10, 1) });
            AddLocal(new Post() { Id = 1361465, TopicId = 15361, TopicIndex = 111869, Date = new DateTime(2017, 11, 1) });
            AddLocal(new Post() { Id = 1365016, TopicId = 15361, TopicIndex = 114934, Date = new DateTime(2017, 12, 1) });
            AddLocal(new Post() { Id = 1367725, TopicId = 15361, TopicIndex = 116647, Date = new DateTime(2018, 1, 1) });
            AddLocal(new Post() { Id = 1372087, TopicId = 15361, TopicIndex = 120120, Date = new DateTime(2018, 2, 1) });
            AddLocal(new Post() { Id = 1374414, TopicId = 15361, TopicIndex = 121956, Date = new DateTime(2018, 3, 1) });
            AddLocal(new Post() { Id = 1377002, TopicId = 15361, TopicIndex = 123869, Date = new DateTime(2018, 4, 1) });
            AddLocal(new Post() { Id = 1379863, TopicId = 15361, TopicIndex = 126262, Date = new DateTime(2018, 5, 1) });
            AddLocal(new Post() { Id = 1382429, TopicId = 15361, TopicIndex = 128435, Date = new DateTime(2018, 6, 1) });
            AddLocal(new Post() { Id = 1386481, TopicId = 15361, TopicIndex = 131838, Date = new DateTime(2018, 7, 1) });
            AddLocal(new Post() { Id = 1389035, TopicId = 15361, TopicIndex = 133708, Date = new DateTime(2018, 8, 1) });
            AddLocal(new Post() { Id = 1390417, TopicId = 15361, TopicIndex = 134621, Date = new DateTime(2018, 9, 1) });
            AddLocal(new Post() { Id = 1391153, TopicId = 15361, TopicIndex = 135015, Date = new DateTime(2018, 10, 1) });
            AddLocal(new Post() { Id = 1392057, TopicId = 15361, TopicIndex = 135499, Date = new DateTime(2018, 11, 1) });
            AddLocal(new Post() { Id = 1392898, TopicId = 15361, TopicIndex = 135970, Date = new DateTime(2018, 12, 1) });
            AddLocal(new Post() { Id = 1393861, TopicId = 15361, TopicIndex = 136616, Date = new DateTime(2019, 1, 1) });
            AddLocal(new Post() { Id = 1394634, TopicId = 15361, TopicIndex = 137016, Date = new DateTime(2019, 2, 1) });
            AddLocal(new Post() { Id = 1395436, TopicId = 15361, TopicIndex = 137500, Date = new DateTime(2019, 3, 1) });
            AddLocal(new Post() { Id = 1396547, TopicId = 15361, TopicIndex = 138331, Date = new DateTime(2019, 4, 1) });
            AddLocal(new Post() { Id = 1398197, TopicId = 15361, TopicIndex = 139744, Date = new DateTime(2019, 5, 1) });
            AddLocal(new Post() { Id = 1399727, TopicId = 15361, TopicIndex = 140956, Date = new DateTime(2019, 6, 1) });
            AddLocal(new Post() { Id = 1402003, TopicId = 15361, TopicIndex = 142798, Date = new DateTime(2019, 7, 1) });
            AddLocal(new Post() { Id = 1405274, TopicId = 15361, TopicIndex = 145592, Date = new DateTime(2019, 8, 1) });
            AddLocal(new Post() { Id = 1406815, TopicId = 15361, TopicIndex = 146658, Date = new DateTime(2019, 9, 1) });
            AddLocal(new Post() { Id = 1408192, TopicId = 15361, TopicIndex = 147795, Date = new DateTime(2019, 10, 1) });
            AddLocal(new Post() { Id = 1409636, TopicId = 15361, TopicIndex = 148825, Date = new DateTime(2019, 11, 1) });
            AddLocal(new Post() { Id = 1410923, TopicId = 15361, TopicIndex = 149661, Date = new DateTime(2019, 12, 1) });
            AddLocal(new Post() { Id = 1412736, TopicId = 15361, TopicIndex = 151208, Date = new DateTime(2020, 1, 1) });
            AddLocal(new Post() { Id = 1415065, TopicId = 15361, TopicIndex = 153151, Date = new DateTime(2020, 2, 1) });
            AddLocal(new Post() { Id = 1416111, TopicId = 15361, TopicIndex = 153794, Date = new DateTime(2020, 3, 1) });
            AddLocal(new Post() { Id = 1417362, TopicId = 15361, TopicIndex = 154567, Date = new DateTime(2020, 4, 1) });
            AddLocal(new Post() { Id = 1418961, TopicId = 15361, TopicIndex = 155411, Date = new DateTime(2020, 5, 1) });
            AddLocal(new Post() { Id = 1426235, TopicId = 15361, TopicIndex = 161680, Date = new DateTime(2020, 6, 1) });
            AddLocal(new Post() { Id = 1432560, TopicId = 15361, TopicIndex = 167240, Date = new DateTime(2020, 7, 1) });
            AddLocal(new Post() { Id = 1436065, TopicId = 15361, TopicIndex = 170116, Date = new DateTime(2020, 8, 1) });
            AddLocal(new Post() { Id = 1438888, TopicId = 15361, TopicIndex = 172343, Date = new DateTime(2020, 9, 1) });
            AddLocal(new Post() { Id = 1439986, TopicId = 15361, TopicIndex = 173095, Date = new DateTime(2020, 10, 1) });
            AddLocal(new Post() { Id = 1440952, TopicId = 15361, TopicIndex = 173695, Date = new DateTime(2020, 11, 1) });
            AddLocal(new Post() { Id = 1442093, TopicId = 15361, TopicIndex = 174441, Date = new DateTime(2020, 12, 1) });
            AddLocal(new Post() { Id = 1443241, TopicId = 15361, TopicIndex = 175260, Date = new DateTime(2021, 1, 1) });
            AddLocal(new Post() { Id = 1444863, TopicId = 15361, TopicIndex = 176499, Date = new DateTime(2021, 2, 1) });
            AddLocal(new Post() { Id = 1445887, TopicId = 15361, TopicIndex = 177167, Date = new DateTime(2021, 3, 1) });
            AddLocal(new Post() { Id = 1448341, TopicId = 15361, TopicIndex = 178819, Date = new DateTime(2021, 4, 1) });
            AddLocal(new Post() { Id = 1450910, TopicId = 15361, TopicIndex = 180772, Date = new DateTime(2021, 5, 1) });
            AddLocal(new Post() { Id = 1453470, TopicId = 15361, TopicIndex = 182857, Date = new DateTime(2021, 6, 1) });
            AddLocal(new Post() { Id = 1455267, TopicId = 15361, TopicIndex = 184201, Date = new DateTime(2021, 7, 1) });
            AddLocal(new Post() { Id = 1456509, TopicId = 15361, TopicIndex = 185060, Date = new DateTime(2021, 8, 1) });
        }
        private void SetDebugSource()
        {
            Posts = new List<Post>();
            Topics = new List<Topic>();

            Topics.Clear();
            Posts.Clear();

            AddLocal(new Topic() { Id = 1, IsChat = true, Name = "Форумный чат" });
            AddLocal(new Topic() { Id = 2, IsChat = false, Name = "Не чат" });

            AddLocal(new Post() { Id = 0, TopicId = 1, TopicIndex = 1, Date = new DateTime(2012, 9, 1) });
            AddLocal(new Post() { Id = 50, TopicId = 1, TopicIndex = 1, Date = new DateTime(2012, 9, 15) });
            AddLocal(new Post() { Id = 75, TopicId = 1, TopicIndex = 1, Date = new DateTime(2012, 9, 30) });
            AddLocal(new Post() { Id = 100, TopicId = 1, TopicIndex = 1, Date = new DateTime(2012, 10, 10) });
            AddLocal(new Post() { Id = 110, TopicId = 1, TopicIndex = 1, Date = new DateTime(2012, 10, 20) });
            AddLocal(new Post() { Id = 115, TopicId = 1, TopicIndex = 1, Date = new DateTime(2012, 10, 30) });
        }
    }
}
