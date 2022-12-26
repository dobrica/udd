## **Description**

Example of indexing and searching textual documents, based on Elasticsearch. 
Client - Server architecture. Client web application provides functionality 
to add and index new scientific papers and to search trough collection of 
these papers by different criteria. It is built using Angular framework. 
Server provides REST API to support these functionalities, integrating 
Elasticsearch and RDB (SQLite), it is implemented using .Net core.

## **Tech stack** 
<img align="left" alt="Angular" width="30px" style="padding-right:10px;" src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/angularjs/angularjs-plain.svg" />
<img align="left" alt=".Net Core" width="30px" style="padding-right:10px;" src="https://upload.wikimedia.org/wikipedia/commons/e/ee/.NET_Core_Logo.svg" />
<img align="left" alt="Elasticsearch" width="30px" style="padding-right:10px;" src="https://cdn.worldvectorlogo.com/logos/elasticsearch.svg" />
<img align="left" alt="SQLite" width="30px" style="padding-right:10px;" src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/sqlite/sqlite-original.svg" />
<img align="left" alt="Docker" width="30px" style="padding-right:10px;" src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/docker/docker-plain-wordmark.svg" />
<br/>
<br/>


## **Functionalities**

* Adding scientific papers to database
* Indexing scientific papers in elasticsearch
* More Like This Search and String Query Search by:
    - magazine title
    - scientific paper title
    - authors firstname and lastname
    - keywords
    - scientific fields
    - contents of attached PDF file
    - combination of previous parameters using BooleanQuery (AND/OR operator)
    - support for PhrazeQuery
* query preprocessing with serbian-analyzer
* dynamic highlighter for search results
* link to download and preview PDF files

## **Easy deploy**

- Requirements: installed Docker, ~5GB of free space
- Build Docker image using Dockerfile from repo. It is based on Ubuntu 18.04 so it can be run on Linux or Windows
- Steps:
    - Download Dockerfile: https://github.com/dobrica/udd/blob/master/Dockerfile
    - Execute commands: 
        ```
        cd <path-to-Dockerfile>
        ```
        ```
        sudo docker build -t scientific-center .
        ```
        ```
        sudo docker run -dt -p 4200:4200 -p 9200:9200 -p 44370:44370 --name sc-test scientific-center
        ```
    - wait few moments until services are up
    - visit https://localhost:44370/testdata and accept self-signed certificate
    - client app is on http://localhost:4200

## **Development environment setup requirements**

1. Java 13.0.1
2. Angular CLI: 9.1.12
3. Node: 14.x
4. .Net Core 3.1
5. Elasticsearch 7.4 can be found here: 
    https://www.elastic.co/downloads/past-releases/elasticsearch-7-4-0
    just download and uzip then install plugins:
    1. ingest-attachment, execute
		```
        cd <path-to-installation-folder>/bin
        ```
        ```
        ./elasticsearch-plugin install ingest-attachment
        ```
	2. serbian-analyzer, download and instructions are available here:
        https://github.com/markomartonosi/udd06/tree/plugin-update 
		
## **How to run**

- start elasticsearch
    ```
    cd <path-to-installation-folder>/bin
    ```
    ```
    ./elasticsearch
    ```
- run .Net service
    - optional: to add and index test data: GET https://localhost:44370/testdata
    - *Visual Studio and Visual Studio Code will use config files and run service on port 44370, if running from CLI
     or any other IDE configure port if necessary
- to start Angular client app, default port 4200 http://localhost:4200/
    ```
    npm install -g @angular/cli
    ```
    ```
    npm update
    ```
    ```
    ng serve 
    ``` 
