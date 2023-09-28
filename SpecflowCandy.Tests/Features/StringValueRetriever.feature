Feature: String Value Retrievers

As a test engineer, I should be able to use placeholders for string values in SpecFlow Feature Files and have their values stored for reuse.

Scenario Outline: Random string
	When I store an object at key "stringValueObject", created from
		| Field       | Value   |
		| StringValue | <value> |
	Then the string field, at key "stringValueObject", is populated by a non-default string

	Examples:
	| value                                                            |
	| {{random}}                                                       |
	| {{rAnDoM}}                                                       |
	| {{ThisIsAContextKey}}                                            |
	| This is just a plain ole sentence                                |
	| {{contextKey:This is a sentenance that I want in a context key}} |
	| {{thisIsAStringGuid:guid}}                                       |

Scenario Outline: Tokens in explicit strings
	When I store an object at key "stringValueObject", created from
         | Field       | Value   |
         | StringValue | <value> |
	Then the string field, at key "stringValueObject", is populated by a non-default string
	And the string field of the object stored at context key "stringValueObject" contains the string value of "{{insideToken}}"

	Examples: 
	| value                                                                                          |
	| This is a longer sentance that will have another, random, sentence at the end. {{insideToken}} |
	| {{insideToken}} That was a random sentence at the begining                                     |
	| Let's put it right here: {{insideToken}} In the middle.                                        |
    
Scenario: String replace syntax without tables
	When I use a string parameter "{{randomStringValue}}" stored at context key "validationValue"
	Then the context key "randomStringValue" is a non-default string
	And the string value "{{randomStringValue}}" equals the value in context at key "validationValue"

Scenario: Explicit String into a context key
    When I use a string parameter "{{explicitString:This is a sentenance that I want in a context key}}" stored at context key "validationValue"
	Then the context key "explicitString" is a non-default string
	And the string value "This is a sentenance that I want in a context key" equals the value in context at key "explicitString"
	And the string value "{{explicitString}}" equals the value in context at key "validationValue"
    
Scenario: String valued Context Keys reused in subsequent tables and parameters
	When I store an object at key "stringValueObject", created from
         | Field       | Value        |
         | StringValue | {{ImAToken}} |
	And I store an object at key "secondObject", created from
         | Field       | Value        |
         | StringValue | {{ImAToken}} |
	Then the string field, at key "stringValueObject", is populated by a non-default string
	And the string field, at key "secondObject", is populated by a non-default string
	And the string value, at key "ImAToken", and the string value of the object at key "stringValueObject" are equal
	And the string value, at key "ImAToken", and the string value of the object at key "secondObject" are equal
