using RumineActivity.Core.Measures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    //граничные посты внутри периода
    //разница ID - "не менее" этого количества было написано постов в периоде
    //граничные посты вне периода
    //разница ID - "не более" этого количества было написано постов в периоде
    public class DateRangePostBorders
    {
        public DateRange DateRange { get; set; }
        public PostRange PostInnerBorders { get; set; }
        public PostRange PostOuterBorders { get; set; }

        public bool IsAllCorrect => PostInnerBorders.IsCorrect && PostOuterBorders.IsCorrect;

        //Границы определены точно, ни одного сообщения не пропущено
        public bool IsAccurate => GetMissedBeforeFirst() == 0 && GetMissedAfterLast() == 0;
        public AccuracyRate Accuracy => AccuracyRate.GetAccuracyFor(GetMissedBeforeFirst() + GetMissedAfterLast());


        //Показатель "пропущено до первого поста"
        public int GetMissedBeforeFirst()
        {
            if (!IsAllCorrect)
            {
                return -1;
            }

            return PostInnerBorders.PostX.Id - PostOuterBorders.PostX.Id - 1;
        }
        //Показатель "пропущено после последнего"
        public int GetMissedAfterLast()
        {
            if (!IsAllCorrect)
            {
                return -1;
            }

            return PostOuterBorders.PostY.Id - PostInnerBorders.PostY.Id - 1;
        }
    }
}
