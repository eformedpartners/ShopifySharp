﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ShopifySharp
{
    public static partial class GraphExtensions
    {
        /// <summary>
        /// Get a single inventory quantity for a variant/location combination.
        /// </summary>
        /// <param name="graphService">The GraphService</param>
        /// <param name="variantId">The normal (long) variant ID.</param>
        /// <param name="locationId">The normal (long) location ID.</param>
        /// <returns>The Graph Inventory Level ID and the integer inventory value.</returns>
        public static async Task<GraphKeyValuePair> InventoryQuantityGetAsync(this GraphService graphService, long variantId, long locationId)
        {
            string body = @"
                query {
                  productVariant (id: ""gid://shopify/ProductVariant/" + variantId + @""" ) {
                            inventoryItem {
                                inventoryLevel(locationId: ""gid://shopify/Location/" + locationId + @""") {     
                        id,
                        available
                               }
                            }
                        }
                    }";

            JToken response = await graphService.PostAsync(body);

            GraphKeyValuePair inventory = new GraphKeyValuePair();
            
            inventory.Value = (int?)response.SelectToken("productVariant.inventoryItem.inventoryLevel.available");
            inventory.Id = (string)response.SelectToken("productVariant.inventoryItem.inventoryLevel.id");              

            return inventory;
        }
    }
}
