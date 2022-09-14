// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add('login', (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add('drag', { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add('dismiss', { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This will overwrite an existing command --
// Cypress.Commands.overwrite('visit', (originalFn, url, options) => { ... })
declare namespace Cypress {
    interface Chainable<Subject> {
      LogIn(url: string, user?: string): Chainable<Subject>;
      PreserveCookies(): Chainable<Subject>;
      VerifyUrlContainsKeyword(keyword: string): Chainable<Subject>;
    }
  }
  
  Cypress.Commands.add("LogIn", (url: string, user?: string) => {
  
    let login: string;
  
    if (user == 'LimitedUser') {
      login = Cypress.env('LIMITED_TEST_LOGIN')
    }
  
    else {
      login = Cypress.env('TEST_LOGIN')
    }
  
    cy.visit(url, {
      auth: {
        username: login,
        password: Cypress.env("TEST_PASSWORD")
      }
    });
  });
  
  Cypress.Commands.add("PreserveCookies", () => {
    Cypress.Cookies.preserveOnce(
      Cypress.env('SESS_EXP_STAGE'),
      Cypress.env('SSO_SESS'),
      Cypress.env('SSO_UI'),
      Cypress.env('ESS_EXP'));
  });
  
  Cypress.Commands.add("VerifyUrlContainsKeyword", (keyword: string) => {
    cy.url().should('contain', keyword);
  });