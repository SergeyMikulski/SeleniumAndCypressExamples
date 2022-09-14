import 'cypress-interface';

class ExportHelper {

    downloadFromSuccesNotificationPopUp(timeout: number) {
        cy.log('verifying if download response status returns 200')
        cy.get('#growler .success-default a', { timeout: timeout }).then(link => {
            cy.request({ url: link.prop('href'), timeout: timeout })
                .then(response => {
                    expect(response.status).to.eq(200)
                });
        });
    }

    downloadFromNotificationPopUpWithSaving(fileName: string) {
        cy.log('downloading from notification popup with saving file')
        this.verifyInfoNotificationPopUp(30000)
        this.verifySuccesNotificationPopUp(30000)
        cy.downloadFromSuccesNotificationPopUp(fileName, 30000)
        this.closeInfoPopUp()
        this.closeSuccesPopUp()
        this.verifyPopUpsClosed()
    }

    verifySuccesNotificationPopUp(timeout: number) {
        cy.contains('cui-growl .success', 'Your file is ready', { timeout: timeout })
    }

    verifyInfoNotificationPopUp(timeout: number) {
        cy.contains('cui-growl .info', 'Your file is processing', { timeout: timeout })
    }

    closeSuccesPopUp() {
        cy.get('cui-growl .success .cui-icon-remove').click({ multiple: true, force: true })
    }

    closeInfoPopUp() {
        cy.get('cui-growl .info .cui-icon-remove').click({ multiple: true, force: true })
    }

    verifyPopUpsClosed() {
        cy.get('cui-growl .success').should('not.exist')
        cy.get('cui-growl .info').should('not.exist')
    }

    exportSearchResultsToExcel() {
        cy.log('Exporting results to excel');
        cy.intercept('POST', '**/search/v1/export').as('exportResultsToExcel');

        cy.get('docstream-export-results button').click();

        cy.wait('@exportResultsToExcel').its('response.statusCode')
            .should('equal', 200);
    }

    verifyXlsxContainsStringArray(fileName: string, stringArray: string[]) {
        for (let num in stringArray) {
            cy.verifyXlsxFileContainsString(fileName, stringArray[num])
        }
    }

    exportToSpecificType(type: string) {
        cy.log(`exporting to ${type}`)
        cy.intercept('POST', '**/ChartExport/**').as('chartExport')
        this.clickOnExportPopOver(type)
        cy.wait('@chartExport').its('response.statusCode').should('eq', 200)
    }

    clickOnExportPopOver(option: string){
        cy.contains('.popover-content .btn', option).click({ force: true })
    }

}

export const exportHelper = new ExportHelper()