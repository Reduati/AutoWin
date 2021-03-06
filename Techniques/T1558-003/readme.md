# T1558-003 - Steal or Forge Kerberos Tickets: Kerberoasting

## Execution Methods

	This method only has one execution method.

## Parameters
	1 - Distinguished Name - Required
		Targeted domain's distinguished name. 
        I.e. "LDAP://DC=dale,DC=com,DC=br"

	2 - Valid User - Required
		User to be used during execution.
        I.e. "dale.com.br\\junin"

    3 - Valid User's Password - Required
        User's password to be used during execution.
        I.e. "Il0veJ3zu5"

## Examples

Kerberoast the "dale.com.br" domain using the domain user "junin" with the password "Il0veJ3zu5":

```json
{
    "Technique": "T1558-003",
    "Parameters": [
        "LDAP://DC=dale,DC=com,DC=br",
        "dale.com.br\\junin",
        "Il0veJ3zu5"
    ]
}
```