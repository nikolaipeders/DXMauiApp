using DXMauiApp.Models;

namespace DXMauiApp.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public MockDataStore()
        {
            DateTime baseDate = DateTime.Today;
            this.items = new List<Item>() {
            new Item { Id = Guid.NewGuid().ToString(), Text = "First lock", Description="This is a lock description.", StartTime = baseDate.AddHours(1), EndTime = baseDate.AddHours(2), Value=17.098 },
            new Item { Id = Guid.NewGuid().ToString(), Text = "Second lock", Description="This is a lock description.", StartTime = baseDate.AddHours(2), EndTime = baseDate.AddHours(4), Value=9.985 },
            new Item { Id = Guid.NewGuid().ToString(), Text = "Third lock", Description="This is a lock description.", StartTime = baseDate.AddHours(3), EndTime = baseDate.AddHours(5), Value=9.597},
            new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth lock", Description="This is a lock description.", StartTime = baseDate.AddHours(5), EndTime = baseDate.AddHours(6), Value=9.834 },
        };
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            this.items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = this.items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            this.items.Remove(oldItem);
            this.items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = this.items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            this.items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(this.items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(this.items);
        }
    }
}