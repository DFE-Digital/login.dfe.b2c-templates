$policyScript = Invoke-WebRequest https://raw.githubusercontent.com/DFE-Digital/operations-devops-tools/master/Powershell/B2C/deployPolicy.ps1
$scriptBlock = [Scriptblock]::Create($policyScript.Content)
$policyPath = $PSScriptRoot + "\policy\TrustFrameworkBase.xml"
Invoke-Command -ScriptBlock $scriptBlock -ArgumentList ($args + @('0b0e3a7d-84b2-4c12-ae97-b40db4120a49', 'OGFNF2-Rz5w_6h6.DoP~P7zJNU9ARjoO75', 'ce495425-4863-4e46-aa86-4a4e5a5bac0d', 'B2C_1A_TrustFrameworkBase_invitation', $policyPath))


