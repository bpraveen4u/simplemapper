// <copyright file="MapperController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Microsoft.Integration.Mapper.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Integration.Mapper.Contracts.Models;
    using Microsoft.Integration.Mapper.Core;

    [Route("api/map")]
    [ApiController]
    public class MapperController : ControllerBase
    {
        private readonly IKeyValueMapper keyValueMapper;

        public MapperController(IKeyValueMapper keyValueMapper)
        {
            this.keyValueMapper = keyValueMapper;
        }

        // GET api/mapkey/partner/dell
        [HttpGet("key/{partition}/{key}")]
        public async Task<IActionResult> Get(string partition, string key)
        {
            var result = await this.keyValueMapper.GetByKey(partition, key).ConfigureAwait(true);
            if (result != null)
            {
                return this.Ok(new { key = result.RowKey, value = result.Value });
            }
            else
            {
                return this.NotFound();
            }
        }

        // GET api/mapkey/partner/dell
        [HttpGet("value/{partition}/{value}")]
        public async Task<IActionResult> GetByValue(string partition, string value)
        {
            var result = await this.keyValueMapper.GetByValue(partition, value).ConfigureAwait(true);
            if (result != null)
            {
                return this.Ok(new { value, keys = result.Select(r => r.RowKey) });
            }
            else
            {
                return this.NotFound();
            }
        }

        // GET api/mapkey/partner/dell
        [HttpPost("")]
        public async Task<IActionResult> Post(Mapping mapping)
        {
            if (this.ModelState.IsValid)
            {
                var result = await this.keyValueMapper.Save(mapping).ConfigureAwait(true);
                if (result != null)
                {
                    var url = new Uri($"{this.Request.Scheme}://{this.Request.Host.Value}/api/key/{result.PartitionKey}/{result.RowKey}");
                    return this.Created(url, new { name = result.RowKey, value = result.Value });
                }
                else
                {
                    this.ModelState.AddModelError("data", "Invalid partition or Key Already exists");
                    return this.BadRequest(this.ModelState);
                }
            }

            return null;
        }
    }
}