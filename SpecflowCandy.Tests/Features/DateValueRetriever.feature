Feature: Date Value Retriever

As a test engineer, I should be able to use placeholders for Date (DateTime) values in SpecFlow Feature Files and have their values stored for reuse.

Scenario Outline: Getting random Dates
    When I store an object at key "dateTimeObject", created from
        | Field         | Value   |
        | DateTimeValue | <value> |
	Then the date field, at key "dateTimeObject", is populated by a non-default date time

    Examples:
        | value                               |
        | 2022-08-30T23:37:24.6577577Z        |
        | {{random}}                          |
        | {{rAnDoM}}                          |
        | {{ThisIsAContextKey}}               |
        | {{Now}}                             |
        | {{Now + 2d}}                        |
        | {{Now - 2d}}                        |
        | {{Now + 34y}}                       |
        | {{ThisIsAContextKey:Now + 34y}}     |
        | {{Now(mst) + 2d}}                   |
        | {{Now(pst) + 2m}}                   |
        | {{Now(est)}}                        |
        | {{now(cst)}}                        |
        | {{ThisIsAContextKey:Now(mst) + 1w}} |

Scenario Outline: Ensure Timezone Conversion
    Then these dates are not equal "<date1>" and "<date2>"

    Examples:
        | date1        | date2        |
        | {{now(mst)}} | {{now}}      |
        | {{now(mst)}} | {{now(utc)}} |
        | {{now(est)}} | {{now}}      |
        | {{now(phx)}} | {{now}}      |
        | {{now(cst)}} | {{now}}      |
        | {{now(pst)}} | {{now}}      |

Scenario Outline: Calculating a date, and storing it in a variable
    When I use a datetime parameter "<TokenWithSet>" stored at context key "validationValue"
	Then the context key "<TokenContextKey>" is a non-default datetime
	And the context key "validationValue" is a non-default datetime
	And the date value "{{<TokenContextKey>}}" equals the value in context at key "validationValue"

    Examples:
        | TokenWithSet                     | TokenContextKey |
        | {{randomDate:random}}            | randomDate      |
        | {{knownDate}}                    | knownDate       |
        | {{twoDaysFromNow:now + 2d}}      | twoDaysFromNow  |
        | {{twoDaysFromNow:Now + 2d}}      | twoDaysFromNow  |
        | {{twoDaysFromNow:Now(mst) + 2d}} | twoDaysFromNow  |
