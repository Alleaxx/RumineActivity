using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public enum DViewTypes
    {
        Values, Gistogramm, Graphic, GraphicAdv, Table
    }

    public class ViewType : EnumType<DViewTypes>
    {
        public string Icon { get; set; }
        public ViewType(DViewTypes diagram) : base(diagram)
        {
            switch (diagram)
            {
                case DViewTypes.Gistogramm:
                    Name = "Гистограмма";
                    Icon = "bar-chart-2.svg";
                    break;
                case DViewTypes.Graphic:
                    Name = "График";
                    Icon = "trending-up.svg";
                    break;
                case DViewTypes.GraphicAdv:
                    Name = "График с заливкой";
                    Icon = "trending-up.svg";
                    break;
                case DViewTypes.Values:
                    Name = "Список значений";
                    Icon = "list.svg";
                    break;
                case DViewTypes.Table:
                    Name = "Таблица";
                    Icon = "grid.svg";
                    break;
            }
        }
    }
}
