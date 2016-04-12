// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;

namespace SampleApp.Controllers
{
    [Route("[controller]")]
	public class OrdersController
    {
        [HttpGet("{id}")]
        public string Index(int id)
        {            
            throw new System.Exception("foo");
			return $"OrdersController {id}" ;
        }
    }
}
