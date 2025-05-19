using RumineActivity.Core.API;

namespace RumineActivity.View.Services
{
    public interface IResourcesService
    {
        public IJsonFileInfo ForumPostsFile { get; }
        public IJsonFileInfo RageStatisticsFile { get; }
    }
    public class ResourcesService : IResourcesService
    {
        public IJsonFileInfo ForumPostsFile { get; init; }
        public IJsonFileInfo RageStatisticsFile { get; init; }


        public ResourcesService()
        {
            ForumPostsFile = JsonFileInfo.FromObjectJson("data/ForumPostsV4.json", true);
            RageStatisticsFile = JsonFileInfo.FromObjectJson("data/RageStatisticsV1.json", true);
        }
    }
}
