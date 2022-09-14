import * as documentStream from 'cypress-document-stream';
import { eq } from 'cypress/types/lodash';

class CalendarHelper{
    initialSetCalendar(minDateClass: string, maxDateClass: string, openCalendarButtonPath: string
        , fromCalendarId: string, toCalendarId: string, calendarHeaderDateFieldClass: string, calendarCellsClass: string){
        var newMinDate
        var newMaxDate
        
        cy.get(minDateClass).then(minDate =>{
            newMinDate = new Date(minDate.text())
            newMinDate.setDate(newMinDate.getDate() + 3)
            newMinDate.setMonth(newMinDate.getMonth() + 2)
            newMinDate.setFullYear(newMinDate.getFullYear() + 1)
        }).then(() =>{
            cy.get(maxDateClass).then(maxDate =>{
                newMaxDate = new Date(maxDate.text())
                newMaxDate.setDate(newMaxDate.getDate() - 3)
                newMaxDate.setMonth(newMaxDate.getMonth() - 2)
                newMaxDate.setFullYear(newMaxDate.getFullYear() - 1)
            })
        }).then(() =>{
            cy.get(fromCalendarId).parent().within(() =>{
                cy.get(openCalendarButtonPath).click().then(() =>{
                    cy.wrap(Cypress.$('body')).within(() =>{
                        this.setCalendarDate(newMinDate, calendarHeaderDateFieldClass, calendarCellsClass)
                    })
                })
            })
        }).then(() =>{
            cy.get(toCalendarId).parent().within(() =>{
                cy.get(openCalendarButtonPath).click().then(() =>{
                    cy.wrap(Cypress.$('body')).within(() =>{
                        this.setCalendarDate(newMaxDate, calendarHeaderDateFieldClass, calendarCellsClass)
                    })
                })
            })
        }).then(() =>{
            cy.wrap(newMinDate).as('minDate')
            cy.wrap(newMaxDate).as('maxDate')
        })
    }

    setCalendarDate(date: Date, calendarHeaderDateFieldClass: string, calendarCellsClass: string){
        var year = date.getFullYear()
        var month = date.toLocaleString('default', { month: 'short' }).toUpperCase()
        var day = date.getDate()
        cy.wrap(null).then(() =>{
            this.chooseYear(year, calendarCellsClass, calendarHeaderDateFieldClass)
        }).then(() =>{
            this.chooseMonth(month, calendarCellsClass)
        }).then(() =>{
            this.chooseDay(day.toString(), calendarCellsClass)
        })
    }

    chooseDay(day: string, calendarCellsClass: string){
        cy.get(calendarCellsClass).contains(day).click()
    }

    chooseMonth(month: string, calendarCellsClass: string){
        cy.get(calendarCellsClass).contains(month).click()
    }

    chooseYear(year: number, calendarCellsClass: string, calendarHeaderDateFieldClass: string){
        var calendarCellsValues = []
        cy.get(calendarHeaderDateFieldClass).click().then(() =>{
            cy.get(calendarCellsClass).each(value =>{
                calendarCellsValues.push(value.text().trim())
            }).then(() =>{
                if(calendarCellsValues.indexOf(year.toString()) != -1){
                    cy.get(calendarCellsClass).contains(year.toString()).click()        
                }
                else{
                    cy.wrap(null).then(() =>{
                        this.getDateRangeNeeded(year, calendarCellsClass)
                    }).then(() => {
                        cy.get(calendarCellsClass).contains(year.toString()).click()
                    })
                }
            })
        })
    }

    getDateRangeNeeded(year: number, calendarCellsClass: string){
        var calendarHeaderLeftArrowPath = '[type="chevron-left"]'
        var calendarHeaderRightArrowPath = '[type="chevron-right"]'
        this.getMaxDateAvailabe(calendarCellsClass)
        cy.get('@maxDateAvailable').then(maxDateAvailable =>{
            if(parseInt(maxDateAvailable.toString()) < year){
                cy.get(calendarHeaderRightArrowPath).click()
            }
        }).then(() =>{
            this.getMinDateAvailabe(calendarCellsClass)
            cy.get('@minDateAvailable').then(minDateAvailable =>{
                if(parseInt(minDateAvailable.toString()) > year){
                    cy.get(calendarHeaderLeftArrowPath).click()
                }
            })
        })
    }

