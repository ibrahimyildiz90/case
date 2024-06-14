Hi,

- The mechanism of asynchronous task locking was used to ensure that tems with different content are processed concurrently without waiting for each other.
- The SemaphoreSlim class was used to the locking based the Task.
- The management of the setting lock's key is under the responsibility user. So if the lock key not sets same for the same item content, the item could be created one more time(please aware of because the user error).
- The lock's keys is keeped a dictinoary and it cleared on saturday every week.

- The SaveItemWithRedis method on the ItemIntegrationService was created to the Distributed System Scenario.
- Please note that enable the SaveItemWithRedis so you cant test it.
- The item data source was singularized when in case of multiple servers containing ItemIntegrationService wit usindg the Redis cache.
- The Redis was implemented using the Singleton pattern(thread safe).
- But If there is a multiple request that is contain the same item content for different server on time, the mechanizm could work one more time and the item could be add one more.
- The last check point could be added to ItemOperationBackend/SaveItem method but we did not because of the 'The implementation must in the service layer' was said on the case.


The Redis must be installed to test code.
I craeted the docker compose file.
You could install it with the 'docker-compose up' comand. 

Best Regards.
İbrahim.