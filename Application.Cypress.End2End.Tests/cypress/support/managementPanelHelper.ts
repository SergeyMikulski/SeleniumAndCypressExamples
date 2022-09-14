import * as documentStream from 'cypress-document-stream';
import { eq } from 'cypress/types/lodash';

class ManagementPanelHelper{

    verifyManagementPanelQueryTabLabels(exporterImporterLabelPath: string){
        const spotCargoLabelName = "Spot Cargo Only";
        const reExportsLabelName = "Re-exports";
        const displayUnitsLabelName = "Display Units";
        const loadedUnloadedLabelName = "Loaded/Unloaded";
        const exporterLabelName = "Exporter:";
        const importerLabelName = "Importer:";

        var spotCargoLabelPath = '[for="spot-cargo"]'
        var reExportsLabelPath = '[for="re-export"]'
        var displayUnitsLabelPath = '[for="display-units"]'
        var loadedUnloadedLabelPath = '[for="loaded-unloaded"]'

        cy.get(spotCargoLabelPath).contains(spotCargoLabelName)
        cy.get(reExportsLabelPath).contains(reExportsLabelName)
        cy.get(displayUnitsLabelPath).contains(displayUnitsLabelName)
        cy.get(loadedUnloadedLabelPath).contains(loadedUnloadedLabelName)
        cy.get(exporterImporterLabelPath).eq(0).contains(exporterLabelName)
        cy.get(exporterImporterLabelPath).eq(1).contains(importerLabelName)
    }

    clickExporterToggleButton(buttonName: string, togglePath: string, buttonPath: string){
        cy.get(togglePath).eq(0).within(() =>{
            cy.get(buttonPath).contains(buttonName).click()
        }).then(() =>{
            cy.wait(2000)
        })
    }
    
    clickImporterToggleButton(buttonName: string, togglePath: string, buttonPath: string){
        cy.get(togglePath).eq(1).within(() =>{
            cy.get(buttonPath).contains(buttonName).click()
        }).then(() =>{
            cy.wait(2000)
        })
    }

    isExporterSearchBoxSelectAllElementChecked(filterLabelName: string, exporterImporterLabelPath: string, selectAllCheckboxCheckedPath: string){
        var isSelectedFlagChecked: boolean

        var length: number
        cy.get(exporterImporterLabelPath).contains(filterLabelName).parent().parent().within(body =>{
            length = body.find(selectAllCheckboxCheckedPath).length
        }).then(() =>{
            console.log(length, '=length')
            if(length > 0){
                isSelectedFlagChecked = true
            }
            else{
                isSelectedFlagChecked = false
            }
            //cy.log(isSelectedFlagChecked.toString(), '=isSelectedFlagChecked')
        }).then(() =>{
            cy.wrap(isSelectedFlagChecked).as('isSelectedFlagChecked')
        })
    }

    isExporterSearchBoxSelectAllElementPartiallyChecked(filterLabelName: string, exporterImporterLabelPath: string, selectAllCheckboxPartiallyCheckedPath: string){
        var isSelectedFlagPatiallyChecked: boolean

        var length: number
        cy.get(exporterImporterLabelPath).contains(filterLabelName).parent().parent().within(body =>{
            length = body.find(selectAllCheckboxPartiallyCheckedPath).length
        }).then(() =>{
            if(length > 0){
                isSelectedFlagPatiallyChecked = true
            }
            else{
                isSelectedFlagPatiallyChecked = false
            }
        }).then(() =>{
            cy.wrap(isSelectedFlagPatiallyChecked).as('isSelectedFlagPatiallyChecked')
        })
    }

    isSearchBoxSelectAllElementChecked(filterLabelName: string, exporterImporterLabelPath: string
        ,selectAllCheckboxCheckedPath: string, selectAllCheckboxPartiallyCheckedPath: string)
        {
            var checkBoxSelectionType: checkBoxCheckType

            this.isExporterSearchBoxSelectAllElementChecked(filterLabelName, exporterImporterLabelPath, selectAllCheckboxCheckedPath)            
            cy.get('@isSelectedFlagChecked').then(isSelectedFlagChecked =>{
                //console.log(isSelectedFlagChecked.toString(), '=isSelectedFlagChecked')
                if (isSelectedFlagChecked){
                    checkBoxSelectionType = checkBoxCheckType.Checked
                    //console.log('Checked')
                }
                else{
                    this.isExporterSearchBoxSelectAllElementPartiallyChecked(filterLabelName, exporterImporterLabelPath, selectAllCheckboxPartiallyCheckedPath)
                    cy.get('@isSelectedFlagPatiallyChecked').then(isSelectedFlagPartiallyChecked =>{
                        //console.log(isSelectedFlagPartiallyChecked.toString(), '=isSelectedFlagPartiallyChecked')
                        if (isSelectedFlagPartiallyChecked){
                            checkBoxSelectionType = checkBoxCheckType.PartiallyChecked
                            //console.log('PartiallyChecked')
                        }
                        else{
                            checkBoxSelectionType = checkBoxCheckType.UnChecked
                            //console.log('UnChecked')
                        }
                    })
                }
            }).then(() =>{
                cy.wrap(checkBoxSelectionType).as('checkBoxSelectionType')
            })
        }

