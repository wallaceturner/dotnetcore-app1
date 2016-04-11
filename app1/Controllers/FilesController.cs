// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;

namespace SampleApp.Controllers
{
    [Route("[controller]")]
	public class FilesController
    {
        [HttpGet("{id}")]
        public FileResult Index(int id)
        {            
            return new FileResult { Field1=$"Hello from {id}" };
        }
    }
	
	public class FileResult
	{
		public string Field1 { get; set; }
	}
}
