Feature: SignIn
	sign in journey

@mytag
Scenario: SignIn
	Given I'm in the signin page
	And I enter email id and password
	When I click sign in
	Then I may redirected to Ts & Cs page if new Ts & Cs rolled out
	Then I should be redicected to jwt.ms with a valid token