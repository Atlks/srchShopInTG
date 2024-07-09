﻿using Microsoft.ClearScript.V8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class embedScrpt
    {

        public void demo()
        {
            // 使用 V8ScriptEngine 来嵌入和执行 JavaScript
            using (var engine = new V8ScriptEngine())
            {
                // 执行简单的 JavaScript 代码
                engine.Execute("console.log('Hello, JavaScript!');");

                // 定义一个 JavaScript 函数
                engine.Execute("function add(a, b) { return a + b; }");

                // 调用 JavaScript 函数并获取结果
                var result = engine.Script.add(5, 3);
               print($"Result of add(5, 3): {result}");

                // 定义一个 JavaScript 对象
                engine.Execute("var person = { name: 'John', age: 30 };");

                // 获取 JavaScript 对象的属性
                dynamic person = engine.Script.person;
               print($"Name: {person.name}, Age: {person.age}");


                // 给 JavaScript 变量赋值
                engine.Execute("var x = 10;");
                engine.Execute("var y = 20;");
                engine.Execute("var message = 'Hello, World!';");

                // 输出初始值
               print($"Initial values: x = {engine.Script.x}, y = {engine.Script.y}, message = '{engine.Script.message}'");

                // 修改 JavaScript 变量的值
                engine.Script.x = 100;
                engine.Script.y = 200;
                engine.Script.message = "Hello from C#!";


                // 定义并执行一个带有返回值的 JavaScript 表达式
                var expressionResult = engine.Evaluate("5 * 10");
            }
        }
    }
}
