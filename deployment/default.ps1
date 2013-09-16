# Deployment script for all environments

properties {
 
    #msbuild
    if ( $msBuildConfig -eq $null){$msBuildConfig = 'debug'}
    if ( $msBuildVerbosity -eq $null){$msBuildVerbosity = 'normal'}
    if ( $solutionLocation -eq $null){$solutionLocation = '' }
     
    #DB migrations
    if ( $migrateConnectionString -eq $null){$migrateConnectionString = ''}
    if ( $migrateDBParams -eq $null){$migrateDBParams = '/verbose'}
    if ( $migrateApplicationDLL -eq $null){$migrateApplicationDLL = ''}
    if ( $migrateExeLocation -eq $null) { $migrateExeLocation = ''}
    if ( $removeMigrateLocation -eq $null) { $removeMigrateLocation = ''}
    if ( $dbUpdate -eq $null){$dbUpdate = ''}
    if ( $webprojectBinLocation -eq $null) { $webprojectBinLocation = ''}
    if ( $migrationProjectBinLocation -eq $null) { $migrationProjectBinLocation = ''}    
  
    #Package
    if ( $packageOutputDir -eq $null) { $packageOutputDir =  '.\buildartifacts\' }
     
    #Deployment
    if ( $msDeployURL -eq $null){$msDeployURL = '' }
    if ( $msDeployUserName -eq $null) {$msDeployUserName = ''}
    if ( $msDeployPassword -eq $null) {$msDeployPassword = ''}
    if ( $webProjectLocation -eq $null){ $webProjectLocation = '' }
    if ( $statusCheckURL -eq $null){$statusCheckURL = '' }
    if ( $deployIisAppPath -eq $null){$deployIisAppPath = '' }
    
    #tests
    if ( $integrationTestDLLLocation -eq $null) { $integrationTestDLLLocation = '' }
    if ( $unitTestDLLLocation -eq $null) { $unitTestDLLLocation = '' }   
    if ( $MSTestLocation -eq $null) { $MSTestLocation = '' }   
    if ( $unitTestResultsFile -eq $null) { $unitTestResultsFile = '' }   
    if ( $testSettings -eq $null) { $testSettings = '' }   
    if ( $integrationTestResultsFile -eq $null) { $integrationTestResultsFile = '' }   
    
    #display
    if ( $displayTaskStartStopTimes -eq $null){ $displayTaskStartStopTimes = $false } 
    if ( $showConfigsAtStart -eq $null) { $showConfigsAtStart =  $false }
} 

#psake functions
FormatTaskName {
   param($taskName)
  
   write-host "----- Task: $taskName -----"   -foregroundcolor Cyan
}

TaskSetup {
    if ( $displayTaskStartStopTimes)
    {
        $currentTaskTime = Get-date
        $currentTaskTime = $currentTaskTime.ToUniversalTime().ToString("u")
        Write-Host "Begin: $currentTaskTime" -ForegroundColor DarkGray
    }
}

TaskTearDown {
    if ( $displayTaskStartStopTimes)
    {
        $currentTaskTime = Get-date
        $currentTaskTime = $currentTaskTime.ToUniversalTime().ToString("u")
        Write-Host "End: $currentTaskTime" -ForegroundColor DarkGray
    }
}
 
#tasks



task -name LoadCon -description "Lists configs"   -action {

    if ( Test-Path $configSettingsFile )
    {
        Include $configSettingsFile
    }
}


 
task -name ValidateConfigs -depends  ListConfigs   -description "Validates that configs which require a value, have a value" -action {

 
    assert( 'debug', 'release' -contains $msBuildConfig) `
    "Invalid msBuildConfig: $msBuildConfig, must be: 'debug' or 'release'"
    assert( 'q', 'quiet', 'm', 'minimal', 'n', 'normal', 'd', 'detailed', 'diag', 'diagnostic' -contains $msBuildVerbosity) `
    "Invalid msBuildVerbosity: $msBuildVerbosity, must be:  'q', 'quiet', 'm', 'minimal', 'n', 'normal', 'd', 'detailed', 'diag' or 'diagnostic'"
    assert( $solutionLocation -ne $null -and $solutionLocation -ne '') `
    "solutionLocation is blank"
    assert( $migrateConnectionString -ne $null -and $migrateConnectionString -ne '') `
    "migrateConnectionString is blank"
    assert( $migrateDBParams -ne $null -and $migrateDBParams -ne '') `
    "migrateDBParams is blank"
    assert( $migrateApplicationDLL -ne $null -and $migrateApplicationDLL -ne '') `
    "migrateApplicationDLL is blank"
    assert( $migrateExeLocation -ne $null -and $migrateExeLocation -ne '') `
    "migrateExeLocation is blank"
     assert( $webprojectBinLocation -ne $null -and $webprojectBinLocation -ne '') `
    "webprojectBinLocation is blank"
    assert( $packageOutputDir -ne $null -and $packageOutputDir -ne '') `
    "packageOutputDir is blank"
    assert( $msDeployURL -ne $null -and $msDeployURL -ne '') `
    "msDeployURL is blank"
    assert( $msDeployUserName -ne $null -and $msDeployUserName -ne '') `
    "msDeployUserName is blank"
    assert( $msDeployPassword -ne $null -and $msDeployPassword -ne '') `
    "msDeployPassword is blank"
    assert( $webProjectLocation -ne $null -and $webProjectLocation -ne '') `
    "webProjectLocation is blank"      
    assert( $statusCheckURL -ne $null -and $statusCheckURL -ne '') `
    "statusCheckURL is blank"      
    assert( $deployIisAppPath -ne $null ) `
    "deployIisAppPath is null"  
    assert( $MSTestLocation -ne $null -and $MSTestLocation -ne '') `
    "MSTestLocation is blank"      
    assert( $unitTestDLLLocation -ne $null -and $unitTestDLLLocation -ne '') `
    "unitTestDLLLocation is blank"      
    assert( $integrationTestDLLLocation -ne $null -and $integrationTestDLLLocation -ne '') `
    "integrationTestDLLLocation is blank"      
    assert( $displayTaskStartStopTimes -ne $null ) `
    "displayTaskStartStopTimes is null"  
    assert( $showConfigsAtStart -ne $null ) `
    "showConfigsAtStart is null"  
    assert( $migrationProjectBinLocation -ne $null ) `
    "migrationProjectBinLocation is null"  

 
};

 

task -name ListConfigs -description "Lists configs"   -action {
    

    if ( $showConfigsAtStart)
    {
       
       Write-Host '$msBuildConfig = ' $msBuildConfig -ForegroundColor Magenta
       Write-Host '$msBuildVerbosity = ' $msBuildVerbosity -ForegroundColor Magenta
       Write-Host '$solutionLocation = ' $solutionLocation -ForegroundColor Magenta
       Write-Host '$migrateConnectionString = ' $migrateConnectionString -ForegroundColor Magenta
       Write-Host '$migrateDBParams = ' $migrateDBParams -ForegroundColor Magenta
       Write-Host '$migrateApplicationDLL = ' $migrateApplicationDLL -ForegroundColor Magenta
       Write-Host '$migrateExeLocation = ' $migrateExeLocation -ForegroundColor Magenta
       Write-Host '$removeMigrateLocation = ' $removeMigrateLocation -ForegroundColor Magenta
       Write-Host '$webprojectBinLocation = ' $webprojectBinLocation -ForegroundColor Magenta
       Write-Host '$msDeployURL = ' $msDeployURL -ForegroundColor Magenta
       Write-Host '$msDeployUserName = ' $msDeployUserName -ForegroundColor Magenta
       Write-Host '$msDeployPassword = ' $msDeployPassword -ForegroundColor Magenta
       Write-Host '$webProjectLocation = ' $webProjectLocation -ForegroundColor Magenta
       Write-Host '$statusCheckURL = ' $statusCheckURL -ForegroundColor Magenta
       Write-Host '$deployIisAppPath = ' $deployIisAppPath -ForegroundColor Magenta
       Write-Host '$MSTestLocation = ' $MSTestLocation -ForegroundColor Magenta
       Write-Host '$unitTestDLLLocation = ' $unitTestDLLLocation -ForegroundColor Magenta
       Write-Host '$displayTaskStartStopTimes = ' $displayTaskStartStopTimes -ForegroundColor Magenta
       Write-Host '$showConfigsAtStart = ' $showConfigsAtStart -ForegroundColor Magenta
       Write-Host '$migrationProjectBinLocation = ' $migrationProjectBinLocation -ForegroundColor Magenta    
       Write-Host '$unitTestResultsFile = ' $unitTestResultsFile -ForegroundColor Magenta    
       Write-Host '$testSettings = ' $testSettings -ForegroundColor Magenta   
       Write-Host '$integrationTestResultsFile = ' $integrationTestResultsFile -ForegroundColor Magenta   
        
       
      
     }

};

task -name Build -description "Build the solution" -depends ValidateConfigs, ListConfigs -action { 
    exec  {
        msbuild $solutionLocation /t:build /verbosity:$msBuildVerbosity /p:configuration=$msBuildConfig /nologo
    }
};

task -name Clean -description "Cleans the solution" -depends ValidateConfigs -action { 
    exec  {
    
        msbuild $solutionLocation /t:clean /verbosity:$msBuildVerbosity /p:configuration=$msBuildConfig /nologo
    }
};

task -name Rebuild -depends Clean, Build -description "Cleans and builds the solution"  

task -name UnitTest -depends Rebuild -description "Runs unit tests" -action { 
    exec  {
        if(Test-Path $unitTestResultsFile)
        {
            Remove-Item $unitTestResultsFile
        }

        & $MSTestLocation   /testcontainer:$unitTestDLLLocation /detail:debugtrace /runconfig:$testSettings  /resultsfile:$unitTestResultsFile /nologo
    }
};

task -name IntegrationTest -depends Rebuild -description "Runs integration tests" -action { 
    exec  {
        
        if(Test-Path $integrationTestResultsFile)
        {
            Remove-Item $integrationTestResultsFile
        }

     #   & $MSTestLocation   /testcontainer:$integrationTestDLLLocation /detail:debugtrace /runconfig:$testSettings /resultsfile:$integrationTestResultsFile /nologo
    }
};

task -name PackageZip -depends Rebuild -description "Makes a zip package" -action { 
    exec  {
     msbuild $webProjectLocation /t:Package /verbosity:$msBuildVerbosity /p:Configuration=$msBuildConfig /p:OutDir=$packageOutputDir /nologo
  }
};


task -name Tests -depends UnitTest, IntegrationTest   -description "Runs all tests on application"  


task -name MigrateDB  -description "Runs migration of database"   -depend Tests  -action { 
   
    Copy-Item  $migrateExeLocation $migrationProjectBinLocation
     
    & $removeMigrateLocation $migrateApplicationDLL /connectionString="$migrateConnectionString" /connectionProviderName="System.Data.SqlClient" $migrateDBParams
     
    Remove-Item $removeMigrateLocation
  
    if($migrationOutput -ne $null -and $migrationOutput.Contains('Exception'))
    {
        throw $migrationOutput
    }
};


task -name DeployPackage -depends PackageZip, MigrateDB -description "Deploys package to environment" -action { 
    exec  {
        if ( $msBuildConfig -eq 'release') {
           
                msbuild $webprojectLocation `
                    /p:Configuration=$msBuildConfig `
                    /P:DeployOnBuild=True `
                    /P:DeployTarget=MSDeployPublish `
                    /P:MsDeployServiceUrl=$msDeployURL `
                    /P:AllowUntrustedCertificate=True `
                    /P:MSDeployPublishMethod=WMSvc `
                    /P:CreatePackageOnPublish=True `
                    /P:UserName=$msDeployUserName `
                    /P:Password=$msDeployPassword `
                    /p:DeployIisAppPath=$deployIisAppPath  `
                    /verbosity:$msBuildVerbosity /nologo

        }
        else 
        {
            Write-Host "Cannot deploy, $msBuildConfig" -ForegroundColor Blue -BackgroundColor White
        }
    }
};



