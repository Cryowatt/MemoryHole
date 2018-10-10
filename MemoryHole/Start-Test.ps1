[cmdletbinding()]
param()

function Start-MemoryTest {
    param (
        $DotnetVersion,
        $MemoryLimit
    )    

    $activity = "Running test on dotnet $dotnetVersion"
    Write-Progress -Activity $activity -Status "Building"
    docker build --build-arg DOTNET_VERSION=$dotnetVersion --tag memoryhole:$dotnetVersion . | Write-Verbose
    Write-Progress -Activity $activity -Status "Running test"
    docker rm memoryhole | Write-Verbose
    docker run --name memoryhole --memory=$memoryLimit memoryhole:$dotnetVersion | Write-Verbose
    $runResult = docker inspect memoryhole | ConvertFrom-Json
    $runResult[0].State
}

$memoryLimit = "128mb"
Start-MemoryTest -DotnetVersion "2.0" -MemoryLimit $memoryLimit
Start-MemoryTest -DotnetVersion "2.1" -MemoryLimit $memoryLimit
Start-MemoryTest -DotnetVersion "2.2" -MemoryLimit $memoryLimit
