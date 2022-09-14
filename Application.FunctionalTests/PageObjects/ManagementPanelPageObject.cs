using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.FunctionalTests.Tests;

namespace Application.FunctionalTests.PageObjects
{
    public class ManagementPanelPageObject : PagesPageObjectBase
    {
        public ManagementPanelPageObject(IAutomateBrowser browser) : base(browser)
        {
            WaitSupplierContainer();
            WaitImporterContainer();
        }

        public ManagementPanelQueryTabTests ManagementPanelQueryTabTests { get; }

        #region Common

        private IDomElement ManagementPanelTabs => LeftContainer.ElementFinder.WithTagName("cui-tabs-nav").FindFirstOrDefault();

        public void ChooseManagementPanelTab(string tabName)
        {
            Browser.WaitForAjax();
            WaitMaskDisappear();
            var tabToBeChosen = ManagementPanelTabs.ElementFinder.WithText(tabName).FindFirstOrDefault();
            tabToBeChosen.Click();
            Browser.WaitForAjax();
        }

        private ISearchForElements ManagementPanelBoardSearchable => LeftContainer.ElementFinder.WithClass("tab-content");

        private IDomElement ManagementPanelBoard => ManagementPanelBoardSearchable.FindFirstOrDefault();

        
        #endregion

        #region QueryTab

        private ISearchForElements SpotCargoLabelSearch => ManagementPanelBoard.ElementFinder.WithAttributeValue("for", "spot-cargo");

        private IDomElement SpotCargoContainer
        {
            get
            {
                Browser.WaitForElement(TimeSpan.FromSeconds(3), SpotCargoLabelSearch);
                return SpotCargoLabelSearch.FindFirstOrDefault().Parent;
            }
        }

        public string SpotCargoLabelText => SpotCargoContainer?.ElementFinder.WithTagName("label").FindFirstOrDefault().Text;

        private IDomElement SpotCargoRadiobuttonsContainer => SpotCargoContainer.ElementFinder.WithTagName("cui-group").FindFirstOrDefault();

        public bool IsSpotCargoRadiobuttonYesChecked()
        {
            var spotCargoRadiobuttonYES = SpotCargoRadiobuttonsContainer.ElementFinder.WithTagName("cui-radio").WithAttributeValue("label", "Yes").FindFirstOrDefault();

            return spotCargoRadiobuttonYES.ElementFinder.WithClass("checked").FindFirstOrDefault() != null;
        }

        public void SpotCargoRadiobuttonCheck(string radiobuttonName)
        {
            var spotCargoRadiobutton = SpotCargoRadiobuttonsContainer.ElementFinder.WithTagName("cui-radio").WithAttributeValue("label", radiobuttonName).FindFirstOrDefault();
            spotCargoRadiobutton.Click();
        }

        private ISearchForElements ReExportsLabelSearch => ManagementPanelBoard.ElementFinder.WithAttributeValue("for", "re-export");

        private IDomElement ReExportsContainer
        {
            get
            {
                Browser.WaitForElement(TimeSpan.FromSeconds(3), ReExportsLabelSearch);
                return ReExportsLabelSearch.FindFirstOrDefault().Parent;
            }
        }

        public string ReExportsLabelText => ReExportsContainer?.ElementFinder.WithTagName("label").FindFirstOrDefault().Text;

        private IDomElement ReExportsRadiobuttonsContainer => ReExportsContainer.ElementFinder.WithTagName("cui-group").FindFirstOrDefault();

        public bool IsReExportsRadiobuttonIncludedChecked()
        {
            var reExportsRadiobuttonIncluded = ReExportsRadiobuttonsContainer.ElementFinder.WithTagName("cui-radio").WithPartialAttributeValue("label", "Included").FindFirstOrDefault();

            return reExportsRadiobuttonIncluded.ElementFinder.WithClass("checked").FindFirstOrDefault() != null;
        }

        public void ReExportsRadiobuttonCheck(string radiobuttonName)
        {
            var reExportsRadiobutton = ReExportsRadiobuttonsContainer.ElementFinder.WithTagName("cui-radio").WithAttributeValue("label", radiobuttonName).FindFirstOrDefault();
            reExportsRadiobutton.Click();
        }

        private ISearchForElements DisplayUnitsLabelSearch => ManagementPanelBoard.ElementFinder.WithAttributeValue("for", "display-units");

