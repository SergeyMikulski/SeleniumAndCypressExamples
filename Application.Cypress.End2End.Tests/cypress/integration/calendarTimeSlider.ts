import { calendarHelper } from '../support/calendarHelper';

const minDateClass = '.date-picker-label-above-left'
const maxDateClass = '.date-picker-label-above-right'

const calendarBarPath = '.date-range-picker label'
const calendarHeaderDateFieldClass = '.fs-sub-section-r'
const calendarCellsClass = '.cui-calendar-body-cell-content'

const fromCalendarId = '#cui-input-0'
const toCalendarId = '#cui-input-1'
const openCalendarButtonPath = '.calendar button'

const fromTimeSliderTooltipId = '#cui-tooltip-1'
const toTimeSliderTooltipId = '#cui-tooltip-2'

describe('Application Calendar', () => {
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
    
    describe('Application Calendar Time Slider tests', () => {
        it('Calendar Influence Time Slider And Fields', () => {
            cy.wrap(null).then(() =>{
                calendarHelper.initialSetCalendar(minDateClass, maxDateClass, openCalendarButtonPath, fromCalendarId, toCalendarId, calendarHeaderDateFieldClass, calendarCellsClass)
            }).then(() =>{
                calendarHelper.changeYearInCalendarDate(fromCalendarId, toCalendarId, openCalendarButtonPath, calendarHeaderDateFieldClass, calendarCellsClass)
            }).then(() =>{
                calendarHelper.verifyDatesInCalendarAndTimeSliderCorrect(fromCalendarId, toCalendarId, fromTimeSliderTooltipId, toTimeSliderTooltipId)
            })
        })
        it('Time Slider Influence Calendar And Fields', () => {
            cy.wrap(null).then(() =>{
                //calendarHelper.initialSetCalendar(minDateClass, maxDateClass, openCalendarButtonPath, fromCalendarId, toCalendarId, calendarHeaderDateFieldClass, calendarCellsClass)
            }).then(() =>{
                calendarHelper.dragAndDropTooltips()
            }).then(() =>{
                calendarHelper.verifyDatesInCalendarAndTimeSliderCoinside(fromCalendarId, toCalendarId, fromTimeSliderTooltipId, toTimeSliderTooltipId)
            })
        })
        it('Time Slider Can Be Dragged By Grab Button', () => {
            var pixelsNumberToDragFirst = 10
            var pixelsNumberToDragSecond = -20
            cy.wrap(null).then(() =>{
                //calendarHelper.initialSetCalendar(minDateClass, maxDateClass, openCalendarButtonPath, fromCalendarId, toCalendarId, calendarHeaderDateFieldClass, calendarCellsClass)
            }).then(() =>{
                calendarHelper.dragAndDropGrabButton(pixelsNumberToDragFirst)
            }).then(() =>{
                calendarHelper.verifyDatesInCalendarAndTimeSliderCoinside(fromCalendarId, toCalendarId, fromTimeSliderTooltipId, toTimeSliderTooltipId)
            }).then(() =>{
                calendarHelper.dragAndDropGrabButton(pixelsNumberToDragSecond)
            }).then(() =>{
                calendarHelper.verifyDatesInCalendarAndTimeSliderCoinside(fromCalendarId, toCalendarId, fromTimeSliderTooltipId, toTimeSliderTooltipId)
            })
        })
        it('Are Calendar Field And Calendar Have The Same Dates', () => {
            cy.wrap(null).then(() =>{
                //calendarHelper.initialSetCalendar(minDateClass, maxDateClass, openCalendarButtonPath, fromCalendarId, toCalendarId, calendarHeaderDateFieldClass, calendarCellsClass)
            }).then(() =>{
                calendarHelper.verifyCalendarFieldDateEqualCalendarPopUpDate(fromCalendarId, calendarHeaderDateFieldClass, openCalendarButtonPath)
            }).then(() =>{
                calendarHelper.verifyCalendarFieldDateEqualCalendarPopUpDate(toCalendarId, calendarHeaderDateFieldClass, openCalendarButtonPath)
            })
        })
        it('Are Min Max Dates Available In Calendar', () => {
            cy.wrap(null).then(() =>{
                //calendarHelper.initialSetCalendar(minDateClass, maxDateClass, openCalendarButtonPath, fromCalendarId, toCalendarId, calendarHeaderDateFieldClass, calendarCellsClass)
            }).then(() =>{
                calendarHelper.verifyMinMaxDatesAvailable(minDateClass, maxDateClass, openCalendarButtonPath, fromCalendarId, toCalendarId, calendarHeaderDateFieldClass, calendarCellsClass)
            })
        })
    })
})