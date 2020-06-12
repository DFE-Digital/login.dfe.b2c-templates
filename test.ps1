# $policyScript = Invoke-WebRequest https://raw.githubusercontent.com/DFE-Digital/operations-devops-tools/master/Powershell/B2C/deployPolicy.ps1
# $scriptBlock = [Scriptblock]::Create($policyScript.Content)
# $policyPath = $PSScriptRoot + "\policy\TrustFrameworkBase.xml"
# Invoke-Command -ScriptBlock $scriptBlock -ArgumentList ($args + @('456313e4-5edf-499b-b3a4-ab54e79c22d4', '8_bDl1v-6b6N1IpaY.YCdCOSctltpWz~Ph ', 'ce495425-4863-4e46-aa86-4a4e5a5bac0d', 'B2C_1A_TrustFrameworkBase_invitation', $policyPath))

$tenantId = "ce495425-4863-4e46-aa86-4a4e5a5bac0d"
$clientId = "456313e4-5edf-499b-b3a4-ab54e79c22d4"
$secret = "8_bDl1v-6b6N1IpaY.YCdCOSctltpWz~Ph"
$scriptPath = ".\scripts\UploadFrameworkPolicy.ps1"

$policyId = "B2C_1A_TrustFrameworkBase_Invitation"
$policyFile = ".\policy\TrustFrameworkBase_Invitation.xml"
Invoke-Expression "$scriptPath $clientId $secret $tenantId $policyId $policyFile"

$policyId = "B2C_1A_TrustFrameworkExtensions_Invitation"
$policyFile = "$($env:BUILD_SOURCESDIRECTORY)\policy\TrustFrameworkBase_Invitation.xml"
Invoke-Expression "$scriptPath $clientId $secret $tenantId $policyId $policyFile"

$policyId = "B2C_1A_Account_Signup"
$policyFile = "$($env:BUILD_SOURCESDIRECTORY)\policy\Account_Signup.xml"
Invoke-Expression "$scriptPath $clientId $secret $tenantId $policyId $policyFile"

$policyId = "B2C_1A_Find_Email"
$policyFile = "$($env:BUILD_SOURCESDIRECTORY)\policy\Find_Email.xml"
Invoke-Expression "$scriptPath $clientId $secret $tenantId $policyId $policyFile"

$policyId = "B2C_1A_Password_Reset_Confirmation"
$policyFile = "$($env:BUILD_SOURCESDIRECTORY)\policy\Password_Reset_Confirmation.xml"
Invoke-Expression "$scriptPath $clientId $secret $tenantId $policyId $policyFile"

$policyId = "B2C_1A_Password_Reset"
$policyFile = "$($env:BUILD_SOURCESDIRECTORY)\policy\Password_Reset.xml"
Invoke-Expression "$scriptPath $clientId $secret $tenantId $policyId $policyFile"

$policyId = "B2C_1A_Signin_Invitation"
$policyFile = "$($env:BUILD_SOURCESDIRECTORY)\policy\Signin_Invitation.xml"
Invoke-Expression "$scriptPath $clientId $secret $tenantId $policyId $policyFile"

$policyId = "B2C_1A_Signup_Confirmation"
$policyFile = "$($env:BUILD_SOURCESDIRECTORY)\policy\Signup_Confirmation.xml"
Invoke-Expression "$scriptPath $clientId $secret $tenantId $policyId $policyFile"

$policyId = "B2C_1A_Signup_Invitation"
$policyFile = "$($env:BUILD_SOURCESDIRECTORY)\policy\Signup_Invitation.xml"
Invoke-Expression "$scriptPath $clientId $secret $tenantId $policyId $policyFile"

##ADO-PipelineManagement

##clientId : 0b0e3a7d-84b2-4c12-ae97-b40db4120a49
##tenantId : ce495425-4863-4e46-aa86-4a4e5a5bac0d
##objectId : 21205a86-02c6-47e5-938d-525b97a53a1d
##secret   : -Xm~Te3xtHi_AGZ~k4sMdP66HJ-9jKUeUa


# [Cmdletbinding()]
# Param(
#     [Parameter(Mandatory = $true)][string]$ClientID,
#     [Parameter(Mandatory = $true)][string]$ClientSecret,
#     [Parameter(Mandatory = $true)][string]$TenantId,
#     [Parameter(Mandatory = $true)][string]$PolicyId,
#     [Parameter(Mandatory = $true)][string]$PathToFile
# )