        private IDomElement DisplayUnitsContainer
        {
            get
            {
                Browser.WaitForElement(TimeSpan.FromSeconds(3), DisplayUnitsLabelSearch);
                return DisplayUnitsLabelSearch.FindFirstOrDefault().Parent;
            }
        }

        public string DisplayUnitsLabelText => DisplayUnitsContainer?.ElementFinder.WithTagName("label").FindFirstOrDefault().Text;

        private IDomElement DisplayUnitsDropDownField => DisplayUnitsContainer.ElementFinder.WithClass("dropdown").FindFirstOrDefault();

        public string DisplayUnitsDropDownItemSelected => DisplayUnitsDropDownField.Text;

        private IDomElement DisplayUnitsDropDownMenu => DisplayUnitsDropDownField.ElementFinder.WithClass("dropdown-menu").FindFirstOrDefault();

        private IDomElement DisplayUnitsDropDownMenuItems => DisplayUnitsDropDownMenu.ElementFinder.WithTagName("ul").FindFirstOrDefault();

        public void DisplayUnitsDropDownMenuItemSelect(string itemToBeSelected)
        {
            var menuItemToBeselected = DisplayUnitsDropDownMenuItems.ElementFinder.WithText(itemToBeSelected).FindFirstOrDefault();
            menuItemToBeselected.Click();
        }

        private ISearchForElements LoadedUnloadedLabelSearch => ManagementPanelBoard.ElementFinder.WithAttributeValue("for", "loaded-unloaded");

        private IDomElement LoadedUnloadedContainer
        {
            get
            {
                Browser.WaitForElement(TimeSpan.FromSeconds(3), LoadedUnloadedLabelSearch);
                return LoadedUnloadedLabelSearch.FindFirstOrDefault().Parent;
            }
        }

        public string LoadedUnloadedLabelText => LoadedUnloadedContainer?.ElementFinder.WithTagName("label").FindFirstOrDefault().Text;

        private IDomElement LoadedUnloadedRadiobuttonsContainer => LoadedUnloadedContainer.ElementFinder.WithTagName("cui-group").FindFirstOrDefault();

        public bool IsLoadedUnloadedRadiobuttonLoadedChecked()
        {
            var loadedUnloadedRadiobuttonLoaded = LoadedUnloadedRadiobuttonsContainer.ElementFinder.WithTagName("cui-radio").WithAttributeValue("label", "Loaded").FindFirstOrDefault();
            var value = loadedUnloadedRadiobuttonLoaded["ng-reflect-value"];
            var model = loadedUnloadedRadiobuttonLoaded["ng-reflect-model"];

            return value == model;
        }

        public void LoadedUnloadedRadiobuttonCheck(string radiobuttonName)
        {
            var loadedUnloadedRadiobutton = LoadedUnloadedRadiobuttonsContainer.ElementFinder.WithTagName("cui-radio").WithAttributeValue("label", radiobuttonName).FindFirstOrDefault();
            loadedUnloadedRadiobutton.Click();
        }

        enum CheckBoxCheckType
        {
            Checked,
            PartiallyChecked,
            UnChecked
        }

        #region SupplierArea
        private ISearchForElements SupplierSearch => ManagementPanelBoard.ElementFinder.WithTagName("country-facility-selector");

        private void WaitSupplierContainer() => Browser.WaitForElement(TimeSpan.FromSeconds(15), SupplierSearch);

        private IDomElement SupplierContainer
        {
            get
            {
                WaitSupplierContainer();
                return SupplierSearch.FindFirstOrDefault();
            }
        }

        private IDomElement SupplierLabel => SupplierContainer.ElementFinder.WithTagName("strong").WithClass("line-height-lg").FindFirstOrDefault();

        public string SupplierLabelText => SupplierLabel?.Text;

        private IDomElement SupplierCountryFacilityToggle => SupplierContainer.ElementFinder.WithClass("btn-group").FindFirstOrDefault();

        public bool IsSupplierToggleCountryButtonChosen()
        {
            var supplierCountryToggleButton = SupplierCountryFacilityToggle?.ElementFinder.WithTagName("label").WithClass("active").FindFirstOrDefault();

            return supplierCountryToggleButton != null;
        }

