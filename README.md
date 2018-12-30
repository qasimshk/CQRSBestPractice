# Instructions
- Setup the database by changing the details in json file.
- Functionality can be tested simply by running the tests, without any db integration
- Setup folder path in log4net.config for logs. Use log4view to view log. ( http://www.log4view.com/log4view/ )

# API Requests

## GET
- https://localhost:44332/api/Student
- https://localhost:44332/api/Student/1
- https://localhost:44332/api/Student/1/courses

## POST
- https://localhost:44332/api/Student/Register

  {
        "FirstName":"Swat",
        "LastName":"Kats",
        "Email":"SK@abc.com",
        "DOB":"1984/11/30",
      }
      
## PUT
- https://localhost:44332/api/Student/Update/1

  {
        "FirstName":"turbo",
        "LastName":"Kat jet"
      }

## PATCH
- https://localhost:44332/api/Student/1

[
 {
    "op": "replace",
    "path": "/LastName",
    "value":"Kat"
 }
]

----------------------------------------------------

[
	{
		"op": "replace",
		"path": "/LastName",
		"value":"Kats"
	},
	{
		"op": "replace",
		"path": "/FirstName",
		"value":"Swat"
	}	
]

----------------------------------------------------

[
	{
		"op": "remove",
		"path": "/LastName"
	}
]

----------------------------------------------------

[
	{
		"op": "add",
		"path": "/LastName",
		"value":"Duckling"
	},
	{
		"op": "copy",
		"from": "/LastName",
		"path": "/FirstName"
	}	
]

----------------------------------------------------

[
	{
		"op": "remove",
		"path": "/FirstName"
	}
]


