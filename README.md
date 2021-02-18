# AssetApi
###### Simple API created with ASP.Net Core and SQL

#### TABLE Content
* [Introduction](#introduction)
* [Technologies](#technologies)
* [Project Explanation](#project-explanation)

#### Introduction
The project consists of API implementation, which consist of updating the assets property from the CSV file. 
This was the task provided from one of the recruiters.

#### Technologies
* ASP.Net Core 3.1
* C#
* EntityFramework Core
* SQL


#### Launch
Project Requires a Visual Studio and docker setup to run the project

#### Project Explanation
1) **POST method asset master**: Post method is created to save the asset in master table  
     **input json** :  
                   ```
                    {
                              "name" : "Laptop"
                    }    
                   ```   
     **output json** :  
                    ```
                    {
                              "id":1,
                              "name":"Laptop"
                    }
                    ```

2) **GET method on asset properties** : http request for assets is used to get the list of the assets Ids based on the search criteria 
if no paramters is passed then list of all assets are retrieved. 
input parameters: Property name and value.
From this http request you can get the asset Id and the Property Ids associated with Assets  
          if Property is not selected as a paramter it will return all the assets with the properties:
 ``` 
  {
    "name": "Laptop",
    "properties": [
      {
        "id": 1,
        "asset": null,
        "name": "Is Cash",
        "value": false,
        "time_stamp": "0001-01-01T00:00:00"
      }
    ]
  },
  {
    "name": "Keyboard",
    "properties": [
      {
        "id": 2,
        "asset": null,
        "name": "is fixed",
        "value": false,
        "time_stamp": "0001-01-01T00:00:00"
      }
    ]
  }

```       
If the property is provided then it will return the assets with the property in it.   
Other assets will also show but the porperties will not be present like the below json response  
```
[
  {
    "name": "Laptop",
    "properties": [
      {
        "id": 1,
        "asset": null,
        "name": "Is Cash",
        "value": false,
        "time_stamp": "0001-01-01T00:00:00"
      }
    ]
  },
  {
    "name": "Keyboard",
    "properties": []
  }
]
```  
3) **Sync http Post request** : This method get the CSV file either from the file upload or the appsetting.json file if the file upload is not selected.
CSV file is a strongly Type model, with the csv file consist of numbers of column as follows:
assetid, properties, value, time_stamp
the sync request will post the properties for the asset as per the assetid in the CSV with default value which is false and timestamp with Min value when that particular property is not present in the database.
and if the properties are already there for the particular assets then it will update the value and the time stamp , if the time stamp from the CSV file is greater then the stored timestamp for that asset and property.

**NOTE: The CSV Reader Method will group the asset properties if they are duplcate with higher timestamp and process to the DB.**

4) **Patch Method** : This method takes assetid and propertyid and a JsonPatch input as a parameter with Type  
[{"op":"replace", "path":"/name", "value":"is cash"},  
{"op":"replace", "path":"/time_stamp", "value":"2022-10-23T18:25:43.511Z"}]

          op 	 : Type of operation to be performed.  
          path  : column name to change the value.  
          value : value to be replace as per the operation.


This method will update the value based on the input provided. 
It will update only if the timestamp provided is greater then the DB timestamp for that assetid and propertyid.




