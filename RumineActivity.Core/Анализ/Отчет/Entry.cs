using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public enum EntrySources
    {
        AllForum, Topics, Difference
    }

    //Для записи с 'к моменту (ДАТА) написано сообщений следует придумать нечто другое'

    //Запись 'За период написано сообщений'
    public class Entry : Named
    {
        public override string ToString() => $"{Name} | {PostsWritten}";

        public DateRange Range { get; protected set; }
        public int Index { get; protected set; }


        //Разделение дат в интерфейсе
        public bool SeparateDates { get; protected set; } = false;
        public string DateFormat { get; private set; } = "dd MMMM yyyy";



        //Периодический отчет
        public Entry(int index, DateRange range, ReportCreatorOptions options)
        {
            Index = index;
            SeparateDates = false;
            Range = range;
            SetFormat(options.Period.DateFormat);

            SetSourceMode(options.PostSource.Mode);
        }

        //Фактический отчет
        public Entry(int index, Post newer, Post older, ReportCreatorOptions options)
        {
            Index = index;
            SeparateDates = true;
            Range = new DateRange(newer, older);
            SetFormat(options.Period.DateFormat);

            SetPostsDiff(newer, older);
            SetSourceMode(options.PostSource.Mode);
            SetLastPost(newer);
        }



        //Редактирование записи
        protected void SetPostsDiff(Post newer, Post older)
        {
            double postsDifferenceAll = newer.Id - older.Id;
            double postsDifferenceTopic = Math.Max(0, newer.TopicIndex - older.TopicIndex);

            Posts[EntrySources.AllForum] = postsDifferenceAll;
            Posts[EntrySources.Topics] = postsDifferenceTopic;
            Posts[EntrySources.Difference] = Math.Abs(postsDifferenceAll - postsDifferenceTopic);
        }
        public void SetLastPost(Post ending)
        {
            EndingPost = ending;
        }
        protected void SetSourceMode(PostSources mode)
        {
            Dictionary<PostSources, EntrySources> Translate = new Dictionary<PostSources, EntrySources>()
            {
                [PostSources.All] = EntrySources.AllForum,
                [PostSources.NotChat] = EntrySources.Difference,
                [PostSources.Topic] = EntrySources.Topics,
                [PostSources.OnlyChat] = EntrySources.Topics
            };
            EntrySource = Translate[mode];
        }
        protected void SetFormat(string dateFormat)
        {
            DateFormat = dateFormat;
            if (!SeparateDates)
            {
                Name = Range.From.ToString(dateFormat);
            }
            else
            {
                Name = Range.ToString(dateFormat);
            }
        }



        private EntrySources EntrySource = EntrySources.AllForum;

        //Суммарно написано к моменту
        private Post EndingPost { get; set; }
        public int LastPostIndex => EntrySource == EntrySources.Topics ? EndingPost.TopicIndex : EndingPost.Id;
        
        //Разница в постах
        private Dictionary<EntrySources, double> Posts { get; set; } = new Dictionary<EntrySources, double>
        {
            [EntrySources.AllForum] = 1,
            [EntrySources.Topics] = 1,
            [EntrySources.Difference] = 1,
        };
        public void Set(double val)
        {
            Posts[EntrySource] = val;
        }
        public double PostsWritten => GetPosts(EntrySource, MeasureMethods.Total, MeasureUnits.Messages);
        public double PostsAverage => GetPosts(EntrySource, MeasureMethods.ByDay, MeasureUnits.Messages);
        public double GetPosts(MeasureMethods methods, MeasureUnits units)
        {
            return GetPosts(EntrySource, methods, units);
        }
        private double GetPosts(EntrySources source, MeasureMethods interval, MeasureUnits unit)
        {
            double val = Posts[source];
            val = MeasureMethod.Create(interval).GetValue(val, Range);
            val = new MeasureUnit(unit).GetValue(val);
            return val;
        }
    }
}