    getMaxDateAvailabe(calendarCellsClass: string){
        var year = 0
        cy.get(calendarCellsClass).each(cell =>{
            if(parseInt(cell.text()) > year){
                year = parseInt(cell.text())
            }
        }).then(() =>{
            cy.wrap(year).as('maxDateAvailable')
        })
    }

    getMinDateAvailabe(calendarCellsClass: string){
        var year = 10000
        cy.get(calendarCellsClass).each(cell =>{
            if(parseInt(cell.text()) < year){
                year = parseInt(cell.text())
            }
        }).then(() =>{
            cy.wrap(year).as('minDateAvailable')
        })
    }

    getCalendarFieldDate(calendarId: string){
        cy.get(calendarId).invoke('val').as('text').then(value =>{
            cy.wrap(value).as('calendarFieldDate')
        })
    }

    getTimeSliderTooltipDate(tooltipId: string){
        cy.get(tooltipId).then(tooltip =>{
            cy.wrap(tooltip.text()).as('tooltipDate')
        })
    }

    getCalendarPopUpAppliedDate(calendarHeaderDateFieldClass: string){
        var calendarSelectedCellClass = '.cui-calendar-body-selected'

        var monthYear: string
        var day: string
        var dateResult

        cy.get(calendarHeaderDateFieldClass).then(monthYearValue =>{
            monthYear = monthYearValue.text().trim()
        }).then(() =>{
            cy.get(calendarSelectedCellClass).then(dayValue =>{
                day = dayValue.text().trim()
            })
        }).then(() =>{
            dateResult = day + ' ' + monthYear
        }).then(() =>{
            cy.wrap(dateResult).as('calendarPopUpDate')
        })
    }

    changeYearInCalendarDate(fromCalendarId: string, toCalendarId: string, openCalendarButtonPath: string, calendarHeaderDateFieldClass: string, calendarCellsClass: string){
        var newMinDate
        var newMaxDate

        this.getCalendarFieldDate(fromCalendarId)
        cy.get('@calendarFieldDate').then(calendarFieldDate =>{
            newMinDate = new Date(calendarFieldDate.toString())
            newMinDate.setDate(newMinDate.getDate() + 3)
            newMinDate.setMonth(newMinDate.getMonth() + 2)
            newMinDate.setFullYear(newMinDate.getFullYear() + 1)
        }).then(() =>{
            this.getCalendarFieldDate(toCalendarId)
            cy.get('@calendarFieldDate').then(calendarFieldDate =>{
                newMaxDate = new Date(calendarFieldDate.toString())
                newMaxDate.setDate(newMaxDate.getDate() - 3)
                newMaxDate.setMonth(newMaxDate.getMonth() - 2)
                newMaxDate.setFullYear(newMaxDate.getFullYear() - 1)
            })
        }).then(() =>{
            cy.get(fromCalendarId).parent().within(() =>{
                cy.get(openCalendarButtonPath).click().then(() =>{
                    cy.wrap(Cypress.$('body')).within(() =>{
                        this.setCalendarDate(newMinDate, calendarHeaderDateFieldClass, calendarCellsClass)
                    })
                })
            })
        }).then(() =>{
            cy.get(toCalendarId).parent().within(() =>{
                cy.get(openCalendarButtonPath).click().then(() =>{
                    cy.wrap(Cypress.$('body')).within(() =>{
                        this.setCalendarDate(newMaxDate, calendarHeaderDateFieldClass, calendarCellsClass)
                    })
                })
            })
        }).then(() =>{
            cy.wrap(newMinDate).as('minDate')
            cy.wrap(newMaxDate).as('maxDate')
        })
    }