task -name Pullcode  -description "Gets code before pushing it to GitHub" -action {

exec {
    git pull

}

};


task -name SafePush -depends Pullcode, UnitTest -description "Tests the code before pushing it to GitHub" -action {

exec {
    git push
}


};

task -name Begin -description "Resets values" -action {

    Write-Host "Resetting last exit code..."
    $LASTEXITCODE = 0
}

task -name Help -description "Prints helpful info" -action {


 
Write-Host "HOW TO USE PROPERITES:"
Write-Host "------------------------------------"
Write-Host "example:"
write-host ""
Write-Host ">invoke-psake .\deployment\default.ps1 monitor  -properties @{'statusCheckURL'='http://dasklub.com'}"


}


task -name Monitor -description "Hits the URLs forever" -action {

            $webClient = new-object System.Net.WebClient
            $webClient.Headers.Add("user-agent", "PowerShell Script")
            
            while (1 -eq 1) {
               $output = ""
               $startTime = get-date
               $output = $webClient.DownloadString($statusCheckURL);
               $endTime = get-date
               $totalSeconds = ($endTime - $startTime).TotalSeconds

                if($totalSeconds -gt 1)
                {
                    Write-Host $statusCheckURL  "`t`t" $startTime.TimeOfDay  "`t`t"  $totalSeconds  " seconds"   "`t`t"   $output.Length   " length" -BackgroundColor Red -ForegroundColor Black
                }
                elseif($totalSeconds -gt .5)
                {
                    Write-Host $statusCheckURL  "`t`t" $startTime.TimeOfDay  "`t`t"  $totalSeconds  " seconds"   "`t`t"   $output.Length   " length" -BackgroundColor Yellow -ForegroundColor Black
                }
                elseif($totalSeconds -gt .1)
                {
                    Write-Host $statusCheckURL  "`t`t" $startTime.TimeOfDay  "`t`t"  $totalSeconds  " seconds"   "`t`t"   $output.Length   " length" -BackgroundColor blue -ForegroundColor Black
                }
                else 
                {
                    Write-Host $statusCheckURL  "`t`t" $startTime.TimeOfDay  "`t`t"  $totalSeconds  " seconds"   "`t`t"   $output.Length   " length" 
                }
            }
        
}



