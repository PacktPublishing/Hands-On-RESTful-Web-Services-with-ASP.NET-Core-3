#!/bin/bash

# Set the service group name
export resource_group=catalogServiceGroup
# Set the plan
export plan=catalogServicePlan
# Set the service name
export app_service_name=catalog-srv
# Set the api ASPNETCORE_ENVIRONMENT variables
export environment=StageAppService

# Create a resource group
az group create \
	--name ${resource_group} \
	--location westeurope

az appservice plan create \
    --name ${plan} \
    --resource-group ${resource_group} \
    --sku B1 \
    --is-linux

az webapp create --resource-group ${resource_group} \
                 --plan ${plan} \
                 --name ${app_service_name} \
                 --deployment-container-image-name myprivatecontainers.azurecr.io/catalog_api:v1

az webapp config appsettings set -g ${resource_group} \
                                 -n ${app_service_name} \
                                 --settings ASPNETCORE_ENVIRONMENT=${environment}