        public void ClickSupplierToggleButton(string buttonToBeClicked)
        {
            MapPanelPageObject map = new MapPanelPageObject(Browser);

            var supplierCountryToggleButton = SupplierCountryFacilityToggle?.ElementFinder.WithText(buttonToBeClicked).FindFirstOrDefault();
            supplierCountryToggleButton.Click();
            Browser.WaitForAjax();
            map.WaitForMapLoaded();
        }

        private IDomElement SupplierSearchBox => SupplierContainer.ElementFinder.WithClass("search-box").FindFirstOrDefault();

        private IDomElement SupplierSearchBoxInputArea => SupplierSearchBox.ElementFinder.WithTagName("input").FindFirstOrDefault();

        private IDomElement SupplierSearchBoxRemoveButton => SupplierSearchBox.ElementFinder.WithClass("remove").FindFirstOrDefault();

        public string SupplierSearchBoxText
        {
            get => SupplierSearchBoxInputArea.Text;
            set => SupplierSearchBoxInputArea.Text = value;
        }

        public bool IsSupplierSearchBoxRemoveButtonDisplayed()
        {
            return !SupplierSearchBoxRemoveButton.CssClasses.Contains("hidden");
        }

        public void SupplierSearchBoxRemoveButtonClick()
        {
            SupplierSearchBoxRemoveButton.Click();
        }

        private IDomElement SupplierSearchBoxSearchButton => SupplierSearchBox.ElementFinder.WithClass("search").FindFirstOrDefault();

        public bool IsSupplierSearchBoxSearchButtonDisplayed()
        {
            return !SupplierSearchBoxSearchButton.CssClasses.Contains("hidden");
        }

        private IDomElement SupplierListBox => SupplierContainer.ElementFinder.WithClass("scroll-area-container").FindFirstOrDefault();


        private IDomElement SupplierCountryListboxArea => SupplierListBox.ElementFinder.WithTagName("country-multiselect-view").FindFirstOrDefault();

        private IDomElement SupplierCountrySelectAllElement => SupplierCountryListboxArea.ElementFinder.WithClass("select-all").FindFirstOrDefault();

        private IDomElement SupplierCountrySelectAllLabelToClick => SupplierCountrySelectAllElement.ElementFinder.WithTagName("cui-checkbox").FindFirstOrDefault();

        private ISearchForElements SupplierCountrySelectAllElementChecked => SupplierCountrySelectAllElement.ElementFinder.WithTagName("span").WithClass("checked");

        private bool IsSupplierCountrySelectAllElementChecked => SupplierCountrySelectAllElementChecked.FindFirstOrDefault() != null;

        private bool IsSupplierCountrySelectAllElementPartiallyChecked => SupplierCountrySelectAllElement.Parent.ElementFinder.WithClass("indeterminate").FindFirstOrDefault() != null;

        private CheckBoxCheckType GetSupplierCountrySelectAllButtonCheckType()
        {
            if (IsSupplierCountrySelectAllElementChecked)
                return CheckBoxCheckType.Checked;
            if (IsSupplierCountrySelectAllElementPartiallyChecked)
                return CheckBoxCheckType.PartiallyChecked;
            return CheckBoxCheckType.UnChecked;
        }

        public void UnCheckAllListItemsInSupplierCountry()
        {
            var checkBoxSelectionType = GetSupplierCountrySelectAllButtonCheckType();

            MapPanelPageObject map = new MapPanelPageObject(Browser);
            if (checkBoxSelectionType == CheckBoxCheckType.Checked)
            {
                SupplierCountrySelectAllLabelToClick.Click();
                Browser.WaitForAjax();
                map.WaitForMapLoaded();
            }
            if (checkBoxSelectionType == CheckBoxCheckType.PartiallyChecked)
            {
                SupplierCountrySelectAllLabelToClick.Click();
                Browser.WaitForElement(TimeSpan.FromSeconds(2), SupplierCountrySelectAllElementChecked);
                SupplierCountrySelectAllLabelToClick.Click();
                Browser.WaitForAjax();
                map.WaitForMapLoaded();
            }
        }

        public void SupplierCountrySelectAllLabelClick()
        {
            var checkBoxSelectionType = GetSupplierCountrySelectAllButtonCheckType();
            if (checkBoxSelectionType != CheckBoxCheckType.Checked)
            {
                MapPanelPageObject map = new MapPanelPageObject(Browser);
                SupplierCountrySelectAllLabelToClick.Click();
                Browser.WaitForAjax();
                map.WaitForMapLoaded();
            }
        }


