# T1550-003 - Use Alternate Authentication Material: Pass the Ticket

## Execution Methods

	This method only has one execution method.

## Parameters
	1 - Ticket File - Required
		Path to ticket file (.kibit format) to be used. 
        I.e. "C:\\Users\\Public\\ticket.kirbi"

## Examples

Injecting the ticket contained in the "C:\\Users\\Public\\ticket.kirbi" file on to the current session:

```json
{
    "Technique": "T1550-003",
    "Parameters": [
        "C:\\Users\\Public\\ticket.kirbi"
    ]
}
```