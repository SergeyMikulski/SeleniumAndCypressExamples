import * as documentStream from 'cypress-document-stream';
import { eq } from 'cypress/types/lodash';

class ResultsTableHelper{
    verifyExpandCollapseResultsTable(){
        var resultsTableCollapsedClass = '.box-header.collapsed'
        var arrowClass = '.toggle-arrow'
        var resultsTableHeaderTitleClass = '.box-header-title'

        const resultsTableHeaderTitleTextExpected = 'Application Results'

        cy.get(resultsTableHeaderTitleClass).contains(resultsTableHeaderTitleTextExpected)
        cy.get(resultsTableCollapsedClass).should('not.exist').then(() =>{
            cy.get(arrowClass).click().then(() =>{
                cy.get(resultsTableCollapsedClass).should('exist')
            })
        })
    }

    verifyColumnsSorting(){
        var sortableColumnClass = '.ui-iggrid-sortableheader'
        var sortedAscColumnClass = '.ui-iggrid-sortableheader.ui-iggrid-colheaderasc'
        var sortedDescColumnClass = '.ui-iggrid-sortableheader.ui-iggrid-colheaderdesc'

        cy.get(sortableColumnClass).each(column =>{
            cy.wrap(column).filter(sortedAscColumnClass).should('not.exist')
            cy.wrap(column).filter(sortedDescColumnClass).should('not.exist')
        }).then(() =>{
            cy.get(sortableColumnClass).each(column =>{
                cy.wrap(column).click().then(() =>{
                    cy.wrap(column).filter(sortedAscColumnClass).should('exist')
                    cy.wrap(column).filter(sortedDescColumnClass).should('not.exist')
                }).then(() =>{
                    cy.wrap(column).click().then(() =>{
                        cy.wrap(column).filter(sortedAscColumnClass).should('not.exist')
                        cy.wrap(column).filter(sortedDescColumnClass).should('exist')
                    })
                })
            })
        })
    }

    verifyColumnHiding(visibleColumnsNamesClass: string, columnHeaderDataHidingArrowClass: string){
        cy.get(visibleColumnsNamesClass).its('length').then(initialColumnsNumber =>{
            cy.get(columnHeaderDataHidingArrowClass).eq(0).click().then(() =>{
                cy.get(visibleColumnsNamesClass).its('length').should('be.lessThan', initialColumnsNumber)
            })
        })
    }

    verifyColumnShowingByColumnMenu(visibleColumnsNamesClass: string, columnHeaderDataHidingArrowClass: string
        , columnHeaderHiddenIndicatorClass: string, columnMenuHiddenItemsPath: string){
        var initialColumnsList = []
        var hiddenColumn = ''

        cy.get(visibleColumnsNamesClass).each(column =>{
            initialColumnsList.push(column.text().trim())
        }).then(() =>{
            cy.get(columnHeaderDataHidingArrowClass).eq(0).click().then(() =>{
                hiddenColumn = initialColumnsList[0]
                cy.get(visibleColumnsNamesClass).its('length').should('be.lessThan', initialColumnsList.length)
            }).then(() =>{
                cy.get(columnHeaderHiddenIndicatorClass).click({force: true}).then(() =>{
                    cy.get(columnMenuHiddenItemsPath).contains(hiddenColumn).click()
                })
            }).then(() =>{
                cy.get(visibleColumnsNamesClass).each(column =>{
                    assert.isTrue(initialColumnsList.indexOf(column.text().trim()) > -1, `column ${column.text().trim()} should be visble`)
                })
            })
        })
    }

    columnChooserResetButtonClick(hiddenColumn: string, columnChooserColumnNamePath: string, columnChooserHideButtonsPath: string
        , columnChooserShowButtonsPath: string, columnChooserApplyButtonId: string){
        var resetButtonId = '#grid_xa1_hiding_modalDialog_reset_button'
        var columnsNumber = 7

        cy.get(columnChooserColumnNamePath).contains(hiddenColumn).parent().find(columnChooserShowButtonsPath).should('exist').then(() =>{
            cy.get(resetButtonId).click().then(() =>{
                cy.get(columnChooserHideButtonsPath).should('have.length', columnsNumber).then(() =>{
                    cy.get(columnChooserApplyButtonId).click()
                })
            })
        })
    }

