import { managementPanelHelper } from '../support/managementPanelHelper';

const _exporterLabelName = "Exporter:";
const _importerLabelName = "Importer:";

const exporterImporterLabelPath = 'strong.line-height-lg'
const countryFacilityTogglePath = 'country-facility-selector'
const countryFacilityButtonPath = 'cui-radio-group-element label'

const countryBoxPath = 'country-multiselect-view '
const facilityBoxPath = 'facility-multiselect-view '

const countryItemsListPath = countryBoxPath + '.selections-tree li .text'
const facilityItemsListPath = facilityBoxPath + '.selections-tree li li .text'

describe('Application Management Panel', () => {
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
    
    describe('Application Management Panel tests', () => {
        it('Management Panel Query Tab Labels Check', () => {
            managementPanelHelper.verifyManagementPanelQueryTabLabels(exporterImporterLabelPath)
        })
        it('Management Panel Query Tab Search Box Country Operation Test', () => {
            cy.wrap(null).then(() =>{
                prepareCountryListBoxesForTests()
            }).then(() =>{
                managementPanelHelper.verifyManagementPanelQueryTabSearchBoxSearchBoxOperation(_exporterLabelName, exporterImporterLabelPath, countryItemsListPath)
                managementPanelHelper.verifyManagementPanelQueryTabSearchBoxSearchBoxOperation(_importerLabelName, exporterImporterLabelPath, countryItemsListPath)
            })
        })
        it('Management Panel Query Tab Search Box Facility Operation Test', () => {
            cy.wrap(null).then(() =>{
                prepareFacilityListBoxesForTests()
            }).then(() =>{
                managementPanelHelper.verifyManagementPanelQueryTabSearchBoxSearchBoxOperation(_exporterLabelName, exporterImporterLabelPath, facilityItemsListPath)
                managementPanelHelper.verifyManagementPanelQueryTabSearchBoxSearchBoxOperation(_importerLabelName, exporterImporterLabelPath, facilityItemsListPath)
            })
        })
        it('Management Panel Legend Tab Labels Check', () => {
            cy.wrap(null).then(() =>{
                prepareCountryListBoxesForLegendTests()
            }).then(() =>{
                managementPanelHelper.chooseLegendTab()
            }).then(() =>{
                managementPanelHelper.verifyLegendTabLabels()
            })
        })
    })
})

function prepareCountryListBoxesForTests()
{
    cy.wrap(null).then(() =>{
        managementPanelHelper.clickExporterToggleButton("Country", countryFacilityTogglePath, countryFacilityButtonPath)
    }).then(() =>{
        managementPanelHelper.clickImporterToggleButton("Country", countryFacilityTogglePath, countryFacilityButtonPath)
    }).then(() =>{
        managementPanelHelper.unCheckAllListItemsSearchBox(_exporterLabelName, exporterImporterLabelPath, countryBoxPath)
    }).then(() =>{
        managementPanelHelper.unCheckAllListItemsSearchBox(_importerLabelName, exporterImporterLabelPath, countryBoxPath)
    })
}

function prepareFacilityListBoxesForTests()
{
    cy.wrap(null).then(() =>{
        managementPanelHelper.clickExporterToggleButton("Facility", countryFacilityTogglePath, countryFacilityButtonPath)
    }).then(() =>{
        managementPanelHelper.clickImporterToggleButton("Facility", countryFacilityTogglePath, countryFacilityButtonPath)
    }).then(() =>{
        managementPanelHelper.unCheckAllListItemsSearchBox(_exporterLabelName, exporterImporterLabelPath, facilityBoxPath)
    }).then(() =>{
        managementPanelHelper.unCheckAllListItemsSearchBox(_importerLabelName, exporterImporterLabelPath, facilityBoxPath)
    })
}

function prepareCountryListBoxesForLegendTests()
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