using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using LargeDataRetrievalAPI.Data;
using EFCore.BulkExtensions;
using TaskEntity = LargeDataRetrievalAPI.Models.Task;
using UserEntity = LargeDataRetrievalAPI.Models.User;
using LargeDataRetrievalAPI.Enums;

namespace LargeDataRetrievalAPI.Extensions
{
    public static class LargeDataContextExtensions
    {
        public static async Task SeedDataAsync(this LargeDataContext context)
        {
            var options = new DbContextOptionsBuilder<LargeDataContext>()
                .UseSqlServer(context.Database.GetDbConnection().ConnectionString)
                .Options;

            using (var initialContext = new LargeDataContext(options))
            {
                var users = await initialContext.Users.ToListAsync();
                if (users.Count == 0)
                {
                    for (int i = 1; i <= 1000; i++)
                    {
                        users.Add(new UserEntity { Name = $"User{i}", Email = $"user{i}@example.com" });
                    }
                    await initialContext.BulkInsertAsync(users);
                }
            }

            long lastInsertedTaskId;
            using (var initialContext = new LargeDataContext(options))
            {
                lastInsertedTaskId = initialContext.Tasks.Max(t => (long?)t.Id) ?? 0;
            }

            var random = new Random();
            var totalRows = 200_000_000;
            var batchSize = 100_000; // Batch size for BulkInsert

            for (long startId = lastInsertedTaskId + 1; startId <= totalRows; startId += batchSize)
            {
                bool success = false;
                while (!success)
                {
                    try
                    {
                        var tasks = new List<TaskEntity>();
                        for (long i = startId; i < startId + batchSize && i <= totalRows; i++)
                        {
                            var userId = random.Next(1, 1001);
                            tasks.Add(new TaskEntity
                            {
                                Title = $"Title{i}",
                                Description = $"Description{i}",
                                Status = (Status)(i % 3)
                            });
                        }

                        using (var localContext = new LargeDataContext(options))
                        {
                            await localContext.BulkInsertAsync(tasks);
                        }
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        // Log the exception and retry
                        Console.WriteLine($"Error inserting batch starting with ID {startId}: {ex.Message}. Retrying...");
                        await Task.Delay(5000); // Wait for 5 seconds before retrying
                    }
                }
            }
        }
    }
}