    verifyColumnChooserResetButtonShowsAllColumns(visibleColumnsNamesClass: string, columnHeaderDataHidingArrowClass: string
        , columnHeaderHiddenIndicatorClass: string, columnChooserButtonPath: string, columnChooserColumnNamePath: string
        , columnChooserHideButtonsPath: string, columnChooserShowButtonsPath: string, columnChooserApplyButtonId: string){
        var initialColumnsList = []
        var hiddenColumn = ''

        cy.get(visibleColumnsNamesClass).each(column =>{
            initialColumnsList.push(column.text().trim())
        }).then(() =>{
            cy.get(columnHeaderDataHidingArrowClass).eq(0).click().then(() =>{
                hiddenColumn = initialColumnsList[0]
                cy.get(visibleColumnsNamesClass).its('length').should('be.lessThan', initialColumnsList.length)
            }).then(() =>{
                cy.get(columnHeaderHiddenIndicatorClass).click({force: true}).then(() =>{
                    cy.get(columnChooserButtonPath).click().then(() =>{
                        this.columnChooserResetButtonClick(hiddenColumn, columnChooserColumnNamePath, columnChooserHideButtonsPath
                            , columnChooserShowButtonsPath, columnChooserApplyButtonId)
                    })
                })
            }).then(() =>{
                cy.get(visibleColumnsNamesClass).each(column =>{
                    assert.isTrue(initialColumnsList.indexOf(column.text().trim()) > -1, `column ${column.text().trim()} should be visble`)
                })
            })
        })
    }

    columnChooserShowHideColumns(initiallyHiddenColumn: string, columnToHideInColumnChooser: string, columnChooserColumnNamePath: string, columnChooserHideButtonsPath: string
        , columnChooserShowButtonsPath: string, columnChooserApplyButtonId: string, isApplyButtonClick: boolean){
            var columnChooserCancelButtonId = '#grid_xa1_hiding_modalDialog_footer_buttoncancel'

        cy.get(columnChooserColumnNamePath).contains(initiallyHiddenColumn).parent().find(columnChooserShowButtonsPath).should('exist').then(() =>{
            cy.get(columnChooserColumnNamePath).contains(columnToHideInColumnChooser).parent().find(columnChooserHideButtonsPath).should('exist').then(() =>{
                cy.get(columnChooserColumnNamePath).contains(initiallyHiddenColumn).parent().find(columnChooserShowButtonsPath).click()
                cy.get(columnChooserColumnNamePath).contains(columnToHideInColumnChooser).parent().find(columnChooserHideButtonsPath).click()
            }).then(() =>{
                if(isApplyButtonClick){
                    cy.get(columnChooserApplyButtonId).click()
                }
                else{
                    cy.get(columnChooserCancelButtonId).click()
                }
            })
        })
    }

    verifyColumnChooserShowHideColumns(visibleColumnsNamesClass: string, columnHeaderDataHidingArrowClass: string
        , columnHeaderHiddenIndicatorClass: string, columnChooserButtonPath: string, columnChooserColumnNamePath: string
        , columnChooserHideButtonsPath: string, columnChooserShowButtonsPath: string, columnChooserApplyButtonId: string){
            var initialColumnsList = []
            var resultColumnsList = []
            var initiallyHiddenColumn = ''
            var columnToHideInColumnChooser = ''
            var isApplyButtonClick = true
    
        cy.get(visibleColumnsNamesClass).each(column =>{
            initialColumnsList.push(column.text().trim())
        }).then(() =>{
            cy.get(columnHeaderDataHidingArrowClass).eq(0).click().then(() =>{
                initiallyHiddenColumn = initialColumnsList[0]
                columnToHideInColumnChooser = initialColumnsList[1]
                cy.get(visibleColumnsNamesClass).its('length').should('be.lessThan', initialColumnsList.length)
            }).then(() =>{
                cy.get(columnHeaderHiddenIndicatorClass).click({force: true}).then(() =>{
                    cy.get(columnChooserButtonPath).click().then(() =>{
                        this.columnChooserShowHideColumns(initiallyHiddenColumn, columnToHideInColumnChooser, columnChooserColumnNamePath, columnChooserHideButtonsPath
                            , columnChooserShowButtonsPath, columnChooserApplyButtonId, isApplyButtonClick)
                    })
                })
            }).then(() =>{
                cy.get(visibleColumnsNamesClass).each(column =>{
                    resultColumnsList.push(column.text().trim())
                }).then(() =>{
                    assert.isTrue(resultColumnsList.indexOf(initiallyHiddenColumn) > -1, `column ${initiallyHiddenColumn} should be visble`)
                    assert.isTrue(resultColumnsList.indexOf(columnToHideInColumnChooser) == -1, `column ${columnToHideInColumnChooser} should not be visble`)
                })
            })
        })
    }

