
#Login-AzureRmAccount
Select-AzureRmSubscription -Subscription "C106-API Management Service-test"
$apimRg = "c106t01-eapim-custeng"
$apimName = "c106t01-apim-custeng"
$policyFile = "C:\Repos\Github\DFE-Digital\login.dfe.b2c-templates\eapim\api\apim.xml"

$ctx = New-AzureRmApiManagementContext -ResourceGroupName $apimRg -ServiceName $apimName
$api = Get-AzureRmApiManagementApi -Context $ctx -Name "NCS Identity"
Get-AzureRmApiManagementPolicy -Context $ctx -ApiId $api.ApiId -SaveAs $policyFile