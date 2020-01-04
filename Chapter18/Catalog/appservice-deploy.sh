#!/bin/bash

# Set the service group name
export resource_group=handsOnAppService
# Set the plan
export plan=catalogServicePlan
# Set the service name
export app_service_name=catalog-srv
# Set the api ASPNETCORE_ENVIRONMENT variables
export environment=StageAppServices
# Defines the ACR registry URL
export registry_address=<registry_name>.azurecr.io


# Create the app service
az webapp create --resource-group ${resource_group} \
                             --plan ${plan} \
                             --name ${app_service_name} \
                             --deployment-container-image-name ${registry_address}/catalog_api:v1
                             
# Set the ASPNETCORE_ENVIRONMENT variable
az webapp config appsettings set -g ${resource_group} \
                                             -n ${app_service_name} \
                                             --settings ASPNETCORE_ENVIRONMENT=${environment}