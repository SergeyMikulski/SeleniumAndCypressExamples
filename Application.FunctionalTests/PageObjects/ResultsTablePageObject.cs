using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.FunctionalTests.Tests;

namespace Application.FunctionalTests.PageObjects
{
    public class ResultsTablePageObject : PagesPageObjectBase
    {
        public ResultsTablePageObject(IAutomateBrowser browser) : base(browser)
        {
        }

        private IDomElement ResultsTableContainer => RightContainer.ElementFinder.WithClass("trade-grid").FindFirstOrDefault();

        #region TableHeader
        private IDomElement TableHeader => ResultsTableContainer.ElementFinder.WithClass("box-header").FindFirstOrDefault();

        private IDomElement TableHeaderExpandCollapseArrow => TableHeader.ElementFinder.WithClass("toggle-arrow").FindFirstOrDefault();

        public string TableHeaderText => TableHeader.Text;

        public void TableHeaderExpandCollapseArrowClick()
        {
            TableHeaderExpandCollapseArrow.Click();
            Browser.WaitForAjax();
        }

        public bool IsResultsTableCollapsed() => TableHeader.CssClasses.Contains("collapsed");
        #endregion

        #region TableContainer
        protected IDomElement TableContainer => ResultsTableContainer.ElementFinder.WithClass("data-wrapper").FindFirstOrDefault();

        private IDomElement TableMask => TableContainer.ElementFinder.WithId("grid_xa1_container_loading").FindFirstOrDefault();

        private bool IsTableMaskVisible => TableMask.Displayed;

        public void WaitTableMaskDisappear()
        {
            Browser.WaitForAjax();
            Browser.Wait(TimeSpan.FromSeconds(20), x => !IsTableMaskVisible);
        }

        #region ExportButton

        private IDomElement GridOptionsContainer => TableContainer.ElementFinder.WithTagName("cui-card-header").FindFirstOrDefault();

        private IDomElement ExportButton => GridOptionsContainer.ElementFinder.WithTagName("button").FindFirstOrDefault();

        public void ExportButtonClick()
        {
            Browser.WaitForAjax();
            WaitMaskDisappear();
            ExportButton.Click();
        }

        #endregion

        #region GridBodyContainer
        private IDomElement GridBodyContainer => TableContainer.ElementFinder.WithTagName("table").WithId("grid_xa1").FindFirstOrDefault();

        #region GridHeader
        private IDomElement GridHeaderContainer => GridBodyContainer.ElementFinder.WithTagName("thead").FindFirstOrDefault();

        private IEnumerable<IDomElement> ColumnsList => GridHeaderContainer.ElementFinder.WithTagName("th").Find();

        private IDomElement ColumnHeaderCell(string columnName) => GridHeaderContainer.ElementFinder.WithText(columnName).FindFirstOrDefault().Parent;

        public void ColumnHeaderCellClick(string columnName)
        {
            ColumnHeaderCell(columnName).Click();
            Browser.WaitForAjax();
            WaitTableMaskDisappear();
        }

        private IDomElement ColumnHeaderIndicatorsContainer(string columnName) => ColumnHeaderCell(columnName).ElementFinder.WithClass("ui-iggrid-indicatorcontainer").FindFirstOrDefault();

        private IDomElement ColumnHeaderDataHidingArrow(string columnName) => ColumnHeaderIndicatorsContainer(columnName).ElementFinder.WithTagName("a").FindFirstOrDefault();

        public void ColumnHeaderDataHidingArrowClick(string columnName)
        {
            ColumnHeaderDataHidingArrow(columnName).Click();
            Browser.WaitForAjax();
            WaitTableMaskDisappear();
        }

        private IDomElement ColumnHeaderSortingArrow(string columnName) => ColumnHeaderIndicatorsContainer(columnName).ElementFinder.WithClass("ui-iggrid-colindicator").WithClass("ui-icon").FindFirstOrDefault();

        public string WhatColumnIsSorted() => ColumnsList.SingleOrDefault(x => x.ElementFinder.WithClass("ui-iggrid-colindicator").WithClass("ui-icon").FindFirstOrDefault() != null)?.Text ?? "";

        public bool IsColumnSortedAsc(string columnName) => ColumnHeaderSortingArrow(columnName).CssClasses.Contains("ui-iggrid-colindicator-asc");

        public List<string> DisplayedColumnsNames()
        {
            List<string> displayedColumnsnames = new List<string>();

            foreach(var column in ColumnsList)
            {
                var name = column.ElementFinder.WithClass("ui-iggrid-headertext").FindFirstOrDefault().Text;
                displayedColumnsnames.Add(name);
            }

            return displayedColumnsnames;
        }

        private IEnumerable<IDomElement> HiddenColumnsElements => ColumnsList.Where(x => x.ElementFinder.WithAttributeValue("data-hiddencolumn-indicator", "true").FindFirstOrDefault() != null);

        public bool IsGridHasHiddenColumns() => HiddenColumnsElements.Count() > 0;

        public void HiddenColumnsGridItemFirstElementClick()
        {
            var firstColumnWithHiddenElement = HiddenColumnsElements.ElementAt(0);
            var hiddenElement = firstColumnWithHiddenElement.ElementFinder.WithAttributeValue("data-hiddencolumn-indicator", "true").FindFirstOrDefault();
            hiddenElement.Click();
            Browser.WaitForAjax();
            WaitTableMaskDisappear();
        }

        public HiddenColumnAndColumnChooserPageObject HiddenColumnMenuAndColumnChooser()
        {
            var hiddenColumnAndColumnChooser = new HiddenColumnAndColumnChooserPageObject(Browser);

            return hiddenColumnAndColumnChooser;
        }

        #endregion

        #region GridTable

