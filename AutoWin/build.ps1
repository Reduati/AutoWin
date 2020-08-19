
function Compile-Project($slnfile) {

    if(test-path($slnfile)) {
        
        $pinfo = New-Object System.Diagnostics.ProcessStartInfo
        $pinfo.FileName = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
        $pinfo.RedirectStandardError = $true
        $pinfo.RedirectStandardOutput = $true
        $pinfo.UseShellExecute = $false
        $pinfo.Arguments = "$slnfile /t:Rebuild /p:WarningLevel=0"
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
    
}

function Resolve-PathSafe {
    param
    (
        [string] $Path
    )
      
    $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($Path)
}
function ConvertTo-Base64 {
    param
    (
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


$use_module = $true # if true, build will create CS file
$techniques_folder = "..\..\..\Techniques\"
$lib_folder = $PSScriptRoot + "\bin\debug\lib\"

New-Item -ItemType "directory" -Path $lib_folder 

write-host "

 █████╗ ██╗   ██╗████████╗ ██████╗    ██╗    ██╗ ██╗███╗   ██╗
██╔══██╗██║   ██║╚══██╔══╝██╔═████╗   ██║    ██║███║████╗  ██║
███████║██║   ██║   ██║   ██║██╔██║   ██║ █╗ ██║╚██║██╔██╗ ██║
██╔══██║██║   ██║   ██║   ████╔╝██║   ██║███╗██║ ██║██║╚██╗██║
██║  ██║╚██████╔╝   ██║   ╚██████╔╝██╗╚███╔███╔╝ ██║██║ ╚████║
╚═╝  ╚═╝ ╚═════╝    ╚═╝    ╚═════╝ ╚═╝ ╚══╝╚══╝  ╚═╝╚═╝  ╚═══╝
                                Build Script - vs 0.0.2
"

write-host "Hello, I'm trying to build your project because your lazy ass don't want to move files around."

$techniques = Get-Childitem –Path $techniques_folder

ForEach ($technique in $techniques) {

    write-host "[-] Checking $technique..."
    $technique_bin = "$techniques_folder$technique\bin\Debug\$technique.exe"
    
    # Compile project
    if(Compile-Project("$techniques_folder$technique\$technique.sln")) {
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