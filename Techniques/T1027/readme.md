# T1027 - Obfuscated Files or Information
## Execution Methods

	This module has four different execution method, please check parameter 1 - executionMethod - for the available options.

## Parameters
	1 - executionMethod - Required
		Obfuscation method to be tested during execution. 
		Either "Padding", "Packing", "Stego" or "Indicator".

	2 - remoteFile - Optional
		URL to the remote file to be downloaded. Should be a software packed file (i.e. a file packed with upx). 
	   
	3 - localFile - Optional
		Local path to download the remoteFile to. Defaults to \Users\Public\debug.exe. Must contain the file name.

## Examples

Dropping software packed file with default parameters:

```json
{
    "Technique": "T1027",
    "Parameters": [
	    "Packing"
    ]
}
```