Feature: ChangeEmailConfirmation
	Change email journey

@mytag
Scenario: Change email journey
	Given I'm navigated to change email confirmation page
	Then Check whether account is already activated
	And If account is not activated them should redirected to we've changed email page