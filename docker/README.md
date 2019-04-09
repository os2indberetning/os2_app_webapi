# Setup

## Starting the MySQL database
```
> cd docker
> docker-compose up -d
```
## Setting up the `Presentation.Web` project
Edit your `connections.config` file in the project to point to the local database
```xml
<add name="DefaultConnection" providerName="MySql.Data.MySqlClient" connectionString="Data Source=127.0.0.1; port=3306; Initial Catalog=db; uid=user; pwd=password; CharSet=utf8; Allow User Variables=True" />
```
## Restoring from a database dump
Place your exported database files (`*.sql`) in the `docker/os2dmzdb/dump` folder and run the following commands:
```
> cd docker
> docker-compose exec os2dmzdb bash
```
Now, from inside the terminal in the docker container, run the following commands:
```
> cd /os2dmzdbdump
> ./restore.sh
```