        private IDomElement GridTable => GridBodyContainer.ElementFinder.WithTagName("tbody").FindFirstOrDefault();

        private IEnumerable<IDomElement> GridTableRows => GridTable.ElementFinder.WithTagName("tr").Find();

        public int GridTableRowsCount => GridTableRows.Count();

        public List<List<string>> GridTableItems()
        {
            var table = new List<List<string>>();
            foreach(var row in GridTableRows)
            {
                var castedRow = new List<string>();
                var cells = row.ElementFinder.WithTagName("td").Find();
                foreach (var cell in cells)
                {
                    castedRow.Add(cell.Text);
                }

                table.Add(castedRow);
            }

            return table;
        }

        #endregion

        #endregion

        #region TableFooter

        protected IDomElement TableFooterContainer => TableContainer.ElementFinder.WithId("grid_xa1_pager").FindFirstOrDefault();

        protected IDomElement TableFooterNumberOfElementsRecord => TableFooterContainer.ElementFinder.WithId("grid_xa1_pager_label").FindFirstOrDefault();

        public string TableFooterNumberOfElementsRecordText => TableFooterNumberOfElementsRecord.Text;

        public int TableFooterNumberOfElementsShown()
        {
            var firstIndex = TableFooterNumberOfElementsRecordText.IndexOf('f') + 2;
            var lastIndex = TableFooterNumberOfElementsRecordText.IndexOf('t') - 2;
            var part = TableFooterNumberOfElementsRecordText.Substring(firstIndex, (lastIndex - firstIndex + 1));

            return int.Parse(part);
        }

        public int SelectedPageNumberShownFromRecord()
        {
            var lastIndex = TableFooterNumberOfElementsRecordText.IndexOf(' ');
            var part = TableFooterNumberOfElementsRecordText.Substring(0, lastIndex);

            var pageNumber = (int.Parse(part) - 1) / int.Parse(NumberOfElementsShownText) + 1;

            return pageNumber;
        }



        protected IDomElement PagingContainer => TableFooterContainer.ElementFinder.WithClass("ui-iggrid-paging").FindFirstOrDefault();

        protected IDomElement PreviousPage => PagingContainer.ElementFinder.WithClass("ui-iggrid-prevpage").FindFirstOrDefault();

        protected IDomElement NextPage => PagingContainer.ElementFinder.WithClass("ui-iggrid-nextpage").FindFirstOrDefault();

        public void PreviousPageClick()
        {
            PreviousPage.Click();
            Browser.WaitForAjax();
            WaitTableMaskDisappear();
        }

        public void NextPageClick()
        {
            NextPage.Click();
            Browser.WaitForAjax();
            WaitTableMaskDisappear();
        }

        protected IDomElement PaginatorPagesContainer => PagingContainer.ElementFinder.WithTagName("ul").WithClass("ui-helper-reset").FindFirstOrDefault();

        protected IDomElement SelectedPage => PagingContainer.ElementFinder.WithTagName("li").WithClass("ui-state-active").FindFirstOrDefault();

        public int SelectedPageNumberFromPaginator => int.Parse(SelectedPage.Text);

        protected IDomElement PageNumberInPaginator(string pageNumber) => PaginatorPagesContainer.ElementFinder.WithText(pageNumber).FindFirstOrDefault();

        public void SelectPageInPaginator(string pageNumber)
        {
            PageNumberInPaginator(pageNumber).Click();
            Browser.WaitForAjax();
            WaitTableMaskDisappear();
        }



        protected IDomElement NumberOfElementsContainer => PagingContainer.ElementFinder.WithClass("ui-igedit-container").FindFirstOrDefault();

        protected IDomElement NumberOfElementsArrow => NumberOfElementsContainer.ElementFinder.WithClass("ui-igedit-buttonimage").FindFirstOrDefault();

        protected void NumberOfElementsArrowClick() => NumberOfElementsArrow.Click();

        protected IDomElement NumberOfElementsShownParent => NumberOfElementsContainer.ElementFinder.WithTagName("span").WithClass("ui-iggrid-pagesizedropdown").FindFirstOrDefault();

        protected IDomElement NumberOfElementsShown => NumberOfElementsShownParent.ElementFinder.WithTagName("input").Find().LastOrDefault();

        public string NumberOfElementsShownText => NumberOfElementsShown["value"];



        protected IDomElement NumberOfElementsDropDown => Browser.ElementFinder.WithTagName("div").WithId("grid_xa1_editor_list").FindFirstOrDefault();

        protected IDomElement NumberOfElementsDropDownItem(string itemText) => NumberOfElementsDropDown.ElementFinder.WithText(itemText).FindFirstOrDefault();

        public void NumberOfElementsDropDownChoose(string itemText)
        {
            for (int i = 0; i < 5; i++)
            {
                NumberOfElementsArrowClick();
                if (IsNumberOfElementsDropDownShown())
                {
                    try
                    {
                        NumberOfElementsDropDownItem(itemText).Click();
                    }
                    catch
                    {
                        continue;
                    }
                    Browser.WaitForAjax();
                    WaitTableMaskDisappear();
                    break;
                }
            }
        }

        protected bool IsNumberOfElementsDropDownShown() => NumberOfElementsDropDown.Displayed;

        #endregion

        #endregion

        public void ShowAllAvailableRecords()
        {
            ManagementPanelPageObject _managementPanelPageObject = new ManagementPanelPageObject(Browser);

            _managementPanelPageObject.ClickSupplierToggleButton("Country");
            _managementPanelPageObject.ClickImporterToggleButton("Country");
            _managementPanelPageObject.SupplierCountrySelectAllLabelClick();
            _managementPanelPageObject.ImporterCountrySelectAllLabelClick();
        }
    }
}
