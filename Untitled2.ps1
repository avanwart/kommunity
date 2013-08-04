﻿
if ( Test-Path .\configs.ps1 )
{
    Include ".\configs.ps1"
}

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
    if ( $removeMigrateLoation -eq $null) { $removeMigrateLoation = ''}
    if ( $dbUpdate -eq $null){$dbUpdate = ''}
    if ( $webprojectBinLocation -eq $null) { $webprojectBinLocation = ''}
  
    #Package
    if ( $packageOutputDir -eq $null) { $packageOutputDir =  '.\buildartifacts\' }
     
    #Deployment
    if ( $msDeployURL -eq $null){$msDeployURL = '' }
    if ( $msDeployUserName -eq $null) {$msDeployUserName = ''}
    if ( $msDeployPassword -eq $null) {$msDeployPassword = ''}
    if ( $webProjectLocation -eq $null){ $webProjectLocation = '' }
    if (  $statusCheckURL -eq $null){$statusCheckURL = '' }
    if (  $deployIisAppPath -eq $null){$deployIisAppPath = '' }
    

    #unit tests
    if ( $MSTestLocation -eq $null) { $MSTestLocation = '' }
    if ( $testDLLLocation -eq $null) { $testDLLLocation = '' }   

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

task -name MAKEITWORK -action { 

Out-File -FilePath ".\test2.txt" -Append


Write-Host "trying..." -BackgroundColor Magenta -ForegroundColor Black
Write-Host $MSTestLocation



Start-Sleep -s 7
};

 
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
    assert( $testDLLLocation -ne $null -and $testDLLLocation -ne '') `
    "testDLLLocation is blank"      
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
       Write-Host '$removeMigrateLoation = ' $removeMigrateLoation -ForegroundColor Magenta
       Write-Host '$webprojectBinLocation = ' $webprojectBinLocation -ForegroundColor Magenta
       Write-Host '$msDeployURL = ' $msDeployURL -ForegroundColor Magenta
       Write-Host '$msDeployUserName = ' $msDeployUserName -ForegroundColor Magenta
       Write-Host '$msDeployPassword = ' $msDeployPassword -ForegroundColor Magenta
       Write-Host '$webProjectLocation = ' $webProjectLocation -ForegroundColor Magenta
       Write-Host '$statusCheckURL = ' $statusCheckURL -ForegroundColor Magenta
       Write-Host '$deployIisAppPath = ' $deployIisAppPath -ForegroundColor Magenta
       Write-Host '$MSTestLocation = ' $MSTestLocation -ForegroundColor Magenta
       Write-Host '$testDLLLocation = ' $testDLLLocation -ForegroundColor Magenta
       Write-Host '$displayTaskStartStopTimes = ' $displayTaskStartStopTimes -ForegroundColor Magenta
       Write-Host '$showConfigsAtStart = ' $showConfigsAtStart -ForegroundColor Magenta
      
     }

};

task -name Build -description "Build the solution" -depends ValidateConfigs, ListConfigs -action { 
    exec  {
        msbuild $solutionLocation /t:build /verbosity:$msBuildVerbosity /p:configuration=$msBuildConfig
    }
};

task -name Clean -description "Cleans the solution" -depends ValidateConfigs -action { 
    exec  {
    
        msbuild $solutionLocation /t:clean /verbosity:$msBuildVerbosity /p:configuration=$msBuildConfig
    }
};

task -name Rebuild -depends Clean, Build -description "Cleans and builds the solution"  

task -name UnitTest -depends Rebuild -description "Runs unit tests" -action { 
    exec  {
     

   & $MSTestLocation   /testcontainer:$testDLLLocation /detail:debugtrace /runconfig:$testSettings
    }
};



task -name PackageZip -depends UnitTest -description "Makes a zip package" -action { 
    exec  {
     msbuild $webProjectLocation /t:Package /verbosity:$msBuildVerbosity /p:Configuration=$msBuildConfig /p:OutDir=$packageOutputDir
  }
};

task -name MigrateDB -depends UnitTest   -description "Runs migration of database" -action { 
    exec  {

        Copy-Item  $migrateExeLocation $migrationProjectBinLocation

        & $removeMigrateLoation $migrateApplicationDLL /connectionString="$migrateConnectionString" /connectionProviderName="System.Data.SqlClient" $migrateDBParams
      
        Remove-Item $removeMigrateLoation
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
                    /verbosity:$msBuildVerbosity

        }
        else 
        {
            Write-Host "Cannot deploy, $msBuildConfig" -ForegroundColor Blue -BackgroundColor White
        }
    }
};



task -name Pullcode  -description "Tests the code before pushing it to GitHub" -action {

exec {
git pull


}


};


task -name SafePush -depends Pullcode, UnitTest -description "Tests the code before pushing it to GitHub" -action {

exec {
    git push
}


};

task -name WaitingVideo -description "Loads YouTube waiting video" -action {

    Write-Host "Resetting last exit code..."
    $LASTEXITCODE = 0

}


task -name Deploy -depends WaitingVideo, DeployPackage  -description "Hits the homepage to ensure the package was deployed" -action {

    exec  {
     if ( $msBuildConfig -eq 'release') {
      Write-Host "Requesting URL: '$statusCheckURL'..."
        
        $webCheckResponse = Invoke-WebRequest  $statusCheckURL   

        Write-host "Status code: " $webCheckResponse.StatusCode


        if ( $webCheckResponse.StatusCode -eq '200')
        {
            
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