    unCheckAllListItemsSearchBox(filterLabelName: string, exporterImporterLabelPath: string, countryFacilityBoxPath: string){
        var selectAllCheckboxPath = countryFacilityBoxPath + '.select-all input'
        var selectAllCheckboxCheckedPath = countryFacilityBoxPath + '.select-all span.checked'
        var selectAllCheckboxPartiallyCheckedPath = countryFacilityBoxPath + '.select-all.indeterminate'

        this.isSearchBoxSelectAllElementChecked(filterLabelName, exporterImporterLabelPath, selectAllCheckboxCheckedPath, selectAllCheckboxPartiallyCheckedPath)
        cy.get('@checkBoxSelectionType').then(checkBoxSelectionType =>{
            if(checkBoxSelectionType.toString() == checkBoxCheckType.Checked.toString()){
                cy.get(exporterImporterLabelPath).contains(filterLabelName).parent().parent().within(body =>{
                    body.find(selectAllCheckboxPath).trigger('click')
                })
            }

            if(checkBoxSelectionType.toString() == checkBoxCheckType.PartiallyChecked.toString()){
                cy.get(exporterImporterLabelPath).contains(filterLabelName).parent().parent().within(body =>{
                    body.find(selectAllCheckboxPath).trigger('click')
                }).then(() =>{
                    cy.wait(2000)
                }).then(() =>{
                    cy.get(exporterImporterLabelPath).contains(filterLabelName).parent().parent().within(body =>{
                        body.find(selectAllCheckboxPath).trigger('click')
                    })
                })
            }
        })
        .then(() =>{
            cy.wait(3000)
        })
    }

    checkAllListItemsSearchBox(filterLabelName: string, exporterImporterLabelPath: string, countryFacilityBoxPath: string){
        var selectAllCheckboxPath = countryFacilityBoxPath + '.select-all input'
        var selectAllCheckboxCheckedPath = countryFacilityBoxPath + '.select-all span.checked'
        var selectAllCheckboxPartiallyCheckedPath = countryFacilityBoxPath + '.select-all.indeterminate'

        this.isSearchBoxSelectAllElementChecked(filterLabelName, exporterImporterLabelPath, selectAllCheckboxCheckedPath, selectAllCheckboxPartiallyCheckedPath)
        cy.get('@checkBoxSelectionType').then(checkBoxSelectionType =>{
            if((checkBoxSelectionType.toString() == checkBoxCheckType.UnChecked.toString()) || 
            (checkBoxSelectionType.toString() == checkBoxCheckType.PartiallyChecked.toString())){
                cy.get(exporterImporterLabelPath).contains(filterLabelName).parent().parent().within(body =>{
                    body.find(selectAllCheckboxPath).trigger('click')
                })
            }
        })
        .then(() =>{
            cy.wait(2000)
        })
    }

    checkFirstItemSearchBox(filterLabelName: string, exporterImporterLabelPath: string, itemsListPath: string){
        cy.get(exporterImporterLabelPath).contains(filterLabelName).parent().parent().within(() =>{
            cy.get(itemsListPath).eq(0).click()
        }).then(() =>{
            cy.wait(2000)
        })
    }

    getSearchBoxItem(filterLabelName: string, itemNumber: number, exporterImporterLabelPath: string, itemsListPath: string){
        var searchBoxItemsListText: Array<string> = []
        var searchBoxItem: string

        cy.get(exporterImporterLabelPath).contains(filterLabelName).parent().parent().within(() =>{
            cy.get(itemsListPath).each((item) =>{
                searchBoxItemsListText.push(item.text())
            })
        }).then(() =>{
            searchBoxItem = searchBoxItemsListText[itemNumber]
            // cy.log(countryItem, '=countryItem initial')
        }).then(() =>{
            cy.wrap(searchBoxItem).as('getSearchBoxItemResult')
        })
    }

