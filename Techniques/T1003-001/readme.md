# T1003.001 - OS Credential Dumping: LSASS Memory

## Execution Methods

​	This method attempts to dump the LSASS memory for goodies, the dump file must be parsed by mimikatz or any other to retrive creds. This module is an adaption of [SharpDump](https://github.com/GhostPack/SharpDump), changes were made to guarantie compatibility with the framework.

## Parameters

​	1 - Dump Directory - Optional parameter that will be used to store LSASS dump file.

​	Requires Admin privileges

## Examples

```json

{
    "Technique": "T1003-001",
    "Parameters": [
    	"c:\\users\\public\\"
    ]
}

```



Without any parameter or with invalid path, windows TEMP folder will be used:

```json

{
    "Technique": "T1003-001",
    "Parameters": [
    	""
    ]
}

```

