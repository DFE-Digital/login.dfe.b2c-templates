Feature: PasswordResetConfirmation
	Password reset confirmation journey

@mytag
Scenario: Password reset confirmation
	Given I'm in password reset confirmation page
	And I enter new password and confirmation password
	When I click reset password button
	Then I should redirected to password reset confirmation message page