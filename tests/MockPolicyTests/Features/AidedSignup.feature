Feature: AidedSignup
	Account activation

@mytag
Scenario: Aided Signup
	Given I am navigated to the email activation link
	And I filled the form
	When I click on Register button
	Then I am taken to the Account activated page