    verifyColumnChooserCancelButtonRevertsChanges(visibleColumnsNamesClass: string, columnHeaderDataHidingArrowClass: string
        , columnHeaderHiddenIndicatorClass: string, columnChooserButtonPath: string, columnChooserColumnNamePath: string
        , columnChooserHideButtonsPath: string, columnChooserShowButtonsPath: string, columnChooserApplyButtonId: string){
            var initialColumnsList = []
            var resultColumnsList = []
            var initiallyHiddenColumn = ''
            var columnToHideInColumnChooser = ''
            var isApplyButtonClick = false
    
        cy.get(visibleColumnsNamesClass).each(column =>{
            initialColumnsList.push(column.text().trim())
        }).then(() =>{
            cy.get(columnHeaderDataHidingArrowClass).eq(0).click().then(() =>{
                initiallyHiddenColumn = initialColumnsList[0]
                columnToHideInColumnChooser = initialColumnsList[1]
                cy.get(visibleColumnsNamesClass).its('length').should('be.lessThan', initialColumnsList.length)
            }).then(() =>{
                cy.get(columnHeaderHiddenIndicatorClass).click({force: true}).then(() =>{
                    cy.get(columnChooserButtonPath).click().then(() =>{
                        this.columnChooserShowHideColumns(initiallyHiddenColumn, columnToHideInColumnChooser, columnChooserColumnNamePath, columnChooserHideButtonsPath
                            , columnChooserShowButtonsPath, columnChooserApplyButtonId, isApplyButtonClick)
                    })
                })
            }).then(() =>{
                cy.get(visibleColumnsNamesClass).each(column =>{
                    resultColumnsList.push(column.text().trim())
                }).then(() =>{
                    assert.isTrue(resultColumnsList.indexOf(initiallyHiddenColumn) == -1, `column ${initiallyHiddenColumn} should not be visble`)
                    assert.isTrue(resultColumnsList.indexOf(columnToHideInColumnChooser) > -1, `column ${columnToHideInColumnChooser} should be visble`)
                })
            })
        })
    }

    verifySettingNumberOfRowsInGrid(){
        var rowsNumberPath = '[data-localeid="ariaNumericEditorFieldLabel"]'
        var rowsNumberDropDownArrow = '.ui-igedit-buttonimage'
        var rowsDropDownMenuPath = '.ui-igedit-listitem'
        var rowDropDownSelected = rowsDropDownMenuPath + '.ui-igedit-listitemselected'
        var gridRowsPath = 'tbody [role="row"]'

        var initialGridRowsNumberExpected = 10
        var newGridRowsNumberExpected = 5
        cy.get(rowsNumberPath).parent().find('input').eq(1).invoke('attr', 'value').should('eq', initialGridRowsNumberExpected.toString()).then(() =>{
            cy.get(gridRowsPath).its('length').should('eq', initialGridRowsNumberExpected).then(() =>{
                cy.get(rowsNumberDropDownArrow).click().then(() =>{
                    cy.get(rowDropDownSelected).contains(initialGridRowsNumberExpected.toString())
                    .then(() =>{
                        cy.get(rowsDropDownMenuPath).contains(newGridRowsNumberExpected).click().then(() =>{
                            cy.wait(5000).then(() =>{
                                cy.get(gridRowsPath).its('length').should('eq', newGridRowsNumberExpected)
                                cy.get(rowsNumberPath).parent().find('input').eq(1).invoke('attr', 'value').should('eq', newGridRowsNumberExpected.toString())
                            })
                        })
                    })
                })
            })
        })
    }

