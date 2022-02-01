# Funda.Client Code Challenge
This project includes the solution for Funda code challenge

# The challenge
## Useful translations:
- koop = purchase
- tuin = garden
- makelaar = real estate agent

## Assigment
The funda API returns information about the objects that are listed on funda.nl which are for sale. An example of one of the URLs in our REST API is: http://partnerapi.funda.nl/feeds/Aanbod.svc/[key]/?type=koop&zo=/amsterdam/tuin/&page=1&pagesize=25 A temporary key which can be used for this exercise is: ac1b0b1572524640a0ecc54de453ea9f You will need to replace [key] with the temporary key. Most of the parameters that you can pass are self explanatory. The parameter 'zo' (zoekopdracht or search query) is the same as the key used in funda.nl URLs. For example the URL shown above searches for houses in Amsterdam with a garden: http://www.funda.nl/koop/amsterdam/tuin/. The API returns data about the object in XML. If you add the key 'json' between 'Aanbod.svc' and [key], a JSON representation of the data will be returned instead of XML. The assignment Determine which makelaar's in Amsterdam have the most object listed for sale. Make a table of the top 10. Then do the same thing but only for objects with a tuin which are listed for sale. For the assignment you may write a program in any object oriented language of your choice and you may use any libraries that you find useful.
## Tips
- If you perform too many API requests in a short period of time (>100 per minute), the API will reject the requests. Your implementation should mitigate (avoid) errors and handle any errors that occur, so take both into account.
- Different people will look at your solution, so make sure it is easy to understand and go through.
- We don't expect solutions to have a comprehensive test suite, but we do expect solutions to be testable. • One of the criteria that our reviewers value is separation of concerns. We value creative problem-solving. If you are not sure about implementation details of the exercise, please remember: It is more important to show your competencies than to get "the right answer". Please explain your decisions and thought process. We look forward to receiving your assignment. Good luck!

# The Solution
The solution uses HttpClinet for consuming the restFul api. All the logic is handled on HttpClientService Class. Since the API rejects more than 100 requests per minutes so there is a time interval of 610 milliseconds between each request in order to make maximum of 98 api calls per minutes.
- GenerateRequestUri method:
  - Receives as input different parameters and generates the valid URI request
- CallApiEndPoint method:
  - Receives the request uri and makes the api call and returns the result after deserializing the object
- GetTopRealEstateAgentsFromPropertyList method:
  - Receives a list of properties for sale and numbers to take and returns the top items of the list
- GetTopRealEstateAgents method:
  - Receives as input three parameters and returns top real states with the most item for that type of sale. example of parameters are:
    - typeOfSale: This parameter is type of sale and can be koop or huur
    - propertyType: This parameter is type of property. for example tuin which stands for properties with garden
    - topNumber: this parameter defines how many top items we want to return. such as top 10 or top 20
