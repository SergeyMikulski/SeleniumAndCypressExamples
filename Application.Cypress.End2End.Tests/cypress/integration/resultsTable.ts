import { resultsTableHelper } from '../support/resultsTableHelper';
import { managementPanelHelper } from '../support/managementPanelHelper';

const visibleColumnsNamesClass = '.ui-iggrid-headercell-featureenabled .ui-iggrid-headertext'
const columnHeaderHiddenIndicatorClass = '.ui-iggrid-hiding-hiddencolumnindicator'
const columnHeaderDataHidingArrowClass = '.ui-iggrid-hiding-indicator'
const columnMenuHiddenItemsPath = '.ui-iggrid-hiding-dropdown-ddlistitemicons span'

const columnChooserButtonPath = '[data-button-show-column-chooser]'
const columnChooserHideButtonsPath = '[data-localeid="columnChooserHideText"]'
const columnChooserShowButtonsPath = '[data-localeid="columnChooserShowText"]'
const columnChooserApplyButtonId = '#grid_xa1_hiding_modalDialog_footer_buttonok'
const columnChooserColumnNamePath = '.ui-iggrid-dialog-text'

const _exporterLabelName = "Exporter:";
const _importerLabelName = "Importer:";

const exporterImporterLabelPath = 'strong.line-height-lg'
const countryFacilityTogglePath = 'country-facility-selector'
const countryFacilityButtonPath = 'cui-radio-group-element label'

const countryBoxPath = 'country-multiselect-view '

describe('Application Results Table', () => {
    before(() => {
        cy.LogIn('/TestEmptyPage');
    })
    beforeEach(() => {
        cy.PreserveCookies()
        cy.on('uncaught:exception', (err, runnable) => {
            return false
          })
        cy.visit('/Application').then(() =>{
            cy.get('cui-loader').should('exist').then(() =>{
                cy.get('cui-loader').should('not.exist').then(() =>{
                    cy.wait(5000)
                })
            })
        })
    })
    
    describe('Application Results Table tests', () => {
        it('Expand Collapse Results Table', () => {
            resultsTableHelper.verifyExpandCollapseResultsTable()
        })
        it('Column Sorting', () => {
            resultsTableHelper.verifyColumnsSorting()
        })
        it('Column Hiding', () => {
            resultsTableHelper.verifyColumnHiding(visibleColumnsNamesClass, columnHeaderDataHidingArrowClass)
        })
        it('Column Showing By Column Menu', () => {
            resultsTableHelper.verifyColumnShowingByColumnMenu(visibleColumnsNamesClass, columnHeaderDataHidingArrowClass, columnHeaderHiddenIndicatorClass, columnMenuHiddenItemsPath)
        })
        it('Column Chooser Reset Button Shows All Columns', () => {
            resultsTableHelper.verifyColumnChooserResetButtonShowsAllColumns(visibleColumnsNamesClass, columnHeaderDataHidingArrowClass, columnHeaderHiddenIndicatorClass
                ,columnChooserButtonPath, columnChooserColumnNamePath, columnChooserHideButtonsPath, columnChooserShowButtonsPath, columnChooserApplyButtonId)
        })
        it('Column Chooser Show Hide Columns', () => {
            resultsTableHelper.verifyColumnChooserShowHideColumns(visibleColumnsNamesClass, columnHeaderDataHidingArrowClass, columnHeaderHiddenIndicatorClass
                , columnChooserButtonPath, columnChooserColumnNamePath, columnChooserHideButtonsPath, columnChooserShowButtonsPath, columnChooserApplyButtonId)
        })
        it('Column Chooser Cancel Button Reverts Changes', () => {
            resultsTableHelper.verifyColumnChooserCancelButtonRevertsChanges(visibleColumnsNamesClass, columnHeaderDataHidingArrowClass, columnHeaderHiddenIndicatorClass
                , columnChooserButtonPath, columnChooserColumnNamePath, columnChooserHideButtonsPath, columnChooserShowButtonsPath, columnChooserApplyButtonId)
        })
        it('Set Number Of Rows In Grid', () => {
            cy.wrap(null).then(() =>{
                prepareCountryListBoxesForPaginatorTests()
            }).then(() =>{
                resultsTableHelper.verifySettingNumberOfRowsInGrid()
            })
        })
        it('Grid Paginator Test', () => {
            cy.wrap(null).then(() =>{
                prepareCountryListBoxesForPaginatorTests()
            }).then(() =>{
                resultsTableHelper.verifyGridPaginator()
            })
        })
    })
})

function prepareCountryListBoxesForPaginatorTests()
{
    cy.wrap(null).then(() =>{
        managementPanelHelper.clickExporterToggleButton("Country", countryFacilityTogglePath, countryFacilityButtonPath)
    }).then(() =>{
        managementPanelHelper.clickImporterToggleButton("Country", countryFacilityTogglePath, countryFacilityButtonPath)
    }).then(() =>{
        managementPanelHelper.checkAllListItemsSearchBox(_exporterLabelName, exporterImporterLabelPath, countryBoxPath)
    }).then(() =>{
        managementPanelHelper.checkAllListItemsSearchBox(_importerLabelName, exporterImporterLabelPath, countryBoxPath)
    })
}