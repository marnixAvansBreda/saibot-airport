echo "====================="
echo "====== Volumes ======"
echo "====================="
docker volume create --name=sqlserverdata
docker volume create --name=rabbitmqdata
docker volume create --name=sqlserverflightscheduledata
docker volume create --name=sqlserverflightmanagementdata
docker volume create --name=baggagesetdata
docker volume create --name=sqlserverflightplanningmanagementdata

# Rebuild all the services that have changes
# If you want to (re)build only a specific service, go to the src folder and execute `docker-compose build <servicename-lowercase>`
docker-compose build --force-rm
