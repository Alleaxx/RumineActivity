using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public enum DataViewTypes
    {
        Values, Gistogramm, Graphic, GraphicAdv, Table
    }

    public class DataViewType : EnumType<DataViewTypes>
    {
        public string Icon { get; set; }
        public DataViewType(DataViewTypes diagram) : base(diagram)
        {
            switch (diagram)
            {
                case DataViewTypes.Gistogramm:
                    Name = "Гистограмма";
                    Icon = "bar-chart-2.svg";
                    break;
                case DataViewTypes.Graphic:
                    Name = "График";
                    Icon = "trending-up.svg";
                    break;
                case DataViewTypes.GraphicAdv:
                    Name = "График с заливкой";
                    Icon = "trending-up.svg";
                    break;
                case DataViewTypes.Values:
                    Name = "Список значений";
                    Icon = "list.svg";
                    break;
                case DataViewTypes.Table:
                    Name = "Таблица";
                    Icon = "grid.svg";
                    break;
            }
        }

        public static DataViewType[] AllValues => Enum.GetValues<DataViewTypes>().Select(d => new DataViewType(d)).ToArray();
    }

}
