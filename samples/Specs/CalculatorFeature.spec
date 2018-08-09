# This file is auto-generated, any changes made to this file will be lost


Feature: Calculator
	In order to avoid silly mistakes
    As a math idiot
    I want to be told the sum of two numbers

Scenario: Add two numbers
			Given I have entered 1 into the calculator
			And I have also entered 2 into the calculator
			WhenAttribute I press add
			Then the result should be 3

Scenario: Multiply two numbers
			Given I have a calculator
			When I key in 10
			When I key in 5 and press multiply
			Then It sets the Total to 50
			Then It sets the equation to 10 x 5

Scenario: Divide two numbers
			Given I have a calculator
			When I key in 10
			When I key in 2 and press divide
			Then It sets the Total to 5
			Then It sets the equation to 10 / 2

			Given I have a calculator
			When I key in 20
			When I key in 4 and press divide
			Then It sets the Total to 5
			Then It sets the equation to 20 / 4

Scenario: Subtract two numbers
			Given I have entered 5 into the calculator
			And I have also entered 2 into the calculator
			WhenAttribute I press minus
			Then the result should be 3