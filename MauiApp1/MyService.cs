using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    internal class MyService
    {
        public async Task<string> GetGreetingAsync(string name)
        {
            return await Task.FromResult($"Hello, {name}!");
        }
    }
}
