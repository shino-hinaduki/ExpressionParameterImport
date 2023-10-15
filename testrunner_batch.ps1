<#
.SYNOPSIS
Unity Test Runner

.DESCRIPTION
Test Automation

.EXAMPLE
./testrunner_batch.ps1 -UNITY "C:\Program Files\UnityHub\2019.4.31f1\Editor\Unity.exe"

#>
Param(
    [String]$Unity = "C:\Program Files\UnityHub\2019.4.31f1\Editor\Unity.exe",
    [String]$Location = ".",
    [String[]]$TestPlatforms = @("PlayMode", "EditMode"),
    [switch]$DryRun
)

function Show-TestSuites([System.Xml.XmlElement]$XmlObj) {
    foreach ($Key in @("test-run", "test-suite", "test-case")) {
        $Target = $XmlObj[$Key]
        if ($Target) {
            $Line = "[" + $Target.result + "][" + $Key + "] " + $Target.fullname
            Write-Output $Line

            if ($Target.result.Equals("Failed") -and $Target.failure) {
                Write-Warning $Target.failure.InnerText
            }

            foreach ($Child in $Target) {
                Show-TestSuites $Child $key
            }
        }
    }
}
function Test-UnityTestRunner([String]$TestPlatform, [bool]$DryRun) {
    $Argument = "-batchmode -runTests -projectPath $Location -testPlatform $TestPlatform -testResults testrunner_batch_result_$TestPlatform.xml -logFile testrunner_batch_log_$TestPlatform.log $OtherArgs"
    if (!$DryRun) {
        Write-Output "Run: $Unity $Argument"
        Start-Process -Wait -FilePath $Unity -ArgumentList $Argument
    }
    $TestXml = [XML](Get-Content "testrunner_batch_result_$TestPlatform.xml")
    Show-TestSuites $TestXml["test-run"]

    $Summary = "[RESULT][" + $TestPlatform + "][" + $TestXml["test-run"].result + "] " + $TestXml["test-run"].passed + " passed / " + $TestXml["test-run"].failed + " failed / " + $TestXml["test-run"].skipped + " skipped"
    Write-Output ""
    Write-Output "================================================================================"
    Write-Output $Summary
    Write-Output "================================================================================"
    Write-Output ""
}

foreach ($TestPlatform in $TestPlatforms) {
    Test-UnityTestRunner $TestPlatform $DryRun
}
