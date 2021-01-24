using BlazorExample.Components.DependencyInjection.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorExample.Components.DependencyInjection.Implementations
{
    public class ConstructorInjectionTwo : IConstructorInjectionTwo
    {
        public void MethodOfTwo()
        {
            Console.WriteLine("Two");
        }
    }
}
