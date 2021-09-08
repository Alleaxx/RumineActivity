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
        public DataViewType(DataViewTypes diagram) : base(diagram)
        {
            switch (diagram)
            {
                case DataViewTypes.Gistogramm:
                    Name = "Гистограмма";
                    break;
                case DataViewTypes.Graphic:
                    Name = "График";
                    break;
                case DataViewTypes.GraphicAdv:
                    Name = "График с заливкой";
                    break;
                case DataViewTypes.Values:
                    Name = "Список значений";
                    break;
                case DataViewTypes.Table:
                    Name = "Таблица";
                    break;
            }
        }

        public static DataViewType[] AllValues => Enum.GetValues<DataViewTypes>().Select(d => new DataViewType(d)).ToArray();
    }

}
