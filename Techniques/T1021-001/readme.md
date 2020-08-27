# T1021-001 - RDP

## Execution Methods

	This module has four different execution method, please check parameter 4 - method - for the available options.

## Parameters
	1 - target - Required
	   Targeted machine to connect via RDP.

	2 - user - Required
	   Username to be sent to the targeted machine during authentication.

	3 - password - Required
	   Password to be sent to the targeted machine during authentication.

	4 - method - Required
	   Execution method to be used after authenticating. 
	   Either "cmd", "powershell", "winr" or "taskmgr".

	5 - authentication - Required
	   Desired authentication method, either Local or NLA.

	6 - command - Required
	   Command to be executed on the target machine.

## Examples

Log on to 192.168.0.1 using Bob's credentials to authenticate locally and executing the "whoami" command through task manager:

```json
{
    "Technique": "T1021-001",
    "Parameters": [
	    "192.168.0.1",
	    "Bob",
	    "il0vec475",
	    "taskmgr",
	    "Local",
	    "whoami"
    ]
}
```