        private IDomElement SupplierCountryListboxItems => SupplierCountryListboxArea.ElementFinder.WithTagName("ul").FindFirstOrDefault();

        private IEnumerable<IDomElement> SupplierCountryListboxItemsDisplayedList => SupplierCountryListboxItems.ElementFinder.WithTagName("li").Find().Where(x => x.Displayed);

        public List<string> SupplierCountryListboxVisibleItemsNamesList()
        {
            List<string> itemsNames = new List<string>();
            foreach (var item in SupplierCountryListboxItemsDisplayedList)
            {
                itemsNames.Add(item.Text);
            }

            return itemsNames;
        }

        public bool IsSupplierCountryListboxContainVisibleItem(string itemName)
        {
            return SupplierCountryListboxVisibleItemsNamesList().Contains(itemName);
        }

        public void SupplierCountryListboxChooseFirstAvailableCountry()
        {
            MapPanelPageObject map = new MapPanelPageObject(Browser);
            var itemInListToChoose = SupplierCountryListboxItemsDisplayedList.FirstOrDefault();
            var itemToClick = itemInListToChoose.ElementFinder.WithTagName("label").FindFirstOrDefault();
            itemToClick.Click();
            Browser.WaitForAjax();
            map.WaitForMapLoaded();
        }





        private IDomElement SupplierFacilityListboxArea => SupplierListBox.ElementFinder.WithTagName("facility-multiselect-view").FindFirstOrDefault();

        private IDomElement SupplierFacilitySelectAllElement => SupplierFacilityListboxArea.ElementFinder.WithClass("select-all").FindFirstOrDefault();

        private IDomElement SupplierFacilitySelectAllLabelToClick => SupplierFacilitySelectAllElement.ElementFinder.WithTagName("label").FindFirstOrDefault();

        private ISearchForElements SupplierFacilitySelectAllElementChecked => SupplierFacilitySelectAllElement.ElementFinder.WithTagName("span").WithClass("checked");

        private bool IsSupplierFacilitySelectAllElementChecked => SupplierFacilitySelectAllElementChecked.FindFirstOrDefault() != null;

        private bool IsSupplierFacilitySelectAllElementPartiallyChecked => SupplierFacilitySelectAllElement.Parent.ElementFinder.WithClass("indeterminate").FindFirstOrDefault() != null;

        private CheckBoxCheckType GetSupplierFacilitySelectAllButtonCheckType()
        {
            if (IsSupplierFacilitySelectAllElementChecked)
                return CheckBoxCheckType.Checked;
            if (IsSupplierFacilitySelectAllElementPartiallyChecked)
                return CheckBoxCheckType.PartiallyChecked;
            return CheckBoxCheckType.UnChecked;
        }

        public void UnCheckAllListItemsInSupplierFacility()
        {
            var checkBoxSelectionType = GetSupplierFacilitySelectAllButtonCheckType();

            MapPanelPageObject map = new MapPanelPageObject(Browser);
            if (checkBoxSelectionType == CheckBoxCheckType.Checked)
            {
                SupplierFacilitySelectAllLabelToClick.Click();
                Browser.WaitForAjax();
                map.WaitForMapLoaded();
            }
            if ((checkBoxSelectionType == CheckBoxCheckType.PartiallyChecked) || (checkBoxSelectionType == CheckBoxCheckType.UnChecked))
            {
                SupplierFacilitySelectAllLabelToClick.Click();
                Browser.WaitForElement(TimeSpan.FromSeconds(2), SupplierFacilitySelectAllElementChecked);
                SupplierFacilitySelectAllLabelToClick.Click();
                Browser.WaitForAjax();
                map.WaitForMapLoaded();
            }
        }


        private IDomElement SupplierFacilityListboxItems => SupplierFacilityListboxArea.ElementFinder.WithTagName("ul").FindFirstOrDefault();

        private IEnumerable<IDomElement> SupplierFacilityListboxItemsDisplayedList => SupplierFacilityListboxItems.ElementFinder.WithTagName("li").Find().Where(x => x.Displayed);

