# Aut0.W1n


Autowin is a framework that helps organizations simulate specific attack scenarios in order to improve detection and response capabilities.

Check out [all the techniques](https://github.com/Reduati/AutoWin/tree/master/Techniques) we developed. Each one has a readme file, so you can understand better what they done and how to create a scenario.
## Build

Right now we are not releasing a builded version of this project, but you can easily build it yourself using Visual Studio.

## Techniques

This is a project in development and new techniques are developed every day. In this moment, you can use those bellow:

| MID | Technique |
| ----|----|
|T1003-001|[OS Credential Dumping: LSASS Memory](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1003-001)|
|T1037-001|[Boot or Logon Initialization Scripts: Logon Script](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1037-001)|
|T1046|[Network Service Scanning](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1046)|
|T1053-005|[Scheduled Tasks](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1053-005)|
|T1059-001|[Powershell](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1059-001)|
|T1059-003|[Command and Scripting Interpreter: Windows Command Shell](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1059-003)|
|T1059-005|[Visual Basic Script](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1059-005)|
|T1059-007|[Javascript/JScript](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1059-007)|
|T1087-000|[Account Discovery](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1087-000)|
|T1110-003|[Brute Force](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1110-003)|
|T1543-003|[Create or Modify System Process: Windows Service](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1543-003)|
|T1547-001|[Registry Run Keys / Startup Folder](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1547-001)|
|T1219|[Account Discovery](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1087-000)|
|T1036-004|[Masquerading: Masquerade Task or Service](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1036-004)|
|T1027|[Obfuscated Files or Information](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1027)|
|T1036-004|[RDP](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1036-004)|

\*Techniques with the "000" suffix are modules that contains multiple subtechniques as execution methods. Modules without any sort of suffix are singular execution method techniques that do not contain any technique.

## Usage

You can use the "flow" option to execute techniques that are part of an specific scenario.

```bash
AutoWin.exe --flow attack_scenario.flow
```
Imagine that you want to create a scenario where the attacker enumates all local users (T1087-000) and tries to brute force (T1110-003), you could achieve this by creating the following flow:

```json
{
    "Campaign": "Brute Force or Password Spray local Users",
    "Datetime": "2020-07-30 10:00:--",
    "Techniques": {
        "1": {
            "Technique": "T1087-000",
	    "EntryData" : {
		"output":"users.txt"
	    },
            "Parameters" : [
                "net",
		        "local"
            ]
           
        },
        "2": {
            "Technique": "T1110-003",
            "Parameters" : [
                "local",
                "users.txt",
                "password.txt"
            ]
           
        }
    }
}

```

For this example, you must provide a password.txt file contaning the passwords that will be tested against the users. 

In the moment, the framework uses as default location the public folder (c:\users\public). All techniques will use this path, so you don't have to worry about it. In an case, you can change it by adding the variable "Workfolder"
 inside the attack flow:

```json
{
    "Campaign": "Just an example",
    "Datetime": "2020-07-30 10:00:--",
    "Workfolder": "c:\\temp\\",
    "Techniques": {
     ...
```

You can also change the path inside the technique scope, as showed in the previous scenario by the variable "output".

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)