
# Aut0.W1n

Autowin is a framework that helps organizations simulate custom attack scenarios in order to improve detection and response capabilities.

Check out [all the techniques](https://github.com/Reduati/AutoWin/tree/master/Techniques) we developed. Each one has a readme.md file, so you can better understand what they do and how to create a custom scenario.

Autowin's entire architechture is based around [Mitre ATT&CK Framework](https://attack.mitre.org/) to facilitate not only the creation of the modules, but also the communication between those who test and those who get tested.

## Build

Right now we are not releasing a built version of this project, but you can easily build it yourself using Visual Studio.

## Techniques

This is a project in development and new techniques are developed every day. Currently available techniques:

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
|T1110-000|[Brute Force](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1110-000)|
|T1543-003|[Create or Modify System Process: Windows Service](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1543-003)|
|T1547-001|[Registry Run Keys / Startup Folder](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1547-001)|
|T1219|[Remote Access Software](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1219)|
|T1036-004|[Masquerading: Masquerade Task or Service](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1036-004)|
|T1027|[Obfuscated Files or Information](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1027)|
|T1036-004|[RDP](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1036-004)|

[!] Techniques with the "000" suffix are modules that contains multiple subtechniques as execution methods. Modules without any sort of suffix are singular execution method techniques that do not contain any subtechnique.

## Usage

Autowin currently supports three execution methods:

### Full

TO DO

### Flow
You can use the "flow" method to execute techniques that are part of an specific scenario.

```bash
AutoWin.exe --flow attack_scenario.flow
```
Imagine that you want to create a scenario where the attacker enumerates all local users (T1087-000) and tries to brute force their password (T1110-000), you could achieve this by creating the following attack flow:
```json
{
    "Campaign": "Brute Force or Password Spray local Users",
    "Datetime": "2020-07-30 10:00:00",
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
            "Technique": "T1110-000",
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

The framework currently uses the public folder (C:\Users\Public) as it's default artifact/resource dropping location. Techniques use that path automatically whenever possible, so you don't have to worry about it. If your simulation requires a specific directory, you can pass the "Workfolder" parameter in your attack flow file, before declaring your techniques:
```json
{
    "Campaign": "Just an example",
    "Datetime": "2020-07-30 10:00:00",
    "Workfolder": "C:\\temp\\",
    "Techniques": {
     ...
```
You can also change the path inside the technique scope, as showed in the previous scenario by the variable "output".

### Debug

TO DO

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as necessary.

## License
[MIT](https://choosealicense.com/licenses/mit/)
