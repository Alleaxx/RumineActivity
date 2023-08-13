using RumineActivity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public class PostRange
    {
        //Сами посты
        public Post PostX { get; init; }
        public Post PostY { get; init; }

        //Валидность
        public bool IsCorrect => AllKnown && !Same;
        public bool AllKnown => PostX != null && PostY != null;
        public bool Same => PostX == PostY;


        //Рассчитанные параметры
        public int DifferenceID { get; init; }

        private int Difference { get; init; }


        public PostRange(Post postX, Post postY, int difference = 1)
        {
            PostX = postX;
            PostY = postY;

            if (postX == null || postY == null)
            {
                DifferenceID = -1;
                return;
            }

            if (postX.Id > PostY.Id)
            {
                DifferenceID = postX.Id - postY.Id;
            }
            else
            {
                DifferenceID = postY.Id - postX.Id;
            }
            Difference = difference;
        }


        public int GetWrittenPosts()
        {
            return IsCorrect ? DifferenceID + Difference : 0;
        }
    }
}
