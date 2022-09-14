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
    [Category("ManagementPanel")]
    public class ManagementPanelQueryTabTests : LayoutTestsBase
    {
        private ManagementPanelPageObject _managementPanelPageObject;

        [SetUp]
        public void NavigateToApplication()
        {
            _managementPanelPageObject = NavigateTo<ManagementPanelPageObject>();
        }

        private const string _spotCargoLabelName = "Spot Cargo Only";
        private const string _reExportsLabelName = "Re-exports";
        private const string _displayUnitsLabelName = "Display Units";
        private const string _loadedUnloadedLabelName = "Loaded/Unloaded";
        private const string _supplierLabelName = "Exporter:";
        private const string _importerLabelName = "Importer:";

        private const string _tradeVolumesLabelName = "Trade Volumes";
        private const string _typeOfFacilityLabelName = "Dominant Type of Facility";
        private const string _liquefaction = "Liquefaction";
        private const string _regasification = "Regasification";
        private const string _dualPurpose = "Dual purpose";
        private const string _typeOfCountryLabelName = "Type of Country";
        private const string _exporter = "Exporter";
        private const string _importer = "Importer";
        private const string _importsExports = "Imports/Exports within borders";
        private const int numberOfTypeOfFacilityItemsInList = 3;
        private const int numberOfTypeOfCountryItemsInList = 3;

        [Test]
        public void ManagementPanelQueryTabLabelsCheck()
        {
            var spotCargoLabelName = _managementPanelPageObject.SpotCargoLabelText;
            var reExportsLabelName = _managementPanelPageObject.ReExportsLabelText;
            var displayUnitsLabelName = _managementPanelPageObject.DisplayUnitsLabelText;
            var loadedUnloadedLabelName = _managementPanelPageObject.LoadedUnloadedLabelText;
            var supplierLabelName = _managementPanelPageObject.SupplierLabelText;
            var importerLabelName = _managementPanelPageObject.ImporterLabelText;
            Assert.Multiple( () =>
            {
                Assert.AreEqual(_spotCargoLabelName, spotCargoLabelName);
                Assert.AreEqual(_reExportsLabelName, reExportsLabelName);
                Assert.AreEqual(_displayUnitsLabelName, displayUnitsLabelName);
                Assert.AreEqual(_loadedUnloadedLabelName, loadedUnloadedLabelName);
                Assert.AreEqual(_supplierLabelName, supplierLabelName);
                Assert.AreEqual(_importerLabelName, importerLabelName);
            });
        }

        [Test]
        public void ManagementPanelQueryTabSupplierSearchBoxCountryOperationTest()
        {
            PrepareCountryListBoxesForTests();

            var firstCountryName = _managementPanelPageObject.SupplierCountryListboxVisibleItemsNamesList()[0];
            var secondCountryName = _managementPanelPageObject.SupplierCountryListboxVisibleItemsNamesList()[1];

            Assert.IsTrue(_managementPanelPageObject.IsSupplierSearchBoxSearchButtonDisplayed());
            _managementPanelPageObject.SupplierSearchBoxText = firstCountryName;
            Assert.IsTrue(_managementPanelPageObject.IsSupplierCountryListboxContainVisibleItem(firstCountryName));
            Assert.IsFalse(_managementPanelPageObject.IsSupplierCountryListboxContainVisibleItem(secondCountryName));

            Assert.IsFalse(_managementPanelPageObject.IsSupplierSearchBoxSearchButtonDisplayed());
            Assert.IsTrue(_managementPanelPageObject.IsSupplierSearchBoxRemoveButtonDisplayed());
            _managementPanelPageObject.SupplierSearchBoxRemoveButtonClick();
            Assert.IsTrue(_managementPanelPageObject.IsSupplierCountryListboxContainVisibleItem(firstCountryName));
            Assert.IsTrue(_managementPanelPageObject.IsSupplierCountryListboxContainVisibleItem(secondCountryName));
            Assert.IsTrue(_managementPanelPageObject.IsSupplierSearchBoxSearchButtonDisplayed());
            Assert.IsFalse(_managementPanelPageObject.IsSupplierSearchBoxRemoveButtonDisplayed());
        }

        [Test]
        public void ManagementPanelQueryTabSupplierSearchBoxFacilityOperationTest()
        {
            PrepareFacilityListBoxesForTests();

            var firstFacilityName = _managementPanelPageObject.SupplierFacilityListboxVisibleItemsNamesList[0];
            var secondFacilityName = _managementPanelPageObject.SupplierFacilityListboxVisibleItemsNamesList[1];

            Assert.IsTrue(_managementPanelPageObject.IsSupplierSearchBoxSearchButtonDisplayed());
            _managementPanelPageObject.SupplierSearchBoxText = firstFacilityName;
            Assert.IsTrue(_managementPanelPageObject.IsSupplierFacilityListboxContainVisibleItem(firstFacilityName));
            Assert.IsFalse(_managementPanelPageObject.IsSupplierFacilityListboxContainVisibleItem(secondFacilityName));

            Assert.IsFalse(_managementPanelPageObject.IsSupplierSearchBoxSearchButtonDisplayed());
            Assert.IsTrue(_managementPanelPageObject.IsSupplierSearchBoxRemoveButtonDisplayed());
            _managementPanelPageObject.SupplierSearchBoxRemoveButtonClick();
            Assert.IsTrue(_managementPanelPageObject.IsSupplierFacilityListboxContainVisibleItem(firstFacilityName));
            Assert.IsTrue(_managementPanelPageObject.IsSupplierFacilityListboxContainVisibleItem(secondFacilityName));
            Assert.IsTrue(_managementPanelPageObject.IsSupplierSearchBoxSearchButtonDisplayed());
            Assert.IsFalse(_managementPanelPageObject.IsSupplierSearchBoxRemoveButtonDisplayed());
        }

        [Test]
        public void ManagementPanelQueryTabImporterSearchBoxCountryOperationTest()
        {
            PrepareCountryListBoxesForTests();

            var firstCountryName = _managementPanelPageObject.ImporterCountryListboxVisibleItemsNamesList()[0];
            var secondCountryName = _managementPanelPageObject.ImporterCountryListboxVisibleItemsNamesList()[1];

            Assert.IsTrue(_managementPanelPageObject.IsImporterSearchBoxSearchButtonDisplayed());
            _managementPanelPageObject.ImporterSearchBoxText = firstCountryName;
            Assert.IsTrue(_managementPanelPageObject.IsImporterCountryListboxContainVisibleItem(firstCountryName));
            Assert.IsFalse(_managementPanelPageObject.IsImporterCountryListboxContainVisibleItem(secondCountryName));

            Assert.IsFalse(_managementPanelPageObject.IsImporterSearchBoxSearchButtonDisplayed());
            Assert.IsTrue(_managementPanelPageObject.IsImporterSearchBoxRemoveButtonDisplayed());
            _managementPanelPageObject.ImporterSearchBoxRemoveButtonClick();
            Assert.IsTrue(_managementPanelPageObject.IsImporterCountryListboxContainVisibleItem(firstCountryName));
            Assert.IsTrue(_managementPanelPageObject.IsImporterCountryListboxContainVisibleItem(secondCountryName));
            Assert.IsTrue(_managementPanelPageObject.IsImporterSearchBoxSearchButtonDisplayed());
            Assert.IsFalse(_managementPanelPageObject.IsImporterSearchBoxRemoveButtonDisplayed());
        }

        [Test]
        public void ManagementPanelQueryTabImporterSearchBoxFacilityOperationTest()
        {
            PrepareFacilityListBoxesForTests();

            var firstFacilityName = _managementPanelPageObject.ImporterFacilityListboxVisibleItemsNamesList[0];
            var secondFacilityName = _managementPanelPageObject.ImporterFacilityListboxVisibleItemsNamesList[1];

            Assert.IsTrue(_managementPanelPageObject.IsImporterSearchBoxSearchButtonDisplayed());
            _managementPanelPageObject.ImporterSearchBoxText = firstFacilityName;
            Assert.IsTrue(_managementPanelPageObject.IsImporterFacilityListboxContainVisibleItem(firstFacilityName));
            Assert.IsFalse(_managementPanelPageObject.IsImporterFacilityListboxContainVisibleItem(secondFacilityName));

            Assert.IsFalse(_managementPanelPageObject.IsImporterSearchBoxSearchButtonDisplayed());
            Assert.IsTrue(_managementPanelPageObject.IsImporterSearchBoxRemoveButtonDisplayed());
            _managementPanelPageObject.ImporterSearchBoxRemoveButtonClick();
            Assert.IsTrue(_managementPanelPageObject.IsImporterFacilityListboxContainVisibleItem(firstFacilityName));
            Assert.IsTrue(_managementPanelPageObject.IsImporterFacilityListboxContainVisibleItem(secondFacilityName));
            Assert.IsTrue(_managementPanelPageObject.IsImporterSearchBoxSearchButtonDisplayed());
            Assert.IsFalse(_managementPanelPageObject.IsImporterSearchBoxRemoveButtonDisplayed());
        }

        [Test]
        public void ManagementPanelLegendTabLabelsCheck()
        {
            ResultsTablePageObject resultsTablePageObject = new ResultsTablePageObject(Browser);
            resultsTablePageObject.ShowAllAvailableRecords();

            _managementPanelPageObject.ChooseManagementPanelTab("Legend");

            Assert.AreEqual(_tradeVolumesLabelName, _managementPanelPageObject.TradeVolumeLabelText);
            Assert.AreEqual(_typeOfFacilityLabelName, _managementPanelPageObject.TypeOfFacilityLabelText);
            Assert.AreEqual(_typeOfCountryLabelName, _managementPanelPageObject.TypeOfCountryLabelText);

            var typeOfFacilityPlantsNames = _managementPanelPageObject.TypeOfFacilityPlantsNames();
            Assert.AreEqual(numberOfTypeOfFacilityItemsInList, typeOfFacilityPlantsNames.Count);
            Assert.IsTrue(typeOfFacilityPlantsNames.Contains(_liquefaction));
            Assert.IsTrue(typeOfFacilityPlantsNames.Contains(_regasification));
            Assert.IsTrue(typeOfFacilityPlantsNames.Contains(_dualPurpose));
            var typeOfCountryPlantsNames = _managementPanelPageObject.TypeOfCountryPlantsNames();
            Assert.AreEqual(numberOfTypeOfCountryItemsInList, typeOfCountryPlantsNames.Count);
            Assert.IsTrue(typeOfCountryPlantsNames.Contains(_exporter));
            Assert.IsTrue(typeOfCountryPlantsNames.Contains(_importer));
            Assert.IsTrue(typeOfCountryPlantsNames.Contains(_importsExports));
        }

        public void PrepareCountryListBoxesForTests()
        {
            _managementPanelPageObject.ClickSupplierToggleButton("Country");
            _managementPanelPageObject.ClickImporterToggleButton("Country");
            _managementPanelPageObject.UnCheckAllListItemsInSupplierCountry();
            _managementPanelPageObject.UnCheckAllListItemsInImporterCountry();

            _managementPanelPageObject = new ManagementPanelPageObject(Browser);
        }

        public void PrepareFacilityListBoxesForTests()
        {
            _managementPanelPageObject.ClickSupplierToggleButton("Facility");
            _managementPanelPageObject.ClickImporterToggleButton("Facility");
            _managementPanelPageObject.UnCheckAllListItemsInSupplierFacility();
            _managementPanelPageObject.UnCheckAllListItemsInImporterFacility();

            _managementPanelPageObject = new ManagementPanelPageObject(Browser);
        }
    }
}
