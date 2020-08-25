
function getDevenvPath() {
    $devenv = "HKLM:\\Software\Microsoft\Windows\CurrentVersion\App Paths\devenv.exe"
    if(test-path($devenv)) {
        $path = Get-ItemProperty -Path $devenv
        return $path."(default)"
    }
    return $false
}

function Should-ICompile {
    param ( $technique, $techniques_folder, $hashtable )
    if(test-path($hashtable)) {
        $table = get-content $hashtable
        # check if technique exists on hashtable - if not its new and should be compiled
        if($table -like "*$technique*") {
            $hash = Get-FolderHash("$techniques_folder\$technique")
            if($table -like "*$hash*") {
                return $false
            }
        }
    }
    
    return $true
}


function Compile-Project {
    
    param($slnfile, $technique, $techniques_folder, $hashtable)

    if((Should-ICompile $technique $techniques_folder $hashtable)) {
        
        if(test-path($slnfile)) {
        
            $pinfo = New-Object System.Diagnostics.ProcessStartInfo
            $pinfo.FileName = (getDevenvPath)
            $pinfo.RedirectStandardError = $true
            $pinfo.RedirectStandardOutput = $true
            $pinfo.UseShellExecute = $false
            $pinfo.Arguments = "$slnfile /rebuild"
            $p = New-Object System.Diagnostics.Process
            $p.StartInfo = $pinfo
            $p.Start() | Out-Null
            $p.WaitForExit(3)
            $stdout = $p.StandardOutput.ReadToEnd()
            $stderr = $p.StandardError.ReadToEnd()
            if($p.ExitCode -eq 0) {
                write-host "[!] MSBUILD compiled {$slnfile} without any errors!"
                return $true
            } else {
                 Write-Host "stdout: $stdout"
                 Write-Host "stderr: $stderr"
                 Write-Error 'Error' -ErrorAction Stop
            }

        } else {
            write-host "[!] Could not find $slnfile"
        }

        return $false

    } else {
        write-host "[*] No changes found on $technique, ignoring compiling phase!"
        return $true
    }
    
   
}

function Resolve-PathSafe {
    param (
        [string] $Path
    )
      
    $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($Path)
}
function ConvertTo-Base64 {
    param (
        [string] $SourceFilePath,
        [string] $TargetFilePath
    )
 
    $SourceFilePath = Resolve-PathSafe $SourceFilePath
    $TargetFilePath = Resolve-PathSafe $TargetFilePath
     
    $bufferSize = 9000 # should be a multiplier of 3
    $buffer = New-Object byte[] $bufferSize
     
    $reader = [System.IO.File]::OpenRead($SourceFilePath)
    $writer = [System.IO.File]::CreateText($TargetFilePath)
     
    $bytesRead = 0
    do
    {
        $bytesRead = $reader.Read($buffer, 0, $bufferSize);
        $writer.Write([Convert]::ToBase64String($buffer, 0, $bytesRead));
    } while ($bytesRead -eq $bufferSize);
     
    $reader.Dispose()
    $writer.Dispose()
}

function Get-FolderHash ($folder) {

 dir $folder -Recurse | ?{ (!$_.psiscontainer) -and ($_.Extension -like "*.cs") }  | %{[Byte[]]$contents += [System.IO.File]::ReadAllBytes($_.fullname)}
 $hasher = [System.Security.Cryptography.SHA1]::Create()
 [string]::Join("",$($hasher.ComputeHash($contents) | %{"{0:x2}" -f $_}))
}

function update-Hashtable {
    param (
        [string] $techniques_folder,
        [string] $hashtable
    )
    if(test-path($hashtable)) {
        Remove-Item -Path $hashtable -force
    }
    $techniques = Get-Childitem –Path $techniques_folder
    ForEach ($technique in $techniques) {
        $hash = Get-FolderHash("$techniques_folder\$technique")
        add-content $hashtable "$technique|$hash"
    }
}


$use_module = $true # if true, build will create CS file
$techniques_folder = "$PSScriptRoot\..\Techniques\"
$lib_folder = $PSScriptRoot + "\bin\debug\lib\"
$hashtable = "$PSScriptRoot\.hashtable"

New-Item -ItemType "directory" -Path $lib_folder -Force

  
write-host "

 █████╗ ██╗   ██╗████████╗ ██████╗    ██╗    ██╗ ██╗███╗   ██╗
██╔══██╗██║   ██║╚══██╔══╝██╔═████╗   ██║    ██║███║████╗  ██║
███████║██║   ██║   ██║   ██║██╔██║   ██║ █╗ ██║╚██║██╔██╗ ██║
██╔══██║██║   ██║   ██║   ████╔╝██║   ██║███╗██║ ██║██║╚██╗██║
██║  ██║╚██████╔╝   ██║   ╚██████╔╝██╗╚███╔███╔╝ ██║██║ ╚████║
╚═╝  ╚═╝ ╚═════╝    ╚═╝    ╚═════╝ ╚═╝ ╚══╝╚══╝  ╚═╝╚═╝  ╚═══╝
                                Build Script - vs 0.0.3
"
write-host "Hello, I'm trying to build your project because your lazy ass don't want to move files around."


$first_time = $false


# As of now (vs 0.0.3) - This script requires a valid VS installation

if(getDevenvPath) {

    $techniques = Get-Childitem –Path $techniques_folder

    ForEach ($technique in $techniques) {

        write-host "[-] Checking $technique..."
        $technique_bin = "$techniques_folder$technique\bin\Debug\$technique.exe"
    
        # Compile project
        if((Compile-Project "$techniques_folder$technique\$technique.sln" $technique $techniques_folder $hashtable)) {
            if(test-path($technique_bin)) {
            
                if($use_module) {
                    write-host "[*] Generating module and copying module for lib folder! "
                    ConvertTo-Base64 $technique_bin "$lib_folder\$technique.m"
                } else {
                    write-host "[*] Copying binary to lib folder!"
                    Copy-Item -Path $technique_bin -Destination "$lib_folder\$technique.exe"
                }
            
            }
        }

        write-host "[#] {$technique} good to go!"

    }

} else {
    write-host "[-] Must have devenv.exe on your machine. Check if your Visual Studio installation is OK!"
}

update-Hashtable $techniques_folder $hashtable