        private IEnumerable<IDomElement> SupplierFacilityListboxChildItemsList => SupplierFacilityListboxItemsDisplayedList.Where(x => x.ElementFinder.WithClass("cui-icon-caret-down").FindFirstOrDefault() == null);

        public List<string> SupplierFacilityListboxVisibleItemsNamesList => SupplierFacilityListboxChildItemsList.Select(x => x.Text).ToList();

        public bool IsSupplierFacilityListboxContainVisibleItem(string itemName)
        {
            foreach (var item in SupplierFacilityListboxVisibleItemsNamesList)
            {
                if (item.Contains(itemName))
                    return true;
            }
            return false;
        }

        #endregion

        #region ImporterArea

        private ISearchForElements ImporterSearch => ManagementPanelBoard.ElementFinder.WithTagName("country-facility-selector");

        private void WaitImporterContainer() => Browser.WaitForElement(TimeSpan.FromSeconds(3), ImporterSearch);

        private IDomElement ImporterContainer
        {
            get
            {
                WaitImporterContainer();
                return ImporterSearch.Find().LastOrDefault();
            }
        }

        private IDomElement ImporterLabel => ImporterContainer.ElementFinder.WithTagName("strong").FindFirstOrDefault();

        public string ImporterLabelText => ImporterLabel?.Text;

        private IDomElement ImporterCountryFacilityToggle => ImporterContainer.ElementFinder.WithClass("btn-group").FindFirstOrDefault();

        public bool IsImporterToggleCountryButtonChosen()
        {
            var importerCountryToggleButton = ImporterCountryFacilityToggle?.ElementFinder.WithTagName("label").WithClass("active").FindFirstOrDefault();

            return importerCountryToggleButton != null;
        }

        public void ClickImporterToggleButton(string buttonToBeClicked)
        {
            MapPanelPageObject map = new MapPanelPageObject(Browser);

            var importerCountryToggleButton = ImporterCountryFacilityToggle?.ElementFinder.WithText(buttonToBeClicked).FindFirstOrDefault();
            importerCountryToggleButton.Click();
            Browser.WaitForAjax();
            map.WaitForMapLoaded();
        }

        private IDomElement ImporterSearchBox => ImporterContainer.ElementFinder.WithClass("search-box").FindFirstOrDefault();

        private IDomElement ImporterSearchBoxInputArea => ImporterSearchBox.ElementFinder.WithTagName("input").FindFirstOrDefault();

        private IDomElement ImporterSearchBoxRemoveButton => ImporterSearchBox.ElementFinder.WithClass("remove").FindFirstOrDefault();

        public string ImporterSearchBoxText
        {
            get => ImporterSearchBoxInputArea.Text;
            set => ImporterSearchBoxInputArea.Text = value;
        }

        public bool IsImporterSearchBoxRemoveButtonDisplayed()
        {
            return !ImporterSearchBoxRemoveButton.CssClasses.Contains("hidden");
        }

        public void ImporterSearchBoxRemoveButtonClick()
        {
            ImporterSearchBoxRemoveButton.Click();
        }

        private IDomElement ImporterSearchBoxSearchButton => ImporterSearchBox.ElementFinder.WithClass("search").FindFirstOrDefault();

        public bool IsImporterSearchBoxSearchButtonDisplayed()
        {
            return !ImporterSearchBoxSearchButton.CssClasses.Contains("hidden");
        }

        private IDomElement ImporterListBox => ImporterContainer.ElementFinder.WithClass("scroll-area-container").FindFirstOrDefault();


        private IDomElement ImporterCountryListboxArea => ImporterListBox.ElementFinder.WithTagName("country-multiselect-view").FindFirstOrDefault();

        private IDomElement ImporterCountrySelectAllElement => ImporterCountryListboxArea.ElementFinder.WithClass("select-all").FindFirstOrDefault();

        private IDomElement ImporterCountrySelectAllLabelToClick => ImporterCountrySelectAllElement.ElementFinder.WithTagName("label").FindFirstOrDefault();

        private ISearchForElements ImporterCountrySelectAllElementChecked => ImporterCountrySelectAllElement.ElementFinder.WithTagName("span").WithClass("checked");

        private bool IsImporterCountrySelectAllElementChecked => ImporterCountrySelectAllElementChecked.FindFirstOrDefault() != null;

