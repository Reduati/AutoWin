# T1087-000 - Account Discovery

## Description

  This module can enumerate local or domain users. It can works using net utility, powershell binary or using techniques to run powershell without powershell.exe.
  For net utility, is useful to retrive users in local or domain context. Also helps to retrive information about specific users.
  Otherside, using powershell (.exe or in memory), can be possible to store the results of enumeration in a file on the disk, already parsed as a great wordlist with the valid domain users. This wordlist can be used with module "T1110.003" to try get access on this accounts (through password spray).

## Execution Methods

  This module has 3 ways to work:

1. **Using 'net.exe' file** (Can enumerate users on the local machine or domain, can also retrieve information about a specific user or a list of them)
  - Parameters
    1. net  -> Specific the binary that will be used to run enum
    2. local | domain -> Will the users of the local machine or domain controller be enumerated?
    3. Additional args (Optional). Can be pass each username to get info about (see examples).

2. **Using 'powershell.exe' binary OR with method "powershel without 'powershell.exe' binary"** (Useful to enumerate domain users and generate wordlist with valid users as result) 
  - Parameters
    1. powershell | psinmemory  -> Define which method will be executed
    2. local **\*** | domain -> Will the users of the local machine or domain controller be enumerated?
    3. enabled | all | pergroup | pwdstats | emailist  -> Possible actions to perform using powershell
        - Enabled: Get only enabled domain users;
        - All: Get all domain users;
        - PerGroup: Useful if you want to control which arguments the command should use
	        3. Group Name: To enumerate domain users per group, 
        - PwdStats: Get users and information about password stats (output has a table);
        - EmaiList: Get all domain users and their email address (Useful for advanced campains);

\* Enumeration of local users using powershell is not properly implemented. This is because Microsoft does not support the use of the Microsoft.PowerShell.LocalAccounts module in 32-bit powershell on a 64-bit system. You can check it out [here](https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.localaccounts/get-localuser?view=powershell-5.1). This implementation will be carried out throughout the project.

### Optional - Storing results in a text file
  If you want to save the output of execution, is needed inform the key "output" at "EntryData" in the file.flow: 
```json
      "EntryData": {
        "output":"TextContent.txt"
      },
```

> The name of key must be "output" and the value of it is the file name.
> One nice thing about the file parameter is that without passing the
> full file path the module will try to find the file inside the
> workfolder. Today, the default path is "c:\users\public". You can
> modify this value by enforcing a new value in your flow file or by
> just entering the full path of your file along with the name, ie:
> "c:\\users\\public\\users.txt". Insert the EntryData content inside
> the technique "T1087-000".


## Examples

In this section, we show some cases for the proper use of this module.

### Local

Can get all users on the local machine:

```json
{
"Technique": "T1087-000",
"Parameters": [
  "net",
  "local"
  ]
}
```

  To get information about specific users on the local machine:

```json
{
"Technique": "T1087-000",
"Parameters": [
  "net",
  "local",
  "User 1",
  "User 2"
  ]
}
```

### Domain

To get all domain users and store the output in C:/Users/Public/results.txt:

```json
{
"EntryData": {
  "output":"results.txt"
},
"Technique": "T1087-000",
"Parameters": [
  "powershell",
  "domain",
  "all"
  ]
}
```

  Can get information about specific domain users using net.exe binary:

```json
{
"Technique": "T1087-000",
"Parameters": [
  "net"
  "domain",
  "User 1",
  "User 2"
  ]
}
```

Retrieve information about all enabled domain users and create a wordlist at C:\Temp\usersToBrute.txt:

```json
{
  "EntryData": {
    "output":"C:\\Temp\\usersToBrute.txt"
  },
  "Technique": "T1087-000",
  "Parameters": [
    "powershell",
    "domain",
    "enabled"
  ]
}
```

Retrieve all domain users and their emaill address and create a wordlist at C:\Users\Public\emailsToPhising.txt:

```json
{
  "EntryData": {
    "output":"emailsToPhising.txt"
  },
  "Technique": "T1087-000",
  "Parameters": [
    "powershell",
    "emailist"
  ]
}
```

To get all domain users in a specific domain group and store the output in C:/Users/User/Download/domainAdmins.txt, all of it running powershell in memory (Usefull to bypass the block execution of powershell.exe by hash, for exemple):

```json
{
"EntryData": {
  "output":"C:\\Users\\User\\Download\\domainAdmins.txt"
},
"Technique": "T1087-000",
"Parameters": [
  "psinmemory",
  "domain",
  "pergroup",
  "Domain Admins"
  ]
}
```

> Note: To perform the enumeration by group, the group name is required as the value of the 4th parameter