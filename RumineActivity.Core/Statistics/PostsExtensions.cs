using RumineActivity.Core.Measures;
using RumineActivity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public static class PostsExtensions
    {
        /// <summary>
        /// Рассчитывает граничные посты для
        /// </summary>
        /// <param name="posts">ОТСОРТИРОВАННЫЕ по ID посты</param>
        /// <returns></returns>
        public static DateRangePostBorders GetBorders(this IEnumerable<Post> posts, DateRange dateRange)
        {
            var firstInnerPost = posts.FirstOrDefault(p => dateRange.IsPostInside(p));
            var lastInnerPost = posts.LastOrDefault(p => dateRange.IsPostInside(p));
            int firstInnerPostIndex = -1;
            int lastInnerPostIndex = -1;
            for (int i = 0; i < posts.Count(); i++)
            {
                if(firstInnerPost == posts.ElementAt(i))
                {
                    firstInnerPostIndex = i;
                }
                if (lastInnerPost == posts.ElementAt(i))
                {
                    lastInnerPostIndex = i;
                    break;
                }
            }

            var firstOuterPost = posts.ElementAtOrDefault(firstInnerPostIndex - 1);
            var lastOuterPost = posts.ElementAtOrDefault(lastInnerPostIndex + 1);

            var bordersInner = new PostRange(firstInnerPost, lastInnerPost);
            var bordersOuter = new PostRange(firstOuterPost, lastOuterPost);

            return new DateRangePostBorders()
            {
                DateRange = dateRange,
                PostInnerBorders = bordersInner,
                PostOuterBorders = bordersOuter
            };
        } 



    }
}
