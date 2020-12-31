# T1547-005 - Boot or Logon Autostart Execution: Security Support Provider

## Description

  This module can set a backdoor in the lsass.exe process and get the credentials of all users who log in to the compromised machine. It also has a built-in script that forces SSP registration, eliminating the need to restart the operating system.
  After execution, the module would create persistence in the environment through the Windows registry keys and after authenticating users, it will create a log file in path ´\Windows\System32\redttpok.log´.


## Execution Methods

  This module has 2 ways to work:

1. **Default execution** (The 'just do it' way)
  - Parameters
    1. run  -> Perform the technique, but need to restart to apply SSP registration and obtain credentials

2. **Enforce SSP register** (The 'opsec' way)
  - Parameters
    1. force  -> Perform the technique, but need to restart to apply SSP registration and obtain credentials


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