    verifyDatesInCalendarAndTimeSliderCorrect(fromCalendarId: string, toCalendarId: string, fromTimeSliderTooltipId: string, toTimeSliderTooltipId: string){
        cy.get('@minDate').then(minDate =>{
            this.getCalendarFieldDate(fromCalendarId)
            cy.get('@calendarFieldDate').then(calendarFieldDate =>{
                var minDateFormat = new Date(minDate.toString())
                var calendarFieldDateFormat = new Date(calendarFieldDate.toString())
                // console.log(minDateFormat.toDateString(), '=minDateFormat')
                // console.log(calendarFieldDateFormat.toDateString(), '=tooltipDateFormat')
                assert.isTrue(minDateFormat.toDateString() == calendarFieldDateFormat.toDateString())
            }).then(() =>{
                this.getTimeSliderTooltipDate(fromTimeSliderTooltipId)
                cy.get('@tooltipDate').then(tooltipDate =>{
                    var minDateFormat = new Date(minDate.toString())
                    var tooltipDateFormat = new Date(tooltipDate.toString())
                    // console.log(minDateFormat.toDateString(), '=minDateFormat')
                    // console.log(tooltipDateFormat.toDateString(), '=tooltipDateFormat')
                    assert.isTrue(minDateFormat.toDateString() == tooltipDateFormat.toDateString())
                })
            })
        }).then(() =>{
            cy.get('@maxDate').then(maxDate =>{
                this.getCalendarFieldDate(toCalendarId)
                cy.get('@calendarFieldDate').then(calendarFieldDate =>{
                    var maxDateFormat = new Date(maxDate.toString())
                    var calendarFieldDateFormat = new Date(calendarFieldDate.toString())
                    assert.isTrue(maxDateFormat.toDateString() == calendarFieldDateFormat.toDateString())
                }).then(() =>{
                    this.getTimeSliderTooltipDate(toTimeSliderTooltipId)
                    cy.get('@tooltipDate').then(tooltipDate =>{
                        var maxDateFormat = new Date(maxDate.toString())
                        var tooltipDateFormat = new Date(tooltipDate.toString())
                        assert.isTrue(maxDateFormat.toDateString() == tooltipDateFormat.toDateString())
                    })
                })
            })
        })
    }

    dragAndDropTooltips(){
        var leftTooltipPath = '[aria-describedby="cui-tooltip-1"]'
        var rightTooltipPath = '[aria-describedby="cui-tooltip-2"]'

        cy.get(leftTooltipPath)
        .trigger('mousedown', { which: 1 })
        .trigger('mousemove', { clientX: 10 })
        .trigger('mouseup', { force: true })
        .then(() =>{
            cy.get(rightTooltipPath)
            .trigger('mousedown', { which: 1 })
            .trigger('mousemove', { clientX: 15 })
            .trigger('mouseup', { force: true })
        })
    }

    dragAndDropGrabButton(pixelNumber: number){
        var grabButtonClass = '.grab-range'

        cy.get(grabButtonClass)
        .trigger('mousedown', { which: 1 })
        .trigger('mousemove', { clientX: pixelNumber })
        .trigger('mouseup', { force: true })
    }

    verifyDatesInCalendarAndTimeSliderCoinside(fromCalendarId: string, toCalendarId: string, fromTimeSliderTooltipId: string, toTimeSliderTooltipId: string){
        var calendarFieldDateFormat = new Date()
        this.getCalendarFieldDate(fromCalendarId)
        cy.get('@calendarFieldDate').then(calendarFieldDate =>{
            calendarFieldDateFormat = new Date(calendarFieldDate.toString())
            // console.log(minDateFormat.toDateString(), '=minDateFormat')
            // console.log(calendarFieldDateFormat.toDateString(), '=tooltipDateFormat')
        }).then(() =>{
            this.getTimeSliderTooltipDate(fromTimeSliderTooltipId)
            cy.get('@tooltipDate').then(tooltipDate =>{
                var tooltipDateFormat = new Date(tooltipDate.toString())
                // console.log(minDateFormat.toDateString(), '=minDateFormat')
                // console.log(tooltipDateFormat.toDateString(), '=tooltipDateFormat')
                assert.isTrue(calendarFieldDateFormat.toDateString() == tooltipDateFormat.toDateString())
            })
        }).then(() =>{
            this.getCalendarFieldDate(toCalendarId)
            cy.get('@calendarFieldDate').then(calendarFieldDate =>{
                calendarFieldDateFormat = new Date(calendarFieldDate.toString())
            }).then(() =>{
                this.getTimeSliderTooltipDate(toTimeSliderTooltipId)
                cy.get('@tooltipDate').then(tooltipDate =>{
                    var tooltipDateFormat = new Date(tooltipDate.toString())
                    assert.isTrue(calendarFieldDateFormat.toDateString() == tooltipDateFormat.toDateString())
                })
            })
        })
    }

