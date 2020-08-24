
# T1087-000 - Account Discovery

## Description

  This module can enumerate local or domain users. It can works using net utility or powershell binary.
  For net utility, is useful to retrive users in local or domain context. Also helps to retrive informartion about specific users.
  Otherside, using powershell, can be possible to store the results of enumeration in a file on the disk, already parsed as a great wordlist with valid domain users. This wordlist can be used with module "T1110.003" to try get access on this accounts (through password spray).

## Execution Methods

  This module has 3 ways to work:

1. **Using 'net.exe' file** (Can enumerate users on the local machine or domain, can also retrieve information about a specific user or a list of them)
  - Parameters
    - net  -> Specific the binary that will be used to run enum
    - local | domain -> Will the users of the local machine or domain controller be enumerated?
    - Additional args (Optional). Can be pass each username to get info about (see examples).

2. **Using 'powershell.exe' binary** (Enumerate domain users using and can generate wordlist with valid users) 
  - Parameters
    - powershell  -> Specific the binary that will be used to run enum
    - enabled | all | pwdstats | emailist | free  -> Possible actions to perform using powershell
        Enabled: Get only enabled domain users;
        All: Get all domain users;
        Pwdstats: Get users and information about password stats (output has a table);
        Emailist: Get all domain users and their email address (Useful for advanced campains);
        Free: Useful if you want to control which arguments the command should use

3. **Run enumeration using powershel without 'powershell.exe' binary** (Enumerate domain users) 
  - Parameters
    - encoded  -> Specific that no binary will be used to run enum (only use DLL's powershell)
    - Set the command to be executed as value in the 2nd parameter (must be encoded as base64)

### Optional
  If you want save output of execution, is needed inform the key "output" at "EntryData" in the file.flow: 
```json
      "EntryData": {
        "output":"TextContent.txt"
      },
```
The name of key must be "output" and the value of it is the file name. One nice thing about the file parameter is that without passing the full file path the module will try to find the file inside the workfolder. Today, the default path is "c:\users\public". You can modify this value by enforcing a new value in your flow file or by just entering the full path of your file along with the name, ie: "c:\\users\\public\\users.txt".
Insert the EntryData content below the number of technique "T1087-000".


## Examples

- Local

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

  Can get information about specific users on the local machine:

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

- Domain

  Can get all domain users and store the output in C:/Users/Public/results.txt:

```json
{
"EntryData": {
  "output":"results.txt"
},
"Technique": "T1087-000",
"Parameters": [
  "net",
  "domain"
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
    "enabled"
  ]
}
```

  Retrieve alldomain users and their emaill address and create a wordlist at C:\Users\Public\emailsToPhising.txt:

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