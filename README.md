## Introduction
Create reports (CRUD).
Add notes to the reports (CRUD).
A note can  not be add if the report(report id) does not exist.


## Run the application
docker build -t reports-image -f Dockerfile .
docker run -p 8080:80 --name reports reports-image

## Run Component/Integration  tests
docker-compose up --abort-on-container-exit --build