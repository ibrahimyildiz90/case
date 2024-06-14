using Integration.Common;
using Integration.Backend;
using Integration.Locks;
using System.Text.Json;

namespace Integration.Service;

public sealed class ItemIntegrationService
{
    //This is a dependency that is normally fulfilled externally.
    private ItemOperationBackend ItemIntegrationBackend { get; set; } = new();
    RedisService _redisService = RedisService.CreateNewInstanseSingleton();

    public async Task ExcAsync(AsyncLock lockObject, string itemContent)
    {
        using (await lockObject.LockAsync())
        {
            await Task.Delay(3000); // The tasks become asynchronous
            SaveItem(itemContent); 
           // SaveItemWithRedis(itemContent); //The SaveItemWithRedis was created for the 'Distributed System Scenario'. Please remove the comment character('//') and the above SaveItem set as a comment to test it.
        }
    }

    // This is called externally and can be called multithreaded, in parallel.
    // More than one item with the same content should not be saved. However,
    // calling this with different contents at the same time is OK, and should
    // be allowed for performance reasons.
    public Result SaveItem(string itemContent)
    {

        // Check the backend to see if the content is already saved.
        if (ItemIntegrationBackend.FindItemsWithContent(itemContent).Count != 0)
        {
            return new Result(false, $"Duplicate item received with content {itemContent}.");
        }       

        var item = ItemIntegrationBackend.SaveItem(itemContent);         
       

        return new Result(true, $"Item with content {itemContent} saved with id {item.Id}");
    }

    public Result SaveItemWithRedis(string itemContent)
    {     
        // 'The implementation must in the service layer' was said on the case. 
        // So i implemented the redis code in here. But i think that the code should be in the ItemIntegrationBackend for redis implementition

        if (IsExistsItemsWithContentFromRedis(itemContent))
        {
            return new Result(false, $"Duplicate item received with content {itemContent}.");
        }

        var item = ItemIntegrationBackend.SaveItem(itemContent);

        //add the itemcontten to Redis.
        var statusForRedis = _redisService.GetDB().StringSetAsync(itemContent, JsonSerializer.Serialize(itemContent));

        return new Result(true, $"Item with content {itemContent} saved with id {item.Id}");
    }

    public List<Item> GetAllItems()
    {
        return ItemIntegrationBackend.GetAllItems();
    }

    public bool IsExistsItemsWithContentFromRedis(string itemContent)
    {
        return _redisService.GetDB().StringGetAsync(itemContent).Result.HasValue;
    }
}