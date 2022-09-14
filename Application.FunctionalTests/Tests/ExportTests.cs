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
    [Category("Export")]
    public class ExportTests : LayoutTestsBase
    {
        private ExportPageObject _exportPageObject;

        [SetUp]
        public void NavigateToApplication()
        {
            _exportPageObject = NavigateTo<ExportPageObject>();
        }

        [Test]
        public void GridExportIsNotEmpty()
        {
            SetSingleSupplierAndImporterCountry();
            _exportPageObject.AssertExcelExportNonEmpty();
        }

        [Test]
        public void GridExportContainsCorrectData()
        {
            SetSingleSupplierAndImporterCountry();
            _exportPageObject.AssertExcelAndGridHaveTheSameData();
        }

        protected void SetSingleSupplierAndImporterCountry()
        {
            ManagementPanelPageObject _managementPanelPageObject = new ManagementPanelPageObject(Browser);

            _managementPanelPageObject.ClickSupplierToggleButton("Country");
            _managementPanelPageObject.ClickImporterToggleButton("Country");
            _managementPanelPageObject.UnCheckAllListItemsInSupplierCountry();
            _managementPanelPageObject.UnCheckAllListItemsInImporterCountry();

            _managementPanelPageObject.SupplierCountryListboxChooseFirstAvailableCountry();
            _managementPanelPageObject.ImporterCountryListboxChooseFirstAvailableCountry();
        }
    }
}
