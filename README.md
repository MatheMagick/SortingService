# SortingService

A sample WCF streaming service that sorts a file by receiving it (probably in multiple chunks) and then calling a simplified version of merge sort. The client can at all times call a get opertaion, that returns the sorted file

The service is implemented, as well as two client:

1. A client that uses the service and sends three request, and after each request asserts that the response received is correct


2. A client that sorts a 16 GB file by sending it to the service, and then gets the result in a local file
