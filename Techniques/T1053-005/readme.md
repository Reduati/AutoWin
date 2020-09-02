

# T1053-005 - Scheduled Tasks

## Description

  This module can schedule tasks in Windows using schtasks.

## Execution Methods

  This method has 4 execution method, aimed at:

1. **PERSISTENCE** (Create)
  - Parameters
    - persistence;
    - Task name;
    - Path of file to run;
    - Schedule frequency:
      * minute | hourly | daily | weekly | monthly | once | onstart | onlogon | onidle | onevent
    - Additional args (Optional).
2. **EXECUTION** (Run) 
  - Parameters
    - exec;
    - Task name;
    - Additional args (Optional).
3. **PRIVILEGE ESCALATION** (Create a task to run as system) 
  - Parameters
    - privesc;
    - Task name;
    - Path of file to run;
    - Schedule frequency ;
      * minute | hourly | daily | weekly | monthly | once | onstart | onlogon | onidle | onevent
    - Additional args (Optional).
4. **QUERY** (Query) 
  - Parameters;
    - query;
    - Task name;
    - Additional args (Optional).

## Examples

- Persistence

  Can create new schedule task:

```json
{
"Technique": "T1053-005",
"Parameters": [
  "persistence",
  "Task test",
  "C:\\Users\\Public\\program.exe",
  "minute"
  ]
}
```

- Execution

  Can force a scheuled task to run:

```json
{
"Technique": "T1053-005",
"Parameters": [
  "exec",
  "Task test"
  ]
}
```

- Privilege Escalation

    You can create a new scheduled task and run as a specific user. If the current user is a local administrator, it will automatically be used as the owner of the task, otherwise, a window will be pop-up to enter valid credentials of an administrator user (It doesn't do UAC Bypass).

```json
{
"Technique": "T1053-005",
"Parameters": [
  "privesc",
  "Privesc task test",
  "C:\\Users\\Public\\programAsSystem.exe",
  "minute"
  ]
}
```

- Query

  Query status of scheduled task by name:

```json
{
"Technique": "T1053-005",
"Parameters": [
  "query",
  "Task test"
  ]
}
```
<br>

#### Additional args are always the last parameter and is optional 

> Persistence sample with not required arguments

```json
{
"Technique": "T1053-005",
"Parameters": [
  "persistence",
  "Task test",
  "C:\\Users\\Public\\program.exe",
  "minute",
  "/ST 12:00:00 /mo 10 /d MON,TUE,WED /np"
  ]
}
```
