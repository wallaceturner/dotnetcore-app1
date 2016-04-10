// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;

namespace SampleApp.Controllers
{
    public class OrdersControlller : Controller
    {
        [HttpGet("Orders/{id}")]
        public FileResult Index(int id)
        {            
            return new FileResult { Field1="Hello from " + id + " " + Request.Path};
        }
    }
	
	public class FileResult
	{
		public string Field1 { get; set; }
	}
}
