using MailChimp.Net.Core;
using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MailChimp.Net.Logic;

internal class TagsLogic : BaseLogic, ITagsLogic
{
    private const string BaseUrl = "lists";
    public TagsLogic(MailChimpOptions mailChimpConfiguration)
        : base(mailChimpConfiguration)
    {
    }

    public async Task<IEnumerable<ListTag>> GetAllAsync(string listId, TagsRequest request = null) => (await GetResponseAsync(listId, request).ConfigureAwait(false))?.Tags;

    public async Task<ListTagsResponse> GetResponseAsync(string listId, TagsRequest request = null)
    {
        request ??= new TagsRequest
        {
            Limit = _limit
        };

        using var client = CreateMailClient($"{BaseUrl}/{listId}/tag-search");
        var response = await client.GetAsync(request.ToQueryString()).ConfigureAwait(false);
        await response.EnsureSuccessMailChimpAsync().ConfigureAwait(false);

        var listTagsResponse = await response.Content.ReadAsAsync<ListTagsResponse>().ConfigureAwait(false);
        return listTagsResponse;
    }
}