    isSearchBoxItemVisible(filterLabelName: string, itemNameToCheck: string, exporterImporterLabelPath: string, itemsListPath: string){
        var searchBoxItemsListText: Array<string> = []
        var isSearchBoxItemVisibleFlag: boolean

        cy.get(exporterImporterLabelPath).contains(filterLabelName).parent().parent().within(() =>{
            cy.get(itemsListPath).each((item) =>{
                searchBoxItemsListText.push(item.text())
            })
        }).then(() =>{
            if(searchBoxItemsListText.indexOf(itemNameToCheck) != -1){
                isSearchBoxItemVisibleFlag = true
            }
            else{
                isSearchBoxItemVisibleFlag = false
            }
        }).then(() =>{
            cy.wrap(isSearchBoxItemVisibleFlag).as('isSearchBoxItemVisibleFlag')
        })
    }

    typeSymbolsToSearchBox(filterLabelName: string, stringToType: string, exporterImporterLabelPath: string){
        var searchBoxInputPath = '.search-box input'
        cy.get(exporterImporterLabelPath).contains(filterLabelName).parent().parent().within(() =>{
            cy.get(searchBoxInputPath).type(stringToType).then(() =>{
                cy.wait(1000)
            })
        })
    }

    isSearchBoxButtonDisplayed(filterLabelName: string, buttonClass: string, exporterImporterLabelPath: string){
        var searchBoxButtonHiddenPath = '.search-box span' + buttonClass + '.hidden'

        var isSearchBoxButtonVisibleFlag: boolean

        cy.get(exporterImporterLabelPath).contains(filterLabelName).parent().parent().within(body =>{
            if(body.find(searchBoxButtonHiddenPath).length > 0){
                isSearchBoxButtonVisibleFlag = false
            }
            else{
                isSearchBoxButtonVisibleFlag = true
            }
        }).then(() =>{
            cy.wrap(isSearchBoxButtonVisibleFlag).as('isSearchBoxButtonVisible')
        })
    }

    removeButtonClick(filterLabelName: string, removeButtonClass: string, exporterImporterLabelPath: string){
        var searchBoxButtonPath = '.search-box span' + removeButtonClass

        cy.get(exporterImporterLabelPath).contains(filterLabelName).parent().parent().within(() =>{
            cy.get(searchBoxButtonPath).click().then(() =>{
                cy.wait(1000)
            })
        })
    }

    verifyManagementPanelQueryTabSearchBoxSearchBoxAfterRemoveButtonClick(filterLabelName: string, removeButtonClass: string, searchButtonClass: string
                                            ,exporterImporterLabelPath: string, firstCountryName: string, secondCountryName: string, itemsListPath: string){
        cy.wrap(null).then(() =>{
            this.removeButtonClick(filterLabelName, removeButtonClass, exporterImporterLabelPath)
        }).then(() =>{
            this.isSearchBoxButtonDisplayed(filterLabelName, searchButtonClass, exporterImporterLabelPath)
            cy.get('@isSearchBoxButtonVisible').should('be.true')
            .then(() =>{
                this.isSearchBoxItemVisible(filterLabelName, firstCountryName, exporterImporterLabelPath, itemsListPath)
                cy.get('@isSearchBoxItemVisibleFlag').then(isSearchBoxItemVisibleFlag =>{
                    assert(isSearchBoxItemVisibleFlag.toString() == 'true', 'items list should contain ' + firstCountryName + 'in searchBox ' + filterLabelName)
                })
            })
            .then(() =>{
                this.isSearchBoxItemVisible(filterLabelName, secondCountryName, exporterImporterLabelPath, itemsListPath)
                cy.get('@isSearchBoxItemVisibleFlag').then(isSearchBoxItemVisibleFlag =>{
                    assert(isSearchBoxItemVisibleFlag.toString() == 'true', 'items list should contain ' + secondCountryName + 'in searchBox ' + filterLabelName)
                })
            })
            .then(() =>{
                this.isSearchBoxButtonDisplayed(filterLabelName, searchButtonClass, exporterImporterLabelPath)
                cy.get('@isSearchBoxButtonVisible').should('be.true')
                .then(() =>{
                    this.isSearchBoxButtonDisplayed(filterLabelName, removeButtonClass, exporterImporterLabelPath)
                    cy.get('@isSearchBoxButtonVisible').should('be.false')
                })
            })
        })
    }