        private bool IsImporterCountrySelectAllElementPartiallyChecked => ImporterCountrySelectAllElement.Parent.ElementFinder.WithClass("indeterminate").FindFirstOrDefault() != null;

        private CheckBoxCheckType GetImporterSelectAllButtonCheckType()
        {
            if (IsImporterCountrySelectAllElementChecked)
                return CheckBoxCheckType.Checked;
            if (IsImporterCountrySelectAllElementPartiallyChecked)
                return CheckBoxCheckType.PartiallyChecked;
            return CheckBoxCheckType.UnChecked;
        }

        public void UnCheckAllListItemsInImporterCountry()
        {
            var checkBoxSelectionType = GetImporterSelectAllButtonCheckType();

            MapPanelPageObject map = new MapPanelPageObject(Browser);
            if (checkBoxSelectionType == CheckBoxCheckType.Checked)
            {
                ImporterCountrySelectAllLabelToClick.Click();
                Browser.WaitForAjax();
                map.WaitForMapLoaded();
            }
            if (checkBoxSelectionType == CheckBoxCheckType.PartiallyChecked)
            {
                ImporterCountrySelectAllLabelToClick.Click();
                Browser.WaitForElement(TimeSpan.FromSeconds(2), ImporterCountrySelectAllElementChecked);
                ImporterCountrySelectAllLabelToClick.Click();
                Browser.WaitForAjax();
                map.WaitForMapLoaded();
            }
        }

        public void ImporterCountrySelectAllLabelClick()
        {
            var checkBoxSelectionType = GetImporterSelectAllButtonCheckType();

            if (checkBoxSelectionType != CheckBoxCheckType.Checked)
            {
                MapPanelPageObject map = new MapPanelPageObject(Browser);
                ImporterCountrySelectAllLabelToClick.Click();
                Browser.WaitForAjax();
                map.WaitForMapLoaded();
            }
        }

        private IDomElement ImporterCountryListboxItems => ImporterCountryListboxArea.ElementFinder.WithTagName("ul").FindFirstOrDefault();

        private IEnumerable<IDomElement> ImporterCountryListboxItemsDisplayedList => ImporterCountryListboxItems.ElementFinder.WithTagName("li").Find().Where(x => x.Displayed);

        public List<string> ImporterCountryListboxVisibleItemsNamesList()
        {
            List<string> itemsNames = new List<string>();
            foreach (var item in ImporterCountryListboxItemsDisplayedList)
            {
                itemsNames.Add(item.Text);
            }

            return itemsNames;
        }

        public bool IsImporterCountryListboxContainVisibleItem(string itemName)
        {
            return ImporterCountryListboxVisibleItemsNamesList().Contains(itemName);
        }

        public void ImporterCountryListboxChooseFirstAvailableCountry()
        {
            MapPanelPageObject map = new MapPanelPageObject(Browser);
            var itemInListToChoose = ImporterCountryListboxItemsDisplayedList.FirstOrDefault();
            var itemToClick = itemInListToChoose.ElementFinder.WithTagName("label").FindFirstOrDefault();
            itemToClick.Click();
            Browser.WaitForAjax();
            map.WaitForMapLoaded();
        }






        private IDomElement ImporterFacilityListboxArea => ImporterListBox.ElementFinder.WithTagName("facility-multiselect-view").FindFirstOrDefault();

        private IDomElement ImporterFacilitySelectAllElement => ImporterFacilityListboxArea.ElementFinder.WithClass("select-all").FindFirstOrDefault();

        private IDomElement ImporterFacilitySelectAllLabelToClick => ImporterFacilitySelectAllElement.ElementFinder.WithTagName("label").FindFirstOrDefault();

        private ISearchForElements ImporterFacilitySelectAllElementChecked => ImporterFacilitySelectAllElement.ElementFinder.WithTagName("span").WithClass("checked");

        private bool IsImporterFacilitySelectAllElementChecked => ImporterFacilitySelectAllElementChecked.FindFirstOrDefault() != null;

        private bool IsImporterFacilitySelectAllElementPartiallyChecked => ImporterFacilitySelectAllElement.Parent.ElementFinder.WithClass("indeterminate").FindFirstOrDefault() != null;

        private CheckBoxCheckType GetImporterFacilitySelectAllButtonCheckType()
        {
            if (IsImporterFacilitySelectAllElementChecked)
                return CheckBoxCheckType.Checked;
            if (IsImporterFacilitySelectAllElementPartiallyChecked)
                return CheckBoxCheckType.PartiallyChecked;
            return CheckBoxCheckType.UnChecked;
        }

