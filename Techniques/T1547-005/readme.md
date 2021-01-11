# T1547-005 - Boot or Logon Autostart Execution: Security Support Provider

## Description

  This module can set a backdoor in the lsass.exe process and get the credentials of all users who log in to the compromised machine in **plaintext**. It also has a built-in script that forces SSP registration (optional), eliminating the need to restart the operating system.
  After execution, the module would create persistence in the environment through the Windows registry keys and after authenticating users, it will create a log file in path ```\Windows\System32\redttpok.log```.

  In the future, we'll add a modified version of this module to register the SSP without requering the usage of powershell and registry, using the technique discussed by XPN's [article](https://blog.xpnsec.com/exploring-mimikatz-part-2/). It's also important to notice when developing a detection for this technique, that powershell is not required but an optional, avoiding the need for a reboot after the registry update.

## Execution Methods

  This module has 2 ways to work:

1. **Default execution** (The 'just do it' way)
  - Parameters
    1. run  -> Perform the technique, but need to restart to apply SSP registration and obtain credentials

2. **Enforce SSP register** (The 'opsec' way)
  - Parameters
    1. force  -> The same, but eliminates the need to restart the operating system (runs a PowerShell script in memory using the technique [T1059-001](https://github.com/Reduati/AutoWin/tree/master/Techniques/T1059-001))


## Examples

In this section, we show some cases for the proper use of this module.

### Default execution

Run the technique but need to restart the machine:

```json
{
"Technique": "T1547-005",
"Parameters": [
  "run"
  ]
}
```

### Enforce SSP register

Run the technique and the script to force register (don't need to restart):

```json
{
"Technique": "T1547-005",
"Parameters": [
  "force"
  ]
}
```
