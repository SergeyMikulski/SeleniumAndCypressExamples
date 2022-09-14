using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.FunctionalTests.PageObjects;

namespace Application.FunctionalTests.Tests
{
    [LogInAs("User")]
    [Category("ResultsTable")]
    public class ResultsTableTests : LayoutTestsBase
    {
        private ResultsTablePageObject _resultsTable;
        private const string _tableHeaderText = "Application Results";

        [SetUp]
        public void NavigateToApplication()
        {
            _resultsTable = NavigateTo<ResultsTablePageObject>();
            Browser.WaitForAjax();
            _resultsTable.WaitMaskDisappear();

            ExpandResultsTable();
        }

        [Test]
        public void ExpandCollapseResultsTable()
        {
            var isResultsTableCollapsed = _resultsTable.IsResultsTableCollapsed();
            var tableHeaderText = _resultsTable.TableHeaderText;
            Assert.AreEqual(_tableHeaderText, tableHeaderText);

            _resultsTable.TableHeaderExpandCollapseArrowClick();
            var isResultsTableCollapsedAfterClick = _resultsTable.IsResultsTableCollapsed();

            if (isResultsTableCollapsed)
            {
                Assert.IsFalse(isResultsTableCollapsedAfterClick);
            }
            else
            {
                Assert.IsTrue(isResultsTableCollapsedAfterClick);
            }
        }

        [Test]
        public void ColumnSorting()
        {
            var sortedColumn = _resultsTable.WhatColumnIsSorted();

            if (sortedColumn == "")
            {
                sortedColumn = "Date Arrived";
                _resultsTable.ColumnHeaderCellClick(sortedColumn);
                Assert.IsTrue(_resultsTable.IsColumnSortedAsc(sortedColumn));
                _resultsTable.ColumnHeaderCellClick(sortedColumn);
                Assert.IsFalse(_resultsTable.IsColumnSortedAsc(sortedColumn));
            }
            else
            {
                var isColumnSortedAsc = _resultsTable.IsColumnSortedAsc(sortedColumn);
                if (isColumnSortedAsc)
                {
                    _resultsTable.ColumnHeaderCellClick(sortedColumn);
                    Assert.IsFalse(_resultsTable.IsColumnSortedAsc(sortedColumn));
                    _resultsTable.ColumnHeaderCellClick(sortedColumn);
                    Assert.IsTrue(_resultsTable.IsColumnSortedAsc(sortedColumn));
                }
            }
        }

        [Test]
        public void ColumnHiding()
        {
            var shownColumnsList = _resultsTable.DisplayedColumnsNames();
            var columnToBeHidden = shownColumnsList[0];
            _resultsTable.ColumnHeaderDataHidingArrowClick(columnToBeHidden);
            Assert.IsTrue(_resultsTable.IsGridHasHiddenColumns());

            shownColumnsList = _resultsTable.DisplayedColumnsNames();

            Assert.IsTrue(!shownColumnsList.Contains(columnToBeHidden));
        }

        [Test]
        public void ColumnShowingByColumnMenu()
        {
            var shownColumnsList = _resultsTable.DisplayedColumnsNames();
            var columnToBeHidden = shownColumnsList[0];
            _resultsTable.ColumnHeaderDataHidingArrowClick(columnToBeHidden);

            shownColumnsList = _resultsTable.DisplayedColumnsNames();
            Assert.IsFalse(shownColumnsList.Contains(columnToBeHidden));

            _resultsTable.HiddenColumnsGridItemFirstElementClick();
            var columnChooserDialogueAndHiddenColumnsMenu = _resultsTable.HiddenColumnMenuAndColumnChooser();
            var columnToBeShown = columnChooserDialogueAndHiddenColumnsMenu.FirstHiddenColumnInHiddenColumnsMenuListText;
            columnChooserDialogueAndHiddenColumnsMenu.FirstHiddenColumnInHiddenColumnsMenuListClick();

            shownColumnsList = _resultsTable.DisplayedColumnsNames();
            Assert.IsTrue(shownColumnsList.Contains(columnToBeShown));
        }

        [Test]
        public void ColumnChooserResetButtonShowsAllColumns()
        {
            var shownColumnsList = _resultsTable.DisplayedColumnsNames();
            var columnToBeHidden = shownColumnsList[0];
            _resultsTable.ColumnHeaderDataHidingArrowClick(columnToBeHidden);
            _resultsTable.HiddenColumnsGridItemFirstElementClick();

            var columnChooserDialogueAndHiddenColumnsMenu = _resultsTable.HiddenColumnMenuAndColumnChooser();
            columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserButtonClick();

            Assert.AreEqual(shownColumnsList, columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserDialogueAllColumnsNames());
            var columnChooserDialogueHiddenColumns = columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserDialogueHiddenColumnsNames();
            Assert.IsTrue(columnChooserDialogueHiddenColumns.Contains(columnToBeHidden));

            columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserDialogueResetButtonClick();
            columnChooserDialogueHiddenColumns = columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserDialogueHiddenColumnsNames();
            Assert.IsTrue(columnChooserDialogueHiddenColumns.Count() == 0);
            var columnChooserDialogueShownColumnsList = columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserDialogueShownColumnsNames();
            columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserDialogueApplyButtonClick();

            shownColumnsList = _resultsTable.DisplayedColumnsNames();
            Assert.AreEqual(columnChooserDialogueShownColumnsList, shownColumnsList);
        }