task -name Deploy -depends DeployPackage   -description "Hits the homepage to ensure the package was deployed" -action {

    exec  {
     if ( $msBuildConfig -eq 'release') {
      Write-Host "Requesting URL: '$statusCheckURL'..."
        
        $webCheckResponse = Invoke-WebRequest  $statusCheckURL   

        Write-host "Status code: " $webCheckResponse.StatusCode

        if ( $webCheckResponse.StatusCode -eq '200')
        {

            Write-Host "EXIT: " $LASTEXITCODE -BackgroundColor Green -ForegroundColor Black
            
            Write-Host "
                _    _            _                      _      
  _ __  __ _ __| |_ (_)_ _  ___  (_)___  _ _ ___ __ _ __| |_  _ 
 | '  \/ _' / _| ' \| | ' \/ -_) | (_-< | '_/ -_) _' / _' | || |
 |_|_|_\__,_\__|_||_|_|_||_\___| |_/__/ |_| \___\__,_\__,_|\_, |
                                                           |__/   
                                                                           " 
      
       # Start-Process chrome $statusCheckURL
        }

     }
     else 
        {
            Write-Host "Cannot check url, '$msBuildConfig'"  -ForegroundColor Blue -BackgroundColor White
        }

       
    }

};

task -name default  -depends ListConfigs 



 