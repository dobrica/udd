## Description

Example of indexing and searching textual documents, based on Elasticsearch.
Client - Server architecture.
Client web application provides functionality to add and index new scientific papers 
and to search trough collection of these papers by different criteria. It is built using
Angular framework. Server provides REST API to support these functionalities, 
integrating Elasticsearch and RDB (SQLite), it is implemented using .Net core.

> Tech stack: Angular, .Net core, Elasticsearch, SQLite

## Functionalities

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

## Requirements and Installation

1. Angular CLI: 9.1.12
2. Node: 12.14.0
3. .Net Core 3.1
4. Elasticsearch 7.4 can be found here: https://www.elastic.co/downloads/past-releases/elasticsearch-7-4-0 just download and uzip then install plugins:
	* ingest-attachment, execute    
        ./elasticsearch-plugin install ingest-attachment
	* serbian-analyzer, download and instructions are available here:
    
        https://github.com/markomartonosi/udd06/tree/plugin-update

## How to run

- start elasticsearch, default port 9200
- run .Net service
    - optional: to add and index test data: 
	GET https://localhost:44370/testdata
- for client app run 
	- npm install -g @angular/cli, only firstime
	- npm update, only firstime
	- ng serve to start Angular client app, default port 4200 http://localhost:4200/