        [Test]
        public void ColumnChooserShowHideColumns()
        {
            var shownColumnsList = _resultsTable.DisplayedColumnsNames();
            var columnToBeHidden = shownColumnsList[0];
            _resultsTable.ColumnHeaderDataHidingArrowClick(columnToBeHidden);
            _resultsTable.HiddenColumnsGridItemFirstElementClick();

            var columnChooserDialogueAndHiddenColumnsMenu = _resultsTable.HiddenColumnMenuAndColumnChooser();
            columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserButtonClick();

            var columnChooserDialogueShownColumns = columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserDialogueShownColumnsNames();
            var columnChooserDialogueHiddenColumns = columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserDialogueHiddenColumnsNames();
            var columnToShow = columnChooserDialogueHiddenColumns[0];
            var columnToHide = columnChooserDialogueShownColumns[0];

            columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserDialogueShowColumn(columnToShow);
            columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserDialogueHideColumn(columnToHide);
            columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserDialogueApplyButtonClick();

            shownColumnsList = _resultsTable.DisplayedColumnsNames();
            Assert.IsTrue(shownColumnsList.Contains(columnToShow));
            Assert.IsFalse(shownColumnsList.Contains(columnToHide));
        }

        [Test]
        public void ColumnChooserCancelButtonRevertsChanges()
        {
            var shownColumnsList = _resultsTable.DisplayedColumnsNames();
            var columnToBeHidden = shownColumnsList[1];
            _resultsTable.ColumnHeaderDataHidingArrowClick(columnToBeHidden);
            _resultsTable.HiddenColumnsGridItemFirstElementClick();

            var columnChooserDialogueAndHiddenColumnsMenu = _resultsTable.HiddenColumnMenuAndColumnChooser();
            columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserButtonClick();

            var columnChooserDialogueShownColumns = columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserDialogueShownColumnsNames();
            var columnChooserDialogueHiddenColumns = columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserDialogueHiddenColumnsNames();
            var columnToShow = columnChooserDialogueHiddenColumns[0];
            var columnToHide = columnChooserDialogueShownColumns[0];

            columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserDialogueShowColumn(columnToShow);
            columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserDialogueHideColumn(columnToHide);
            columnChooserDialogueAndHiddenColumnsMenu.ColumnChooserDialogueCancelButtonClick();

            shownColumnsList = _resultsTable.DisplayedColumnsNames();
            Assert.IsFalse(shownColumnsList.Contains(columnToShow));
            Assert.IsTrue(shownColumnsList.Contains(columnToHide));
        }

        [Test]
        public void SetNumberOfRowsInGrid()
        {
            _resultsTable.ShowAllAvailableRecords();

            var numberOfElementsShownInFooter = _resultsTable.NumberOfElementsShownText;
            var rowsCount = _resultsTable.GridTableRowsCount;
            Assert.AreEqual(int.Parse(numberOfElementsShownInFooter), rowsCount);

            if (rowsCount == 5)
                _resultsTable.NumberOfElementsDropDownChoose("10");
            else
                _resultsTable.NumberOfElementsDropDownChoose("5");

            numberOfElementsShownInFooter = _resultsTable.NumberOfElementsShownText;
            rowsCount = _resultsTable.GridTableRowsCount;

            Assert.AreEqual(int.Parse(numberOfElementsShownInFooter), rowsCount);
        }

        [Test]
        public void GridPaginatorTest()
        {
            _resultsTable.ShowAllAvailableRecords();

            var selectedPageFromPaginator = _resultsTable.SelectedPageNumberFromPaginator;
            var selectedPageFromRecord = _resultsTable.SelectedPageNumberShownFromRecord();
            Assert.AreEqual(selectedPageFromRecord, selectedPageFromPaginator);

            _resultsTable.NextPageClick();

            var selectedPageFromPaginatorNextPage = _resultsTable.SelectedPageNumberFromPaginator;
            var selectedPageFromRecordNextPage = _resultsTable.SelectedPageNumberShownFromRecord();
            Assert.AreEqual(selectedPageFromRecordNextPage, selectedPageFromPaginatorNextPage);

            _resultsTable.SelectPageInPaginator((selectedPageFromPaginatorNextPage + 2).ToString());

            var selectedPageFromPaginatorManuallyChosen = _resultsTable.SelectedPageNumberFromPaginator;
            var selectedPageFromRecordManuallyChosen = _resultsTable.SelectedPageNumberShownFromRecord();
            Assert.AreEqual(selectedPageFromRecordManuallyChosen, selectedPageFromPaginatorManuallyChosen);

            _resultsTable.PreviousPageClick();

            var selectedPageFromPaginatorPreviousPage = _resultsTable.SelectedPageNumberFromPaginator;
            var selectedPageFromRecordPreviousPage = _resultsTable.SelectedPageNumberShownFromRecord();
            Assert.AreEqual(selectedPageFromRecordPreviousPage, selectedPageFromPaginatorPreviousPage);
        }

        private void ExpandResultsTable()
        {
            var isResultsTableCollapsed = _resultsTable.IsResultsTableCollapsed();

            if (isResultsTableCollapsed)
            {
                _resultsTable.TableHeaderExpandCollapseArrowClick();
            }
        }
    }
}
