#!/bin/bash

# Set the service group name
export resource_group=catalogServiceGroup
# Set the sql container name
export sql_name=catalog-db
# Set the sql admin password
export sql_admin_pass=P@ssw0rd
# Set the event service bus name
export esb_name=catalog-esb
# Set the event service bus username
export rabbitmq_user=guest
# Set the event service bus password
export rabbitmq_pass=guest
# Set the cache container name
export cache_name=catalog-cache
# Set the service group name
export resource_group=catalogServiceGroup
# Set the service name
export api_name=catalogapi
# Set the api ASPNETCORE_ENVIRONMENT variables
export environment=Stage
# Set the registry password
export registry_password=<registry_password>



az container create --resource-group ${resource_group} \
                    --location westeurope \
                    --name ${sql_name} \
                    --image microsoft/mssql-server-linux \
                    --cpu 1 \
                    --memory 1 \
                    --dns-name-label ${sql_name} \
                    --ip-address public \
                    --ports 1433 \
                    --environment-variables ACCEPT_EULA=Y SA_PASSWORD=${sql_admin_pass}
                    
az container create --resource-group ${resource_group} \
                    --location westeurope \
                    --name ${esb_name} \
                    --image rabbitmq:3-management \
                    --cpu 1 \
                    --memory 1 \
                    --dns-name-label ${esb_name} \
                    --ip-address public \
                    --ports 5672 \
                    --environment-variables RABBITMQ_DEFAULT_USER=${rabbitmq_user} RABBITMQ_DEFAULT_PASS=${rabbitmq_pass} 

az container create --resource-group ${resource_group} \
                    --name ${cache_name} \
                    --image redis:alpine \
                    --cpu 1 \
                    --memory 1 \
                    --dns-name-label ${cache_name} \
                    --ip-address public \
                    --ports 6379 

az container create --resource-group ${resource_group} \
                    --location westeurope \
                    --name ${api_name} \
                    --image myprivatecontainers.azurecr.io/catalog_api:v1 \
                    --cpu 1 \
                    --memory 1 \
                    --dns-name-label ${api_name} \
                    --ip-address public \
                    --ports 80 \
                    --environment-variables ASPNETCORE_ENVIRONMENT=${environment} --registry-password=${registry_password}