    verifyManagementPanelQueryTabSearchBoxSearchBoxOperation(filterLabelName: string, exporterImporterLabelPath: string, itemsListPath: string){
        var firstCountryName: string
        var secondCountryName: string
        var searchButtonClass = '.search'
        var removeButtonClass = '.remove'
        
        this.isSearchBoxButtonDisplayed(filterLabelName, searchButtonClass, exporterImporterLabelPath)
        cy.get('@isSearchBoxButtonVisible').should('be.true')
        .then(() =>{
            this.isSearchBoxButtonDisplayed(filterLabelName, removeButtonClass, exporterImporterLabelPath)
            cy.get('@isSearchBoxButtonVisible').should('be.false')
            .then(() =>{
                this.getSearchBoxItem(filterLabelName, 0, exporterImporterLabelPath, itemsListPath)
                cy.get('@getSearchBoxItemResult').then(searchBoxItem =>{
                    firstCountryName = searchBoxItem.toString()
                    // cy.log(searchBoxItem.toString(), '=countryItem123')
                }).then(() =>{
                    this.getSearchBoxItem(filterLabelName, 1, exporterImporterLabelPath, itemsListPath)
                    cy.get('@getSearchBoxItemResult').then(searchBoxItem =>{
                        secondCountryName = searchBoxItem.toString()
                        // cy.log(searchBoxItem.toString(), '=countryItem123123')
                    })
                }).then(() =>{
                    this.typeSymbolsToSearchBox(filterLabelName, firstCountryName, exporterImporterLabelPath)
                }).then(() =>{
                    this.isSearchBoxItemVisible(filterLabelName, firstCountryName, exporterImporterLabelPath, itemsListPath)
                    cy.get('@isSearchBoxItemVisibleFlag').then(isSearchBoxItemVisibleFlag =>{
                        assert(isSearchBoxItemVisibleFlag.toString() == 'true', 'items list should contain ' + firstCountryName + 'in searchBox ' + filterLabelName)
                    })
                })
                .then(() =>{
                    this.isSearchBoxItemVisible(filterLabelName, secondCountryName, exporterImporterLabelPath, itemsListPath)
                    cy.get('@isSearchBoxItemVisibleFlag').then(isSearchBoxItemVisibleFlag =>{
                        assert(isSearchBoxItemVisibleFlag.toString() == 'false', 'items list should not contain ' + secondCountryName + 'in searchBox ' + filterLabelName)
                    })
                })
                .then(() =>{
                    this.isSearchBoxButtonDisplayed(filterLabelName, searchButtonClass, exporterImporterLabelPath)
                    cy.get('@isSearchBoxButtonVisible').should('be.false')
                    .then(() =>{
                        this.isSearchBoxButtonDisplayed(filterLabelName, removeButtonClass, exporterImporterLabelPath)
                        cy.get('@isSearchBoxButtonVisible').should('be.true')
                    })
                })
                .then(() =>{
                    this.verifyManagementPanelQueryTabSearchBoxSearchBoxAfterRemoveButtonClick(
                        filterLabelName, removeButtonClass, searchButtonClass,exporterImporterLabelPath, firstCountryName, secondCountryName, itemsListPath)
                })
            })
        })
    }

    chooseLegendTab(){
        var tabPath = 'cui-tab-nav'
        cy.get(tabPath).contains('Legend').click().then(() =>{
            cy.wait(2000)
        })
    }

    verifyLegendTabLabels(){
        const tradeVolumesLabelName = "Trade Volumes";
        const typeOfCountryLabelName = "Type of Country";
        const typeOfFacilityLabelName = "Dominant Type of Facility";
        const liquefaction = "Liquefaction";
        const regasification = "Regasification";
        const dualPurpose = "Dual purpose";
        const exporter = "Exporter";
        const importer = "Importer";
        const importsExports = "Imports/Exports within borders";
        const numberOfTypeOfFacilityItemsInList = 3;
        const numberOfTypeOfCountryItemsInList = 3;

        var legendLabelsPath = '.line-height-lg'

        cy.get(legendLabelsPath).eq(0).contains(tradeVolumesLabelName)
        cy.get(legendLabelsPath).eq(1).contains(typeOfCountryLabelName)
        cy.get(legendLabelsPath).eq(2).contains(typeOfFacilityLabelName)
        
        cy.get(legendLabelsPath).contains(typeOfCountryLabelName).parent().parent().within(() =>{
            cy.get('img[title="' + exporter + '"]').should('exist')
            cy.get('span').contains(exporter)

            cy.get('img[title="' + importer + '"]').should('exist')
            cy.get('span').contains(importer)

            cy.get('img[title="' + importsExports + '"]').should('exist')
            cy.get('span').contains(importsExports)

            cy.get('span').its('length').should('eq', numberOfTypeOfCountryItemsInList)
        })

        cy.get(legendLabelsPath).contains(typeOfFacilityLabelName).parent().parent().within(() =>{
            cy.get('img[title="' + liquefaction + '"]').should('exist')
            cy.get('span').contains(liquefaction)

            cy.get('img[title="' + regasification + '"]').should('exist')
            cy.get('span').contains(regasification)

            cy.get('img[title="' + dualPurpose + '"]').should('exist')
            cy.get('span').contains(dualPurpose)

            cy.get('span').its('length').should('eq', numberOfTypeOfFacilityItemsInList)
        })
    }
}

enum checkBoxCheckType{
    Checked,
    PartiallyChecked,
    UnChecked
}

export const managementPanelHelper = new ManagementPanelHelper()