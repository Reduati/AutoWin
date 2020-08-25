# Aut0.W1n

Autowin is a framework that helps organizations simulate specific attack scenarios in order to improve detection and response capabilities.

## Build

Right now we are not releasing a builded version of this project, but you can easily build it yourself using Visual Studio.


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