        public void UnCheckAllListItemsInImporterFacility()
        {
            var checkBoxSelectionType = GetImporterFacilitySelectAllButtonCheckType();

            MapPanelPageObject map = new MapPanelPageObject(Browser);
            if (checkBoxSelectionType == CheckBoxCheckType.Checked)
            {
                ImporterFacilitySelectAllLabelToClick.Click();
                Browser.WaitForAjax();
                map.WaitForMapLoaded();
            }
            if ((checkBoxSelectionType == CheckBoxCheckType.PartiallyChecked) || (checkBoxSelectionType == CheckBoxCheckType.UnChecked))
            {
                ImporterFacilitySelectAllLabelToClick.Click();
                Browser.WaitForElement(TimeSpan.FromSeconds(2), ImporterFacilitySelectAllElementChecked);
                ImporterFacilitySelectAllLabelToClick.Click();
                Browser.WaitForAjax();
                map.WaitForMapLoaded();
            }
        }


        private IDomElement ImporterFacilityListboxItems => ImporterFacilityListboxArea.ElementFinder.WithTagName("ul").FindFirstOrDefault();

        private IEnumerable<IDomElement> ImporterFacilityListboxItemsDisplayedList => ImporterFacilityListboxItems.ElementFinder.WithTagName("li").Find().Where(x => x.Displayed);

        private IEnumerable<IDomElement> ImporterFacilityListboxChildItemsList => ImporterFacilityListboxItemsDisplayedList.Where(x => x.ElementFinder.WithClass("cui-icon-caret-down").FindFirstOrDefault() == null);

        public List<string> ImporterFacilityListboxVisibleItemsNamesList => ImporterFacilityListboxChildItemsList.Select(x => x.Text).ToList();

        public bool IsImporterFacilityListboxContainVisibleItem(string itemName)
        {
            foreach (var item in ImporterFacilityListboxVisibleItemsNamesList)
            {
                if (item.Contains(itemName))
                    return true;
            }
            return false;
        }

        #endregion

        #endregion

        #region LegendTab

        private IDomElement TradeVolumeContainer => ManagementPanelBoard.ElementFinder.WithClass("padding-ver").FindFirstOrDefault();

        private IDomElement TradeVolumeLabel => ManagementPanelBoard.ElementFinder.WithTagName("label").FindFirstOrDefault();

        public string TradeVolumeLabelText => TradeVolumeLabel.Text;



        private IDomElement TypesContainer => ManagementPanelBoard.ElementFinder.WithClass("padding-ver").Find().LastOrDefault();



        private IDomElement TypeOfCountryLabel => TypesContainer.ElementFinder.WithTagName("label").FindFirstOrDefault();

        public string TypeOfCountryLabelText => TypeOfCountryLabel.Text;

        private ISearchForElements TypeOfCountryPlants => TypeOfCountryLabel.Parent.ElementFinder.WithTagName("div");

        public List<string> TypeOfCountryPlantsNames()
        {
            var typeOfCountryPlantsItems = TypeOfCountryPlants.Find().ToList<IDomElement>();
            List<string> typeOfCountryPlantsNames = new List<string>();
            foreach (var p in typeOfCountryPlantsItems)
            {
                typeOfCountryPlantsNames.Add(p.Text);
            }
            return typeOfCountryPlantsNames;
        }



        private IDomElement TypeOfFacilityLabel => TypesContainer.ElementFinder.WithTagName("label").Find().LastOrDefault();

        public string TypeOfFacilityLabelText => TypeOfFacilityLabel.Text;

        private ISearchForElements TypeOfFacilityPlants => TypeOfFacilityLabel.Parent.ElementFinder.WithTagName("div");

        public List<string> TypeOfFacilityPlantsNames()
        {
            var typeOfFacilityPlantsItems = TypeOfFacilityPlants.Find().ToList<IDomElement>();
            List<string> typeOfFacilityPlantsNames = new List<string>();
            foreach (var p in typeOfFacilityPlantsItems)
            {
                typeOfFacilityPlantsNames.Add(p.Text);
            }
            return typeOfFacilityPlantsNames;
        }
        #endregion
    }
}
