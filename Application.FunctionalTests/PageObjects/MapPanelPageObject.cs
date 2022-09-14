using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FunctionalTests.PageObjects
{
    public class MapPanelPageObject : PagesPageObjectBase
    {
        public MapPanelPageObject(IAutomateBrowser browser) : base(browser)
        {
        }

        private IDomElement MapPanel => RightContainer.ElementFinder.WithClass("map-panel").FindFirstOrDefault();

        private IDomElement MapLoadingItem => MapPanel.ElementFinder.WithTagName("cui-loader").FindFirstOrDefault();

        private bool IsMapLoadingItemDisplayed() => MapLoadingItem != null ? MapLoadingItem.Displayed : false;

        private void WaitForMapLoadingItemToAppear() => Browser.Wait(TimeSpan.FromSeconds(15), x => IsMapLoadingItemDisplayed());

        private void WaitForMapLoadingItemDisappear() => Browser.Wait(TimeSpan.FromSeconds(10), x => !IsMapLoadingItemDisplayed());

        public void WaitForMapLoaded()
        {
            try
            {
                WaitForMapLoadingItemToAppear();
                WaitForMapLoadingItemDisappear();
            }
            catch
            {
                WaitForMapLoadingItemDisappear();
            }
        }
    }
}
