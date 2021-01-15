# Catch the Banana (meeting edition) ![Monkey](https://raw.githubusercontent.com/JanneMattila/catch-the-banana-meeting-edition/main/src/CTB/Client/wwwroot/images/monkey1.png)

## Build Status

[![Build Status](https://dev.azure.com/jannemattila/jannemattila/_apis/build/status/JanneMattila.catch-the-banana-meeting-edition?branchName=main)](https://dev.azure.com/jannemattila/jannemattila/_build/latest?definitionId=56&branchName=main)
[![Docker Pulls](https://img.shields.io/docker/pulls/jannemattila/catch-the-banana?style=plastic)](https://hub.docker.com/r/jannemattila/quizsim)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FJanneMattila%2Fcatch-the-banana-meeting-edition%2Fmain%2Fdeploy%2Fazuredeploy.json)

## Background

Original [Catch the Banana](https://arcade.makecode.com/29959-08678-58221-91989)
was done using [Microsoft MakeCode](https://arcade.makecode.com/) and it was
practice work for young developer.

This has been now converted into multiplayer version!
![Monkey](https://raw.githubusercontent.com/JanneMattila/catch-the-banana-meeting-edition/main/src/CTB/Client/wwwroot/images/monkey2.png)
Now you can play with your friends or with you collegues. Especially when
you're waiting for your meeting to start:
_"Let's give it a minute for people to join"_ can be now changed to
_"Hey let's play few minutes Catch the Banana ![Banana](https://raw.githubusercontent.com/JanneMattila/catch-the-banana-meeting-edition/main/src/CTB/Client/wwwroot/images/banana.png)
while we're waiting people to join"_!
Hence the name Catch the Banana (meeting edition).
![Monkey](https://raw.githubusercontent.com/JanneMattila/catch-the-banana-meeting-edition/main/src/CTB/Client/wwwroot/images/monkey3.png)

![Monkey](https://user-images.githubusercontent.com/2357647/104758170-b4691b80-5766-11eb-8c5a-2aa8cc3d9c4e.gif)

## Working with 'Catch the Banana'

### How to create image locally

```batch
# Build container image
docker build . -t catch-the-banana:latest

# Run container using command
docker run -p "2001:80" catch-the-banana:latest
``` 

### How to deploy to Azure App Service

Deploys Linux App Service and with
container from [Docker Hub](https://hub.docker.com/r/jannemattila/catch-the-banana)
![Monkey](https://raw.githubusercontent.com/JanneMattila/catch-the-banana-meeting-edition/main/src/CTB/Client/wwwroot/images/monkey1.png).

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FJanneMattila%2Fcatch-the-banana-meeting-edition%2Fmain%2Fdeploy%2Fazuredeploy.json)

### How to deploy to Azure Container Instances (ACI)

Deploy published image to [Azure Container Instances (ACI)](https://docs.microsoft.com/en-us/azure/container-instances/) the Azure CLI way:

```batch
# Variables
aciName="catch-the-banana"
resourceGroup="catch-the-banana-dev-rg"
location="westeurope"
image="jannemattila/catch-the-banana"

# Login to Azure
az login

# *Explicitly* select your working context
az account set --subscription <YourSubscriptionName>

# Create new resource group
az group create --name $resourceGroup --location $location

# Create ACI
az container create --name $aciName --image $image --resource-group $resourceGroup --ip-address public

# Show the properties
az container show --name $aciName --resource-group $resourceGroup

# Show the logs
az container logs --name $aciName --resource-group $resourceGroup

# Wipe out the resources
az group delete --name $resourceGroup -y
``` 

Deploy published image to [Azure Container Instances (ACI)](https://docs.microsoft.com/en-us/azure/container-instances/) the Azure PowerShell way:

```powershell
# Variables
$aciName="catch-the-banana"
$resourceGroup="catch-the-banana-dev-rg"
$location="westeurope"
$image="jannemattila/catch-the-banana"

# Login to Azure
Login-AzAccount

# *Explicitly* select your working context
Select-AzSubscription -SubscriptionName <YourSubscriptionName>

# Create new resource group
New-AzResourceGroup -Name $resourceGroup -Location $location

# Create ACI
New-AzContainerGroup -Name $aciName -Image $image -ResourceGroupName $resourceGroup -IpAddressType Public

# Show the properties
Get-AzContainerGroup -Name $aciName -ResourceGroupName $resourceGroup

# Show the logs
Get-AzContainerInstanceLog -ContainerGroupName $aciName -ResourceGroupName $resourceGroup

# Wipe out the resources
Remove-AzResourceGroup -Name $resourceGroup -Force
```

![Monkey](https://raw.githubusercontent.com/JanneMattila/catch-the-banana-meeting-edition/main/src/CTB/Client/wwwroot/images/monkey2.png)
![Shark](https://raw.githubusercontent.com/JanneMattila/catch-the-banana-meeting-edition/main/src/CTB/Client/wwwroot/images/shark.png)
