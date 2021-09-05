using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class StatApp
    {
        private static readonly StatApp app = new StatApp();
        public static StatApp App => app;

        public bool OfflineMode { get; set; } = true;
        public NewPost EditingPost { get; private set; }


        public List<Post> Posts { get; private set; }
        public List<Topic> Topics { get; private set; }

        public void Add(Post newPost)
        {
            Posts.Add(newPost);
        }
        public void Add(Topic newTopic)
        {
            Topics.Add(newTopic);
        }

        public void Remove(Post post)
        {
            Posts.Remove(post);
        }
        public void Remove(Topic topic)
        {
            Topics.Remove(topic);
        }

        public StatApp()
        {
            EditingPost = new NewPost();
            Posts = new List<Post>();
            Topics = new List<Topic>();

            if (OfflineMode)
            {
                Add(new Topic() { ID = 15361, IsChat = true, Name = "Форумный чат" });
                Add(new Topic() { ID = 13408, IsChat = true, Name = "[ФОРYМHЫИ ЧRT]" });
                Add(new Topic() { ID = 11371, IsChat = true, Name = "TYXЛRЧ0К" });

                Add(new Post() { ID = 891466, TopicID = 11371, TopicIndex = 1, Date = new DateTime(2014, 7, 21) });
                Add(new Post() { ID = 906988, TopicID = 11371, TopicIndex = 11472, Date = new DateTime(2014, 8, 1) });
                Add(new Post() { ID = 973813, TopicID = 11371, TopicIndex = 43147, Date = new DateTime(2014, 9, 1) });
                Add(new Post() { ID = 989246, TopicID = 11371, TopicIndex = 54536, Date = new DateTime(2014, 10, 1) });
                Add(new Post() { ID = 1004209, TopicID = 11371, TopicIndex = 66275, Date = new DateTime(2014, 11, 1) });
                Add(new Post() { ID = 1016646, TopicID = 11371, TopicIndex = 75422, Date = new DateTime(2014, 12, 1) });
                Add(new Post() { ID = 1036078, TopicID = 11371, TopicIndex = 86701, Date = new DateTime(2015, 1, 1) });
                Add(new Post() { ID = 1063961, TopicID = 11371, TopicIndex = 108719, Date = new DateTime(2015, 2, 1) });
                Add(new Post() { ID = 1079034, TopicID = 11371, TopicIndex = 121312, Date = new DateTime(2015, 3, 1) });
                Add(new Post() { ID = 1090150, TopicID = 11371, TopicIndex = 130062, Date = new DateTime(2015, 4, 1) });
                Add(new Post() { ID = 1094867, TopicID = 11371, TopicIndex = 134060, Date = new DateTime(2015, 4, 10) });
                Add(new Post() { ID = 1094942, TopicID = 13408, TopicIndex = 1, Date = new DateTime(2015, 4, 10) });
                Add(new Post() { ID = 1101882, TopicID = 13408, TopicIndex = 5835, Date = new DateTime(2015, 5, 1) });
                Add(new Post() { ID = 1109911, TopicID = 13408, TopicIndex = 12475, Date = new DateTime(2015, 6, 1) });
                Add(new Post() { ID = 1116139, TopicID = 13408, TopicIndex = 17386, Date = new DateTime(2015, 7, 1) });
                Add(new Post() { ID = 1130308, TopicID = 13408, TopicIndex = 28879, Date = new DateTime(2015, 8, 1) });
                Add(new Post() { ID = 1145394, TopicID = 13408, TopicIndex = 41810, Date = new DateTime(2015, 9, 1) });
                Add(new Post() { ID = 1152127, TopicID = 13408, TopicIndex = 47585, Date = new DateTime(2015, 10, 1) });
                Add(new Post() { ID = 1158425, TopicID = 13408, TopicIndex = 52971, Date = new DateTime(2015, 11, 1) });
                Add(new Post() { ID = 1164668, TopicID = 13408, TopicIndex = 58119, Date = new DateTime(2015, 12, 1) });
                Add(new Post() { ID = 1169757, TopicID = 13408, TopicIndex = 61836, Date = new DateTime(2016, 1, 1) });
                Add(new Post() { ID = 1173984, TopicID = 13408, TopicIndex = 64883, Date = new DateTime(2016, 2, 1) });
                Add(new Post() { ID = 1177698, TopicID = 13408, TopicIndex = 67430, Date = new DateTime(2016, 3, 1) });
                Add(new Post() { ID = 1191412, TopicID = 13408, TopicIndex = 79349, Date = new DateTime(2016, 4, 1) });
                Add(new Post() { ID = 1202086, TopicID = 13408, TopicIndex = 88946, Date = new DateTime(2016, 5, 1) });
                Add(new Post() { ID = 1208486, TopicID = 13408, TopicIndex = 94450, Date = new DateTime(2016, 6, 1) });
                Add(new Post() { ID = 1215008, TopicID = 13408, TopicIndex = 99636, Date = new DateTime(2016, 7, 1) });
                Add(new Post() { ID = 1235106, TopicID = 13408, TopicIndex = 118139, Date = new DateTime(2016, 8, 1) });
                Add(new Post() { ID = 1239049, TopicID = 13408, TopicIndex = 121930, Date = new DateTime(2016, 8, 3) });
                Add(new Post() { ID = 1239050, TopicID = 15361, TopicIndex = 1, Date = new DateTime(2016, 8, 3) });
                Add(new Post() { ID = 1266015, TopicID = 15361, TopicIndex = 25108, Date = new DateTime(2016, 9, 1) });
                Add(new Post() { ID = 1276411, TopicID = 15361, TopicIndex = 34968, Date = new DateTime(2016, 10, 1) });
                Add(new Post() { ID = 1281285, TopicID = 15361, TopicIndex = 39317, Date = new DateTime(2016, 11, 1) });
                Add(new Post() { ID = 1283146, TopicID = 15361, TopicIndex = 40687, Date = new DateTime(2016, 12, 1) });
                Add(new Post() { ID = 1287860, TopicID = 15361, TopicIndex = 44202, Date = new DateTime(2017, 1, 1) });
                Add(new Post() { ID = 1296431, TopicID = 15361, TopicIndex = 52009, Date = new DateTime(2017, 2, 1) });
                Add(new Post() { ID = 1306895, TopicID = 15361, TopicIndex = 61870, Date = new DateTime(2017, 3, 1) });
                Add(new Post() { ID = 1317647, TopicID = 15361, TopicIndex = 71955, Date = new DateTime(2017, 4, 1) });
                Add(new Post() { ID = 1323059, TopicID = 15361, TopicIndex = 76978, Date = new DateTime(2017, 5, 1) });
                Add(new Post() { ID = 1327463, TopicID = 15361, TopicIndex = 81044, Date = new DateTime(2017, 6, 1) });
                Add(new Post() { ID = 1336051, TopicID = 15361, TopicIndex = 89203, Date = new DateTime(2017, 7, 1) });
                Add(new Post() { ID = 1340855, TopicID = 15361, TopicIndex = 93441, Date = new DateTime(2017, 8, 1) });
                Add(new Post() { ID = 1347935, TopicID = 15361, TopicIndex = 99564, Date = new DateTime(2017, 9, 1) });
                Add(new Post() { ID = 1353460, TopicID = 15361, TopicIndex = 104444, Date = new DateTime(2017, 10, 1) });
                Add(new Post() { ID = 1361465, TopicID = 15361, TopicIndex = 111869, Date = new DateTime(2017, 11, 1) });
                Add(new Post() { ID = 1365016, TopicID = 15361, TopicIndex = 114934, Date = new DateTime(2017, 12, 1) });
                Add(new Post() { ID = 1367725, TopicID = 15361, TopicIndex = 116647, Date = new DateTime(2018, 1, 1) });
                Add(new Post() { ID = 1372087, TopicID = 15361, TopicIndex = 120120, Date = new DateTime(2018, 2, 1) });
                Add(new Post() { ID = 1374414, TopicID = 15361, TopicIndex = 121956, Date = new DateTime(2018, 3, 1) });
                Add(new Post() { ID = 1377002, TopicID = 15361, TopicIndex = 123869, Date = new DateTime(2018, 4, 1) });
                Add(new Post() { ID = 1379863, TopicID = 15361, TopicIndex = 126262, Date = new DateTime(2018, 5, 1) });
                Add(new Post() { ID = 1382429, TopicID = 15361, TopicIndex = 128435, Date = new DateTime(2018, 6, 1) });
                Add(new Post() { ID = 1386481, TopicID = 15361, TopicIndex = 131838, Date = new DateTime(2018, 7, 1) });
                Add(new Post() { ID = 1389035, TopicID = 15361, TopicIndex = 133708, Date = new DateTime(2018, 8, 1) });
                Add(new Post() { ID = 1390417, TopicID = 15361, TopicIndex = 134621, Date = new DateTime(2018, 9, 1) });
                Add(new Post() { ID = 1391153, TopicID = 15361, TopicIndex = 135015, Date = new DateTime(2018, 10, 1) });
                Add(new Post() { ID = 1392057, TopicID = 15361, TopicIndex = 135499, Date = new DateTime(2018, 11, 1) });
                Add(new Post() { ID = 1392898, TopicID = 15361, TopicIndex = 135970, Date = new DateTime(2018, 12, 1) });
                Add(new Post() { ID = 1393861, TopicID = 15361, TopicIndex = 136616, Date = new DateTime(2019, 1, 1) });
                Add(new Post() { ID = 1394634, TopicID = 15361, TopicIndex = 137016, Date = new DateTime(2019, 2, 1) });
                Add(new Post() { ID = 1395436, TopicID = 15361, TopicIndex = 137500, Date = new DateTime(2019, 3, 1) });
                Add(new Post() { ID = 1396547, TopicID = 15361, TopicIndex = 138331, Date = new DateTime(2019, 4, 1) });
                Add(new Post() { ID = 1398197, TopicID = 15361, TopicIndex = 139744, Date = new DateTime(2019, 5, 1) });
                Add(new Post() { ID = 1399727, TopicID = 15361, TopicIndex = 140956, Date = new DateTime(2019, 6, 1) });
                Add(new Post() { ID = 1402003, TopicID = 15361, TopicIndex = 142798, Date = new DateTime(2019, 7, 1) });
                Add(new Post() { ID = 1405274, TopicID = 15361, TopicIndex = 145592, Date = new DateTime(2019, 8, 1) });
                Add(new Post() { ID = 1406815, TopicID = 15361, TopicIndex = 146658, Date = new DateTime(2019, 9, 1) });
                Add(new Post() { ID = 1408192, TopicID = 15361, TopicIndex = 147795, Date = new DateTime(2019, 10, 1) });
                Add(new Post() { ID = 1409636, TopicID = 15361, TopicIndex = 148825, Date = new DateTime(2019, 11, 1) });
                Add(new Post() { ID = 1410923, TopicID = 15361, TopicIndex = 149661, Date = new DateTime(2019, 12, 1) });
                Add(new Post() { ID = 1412736, TopicID = 15361, TopicIndex = 151208, Date = new DateTime(2020, 1, 1) });
                Add(new Post() { ID = 1415065, TopicID = 15361, TopicIndex = 153151, Date = new DateTime(2020, 2, 1) });
                Add(new Post() { ID = 1416111, TopicID = 15361, TopicIndex = 153794, Date = new DateTime(2020, 3, 1) });
                Add(new Post() { ID = 1417362, TopicID = 15361, TopicIndex = 154567, Date = new DateTime(2020, 4, 1) });
                Add(new Post() { ID = 1418961, TopicID = 15361, TopicIndex = 155411, Date = new DateTime(2020, 5, 1) });
                Add(new Post() { ID = 1426235, TopicID = 15361, TopicIndex = 161680, Date = new DateTime(2020, 6, 1) });
                Add(new Post() { ID = 1432560, TopicID = 15361, TopicIndex = 167240, Date = new DateTime(2020, 7, 1) });
                Add(new Post() { ID = 1436065, TopicID = 15361, TopicIndex = 170116, Date = new DateTime(2020, 8, 1) });
                Add(new Post() { ID = 1438888, TopicID = 15361, TopicIndex = 172343, Date = new DateTime(2020, 9, 1) });
                Add(new Post() { ID = 1439986, TopicID = 15361, TopicIndex = 173095, Date = new DateTime(2020, 10, 1) });
                Add(new Post() { ID = 1440952, TopicID = 15361, TopicIndex = 173695, Date = new DateTime(2020, 11, 1) });
                Add(new Post() { ID = 1442093, TopicID = 15361, TopicIndex = 174441, Date = new DateTime(2020, 12, 1) });
                Add(new Post() { ID = 1443241, TopicID = 15361, TopicIndex = 175260, Date = new DateTime(2021, 1, 1) });
                Add(new Post() { ID = 1444863, TopicID = 15361, TopicIndex = 176499, Date = new DateTime(2021, 2, 1) });
                Add(new Post() { ID = 1445887, TopicID = 15361, TopicIndex = 177167, Date = new DateTime(2021, 3, 1) });
                Add(new Post() { ID = 1448341, TopicID = 15361, TopicIndex = 178819, Date = new DateTime(2021, 4, 1) });
                Add(new Post() { ID = 1450910, TopicID = 15361, TopicIndex = 180772, Date = new DateTime(2021, 5, 1) });
                Add(new Post() { ID = 1453470, TopicID = 15361, TopicIndex = 182857, Date = new DateTime(2021, 6, 1) });
                Add(new Post() { ID = 1455267, TopicID = 15361, TopicIndex = 184201, Date = new DateTime(2021, 7, 1) });
                Add(new Post() { ID = 1456509, TopicID = 15361, TopicIndex = 185060, Date = new DateTime(2021, 8, 1) });

            }
        }
    }
}