    verifyCalendarFieldDateEqualCalendarPopUpDate(calendarId: string, calendarHeaderDateFieldClass: string, openCalendarButtonPath: string){
        var calendarFieldDateFormat = new Date()

        this.getCalendarFieldDate(calendarId)
        cy.get('@calendarFieldDate').then(calendarFieldDate =>{
            calendarFieldDateFormat = new Date(calendarFieldDate.toString())
        }).then(() =>{
            cy.get(calendarId).parent().within(() =>{
                cy.get(openCalendarButtonPath).click().then(() =>{
                    cy.wrap(Cypress.$('body')).within(() =>{
                        this.getCalendarPopUpAppliedDate(calendarHeaderDateFieldClass)
                        cy.get('@calendarPopUpDate').then(calendarPopUpDate =>{
                            var calendarPopUpDateFormat = new Date(calendarPopUpDate.toString())
                            assert.isTrue(calendarFieldDateFormat.toDateString() == calendarPopUpDateFormat.toDateString())
                        })
                    })
                })
            })
        })
    }

    verifyMinMaxDatesAvailable(minDateClass: string, maxDateClass: string, openCalendarButtonPath: string
        , fromCalendarId: string, toCalendarId: string, calendarHeaderDateFieldClass: string, calendarCellsClass: string){

        var minDate
        var maxDate
        
        cy.get(minDateClass).then(minDateCalendar =>{
            minDate = new Date(minDateCalendar.text())
        }).then(() =>{
            cy.get(maxDateClass).then(maxDateCalendar =>{
                maxDate = new Date(maxDateCalendar.text())
            })
        }).then(() =>{
            cy.get(fromCalendarId).parent().within(() =>{
                cy.get(openCalendarButtonPath).click().then(() =>{
                    cy.wrap(Cypress.$('body')).within(() =>{
                        this.checkCalendarAvailableUnavailableDates(minDate, calendarHeaderDateFieldClass, calendarCellsClass, -1)
                    })
                })
            })
        }).then(() =>{
            cy.get(toCalendarId).parent().within(() =>{
                cy.get(openCalendarButtonPath).click().then(() =>{
                    cy.wrap(Cypress.$('body')).within(() =>{
                        this.checkCalendarAvailableUnavailableDates(maxDate, calendarHeaderDateFieldClass, calendarCellsClass, 1)
                    })
                })
            })
        })
    }

    checkCalendarAvailableUnavailableDates(minMaxDate: Date, calendarHeaderDateFieldClass: string, calendarCellsClass: string, addRemoveYearMonth: number){
        var calendarDisabledCellsClass = '.cui-calendar-body-disabled'
        
        var disabledYears = []
        var disabledMonths = []
        
        var year = minMaxDate.getFullYear()
        var disabledYear = year + addRemoveYearMonth
        var minMonth = minMaxDate.toLocaleString('default', { month: 'short' }).toUpperCase()
        var monthNew = new Date(minMaxDate.setMonth(minMaxDate.getMonth() + addRemoveYearMonth));
        var disabledMonth = monthNew.toLocaleString('default', { month: 'short' }).toUpperCase()

        cy.get(calendarHeaderDateFieldClass).click().then(() =>{
            cy.get(calendarDisabledCellsClass).each(value =>{
                disabledYears.push(value.text().trim())
            }).then(() =>{
                assert.isTrue(disabledYears.indexOf(year.toString()) == -1)
                assert.isTrue(disabledYears.indexOf(disabledYear.toString()) != -1)
            }).then(() => {
                cy.get(calendarCellsClass).contains(year.toString()).click()
            }).then(() =>{
                if((minMonth != 'DEC') && (addRemoveYearMonth != -1)){
                    cy.get(calendarDisabledCellsClass).each(value =>{
                        disabledMonths.push(value.text().trim())
                    }).then(() =>{
                        assert.isTrue(disabledMonths.indexOf(minMonth.toString()) == -1)
                        assert.isTrue(disabledMonths.indexOf(disabledMonth.toString()) != -1)
                    })
                }
            })
        })
    }
}

export const calendarHelper = new CalendarHelper()