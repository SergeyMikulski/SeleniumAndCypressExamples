import { managementPanelHelper } from '../support/managementPanelHelper';
import { exportHelper } from '../support/exportHelper';

const _exporterLabelName = "Exporter:";
const _importerLabelName = "Importer:";

const exporterImporterLabelPath = 'strong.line-height-lg'
const countryFacilityTogglePath = 'country-facility-selector'
const countryFacilityButtonPath = 'cui-radio-group-element label'

const countryBoxPath = 'country-multiselect-view '

const countryItemsListPath = countryBoxPath + '.selections-tree li .text'

describe('Application Results Table Export', () => {
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
    
    describe('Application Results Table Export tests', () => {
        it('Application Results Table Export test', () => {
            cy.wrap(null).then(() =>{
                prepareCountryListBoxesForExportTests()
            }).then(() =>{
                verifyExportFromResultsTable()
            })
        })
    })
})

function verifyExportFromResultsTable(){
    var columnHeaderTextClass = '.ui-iggrid-headertext'
    var tableRowsPath = '.ui-iggrid-tablebody tr'

    var fileDate = getfileDate()

    var stringHeadersToCheck = ''
    var rowStringsToCheck = []
    var rowString = ''
    var filename = 'LNGTrade_' + fileDate + '.xlsx'
    
    cy.get(columnHeaderTextClass).each(columnHeader =>{
        stringHeadersToCheck += columnHeader.text().trim() + ','
    }).then(() =>{
        cy.get(tableRowsPath).each(row =>{
            cy.wrap(null).then(() =>{
                row.find('td').each((i, cell) =>{
                    rowString += cell.textContent.trim() + ','
                })
            }).then(() =>{
                console.log(rowString, '=rowString')
            }).then(() =>{
                rowStringsToCheck.push(rowString)
                rowString= ''
            })
        })
    }).then(() =>{
        triggerExport()
    }).then(() =>{
        cy.wait(2000)
    }).then(() =>{
        exportHelper.verifyXlsxContainsStringArray(filename, [stringHeadersToCheck])
        exportHelper.verifyXlsxContainsStringArray(filename, rowStringsToCheck)
    }).then(() =>{
        cy.clearDownloads()
    })
}

function triggerExport(){
    var exportButtonClass = '.btn-flat'

    cy.get(exportButtonClass).click()
}

function getfileDate() : string{
    var date = new Date()
    var year = date.getFullYear().toString()
    var month = date.getMonth() + 1
    var day = date.getDate()
    
    var newMonth
    var newDay
    if(month < 10){
        newMonth = '0' + month.toString()
    }
    else{
        newMonth = month.toString()
    }

    if(newDay < 10){
        newDay = '0' + day.toString()
    }
    else{
        newDay = day.toString()
    }
    var newDate = year + '-' + newMonth + '-' + newDay
    
    return newDate
}

function prepareCountryListBoxesForExportTests()
{
    cy.wrap(null).then(() =>{
        managementPanelHelper.clickExporterToggleButton("Country", countryFacilityTogglePath, countryFacilityButtonPath)
    }).then(() =>{
        managementPanelHelper.clickImporterToggleButton("Country", countryFacilityTogglePath, countryFacilityButtonPath)
    }).then(() =>{
        managementPanelHelper.unCheckAllListItemsSearchBox(_exporterLabelName, exporterImporterLabelPath, countryBoxPath)
    }).then(() =>{
        managementPanelHelper.unCheckAllListItemsSearchBox(_importerLabelName, exporterImporterLabelPath, countryBoxPath)
    }).then(() =>{
        managementPanelHelper.checkFirstItemSearchBox(_exporterLabelName, exporterImporterLabelPath, countryItemsListPath)
    }).then(() =>{
        managementPanelHelper.checkFirstItemSearchBox(_importerLabelName, exporterImporterLabelPath, countryItemsListPath)
    })
}