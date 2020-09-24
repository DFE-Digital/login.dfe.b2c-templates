Feature: PasswordReset
	Password reset journey

@mytag
Scenario: Password reset
	Given I'm in password reset page
	And I enter my email address
	When I click click reset password
	Then I redirected to we've sent an email page