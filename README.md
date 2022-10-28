# Clean Architecture ASP.NET Core 6 APIs

## Project Overview
This is an ASP.NET Core 6 application that serves APIs to manage real-world addresses, allowing you to Add, Delete, getById, and Edit an Addresse, 
as well as see all addresses in the database, do a dynamic search for an address, and sort addresses.
The code is very clean and it is very manageable. You can add your own models, validations, logic, etc. You can also create MVC project on top of it,
orjust use it as a api, deployed locally or publish on some server and use some front-end framework to show the data.

## Principle, Patterns and external libraries used.
1. MicrosoftEntityFrameWorkCore
2. MicrosoftEntityFrameWorkCore.Design
3. Swachbuckle.AspNetCore
4. Geocoding.Core
5. Geocoding.Google
6. GeoLocation
7. GoogleMaps.LocationServices
   * note : you don't need to install these packages, they are already installed you just need to clone the project
## Install and test the project
1. Download the code - Clone the repository or download the zip file
2. you don't need to Change the connection string in the appsettings.json file because the API uses a simple SQLite database, and the database dile (.net6addressesapi-database.db) is integrated in the project
   * note : if you want open the data base file just download and install the Sqlite db Browser [SqliteDbBowser](https://sqlitebrowser.org/dl/)
3. Run the Project
4. And now you can test the APIs with Swagger

## Testing the APIs
* Post Method AddAdress(Address newAddress), you don't need to add the primary key value, it will be generated auto, note that all field are required!
* Get Method GetAllAddresses(), this method return all the addresses in the data base
* Put Method EditAddress(Address newAddress), you add an address response body with an id of the address you wanna edit, if the address exist their fields will be updated with the new Address
* GetById Method GetAddressById(int id), this method will find and return the address by id if it is exist in the data base
* Delete Method DeleteAddressById(int id), this method will delete the address by id
* Sort Method GetAllSorted(string AscOrDesc), if the parameter is "Desc" the method return the list of addresses sorted in descending order else in Ascending order
* search Method Search(string Value), you enter a string value as a parameter and the method return the addresses that are one of their field contain this value
* distance Test Method distanceBetweenTostaticCoordinates(), it is a static method that use the api the package [GeoLocation](https://www.nuget.org/packages/Geolocation), this method take coordinates of 2 addresses and return the distance between them
* note : - To calculate distance between 2 addresses i used the method DistanceBetweenAddresses(int address1Id, int address2Id) in this method i tried to pass 2 parameters in the request for the first and the second address that i want to
           know the distance between them if the 2 addresses are not null, then i used the package [GoogleMaps.LocationServices](https://www.nuget.org/packages/GoogleMaps.LocationServices) and  that suposed to give me the coordinates of each address, then when i have the coordinates 
           the package GeoLocation (https://www.nuget.org/packages/Geolocation) calculate the distance between them 
           issue: the package Geolocation work successfuly, but GoogleMaps.LocationServices doesen't work so i make this method as a comment and add it to my to do list
         
         - I tried to find a way to sort the Addresses dynamically, so that if an address attribut was added, I don't have to add extra code for sorting, by getting the field name as a parameter and then sort addresses by that parameter but I couldn't find a way to convert a string to an instance attribute

## Parts I am proud of
* The code is very clean and it is very manageable
* the search method is dynamic and check all the fields of the instance 

## To Do 
* Solve tho issue of the method DistanceBetweenAddresses(int address1Id, int address2Id), try to find another package so that can give me the coordinates of an Address while the input is the address instance
* Sort the list of Addresses dynamically, so that if an address field was added, I don't have to add extra code for filtering and sorting