    verifyGridPaginator(){
        var rowsCounterId = '#grid_xa1_pager_label'
        var goToFirstpageClass = '.ui-iggrid-firstpage'
        var goToLastPageClass = '.ui-iggrid-lastpage'
        var previousPageClass = '.ui-iggrid-prevpage'
        var nextPageClass = '.ui-iggrid-nextpage'
        var currentPageClass = '.ui-iggrid-pagelinkcurrent'
        var unchosenPagesClass = '.ui-iggrid-pagelink'

        var waitAfterClickTime = 2000
        var waitAfterClickTimeLastPage = 10000

        cy.get(unchosenPagesClass).contains('3').click().then(() =>{
            cy.wait(waitAfterClickTime)
        }).then(() =>{
            this.getSelectedPageNumberShownFromRecord(rowsCounterId)
            cy.get('@pageNumberFromString').should('eq', 3)
            cy.get(currentPageClass).should('have.text', '3')
        }).then(() =>{
            cy.get(nextPageClass).click().then(() =>{
                cy.wait(waitAfterClickTime)
            }).then(() =>{
                this.getSelectedPageNumberShownFromRecord(rowsCounterId)
                cy.get('@pageNumberFromString').should('eq', 4)
                cy.get(currentPageClass).should('have.text', '4')
            })
        }).then(() =>{
            cy.get(previousPageClass).click().then(() =>{
                cy.wait(waitAfterClickTime)
            }).then(() =>{
                this.getSelectedPageNumberShownFromRecord(rowsCounterId)
                cy.get('@pageNumberFromString').should('eq', 3)
                cy.get(currentPageClass).should('have.text', '3')
            })
        }).then(() =>{
            cy.get(goToLastPageClass).click().then(() =>{
                cy.wait(waitAfterClickTimeLastPage)
            }).then(() =>{
                cy.get(currentPageClass).then(currentPage =>{
                    this.getLastPageNumberShownFromRecord(rowsCounterId)
                    cy.get('@lastPageNumberFromString').should('eq', parseInt(currentPage.text().trim()))
                })
            })
        }).then(() =>{
            cy.get(goToFirstpageClass).click().then(() =>{
                cy.wait(waitAfterClickTime)
            }).then(() =>{
                this.getSelectedPageNumberShownFromRecord(rowsCounterId)
                cy.get('@pageNumberFromString').should('eq', 1)
                cy.get(currentPageClass).should('have.text', '1')
            })
        })
    }

    getSelectedPageNumberShownFromRecord(rowsCounterId: string){
        var pageNumberFromString: number

        cy.get(rowsCounterId).then(record =>{
            var recordsNumberString = record.text().trim()
            var recordFormat =  recordsNumberString.slice(recordsNumberString.indexOf('-') + 1, recordsNumberString.indexOf('of')).trim()
            pageNumberFromString = parseInt(recordFormat) / 10
        }).then(() =>{
            cy.wrap(pageNumberFromString).as('pageNumberFromString')
        })
    }

    getLastPageNumberShownFromRecord(rowsCounterId: string){
        var lastPageNumberFromString: number

        cy.get(rowsCounterId).then(record =>{
            var recordsNumberString = record.text().trim()
            var recordFormat = recordsNumberString.slice(0, recordsNumberString.indexOf('-')).trim()
            lastPageNumberFromString = ((parseInt(recordFormat) - 1) / 10) + 1
        }).then(() =>{
            cy.wrap(lastPageNumberFromString).as('lastPageNumberFromString')
        })
    }
}

export const resultsTableHelper = new ResultsTableHelper()