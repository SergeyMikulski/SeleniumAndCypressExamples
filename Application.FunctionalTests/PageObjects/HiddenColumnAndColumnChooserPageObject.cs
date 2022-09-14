using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.FunctionalTests.Tests;

namespace Application.FunctionalTests.PageObjects
{
    public class HiddenColumnAndColumnChooserPageObject : ResultsTablePageObject
    {
        public HiddenColumnAndColumnChooserPageObject(IAutomateBrowser browser) : base(browser)
        {
        }

        public ResultsTableTests ResultsTableTests { get; }

        #region HiddenColumnMenu
        private IEnumerable<IDomElement> HiddenColumnMenuList => TableContainer.ElementFinder.WithClass("ui-iggrid-hiding-dropdown-dialog").Find();

        private IDomElement HiddenColumnMenuDisplayed => HiddenColumnMenuList.Except(
            HiddenColumnMenuList.Where(x => x.ElementFinder.WithPartialAttributeValue("style", "display: none").FindFirstOrDefault() != null)
            ).FirstOrDefault();


        private IEnumerable<IDomElement> HiddenColumnsInHiddenColumnsMenuList => HiddenColumnMenuDisplayed.ElementFinder.WithTagName("span").Find();

        private IDomElement FirstHiddenColumnInHiddenColumnsMenuList => HiddenColumnsInHiddenColumnsMenuList.FirstOrDefault();

        public string FirstHiddenColumnInHiddenColumnsMenuListText => FirstHiddenColumnInHiddenColumnsMenuList.Text;

        public void FirstHiddenColumnInHiddenColumnsMenuListClick()
        {
            FirstHiddenColumnInHiddenColumnsMenuList.Click();
            Browser.WaitForAjax();
            WaitTableMaskDisappear();
        }

        private IDomElement ColumnChooserButton => HiddenColumnMenuDisplayed.ElementFinder.WithTagName("a").FindFirstOrDefault();

        public void ColumnChooserButtonClick()
        {
            ColumnChooserButton.Click();
            Browser.WaitForAjax();
            WaitTableMaskDisappear();
        }

        #endregion

        #region ColumnChooser

        private IDomElement ColumnChooserDialogue => TableContainer.ElementFinder.WithId("grid_xa1_hiding_modalDialog").FindFirstOrDefault();

        #region ColumnChooserHeader
        private IDomElement ColumnChooserDialogueHeader => ColumnChooserDialogue.ElementFinder.WithClass("ui-dialog-titlebar").FindFirstOrDefault();

        private IDomElement ColumnChooserDialogueTitle => ColumnChooserDialogueHeader.ElementFinder.WithClass("ui-dialog-title").FindFirstOrDefault();

        private string ColumnChooserDialogueTitleText => ColumnChooserDialogueTitle.Text;

        private IDomElement ColumnChooserDialogueResetButton => ColumnChooserDialogueHeader.ElementFinder.WithTagName("button").FindFirstOrDefault();

        public void ColumnChooserDialogueResetButtonClick() => ColumnChooserDialogueResetButton.Click();

        #endregion

        #region ColumnChooserFooter

        private IDomElement ColumnChooserDialogueFooter => ColumnChooserDialogue.ElementFinder.WithClass("ui-dialog-buttonpane").FindFirstOrDefault();

        private IDomElement ColumnChooserDialogueApplyButton => ColumnChooserDialogueFooter.ElementFinder.WithText("Apply").FindFirstOrDefault();

        private IDomElement ColumnChooserDialogueCancelButton => ColumnChooserDialogueFooter.ElementFinder.WithText("Cancel").FindFirstOrDefault();

        public void ColumnChooserDialogueApplyButtonClick()
        {
            ColumnChooserDialogueApplyButton.Click();
            Browser.WaitForAjax();
            WaitTableMaskDisappear();
        }

        public void ColumnChooserDialogueCancelButtonClick()
        {
            ColumnChooserDialogueCancelButton.Click();
            Browser.WaitForAjax();
            WaitTableMaskDisappear();
        }

        #endregion

        #region ColumnChooserBody

        private IDomElement ColumnChooserDialogueBody => ColumnChooserDialogue.ElementFinder.WithClass("ui-dialog-content").FindFirstOrDefault();

        private IEnumerable<IDomElement> ColumnChooserDialogueBodyAllItemsList => ColumnChooserDialogueBody.ElementFinder.WithTagName("li").Find();

        private IEnumerable<IDomElement> ColumnChooserDialogueBodyHiddenItemsList => ColumnChooserDialogueBody.ElementFinder.WithTagName("li").WithClass("ui-iggrid-columnchooser-itemhidden").Find();

        private IEnumerable<IDomElement> ColumnChooserDialogueBodyShownItemsList => ColumnChooserDialogueBody.ElementFinder.WithTagName("li").WithoutClass("ui-iggrid-columnchooser-itemhidden").Find();

        public List<string> ColumnChooserDialogueAllColumnsNames()
        {
            List<string> columnChooserDialogueAllColumnsNames = new List<string>();

            foreach (var name in ColumnChooserDialogueBodyAllItemsList)
            {
                columnChooserDialogueAllColumnsNames.Add(name.ElementFinder.WithClass("ui-iggrid-dialog-text").FindFirstOrDefault().Text);
            }

            return columnChooserDialogueAllColumnsNames;
        }

        public List<string> ColumnChooserDialogueHiddenColumnsNames()
        {
            List<string> columnChooserDialogueHiddenColumnsNames = new List<string>();

            foreach (var name in ColumnChooserDialogueBodyHiddenItemsList)
            {
                columnChooserDialogueHiddenColumnsNames.Add(name.ElementFinder.WithClass("ui-iggrid-dialog-text").FindFirstOrDefault().Text);
            }

            return columnChooserDialogueHiddenColumnsNames;
        }

        public List<string> ColumnChooserDialogueShownColumnsNames()
        {
            List<string> columnChooserDialogueShownColumnsNames = new List<string>();

            foreach (var name in ColumnChooserDialogueBodyShownItemsList)
            {
                columnChooserDialogueShownColumnsNames.Add(name.ElementFinder.WithClass("ui-iggrid-dialog-text").FindFirstOrDefault().Text);
            }

            return columnChooserDialogueShownColumnsNames;
        }

        public void ColumnChooserDialogueShowColumn(string columnName)
        {
            ColumnChooserDialogueBodyHiddenItemsList.SingleOrDefault(x => x.ElementFinder.WithClass("ui-iggrid-dialog-text").FindFirstOrDefault().Text == columnName).Click();
        }

        public void ColumnChooserDialogueHideColumn(string columnName)
        {
            ColumnChooserDialogueBodyShownItemsList.SingleOrDefault(x => x.ElementFinder.WithClass("ui-iggrid-dialog-text").FindFirstOrDefault().Text == columnName).Click();
        }

        #endregion

        #endregion
    }
}
