using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RumineActivity.Core;
using RumineActivity.Core.API;

namespace RumineActivity.View
{
    public class StatComponent : ComponentBase, IAsyncDisposable
    {
        [Inject]
        public IStatApp App { get; set; }
        [Inject]
        public IReportsFactory ReportsFactory { get; set; }
        [Inject]
        public IActivityApi API { get; set; }

        public ValuesViewConfig ValuesViewConfig => App.ValuesViewConfig;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            App.OnLoadEnded += App_OnLoadEnded;
            if(!App.IsLoaded.HasValue)
            {
                await App.InitReports();
            }
        }

        private void App_OnLoadEnded()
        {
            StateHasChanged();
        }
        public virtual async ValueTask DisposeAsync()
        {

        }

        #region Вспомогательные методы CSS

        const string SelectedClass = "selected";
        const string SelectableClass = "selectable";
        protected string CssIsSelected<T>(T a, T to)
        {
            return a.Equals(to) ? SelectedClass : SelectableClass;
        }
        protected string CssIsNull<T>(T a, string classNull, string classNotNull = "")
        {
            return a == null ? classNull : classNotNull;
        }
        protected string CssIsEqual<T>(T a, T b, string classEqual, string classNonEqual = "")
        {
            if(a == null && b == null)
            {
                return classEqual;
            }
            else if((a == null && b != null) || (a != null && b == null))
            {
                return classNonEqual;
            }

            return a.Equals(b) ? classEqual : classNonEqual;
        }
        protected string CssIsTrue(bool a, string classTrue, string classFalse = "")
        {
            return a ? classTrue : classFalse;
        }
        protected string CssIsFalse(bool a, string classFalse, string classTrue = "")
        {
            return !a ? classFalse : classTrue;
        }
        #endregion
    }
}
