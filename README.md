# Database Manager - Traffic Accidents

## Purpose
This project was created for a school project. The focus was on interaction with Cassandra in C#.
The application allows you to view, add, edit and delete traffic accidents in a database. I use Cassandra as the database technology. The dataset with which the application works is provided by [opendata.swiss](https://opendata.swiss/).
[The dataset](https://opendata.swiss/de/dataset/strassenverkehrsunfalle1) is a compilation of traffic accidents since 2011 in the canton Basel-Stadt.

## Requirements

In order to use this application you need to install [Docker Desktop](https://www.docker.com/products/docker-desktop/) on your machine.
The dataset must be deployed using the image `datastax/dse-server:6.8.12`, an Apache Cassandra distribution by [DataStax](https://www.datastax.com/). It is deployed on [DockerHub](https://hub.docker.com/r/datastax/dse-server/).

## Setup

To use the program, you must follow the following step-by-step instructions:

1. Start Docker Desktop and wait for the Docker daemon to start.
2. Run the docker container: `docker run -e DS_LICENSE=accept --name CONTAINER_NAME_HERE -d -p 9042:9042 datastax/dse-server:6.8.12`
3. Start your container: `docker start CONTAINER_NAME_HERE`
4. Access the Cassandra Shell using Docker: `docker exec -it lb165 cqlsh`
5. In your database, create a new keyspace named "traffic": `CREATE KEYSPACE traffic
WITH replication = {'class': 'SimpleStrategy', 'replication_factor': 1};`
6. Create a table named "accidents": `CREATE TABLE traffic.accidents (
    geo_point_2d text,
    geo_shape text,
    id_unfall int PRIMARY KEY,
    typ text,
    schwere text,
    jahr int,
    monat int,
    wochentag text,
    stunde int,
    strasseart text,
    fussgg_bet boolean,
    fahrrd_bet boolean,
    motord_bet boolean
);`
7. Copy the .csv file provided by opendata.swiss into the Docker container: `docker cp "LOCAL_PATH_TO_CSV_FILE" CONTAINER_NAME_HERE:/mnt/NAME_OF_CSV_FILE`
   You can find the dataset used for this application [here](https://opendata.swiss/de/dataset/strassenverkehrsunfalle1).
8. Again using the Cassandra Shell, copy the dataset into your database: `COPY traffic.accidents (geo_point_2d, geo_shape, id_unfall, typ, schwere, jahr, monat, wochentag, stunde, strasseart, fussgg_bet, fahrrd_bet, motord_bet)
FROM '/mnt/accidents.csv'
WITH DELIMITER = ';'
AND HEADER = TRUE;`
