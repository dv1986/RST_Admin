using ModelDemo;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceDemo
{
    public interface IDemoService
    {
        List<DemoDTO> GetAll(string searchString);
    }
}
