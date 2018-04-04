Feature: Example API Testing
       In order to learning about API Testing
       As a testing analyst
       I want to see result of get


Scenario: Simple Get - Testing status code
       Given a collection of posts exposed as api
       When I request post with id 1
       Then response StatusCode is Ok  

Scenario: Simple Get - Testing value
       Given a collection of posts exposed as api
       When I request post with id 1
	   Then response StatusCode is Ok  
       Then response content is composed by title with value "sunt aut facere repellat provident occaecati excepturi optio reprehenderit"
       And response content is composed by id with value 1
       And response content is composed by userId with value "1"
       And response content is composed by body with value "quia et suscipit suscipit recusandae consequuntur expedita et cum reprehenderit molestiae ut ut quas totam nostrum rerum est autem sunt rem eveniet architecto"


Scenario: Simple Get - Testing FakeResponse
       Given a collection of posts exposed as api
       When I request post with id 1
       Then response StatusCode is Ok  
	   And response content is composed by all fields with value 2