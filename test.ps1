$policyScript = Invoke-WebRequest https://raw.githubusercontent.com/DFE-Digital/operations-devops-tools/master/Powershell/B2C/deployPolicy.ps1
$scriptBlock = [Scriptblock]::Create($policyScript.Content)
$policyPath = $PSScriptRoot + "\policy\TrustFrameworkBase.xml"
Invoke-Command -ScriptBlock $scriptBlock -ArgumentList ($args + @('0b0e3a7d-84b2-4c12-ae97-b40db4120a49', '-Xm~Te3xtHi_AGZ~k4sMdP66HJ-9jKUeUa', 'ce495425-4863-4e46-aa86-4a4e5a5bac0d', 'B2C_1A_TrustFrameworkBase_invitation', $policyPath))


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