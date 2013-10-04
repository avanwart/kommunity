#clean iis logs

#if it prompts with an execution policy message, run: & "$Env:SystemRoot\Microsoft.NET\Framework64\v2.0.50727\caspol.exe" -machine -listgroups

Import-Module WebAdministration

$DaysToRetain = 30


function DeleteLogsForSite($siteName){

    $website = Get-item IIS:\Sites\$siteName

    $LogFileDirectory = $website.logfile.directory

    if ($LogFileDirectory -match "(%.*%)\\") {
        $LogFileDirectory = $LogFileDirectory -replace "%(.*%)\\","$(cmd /C echo $matches[0])"
     }
 
    $toBeDeleted = Get-ChildItem -Path $LogFileDirectory -Recurse | where {
        $_.LastWriteTime -lt (Get-Date).adddays(-$DaysToRetain) -and $_.PSIsContainer -eq $false
    }
 
    if ($toBeDeleted -ne $null) {
        $toBeDeleted | % {
            $websiteID = $website.id
            $filePath = "$LogFileDirectory\W3SVC$websiteID\$_"
        
            if(Test-Path $filePath)
            {
                Write-Host "Deleting.. $filePath"
                Remove-Item $filePath
            }
        }
     }
 
}

$allWebsites = Get-Website | select name

 ForEach ($site in  $allWebsites)  {
     DeleteLogsForSite($site.name)
 }