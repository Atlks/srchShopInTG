using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
   // [JSInvokableAttribute("GetGreetingAsync")]
    public class MyService
    {
    
        [JSInvokable]
        public static   Task<string> GetGreetingAsync(string name)
        {
            return   Task.FromResult($"Hello, {name}!");
        }
    }
}
