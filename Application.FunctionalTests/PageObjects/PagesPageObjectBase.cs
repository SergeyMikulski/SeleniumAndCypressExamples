using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FunctionalTests.PageObjects
{
    [RelativePageUrl("/map")]
    public class PagesPageObjectBase : PageObjectBase
    {

        public PagesPageObjectBase(IAutomateBrowser browser) : base(browser)
        {
            Browser.WaitForAjax();
            WaitPageBodyContainer();
            WaitLeftContainer();
            WaitRightContainer();
            WaitMaskDisappear();
        }

        private IDomElement PageHeaderContainer => PageContainer.ElementFinder.WithTagName("header").FindFirstOrDefault();

        private IDomElement PageHeaderName => PageHeaderContainer.ElementFinder.WithTagName("cui-page-title").FindFirstOrDefault();

        public string PageHeaderNameText => PageHeaderName.Text;

        private ISearchForElements PageContainerSearchable => Browser.ElementFinder.WithTagName("cui-page-template");

        private IDomElement PageContainer => PageContainerSearchable.FindFirstOrDefault();

        private ISearchForElements PageBodyContainerSearchable => PageContainer.ElementFinder.WithTagName("cui-content");

        private void WaitPageBodyContainer() => Browser.WaitForElement(TimeSpan.FromSeconds(20), PageBodyContainerSearchable);

        private IDomElement PageBodyContainer => PageBodyContainerSearchable.FindFirstOrDefault();

        private ISearchForElements LeftContainerSearchable => PageBodyContainer.ElementFinder.WithClass("left-panel");

        private void WaitLeftContainer() => Browser.WaitForElement(TimeSpan.FromSeconds(20), LeftContainerSearchable);

        protected IDomElement LeftContainer => LeftContainerSearchable.FindFirstOrDefault();

        private ISearchForElements RightContainerSearchable => PageBodyContainer.ElementFinder.WithClass("right-panel");

        private void WaitRightContainer() => Browser.WaitForElement(TimeSpan.FromSeconds(20), RightContainerSearchable);

        protected IDomElement RightContainer => RightContainerSearchable.FindFirstOrDefault();

        protected IEnumerable<IDomElement> MaskList => PageBodyContainer.ElementFinder.WithTagName("cui-loader").Find();

        public void WaitMaskDisappear() => Browser.Wait(TimeSpan.FromSeconds(30), x => MaskList.Count() == 0);

        public TimeSliderCalendarPageObject TimeSliderCalendarPageObject { get; }

        public ResultsTablePageObject ResultsTablePageObject { get; }

        public MapPanelPageObject MapPanelPageObject { get; }

        public ManagementPanelPageObject ManagementPanelPageObject { get; }

        public ExportPageObject ExportPageObject { get; }
    }
}
